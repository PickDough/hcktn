using System.Security.Claims;
using System.Text;
using hcktn.application.command.createEvent;
using hcktn.application.command.loginAdmin;
using hcktn.application.command.LoginOrganisation;
using hcktn.application.command.registerOrganisation;
using hcktn.application.command.validateOrganisation;
using hcktn.application.query.getPendingOrganisation;
using hcktn.application.query.listCity;
using hcktn.application.query.listEvent;
using hcktn.application.query.listTag;
using hcktn.infrastructure.auth;
using hcktn.infrastructure.db;
using hcktn.infrastructure.db.context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddDbContext<HcktnContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(Program).Assembly));

var jwtSecret = builder.Configuration["Jwt:Secret"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.MapInboundClaims = false;
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
        };
    });

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("AdminPolicy", p => p.RequireRole("Admin"));
    opt.AddPolicy("OrganisationPolicy", p => p.RequireRole("Organisation"));
});

builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IOrganisationRepository, OrganisationRepository>();
builder.Services.AddScoped<IEventRepository, EventRepositoryImpl>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<TokenService>();

builder.Services.AddScoped<ListCityHandler>();
builder.Services.AddScoped<ListTagHandler>();
builder.Services.AddScoped<ListEventHandler>();
builder.Services.AddScoped<GetPendingOrganisationHandler>();
builder.Services.AddScoped<CreateEventHandler>();
builder.Services.AddScoped<RegisterOrganisationHandler>();
builder.Services.AddScoped<ValidateOrganisationHandler>();
builder.Services.AddScoped<LoginAdminHandler>();
builder.Services.AddScoped<LoginOrganisationHandler>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<HcktnContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseExceptionHandler(errApp => errApp.Run(async ctx =>
{
    var feature = ctx.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
    var ex = feature?.Error;
    ctx.Response.StatusCode = ex switch
    {
        UnauthorizedAccessException => 401,
        KeyNotFoundException => 404,
        _ => 500
    };
    ctx.Response.ContentType = "application/json";
    await ctx.Response.WriteAsJsonAsync(new { error = ex?.Message });
}));

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// ─── Queries (open) ───────────────────────────────────────────────────────────

app.MapGet("/api/cities", (ListCityHandler handler) =>
    handler.Handle(new ListCityQuery()));

app.MapGet("/api/tags", (ListTagHandler handler) =>
    handler.Handle(new ListTagQuery()));

app.MapGet("/api/events", (ListEventHandler handler, uint? idCity, [FromQuery] uint[] idTags, uint idLast = 0, uint limit = 20) =>
    handler.Handle(new ListEventQuery(idCity, idTags.Length > 0 ? idTags.ToList() : null, idLast, limit)));

app.MapGet("/api/organisations/pending", (GetPendingOrganisationHandler handler) =>
    handler.Handle(new GetPendingOrganisation()));

// ─── Auth (open) ──────────────────────────────────────────────────────────────

app.MapPost("/api/auth/admin", (LoginAdminHandler handler, LoginAdminCommand command) =>
    handler.Handle(command));

app.MapPost("/api/auth/organisation", (LoginOrganisationHandler handler, LoginOrganisationCommand command) =>
    handler.Handle(command));

// ─── Commands ─────────────────────────────────────────────────────────────────

app.MapPost("/api/events", (
    CreateEventHandler handler,
    IOrganisationRepository orgRepo,
    ClaimsPrincipal user,
    [FromBody] CreateEventBody body) =>
{
    var orgId = uint.Parse(user.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)!);
    _ = orgRepo.FindById(orgId)
        ?? throw new UnauthorizedAccessException("Organisation not found");
    var command = new CreateEventCommand(body.Title, body.Description, body.Images, body.Tags, body.IdCity, body.Price, orgId, body.StartDate, body.EndDate);
    var result = handler.Handle(command);
    return Results.Created($"/api/events/{result.Id}", result);
}).RequireAuthorization("OrganisationPolicy");

app.MapPost("/api/organisations", (RegisterOrganisationHandler handler, RegisterOrganisationCommand command) =>
{
    var result = handler.Handle(command);
    return Results.Created($"/api/organisations/{result.Id}", result);
});

app.MapPost("/api/organisations/{id}/validate", (
    ValidateOrganisationHandler handler,
    IAdminRepository adminRepo,
    ClaimsPrincipal user,
    uint id,
    [FromBody] ValidateOrganisationBody body) =>
{
    var adminId = uint.Parse(user.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)!);
    _ = adminRepo.FindById(adminId)
        ?? throw new UnauthorizedAccessException("Admin not found");
    return handler.Handle(new ValidateOrganisationCommand(id, body.IsVerified));
}).RequireAuthorization("AdminPolicy");

app.Run();

record ValidateOrganisationBody(bool IsVerified);

record CreateEventBody(
    string Title,
    string Description,
    List<string> Images,
    List<uint> Tags,
    uint IdCity,
    hcktn.src.domain.Price Price,
    DateTime StartDate,
    DateTime EndDate
);

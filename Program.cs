using System.Security.Claims;
using System.Text;
using hcktn.application.command.aiQuery;
using hcktn.application.command.createEvent;
using hcktn.application.command.createSuggestion;
using hcktn.application.command.loginAdmin;
using hcktn.application.command.LoginOrganisation;
using hcktn.application.command.registerOrganisation;
using hcktn.application.command.validateOrganisation;
using hcktn.application.query.getEvent;
using hcktn.application.query.getOrganisation;
using hcktn.application.query.getPendingOrganisation;
using hcktn.application.query.listCity;
using hcktn.application.query.listEvent;
using hcktn.application.query.listTag;
using hcktn.application.query.searchEvent;
using hcktn.infrastructure.ai;
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

builder.Services.AddCors(opt => opt.AddDefaultPolicy(p =>
    p.WithOrigins("https://dist-sandy-eight-22.vercel.app")
     .AllowAnyHeader()
     .AllowAnyMethod()));

var connString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection is not configured.");
builder.Services.AddDbContext<HcktnContext>(opt => opt.UseNpgsql(connString));

builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(Program).Assembly));

var jwtSecret = builder.Configuration["Jwt:Secret"]
    ?? throw new InvalidOperationException("Jwt:Secret is not configured. Set the Jwt__Secret environment variable.");
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
builder.Services.AddScoped<ISuggestionRepository, SuggestionRepository>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddHttpClient<AiService>();
builder.Services.AddScoped<AiQueryHandler>();

builder.Services.AddScoped<ListCityHandler>();
builder.Services.AddScoped<ListTagHandler>();
builder.Services.AddScoped<ListEventHandler>();
builder.Services.AddScoped<GetEventHandler>();
builder.Services.AddScoped<GetOrganisationHandler>();
builder.Services.AddScoped<GetPendingOrganisationHandler>();
builder.Services.AddScoped<SearchEventHandler>();
builder.Services.AddScoped<CreateEventHandler>();
builder.Services.AddScoped<RegisterOrganisationHandler>();
builder.Services.AddScoped<ValidateOrganisationHandler>();
builder.Services.AddScoped<CreateSuggestionHandler>();
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
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

// ─── Queries (open) ───────────────────────────────────────────────────────────

app.MapGet("/api/cities", (ListCityHandler handler) =>
    handler.Handle(new ListCityQuery()));

app.MapGet("/api/tags", (ListTagHandler handler) =>
    handler.Handle(new ListTagQuery()));

app.MapGet("/api/events", (ListEventHandler handler, uint? idCity, [FromQuery] uint[] idTags, uint idLast = 0, uint limit = 20) =>
    handler.Handle(new ListEventQuery(idCity, idTags.Length > 0 ? idTags.ToList() : null, idLast, limit)));

app.MapGet("/api/events/search", (SearchEventHandler handler, string q) =>
    handler.Handle(new SearchEventQuery(q)));

app.MapGet("/api/events/{id}", (GetEventHandler handler, uint id) =>
{
    var result = handler.Handle(new GetEventQuery(id));
    return result is null ? Results.NotFound() : Results.Ok(result);
});

app.MapGet("/api/organisations/pending", (GetPendingOrganisationHandler handler) =>
    handler.Handle(new GetPendingOrganisation()));

app.MapGet("/api/organisations/{id}", (GetOrganisationHandler handler, uint id) =>
{
    var result = handler.Handle(new GetOrganisationQuery(id));
    return result is null ? Results.NotFound() : Results.Ok(result);
});

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
    var command = new CreateEventCommand(
        body.Title, body.Description, body.Images, body.Tags, body.IdCity,
        body.PriceType, body.PriceValue, body.PriceNotes,
        orgId, body.StartDate, body.EndDate,
        body.MeetingType, body.GoogleMeetUrl, body.Recurrence, body.Capacity,
        body.TransferAvailable, body.TransferDetails, body.InclusivityIds, body.BarrierFreeUrl,
        body.Latitude, body.Longitude, body.Address);
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

app.MapPost("/api/suggestions", (CreateSuggestionHandler handler, [FromBody] CreateSuggestionCommand command) =>
{
    handler.Handle(command);
    return Results.Ok();
});

app.MapPost("/api/ai/query", async (AiQueryHandler handler, [FromBody] AiQueryCommand command) =>
    await handler.Handle(command));

app.Run();

record ValidateOrganisationBody(bool IsVerified);

record CreateEventBody(
    string Title,
    string Description,
    List<string> Images,
    List<uint> Tags,
    uint IdCity,
    string PriceType,
    uint? PriceValue,
    string? PriceNotes,
    DateTime StartDate,
    DateTime EndDate,
    string MeetingType = "offline",
    string? GoogleMeetUrl = null,
    string Recurrence = "none",
    uint? Capacity = null,
    bool TransferAvailable = false,
    string? TransferDetails = null,
    string[]? InclusivityIds = null,
    string? BarrierFreeUrl = null,
    double? Latitude = null,
    double? Longitude = null,
    string? Address = null
)
{
    public string[] InclusivityIds { get; init; } = InclusivityIds ?? [];
}

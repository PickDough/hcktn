using hcktn.application.command.createEvent;
using hcktn.application.command.registerOrganisation;
using hcktn.application.command.validateOrganisation;
using hcktn.application.query.getPendingOrganisation;
using hcktn.application.query.listCity;
using hcktn.application.query.listEvent;
using hcktn.application.query.listTag;
using hcktn.infrastructure.db;
using hcktn.infrastructure.db.context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddDbContext<HcktnContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(Program).Assembly));

builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IOrganisationRepository, OrganisationRepository>();
builder.Services.AddScoped<IEventRepository, EventRepositoryImpl>();

builder.Services.AddScoped<ListCityHandler>();
builder.Services.AddScoped<ListTagHandler>();
builder.Services.AddScoped<ListEventHandler>();
builder.Services.AddScoped<GetPendingOrganisationHandler>();
builder.Services.AddScoped<CreateEventHandler>();
builder.Services.AddScoped<RegisterOrganisationHandler>();
builder.Services.AddScoped<ValidateOrganisationHandler>();

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

app.UseHttpsRedirection();

// ─── Queries ──────────────────────────────────────────────────────────────────

app.MapGet("/api/cities", (ListCityHandler handler) =>
    handler.Handle(new ListCityQuery()));

app.MapGet("/api/tags", (ListTagHandler handler) =>
    handler.Handle(new ListTagQuery()));

app.MapGet("/api/events", (ListEventHandler handler, uint? idCity, [FromQuery] uint[] idTags, uint idLast = 0, uint limit = 20) =>
    handler.Handle(new ListEventQuery(idCity, idTags.ToList(), idLast, limit)));

app.MapGet("/api/organisations/pending", (GetPendingOrganisationHandler handler) =>
    handler.Handle(new GetPendingOrganisation()));

// ─── Commands ─────────────────────────────────────────────────────────────────

app.MapPost("/api/events", (CreateEventHandler handler, CreateEventCommand command) =>
{
    var result = handler.Handle(command);
    return Results.Created($"/api/events/{result.Id}", result);
});

app.MapPost("/api/organisations", (RegisterOrganisationHandler handler, RegisterOrganisationCommand command) =>
{
    var result = handler.Handle(command);
    return Results.Created($"/api/organisations/{result.Id}", result);
});

app.MapPost("/api/organisations/{id}/validate", (ValidateOrganisationHandler handler, uint id, [FromBody] ValidateOrganisationBody body) =>
    handler.Handle(new ValidateOrganisationCommand(id, body.IsVerified)));

app.Run();

record ValidateOrganisationBody(bool IsVerified);

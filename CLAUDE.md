# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
dotnet build              # build
dotnet run                # run locally (HTTP on 5000, HTTPS on 5001 by default)
dotnet watch              # run with hot reload
docker compose up --build # build image and run in container
```

## Architecture

ASP.NET Core 10 Web API using a CQRS pattern with clean-architecture layering.

**Layers:**
- `domain/` — immutable `record` types (entities and value objects). No business logic yet.
- `application/command/` and `application/query/` — CQRS handlers. Each use-case lives in its own subfolder containing a `*Command`/`*Query` record (the input) and a `*Handler` class that extends the base.
- `inftastructure/db/` — repository interfaces only (no implementations yet). Note the folder is intentionally spelled `inftastructure`.

**CQRS base classes** (`application/command/Command.cs`, `application/query/Query.cs`):
```csharp
public abstract class Command<TC, TR> { public abstract TR Handle(TC command); }
public abstract class Query<TQ, TR>   { public abstract TR Handle(TQ query); }
```
Every handler extends one of these and overrides `Handle`.

**Domain model summary:**
- `Organisation` — name, phone, contactInfo; goes through `ValidationStatus` (`Pending → Approved | Rejected`)
- `Event` — title, description, images, tags, optional location, price, owning organisation, date range
- `City`, `Tag` — simple reference data

**Current state:** `Program.cs` still contains the default ASP.NET template (WeatherForecast). The endpoints, DI wiring, and repository implementations have not been added yet.

**Naming inconsistency to be aware of:** `domain/event/` types use namespace `hcktn.src.domain`; organisation/tag types use `hcktn.domain.*`. The `EventRepository` interface is missing the `I` prefix (unlike `IOrganisationRepository`, `ICityRepository`, `ITagRepository`).

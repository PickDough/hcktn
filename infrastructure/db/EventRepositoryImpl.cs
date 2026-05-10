using AutoMapper;
using hcktn.application.command.createEvent;
using hcktn.application.query.listEvent;
using hcktn.infrastructure.db.context;
using hcktn.src.domain;
using Microsoft.EntityFrameworkCore;

namespace hcktn.infrastructure.db;

public class EventRepositoryImpl(HcktnContext context, IMapper mapper) : IEventRepository
{
    private IQueryable<EventDb> WithIncludes() =>
        context.Events
            .Include(e => e.Tags).ThenInclude(et => et.Tag)
            .Include(e => e.Images)
            .Include(e => e.City)
            .Include(e => e.Price)
            .Include(e => e.Registrations);

    public List<Event> List(ListEventQuery query)
    {
        var q = WithIncludes().Where(e => e.Id > query.IdLast);

        if (query.IdCity.HasValue)
            q = q.Where(e => e.CityId == query.IdCity.Value);

        if (query.IdTags is { Count: > 0 })
            q = q.Where(e => e.Tags.Any(et => query.IdTags.Contains(et.TagId)));

        return q.Take((int)query.Limit)
            .AsEnumerable()
            .Select(e => mapper.Map<Event>(e))
            .ToList();
    }

    public Event? GetById(uint id) =>
        WithIncludes()
            .Where(e => e.Id == id)
            .AsEnumerable()
            .Select(e => mapper.Map<Event>(e))
            .FirstOrDefault();

    public List<Event> Search(string query)
    {
        var lower = query.ToLower();
        return WithIncludes()
            .Where(e => EF.Functions.ILike(e.Title, $"%{lower}%")
                     || EF.Functions.ILike(e.Description, $"%{lower}%"))
            .Take(20)
            .AsEnumerable()
            .Select(e => mapper.Map<Event>(e))
            .ToList();
    }

    public Event Create(CreateEventCommand command)
    {
        var price = new PriceDb
        {
            PriceType = command.PriceType,
            PriceValue = command.PriceValue,
            PriceNotes = command.PriceNotes
        };
        context.Prices.Add(price);
        context.SaveChanges();

        var eventDb = new EventDb
        {
            Title = command.Title,
            Description = command.Description,
            PriceId = price.Id,
            IdOrganisation = command.IdOrganisation,
            StartDate = command.StartDate.ToUniversalTime(),
            EndDate = command.EndDate.ToUniversalTime(),
            CreatedAt = DateTime.UtcNow,
            CityId = command.IdCity,
            Latitude = command.Latitude,
            Longitude = command.Longitude,
            Address = command.Address,
            MeetingType = command.MeetingType,
            GoogleMeetUrl = command.GoogleMeetUrl,
            Recurrence = command.Recurrence,
            Capacity = command.Capacity,
            TransferAvailable = command.TransferAvailable,
            TransferDetails = command.TransferDetails,
            InclusivityIds = command.InclusivityIds,
            BarrierFreeUrl = command.BarrierFreeUrl,
            Tags = command.Tags.Select(tagId => new EventTagDb { TagId = tagId }).ToList(),
            Images = command.Images.Select(url => new EventImageDb { Url = url }).ToList()
        };

        context.Events.Add(eventDb);
        context.SaveChanges();

        return WithIncludes()
            .Where(e => e.Id == eventDb.Id)
            .AsEnumerable()
            .Select(e => mapper.Map<Event>(e))
            .First();
    }
}

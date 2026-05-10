using AutoMapper;
using hcktn.application.command.createEvent;
using hcktn.application.query.listEvent;
using hcktn.infrastructure.db.context;
using hcktn.src.domain;
using Microsoft.EntityFrameworkCore;

namespace hcktn.infrastructure.db;

public class EventRepositoryImpl(HcktnContext context, IMapper mapper) : IEventRepository
{
    public List<Event> List(ListEventQuery query)
    {
        var q = context.Events
            .Include(e => e.Tags).ThenInclude(et => et.Tag)
            .Include(e => e.Images)
            .Include(e => e.City)
            .Where(e => e.Id > query.IdLast);

        if (query.IdCity.HasValue)
            q = q.Where(e => e.CityId == query.IdCity.Value);

        if (query.IdTags.Count > 0)
            q = q.Where(e => e.Tags.Any(et => query.IdTags.Contains(et.TagId)));

        return q.Take((int)query.Limit)
            .AsEnumerable()
            .Select(e => mapper.Map<Event>(e))
            .ToList();
    }

    public Event Create(CreateEventCommand command)
    {
        var eventDb = new EventDb
        {
            Title = command.Title,
            Description = command.Description,
            Price = command.Price,
            IdOrganisation = command.IdOrganisation,
            StartDate = command.StartDate.ToUniversalTime(),
            EndDate = command.EndDate.ToUniversalTime(),
            CityId = command.IdCity,
            Tags = command.Tags.Select(tagId => new EventTagDb { TagId = tagId }).ToList(),
            Images = command.Images.Select(url => new EventImageDb { Url = url }).ToList()
        };

        context.Events.Add(eventDb);
        context.SaveChanges();

        return context.Events
            .Include(e => e.Tags).ThenInclude(et => et.Tag)
            .Include(e => e.Images)
            .Include(e => e.City)
            .Where(e => e.Id == eventDb.Id)
            .AsEnumerable()
            .Select(e => mapper.Map<Event>(e))
            .First();
    }
}

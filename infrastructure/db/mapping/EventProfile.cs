using AutoMapper;
using hcktn.domain.tag;
using hcktn.infrastructure.db.context;
using hcktn.src.domain;

namespace hcktn.infrastructure.db.mapping;

public class EventProfile : Profile
{
    public EventProfile()
    {
        CreateMap<EventDb, Event>()
            .ConstructUsing((src, ctx) => new Event(
                src.Id,
                src.Title,
                src.Description,
                src.Images.Select(i => i.Url).ToList(),
                src.Tags.Select(t => ctx.Mapper.Map<Tag>(t.Tag)).ToList(),
                src.City != null ? new Location(
                    new City(src.City.Id, src.City.Name),
                    src.Latitude ?? 0,
                    src.Longitude ?? 0,
                    src.Address ?? string.Empty,
                    src.LocationLink
                ) : null,
                new Price(src.Price.PriceType, src.Price.PriceValue, src.Price.PriceNotes),
                src.IdOrganisation,
                src.StartDate,
                src.EndDate,
                src.MeetingType,
                src.GoogleMeetUrl,
                src.Recurrence,
                src.Capacity,
                (uint)src.Registrations.Count,
                src.TransferAvailable,
                src.TransferDetails,
                src.InclusivityIds,
                src.BarrierFreeUrl,
                src.CreatedAt
            ))
            .ForAllMembers(opt => opt.Ignore());
    }
}

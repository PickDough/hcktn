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
                (uint)src.Id,
                src.Title,
                src.Description,
                src.Images.Select(i => i.Url).ToList(),
                src.Tags.Select(t => ctx.Mapper.Map<Tag>(t.Tag)).ToList(),
                src.City != null ? new Location(new City((uint)src.City.Id, src.City.Name)) : null,
                src.Price,
                (uint)src.IdOrganisation,
                src.StartDate,
                src.EndDate
            ))
            .ForAllMembers(opt => opt.Ignore());
    }
}

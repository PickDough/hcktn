using AutoMapper;
using hcktn.domain.tag;
using hcktn.infrastructure.db.context;

namespace hcktn.infrastructure.db.mapping;

public class TagProfile : Profile
{
    public TagProfile()
    {
        CreateMap<TagDb, Tag>();
    }
}

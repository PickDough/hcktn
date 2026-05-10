using AutoMapper;
using hcktn.domain.tag;
using hcktn.infrastructure.db.context;

namespace hcktn.infrastructure.db;

public class TagRepository(HcktnContext context, IMapper mapper) : ITagRepository
{
    public List<Tag> List()
    {
        return context.Tags.Select(t => mapper.Map<Tag>(t)).ToList();
    }
}

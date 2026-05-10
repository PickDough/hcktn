using hcktn.domain.tag;

namespace hcktn.infrastructure.db;

public interface ITagRepository
{
    List<Tag> List();
}

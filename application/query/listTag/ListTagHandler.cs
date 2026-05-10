using hcktn.domain.tag;
using hcktn.infrastructure.db;

namespace hcktn.application.query.listTag;

public class ListTagHandler(ITagRepository repo) : Query<ListTagQuery, List<Tag>>
{
    public override List<Tag> Handle(ListTagQuery query) => repo.List();
}

using hcktn.infrastructure.db;
using hcktn.src.domain;

namespace hcktn.application.query.searchEvent;

public class SearchEventHandler(IEventRepository repo) : Query<SearchEventQuery, List<Event>>
{
    public override List<Event> Handle(SearchEventQuery query) => repo.Search(query.Q);
}

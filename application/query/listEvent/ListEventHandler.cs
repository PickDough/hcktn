using hcktn.infrastructure.db;
using hcktn.src.domain;

namespace hcktn.application.query.listEvent;

public class ListEventHandler(IEventRepository repo) : Query<ListEventQuery, List<Event>>
{
    public override List<Event> Handle(ListEventQuery query) => repo.List(query);
}

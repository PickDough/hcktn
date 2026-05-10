using hcktn.infrastructure.db;
using hcktn.src.domain;

namespace hcktn.application.query.getEvent;

public class GetEventHandler(IEventRepository repo) : Query<GetEventQuery, Event?>
{
    public override Event? Handle(GetEventQuery query) => repo.GetById(query.Id);
}

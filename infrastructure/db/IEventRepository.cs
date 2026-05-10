using hcktn.application.command.createEvent;
using hcktn.application.query.listEvent;
using hcktn.src.domain;

namespace hcktn.infrastructure.db;

public interface IEventRepository
{
    Event Create(CreateEventCommand createEvent);
    List<Event> List(ListEventQuery query);
}

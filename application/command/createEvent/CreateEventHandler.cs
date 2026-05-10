using hcktn.infrastructure.db;
using hcktn.src.domain;

namespace hcktn.application.command.createEvent;

public class CreateEventHandler(IEventRepository repo) : Command<CreateEventCommand, Event>
{
    public override Event Handle(CreateEventCommand command) => repo.Create(command);
}

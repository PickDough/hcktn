using hcktn.domain.tag;
using hcktn.src.domain;

namespace hcktn.application.command.createEvent;

public record CreateEventCommand(
    string Title,
    string Description,
    List<string> Images,
    List<uint> Tags,
    uint IdCity,
    Price Price,
    uint IdOrganisation,
    DateTime StartDate,
    DateTime EndDate
);

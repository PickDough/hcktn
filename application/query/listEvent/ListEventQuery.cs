namespace hcktn.application.query.listEvent;

public record ListEventQuery(
    uint? IdCity,
    List<uint>? IdTags = null,
    uint IdLast = 0,
    uint Limit = 20
);

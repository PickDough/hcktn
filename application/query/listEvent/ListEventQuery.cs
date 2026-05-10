namespace hcktn.application.query.listEvent;

public record ListEventQuery(
    uint? IdCity,
    List<uint> IdTags,
    uint IdLast,
    uint Limit = 20
);

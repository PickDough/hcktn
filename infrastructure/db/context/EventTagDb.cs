namespace hcktn.infrastructure.db.context;

public class EventTagDb
{
    public uint EventId { get; set; }
    public EventDb Event { get; set; } = null!;
    public uint TagId { get; set; }
    public TagDb Tag { get; set; } = null!;
}

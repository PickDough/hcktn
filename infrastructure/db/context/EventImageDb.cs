namespace hcktn.infrastructure.db.context;

public class EventImageDb
{
    public uint Id { get; set; }
    public uint EventId { get; set; }
    public EventDb Event { get; set; } = null!;
    public string Url { get; set; } = string.Empty;
}

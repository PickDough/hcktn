namespace hcktn.infrastructure.db.context;

public class RegistrationDb
{
    public uint Id { get; set; }
    public uint EventId { get; set; }
    public EventDb Event { get; set; } = null!;
}

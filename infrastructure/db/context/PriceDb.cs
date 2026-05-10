namespace hcktn.infrastructure.db.context;

public class PriceDb
{
    public uint Id { get; set; }
    public string PriceType { get; set; } = "free";
    public uint? PriceValue { get; set; }
    public string? PriceNotes { get; set; }
}

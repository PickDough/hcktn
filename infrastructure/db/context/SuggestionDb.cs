namespace hcktn.infrastructure.db.context;

public class SuggestionDb
{
    public uint Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

namespace hcktn.infrastructure.db.context;

public class AdminDb
{
    public uint Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
}

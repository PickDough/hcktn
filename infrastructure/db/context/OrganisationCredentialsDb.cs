namespace hcktn.infrastructure.db.context;

public class OrganisationCredentialsDb
{
    public uint Id { get; set; }
    public uint OrganisationId { get; set; }
    public OrganisationDb Organisation { get; set; } = null!;
    public string Login { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
}

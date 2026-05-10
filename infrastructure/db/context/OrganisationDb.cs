using hcktn.domain.organisation;

namespace hcktn.infrastructure.db.context;

public class OrganisationDb
{
    public uint Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string ContactInfo { get; set; } = string.Empty;
    public ValidationStatus Status { get; set; }
}

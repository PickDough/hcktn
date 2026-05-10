namespace hcktn.domain.organisation;

public record Organisation(
    uint Id,
    string Name,
    string Phone,
    string ContactInfo,
    ValidationStatus Status
);

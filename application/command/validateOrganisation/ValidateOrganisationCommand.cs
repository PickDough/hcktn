namespace hcktn.application.command.validateOrganisation;

public record ValidateOrganisationCommand(
    uint IdOrganisation,
    bool IsVerified
);

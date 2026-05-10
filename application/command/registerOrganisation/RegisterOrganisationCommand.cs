namespace hcktn.application.command.registerOrganisation;

public record RegisterOrganisationCommand(
    string Name,
    string Phone,
    string ContactInfo
);

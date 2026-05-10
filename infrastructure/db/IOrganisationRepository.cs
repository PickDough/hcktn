using hcktn.application.command.registerOrganisation;
using hcktn.application.command.validateOrganisation;
using hcktn.domain.organisation;

namespace hcktn.infrastructure.db;

public interface IOrganisationRepository
{
    Organisation? GetPendingOrganisation();
    Organisation Create(RegisterOrganisationCommand command);
    Organisation Validate(ValidateOrganisationCommand command);
}

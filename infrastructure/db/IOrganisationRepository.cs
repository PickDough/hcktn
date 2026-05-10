using hcktn.application.command.registerOrganisation;
using hcktn.application.command.validateOrganisation;
using hcktn.domain.organisation;
using hcktn.infrastructure.db.context;

namespace hcktn.infrastructure.db;

public interface IOrganisationRepository
{
    Organisation? GetPendingOrganisation();
    Organisation Create(RegisterOrganisationCommand command);
    Organisation Validate(ValidateOrganisationCommand command);
    (OrganisationCredentialsDb Creds, OrganisationDb Org)? FindCredentialsByLogin(string login);
    OrganisationDb? FindById(uint id);
    Organisation? GetById(uint id);
}

using hcktn.domain.organisation;
using hcktn.infrastructure.db;

namespace hcktn.application.command.validateOrganisation;

public class ValidateOrganisationHandler(IOrganisationRepository repo) : Command<ValidateOrganisationCommand, Organisation>
{
    public override Organisation Handle(ValidateOrganisationCommand command) => repo.Validate(command);
}

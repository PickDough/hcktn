using hcktn.domain.organisation;
using hcktn.infrastructure.db;

namespace hcktn.application.command.registerOrganisation;

public class RegisterOrganisationHandler(IOrganisationRepository repo) : Command<RegisterOrganisationCommand, Organisation>
{
    public override Organisation Handle(RegisterOrganisationCommand command) => repo.Create(command);
}

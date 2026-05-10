using hcktn.domain.organisation;
using hcktn.infrastructure.db;

namespace hcktn.application.query.getPendingOrganisation;

public class GetPendingOrganisationHandler(IOrganisationRepository repo)
    : Query<GetPendingOrganisation, Organisation?>
{
    public override Organisation? Handle(GetPendingOrganisation query) =>
        repo.GetPendingOrganisation();
}

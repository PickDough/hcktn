using hcktn.domain.organisation;
using hcktn.infrastructure.db;

namespace hcktn.application.query.getOrganisation;

public class GetOrganisationHandler(IOrganisationRepository repo) : Query<GetOrganisationQuery, Organisation?>
{
    public override Organisation? Handle(GetOrganisationQuery query) => repo.GetById(query.Id);
}

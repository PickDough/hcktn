using AutoMapper;
using hcktn.domain.organisation;
using hcktn.infrastructure.db.context;

namespace hcktn.infrastructure.db.mapping;

public class OrganisationProfile : Profile
{
    public OrganisationProfile()
    {
        CreateMap<OrganisationDb, Organisation>();
    }
}

using AutoMapper;
using hcktn.domain.organisation;
using hcktn.infrastructure.db.context;

namespace hcktn.infrastructure.db.mapping;

public class OrganisationProfile : Profile
{
    public OrganisationProfile()
    {
        CreateMap<OrganisationDb, Organisation>()
            .ConstructUsing(src => new Organisation(
                src.Id, src.Name, src.Phone, src.ContactInfo, src.Status,
                src.ContactAddress, src.IsVeteran, src.ProfilePhoto
            ))
            .ForAllMembers(opt => opt.Ignore());
    }
}

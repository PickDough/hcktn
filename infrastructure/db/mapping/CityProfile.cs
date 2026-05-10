using AutoMapper;
using hcktn.infrastructure.db.context;
using hcktn.src.domain;

namespace hcktn.infrastructure.db.mapping;

public class CityProfile : Profile
{
    public CityProfile()
    {
        CreateMap<CityDb, City>();
    }
}

using AutoMapper;
using hcktn.infrastructure.db.context;
using hcktn.src.domain;

namespace hcktn.infrastructure.db;

public class CityRepository(HcktnContext context, IMapper mapper) : ICityRepository
{
    public List<City> List()
    {
        return context.Cities.Select(c => mapper.Map<City>(c)).ToList();
    }
}

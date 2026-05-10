using hcktn.infrastructure.db;
using hcktn.src.domain;

namespace hcktn.application.query.listCity;

public class ListCityHandler(ICityRepository repo) : Query<ListCityQuery, List<City>>
{
    public override List<City> Handle(ListCityQuery query) => repo.List();
}

using hcktn.infrastructure.db.context;

namespace hcktn.infrastructure.db;

public interface IAdminRepository
{
    AdminDb? FindByLogin(string login);
    AdminDb? FindById(uint id);
}

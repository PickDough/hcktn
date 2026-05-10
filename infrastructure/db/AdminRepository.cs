using hcktn.infrastructure.db.context;

namespace hcktn.infrastructure.db;

public class AdminRepository(HcktnContext context) : IAdminRepository
{
    public AdminDb? FindByLogin(string login) =>
        context.Admins.FirstOrDefault(a => a.Login == login);

    public AdminDb? FindById(uint id) =>
        context.Admins.Find(id);
}

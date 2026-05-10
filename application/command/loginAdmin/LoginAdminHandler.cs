using hcktn.application.command.loginAdmin.Result;
using hcktn.infrastructure.auth;
using hcktn.infrastructure.db;

namespace hcktn.application.command.loginAdmin;

public class LoginAdminHandler(IAdminRepository repo, TokenService tokenService)
    : Command<LoginAdminCommand, LoggedInAdmin>
{
    public override LoggedInAdmin Handle(LoginAdminCommand command)
    {
        var admin = repo.FindByLogin(command.Login)
            ?? throw new UnauthorizedAccessException("Invalid credentials");

        if (!BCrypt.Net.BCrypt.Verify(command.Password, admin.PasswordHash))
            throw new UnauthorizedAccessException("Invalid credentials");

        var token = tokenService.GenerateToken(admin.Id, "Admin");
        return new LoggedInAdmin(token);
    }
}

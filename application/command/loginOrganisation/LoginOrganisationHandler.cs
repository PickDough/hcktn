using hcktn.application.command.LoginOrganisation.Result;
using hcktn.domain.organisation;
using hcktn.infrastructure.auth;
using hcktn.infrastructure.db;

namespace hcktn.application.command.LoginOrganisation;

public class LoginOrganisationHandler(IOrganisationRepository repo, TokenService tokenService)
    : Command<LoginOrganisationCommand, AuthenticatedOrganisation>
{
    public override AuthenticatedOrganisation Handle(LoginOrganisationCommand command)
    {
        var found = repo.FindCredentialsByLogin(command.Login)
            ?? throw new UnauthorizedAccessException("Invalid credentials");

        if (found.Org.Status != ValidationStatus.Approved)
            throw new UnauthorizedAccessException("Organisation is not approved");

        if (!BCrypt.Net.BCrypt.Verify(command.Password, found.Creds.PasswordHash))
            throw new UnauthorizedAccessException("Invalid credentials");

        var token = tokenService.GenerateToken(found.Org.Id, "Organisation");
        return new AuthenticatedOrganisation(token);
    }
}

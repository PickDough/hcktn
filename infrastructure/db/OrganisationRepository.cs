using AutoMapper;
using hcktn.application.command.registerOrganisation;
using hcktn.application.command.validateOrganisation;
using hcktn.domain.organisation;
using hcktn.infrastructure.db.context;
using Microsoft.EntityFrameworkCore;

namespace hcktn.infrastructure.db;

public class OrganisationRepository(HcktnContext context, IMapper mapper) : IOrganisationRepository
{
    public Organisation? GetPendingOrganisation()
    {
        var org = context.Organisations.FirstOrDefault(o => o.Status == ValidationStatus.Pending);
        return org is null ? null : mapper.Map<Organisation>(org);
    }

    public Organisation Create(RegisterOrganisationCommand command)
    {
        var org = new OrganisationDb
        {
            Name = command.Name,
            Phone = command.Phone,
            ContactInfo = command.ContactInfo,
            Status = ValidationStatus.Pending
        };
        context.Organisations.Add(org);
        context.SaveChanges();
        return mapper.Map<Organisation>(org);
    }

    public Organisation Validate(ValidateOrganisationCommand command)
    {
        var org = context.Organisations.Find(command.IdOrganisation)
            ?? throw new KeyNotFoundException($"Organisation {command.IdOrganisation} not found");
        org.Status = command.IsVerified ? ValidationStatus.Approved : ValidationStatus.Rejected;
        context.SaveChanges();
        return mapper.Map<Organisation>(org);
    }

    public (OrganisationCredentialsDb Creds, OrganisationDb Org)? FindCredentialsByLogin(string login)
    {
        var creds = context.OrganisationCredentials
            .Include(c => c.Organisation)
            .FirstOrDefault(c => c.Login == login);
        return creds is null ? null : (creds, creds.Organisation);
    }

    public OrganisationDb? FindById(uint id) =>
        context.Organisations.Find(id);
}

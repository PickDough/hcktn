using System.ComponentModel.DataAnnotations.Schema;
using hcktn.src.domain;

namespace hcktn.infrastructure.db.context;

public class EventDb
{
    public uint Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Price Price { get; set; }
    public uint IdOrganisation { get; set; }
    [ForeignKey(nameof(IdOrganisation))]
    public OrganisationDb Organisation { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public uint? CityId { get; set; }
    public CityDb? City { get; set; }
    public ICollection<EventTagDb> Tags { get; set; } = new List<EventTagDb>();
    public ICollection<EventImageDb> Images { get; set; } = new List<EventImageDb>();
}

using System.ComponentModel.DataAnnotations.Schema;

namespace hcktn.infrastructure.db.context;

public class EventDb
{
    public uint Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public uint PriceId { get; set; }
    [ForeignKey(nameof(PriceId))]
    public PriceDb Price { get; set; } = null!;

    public uint IdOrganisation { get; set; }
    [ForeignKey(nameof(IdOrganisation))]
    public OrganisationDb Organisation { get; set; } = null!;

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime CreatedAt { get; set; }

    public uint? CityId { get; set; }
    public CityDb? City { get; set; }

    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? Address { get; set; }
    public string? LocationLink { get; set; }

    public string MeetingType { get; set; } = "offline";
    public string? GoogleMeetUrl { get; set; }
    public string Recurrence { get; set; } = "none";
    public uint? Capacity { get; set; }
    public bool TransferAvailable { get; set; }
    public string? TransferDetails { get; set; }
    public string[] InclusivityIds { get; set; } = [];
    public string? BarrierFreeUrl { get; set; }

    public ICollection<EventTagDb> Tags { get; set; } = new List<EventTagDb>();
    public ICollection<EventImageDb> Images { get; set; } = new List<EventImageDb>();
    public ICollection<RegistrationDb> Registrations { get; set; } = new List<RegistrationDb>();
}

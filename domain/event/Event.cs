using hcktn.domain.tag;

namespace hcktn.src.domain;

public record Event(
    uint Id,
    string Title,
    string Description,
    List<string> Images,
    List<Tag> Tags,
    Location? Location,
    Price Price,
    uint IdOrganisation,
    DateTime StartDate,
    DateTime EndDate,
    string MeetingType,
    string? GoogleMeetUrl,
    string Recurrence,
    uint? Capacity,
    uint RegisteredCount,
    bool TransferAvailable,
    string? TransferDetails,
    string[] InclusivityIds,
    string? BarrierFreeUrl,
    DateTime CreatedAt
);

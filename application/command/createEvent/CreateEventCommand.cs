namespace hcktn.application.command.createEvent;

public record CreateEventCommand(
    string Title,
    string Description,
    List<string> Images,
    List<uint> Tags,
    uint IdCity,
    string PriceType,
    uint? PriceValue,
    string? PriceNotes,
    uint IdOrganisation,
    DateTime StartDate,
    DateTime EndDate,
    string MeetingType,
    string? GoogleMeetUrl,
    string Recurrence,
    uint? Capacity,
    bool TransferAvailable,
    string? TransferDetails,
    string[] InclusivityIds,
    string? BarrierFreeUrl,
    double? Latitude,
    double? Longitude,
    string? Address
);

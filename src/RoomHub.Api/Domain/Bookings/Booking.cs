namespace RoomHub.Api.Domain.Bookings;

public sealed class Booking
{
    public static readonly TimeSpan MaxDuration = TimeSpan.FromHours(4);

    private Booking() { }   // for EF Core

    public Guid Id { get; private set; }
    public Guid RoomId { get; private set; }
    public string BookedBy { get; private set; } = string.Empty;
    public DateTime StartUtc { get; private set; }
    public DateTime EndUtc { get; private set; }
    public int Attendees { get; private set; }
    public BookingStatus Status { get; private set; }

    public bool IsActive => Status == BookingStatus.Active;

    // Order: bookedBy -> range -> attendees -> past -> duration -> overlap.
    // The room (404) is checked BEFORE this, in the handler.
    public static Result<Booking> Create(
        Room room,
        string? bookedBy,
        DateTime startUtc,
        DateTime endUtc,
        int attendees,
        DateTime nowUtc,
        IEnumerable<Booking> roomBookings)
    {
        if (string.IsNullOrWhiteSpace(bookedBy))
            return BookingErrors.BookedByRequired;                    // R2

        var start = ToUtc(startUtc);
        var end = ToUtc(endUtc);

        if (end <= start)
            return BookingErrors.EndBeforeStart;                      // R3

        if (attendees <= 0)
            return BookingErrors.InvalidAttendees;                    // R4
        if (attendees > room.Capacity)
            return BookingErrors.AttendeesExceedCapacity;             // R4

        if (start < nowUtc)
            return BookingErrors.StartInPast;                         // R5

        if (end - start > MaxDuration)
            return BookingErrors.TooLong;                             // R6 (exactly 4h is allowed)

        if (roomBookings.Any(b => b.IsActive && b.Overlaps(start, end)))
            return BookingErrors.RoomOccupied;                        // R7

        return new Booking
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            BookedBy = bookedBy.Trim(),
            StartUtc = start,
            EndUtc = end,
            Attendees = attendees,
            Status = BookingStatus.Active
        };
    }

    public Result<Unit> Cancel()                                      // R8
    {
        if (Status == BookingStatus.Cancelled)
            return BookingErrors.AlreadyCancelled;

        Status = BookingStatus.Cancelled;
        return Unit.Value;
    }

    // Half-open interval [start, end): only "<", never "<=".
    public bool Overlaps(DateTime startUtc, DateTime endUtc) =>
        StartUtc < endUtc && startUtc < EndUtc;

    private static DateTime ToUtc(DateTime value) => value.Kind switch
    {
        DateTimeKind.Utc => value,
        DateTimeKind.Local => value.ToUniversalTime(),
        _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
    };
}

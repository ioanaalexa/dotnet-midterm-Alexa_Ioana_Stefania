namespace RoomHub.Api.Infrastructure.Persistence.Repositories;

public sealed class BookingRepository(RoomHubDbContext db) : IBookingRepository
{
    public Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        db.Bookings.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

    public Task<List<Booking>> ListActiveForRoomAsync(Guid roomId, CancellationToken cancellationToken) =>
        db.Bookings
            .Where(b => b.RoomId == roomId && b.Status == BookingStatus.Active)
            .ToListAsync(cancellationToken);

    public Task<List<Booking>> ListActiveForParticipantAsync(string bookedBy, CancellationToken cancellationToken) =>
        db.Bookings
            .Where(b => b.BookedBy == bookedBy && b.Status == BookingStatus.Active)
            .ToListAsync(cancellationToken);

    public void Add(Booking booking) => db.Bookings.Add(booking);
}

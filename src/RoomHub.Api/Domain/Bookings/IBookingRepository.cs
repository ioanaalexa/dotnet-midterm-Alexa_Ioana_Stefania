namespace RoomHub.Api.Domain.Bookings;

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<Booking>> ListActiveForRoomAsync(Guid roomId, CancellationToken cancellationToken);
    void Add(Booking booking);
}

namespace RoomHub.Api.Domain.Bookings;

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<Booking>> ListActiveForRoomAsync(Guid roomId, CancellationToken cancellationToken);
    Task<List<Booking>> ListActiveForParticipantAsync(string bookedBy, CancellationToken cancellationToken);
    void Add(Booking booking);
}

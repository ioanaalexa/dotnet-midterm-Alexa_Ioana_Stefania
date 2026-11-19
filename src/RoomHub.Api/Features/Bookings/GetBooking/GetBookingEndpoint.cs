namespace RoomHub.Api.Features.Bookings.GetBooking;

public static class GetBookingEndpoint
{
    public sealed record Response(
        Guid Id, Guid RoomId, string BookedBy,
        DateTime StartUtc, DateTime EndUtc, int Attendees, BookingStatus Status);

    public static void Map(RouteGroupBuilder group) =>
        group.MapGet("/bookings/{id:guid}", Handle).WithName("GetBooking");

    private static async Task<Results<Ok<Response>, ProblemHttpResult>> Handle(
        Guid id, IBookingRepository bookings, CancellationToken ct)
    {
        var b = await bookings.GetByIdAsync(id, ct);
        if (b is null)
            return BookingErrors.NotFound.ToProblem();

        return TypedResults.Ok(new Response(
            b.Id, b.RoomId, b.BookedBy, b.StartUtc, b.EndUtc, b.Attendees, b.Status));
    }
}

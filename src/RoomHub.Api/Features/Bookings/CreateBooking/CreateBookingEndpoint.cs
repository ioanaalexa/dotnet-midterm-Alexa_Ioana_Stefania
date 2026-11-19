namespace RoomHub.Api.Features.Bookings.CreateBooking;

public static class CreateBookingEndpoint
{
    public sealed record Request(string? BookedBy, DateTime StartUtc, DateTime EndUtc, int Attendees);

    public sealed record Response(Guid Id, Guid RoomId, BookingStatus Status);

    public static void Map(RouteGroupBuilder group) =>
        group.MapPost("/rooms/{roomId:guid}/bookings", Handle).WithName("CreateBooking");

    private static async Task<Results<Created<Response>, ProblemHttpResult>> Handle(
        Guid roomId,
        Request request,
        ICommandHandler<CreateBookingCommand, CreateBookingResult> handler,
        CancellationToken ct)
    {
        var result = await handler.Handle(
            new CreateBookingCommand(roomId, request.BookedBy, request.StartUtc, request.EndUtc, request.Attendees),
            ct);

        if (result.IsFailure)
            return result.Error.ToProblem();

        var b = result.Value;
        return TypedResults.Created($"/api/bookings/{b.Id}", new Response(b.Id, b.RoomId, b.Status));
    }
}

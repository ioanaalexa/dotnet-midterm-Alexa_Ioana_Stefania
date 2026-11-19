using RoomHub.Api.Features.Bookings.CancelBooking;
using RoomHub.Api.Features.Bookings.CreateBooking;
using RoomHub.Api.Features.Bookings.GetBooking;

namespace RoomHub.Api.Features.Bookings;

public static class BookingsEndpoints
{
    public static IEndpointRouteBuilder MapBookings(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api").WithTags("Bookings");
        CreateBookingEndpoint.Map(group);
        GetBookingEndpoint.Map(group);
        CancelBookingEndpoint.Map(group);
        return app;
    }
}

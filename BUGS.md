# BUGS

| Rule | Where in the starter | How I found it | What I changed |
|---|---|---|---|
| R7 | `BookingService.Create`, inside the `foreach` loop | Compared the starter code with the rules. It used `<=` for intervals, so touching bookings (like 10-12 and 12-14) gave a conflict | Changed `Booking.Overlaps` to use strict `<` (`StartUtc < endUtc && startUtc < EndUtc`) |
| R7 / R8 | `BookingService.Create` | It checked all bookings in the room, even cancelled ones | Filtered to only active bookings (`ListActiveForRoomAsync` and `IsActive`) |
| R8 | `BookingService.Cancel` | It just set `Cancelled` without checking if it was already cancelled | Added a check in `Booking.Cancel` to return `AlreadyCancelled` (409) |
| R5 | `BookingService.Create` | Past booking check was missing and it used `DateTime.UtcNow` directly | Added the past check in `Booking.Create` and injected `TimeProvider` in the handler |
| R6 | `BookingService.Create` | Max duration check was missing | Added the 4-hour max limit in `Booking.Create` (4h exact works) |

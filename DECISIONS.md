# DECISIONS

## 1. A rule I moved
I moved the overlap check (R7) from `Services/BookingService.cs` into `Domain/Bookings/Booking.cs` (`Create` and `Overlaps`). Putting it on the entity means no handler can bypass it.

## 2. Result instead of an exception
A room being occupied or hitting a daily limit is a normal business case, not a crash. `Result<Booking>` makes it obvious from the signature that creation can fail. The handler returns `result.Error`, and `ToProblem()` turns it into 400, 404, or 409.

## 3. Who knows about EF Core?
Only `Infrastructure/Persistence` (`RoomHubDbContext`, configurations, `RoomRepository`, `BookingRepository`) and `Program.cs`. The domain and handlers only know `IRoomRepository`, `IBookingRepository`, and `IUnitOfWork`. I get decoupled business logic, at the cost of extra files and interfaces.

## 4. Flow of a request
`POST /api/rooms/{roomId}/bookings`
-> `CreateBookingEndpoint.Handle`
-> `CreateBookingHandler.Handle`
-> `RoomRepository.GetByIdAsync` (404 right away if room is missing)
-> `BookingRepository.ListActiveForRoomAsync` & `ListActiveForParticipantAsync`
-> `Booking.Create` (checks R2–R7, R9, R10 with `TimeProvider`)
-> `BookingRepository.Add`
-> `IUnitOfWork.SaveChangesAsync` -> SQLite
-> `Result` -> `201 Created` or `ToProblem()`

## 5. What I checked and adjusted myself
Some terminal commands from the AI had weird non-breaking spaces and ran `dotnet build` from the root folder, so I fixed them to target `src/RoomHub.Api`. I also verified edge cases myself: touching bookings (10:00–12:00 and 12:00–14:00) return 201, and 4h exact on a normal room is allowed while 4h01 fails with 400.

## 6. If I had 30 more minutes
I would add unit tests for `Booking.Create` and `Booking.Cancel`, and switch from `EnsureCreated()` to EF migrations. A remaining risk is concurrency: two simultaneous requests could both pass the overlap or R9 limit check before saving to SQLite. Also, `bookedBy` is case-sensitive (`"Ana"` != `"ana"`).

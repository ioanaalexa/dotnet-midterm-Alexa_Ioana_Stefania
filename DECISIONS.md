# DECISIONS

## 1. O regulă mutată
Am mutat verificarea de suprapunere (R7) din `Services/BookingService.cs` în `Domain/Bookings/Booking.cs` (`Create` și `Overlaps`). Așa regula stă direct pe entitate și nu poate fi ocolită din niciun handler.

## 2. Result în loc de excepții
Faptul că sala e ocupată sau că s-a depășit limita pe zi sunt cazuri normale, nu erori de sistem. Cu `Result<Booking>` se vede direct din semnătură că metoda poate eșua. Handler-ul întoarce `result.Error`, iar endpoint-ul apelează `ToProblem()` care face maparea la 400, 404 sau 409.

## 3. Cine știe de EF Core?
Doar `Infrastructure/Persistence` (`RoomHubDbContext`, configurările, `RoomRepository`, `BookingRepository`) și `Program.cs`. Domeniul și handlerele știu doar de `IRoomRepository`, `IBookingRepository` și `IUnitOfWork`. Avantajul e că logica e separată de baza de date, dezavantajul e că sunt mai multe fișiere și interfețe de scris.

## 4. Fluxul unui request
`POST /api/rooms/{roomId}/bookings`
-> `CreateBookingEndpoint.Handle`
-> `CreateBookingHandler.Handle`
-> `RoomRepository.GetByIdAsync` (dă 404 direct dacă nu există sala)
-> `BookingRepository.ListActiveForRoomAsync` & `ListActiveForParticipantAsync`
-> `Booking.Create` (verifică R2–R7, R9, R10 cu `TimeProvider`)
-> `BookingRepository.Add`
-> `IUnitOfWork.SaveChangesAsync` -> SQLite
-> `Result` -> `201 Created` sau `ToProblem()`

## 5. Ce am verificat și ajustat singură
Comenzile primite pentru terminal aveau spații invizibile la copy-paste și rulau `dotnet build` din root în loc de `src/RoomHub.Api`, așa că le-am reparat. Am testat manual și cazurile limită: două rezervări care se ating la fix (ex: 10:00–12:00 și 12:00–14:00) dau 201, iar pe sală normală 4h merge (201) și 4h01 pică (400).

## 6. Ce aș face dacă aș mai avea 30 de minute
Aș scrie unit teste pe `Booking.Create` și `Booking.Cancel` și aș pune migrări EF în loc de `EnsureCreated()`. O problemă rămasă e pe concurență: două request-uri simultane pot trece ambele de check-ul de overlap sau de limita pe zi înainte să se facă save în SQLite. În plus, `bookedBy` e case-sensitive (`"Ana"` != `"ana"`).
# BUGS

| Regulă | Unde era în starter | Cum am găsit-o | Ce am schimbat |
|---|---|---|---|
| R7 | `BookingService.Create`, în `foreach` | Am comparat codul cu cerința. Folosea `<=` la intervale, deci două rezervări lipite (ex: 10-12 și 12-14) dădeau conflict | Am pus `<` strict în `Booking.Overlaps` (`StartUtc < endUtc && startUtc < EndUtc`) |
| R7 / R8 | `BookingService.Create` | Verifica toate rezervările din sală, chiar și pe cele anulate | Verific doar rezervările active (`ListActiveForRoomAsync` și `IsActive`) |
| R8 | `BookingService.Cancel` | Punea direct statusul `Cancelled` fără să vadă dacă era deja anulată | Am adăugat check în `Booking.Cancel` să dea `AlreadyCancelled` (409) |
| R5 | `BookingService.Create` | Lipsea verificarea pentru rezervări în trecut și folosea direct `DateTime.UtcNow` | Am pus check-ul în `Booking.Create` și am injectat `TimeProvider` în handler |
| R6 | `BookingService.Create` | Nu verifica durata maximă | Am pus limita de maxim 4 ore în `Booking.Create` (fix 4 ore merge) |
# AI-LOG

| Time (approx.) | What I asked | What I kept | What I changed or checked myself |
|---|---|---|---|
| 08:48 | How to clone my GitHub repo and open it in VS Code / Rider on Mac | `git clone` and `open -a "Rider"` commands | Deleted an old empty folder with `.idea` first and cloned clean |
| 09:02 | Clean vertical slices structure (`Features/`, `Domain/`, EF Core SQLite) | The slice layout, `Result` pattern, and repositories | Read `Booking.cs` and `CreateBookingHandler.cs` to check 404 happens before 400/409 |
| 09:14 | Why files weren't showing in Rider and how to create `.gitignore` | `dotnet new gitignore` + `*.db` and reload from disk | Checked with `ls -la` and `dotnet build src/RoomHub.Api` that `bin/` and `obj/` are ignored |
| 09:22 | How to implement R9 (max 2 active bookings per UTC day per participant) | Rule in `Booking.Create` + `ListActiveForParticipantAsync` | Cleaned up copy-paste spacing in terminal and kept day filtering in domain, not SQL |
| 09:30 | How to implement R10 and R11 (large room 2h limit and cancel cutoff) | Domain checks using room capacity and `TimeProvider` | Tested boundary cases manually (2h vs 2h01, touching intervals) |
| 09:42 | Quick `curl` commands to test the API on localhost:5090 | The `POST /api/rooms` and booking `curl` commands | Ran them in a second terminal and verified 201 Created and 409 Conflict |

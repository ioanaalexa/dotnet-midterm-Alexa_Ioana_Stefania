# AI-LOG

| Ora (aprox.) | Ce am întrebat | Ce am păstrat | Ce am schimbat / verificat eu |
|---|---|---|---|
| 08:48 | Cum clonez repo-ul meu de pe GitHub și cum îl deschid în VS Code / Rider pe Mac | Comenzile `git clone` și `open -a "Rider"` | Aveam deja un folder gol cu `.idea`, l-am șters din terminal și am clonat curat |
| 09:02 | Structură pe vertical slices (`Features/`, `Domain/`, EF Core SQLite) în loc de folderele din starter | Arhitectura pe slice-uri, `Result` și repository-urile | Am citit `Booking.cs` și handler-ul ca să fiu sigură că 404 se verifică înainte de 400/409 |
| 09:14 | De ce nu se văd fișierele în Rider și cum fac `.gitignore` | `dotnet new gitignore` + `*.db` și reload din Rider | Am verificat cu `ls -la` și `dotnet build src/RoomHub.Api` că nu intră `bin/` și `obj/` |
| 09:22 | Cum implementez R9 (maxim 2 rezervări active pe zi per participant) | Regula în `Booking.Create` + `ListActiveForParticipantAsync` | Am curățat comenzile pentru că aveau spații ciudate la copy-paste |
| 09:28 | Cum implementez R10 (durată maximă 2h la săli cu capacitate > 10) | Verificarea pe capacitatea sălii în domeniu | Am testat în Rider limitele exacte: 2h (201) și 2h01 (400) |
| 09:33 | Cereri de test pentru `roomhub.http` pe R10 și cazurile limită | Cererile din `roomhub.http` | Am rulat request-urile în Rider și am verificat statusurile 201, 400, 404, 409 |
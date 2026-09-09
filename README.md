# MediCare Clinic — ASP.NET Core MVC (.NET 8)

This project recreates the supplied MediCare Clinic home page in English and makes the important buttons functional.

## Included
- Responsive home page matching the reference layout
- Specialties linked to filtered doctor lists
- Featured doctors loaded from SQL Server through EF Core
- Doctor profile pages
- Account registration / login / logout using ASP.NET Core Identity
- Appointment booking and "My Appointments"
- Appointment cancellation
- Basic Admin dashboard
- Seeded specialties and doctors
- Images extracted from the supplied reference screenshot and bundled under `wwwroot/images`

## Requirements
- Visual Studio 2022 (17.8 or newer recommended)
- .NET 8 SDK
- SQL Server LocalDB (installed by the Visual Studio ASP.NET workload)

## Open correctly in Visual Studio
1. Extract the ZIP.
2. Open `MediCareClinic.csproj` in Visual Studio (or File > Open > Project/Solution).
3. Wait for NuGet restore to finish.
4. Make sure the startup profile uses HTTPS / IIS Express or the project name.
5. Press `Ctrl + F5` to run.
6. The database `MediCareClinicDb` is created automatically the first time the app starts.

## Admin test account
- Email: `admin@medicare.local`
- Password: `Admin123!`

## If LocalDB is not available
Change `DefaultConnection` in `appsettings.json` to your SQL Server instance, then run again.

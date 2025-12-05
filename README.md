# Appointment System

A .NET 8 / Avalonia desktop application for managing medical appointments, clients, and services (imaging modalities).

## Features

- **Clients Management**: Add, edit, delete, and view client (patient) records with contact info and notes.
- **Services Management**: Manage imaging services (XRAY, ULTRASOUND, CT SCAN, MRI SCAN, MAMMOGRAPHY) with duration and pricing.
- **Appointments Scheduling**: Create, edit, and delete appointments linking clients to services with status tracking.
- **Dashboard**: Overview of system statistics.
- **User Authentication**: Login system with role-based access.
- **SQLite Database**: Persistent data storage with automatic seeding of sample data.

## Technology Stack

- **.NET 8** (net8.0)
- **Avalonia UI 11.x** - Cross-platform desktop framework
- **Entity Framework Core** - ORM for database access
- **SQLite** - Lightweight embedded database
- **MVVM Pattern** - Clean architecture with ViewModels

## Project Structure

```
AppointmentSystem/
├── AppointmentSystem.Desktop/          # Avalonia UI (WinExe)
│   ├── Views/                          # XAML UI components
│   ├── ViewModels/                     # MVVM ViewModels
│   ├── Styles/                         # Global styles
│   └── Program.cs / App.axaml.cs       # App entry point
├── AppointmentSystem.Business/         # Business logic & DTOs
│   ├── Services/                       # Service classes
│   └── Models/                         # Data Transfer Objects (DTOs)
├── AppointmentSystem.Data/             # Data access layer
│   ├── Repositories/                   # Repository pattern
│   ├── Models/                         # Entity models
│   └── AppointmentsDbContext.cs        # EF Core DbContext
└── AppointmentSystem.Web/              # ASP.NET Core web project (optional)
```

## Setup & Running

### Prerequisites
- .NET 8 SDK
- SQLite (usually included with .NET)

### Clone & Build
```bash
git clone https://github.com/yourusername/AppointmentSystem.git
cd AppointmentSystem
dotnet build AppointmentSystem.sln
```

### Run Desktop App
```bash
dotnet run --project AppointmentSystem.Desktop/AppointmentSystem.Desktop.csproj
```

The app will:
1. Create the SQLite database at `bin/Debug/net8.0/appointments.db` on first run.
2. Seed sample clients, services, and appointments.
3. Display the login screen (default: admin/admin or staff/staff).

### Database Location
- **Development**: `AppointmentSystem.Desktop/bin/Debug/net8.0/appointments.db`

## Usage

### Login
- Default credentials:
  - Username: `admin` / Password: `admin` (Admin role)
  - Username: `staff` / Password: `staff` (Staff role)

### Clients Tab
- **View**: List of all patients/clients
- **Add**: Create a new client (name, phone, email, notes)
- **Edit**: Double-click or click Edit button to modify client details
- **Delete**: Remove a client
- **Refresh**: Reload the list from the database

### Services Tab
- **View**: List of imaging services (XRAY, ULTRASOUND, CT SCAN, MRI SCAN, MAMMOGRAPHY)
- **Add**: Create a new service (name, duration in minutes, price)
- **Edit**: Double-click or click Edit button to modify service details
- **Delete**: Remove a service
- **Refresh**: Reload the list from the database

### Appointments Tab
- **View**: List of scheduled appointments
- **Add**: Create an appointment (select client, service, date/time, status, notes)
- **Edit**: Double-click or click Edit button to modify appointment
- **Delete**: Cancel an appointment
- **Refresh**: Reload the list; optionally filter by date

### Dashboard Tab
- Overview and statistics (extensible).

## UI Notes

- **Platform**: Tested on Linux (X11 and Wayland)
- **Rendering**: Uses ListBox controls with templated Grid layouts for reliable cross-platform rendering
- **XAML**: Runtime XAML loading (compiled XAML disabled for development flexibility)

## Known Limitations & Future Improvements

- DataGrid rendering on some platforms; currently using ListBox as fallback
- Compiled XAML/bindings can be re-enabled after further testing
- Additional validation and error handling can be enhanced
- Export/reporting features (PDF, CSV) not yet implemented
- Payment/billing integration not included

## Development Notes

- **Diagnostic Logging**: Console logs available for debugging (can be removed for release)
- **MVVM Pattern**: ViewModels use ObservableCollections for reactive UI updates
- **Dependency Injection**: Configured in `App.xaml.cs` for services and repositories
- **Code-behind**: Minimal use; primarily for event handlers and control initialization

## License

[Add your license here, e.g., MIT, Apache 2.0, etc.]

## Contact

[Your name/contact info]

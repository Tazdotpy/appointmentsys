using System;
using System.Threading.Tasks;
using AppointmentSystem.Data.Models;
using AppointmentSystem.Data.Utils;
using Microsoft.EntityFrameworkCore;

namespace AppointmentSystem.Data;

public static class DatabaseInitializer
{
    public static async Task EnsureCreatedAndSeedAsync()
    {
        using var db = new AppointmentsDbContext();

        await db.Database.EnsureCreatedAsync();

        // Already seeded?
        if (await db.Clients.AnyAsync())
            return;

        // --- Clients ---
        var clients = new[]
        {
            new Client { Name = "Carlos Méndez",  Phone = "809-555-1212", Email = "carlos@example.com",    Notes = "Prefers morning appointments" },
            new Client { Name = "María González", Phone = "829-555-4545", Email = "maria.g@example.com" },
            new Client { Name = "Juan Pérez",     Phone = "849-555-9898", Email = "juanp@example.com",    Notes = "Follow-up every 2 weeks" },
            new Client { Name = "Ana Martínez",   Phone = "809-555-7744", Email = "ana.m@example.com" },
            new Client { Name = "Luis Ramírez",   Phone = "829-555-1122", Email = "lramirez@example.com", Notes = "Chronic pain patient" },
            new Client { Name = "Sofía Vargas",   Phone = "849-555-3141", Email = "svargas@example.com",  Notes = "New client" }
        };

        db.Clients.AddRange(clients);

        // --- Services ---
        var services = new[]
        {
            new Service { Name = "XRAY",        DurationMinutes = 15, Price = 2000m },
            new Service { Name = "ULTRASOUND",  DurationMinutes = 30, Price = 3500m },
            new Service { Name = "CT SCAN",     DurationMinutes = 45, Price = 8000m },
            new Service { Name = "MRI SCAN",    DurationMinutes = 60, Price = 12000m },
            new Service { Name = "MAMMOGRAPHY", DurationMinutes = 30, Price = 5000m }
        };

        db.Services.AddRange(services);
        await db.SaveChangesAsync();

        // Short aliases
        var c = clients;
        var s = services;

        // Helper to create appointments using service duration
        Appointment Appt(int ci, int si, int year, int month, int day, int hour, int minute, string status, string? notes = null)
        {
            var svc = s[si];
            var start = new DateTime(year, month, day, hour, minute, 0);
            var end = start.AddMinutes(svc.DurationMinutes);
            return new Appointment
            {
                Client   = c[ci],
                Service  = svc,
                StartTime = start,
                EndTime   = end,
                Status    = status,
                Notes     = notes
            };
        }

        var appts = new[]
        {
            Appt(0, 0, 2025, 1, 15,  9,  0, "Scheduled"),
            Appt(1, 1, 2025, 1, 15, 10,  0, "Scheduled", "First session"),
            Appt(2, 2, 2025, 1, 15, 11, 30, "Completed"),
            Appt(3, 3, 2025, 1, 15, 12,  0, "Scheduled"),
            Appt(4, 4, 2025, 1, 15, 14,  0, "Canceled", "Rescheduled"),

            Appt(5, 0, 2025, 1, 16,  9,  0, "Scheduled"),
            Appt(1, 2, 2025, 1, 16, 10,  0, "Completed"),
            Appt(0, 1, 2025, 1, 16, 11,  0, "Scheduled"),
            Appt(2, 0, 2025, 1, 16, 13,  0, "Scheduled"),

            Appt(3, 4, 2025, 1, 17,  9, 30, "Scheduled"),
            Appt(4, 0, 2025, 1, 17, 11,  0, "Scheduled"),
            Appt(5, 3, 2025, 1, 17, 12,  0, "Scheduled")
        };

        db.Appointments.AddRange(appts);

        // --- Users ---
        var users = new[]
        {
            new User
            {
                Username     = "admin",
                PasswordHash = SecurityHelper.HashPassword("admin"),
                Role         = "Admin"
            },
            new User
            {
                Username     = "staff",
                PasswordHash = SecurityHelper.HashPassword("staff"),
                Role         = "Staff"
            }
        };

        db.Users.AddRange(users);

        await db.SaveChangesAsync();
    }
}


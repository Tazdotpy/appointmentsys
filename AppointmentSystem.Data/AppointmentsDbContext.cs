using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using AppointmentSystem.Data.Models;

namespace AppointmentSystem.Data;

public class AppointmentsDbContext : DbContext
{
    private readonly string _dbPath;

    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<User> Users => Set<User>();

    public AppointmentsDbContext()
    {
        // DB in working directory (bin/Debug/net8.0/)
        _dbPath = Path.Combine(AppContext.BaseDirectory, "appointments.db");
    }

    public AppointmentsDbContext(DbContextOptions<AppointmentsDbContext> options)
        : base(options)
    {
        _dbPath = Path.Combine(AppContext.BaseDirectory, "appointments.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite($"Data Source={_dbPath}");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Client>(e =>
        {
            e.Property(c => c.Name).IsRequired().HasMaxLength(120);
        });

        modelBuilder.Entity<Service>(e =>
        {
            e.Property(s => s.Name).IsRequired().HasMaxLength(120);
        });

        modelBuilder.Entity<Appointment>(e =>
        {
            e.Property(a => a.Status).IsRequired().HasMaxLength(20);
        });

        modelBuilder.Entity<User>(e =>
        {
            e.Property(u => u.Username).IsRequired().HasMaxLength(50);
            e.Property(u => u.PasswordHash).IsRequired();
            e.Property(u => u.Role).IsRequired().HasMaxLength(20);
        });
    }
}

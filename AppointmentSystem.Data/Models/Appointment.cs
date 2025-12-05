using System;

namespace AppointmentSystem.Data.Models;

public class Appointment
{
    public int AppointmentId { get; set; }

    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;

    public int ServiceId { get; set; }
    public Service Service { get; set; } = null!;

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    // Scheduled / Completed / Canceled
    public string Status { get; set; } = "Scheduled";

    public string? Notes { get; set; }
}

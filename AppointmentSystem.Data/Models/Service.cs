using System.Collections.Generic;

namespace AppointmentSystem.Data.Models;

public class Service
{
    public int ServiceId { get; set; }
    public string Name { get; set; } = null!;
    public int DurationMinutes { get; set; }
    public decimal? Price { get; set; }

    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}

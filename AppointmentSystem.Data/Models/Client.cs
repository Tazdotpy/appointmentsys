using System.Collections.Generic;

namespace AppointmentSystem.Data.Models;

public class Client
{
    public int ClientId { get; set; }
    public string Name { get; set; } = null!;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Notes { get; set; }

    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}

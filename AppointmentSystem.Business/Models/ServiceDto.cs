namespace AppointmentSystem.Business.Models;

public class ServiceDto
{
    public int ServiceId { get; set; }
    public string Name { get; set; } = null!;
    public int DurationMinutes { get; set; }
    public decimal? Price { get; set; }
}


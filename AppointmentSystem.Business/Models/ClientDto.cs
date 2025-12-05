namespace AppointmentSystem.Business.Models;

public class ClientDto
{
    public int ClientId { get; set; }
    public string Name { get; set; } = null!;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Notes { get; set; }
}

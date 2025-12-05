namespace AppointmentSystem.Business.Models;

public class AppointmentDto
{
    public int AppointmentId { get; set; }
    public int ClientId { get; set; }
    public int ServiceId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Status { get; set; } = "";
    public string? Notes { get; set; }

    public override string ToString()
    {
        try
        {
            return $"{StartTime:yyyy-MM-dd HH:mm} — Client:{ClientId} Service:{ServiceId} {Status} {Notes}".Trim();
        }
        catch
        {
            return base.ToString();
        }
    }
}

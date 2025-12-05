using AppointmentSystem.Data.Models;
using AppointmentSystem.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AppointmentSystem.Data.Repositories.Implementations;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly AppointmentsDbContext _db;

    public AppointmentRepository(AppointmentsDbContext db)
    {
        _db = db;
    }

    public List<Appointment> GetAll()
        => _db.Appointments
              .Include(a => a.Client)
              .Include(a => a.Service)
              .OrderBy(a => a.StartTime)
              .ToList();

    public Appointment? GetById(int id)
        => _db.Appointments
              .Include(a => a.Client)
              .Include(a => a.Service)
              .FirstOrDefault(a => a.AppointmentId == id);

    public void Add(Appointment appointment)
    {
        _db.Appointments.Add(appointment);
        _db.SaveChanges();
    }

    public void Update(Appointment appointment)
    {
        _db.Appointments.Update(appointment);
        _db.SaveChanges();
    }

    public void Delete(int id)
    {
        var existing = _db.Appointments.Find(id);
        if (existing is null) return;
        _db.Appointments.Remove(existing);
        _db.SaveChanges();
    }

    public List<Appointment> GetByDate(DateTime date)
        => _db.Appointments
              .Include(a => a.Client)
              .Include(a => a.Service)
              .Where(a => a.StartTime.Date == date.Date)
              .OrderBy(a => a.StartTime)
              .ToList();

    public List<Appointment> GetByClient(int clientId)
        => _db.Appointments
              .Include(a => a.Client)
              .Include(a => a.Service)
              .Where(a => a.ClientId == clientId)
              .OrderBy(a => a.StartTime)
              .ToList();

    public List<Appointment> GetByService(int serviceId)
        => _db.Appointments
              .Include(a => a.Client)
              .Include(a => a.Service)
              .Where(a => a.ServiceId == serviceId)
              .OrderBy(a => a.StartTime)
              .ToList();
}

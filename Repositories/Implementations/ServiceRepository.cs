using AppointmentSystem.Data.Models;
using AppointmentSystem.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AppointmentSystem.Data.Repositories.Implementations;

public class ServiceRepository : IServiceRepository
{
    private readonly AppointmentsDbContext _db;

    public ServiceRepository(AppointmentsDbContext db)
    {
        _db = db;
    }

    public List<Service> GetAll()
        => _db.Services
              .OrderBy(s => s.Name)
              .ToList();

    public Service? GetById(int id)
        => _db.Services.Find(id);

    public void Add(Service service)
    {
        _db.Services.Add(service);
        _db.SaveChanges();
    }

    public void Update(Service service)
    {
        _db.Services.Update(service);
        _db.SaveChanges();
    }

    public void Delete(int id)
    {
        var existing = _db.Services.Find(id);
        if (existing is null) return;
        _db.Services.Remove(existing);
        _db.SaveChanges();
    }
}

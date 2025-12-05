using AppointmentSystem.Data.Models;
using AppointmentSystem.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AppointmentSystem.Data.Repositories.Implementations;

public class ClientRepository : IClientRepository
{
    private readonly AppointmentsDbContext _db;

    public ClientRepository(AppointmentsDbContext db)
    {
        _db = db;
    }

    public List<Client> GetAll()
        => _db.Clients
              .OrderBy(c => c.Name)
              .ToList();

    public Client? GetById(int id)
        => _db.Clients.Find(id);

    public void Add(Client client)
    {
        _db.Clients.Add(client);
        _db.SaveChanges();
    }

    public void Update(Client client)
    {
        _db.Clients.Update(client);
        _db.SaveChanges();
    }

    public void Delete(int id)
    {
        var existing = _db.Clients.Find(id);
        if (existing is null) return;
        _db.Clients.Remove(existing);
        _db.SaveChanges();
    }
}

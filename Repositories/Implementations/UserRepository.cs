using AppointmentSystem.Data.Models;
using AppointmentSystem.Data.Repositories.Interfaces;

namespace AppointmentSystem.Data.Repositories.Implementations;

public class UserRepository : IUserRepository
{
    private readonly AppointmentsDbContext _db;

    public UserRepository(AppointmentsDbContext db)
    {
        _db = db;
    }

    public User? GetByUsername(string username)
        => _db.Users.FirstOrDefault(u => u.Username == username);

    public void Add(User user)
    {
        _db.Users.Add(user);
        _db.SaveChanges();
    }
}

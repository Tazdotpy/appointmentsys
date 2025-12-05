using AppointmentSystem.Data.Models;

namespace AppointmentSystem.Data.Repositories.Interfaces;

public interface IClientRepository
{
    List<Client> GetAll();
    Client? GetById(int id);
    void Add(Client client);
    void Update(Client client);
    void Delete(int id);
}

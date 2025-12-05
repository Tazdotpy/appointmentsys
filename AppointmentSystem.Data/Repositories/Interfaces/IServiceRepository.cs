using System;
using System.Collections.Generic;


using AppointmentSystem.Data.Models;

namespace AppointmentSystem.Data.Repositories.Interfaces;

public interface IServiceRepository
{
    List<Service> GetAll();
    Service? GetById(int id);
    void Add(Service service);
    void Update(Service service);
    void Delete(int id);
}

using System;
using System.Collections.Generic;


using AppointmentSystem.Data.Models;

namespace AppointmentSystem.Data.Repositories.Interfaces;

public interface IUserRepository
{
    User? GetByUsername(string username);
    void Add(User user);
}

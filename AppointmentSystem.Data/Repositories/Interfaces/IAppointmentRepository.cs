using System;
using System.Collections.Generic;


using AppointmentSystem.Data.Models;

namespace AppointmentSystem.Data.Repositories.Interfaces;

public interface IAppointmentRepository
{
    List<Appointment> GetAll();
    Appointment? GetById(int id);

    void Add(Appointment appointment);
    void Update(Appointment appointment);
    void Delete(int id);

    List<Appointment> GetByDate(DateTime date);
    List<Appointment> GetByClient(int clientId);
    List<Appointment> GetByService(int serviceId);
}


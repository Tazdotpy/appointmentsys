using System;
using System.Collections.Generic;
using AppointmentSystem.Business.Models;

namespace AppointmentSystem.Business.Services.Interfaces;

public interface IAppointmentService
{
    List<AppointmentDto> GetAll();
    AppointmentDto? Get(int id);

    void Create(AppointmentDto dto);
    void Update(AppointmentDto dto);
    void Delete(int id);

    List<AppointmentDto> GetByDate(DateTime date);
}

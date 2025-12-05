using System.Collections.Generic;
using AppointmentSystem.Business.Models;

namespace AppointmentSystem.Business.Services.Interfaces;

public interface IServiceService
{
    List<ServiceDto> GetAll();
    ServiceDto? Get(int id);
    void Create(ServiceDto dto);
    void Update(ServiceDto dto);
    void Delete(int id);
}

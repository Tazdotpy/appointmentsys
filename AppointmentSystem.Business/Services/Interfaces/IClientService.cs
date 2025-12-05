using System.Collections.Generic;
using AppointmentSystem.Business.Models;

namespace AppointmentSystem.Business.Services.Interfaces;

public interface IClientService
{
    List<ClientDto> GetAll();
    ClientDto? Get(int id);
    void Create(ClientDto dto);
    void Update(ClientDto dto);
    void Delete(int id);
}

using System.Collections.Generic;
using System.Linq;
using AppointmentSystem.Business.Exceptions;
using AppointmentSystem.Business.Models;
using AppointmentSystem.Business.Services.Interfaces;

using AppointmentSystem.Data.Repositories.Interfaces;
using AppointmentSystem.Data.Models;


namespace AppointmentSystem.Business.Services.Implementations;

public class ClientService : IClientService
{
    private readonly IClientRepository _repo;

    public ClientService(IClientRepository repo)
    {
        _repo = repo;
    }

    public List<ClientDto> GetAll()
        => _repo.GetAll()
                .Select(c => new ClientDto
                {
                    ClientId = c.ClientId,
                    Name = c.Name,
                    Phone = c.Phone,
                    Email = c.Email,
                    Notes = c.Notes
                })
                .ToList();

    public ClientDto? Get(int id)
    {
        var c = _repo.GetById(id);
        if (c == null) return null;

        return new ClientDto
        {
            ClientId = c.ClientId,
            Name = c.Name,
            Phone = c.Phone,
            Email = c.Email,
            Notes = c.Notes
        };
    }

    public void Create(ClientDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new BusinessException("Client name is required.");

        var entity = new Client
        {
            Name = dto.Name,
            Phone = dto.Phone,
            Email = dto.Email,
            Notes = dto.Notes
        };

        _repo.Add(entity);
    }

    public void Update(ClientDto dto)
    {
        var existing = _repo.GetById(dto.ClientId)
                      ?? throw new BusinessException("Client not found.");

        existing.Name = dto.Name;
        existing.Phone = dto.Phone;
        existing.Email = dto.Email;
        existing.Notes = dto.Notes;

        _repo.Update(existing);
    }

    public void Delete(int id)
    {
        _repo.Delete(id);
    }
}

using System.Collections.Generic;
using System.Linq;
using AppointmentSystem.Business.Exceptions;
using AppointmentSystem.Business.Models;
using AppointmentSystem.Business.Services.Interfaces;

using AppointmentSystem.Data.Repositories.Interfaces;
using AppointmentSystem.Data.Models;


namespace AppointmentSystem.Business.Services.Implementations;

public class ServiceService : IServiceService
{
    private readonly IServiceRepository _repo;

    public ServiceService(IServiceRepository repo)
    {
        _repo = repo;
    }

    public List<ServiceDto> GetAll()
        => _repo.GetAll().Select(s => new ServiceDto
        {
            ServiceId = s.ServiceId,
            Name = s.Name,
            DurationMinutes = s.DurationMinutes,
            Price = s.Price
        }).ToList();

    public ServiceDto? Get(int id)
    {
        var s = _repo.GetById(id);
        if (s == null) return null;

        return new ServiceDto
        {
            ServiceId = s.ServiceId,
            Name = s.Name,
            DurationMinutes = s.DurationMinutes,
            Price = s.Price
        };
    }

    public void Create(ServiceDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new BusinessException("Service name is required.");

        if (dto.DurationMinutes <= 0)
            throw new BusinessException("Duration must be positive.");

        var entity = new Service
        {
            Name = dto.Name,
            DurationMinutes = dto.DurationMinutes,
            Price = dto.Price
        };

        _repo.Add(entity);
    }

    public void Update(ServiceDto dto)
    {
        var s = _repo.GetById(dto.ServiceId)
                ?? throw new BusinessException("Service not found.");

        s.Name = dto.Name;
        s.DurationMinutes = dto.DurationMinutes;
        s.Price = dto.Price;

        _repo.Update(s);
    }

    public void Delete(int id)
    {
        _repo.Delete(id);
    }
}

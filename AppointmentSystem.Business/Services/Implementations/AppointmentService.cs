using System;
using System.Collections.Generic;
using System.Linq;
using AppointmentSystem.Business.Exceptions;
using AppointmentSystem.Business.Models;
using AppointmentSystem.Business.Services.Interfaces;
using AppointmentSystem.Data.Models;
using AppointmentSystem.Data.Repositories.Interfaces;

namespace AppointmentSystem.Business.Services.Implementations;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _apptRepo;
    private readonly IClientRepository _clientRepo;
    private readonly IServiceRepository _serviceRepo;

    public AppointmentService(
        IAppointmentRepository apptRepo,
        IClientRepository clientRepo,
        IServiceRepository serviceRepo)
    {
        _apptRepo = apptRepo;
        _clientRepo = clientRepo;
        _serviceRepo = serviceRepo;
    }

    public List<AppointmentDto> GetAll()
        => _apptRepo.GetAll()
                    .Select(ToDto)
                    .ToList();

    public AppointmentDto? Get(int id)
    {
        var a = _apptRepo.GetById(id);
        return a == null ? null : ToDto(a);
    }

    public void Create(AppointmentDto dto)
    {
        Validate(dto);

        var service = _serviceRepo.GetById(dto.ServiceId)
                      ?? throw new BusinessException("Service not found.");

        // Auto calculate end time
        dto.EndTime = dto.StartTime.AddMinutes(service.DurationMinutes);

        CheckOverlap(dto, isUpdate: false);

        var entity = new Appointment
        {
            ClientId = dto.ClientId,
            ServiceId = dto.ServiceId,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            Status = dto.Status,
            Notes = dto.Notes
        };

        _apptRepo.Add(entity);
    }

    public void Update(AppointmentDto dto)
    {
        var existing = _apptRepo.GetById(dto.AppointmentId)
                       ?? throw new BusinessException("Appointment not found.");

        Validate(dto);
        CheckOverlap(dto, isUpdate: true);

        existing.ClientId = dto.ClientId;
        existing.ServiceId = dto.ServiceId;
        existing.StartTime = dto.StartTime;
        existing.EndTime = dto.EndTime;
        existing.Status = dto.Status;
        existing.Notes = dto.Notes;

        _apptRepo.Update(existing);
    }

    public void Delete(int id)
    {
        _apptRepo.Delete(id);
    }

    public List<AppointmentDto> GetByDate(DateTime date)
        => _apptRepo.GetByDate(date)
                    .Select(ToDto)
                    .ToList();

    // -----------------------
    // Helper methods
    // -----------------------

    private void Validate(AppointmentDto dto)
    {
        if (_clientRepo.GetById(dto.ClientId) == null)
            throw new BusinessException("Client does not exist.");

        if (_serviceRepo.GetById(dto.ServiceId) == null)
            throw new BusinessException("Service does not exist.");

        if (dto.StartTime == default)
            throw new BusinessException("Start time is required.");

        if (dto.StartTime < DateTime.Now.Date.AddYears(-1))
            throw new BusinessException("Start time is invalid.");
    }

    private void CheckOverlap(AppointmentDto dto, bool isUpdate)
    {
        var sameDayAppts = _apptRepo.GetByDate(dto.StartTime.Date);

        foreach (var ap in sameDayAppts)
        {
            if (isUpdate && ap.AppointmentId == dto.AppointmentId)
                continue;

            bool overlap =
                dto.StartTime < ap.EndTime &&
                dto.EndTime > ap.StartTime;

            if (overlap)
                throw new BusinessException("Appointment overlaps with an existing one.");
        }
    }

    private AppointmentDto ToDto(Appointment a)
    {
        return new AppointmentDto
        {
            AppointmentId = a.AppointmentId,
            ClientId = a.ClientId,
            ServiceId = a.ServiceId,
            StartTime = a.StartTime,
            EndTime = a.EndTime,
            Status = a.Status,
            Notes = a.Notes
        };
    }
}

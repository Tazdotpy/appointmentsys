using AppointmentSystem.Business.Models;
using System.Collections.ObjectModel;
using AppointmentSystem.Business.Services.Implementations;
using AppointmentSystem.Data;
using AppointmentSystem.Data.Repositories.Implementations;
using System.Linq;
using System;

namespace AppointmentSystem.Desktop.ViewModels;

public class AppointmentsViewModel : ViewModelBase
{
    public UserDto CurrentUser { get; }

    public ObservableCollection<AppointmentDto> Appointments { get; } = new();

    private readonly AppointmentService _appointmentService;

    public AppointmentsViewModel(UserDto user)
    {
        CurrentUser = user;

        var db = new AppointmentsDbContext();
        _appointmentService = new AppointmentService(
            new AppointmentRepository(db),
            new ClientRepository(db),
            new ServiceRepository(db)
        );
    }

    public void LoadAll()
    {
        var list = _appointmentService.GetAll();
        Appointments.Clear();
        foreach (var a in list)
            Appointments.Add(a);
    }

    public void LoadByDate(DateTime date)
    {
        var list = _appointmentService.GetByDate(date);
        Appointments.Clear();
        foreach (var a in list)
            Appointments.Add(a);
    }

    public void Delete(int appointmentId)
    {
        _appointmentService.Delete(appointmentId);
        var existing = Appointments.FirstOrDefault(a => a.AppointmentId == appointmentId);
        if (existing != null) Appointments.Remove(existing);
    }

    public AppointmentDto? FindById(int appointmentId) => Appointments.FirstOrDefault(a => a.AppointmentId == appointmentId);
}

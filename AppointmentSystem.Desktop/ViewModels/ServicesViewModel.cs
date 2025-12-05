using AppointmentSystem.Business.Models;
using System.Collections.ObjectModel;
using AppointmentSystem.Business.Services.Implementations;
using AppointmentSystem.Data;
using AppointmentSystem.Data.Repositories.Implementations;
using System.Linq;

namespace AppointmentSystem.Desktop.ViewModels;

public class ServicesViewModel : ViewModelBase
{
    public UserDto CurrentUser { get; }

    public ObservableCollection<ServiceDto> Services { get; } = new();

    private readonly ServiceService _serviceService;

    public ServicesViewModel(UserDto user)
    {
        CurrentUser = user;

        var db = new AppointmentsDbContext();
        _serviceService = new ServiceService(new ServiceRepository(db));
    }

    public void LoadAll()
    {
        var list = _serviceService.GetAll();
        Services.Clear();
        foreach (var s in list)
            Services.Add(s);
    }

    public void Delete(int id)
    {
        _serviceService.Delete(id);
        var existing = Services.FirstOrDefault(s => s.ServiceId == id);
        if (existing != null) Services.Remove(existing);
    }
}

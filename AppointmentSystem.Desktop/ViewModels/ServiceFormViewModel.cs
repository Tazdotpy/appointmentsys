using System;
using AppointmentSystem.Business.Models;

namespace AppointmentSystem.Desktop.ViewModels;

public class ServiceFormViewModel : ViewModelBase
{
    public ServiceDto? EditingService { get; }
    public Action? CloseAction { get; set; }

    public ServiceFormViewModel(ServiceDto? editing = null)
    {
        EditingService = editing;
    }
}

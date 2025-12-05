using System;
using AppointmentSystem.Business.Models;

namespace AppointmentSystem.Desktop.ViewModels;

public class AppointmentFormViewModel : ViewModelBase
{
    public AppointmentDto? EditingAppointment { get; }

    public Action? CloseAction { get; set; }

    public AppointmentFormViewModel(AppointmentDto? editing = null)
    {
        EditingAppointment = editing;
    }
}

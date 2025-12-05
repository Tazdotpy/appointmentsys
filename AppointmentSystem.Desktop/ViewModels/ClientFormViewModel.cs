using System;
using AppointmentSystem.Business.Models;

namespace AppointmentSystem.Desktop.ViewModels;

public class ClientFormViewModel : ViewModelBase
{
    public ClientDto? EditingClient { get; }
    public Action? CloseAction { get; set; }

    public ClientFormViewModel(ClientDto? editing = null)
    {
        EditingClient = editing;
    }
}

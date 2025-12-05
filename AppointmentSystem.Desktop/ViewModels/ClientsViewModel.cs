using AppointmentSystem.Business.Models;
using System.Linq;
using System.Collections.ObjectModel;
using AppointmentSystem.Business.Services.Implementations;
using AppointmentSystem.Data;
using AppointmentSystem.Data.Repositories.Implementations;
using System;

namespace AppointmentSystem.Desktop.ViewModels;

public class ClientsViewModel : ViewModelBase
{
    public UserDto CurrentUser { get; }

    public ObservableCollection<ClientDto> Clients { get; } = new();

    private readonly ClientService _clientService;

    public ClientsViewModel(UserDto user)
    {
        CurrentUser = user;

        var db = new AppointmentsDbContext();
        _clientService = new ClientService(new ClientRepository(db));
    }

    public void LoadAll()
    {
        var list = _clientService.GetAll();
        Clients.Clear();
        foreach (var c in list)
            Clients.Add(c);
    }

    public void Delete(int clientId)
    {
        _clientService.Delete(clientId);
        var existing = Clients.FirstOrDefault(c => c.ClientId == clientId);
        if (existing != null) Clients.Remove(existing);
    }
}

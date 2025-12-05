using AppointmentSystem.Business.Models;

namespace AppointmentSystem.Desktop.ViewModels;

public class DashboardViewModel : ViewModelBase
{
    public UserDto CurrentUser { get; }

    public DashboardViewModel(UserDto user)
    {
        CurrentUser = user;
    }
}

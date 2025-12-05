using AppointmentSystem.Business.Models;

namespace AppointmentSystem.Desktop.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public UserDto CurrentUser { get; }

    public MainWindowViewModel(UserDto user)
    {
        CurrentUser = user;
    }
}

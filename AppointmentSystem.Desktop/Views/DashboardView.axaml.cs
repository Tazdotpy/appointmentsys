using Avalonia.Controls;
using AppointmentSystem.Business.Models;
using AppointmentSystem.Desktop.ViewModels;

namespace AppointmentSystem.Desktop.Views;

public partial class DashboardView : UserControl
{
    public DashboardView()
    {
        InitializeComponent();
    }

    public DashboardView(UserDto user)
    {
        InitializeComponent();

        DataContext = new DashboardViewModel(user);
    }
}

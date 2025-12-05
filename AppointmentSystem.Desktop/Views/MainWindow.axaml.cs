using Avalonia.Controls;
using AppointmentSystem.Business.Models;

namespace AppointmentSystem.Desktop.Views;

public partial class MainWindow : Window
{
    private readonly UserDto? _currentUser;

    public MainWindow()
    {
        InitializeComponent();
    }

    public MainWindow(UserDto user)
    {
        InitializeComponent();
        _currentUser = user;

        // Load the dashboard by default
        ContentArea.Content = new DashboardView(_currentUser);

        DashboardBtn.Click      += (_, _) => ContentArea.Content = new DashboardView(_currentUser);
        ClientsBtn.Click        += (_, _) => ContentArea.Content = new ClientsView(_currentUser);
        ServicesBtn.Click       += (_, _) => ContentArea.Content = new ServicesView(_currentUser);
        AppointmentsBtn.Click   += (_, _) => ContentArea.Content = new AppointmentsView(_currentUser);
    }
}

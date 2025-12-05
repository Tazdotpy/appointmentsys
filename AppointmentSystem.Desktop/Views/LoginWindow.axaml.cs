using Avalonia.Controls;
using AppointmentSystem.Desktop.ViewModels;
using AppointmentSystem.Business.Models;

// NEW NAMESPACES
using AppointmentSystem.Business.Services.Implementations;
using AppointmentSystem.Data.Repositories.Implementations;
using AppointmentSystem.Data;

namespace AppointmentSystem.Desktop.Views;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();

        // Build the actual service
        var userService = new UserService(
            new UserRepository(new AppointmentsDbContext())
        );

        var vm = new LoginViewModel(userService);
        vm.OnLoginSuccess += Vm_OnLoginSuccess;

        DataContext = vm;

        // Try to give initial keyboard focus to the username box so user can type
        // Post to the UI thread to ensure focus is set after the window is shown
        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            try
            {
                var tb = this.FindControl<Avalonia.Controls.TextBox>("UsernameBox");
                tb?.Focus();
            }
            catch { }
        });

        // No debug preview wiring; rely on theme and normal focus behavior.
    }

    private void Vm_OnLoginSuccess(object? sender, UserDto user)
    {
        var main = new MainWindow(user);
        main.Show();
        Close();
    }

    private void OnLoginClicked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (DataContext is AppointmentSystem.Desktop.ViewModels.LoginViewModel vm)
        {
            // Password is bound to the VM (TextBox with PasswordChar), so just execute the login command.
            try
            {
                vm.LoginCommand?.Execute(null);
            }
            catch { }
        }
    }
}

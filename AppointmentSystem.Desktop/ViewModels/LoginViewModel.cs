using System;
using System.Threading.Tasks;
using System.Windows.Input;
using AppointmentSystem.Business.Models;
using AppointmentSystem.Business.Services.Interfaces;

namespace AppointmentSystem.Desktop.ViewModels;

public class LoginViewModel : ViewModelBase
{
    private readonly IUserService _userService;

    public LoginViewModel(IUserService userService)
    {
        _userService = userService;
        LoginCommand = new AsyncCommand(LoginAsync);
    }

    private string _username = "";
    public string Username
    {
        get => _username;
        set { _username = value; RaisePropertyChanged(); }
    }

    private string _password = "";
    public string Password
    {
        get => _password;
        set { _password = value; RaisePropertyChanged(); }
    }

    private string _errorMessage = "";
    public string ErrorMessage
    {
        get => _errorMessage;
        set { _errorMessage = value; RaisePropertyChanged(); }
    }

    public ICommand LoginCommand { get; }

    public event EventHandler<UserDto>? OnLoginSuccess;

    private async Task LoginAsync()
    {
        ErrorMessage = "";

        var user = await _userService.LoginAsync(Username, Password);

        if (user is null)
        {
            ErrorMessage = "Invalid username or password.";
            return;
        }

        OnLoginSuccess?.Invoke(this, user);
    }
}

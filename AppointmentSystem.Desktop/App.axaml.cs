using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

using AppointmentSystem.Desktop.Views;
using AppointmentSystem.Data;
using AppointmentSystem.Data.Repositories.Interfaces;
using AppointmentSystem.Data.Repositories.Implementations;
using AppointmentSystem.Business.Services.Interfaces;
using AppointmentSystem.Business.Services.Implementations;

using Microsoft.Extensions.DependencyInjection;

namespace AppointmentSystem.Desktop;

public partial class App : Application
{
    private ServiceProvider? _serviceProvider;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Build the DI container
        var services = new ServiceCollection();

        // DbContext
        services.AddSingleton<AppointmentsDbContext>();

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();

        // Business Services
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IServiceService, ServiceService>();
        services.AddScoped<IAppointmentService, AppointmentService>();

        _serviceProvider = services.BuildServiceProvider();
        // Ensure database exists and seed sample data if needed
        DatabaseInitializer.EnsureCreatedAndSeedAsync().GetAwaiter().GetResult();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Resolve IUserService from DI
            var userService = _serviceProvider.GetRequiredService<IUserService>();

            // Open LoginWindow with injected service
            desktop.MainWindow = new LoginWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }
}

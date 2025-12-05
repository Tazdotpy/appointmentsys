using System;
using Avalonia.Controls;
using AppointmentSystem.Business.Models;
using AppointmentSystem.Business.Services.Implementations;
using AppointmentSystem.Data;
using AppointmentSystem.Data.Repositories.Implementations;
using AppointmentSystem.Desktop.ViewModels;

namespace AppointmentSystem.Desktop.Views;

public partial class ServiceFormWindow : Window
{
    private readonly ServiceService _serviceService;
    private readonly ServiceDto? _editing;

    public ServiceFormWindow()
        : this(null)
    {
    }

    public ServiceFormWindow(ServiceDto? editing = null)
    {
        InitializeComponent();

        _editing = editing;

        var db = new AppointmentsDbContext();
        _serviceService = new ServiceService(new ServiceRepository(db));

        DataContext = new ServiceFormViewModel(editing)
        {
            CloseAction = Close
        };

        var nameBox = this.FindControl<TextBox>("NameBox");
        var durationBox = this.FindControl<TextBox>("DurationBox");
        var priceBox = this.FindControl<TextBox>("PriceBox");
        var errorText = this.FindControl<TextBlock>("ErrorText");
        var saveBtn = this.FindControl<Button>("SaveBtn");
        var cancelBtn = this.FindControl<Button>("CancelBtn");

        // Prefill if editing
        Console.WriteLine($"ServiceFormWindow ctor called. Editing passed: {(_editing is not null)}");
        if (_editing is not null)
        {
            Console.WriteLine($"ServiceFormWindow prefill values: Name='{_editing.Name}', Duration={_editing.DurationMinutes}, Price={_editing.Price}");
            if (nameBox != null)
            {
                nameBox.Text = _editing.Name;
                Console.WriteLine("ServiceFormWindow: nameBox set");
            }

            if (durationBox != null)
            {
                durationBox.Text = _editing.DurationMinutes.ToString();
                Console.WriteLine("ServiceFormWindow: durationBox set");
            }

            if (priceBox != null)
            {
                priceBox.Text = _editing.Price?.ToString("0.00") ?? "0.00";
                Console.WriteLine("ServiceFormWindow: priceBox set");
            }
        }

        if (cancelBtn != null)
            cancelBtn.Click += (_, __) => Close();

        if (saveBtn != null)
        {
            saveBtn.Click += (_, __) =>
            {
                if (errorText != null)
                    errorText.Text = string.Empty;

                try
                {
                    var name = nameBox?.Text?.Trim() ?? string.Empty;
                    if (string.IsNullOrWhiteSpace(name))
                        throw new Exception("Name is required.");

                    if (!int.TryParse(durationBox?.Text, out var duration) || duration <= 0)
                        throw new Exception("Duration must be a positive integer.");

                    if (!decimal.TryParse(priceBox?.Text, out var price) || price < 0)
                        throw new Exception("Price must be a valid non-negative number.");

                    var dto = new ServiceDto
                    {
                        ServiceId = _editing?.ServiceId ?? 0,
                        Name = name,
                        DurationMinutes = duration,
                        Price = price
                    };

                    if (dto.ServiceId == 0)
                        _serviceService.Create(dto);
                    else
                        _serviceService.Update(dto);

                    Close();
                }
                catch (Exception ex)
                {
                    if (errorText != null)
                        errorText.Text = ex.Message;
                }
            };
        }
    }
}

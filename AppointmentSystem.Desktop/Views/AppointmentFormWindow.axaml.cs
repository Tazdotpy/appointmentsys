using System;
using System.Linq;
using Avalonia.Controls;
using AppointmentSystem.Business.Models;
using AppointmentSystem.Business.Services.Implementations;
using AppointmentSystem.Data;
using AppointmentSystem.Data.Repositories.Implementations;
using AppointmentSystem.Desktop.ViewModels;

namespace AppointmentSystem.Desktop.Views;

public partial class AppointmentFormWindow : Window
{
    private readonly AppointmentService _appointmentService;
    private readonly ClientService _clientService;
    private readonly ServiceService _serviceService;
    private readonly AppointmentDto? _editing;

    public AppointmentFormWindow()
        : this(null)
    {
    }

    public AppointmentFormWindow(AppointmentDto? editing = null)
    {
        InitializeComponent();

        _editing = editing;

        // Create lightweight services for this window
        var db = new AppointmentsDbContext();
        _appointmentService = new AppointmentService(new AppointmentRepository(db), new ClientRepository(db), new ServiceRepository(db));
        _clientService = new ClientService(new ClientRepository(db));
        _serviceService = new ServiceService(new ServiceRepository(db));

        DataContext = new AppointmentFormViewModel(editing)
        {
            CloseAction = Close
        };

        // Populate client and service comboboxes
        var clients = _clientService.GetAll();
        var services = _serviceService.GetAll();

        var clientBoxControl = this.FindControl<ComboBox>("ClientBox"); ComboBox? clientBox = clientBoxControl;
        var serviceBoxControl = this.FindControl<ComboBox>("ServiceBox"); ComboBox? serviceBox = serviceBoxControl;
        var dateBoxControl = this.FindControl<DatePicker>("DateBox"); DatePicker? dateBox = dateBoxControl;
        var startBoxControl = this.FindControl<TextBox>("StartTimeBox"); TextBox? startBox = startBoxControl;
        var statusBoxControl = this.FindControl<ComboBox>("StatusBox"); ComboBox? statusBox = statusBoxControl;
        var notesBoxControl = this.FindControl<TextBox>("NotesBox"); TextBox? notesBox = notesBoxControl;
        var errorTextControl = this.FindControl<TextBlock>("ErrorText"); TextBlock? errorText = errorTextControl;

        // Set items via Items or ItemsSource depending on control implementation
        if (clientBox != null)
        {
            var clientItemsProp = clientBox.GetType().GetProperty("ItemsSource") ?? clientBox.GetType().GetProperty("Items");
            if (clientItemsProp != null)
                clientItemsProp.SetValue(clientBox, clients);
        }

        if (serviceBox != null)
        {
            var serviceItemsProp = serviceBox.GetType().GetProperty("ItemsSource") ?? serviceBox.GetType().GetProperty("Items");
            if (serviceItemsProp != null)
                serviceItemsProp.SetValue(serviceBox, services);
        }

        // If editing, prefill values
        if (_editing is not null)
        {
            // select client/service items by id
            if (clientBox != null)
                clientBox.SelectedItem = clients.FirstOrDefault(c => c.ClientId == _editing.ClientId);

            if (serviceBox != null)
                serviceBox.SelectedItem = services.FirstOrDefault(s => s.ServiceId == _editing.ServiceId);

            if (dateBox != null)
                dateBox.SelectedDate = new DateTimeOffset(_editing.StartTime);

            if (startBox != null)
                startBox.Text = _editing.StartTime.ToString("HH:mm");

            // select status
            if (statusBox != null)
            {
                foreach (var item in (statusBox.Items as System.Collections.IEnumerable) ?? Array.Empty<object>())
                {
                    if (item is ComboBoxItem cbi && cbi.Content?.ToString() == _editing.Status)
                    {
                        statusBox.SelectedItem = cbi;
                        break;
                    }
                }
            }

            if (notesBox != null)
                notesBox.Text = _editing.Notes;
        }

        // Wire buttons
        var saveBtnControl = this.FindControl<Button>("SaveBtn"); Button? saveBtn = saveBtnControl;
        var cancelBtnControl = this.FindControl<Button>("CancelBtn"); Button? cancelBtn = cancelBtnControl;

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
                    // read selections
                    if (!(clientBox?.SelectedItem is ClientDto client))
                        throw new Exception("Please select a client.");

                    if (!(serviceBox?.SelectedItem is ServiceDto service))
                        throw new Exception("Please select a service.");

                    if (dateBox?.SelectedDate is not DateTimeOffset dto)
                        throw new Exception("Please select a date.");

                    if (string.IsNullOrWhiteSpace(startBox?.Text))
                        throw new Exception("Please enter a start time (HH:mm).");

                    if (!TimeSpan.TryParse(startBox!.Text, out var time))
                        throw new Exception("Start time must be in HH:mm format.");

                    var start = dto.Date + time;

                    var dtoAppt = new AppointmentDto
                    {
                        AppointmentId = _editing?.AppointmentId ?? 0,
                        ClientId = client.ClientId,
                        ServiceId = service.ServiceId,
                        StartTime = start,
                        EndTime = start.AddMinutes(service.DurationMinutes),
                        Status = (statusBox?.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Scheduled",
                        Notes = notesBox?.Text ?? string.Empty
                    };

                    if (dtoAppt.AppointmentId == 0)
                    {
                        _appointmentService.Create(dtoAppt);
                            // Debug: log count after create
                            var all = _appointmentService.GetAll();
                            // Removed Console.WriteLine for debugging
                    }
                    else
                    {
                        _appointmentService.Update(dtoAppt);
                        // Debug: log count after update
                        var all = _appointmentService.GetAll();
                        // Removed Console.WriteLine for debugging
                    }

                    Close();
                }
                catch (Exception ex)
                {
                    // show message
                    if (errorText != null)
                        errorText.Text = ex.Message;
                }
            };
        }
    }
}

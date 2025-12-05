using System;
using Avalonia.Controls;
using AppointmentSystem.Business.Models;
using AppointmentSystem.Business.Services.Implementations;
using AppointmentSystem.Data;
using AppointmentSystem.Data.Repositories.Implementations;
using AppointmentSystem.Desktop.ViewModels;

namespace AppointmentSystem.Desktop.Views;

public partial class ClientFormWindow : Window
{
    private readonly ClientService _clientService;
    private readonly ClientDto? _editing;

    public ClientFormWindow()
        : this(null)
    {
    }

    public ClientFormWindow(ClientDto? editing = null)
    {
        InitializeComponent();

        _editing = editing;

        var db = new AppointmentsDbContext();
        _clientService = new ClientService(new AppointmentSystem.Data.Repositories.Implementations.ClientRepository(db));

        DataContext = new ClientFormViewModel(editing)
        {
            CloseAction = Close
        };

        var nameBox = this.FindControl<TextBox>("NameBox");
        var phoneBox = this.FindControl<TextBox>("PhoneBox");
        var emailBox = this.FindControl<TextBox>("EmailBox");
        var notesBox = this.FindControl<TextBox>("NotesBox");
        var errorText = this.FindControl<TextBlock>("ErrorText");
        var saveBtn = this.FindControl<Button>("SaveBtn");
        var cancelBtn = this.FindControl<Button>("CancelBtn");

        // Prefill if editing
        Console.WriteLine($"ClientFormWindow ctor called. Editing passed: {(_editing is not null)}");
        if (_editing is not null)
        {
            Console.WriteLine($"ClientFormWindow prefill values: Name='{_editing.Name}', Phone='{_editing.Phone}', Email='{_editing.Email}'");
            if (nameBox != null)
            {
                nameBox.Text = _editing.Name;
                Console.WriteLine("ClientFormWindow: nameBox set");
            }

            if (phoneBox != null)
            {
                phoneBox.Text = _editing.Phone;
                Console.WriteLine("ClientFormWindow: phoneBox set");
            }

            if (emailBox != null)
            {
                emailBox.Text = _editing.Email;
                Console.WriteLine("ClientFormWindow: emailBox set");
            }

            if (notesBox != null)
            {
                notesBox.Text = _editing.Notes;
                Console.WriteLine("ClientFormWindow: notesBox set");
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

                    var dto = new ClientDto
                    {
                        ClientId = _editing?.ClientId ?? 0,
                        Name = name,
                        Phone = phoneBox?.Text?.Trim() ?? string.Empty,
                        Email = emailBox?.Text?.Trim() ?? string.Empty,
                        Notes = notesBox?.Text ?? string.Empty
                    };

                    if (dto.ClientId == 0)
                        _clientService.Create(dto);
                    else
                        _clientService.Update(dto);

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

using System;
using System.Linq;
using Avalonia.Controls;
using AppointmentSystem.Business.Models;
using AppointmentSystem.Desktop.ViewModels;

namespace AppointmentSystem.Desktop.Views;

public partial class ClientsView : UserControl
{
    public ClientsView()
    {
        InitializeComponent();
    }

    public ClientsView(UserDto user)
    {
        InitializeComponent();
        var vm = new ClientsViewModel(user);
        DataContext = vm;

        var grid = this.FindControl<ListBox>("ClientsGrid");
        var refreshBtn = this.FindControl<Button>("RefreshBtn");
        var addBtn = this.FindControl<Button>("AddBtn");
        var editBtn = this.FindControl<Button>("EditBtn");
        var deleteBtn = this.FindControl<Button>("DeleteBtn");

        // initial load
        vm.LoadAll();

        // Refresh button
        if (refreshBtn != null)
        {
            refreshBtn.Click += (_, __) => vm.LoadAll();
        }

        // Double-click to edit
        if (grid != null)
        {
            grid.DoubleTapped += async (_, __) =>
            {
                var selected = grid.SelectedItem as ClientDto;
                if (selected != null)
                {
                    var wnd = new ClientFormWindow(selected);
                    var owner = this.VisualRoot as Window;
                    if (owner != null)
                        await wnd.ShowDialog(owner);
                    else
                        wnd.Show();

                    vm.LoadAll();
                }
            };
        }

        if (addBtn != null)
        {
            addBtn.Click += async (_, __) =>
            {
                var wnd = new ClientFormWindow(null);
                var owner = this.VisualRoot as Window;
                if (owner != null)
                    await wnd.ShowDialog(owner);
                else
                    wnd.Show();

                vm.LoadAll();
            };
        }

        if (editBtn != null)
        {
            editBtn.Click += async (_, __) =>
            {
                var selected = grid?.SelectedItem as ClientDto;
                Console.WriteLine($"ClientsView.Edit clicked. Selected != null: {selected != null}");
                if (selected == null)
                {
                    // fallback: pick first item if none selected
                    try
                    {
                        var first = vm.Clients.FirstOrDefault();
                        if (first != null)
                        {
                            selected = first;
                            try { grid.SelectedItem = selected; } catch {}
                            Console.WriteLine("ClientsView.Edit: no selection; falling back to first item.");
                        }
                    }
                    catch { }
                }

                if (selected != null)
                {
                    Console.WriteLine($"ClientsView: editing client id={selected.ClientId} name='{selected.Name}'");
                    var wnd = new ClientFormWindow(selected);
                    var owner = this.VisualRoot as Window;
                    if (owner != null)
                        await wnd.ShowDialog(owner);
                    else
                        wnd.Show();

                    vm.LoadAll();
                }
            };
        }

        if (deleteBtn != null)
        {
            deleteBtn.Click += (_, __) =>
            {
                var selected = grid?.SelectedItem as ClientDto;
                Console.WriteLine($"ClientsView.Delete clicked. Selected != null: {selected != null}");
                if (selected != null)
                {
                    Console.WriteLine($"ClientsView: deleting client id={selected.ClientId} name='{selected.Name}'");
                    vm.Delete(selected.ClientId);
                }
            };
        }
    }
}

using System;
using System.Linq;
using Avalonia.Controls;
using AppointmentSystem.Business.Models;
using AppointmentSystem.Desktop.ViewModels;

namespace AppointmentSystem.Desktop.Views;

public partial class ServicesView : UserControl
{
    public ServicesView()
    {
        InitializeComponent();
    }

    public ServicesView(UserDto user)
    {
        InitializeComponent();
        var vm = new ServicesViewModel(user);
        DataContext = vm;

        var grid = this.FindControl<ListBox>("ServicesGrid");
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
                var selected = grid.SelectedItem as ServiceDto;
                if (selected != null)
                {
                    var wnd = new ServiceFormWindow(selected);
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
                var wnd = new ServiceFormWindow(null);
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
                var selected = grid?.SelectedItem as ServiceDto;
                Console.WriteLine($"ServicesView.Edit clicked. Selected != null: {selected != null}");
                if (selected == null)
                {
                    // fallback: pick first item if none selected
                    try
                    {
                        var first = vm.Services.FirstOrDefault();
                        if (first != null)
                        {
                            selected = first;
                            try { grid.SelectedItem = selected; } catch {}
                            Console.WriteLine("ServicesView.Edit: no selection; falling back to first item.");
                        }
                    }
                    catch { }
                }

                if (selected != null)
                {
                    Console.WriteLine($"ServicesView: editing service id={selected.ServiceId} name='{selected.Name}'");
                    var wnd = new ServiceFormWindow(selected);
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
                var selected = grid?.SelectedItem as ServiceDto;
                Console.WriteLine($"ServicesView.Delete clicked. Selected != null: {selected != null}");
                if (selected != null)
                {
                    Console.WriteLine($"ServicesView: deleting service id={selected.ServiceId} name='{selected.Name}'");
                    vm.Delete(selected.ServiceId);
                }
            };
        }
    }
}

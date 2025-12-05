using Avalonia.Controls;
using System;
using System.Linq;
using AppointmentSystem.Business.Models;
using AppointmentSystem.Desktop.ViewModels;

namespace AppointmentSystem.Desktop.Views;

public partial class AppointmentsView : UserControl
{
    private readonly AppointmentsViewModel? _vm;

    public AppointmentsView()
    {
        InitializeComponent();
    }

    public AppointmentsView(UserDto user)
    {
        InitializeComponent();

        _vm = new AppointmentsViewModel(user);
        DataContext = _vm;

        // Wire UI actions
        var addBtnControl = this.FindControl<Button>("AddBtn"); Button? addBtn = addBtnControl;
        var deleteBtnControl = this.FindControl<Button>("DeleteBtn"); Button? deleteBtn = deleteBtnControl;
        var editBtnControl = this.FindControl<Button>("EditBtn"); Button? editBtn = editBtnControl;
        var refreshBtnControl = this.FindControl<Button>("RefreshBtn"); Button? refreshBtn = refreshBtnControl;
        var datePickerControl = this.FindControl<DatePicker>("DatePicker"); DatePicker? datePicker = datePickerControl;
        var listControl = this.FindControl<ListBox>("AppointmentsList"); ListBox? list = listControl;

        if (addBtn != null)
        {
            addBtn.Click += async (_, __) =>
            {
                var wnd = new AppointmentFormWindow(null);
                var owner = this.VisualRoot as Window;
                if (owner != null)
                    await wnd.ShowDialog(owner);
                else
                    wnd.Show();

                // Refresh current view from ViewModel
                _vm?.LoadAll();
            };
        }

        if (refreshBtn != null)
        {
            refreshBtn.Click += (_, __) =>
            {
                if (datePicker != null && datePicker.SelectedDate.HasValue)
                {
                    _vm?.LoadByDate(datePicker.SelectedDate.Value.Date);
                }
                else
                {
                    _vm?.LoadAll();
                }
            };
        }

        if (deleteBtn != null)
        {
            deleteBtn.Click += (_, __) =>
            {
                var selected = list?.SelectedItem as AppointmentDto;
                if (selected != null)
                {
                    _vm?.Delete(selected.AppointmentId);
                }
            };
        }

        if (editBtn != null)
        {
            editBtn.Click += async (_, __) =>
            {
                var selected = list?.SelectedItem as AppointmentDto;
                if (selected != null)
                {
                    var wnd = new AppointmentFormWindow(selected);
                    var owner = this.VisualRoot as Window;
                    if (owner != null)
                        await wnd.ShowDialog(owner);
                    else
                        wnd.Show();

                    _vm?.LoadAll();
                }
            };
        }

        if (datePicker != null)
        {
            datePicker.SelectedDateChanged += (_, __) =>
            {
                var selected = datePicker.SelectedDate;
                if (selected.HasValue)
                {
                    _vm?.LoadByDate(selected.Value.Date);
                }
                else
                {
                    _vm?.LoadAll();
                }
            };
        }

        // Initial load
        _vm?.LoadAll();

        // When the visual tree is ready we can attach lightweight interactions
        this.Loaded += (_, __) =>
        {
            // Attach double-tap to open editor for convenience
            if (list != null)
            {
                list.DoubleTapped += async (_, __) =>
                {
                    var selected = list.SelectedItem as AppointmentDto;
                    if (selected != null)
                    {
                        var wnd = new AppointmentFormWindow(selected);
                        var owner = this.VisualRoot as Window;
                        if (owner != null)
                            await wnd.ShowDialog(owner);
                        else
                            wnd.Show();

                        _vm?.LoadAll();
                    }
                };
            }
        };
    }
}

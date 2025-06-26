using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamerCompanion.Models;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace GamerCompanion.ViewModels;

public partial class ReminderViewModel : ObservableObject {
    public ObservableCollection<Reminder> Reminders { get; } = new();

    private DispatcherTimer _reminderTimer;

    [ObservableProperty]
    private string newReminderText = "";
    [ObservableProperty]
    private int newReminderTimeMinutes = 10;
    [ObservableProperty]
    private bool newReminderRepeating = false;

    public ReminderViewModel() {
        _reminderTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(30) };
        _reminderTimer.Tick += (s, e) => UpdateReminderTimes();
        _reminderTimer.Start();
    }

    private void UpdateReminderTimes() {
        foreach (var reminder in Reminders)
            reminder.RaiseTimeChanged();
    }

    [RelayCommand]
    private void AddReminder() {
        if (string.IsNullOrWhiteSpace(NewReminderText) || NewReminderTimeMinutes <= 0)
            return;

        var newReminder = new Reminder { Text = NewReminderText, TargetTime = DateTime.Now + TimeSpan.FromMinutes(NewReminderTimeMinutes), Repeating = NewReminderRepeating };
        Reminders.Add(newReminder);

        NewReminderText = "";
        NewReminderTimeMinutes = 10;
        NewReminderRepeating = false;
    }

    [RelayCommand]
    private void RemoveReminder(Reminder reminder) {
        if (reminder != null)
            Reminders.Remove(reminder);
    }
}

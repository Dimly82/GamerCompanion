using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamerCompanion.Models;
using GamerCompanion.Views;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace GamerCompanion.ViewModels;

public partial class ReminderViewModel : ObservableObject {
    public ObservableCollection<Reminder> Reminders { get; } = new();

    private DispatcherTimer _reminderTimer;

    [ObservableProperty]
    private string newReminderText = "Reminder text";
    [ObservableProperty]
    private int newReminderTimeMinutes = 10;
    [ObservableProperty]
    private bool newReminderRepeating = false;

    public ReminderViewModel() {
        _reminderTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _reminderTimer.Tick += (s, e) => CheckReminders();
        _reminderTimer.Start();
    }

    private void CheckReminders() {
        foreach (var reminder in Reminders) {
            reminder.RaiseTimeChanged();
        }

        var triggered = Reminders.Where(r => r.TargetTime <= DateTime.Now && !r.IsPending).ToList();
        foreach (var reminder in triggered) {
            var toast = new ToastWindow($"Reminder: {reminder.Text}");
            reminder.IsPending = true;

            toast.RemindLater += () => {
                reminder.IsPending = false;
                reminder.TargetTime = DateTime.Now + TimeSpan.FromMinutes(5);
            };
            toast.Dismissed += () => {
                reminder.IsPending = false;
                if (!reminder.Repeating)
                    Reminders.Remove(reminder);
                else
                    reminder.TargetTime = DateTime.Now + TimeSpan.FromMinutes(reminder.RepeatIntervalMinutes);
            };
            toast.Show();
        }
    }

    [RelayCommand]
    private void AddReminder() {
        if (string.IsNullOrWhiteSpace(NewReminderText) || NewReminderTimeMinutes <= 0)
            return;

        var newReminder = new Reminder { Text = NewReminderText, TargetTime = DateTime.Now + TimeSpan.FromMinutes(NewReminderTimeMinutes), Repeating = NewReminderRepeating };
        Reminders.Add(newReminder);

        NewReminderText = "Reminder text";
        NewReminderTimeMinutes = 10;
        NewReminderRepeating = false;
    }

    [RelayCommand]
    private void RemoveReminder(Reminder reminder) {
        if (reminder != null)
            Reminders.Remove(reminder);
    }
}

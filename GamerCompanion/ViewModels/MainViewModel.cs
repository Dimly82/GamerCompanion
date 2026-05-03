using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamerCompanion.Services;
using GamerCompanion.Views;

namespace GamerCompanion.ViewModels;

public partial class MainViewModel : ObservableObject {
    public MonitoringViewModel Monitoring { get; }
    public ReminderViewModel Reminders { get; }
    public SettingsService Settings { get; } = new SettingsService("settings.json");

    public MainViewModel() {
        Monitoring = new MonitoringViewModel(Settings);
        Reminders = new ReminderViewModel();
    }

    [RelayCommand]
    private void OpenSettings() {
        new SettingsWindow(Settings, Monitoring).ShowDialog();
    }
}

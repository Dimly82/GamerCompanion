using CommunityToolkit.Mvvm.ComponentModel;

namespace GamerCompanion.ViewModels;

public partial class MainViewModel : ObservableObject {
    public MonitoringViewModel Monitoring { get; }
    public ReminderViewModel Reminders { get; }

    public MainViewModel() {
        Monitoring = new MonitoringViewModel();
        Reminders = new ReminderViewModel();
    }
}

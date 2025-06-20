using CommunityToolkit.Mvvm.ComponentModel;
using GamerCompanion.Services;

namespace GamerCompanion.ViewModels;

public partial class MonitoringViewModel : ObservableObject {
    private readonly SystemMonitoringService _monitoringService;
    private readonly System.Timers.Timer _timer;

    [ObservableProperty]
    private float cpuLoad;

    [ObservableProperty]
    private float ramUsed;

    [ObservableProperty]
    private float gpuLoad;

    public MonitoringViewModel() {
        _monitoringService = new SystemMonitoringService();

        _timer = new System.Timers.Timer(1000);
        _timer.Elapsed += (s, e) => Refresh();
        _timer.Start();
    }

    private void Refresh() {
        _monitoringService.Update();
        CpuLoad = _monitoringService.CpuLoad;
        RamUsed = _monitoringService.RamUsed;
        GpuLoad = _monitoringService.GpuLoad;
    }
}


using CommunityToolkit.Mvvm.ComponentModel;
using GamerCompanion.Helpers;
using GamerCompanion.Models;
using GamerCompanion.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace GamerCompanion.ViewModels;

public partial class MonitoringViewModel : ObservableObject {
    private readonly SystemMonitoringService _monitoringService;
    private readonly System.Timers.Timer _timer;

    [ObservableProperty]
    private int updateInterval = 1000;
    [ObservableProperty]
    private int[] availableIntervals = { 500, 1000, 1500, 2000 };

    [ObservableProperty]
    private float cpuLoad;
    [ObservableProperty]
    private float cpuTemp;
    [ObservableProperty]
    private string cpuName;

    [ObservableProperty]
    private float ramUsed;
    [ObservableProperty]
    private float ramTotal;
    public string RamStatus => $"{RamUsed:F1}/{RamTotal:F1} GB";

    [ObservableProperty]
    private float gpuLoad;
    [ObservableProperty]
    private float gpuTemp;
    [ObservableProperty]
    private float gpuMemUsed;
    [ObservableProperty]
    private float gpuMemTotal;
    public string GpuMemStatus => $"{GpuMemUsed}/{GpuMemTotal} MB";
    [ObservableProperty]
    private string gpuName;

    public ObservableCollection<GameInfo> KnownGames { get; } = new() { new GameInfo { Name = "Death Stranding", ProcessName = "DeathStranding" } };

    [ObservableProperty]
    private string? activeGame;

    public MonitoringViewModel() {
        _monitoringService = new SystemMonitoringService();

        _timer = new System.Timers.Timer(1000);
        _timer.Elapsed += (s, e) => Refresh();
        _timer.Start();
    }

    partial void OnUpdateIntervalChanged(int value) {
        if (_timer != null)
            _timer.Interval = value;
    }

    private void Refresh() {
        _monitoringService.Update();
        CpuLoad = _monitoringService.CpuLoad;
        CpuTemp = _monitoringService.CpuTemp;
        CpuName = _monitoringService.CpuName;

        RamUsed = _monitoringService.RamUsed;
        RamTotal = _monitoringService.RamTotal;

        GpuLoad = _monitoringService.GpuLoad;
        GpuTemp = _monitoringService.GpuTemp;
        GpuMemUsed = _monitoringService.GpuMemUsed;
        GpuMemTotal = _monitoringService.GpuMemTotal;
        GpuName = _monitoringService.GpuName;

        OnPropertyChanged(nameof(RamStatus));
        OnPropertyChanged(nameof(GpuMemStatus));

        //string processName = ActiveWindowHelper.GetActiveProcessName();
        //ActiveGame = processName;

        var runningProcesses = Process.GetProcesses().Select(p => p.ProcessName.ToLower()).ToHashSet();
        foreach (var game in KnownGames)
            game.IsRunning = runningProcesses.Contains(game.ProcessName.ToLower());

        ActiveGame = KnownGames.FirstOrDefault(g => g.IsRunning)?.Name ?? "None";
    }
}


using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamerCompanion.Helpers;
using GamerCompanion.Models;
using GamerCompanion.Services;
using GamerCompanion.Views;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace GamerCompanion.ViewModels;

public partial class MonitoringViewModel : ObservableObject {
    private readonly SystemMonitoringService _monitoringService;
    private readonly LoggerService _logger;
    private readonly System.Timers.Timer _timer;
    private AppRegisterService _appRegisterService;

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

    //public ObservableCollection<GameInfo> KnownGames { get; } = new() {
    //    new GameInfo { Name = "Death Stranding", ProcessName = "DeathStranding" },
    //    new GameInfo { Name = "Windows Terminal", ProcessName = "WindowsTerminal"}
    //};

    [ObservableProperty]
    private string? activeApp;
    [ObservableProperty]
    private bool isLogging;

    private const int maxPoints = 60;
    public ObservableCollection<double> CpuLoadHistory { get; } = new();
    public ISeries[] CpuLoadSeries { get; }
    public ObservableCollection<double> GpuLoadHistory { get; } = new();
    public ISeries[] GpuLoadSeries { get; }

    public MonitoringViewModel() {
        _monitoringService = new SystemMonitoringService();
        _logger = new LoggerService("C:\\Users\\pdimo\\source\\repos\\GamerCompanion\\Logs");

        CpuLoadSeries = new ISeries[] {
            new LineSeries<double> {
                Values = CpuLoadHistory,
                Fill = null,
                GeometrySize = 0
            }
        };
        GpuLoadSeries = new ISeries[] {
            new LineSeries<double> {
                Values = GpuLoadHistory,
                Fill = null,
                GeometrySize = 0
            }
        };

        _timer = new System.Timers.Timer(1000);
        _timer.Elapsed += (s, e) => Refresh();
        _timer.Start();

        _appRegisterService = new AppRegisterService(
            "C:\\Users\\pdimo\\source\\repos\\GamerCompanion\\AppRegistry.json");
    }

    partial void OnUpdateIntervalChanged(int value) {
        if (_timer != null)
            _timer.Interval = value;
    }

    private void LogCurrentData() {
        if (_logger == null)
            return;

        var timestamp = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        string line = $"{timestamp},{CpuLoad:F1},{CpuTemp},{RamUsed:F1},{RamTotal:F1},{GpuLoad},{GpuTemp},{GpuMemUsed},{GpuMemTotal},{ActiveApp}";
        _logger.Log(line);
    }

    [RelayCommand]
    private void ToggleLogging() {
        IsLogging = !IsLogging;
    }

    [RelayCommand]
    private void ShowRegisteredGames() {
        var window = new RegisteredGamesWindow(_appRegisterService);
        window.ShowDialog();
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

        if (CpuLoadHistory.Count >= maxPoints)
            CpuLoadHistory.RemoveAt(0);
        CpuLoadHistory.Add(CpuLoad);
        if (GpuLoadHistory.Count >= maxPoints)
            GpuLoadHistory.RemoveAt(0);
        GpuLoadHistory.Add(GpuLoad);

        //string processName = ActiveWindowHelper.GetActiveProcessName();
        //ActiveGame = processName;

        var runningProcesses = Process.GetProcesses().Select(p => p.ProcessName.ToLower()).ToHashSet();
        foreach (var game in _appRegisterService.Apps)
            game.IsRunning = runningProcesses.Contains(game.ProcessName.ToLower());

        ActiveApp = _appRegisterService.GetActiveApp(runningProcesses);

        if (IsLogging)
            LogCurrentData();
    }
}


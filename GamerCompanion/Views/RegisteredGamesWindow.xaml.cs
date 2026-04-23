using GamerCompanion.Models;
using GamerCompanion.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;

namespace GamerCompanion.Views;

public partial class RegisteredGamesWindow : Window {
    private AppRegisterService _appRegisterService;

    public RegisteredGamesWindow(AppRegisterService appRegisterService) {
        InitializeComponent();

        _appRegisterService = appRegisterService;
        DataContext = _appRegisterService;

        ProcessComboBox.ItemsSource = Process.GetProcesses()
            .Select(p => p.ProcessName)
            .Distinct()
            .OrderBy(p => p)
            .ToList();
    }

    private void Add_Click(object sender, RoutedEventArgs e) {
        if (string.IsNullOrWhiteSpace(AppNameBox.Text) || ProcessComboBox.SelectedItem == null)
            return;

        _appRegisterService.Apps.Add(new AppInfo {
            Name = AppNameBox.Text,
            ProcessName = ProcessComboBox.SelectedItem.ToString()
        });
        _appRegisterService.Save();

        AppNameBox.Text = "";
        ProcessComboBox.SelectedItem = null;
    }

    private void Remove_Click(object sender, RoutedEventArgs e) {
        if (AppsList.SelectedItem is AppInfo app) {
            _appRegisterService.Apps.Remove(app);
            _appRegisterService.Save();
        }
    }
}

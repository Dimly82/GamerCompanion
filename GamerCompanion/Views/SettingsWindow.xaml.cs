using GamerCompanion.Services;
using GamerCompanion.ViewModels;
using ModernWpf;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GamerCompanion.Views
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window {
        private readonly SettingsService _settingsService;
        private readonly MonitoringViewModel _monitoringViewModel;

        public SettingsWindow(SettingsService settingsService, MonitoringViewModel monitoringViewModel) {
            InitializeComponent();
            _settingsService = settingsService;
            _monitoringViewModel = monitoringViewModel;

            var s = _settingsService.Settings;
            LogFolderBox.Text = s.LogFolder;
            IntervalCombo.ItemsSource = new[] { 500, 1000, 1500, 2000 };
            IntervalCombo.SelectedItem = s.UpdateInterval;
            ThemeCombo.SelectedIndex = s.Theme switch {
                "Dark" => 0,
                "Light" => 1,
                _ => 2
            };
            CpuTempBox.Text = s.CpuTempThreshold.ToString();
            GpuTempBox.Text = s.GpuTempThreshold.ToString();
            CpuLoadBox.Text = s.CpuLoadThreshold.ToString();
            GpuLoadBox.Text = s.GpuLoadThreshold.ToString();
            RamBox.Text = s.RamUsageThreshold.ToString();
            AutoLoggingCheck.IsChecked = s.AutoLogging;
        }

        private void BrowseFolder_Click(object sender, RoutedEventArgs e) {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                LogFolderBox.Text = dialog.SelectedPath;
        }

        private void Save_Click(object sender, RoutedEventArgs e) {
            var s = _settingsService.Settings;
            s.LogFolder = LogFolderBox.Text;
            s.UpdateInterval = (int)IntervalCombo.SelectedItem;
            s.Theme = ((ComboBoxItem)ThemeCombo.SelectedItem).Content.ToString();
            s.CpuTempThreshold = float.Parse(CpuTempBox.Text);
            s.GpuTempThreshold = float.Parse(GpuTempBox.Text);
            s.CpuLoadThreshold = float.Parse(CpuLoadBox.Text);
            s.GpuLoadThreshold = float.Parse(GpuLoadBox.Text);
            s.RamUsageThreshold = float.Parse(RamBox.Text);
            s.AutoLogging = AutoLoggingCheck.IsChecked == true;

            ThemeManager.Current.ApplicationTheme = s.Theme switch {
                "Dark" => ApplicationTheme.Dark,
                "Light" => ApplicationTheme.Light,
                _ => null
            };

            _settingsService.Save();
            _monitoringViewModel.ApplySettings();
            Close();
        }
    }
}

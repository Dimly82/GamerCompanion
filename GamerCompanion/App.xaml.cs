using GamerCompanion.Services;
using ModernWpf;
using System.Diagnostics;
using System.Windows;

namespace GamerCompanion;

public partial class App : System.Windows.Application {
    private NotifyIcon _trayIcon;

    protected override void OnStartup(StartupEventArgs e) {
        base.OnStartup(e);

        //ModernWpf.ThemeManager.Current.ApplicationTheme = ModernWpf.ApplicationTheme.Light;

        var settings = new SettingsService("settings.json");
        ThemeManager.Current.ApplicationTheme = settings.Settings.Theme switch {
            "Dark" => ApplicationTheme.Dark,
            "Light" => ApplicationTheme.Light,
            _ => null
        };

        if (settings.Settings.Theme == "System")
            ThemeManager.Current.ApplicationTheme = null;

        _trayIcon = new NotifyIcon {
            Icon = new Icon("Resources/tray_icon.ico"),
            Visible = true,
            Text = "Gamer Companion",
            ContextMenuStrip = new ContextMenuStrip()
        };

        _trayIcon.ContextMenuStrip.Items.Add("Open", null,
            (s, args) => {
                Current.MainWindow.Show();
                Current.MainWindow.WindowState = WindowState.Normal;
            });

        _trayIcon.ContextMenuStrip.Items.Add("Exit", null,
            (s, args) => {
                _trayIcon.Visible = false;
                Current.Shutdown();
            });

        _trayIcon.Click += (s, args) => {
            Current.MainWindow.Show();
            Current.MainWindow.WindowState = WindowState.Normal;
        };
    }
}
using GamerCompanion.ViewModels;
using System.ComponentModel;
using System.Windows;

namespace GamerCompanion.Views;

public partial class MainWindow : Window {
    public MainWindow() {
        InitializeComponent();
    }

    protected override void OnClosing(CancelEventArgs e) {
        e.Cancel = true;
        Hide();
    }
}
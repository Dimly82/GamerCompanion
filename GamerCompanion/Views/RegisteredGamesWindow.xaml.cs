using GamerCompanion.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace GamerCompanion.Views;

public partial class RegisteredGamesWindow : Window {
    public ObservableCollection<GameInfo> Games { get; set; }

    public RegisteredGamesWindow(ObservableCollection<GameInfo> games) { 
        InitializeComponent();

        Games = games;
        DataContext = this;
    }

    private void Close_Click(object s, RoutedEventArgs e) {
        Close();
    }
}

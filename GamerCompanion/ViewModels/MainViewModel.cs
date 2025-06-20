using CommunityToolkit.Mvvm.ComponentModel;

namespace GamerCompanion.ViewModels;

public partial class MainViewModel : ObservableObject {
    [ObservableProperty]
    private string title = "Gamer Companion";
}

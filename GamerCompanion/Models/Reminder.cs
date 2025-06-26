using System.ComponentModel;

namespace GamerCompanion.Models;

public class Reminder : INotifyPropertyChanged {
    public string Text { get; set; }

    public DateTime TargetTime { get; set; }

    public int TimeMinutes => Math.Max(0, (int)(TargetTime - DateTime.Now).TotalMinutes);

    public bool Repeating { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

    public void RaiseTimeChanged() {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TimeMinutes)));
    }
}

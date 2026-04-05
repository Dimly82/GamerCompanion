using System.ComponentModel;

namespace GamerCompanion.Models;

public class Reminder : INotifyPropertyChanged {
    public string Text { get; set; }

    public bool IsPending { get; set; }

    private DateTime _targetTime;
    public DateTime TargetTime {
        get => _targetTime; set {
            _targetTime = value;
            RaiseTimeChanged();
        }
    }

    public int RepeatIntervalMinutes { get; set; }

    public string TimeLeft {
        get {
            var diff = TargetTime - DateTime.Now;
            if(IsPending)
                return "Pending...";
            if (diff.TotalSeconds <= 0)
                return "Now";
            if (diff.TotalMinutes < 1)
                return $"{(int)diff.TotalSeconds} sec";
            return $"{(int)diff.TotalMinutes} min";
        }
    }

    public bool Repeating { get; set; }

    public string TargetTimeFormatted => TargetTime.ToString("HH:mm:ss");

    public event PropertyChangedEventHandler PropertyChanged;

    public void RaiseTimeChanged() {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TimeLeft)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TargetTimeFormatted)));
    }
}

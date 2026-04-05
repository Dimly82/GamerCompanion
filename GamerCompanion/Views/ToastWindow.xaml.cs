using System.Windows;
using System.Windows.Threading;

namespace GamerCompanion.Views {
    /// <summary>
    /// Interaction logic for ToastWindow.xaml
    /// </summary>
    public partial class ToastWindow : Window {
        public event Action Dismissed;

        public event Action RemindLater;

        private void Dismiss_Click(object sender, RoutedEventArgs e) {
            Dismissed?.Invoke();
            Close();
        }

        private void RemindLater_Click(object sender, RoutedEventArgs e) {
            RemindLater?.Invoke();
            Close();
        }

        public ToastWindow(string message) {
            InitializeComponent();
            ToastText.Text = message;
            System.Media.SystemSounds.Exclamation.Play();

            Loaded += (s, e) => {
                var screen = SystemParameters.WorkArea;
                Left = screen.Right - screen.Width / 2 - Width / 2;
                Top = 10;
            };
        }
    }
}

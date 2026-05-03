using GamerCompanion.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace GamerCompanion.Services {
    public class AppRegisterService {
        private readonly string _filePath;

        public ObservableCollection<AppInfo> Apps { get; private set; } = new();

        public AppRegisterService(string filePath) {
            _filePath = filePath;
            Load();
        }

        public string? GetActiveApp(IEnumerable<string> runningProcesses) {
            var activeApps = Apps.Where((g) => runningProcesses.Contains(g.ProcessName.ToLower()));
            if (activeApps.Any())
                return activeApps.First().Name;
            return null;
        }

        private void Load() {
            if (File.Exists(_filePath)) {
                var json = File.ReadAllText(_filePath);
                Apps = JsonSerializer.Deserialize<ObservableCollection<AppInfo>>(json);
            }
        }

        public void Save() {
            File.WriteAllText(_filePath, JsonSerializer.Serialize(Apps));
        }
    }
}

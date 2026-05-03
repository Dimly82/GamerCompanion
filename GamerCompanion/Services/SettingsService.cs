using GamerCompanion.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace GamerCompanion.Services {
    public class SettingsService {
        private readonly string _filePath;
        public AppSettings Settings { get; private set; } = new();

        public SettingsService(string filePath) {
            _filePath = filePath;
            Load();
        }

        private void Load() {
            if (File.Exists(_filePath)) {
                var json = File.ReadAllText(_filePath);
                Settings = JsonSerializer.Deserialize<AppSettings>(json) ?? new();
            }
        }

        public void Save() {
            File.WriteAllText(_filePath, JsonSerializer.Serialize(Settings));
        }
    }
}

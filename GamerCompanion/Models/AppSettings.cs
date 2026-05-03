using System;
using System.Collections.Generic;
using System.Text;

namespace GamerCompanion.Models {
    public class AppSettings {
        public string LogFolder { get; set; } = "Logs";
        public float CpuTempThreshold { get; set; } = 85f;
        public float GpuTempThreshold { get; set; } = 85f;
        public float CpuLoadThreshold { get; set; } = 90f;
        public float GpuLoadThreshold { get; set; } = 90f;
        public float RamUsageThreshold { get; set; } = 90f;
        public int UpdateInterval { get; set; } = 1000;
        public string Theme { get; set; } = "Dark";
        public bool AutoLogging { get; set; } = false;
    }
}

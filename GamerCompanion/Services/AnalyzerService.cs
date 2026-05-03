using GamerCompanion.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GamerCompanion.Services {
    public class AnalyzerService {
        public float CpuTempThreshold { get; set; } = 85f;
        public float GpuTempThreshold { get; set; } = 85f;
        public float RamUsageThreshold { get; set; } = 90f;
        public float CpuLoadThreshold { get; set; } = 90f;
        public float GpuLoadThreshold { get; set; } = 90f;

        public List<Recommendation> Analyze(float cpuTemp, float cpuLoad, float gpuTemp, float gpuLoad, float ramUsed, float ramTotal) {
            var recommendations = new List<Recommendation>();

            if (cpuTemp >= CpuTempThreshold)
                recommendations.Add(new Recommendation {
                    Message = $"High CPU temperature: {cpuTemp}°C. Check your cooling system.",
                    Severity = RecommendationSeverity.Critical
                });

            if (cpuLoad >= CpuLoadThreshold)
                recommendations.Add(new Recommendation {
                    Message = $"High CPU load: {cpuLoad:F1}%. Consider closing unused processes.",
                    Severity = RecommendationSeverity.Warning
                });

            if (gpuTemp >= GpuTempThreshold)
                recommendations.Add(new Recommendation {
                    Message = $"High GPU temperature: {gpuTemp}°C. Check your ventilation.",
                    Severity = RecommendationSeverity.Critical
                });

            if (gpuLoad >= GpuLoadThreshold)
                recommendations.Add(new Recommendation {
                    Message = $"High GPU load: {gpuLoad:F1}%.",
                    Severity = RecommendationSeverity.Warning
                });

            var ramPercent = ramTotal > 0 ? (ramUsed / ramTotal) * 100f : 0f;
            if (ramPercent >= RamUsageThreshold)
                recommendations.Add(new Recommendation {
                    Message = $"High RAM usage: {ramPercent:F1}%. Consider freeing up memory.",
                    Severity = RecommendationSeverity.Warning
                });

            return recommendations;
        }

        public AnalysisReport AnalyzeHistory(string logFolder, DateTime from, DateTime to) {
            var report = new AnalysisReport();
            var files = Directory.GetFiles(logFolder, "log_*.csv")
                .Where(f => {
                    var date = DateTime.ParseExact(
                        Path.GetFileNameWithoutExtension(f).Replace("log_", ""),
                        "yyyy-MM-dd", null);
                    return date >= from.Date && date <= to.Date;
                });

            var rows = new List<LogRow>();
            foreach (var file in files) {
                var lines = File.ReadAllLines(file).Skip(1); // пропускаем заголовок
                foreach (var line in lines) {
                    var parts = line.Split(',');
                    if (parts.Length < 9) continue;
                    rows.Add(new LogRow {
                        CpuLoad = float.Parse(parts[1]),
                        CpuTemp = float.Parse(parts[2]),
                        RamUsed = float.Parse(parts[3]),
                        RamTotal = float.Parse(parts[4]),
                        GpuLoad = float.Parse(parts[5]),
                        GpuTemp = float.Parse(parts[6])
                    });
                }
            }

            if (rows.Count == 0) return report;

            report.AvgCpuLoad = rows.Average(r => r.CpuLoad);
            report.MaxCpuLoad = rows.Max(r => r.CpuLoad);
            report.AvgCpuTemp = rows.Average(r => r.CpuTemp);
            report.MaxCpuTemp = rows.Max(r => r.CpuTemp);
            report.AvgGpuLoad = rows.Average(r => r.GpuLoad);
            report.MaxGpuLoad = rows.Max(r => r.GpuLoad);
            report.AvgGpuTemp = rows.Average(r => r.GpuTemp);
            report.MaxGpuTemp = rows.Max(r => r.GpuTemp);
            report.AvgRamUsed = rows.Average(r => r.RamUsed);
            report.MaxRamUsed = rows.Max(r => r.RamUsed);

            return report;
        }
    }

    public class LogRow {
        public float CpuLoad { get; set; }
        public float CpuTemp { get; set; }
        public float RamUsed { get; set; }
        public float RamTotal { get; set; }
        public float GpuLoad { get; set; }
        public float GpuTemp { get; set; }
    }

    public class AnalysisReport {
        public float AvgCpuLoad { get; set; }
        public float MaxCpuLoad { get; set; }
        public float AvgCpuTemp { get; set; }
        public float MaxCpuTemp { get; set; }
        public float AvgGpuLoad { get; set; }
        public float MaxGpuLoad { get; set; }
        public float AvgGpuTemp { get; set; }
        public float MaxGpuTemp { get; set; }
        public float AvgRamUsed { get; set; }
        public float MaxRamUsed { get; set; }
    }
}

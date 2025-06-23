using System.IO;
using System.Text;

namespace GamerCompanion.Services;

public class LoggerService {
    private readonly string _logFilePath;
    private readonly object _lock = new object();

    public LoggerService(string logFilePath) {
        _logFilePath = logFilePath;

        if(!File.Exists(_logFilePath))
            File.WriteAllText(
                _logFilePath,
                "Timestamp,CPU Load %,CPU Temp °C,RAM Used GB,RAM Total GB,GPU Load %,GPU Temp °C,GPU Mem Used MB,GPU Mem Total MB,Active Game\n");
    }

    public void Log(string csvLine) {
        lock (_lock) {
            File.AppendAllText(_logFilePath, csvLine + Environment.NewLine, Encoding.UTF8);
        }
    }
}


using System.IO;
using System.Text;

namespace GamerCompanion.Services;

public class LoggerService {
    private readonly string _logFolder;
    private readonly object _lock = new object();

    public LoggerService(string logFolder) {
        _logFolder = logFolder;

        Directory.CreateDirectory(_logFolder);
    }

    public void Log(string csvLine) {
        var fileName = $"log_{DateTime.Now:yyyy-MM-dd}.csv";
        var filePath = Path.Combine(_logFolder, fileName);


        lock (_lock) {
            if (!File.Exists(filePath))
                File.WriteAllText(
                    filePath,
                    "Timestamp,CPU Load %,CPU Temp °C,RAM Used GB,RAM Total GB,GPU Load %,GPU Temp °C,GPU Mem Used MB,GPU Mem Total MB,Active Game\n");
            File.AppendAllText(filePath, csvLine + Environment.NewLine, Encoding.UTF8);
        }
    }
}


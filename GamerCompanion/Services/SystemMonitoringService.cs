using LibreHardwareMonitor.Hardware;
using System.Diagnostics;

namespace GamerCompanion.Services;

public class SystemMonitoringService : IVisitor {
    private Computer _computer;

    public float CpuLoad { get; private set; }
    public float CpuTemp { get; private set; }
    public string CpuName { get; private set; }

    public float RamUsed { get; private set; }
    public float RamTotal { get; private set; }

    public float GpuLoad { get; private set; }
    public float GpuTemp { get; private set; }
    public float GpuMemUsed { get; private set; }
    public float GpuMemTotal { get; private set; }
    public string GpuName { get; private set; }

    public SystemMonitoringService() {
        _computer = new Computer { IsCpuEnabled = true, IsMemoryEnabled = true, IsGpuEnabled = true };
        _computer.Open();
    }

    public void Update() {
        _computer.Accept(this);
    }

    public void VisitComputer(IComputer computer) {
        foreach (var hardware in computer.Hardware) {
            hardware.Update();
            hardware.Accept(this);
        }
    }

    public void VisitHardware(IHardware hardware) {
        foreach (var sensor in hardware.Sensors) {
            if (hardware.HardwareType == HardwareType.Cpu) {
                CpuName = hardware.Name;
                if (sensor.SensorType == SensorType.Load && sensor.Name == "CPU Total")
                    CpuLoad = sensor.Value ?? 0;
                if (sensor.SensorType == SensorType.Temperature && sensor.Name == "Core Average")
                    CpuTemp = sensor.Value ?? 0;
            }

            if (hardware.HardwareType == HardwareType.Memory && sensor.SensorType == SensorType.Data && sensor.Name == "Memory Used")
                RamUsed = sensor.Value ?? 0;
            if (hardware.HardwareType == HardwareType.Memory && sensor.SensorType == SensorType.Data && sensor.Name == "Memory Available")
                RamTotal = (sensor.Value + RamUsed) ?? 0;

            if (hardware.HardwareType == HardwareType.GpuNvidia || hardware.HardwareType == HardwareType.GpuAmd) {
                GpuName = hardware.Name;
                if (sensor.SensorType == SensorType.Load && sensor.Name == "GPU Core")
                    GpuLoad = sensor.Value ?? 0;
                if (sensor.SensorType == SensorType.Temperature && sensor.Name == "GPU Core")
                    GpuTemp = sensor.Value ?? 0;
                if (sensor.SensorType == SensorType.SmallData) {
                    if (sensor.Name == "GPU Memory Used")
                        GpuMemUsed = sensor.Value ?? 0;
                    if (sensor.Name == "GPU Memory Total")
                        GpuMemTotal = sensor.Value ?? 0;
                }
            }

            //if (hardware.HardwareType == HardwareType.GpuNvidia)
            //    Debug.WriteLine($"{sensor.SensorType}, {sensor.Name}");

            foreach (var subHardware in hardware.SubHardware)
                subHardware.Accept(this);
        }
    }

    public void VisitSensor(ISensor sensor) { }

    public void VisitParameter(IParameter parameter) { }
}


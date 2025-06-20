using LibreHardwareMonitor.Hardware;

namespace GamerCompanion.Services;

public class SystemMonitoringService : IVisitor {
    private Computer _computer;

    public float CpuLoad { get; private set; }
    public float RamUsed { get; private set; }
    public float GpuLoad { get; private set; }

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
            if (hardware.HardwareType == HardwareType.Cpu && sensor.SensorType == SensorType.Load && sensor.Name == "CPU Total")
                CpuLoad = sensor.Value ?? 0;
            if (hardware.HardwareType == HardwareType.Memory && sensor.SensorType == SensorType.Data && sensor.Name == "Memory Used")
                RamUsed = sensor.Value ?? 0;
            if (hardware.HardwareType == HardwareType.GpuNvidia || hardware.HardwareType == HardwareType.GpuAmd)
                if (sensor.SensorType == SensorType.Load && sensor.Name == "GPU Core")
                    GpuLoad = sensor.Value ?? 0;

            foreach (var subHardware in hardware.SubHardware)
                subHardware.Accept(this);
        }
    }

    public void VisitSensor(ISensor sensor) { }

    public void VisitParameter(IParameter parameter) { }
}


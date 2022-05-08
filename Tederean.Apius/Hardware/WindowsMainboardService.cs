#if WINDOWS
using System.Diagnostics;
using System.Runtime.Versioning;
using Tederean.Apius.Interop.Kernel32;

namespace Tederean.Apius.Hardware
{

  [SupportedOSPlatform("windows")]
  public class WindowsMainboardService : IMainboardService
  {

    private readonly PerformanceCounter _cpuPerformanceCounter;


    public string CpuName => "?";


    public WindowsMainboardService()
    {
      _cpuPerformanceCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
    }


    public MainboardSensors GetMainboardSensors()
    {
      var mainboardSensors = new MainboardSensors();

      GetRamValues(mainboardSensors);

      GetLoadValues(mainboardSensors);

      return mainboardSensors;
    }


    private void GetLoadValues(MainboardSensors mainboardSensors)
    {
      var loadPercent = (double)_cpuPerformanceCounter.NextValue();

      mainboardSensors.Load_percent = new Sensor(loadPercent, 0.0, 100.0);
    }

    private void GetRamValues(MainboardSensors mainboardSensors)
    {
      var memoryStatus = Kernel32.GetGlobalMemoryStatusEx();

      var totalMemory_B = memoryStatus.TotalPhysicalMemory;
      var freeMemory_B = memoryStatus.AvailableVirtualMemory;

      var usedMemory_B = totalMemory_B - freeMemory_B;

      mainboardSensors.Memory_B = new Sensor(usedMemory_B, 0.0, totalMemory_B);
    }


    public void Dispose()
    {
    }
  }
}
#endif
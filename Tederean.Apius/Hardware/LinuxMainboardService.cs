using Tederean.Apius.Interop.LibC;

namespace Tederean.Apius.Hardware
{

  public class LinuxMainboardService : IMainboardService
  {

    private readonly IRaplService? _raplService;


    private CpuLoadSample? _lastCpuLoadSample;


    public string CpuName { get; }


    public LinuxMainboardService()
    {
      var cpuinfo = File.ReadAllLines("/proc/cpuinfo");

      var vendorName = GetCpuinfoValue(cpuinfo.First(entry => entry.StartsWith("vendor_id")));
      var cpuBrandName = GetCpuinfoValue(cpuinfo.First(entry => entry.StartsWith("model name")));
      var cpuFlags = GetCpuinfoValue(cpuinfo.First(entry => entry.StartsWith("flags"))).Split(' ');
      var cpuVendor = CpuUtils.GetCpuVendor(vendorName);
      var virtualCores = cpuinfo.Where(entry => entry.StartsWith("processor")).Select(line => int.Parse(GetCpuinfoValue(line))).ToArray();

      CpuName = CpuUtils.GetCpuName(cpuBrandName);

      if (cpuVendor == CpuVendor.AMD && cpuFlags.Contains("rapl") && LibC.IsRoot())
      {
        _raplService = new LinuxAmdRaplService(virtualCores);
      }
    }

    public void Dispose()
    {
      _raplService?.Dispose();
    }


    public MainboardValues GetMainboardValues()
    {
      var mainboardValues = new MainboardValues();

      GetRamValues(mainboardValues);

      GetLoadValues(mainboardValues);

      _raplService?.GetWattageValues(mainboardValues);

      return mainboardValues;
    }


    private void GetLoadValues(MainboardValues mainboardValues)
    {
      var procStat = File.ReadAllLines("/proc/stat");
      var jiffieValues = procStat.First().Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(value => long.Parse(value)).ToArray();

      var totalJiffies = jiffieValues.Sum();
      var workJiffies = jiffieValues.Take(3).Sum();

      var lastCpuLoadSample = _lastCpuLoadSample;
      var currentCpuLoadSample = _lastCpuLoadSample = new CpuLoadSample(workJiffies, totalJiffies);


      if (lastCpuLoadSample != null)
      {
        var deltaWorkJiffies = (double)currentCpuLoadSample.WorkJiffies - lastCpuLoadSample.WorkJiffies;
        var deltaTotalJiffies = (double)currentCpuLoadSample.TotalJiffies - lastCpuLoadSample.TotalJiffies;

        if (deltaTotalJiffies != 0)
        {
          mainboardValues.CurrentLoad_percent = (deltaWorkJiffies / deltaTotalJiffies) * 100.0;
          mainboardValues.MaximumLoad_percent = 100.0;
        }
      }
    }

    private void GetRamValues(MainboardValues mainboardValues)
    {
      var memoryInfo = File.ReadAllLines("/proc/meminfo");

      var totalMemory_kB = GetMemInfoValue(memoryInfo.First(entry => entry.StartsWith("MemTotal:")));
      var freeMemory_kB = GetMemInfoValue(memoryInfo.First(entry => entry.StartsWith("MemFree:")));
      var cachedMemory_kB = GetMemInfoValue(memoryInfo.First(entry => entry.StartsWith("Cached:")));

      var usedMemory_kB = totalMemory_kB - freeMemory_kB - cachedMemory_kB;


      mainboardValues.CurrentMemory_B = usedMemory_kB * 1024.0;
      mainboardValues.MaximumMemory_B = totalMemory_kB * 1024.0;
    }


    private long GetMemInfoValue(string line)
    {
      // Example: "MemTotal:       32849676 kB"

      var valueWithUnit = line.Split(':').Skip(1).First().Trim();
      var valueAsString = valueWithUnit.Split(' ').First();

      return long.Parse(valueAsString);
    }

    private string GetCpuinfoValue(string line)
    {
      // Example: "TLB size	: 2560 4K pages"

      return line.Split(':').Skip(1).First().Trim();
    }


    private record class CpuLoadSample(long WorkJiffies, long TotalJiffies);
  }
}
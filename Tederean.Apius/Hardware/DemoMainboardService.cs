namespace Tederean.Apius.Hardware
{

  public class DemoMainboardService : IMainboardService
  {

    private readonly Random _random = new();


    public string CpuName => "AMD Ryzen 7 5800X";

    public MainboardValues MainboardValues => GetMainboardValues();


    private MainboardValues GetMainboardValues()
    {
      var cpuUtilizationMax = 100.0;
      var cpuWattageMax = 105.0;
      var cpuTemperatureMax = 105.0;
      var cpuMemoryMax = 32768.0;

      var cpuUtilization = _random.NextDouble() * cpuUtilizationMax;
      var cpuWattage = _random.NextDouble() * cpuWattageMax;
      var cpuTemperature = _random.NextDouble() * cpuTemperatureMax;
      var cpuMemory = _random.NextDouble() * cpuMemoryMax;


      return new MainboardValues()
      {
        CurrentUtilization = cpuUtilization,
        MaximumUtilization = cpuUtilizationMax,
        CurrentWattage = cpuWattage,
        MaximumWattage = cpuWattageMax,
        CurrentTemperature = cpuTemperature,
        MaximumTemperature = cpuTemperatureMax,
        CurrentMemory = cpuMemory,
        MaximumMemory = cpuMemoryMax,
      };
    }
  }
}

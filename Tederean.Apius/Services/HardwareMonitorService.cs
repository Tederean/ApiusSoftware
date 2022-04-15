using Tederean.Apius.Interfaces;
using Tederean.Apius.Types;
using Tederean.Apius.Extensions;

namespace Tederean.Apius.Services
{

  public class HardwareMonitorService : IHardwareMonitorService
  {

    private readonly Random _random = new Random();


    public void Start()
    {
    }

    public void Dispose()
    {
    }


    public InitializationCommand GetInitializationCommand()
    {
      return new InitializationCommand()
      {
        CommandID = Enums.CommandID.Initialize,

        Tile0 = "AMD Ryzen 7 5800X",
        Tile1 = "NVIDIA GeForce RTX 3070",

        Chart0 = "Utilization",
        Chart1 = "Wattage",
        Chart2 = "Temperature",
        Chart3 = "Memory",

        Chart4 = "Utilization",
        Chart5 = "Wattage",
        Chart6 = "Temperature",
        Chart7 = "Memory",
      };
    }

    public UpdateCommand GetUpdateCommand()
    {
      var cpuUtilizationMax = 100.0;
      var cpuWattageMax = 105.0;
      var cpuTemperatureMax = 105.0;
      var cpuMemoryMax = 32768.0;

      var gpuUtilizationMax = 100.0;
      var gpuWattageMax = 230.0;
      var gpuTemperatureMax = 105.0;
      var gpuMemoryMax = 8192.0;


      var cpuUtilization = _random.NextDouble() * cpuUtilizationMax;
      var cpuWattage = _random.NextDouble() * cpuWattageMax;
      var cpuTemperature = _random.NextDouble() * cpuTemperatureMax;
      var cpuMemory = _random.NextDouble() * cpuMemoryMax;

      var gpuUtilization = _random.NextDouble() * gpuUtilizationMax;
      var gpuWattage = _random.NextDouble() * gpuWattageMax;
      var gpuTemperature = _random.NextDouble() * gpuTemperatureMax;
      var gpuMemory = _random.NextDouble() * gpuMemoryMax;


      return new UpdateCommand()
      {
        CommandID = Enums.CommandID.Update,

        Ratio0 = ToRatio(cpuUtilization, 0.0, cpuUtilizationMax),
        Ratio1 = ToRatio(cpuWattage, 0.0, cpuWattageMax),
        Ratio2 = ToRatio(cpuTemperature, 0.0, cpuTemperatureMax),
        Ratio3 = ToRatio(cpuMemory, 0.0, cpuMemoryMax),

        Ratio4 = ToRatio(gpuUtilization, 0.0, gpuUtilizationMax),
        Ratio5 = ToRatio(gpuWattage, 0.0, gpuWattageMax),
        Ratio6 = ToRatio(gpuTemperature, 0.0, gpuTemperatureMax),
        Ratio7 = ToRatio(gpuMemory, 0.0, gpuMemoryMax),

        Text0 = cpuUtilization.ToString("0") + " %",
        Text1 = cpuWattage.ToString("0") + " W",
        Text2 = cpuTemperature.ToString("0") + " °C",
        Text3 = cpuMemory.ToString("0") + " MiB",

        Text4 = gpuUtilization.ToString("0") + " %",
        Text5 = gpuWattage.ToString("0") + " W",
        Text6 = gpuTemperature.ToString("0") + " °C",
        Text7 = gpuMemory.ToString("0") + " MiB",
      };
    }

    private int ToRatio(double inputValue, double inputMinimum, double inputMaximum)
    {
      var mappedValue = (int)Math.Round(inputValue.Map(inputMinimum, inputMaximum, 0.0, 32767.0));

      return mappedValue.Clamp(0, 32767);
    }
  }
}

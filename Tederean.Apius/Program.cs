using System.Diagnostics;
using Tederean.Apius.Hardware;
using Tederean.Apius.Interop;
using Tederean.Apius.Serial;
using Tederean.Apius.Tools;

namespace Tederean.Apius
{

  public static class Program
  {

    private static readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

    private static readonly TimeSpan _updateInterval = TimeSpan.FromSeconds(1);


    public static async Task Main(string[] args)
    {
#if DEBUG
      if (!Debugger.IsAttached)
      {
        while (!Debugger.IsAttached)
        {
          await Task.Delay(100);
        }

        Debugger.Break();
      }
#endif


      NativeLibraryResolver.Initialize();


      if (SerialUtils.TryGetSerialPort(args, out string? requestedSerialPort))
      {
        WindowUtil.SetWindowVisibility(isVisible: false);


        using var serial = new SerialService(requestedSerialPort);
        using var monitor = new HardwareService();


        serial.Start();

        await Task.Delay(500);

        serial.SendCommand(monitor.GetInitializationCommand());

        await Task.Delay(500);


        while (!_cancellationTokenSource.IsCancellationRequested)
        {
          await using (new FixedTime(_updateInterval))
          {
            var updateCommand = monitor.GetUpdateCommand();

            serial.SendCommand(updateCommand);
          }
        }
      }
    }
  }
}





//public InitializationCommand GetInitializationCommand()
//{
//  var graphicsCardName = _nvml.DeviceGetName(_nvmlDevice);

//  return new InitializationCommand()
//  {
//    CommandID = Enums.CommandID.Initialize,

//    Tile0 = "AMD Ryzen 7 5800X",
//    Tile1 = graphicsCardName,

//    Chart0 = "Auslastung",
//    Chart1 = "Leistung",
//    Chart2 = "Temperatur",
//    Chart3 = "Speicher",

//    Chart4 = "Auslastung",
//    Chart5 = "Leistung",
//    Chart6 = "Temperatur",
//    Chart7 = "Speicher",
//  };
//}

//public UpdateCommand GetUpdateCommand()
//{
//  var cpuUtilizationMax = 100.0;
//  var cpuWattageMax = 105.0;
//  var cpuTemperatureMax = 105.0;
//  var cpuMemoryMax = 32768.0;

//  var cpuUtilization = _random.NextDouble() * cpuUtilizationMax;
//  var cpuWattage = _random.NextDouble() * cpuWattageMax;
//  var cpuTemperature = _random.NextDouble() * cpuTemperatureMax;
//  var cpuMemory = _random.NextDouble() * cpuMemoryMax;



//  var gpuUtilization = _nvml.DeviceGetUtilizationRates(_nvmlDevice);

//  var powerConsumption_W = _nvml.DeviceGetPowerUsage_mW(_nvmlDevice) / 1000.0;
//  var powerTarget_W = _nvml.DeviceGetPowerManagementLimit_mW(_nvmlDevice) / 1000.0;

//  var gpuTemperature_C = _nvml.DeviceGetTemperature(_nvmlDevice, NvmlTemperatureSensor.Gpu);
//  var gpuThrottle_C = _nvml.DeviceGetTemperatureThreshold(_nvmlDevice, NvmlTemperatureThresholds.Slowdown);

//  var memoryInfo = _nvml.DeviceGetMemoryInfo(_nvmlDevice);



//  var gpuUtilizationRatio = ToRatio(gpuUtilization.Gpu, 0.0, 100.0);
//  var gpuUtilizationText = gpuUtilization.Gpu.ToString("0") + " %";

//  var gpuWattageRatio = ToRatio(powerConsumption_W, 0.0, powerTarget_W);
//  var gpuWattageText = powerConsumption_W.ToString("0") + " W";

//  var gpuTemperatureRatio = ToRatio(gpuTemperature_C, 0.0, gpuThrottle_C);
//  var gpuTemperatureText = gpuTemperature_C.ToString("0") + " °C";

//  var gpuMemoryRatio = ToRatio(memoryInfo.UsedBytes, 0.0, memoryInfo.TotalBytes);
//  var gpuMemoryText = BinaryFormatter.Format(memoryInfo.UsedBytes, "B");



//  return new UpdateCommand()
//  {
//    CommandID = Enums.CommandID.Update,

//    Ratio0 = ToRatio(cpuUtilization, 0.0, cpuUtilizationMax),
//    Ratio1 = ToRatio(cpuWattage, 0.0, cpuWattageMax),
//    Ratio2 = ToRatio(cpuTemperature, 0.0, cpuTemperatureMax),
//    Ratio3 = ToRatio(cpuMemory, 0.0, cpuMemoryMax),

//    Ratio4 = gpuUtilizationRatio,
//    Ratio5 = gpuWattageRatio,
//    Ratio6 = gpuTemperatureRatio,
//    Ratio7 = gpuMemoryRatio,

//    Text0 = cpuUtilization.ToString("0") + " %",
//    Text1 = cpuWattage.ToString("0") + " W",
//    Text2 = cpuTemperature.ToString("0") + " °C",
//    Text3 = BinaryFormatter.Format(cpuMemory * 1024.0 * 1024.0, "B"),

//    Text4 = gpuUtilizationText,
//    Text5 = gpuWattageText,
//    Text6 = gpuTemperatureText,
//    Text7 = gpuMemoryText,
//  };
//}

//private int ToRatio(double inputValue, double inputMinimum, double inputMaximum)
//{
//  var mappedValue = (int)Math.Round(inputValue.Map(inputMinimum, inputMaximum, 0.0, 32767.0));

//  return mappedValue.Clamp(0, 32767);
//}
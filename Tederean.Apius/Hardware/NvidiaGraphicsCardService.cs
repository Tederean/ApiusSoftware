using Tederean.Apius.Extensions;
using Tederean.Apius.Interop.Nvml;

namespace Tederean.Apius.Hardware
{

  public class NvidiaGraphicsCardService : IGraphicsCardService
  {

    private readonly Nvml _nvml;

    private readonly NvmlDevice _nvmlDevice;


    public string GraphicsCardName => _nvml.DeviceGetName(_nvmlDevice);

    public GraphicsCardValues GraphicsCardValues => GetGraphicsCardValues();


    public NvidiaGraphicsCardService(Nvml nvml, NvmlDevice nvmlDevice)
    {
      nvml.ThrowIfNull(nameof(nvml));
      nvmlDevice.ThrowIfNull(nameof(nvmlDevice));

      _nvml = nvml;
      _nvmlDevice = nvmlDevice;
    }


    private GraphicsCardValues GetGraphicsCardValues()
    {
      var gpuUtilization = _nvml.DeviceGetUtilizationRates(_nvmlDevice);

      var powerConsumption_W = _nvml.DeviceGetPowerUsage_mW(_nvmlDevice) / 1000.0;
      var powerTarget_W = _nvml.DeviceGetPowerManagementLimit_mW(_nvmlDevice) / 1000.0;

      var gpuTemperature_C = _nvml.DeviceGetTemperature(_nvmlDevice, NvmlTemperatureSensor.Gpu);
      var gpuThrottle_C = _nvml.DeviceGetTemperatureThreshold(_nvmlDevice, NvmlTemperatureThresholds.Slowdown);

      var memoryInfo = _nvml.DeviceGetMemoryInfo(_nvmlDevice);


      return new GraphicsCardValues()
      {
        CurrentUtilization = gpuUtilization.Gpu,
        MaximumUtilization = 100.0,
        CurrentWattage = powerConsumption_W,
        MaximumWattage = powerTarget_W,
        CurrentTemperature = gpuTemperature_C,
        MaximumTemperature = gpuThrottle_C,
        CurrentMemory = memoryInfo.UsedBytes,
        MaximumMemory = memoryInfo.TotalBytes,
      };
    }
  }
}

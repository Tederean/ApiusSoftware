using Tederean.Apius.Extensions;
using Tederean.Apius.Interop.Nvml;

namespace Tederean.Apius.Hardware
{

  public class ProprietaryNvidiaGraphicsCardService : IGraphicsCardService
  {

    private readonly Nvml _nvml;

    private readonly NvmlDevice _nvmlDevice;


    public string GraphicsCardName { get; }


    public ProprietaryNvidiaGraphicsCardService(Nvml nvml, NvmlDevice nvmlDevice)
    {
      nvml.ThrowIfNull(nameof(nvml));
      nvmlDevice.ThrowIfNull(nameof(nvmlDevice));

      _nvml = nvml;
      _nvmlDevice = nvmlDevice;

      GraphicsCardName = GetGraphicsCardName();
    }

    public void Dispose()
    {
    }


    public GraphicsCardSensors GetGraphicsCardSensors()
    {
      var gpuUtilization = _nvml.DeviceGetUtilizationRates(_nvmlDevice);

      var powerConsumption_W = _nvml.DeviceGetPowerUsage_mW(_nvmlDevice) / 1000.0;
      var powerTarget_W = _nvml.DeviceGetPowerManagementLimit_mW(_nvmlDevice) / 1000.0;

      var gpuTemperature_C = _nvml.DeviceGetTemperature(_nvmlDevice, NvmlTemperatureSensor.Gpu);
      var gpuThrottle_C = _nvml.DeviceGetTemperatureThreshold(_nvmlDevice, NvmlTemperatureThresholds.Slowdown);

      var memoryInfo = _nvml.DeviceGetMemoryInfo(_nvmlDevice);


      return new GraphicsCardSensors()
      {
        Load_percent = new Sensor(gpuUtilization.Gpu, 0.0, 100.0),

        Wattage_W = new Sensor(powerConsumption_W, 0.0, powerTarget_W),

        Temperature_C = new Sensor(gpuTemperature_C, 0.0, gpuThrottle_C),

        Memory_B = new Sensor(memoryInfo.UsedBytes, 0.0, memoryInfo.TotalBytes),
      };
    }


    private string GetGraphicsCardName()
    {
      var graphicsCardName = _nvml.DeviceGetName(_nvmlDevice);

      if (!graphicsCardName.StartsWith("NVIDIA"))
        graphicsCardName = "NVIDIA " + graphicsCardName;

      return graphicsCardName;
    }
  }
}

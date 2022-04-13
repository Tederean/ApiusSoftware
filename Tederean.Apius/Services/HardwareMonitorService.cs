using Tederean.Apius.Interfaces;
using Tederean.Apius.Types;

namespace Tederean.Apius.Services
{

  public class HardwareMonitorService : IHardwareMonitorService
  {

    private readonly Random _random = new Random();


    public void Start()
    {

    }


    public CommunicationData GetData()
    {
      return new CommunicationData()
      {
        CpuUtilization = _random.NextSingle() * 100.0f,
        CpuWattage = _random.NextSingle() * 105.0f,
        CpuTemperature = _random.NextSingle() * 105.0f,
        CpuMemory = _random.NextSingle() * 32768.0f,

        GpuUtilization = _random.NextSingle() * 100.0f,
        GpuWattage = _random.NextSingle() * 230.0f,
        GpuTemperature = _random.NextSingle() * 105.0f,
        GpuMemory = _random.NextSingle() * 8192.0f,
      };
    }


    public void Dispose()
    {

    }
  }
}

using Tederean.Apius.Interop.LmSensors;
using Tederean.Apius.Interop.Nvml;

namespace Tederean.Apius.Hardware
{

  public class HardwareService : IHardwareService
  {

    private Nvml? _nvml;

    private LmSensors? _lmSensors;


    public IMainboardService? MainboardService { get; set; }

    public IGraphicsCardService? GraphicsCardService { get; set; }


    public HardwareService()
    {
      if (OperatingSystem.IsLinux())
      {
        LmSensors.TryCreateInstance(out _lmSensors);

        MainboardService = new LinuxMainboardService(_lmSensors);
      }

      if (Nvml.TryCreateInstance(out _nvml))
      {
        var firstGraphicsCard = _nvml.DeviceGetHandles().FirstOrDefault();

        if (firstGraphicsCard != null)
        {
          GraphicsCardService = new ProprietaryNvidiaGraphicsCardService(_nvml, firstGraphicsCard);
        }
      }
    }

    public void Dispose()
    {
      _nvml?.Dispose();
      _lmSensors?.Dispose();

      MainboardService?.Dispose();
      GraphicsCardService?.Dispose();
    }


    public HardwareSensors GetHardwareInfo()
    {
      return new HardwareSensors()
      {
        MainboardValues = MainboardService?.GetMainboardSensors(),
        GraphicsCardValues = GraphicsCardService?.GetGraphicsCardSensors(),
      };
    }
  }
}

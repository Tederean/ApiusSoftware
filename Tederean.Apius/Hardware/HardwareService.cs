using Tederean.Apius.Interop.Nvml;

namespace Tederean.Apius.Hardware
{

  public class HardwareService : IHardwareService
  {

    private Nvml? _nvml;


    public IMainboardService? MainboardService { get; set; }

    public IGraphicsCardService? GraphicsCardService { get; set; }


    public HardwareService()
    {
      if (OperatingSystem.IsLinux())
      {
        MainboardService = new LinuxMainboardService();
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
    }


    public HardwareValues GetHardwareInfo()
    {
      return new HardwareValues()
      {
        MainboardValues = MainboardService?.GetMainboardValues(),
        GraphicsCardValues = GraphicsCardService?.GetGraphicsCardValues(),
      };
    }
  }
}

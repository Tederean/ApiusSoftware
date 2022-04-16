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
      MainboardService = new DemoMainboardService();

      if (Nvml.TryCreateInstance(out _nvml))
      {
        var firstGraphicsCard = _nvml.DeviceGetHandles().FirstOrDefault();

        if (firstGraphicsCard != null)
        {
          GraphicsCardService = new NvidiaGraphicsCardService(_nvml, firstGraphicsCard);
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
        MainboardValues = MainboardService?.MainboardValues,
        GraphicsCardValues = GraphicsCardService?.GraphicsCardValues,
      };
    }
  }
}

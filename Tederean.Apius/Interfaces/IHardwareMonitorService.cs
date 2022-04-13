using Tederean.Apius.Types;

namespace Tederean.Apius.Interfaces
{

  public interface IHardwareMonitorService : IDisposable
  {

    void Start();


    CommunicationData GetData();
  }
}

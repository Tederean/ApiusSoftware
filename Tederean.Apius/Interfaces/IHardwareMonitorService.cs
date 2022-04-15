using Tederean.Apius.Types;

namespace Tederean.Apius.Interfaces
{

  public interface IHardwareMonitorService : IDisposable
  {

    void Start();


    InitializationCommand GetInitializationCommand();


    UpdateCommand GetUpdateCommand();
  }
}

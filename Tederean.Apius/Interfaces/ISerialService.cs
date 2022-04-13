using Tederean.Apius.Types;

namespace Tederean.Apius.Interfaces
{

  public interface ISerialService : IDisposable
  {

    public void Start();


    public void WriteData(CommunicationData data);
  }
}

namespace Tederean.Apius.Serial
{

  public interface ISerialService : IDisposable
  {

    void Start();


    void SendCommand(ISerialCommand serialCommand);
  }
}

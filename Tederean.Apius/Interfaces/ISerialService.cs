namespace Tederean.Apius.Interfaces
{

  public interface ISerialService : IDisposable
  {

    void Start();


    void SendCommand(ICommand command);
  }
}

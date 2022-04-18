namespace Tederean.Apius.Hardware
{

  public interface IRaplService : IDisposable
  {

    void GetWattageValues(MainboardValues mainboardValues);
  }
}

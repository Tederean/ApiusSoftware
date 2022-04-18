namespace Tederean.Apius.Hardware
{

  public interface IMainboardService : IDisposable
  {

    string CpuName { get; }


    MainboardValues GetMainboardValues();
  }
}

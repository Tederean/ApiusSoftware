namespace Tederean.Apius.Hardware
{

  public interface IGraphicsCardService : IDisposable
  {

    string GraphicsCardName { get; }


    GraphicsCardValues GetGraphicsCardValues();
  }
}

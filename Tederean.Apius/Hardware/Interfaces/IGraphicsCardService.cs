namespace Tederean.Apius.Hardware
{

  public interface IGraphicsCardService
  {

    string GraphicsCardName { get; }

    GraphicsCardValues GraphicsCardValues { get; }
  }
}

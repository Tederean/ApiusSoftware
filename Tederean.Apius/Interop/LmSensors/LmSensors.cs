using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Tederean.Apius.Interop.LmSensors
{

  public class LmSensors : IDisposable
  {

    private bool _disposed;


    public static bool TryCreateInstance([NotNullWhen(true)] out LmSensors? lmSensors)
    {
      try
      {
        if (SensorsInit(IntPtr.Zero) == 0)
        {
          lmSensors = new LmSensors();
          return true;
        }
      }
      catch (DllNotFoundException) { }

      lmSensors = null;
      return false;
    }


    private LmSensors()
    {
      _disposed = false;
    }

    ~LmSensors()
    {
      _disposed = true;

      SensorsCleanup();
    }

    public void Dispose()
    {
      if (!_disposed)
      {
        _disposed = true;

        SensorsCleanup();
        GC.SuppressFinalize(this);
      }
    }



    [DllImport("sensors", EntryPoint = "sensors_init")]
    private static extern int SensorsInit(IntPtr fileHandle);

    [DllImport("sensors", EntryPoint = "sensors_cleanup")]
    private static extern void SensorsCleanup();



    private void ThrowIfDisposed()
    {
      if (_disposed)
      {
        throw new ObjectDisposedException(nameof(LmSensors));
      }
    }
  }
}

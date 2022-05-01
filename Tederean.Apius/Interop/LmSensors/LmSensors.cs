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
        if (Initialize(IntPtr.Zero) == 0)
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

      Cleanup();
    }

    public void Dispose()
    {
      if (!_disposed)
      {
        _disposed = true;

        Cleanup();
        GC.SuppressFinalize(this);
      }
    }

    public SensorsChipName[] GetDetectedChipNames()
    {
      ThrowIfDisposed();

      var list = new List<SensorsChipName>();

      SensorsChipName? chipName;
      int counter = 0;

      while ((chipName = GetSensorChipName(GetDetectedChips(IntPtr.Zero, ref counter))).HasValue)
      {
        list.Add(chipName.Value);
      }

      return list.ToArray();
    }


    private SensorsChipName? GetSensorChipName(IntPtr pointer)
    {
      if (pointer != IntPtr.Zero)
      {
        return Marshal.PtrToStructure<SensorsChipName>(pointer);
      }

      return null;
    }


    [DllImport(NativeLibraryResolver.LmSensorsLibrary, EntryPoint = "sensors_init")]
    private static extern int Initialize(IntPtr fileHandle);

    [DllImport(NativeLibraryResolver.LmSensorsLibrary, EntryPoint = "sensors_cleanup")]
    private static extern void Cleanup();

    [DllImport(NativeLibraryResolver.LmSensorsLibrary, EntryPoint = "sensors_get_detected_chips")]
    private static extern IntPtr GetDetectedChips(IntPtr match, ref int nr);


    // https://github.com/paroj/sensors.py/blob/master/sensors.py
    // code /usr/include/sensors/sensors.h


    private void ThrowIfDisposed()
    {
      if (_disposed)
      {
        throw new ObjectDisposedException(nameof(LmSensors));
      }
    }
  }
}

using System.Reflection;
using System.Runtime.InteropServices;

namespace Tederean.Apius.Interop
{

  public static class NativeLibraryResolver
  {

    public const string NvmlLibrary = "Nvidia Management Library";

    private const string NvmlWindowsLibraryName = "nvml.dll";

    private const string NvmlLinuxLibraryName = "libnvidia-ml.so";


    public const string LmSensorsLibrary = "Lm Sensors Library";

    private const string LmSensorsLinuxLibraryName = "libsensors.so";


    public const string LibCLibrary = "LibC Library";

    private const string LibCLinuxLibraryName = "libc.so";



    public static void Initialize()
    {
      NativeLibrary.SetDllImportResolver(Assembly.GetExecutingAssembly(), ImportResolver);
    }


    private static IntPtr ImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
    {
      if (OperatingSystem.IsWindows())
      {
        if (libraryName == NvmlLibrary && NativeLibrary.TryLoad(NvmlWindowsLibraryName, out IntPtr nvmlHandle))
        {
          return nvmlHandle;
        }
      }

      if (OperatingSystem.IsLinux())
      {
        if (libraryName == NvmlLibrary && NativeLibrary.TryLoad(NvmlLinuxLibraryName, out IntPtr nvmlHandle))
        {
          return nvmlHandle;
        }

        if (libraryName == LmSensorsLibrary && NativeLibrary.TryLoad(LmSensorsLinuxLibraryName, out IntPtr lmSensorsHandle))
        {
          return lmSensorsHandle;
        }

        if (libraryName == LibCLibrary && NativeLibrary.TryLoad(LibCLinuxLibraryName, out IntPtr libcHandle))
        {
          return libcHandle;
        }
      }

      return IntPtr.Zero;
    }
  }
}

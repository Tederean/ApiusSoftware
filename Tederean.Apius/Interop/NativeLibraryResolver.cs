using System.Reflection;
using System.Runtime.InteropServices;

namespace Tederean.Apius.Interop
{

  public static class NativeLibraryResolver
  {

    public const string NvmlLibraryPlaceholder = "Nvidia Management Library";

    private const string NvmlWindowsLibraryName = "nvml.dll";

    private const string NvmlLinuxLibraryName = "libnvidia-ml.so";


    public static void Initialize()
    {
      NativeLibrary.SetDllImportResolver(Assembly.GetExecutingAssembly(), ImportResolver);
    }


    private static IntPtr ImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
    {
      if (OperatingSystem.IsWindows())
      {
        if (libraryName == NvmlLibraryPlaceholder && NativeLibrary.TryLoad(NvmlWindowsLibraryName, out IntPtr handle))
        {
          return handle;
        }
      }

      if (OperatingSystem.IsLinux())
      {
        if (libraryName == NvmlLibraryPlaceholder && NativeLibrary.TryLoad(NvmlLinuxLibraryName, out IntPtr handle))
        {
          return handle;
        }
      }

      return IntPtr.Zero;
    }
  }
}

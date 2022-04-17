using System.Runtime.InteropServices;

namespace Tederean.Apius.Interop.LibC
{

  public static class LibC
  {

    [DllImport("libc", EntryPoint = "getuid")]
    public static extern uint GetUid();


    public static bool IsRoot() => GetUid() == 0;
  }
}

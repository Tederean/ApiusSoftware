using System.Diagnostics.CodeAnalysis;
using System.IO.Ports;

namespace Tederean.Apius.Serial
{

  public static class SerialUtils
  {

    public static bool TryGetSerialPort(string[] args, [NotNullWhen(true)] out string? requestedSerialPort)
    {
      var argsSerialPort = args.ElementAtOrDefault(0);
      var availableSerialPorts = SerialPort.GetPortNames();


      if (!availableSerialPorts.Any())
      {
        Console.Error.WriteLine($"Cannot find any serial port.");

        requestedSerialPort = null;
        return false;
      }


      if (string.IsNullOrWhiteSpace(argsSerialPort))
      {
        Console.Error.WriteLine($"Please specifiy a serial port name as argument. Currently available: {string.Join(" ", availableSerialPorts)}");

        requestedSerialPort = null;
        return false;
      }


      foreach (var foundSerialPort in availableSerialPorts)
      {
        if (foundSerialPort.Equals(argsSerialPort, StringComparison.OrdinalIgnoreCase))
        {
          requestedSerialPort = foundSerialPort;
          return true;
        }
      }


      Console.Error.WriteLine($"Cannot find serial port '{argsSerialPort}'. Currently available: {string.Join(" ", availableSerialPorts)}");

      requestedSerialPort = null;
      return false;
    }
  }
}

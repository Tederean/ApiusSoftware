using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO.Ports;
using Tederean.Apius.Services;
using Tederean.Apius.Tools;

namespace Tederean.Apius
{

  public static class Program
  {

    private static readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

    private static readonly TimeSpan _updateInterval = TimeSpan.FromSeconds(1);


    public static async Task Main(string[] args)
    {
#if DEBUG
      if (!Debugger.IsAttached)
      {
        while (!Debugger.IsAttached)
        {
          await Task.Delay(100);
        }

        Debugger.Break();
      }
#endif


      if (TryGetSerialPort(args, out string? requestedSerialPort))
      {
        WindowUtil.SetWindowVisibility(isVisible: false);

        using var serial = new SerialService(requestedSerialPort);
        using var monitor = new HardwareMonitorService();

        serial.Start();
        monitor.Start();

        await Task.Delay(500);

        serial.SendCommand(monitor.GetInitializationCommand());

        await Task.Delay(500);

        while (!_cancellationTokenSource.IsCancellationRequested)
        {
          await using (new FixedTime(_updateInterval))
          {
            var updateCommand = monitor.GetUpdateCommand();

            serial.SendCommand(updateCommand);
          }
        }
      }
    }


    private static bool TryGetSerialPort(string[] args, [NotNullWhen(true)] out string? requestedSerialPort)
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
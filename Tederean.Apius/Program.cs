using System.Diagnostics;
using Tederean.Apius.Extensions;
using Tederean.Apius.Formating;
using Tederean.Apius.Hardware;
using Tederean.Apius.Interop;
using Tederean.Apius.Serial;
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


      NativeLibraryResolver.Initialize();


      if (SerialUtils.TryGetSerialPort(args, out string? requestedSerialPort))
      {
        WindowUtil.SetWindowVisibility(isVisible: false);

        using var serial = new SerialService(requestedSerialPort);
        using var hardware = new HardwareService();


        serial.Start();
        hardware.GetHardwareInfo();

        await Task.Delay(500);

        SendInitializationCommand(hardware, serial);

        await Task.Delay(500);


        while (!_cancellationTokenSource.IsCancellationRequested)
        {
          await using (new FixedTime(_updateInterval))
          {
            SendUpdateCommand(hardware, serial);
          }
        }
      }
    }

    private static void SendInitializationCommand(IHardwareService hardwareService, ISerialService serialService)
    {
      var initializationCommand = new InitializationCommand()
      {
        Tile0 = hardwareService.MainboardService?.CpuName ?? "Prozessor & Mainboard",
        Tile1 = hardwareService.GraphicsCardService?.GraphicsCardName ?? "Grafikkarte",

        Chart0 = "Auslastung",
        Chart1 = "Leistung",
        Chart2 = "Temperatur",
        Chart3 = "Speicher",

        Chart4 = "Auslastung",
        Chart5 = "Leistung",
        Chart6 = "Temperatur",
        Chart7 = "Speicher",
      };

      serialService.SendCommand(initializationCommand);
    }

    private static void SendUpdateCommand(IHardwareService hardwareService, ISerialService serialService)
    {
      var hardwareInfo = hardwareService.GetHardwareInfo();
      var updateCommand = new UpdateCommand()
      {
        Text0 = string.Empty,
        Text1 = string.Empty,
        Text2 = string.Empty,
        Text3 = string.Empty,

        Text4 = string.Empty,
        Text5 = string.Empty,
        Text6 = string.Empty,
        Text7 = string.Empty,
      };


      var mainboardValues = hardwareInfo.MainboardValues;
      var graphicsCardValues = hardwareInfo.GraphicsCardValues;

      if (mainboardValues != null)
      {
        var load_percent = mainboardValues.Load_percent;
        var wattage_W = mainboardValues.Wattage_W;
        var temperature_C = mainboardValues.Temperature_C;
        var memory_B = mainboardValues.Memory_B;


        if (load_percent != null)
        {
          updateCommand.Ratio0 = ToRatio(load_percent);
          updateCommand.Text0 = load_percent.Value.ToString("0") + " %";
        }

        if (wattage_W != null)
        {
          updateCommand.Ratio1 = ToRatio(wattage_W);
          updateCommand.Text1 = wattage_W.Value.ToString("0") + " W";
        }

        if (temperature_C != null)
        {
          updateCommand.Ratio2 = ToRatio(temperature_C);
          updateCommand.Text2 = temperature_C.Value.ToString("0") + " °C";
        }

        if (memory_B != null)
        {
          updateCommand.Ratio3 = ToRatio(memory_B);
          updateCommand.Text3 = BinaryFormatter.Format(memory_B.Value, "B");
        }
      }


      if (graphicsCardValues != null)
      {
        var load_percent = graphicsCardValues.Load_percent;
        var wattage_W = graphicsCardValues.Wattage_W;
        var temperature_C = graphicsCardValues.Temperature_C;
        var memory_B = graphicsCardValues.Memory_B;


        if (load_percent != null)
        {
          updateCommand.Ratio4 = ToRatio(load_percent);
          updateCommand.Text4 = load_percent.Value.ToString("0") + " %";
        }

        if (wattage_W != null)
        {
          updateCommand.Ratio5 = ToRatio(wattage_W);
          updateCommand.Text5 = wattage_W.Value.ToString("0") + " W";
        }

        if (temperature_C != null)
        {
          updateCommand.Ratio6 = ToRatio(temperature_C);
          updateCommand.Text6 = temperature_C.Value.ToString("0") + " °C";
        }

        if (memory_B != null)
        {
          updateCommand.Ratio7 = ToRatio(memory_B);
          updateCommand.Text7 = BinaryFormatter.Format(memory_B.Value, "B");
        }
      }


      serialService.SendCommand(updateCommand);
    }


    private static int ToRatio(Sensor sensor)
    {
      var mappedValue = (int)Math.Round(sensor.Value.Map(sensor.Minimum, sensor.Maximum, 0.0, 32767.0));

      return mappedValue.Clamp(0, 32767);
    }
  }
}
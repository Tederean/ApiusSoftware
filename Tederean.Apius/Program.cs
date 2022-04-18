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
        Tile0 = hardwareService.MainboardService?.CpuName ?? "?",
        Tile1 = hardwareService.GraphicsCardService?.GraphicsCardName ?? "?",

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
      var updateCommand = new UpdateCommand();

      var mainboardValues = hardwareInfo.MainboardValues;
      var graphicsCardValues = hardwareInfo.GraphicsCardValues;


      if (mainboardValues != null)
      {
        updateCommand.Ratio0 = ToRatio(mainboardValues.CurrentLoad_percent, mainboardValues.MaximumLoad_percent);
        updateCommand.Ratio1 = ToRatio(mainboardValues.CurrentWattage_W, mainboardValues.MaximumWattage_W);
        updateCommand.Ratio2 = ToRatio(mainboardValues.CurrentTemperature_C, mainboardValues.MaximumTemperature_C);
        updateCommand.Ratio3 = ToRatio(mainboardValues.CurrentMemory_B, mainboardValues.MaximumMemory_B);

        updateCommand.Text0 = mainboardValues.CurrentLoad_percent.ToString("0") + " %";
        updateCommand.Text1 = mainboardValues.CurrentWattage_W.ToString("0") + " W";
        updateCommand.Text2 = mainboardValues.CurrentTemperature_C.ToString("0") + " °C";
        updateCommand.Text3 = BinaryFormatter.Format(mainboardValues.CurrentMemory_B, "B");
      }
      else
      {
        updateCommand.Text0 = string.Empty;
        updateCommand.Text1 = string.Empty;
        updateCommand.Text2 = string.Empty;
        updateCommand.Text3 = string.Empty;
      }


      if (graphicsCardValues != null)
      {
        updateCommand.Ratio4 = ToRatio(graphicsCardValues.CurrentLoad_percent, graphicsCardValues.MaximumLoad_percent);
        updateCommand.Ratio5 = ToRatio(graphicsCardValues.CurrentWattage_W, graphicsCardValues.MaximumWattage_W);
        updateCommand.Ratio6 = ToRatio(graphicsCardValues.CurrentTemperature_C, graphicsCardValues.MaximumTemperature_C);
        updateCommand.Ratio7 = ToRatio(graphicsCardValues.CurrentMemory_B, graphicsCardValues.MaximumMemory_B);

        updateCommand.Text4 = graphicsCardValues.CurrentLoad_percent.ToString("0") + " %";
        updateCommand.Text5 = graphicsCardValues.CurrentWattage_W.ToString("0") + " W";
        updateCommand.Text6 = graphicsCardValues.CurrentTemperature_C.ToString("0") + " °C";
        updateCommand.Text7 = BinaryFormatter.Format(graphicsCardValues.CurrentMemory_B, "B");
      }
      else
      {
        updateCommand.Text4 = string.Empty;
        updateCommand.Text5 = string.Empty;
        updateCommand.Text6 = string.Empty;
        updateCommand.Text7 = string.Empty;
      }


      serialService.SendCommand(updateCommand);
    }

    private static int ToRatio(double inputValue, double maximum)
    {
      var mappedValue = (int)Math.Round(inputValue.Map(0.0, maximum, 0.0, 32767.0));

      return mappedValue.Clamp(0, 32767);
    }
  }
}
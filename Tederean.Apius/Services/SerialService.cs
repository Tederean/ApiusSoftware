using System.IO.Ports;
using System.Text.Json;
using Tederean.Apius.Extensions;
using Tederean.Apius.Interfaces;
using Tederean.Apius.Types;

namespace Tederean.Apius.Services
{

  public class SerialService : ISerialService
  {

    private readonly SerialPort _serialPort;

    private readonly JsonSerializerOptions _serializerOptions;


    public SerialService(string serialPort)
    {
      serialPort.ThrowIfEmptyOrWhiteSpace(nameof(serialPort));

      _serialPort = new SerialPort(serialPort, baudRate: 115200);

      _serializerOptions = new JsonSerializerOptions() { WriteIndented = false };
    }


    public void Start()
    {
      _serialPort.Open();
    }


    public void WriteData(CommunicationData communicationData)
    {
      communicationData.ThrowIfNull(nameof(communicationData));

      var jsonString = JsonSerializer.Serialize(communicationData, _serializerOptions);

      _serialPort.WriteLine(jsonString);
    }


    public void Dispose()
    {
      _serialPort.Dispose();
    }
  }
}

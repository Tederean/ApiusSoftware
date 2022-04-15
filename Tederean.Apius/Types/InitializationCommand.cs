using Tederean.Apius.Enums;
using Tederean.Apius.Interfaces;

namespace Tederean.Apius.Types
{

  public class InitializationCommand : ICommand
  {

    public CommandID CommandID { get; set; }


    public string? Tile0 { get; set; }

    public string? Tile1 { get; set; }


    public string? Chart0 { get; set; }

    public string? Chart1 { get; set; }

    public string? Chart2 { get; set; }

    public string? Chart3 { get; set; }


    public string? Chart4 { get; set; }

    public string? Chart5 { get; set; }

    public string? Chart6 { get; set; }

    public string? Chart7 { get; set; }
  }
}

﻿using Tederean.Apius.Enums;
using Tederean.Apius.Interfaces;

namespace Tederean.Apius.Types
{
  public  class UpdateCommand : ICommand
  {

    public CommandID CommandID { get; set; }


    public int Ratio0 { get; set; }

    public int Ratio1 { get; set; }

    public int Ratio2 { get; set; }

    public int Ratio3 { get; set; }


    public int Ratio4 { get; set; }

    public int Ratio5 { get; set; }

    public int Ratio6 { get; set; }

    public int Ratio7 { get; set; }


    public string? Text0 { get; set; }

    public string? Text1 { get; set; }

    public string? Text2 { get; set; }

    public string? Text3 { get; set; }


    public string? Text4 { get; set; }

    public string? Text5 { get; set; }

    public string? Text6 { get; set; }

    public string? Text7 { get; set; }
  }
}

﻿namespace Tederean.Apius.Hardware
{

  public interface IMainboardService
  {

    string CpuName { get; }

    MainboardValues MainboardValues { get; }
  }
}

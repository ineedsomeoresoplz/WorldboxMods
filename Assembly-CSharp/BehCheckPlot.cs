// Decompiled with JetBrains decompiler
// Type: BehCheckPlot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class BehCheckPlot : BehCheckPlotBase
{
  public override BehResult execute(Actor pActor)
  {
    if (!pActor.hasPlot())
      return BehResult.Stop;
    if (this.isBasePlotCheckOk(pActor))
      return this.forceTask(pActor, "progress_plot");
    pActor.leavePlot();
    return BehResult.Stop;
  }
}

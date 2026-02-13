// Decompiled with JetBrains decompiler
// Type: BehCheckPlotIsOk
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class BehCheckPlotIsOk : BehCheckPlotBase
{
  public override BehResult execute(Actor pActor)
  {
    if (this.isBasePlotCheckOk(pActor))
      return BehResult.Continue;
    pActor.leavePlot();
    return BehResult.Stop;
  }
}

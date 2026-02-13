// Decompiled with JetBrains decompiler
// Type: AvgStats
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public readonly struct AvgStats(double pAvg, int pCount, string pName)
{
  public readonly double avg = pAvg;
  public readonly int count = pCount;
  public readonly string name = pName;

  public AvgStats add(double pValue)
  {
    return new AvgStats((this.avg * (double) this.count + pValue) / (double) (this.count + 1), this.count + 1, this.name);
  }
}

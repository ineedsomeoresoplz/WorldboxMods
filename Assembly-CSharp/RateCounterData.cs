// Decompiled with JetBrains decompiler
// Type: RateCounterData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
internal struct RateCounterData(double pTimestamp, double pValue = 0.0)
{
  public double timestamp = pTimestamp;
  public double value = pValue;
}

// Decompiled with JetBrains decompiler
// Type: RewardedPower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public class RewardedPower
{
  public string name;
  public double timeStamp;

  public RewardedPower(string pName, double pTimeStamp)
  {
    this.name = pName;
    this.timeStamp = pTimeStamp;
  }
}

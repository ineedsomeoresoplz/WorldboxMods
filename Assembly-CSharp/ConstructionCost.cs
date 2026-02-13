// Decompiled with JetBrains decompiler
// Type: ConstructionCost
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Beebyte.Obfuscator;
using System;

#nullable disable
[ObfuscateLiterals]
[Serializable]
public class ConstructionCost
{
  public int wood;
  public int stone;
  public int common_metals;
  public int gold;

  public ConstructionCost(int pWood = 0, int pStone = 0, int pCommonMetals = 0, int pGold = 0)
  {
    this.wood = pWood;
    this.stone = pStone;
    this.common_metals = pCommonMetals;
    this.gold = pGold;
  }
}

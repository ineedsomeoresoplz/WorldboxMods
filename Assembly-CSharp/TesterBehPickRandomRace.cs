// Decompiled with JetBrains decompiler
// Type: TesterBehPickRandomRace
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;
using System.Collections.Generic;

#nullable disable
public class TesterBehPickRandomRace : BehaviourActionTester
{
  private static List<string> assets;

  public TesterBehPickRandomRace()
  {
    if (TesterBehPickRandomRace.assets != null)
      return;
    TesterBehPickRandomRace.assets = new List<string>()
    {
      "human",
      "elf",
      "orc",
      "dwarf"
    };
  }

  public override BehResult execute(AutoTesterBot pObject)
  {
    pObject.beh_asset_target = TesterBehPickRandomRace.assets.GetRandom<string>();
    return base.execute(pObject);
  }
}

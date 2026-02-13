// Decompiled with JetBrains decompiler
// Type: TesterBehSpawnRandomPower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;
using System.Collections.Generic;

#nullable disable
public class TesterBehSpawnRandomPower : TesterBehSpawnPower
{
  private static List<string> events;

  public TesterBehSpawnRandomPower()
    : base()
  {
    if (TesterBehSpawnRandomPower.events != null)
      return;
    TesterBehSpawnRandomPower.events = new List<string>();
    foreach (GodPower godPower in AssetManager.powers.list)
    {
      if (godPower.id[0] != '_' && godPower.tester_enabled)
      {
        TesterBehSpawnRandomPower.events.Add(godPower.id);
        if (godPower.type == PowerActionType.PowerDrawTile)
        {
          TesterBehSpawnRandomPower.events.Add(godPower.id);
          TesterBehSpawnRandomPower.events.Add(godPower.id);
          TesterBehSpawnRandomPower.events.Add(godPower.id);
        }
      }
    }
  }

  public override BehResult execute(AutoTesterBot pObject)
  {
    this._power = TesterBehSpawnRandomPower.events.GetRandom<string>();
    return base.execute(pObject);
  }
}

// Decompiled with JetBrains decompiler
// Type: TesterBehSpawnRandomBuilding
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;
using System.Collections.Generic;

#nullable disable
public class TesterBehSpawnRandomBuilding : BehaviourActionTester
{
  private static List<string> assets = new List<string>();
  private static int last_id = 0;

  public TesterBehSpawnRandomBuilding()
  {
    if (TesterBehSpawnRandomBuilding.assets.Count != 0)
      return;
    TesterBehSpawnRandomBuilding.assets.Add("tree_green_1");
    TesterBehSpawnRandomBuilding.assets.Add("fruit_bush");
    TesterBehSpawnRandomBuilding.assets.Add("palm_tree");
    TesterBehSpawnRandomBuilding.assets.Add("pine_tree");
    TesterBehSpawnRandomBuilding.assets.Add("tumor");
    TesterBehSpawnRandomBuilding.assets.Add("golden_brain");
    TesterBehSpawnRandomBuilding.assets.Add("corrupted_brain");
    TesterBehSpawnRandomBuilding.assets.Add("beehive");
    TesterBehSpawnRandomBuilding.assets.Add("ice_tower");
    TesterBehSpawnRandomBuilding.assets.Add("flame_tower");
    TesterBehSpawnRandomBuilding.assets.Add("volcano");
    TesterBehSpawnRandomBuilding.assets.Add("geyser_acid");
    TesterBehSpawnRandomBuilding.assets.Add("geyser");
  }

  public override BehResult execute(AutoTesterBot pObject)
  {
    if (TesterBehSpawnRandomBuilding.last_id > TesterBehSpawnRandomBuilding.assets.Count - 1)
    {
      TesterBehSpawnRandomBuilding.last_id = 0;
      TesterBehSpawnRandomBuilding.assets.Shuffle<string>();
    }
    string asset = TesterBehSpawnRandomBuilding.assets[TesterBehSpawnRandomBuilding.last_id++];
    for (int index = 0; index < 3; ++index)
    {
      TileZone random = BehaviourActionBase<AutoTesterBot>.world.zone_calculator.zones.GetRandom<TileZone>();
      BehaviourActionBase<AutoTesterBot>.world.buildings.addBuilding(asset, random.centerTile, true);
    }
    return base.execute(pObject);
  }
}

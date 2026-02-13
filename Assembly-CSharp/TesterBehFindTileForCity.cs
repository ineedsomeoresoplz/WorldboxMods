// Decompiled with JetBrains decompiler
// Type: TesterBehFindTileForCity
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class TesterBehFindTileForCity : BehaviourActionTester
{
  public override BehResult execute(AutoTesterBot pObject)
  {
    TileZone random = BehaviourActionBase<AutoTesterBot>.world.zone_calculator.zones.GetRandom<TileZone>();
    for (int index = 0; index < 100; ++index)
    {
      if (random.isGoodForNewCity())
      {
        pObject.beh_tile_target = random.centerTile;
        return BehResult.Continue;
      }
      random = BehaviourActionBase<AutoTesterBot>.world.zone_calculator.zones.GetRandom<TileZone>();
    }
    return base.execute(pObject);
  }
}

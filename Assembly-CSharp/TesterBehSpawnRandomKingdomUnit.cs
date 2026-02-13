// Decompiled with JetBrains decompiler
// Type: TesterBehSpawnRandomKingdomUnit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class TesterBehSpawnRandomKingdomUnit : BehaviourActionTester
{
  public override BehResult execute(AutoTesterBot pObject)
  {
    if (BehaviourActionBase<AutoTesterBot>.world.kingdoms.Count == 0)
      return new TesterBehSpawnRandomCivUnit().execute(pObject);
    Kingdom random1 = BehaviourActionBase<AutoTesterBot>.world.kingdoms.getRandom();
    if (random1 == null || !random1.hasUnits() || !random1.hasCities())
      return BehResult.Continue;
    City random2 = random1.getCities().GetRandom<City>();
    if (random2 == null || !random2.hasZones())
      return BehResult.Continue;
    TileZone random3 = random2.zones.GetRandom<TileZone>();
    if (random3 == null)
      return BehResult.Continue;
    WorldTile random4 = random3.tiles.GetRandom<WorldTile>();
    if (random4 == null)
      return BehResult.Continue;
    BehaviourActionBase<AutoTesterBot>.world.units.spawnNewUnit(random1.getActorAsset().id, random4, pMiracleSpawn: true);
    return BehResult.Continue;
  }
}

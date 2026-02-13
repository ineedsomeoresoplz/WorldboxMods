// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehDragonCheckAttackCity
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehDragonCheckAttackCity : BehDragon
{
  public override BehResult execute(Actor pActor)
  {
    if (pActor.beh_tile_target != null)
      return BehResult.Continue;
    long pResult1;
    pActor.data.get("cityToAttack", out pResult1, -1L);
    if (!pResult1.hasValue())
      return BehResult.Continue;
    int pResult2;
    pActor.data.get("attacksForCity", out pResult2);
    if (pResult2 < 1)
    {
      pActor.data.removeLong("cityToAttack");
      return BehResult.Continue;
    }
    City city = BehaviourActionBase<Actor>.world.cities.get(pResult1);
    bool flag = true;
    if (city != null && city.isAlive() && city.buildings.Count > 0)
    {
      WorldTile random = city.buildings.GetRandom<Building>().current_tile.zone.tiles.GetRandom<WorldTile>();
      pActor.beh_tile_target = this.dragon.randomTileWithinLandAttackRange(random);
      int num;
      pActor.data.set("attacksForCity", num = pResult2 - 1);
      flag = false;
    }
    if (flag)
    {
      pActor.data.removeLong("cityToAttack");
      pActor.data.removeInt("attacksForCity");
    }
    return BehResult.Continue;
  }
}

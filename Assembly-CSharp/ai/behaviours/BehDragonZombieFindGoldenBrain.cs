// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehDragonZombieFindGoldenBrain
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehDragonZombieFindGoldenBrain : BehDragon
{
  public override BehResult execute(Actor pActor)
  {
    if (!pActor.hasTrait("zombie") || this.dragon.aggroTargets.Count > 0)
      return BehResult.Continue;
    long pResult;
    pActor.data.get("cityToAttack", out pResult, -1L);
    if (pResult.hasValue() || !BehaviourActionBase<Actor>.world.kingdoms_wild.get("golden_brain").hasBuildings())
      return BehResult.Continue;
    float num1 = 0.0f;
    WorldTile worldTile = (WorldTile) null;
    foreach (Building building in BehaviourActionBase<Actor>.world.kingdoms_wild.get("golden_brain").buildings)
    {
      float num2 = Toolbox.DistTile(building.current_tile, pActor.current_tile);
      if (worldTile == null || (double) num2 < (double) num1)
      {
        worldTile = building.current_tile;
        num1 = num2;
      }
    }
    if (worldTile != null)
    {
      if (this.dragon.landAttackRange(worldTile))
        return this.forceTask(pActor, "dragon_land");
      pActor.beh_tile_target = this.dragon.randomTileWithinLandAttackRange(worldTile);
    }
    return BehResult.Continue;
  }
}

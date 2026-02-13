// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehDragonCheckAttackTile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehDragonCheckAttackTile : BehDragon
{
  public override BehResult execute(Actor pActor)
  {
    if (this.dragon.aggroTargets.Count == 0)
      return BehResult.Continue;
    Actor closestActor = Toolbox.getClosestActor(this.dragon.aggroTargets, pActor.current_tile);
    if (closestActor != null && closestActor.data != null && closestActor.isAlive() && closestActor.current_tile != null)
    {
      pActor.beh_tile_target = this.dragon.randomTileWithinLandAttackRange(closestActor.current_tile);
      if (pActor.current_tile != this.dragon.lastLanded && this.dragon.landAttackRange(closestActor.current_tile) && Dragon.canLand(pActor))
        return this.forceTask(pActor, "dragon_land");
    }
    if (pActor.isFlying())
    {
      foreach (Actor aggroTarget in this.dragon.aggroTargets)
      {
        if (aggroTarget != null && aggroTarget.isAlive() && this.dragon.targetWithinSlide(aggroTarget.current_tile))
          return this.forceTask(pActor, "dragon_slide");
      }
    }
    return BehResult.Continue;
  }
}

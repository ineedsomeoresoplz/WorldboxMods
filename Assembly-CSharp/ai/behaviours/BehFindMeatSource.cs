// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehFindMeatSource
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehFindMeatSource : BehaviourActionActor
{
  private MeatTargetType _meat_target_type;
  private bool _check_for_factions;

  public BehFindMeatSource(MeatTargetType pMeatTargetType = MeatTargetType.Meat, bool pCheckForFactions = true)
  {
    this._check_for_factions = pCheckForFactions;
    this._meat_target_type = pMeatTargetType;
  }

  public override BehResult execute(Actor pActor)
  {
    if (pActor.beh_actor_target != null && this.isTargetOk(pActor, pActor.beh_actor_target.a))
      return BehResult.Continue;
    pActor.beh_actor_target = (BaseSimObject) this.getClosestMeatActor(pActor);
    return pActor.beh_actor_target != null ? BehResult.Continue : BehResult.Stop;
  }

  private Actor getClosestMeatActor(Actor pActor)
  {
    bool pRandom = Randy.randomBool();
    WorldTile currentTile = pActor.current_tile;
    float num1 = (float) int.MaxValue;
    Actor closestMeatActor = (Actor) null;
    int pChunkRadius = Randy.randomInt(1, 3);
    foreach (Actor pTarget in Finder.getUnitsFromChunk(currentTile, pChunkRadius, pRandom: pRandom))
    {
      float num2 = (float) Toolbox.SquaredDistTile(pTarget.current_tile, currentTile);
      if ((double) num2 < (double) num1 && this.isTargetOk(pActor, pTarget))
      {
        bool flag = pTarget.isSameSpecies(pActor.asset.id);
        switch (this._meat_target_type)
        {
          case MeatTargetType.Meat:
            if (!pTarget.asset.source_meat || flag)
              continue;
            break;
          case MeatTargetType.MeatSameSpecies:
            if (flag)
              break;
            continue;
          case MeatTargetType.Insect:
            if (!pTarget.asset.source_meat_insect || flag)
              continue;
            break;
        }
        num1 = num2;
        closestMeatActor = pTarget;
        if (pRandom)
        {
          if (Randy.randomBool())
            break;
        }
      }
    }
    return closestMeatActor;
  }

  private bool isTargetOk(Actor pActor, Actor pTarget)
  {
    return pTarget != pActor && pActor.canAttackTarget((BaseSimObject) pTarget, this._check_for_factions) && pTarget.asset.actor_size <= pActor.asset.actor_size && pTarget.current_tile.isSameIsland(pActor.current_tile);
  }
}

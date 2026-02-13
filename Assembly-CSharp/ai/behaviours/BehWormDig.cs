// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehWormDig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
namespace ai.behaviours;

public class BehWormDig : BehaviourActionActor
{
  protected override void setupErrorChecks()
  {
    base.setupErrorChecks();
    this.null_check_tile_target = true;
  }

  public override BehResult execute(Actor pActor)
  {
    int pResult;
    pActor.data.get("size", out pResult);
    if (pResult > 0 && pActor.beh_tile_target.Height < 220)
      BehaviourActionBase<Actor>.world.loopWithBrush(pActor.beh_tile_target, Brush.get(pResult), new PowerActionWithID(BehWormDig.tileDrawWorm));
    else
      BehaviourActionBase<Actor>.world.loopWithBrush(pActor.beh_tile_target, Brush.get(pResult), new PowerActionWithID(BehWormDig.tileFlashWorm));
    return BehResult.RestartTask;
  }

  public static bool tileFlashWorm(WorldTile pTile, string pPowerID)
  {
    BehaviourActionBase<Actor>.world.flash_effects.flashPixel(pTile, 20);
    return true;
  }

  public static bool tileDrawWorm(WorldTile pTile, string pPowerID)
  {
    BehWormDig.wormTile(pTile);
    return true;
  }

  public static void wormTile(WorldTile pTile)
  {
    BehaviourActionBase<Actor>.world.flash_effects.flashPixel(pTile, 20);
    if (pTile.top_type != null)
    {
      MapAction.decreaseTile(pTile, false);
    }
    else
    {
      if (pTile.Type.increase_to == null || pTile.Type.road)
        return;
      int num = pTile.Type.increase_to.id.StartsWith("mountain", StringComparison.Ordinal) ? 1 : 0;
      bool flag = pTile.Type.increase_to.id.StartsWith("hill", StringComparison.Ordinal);
      if (num == 0 && !flag && (pTile.Type.decrease_to == null || Randy.randomBool()))
      {
        MapAction.increaseTile(pTile, false, "destroy");
      }
      else
      {
        if (pTile.Type.decrease_to == null)
          return;
        MapAction.decreaseTile(pTile, false, "destroy");
      }
    }
  }
}

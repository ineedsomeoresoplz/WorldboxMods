// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehCityActorRemoveFire
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace ai.behaviours;

public class BehCityActorRemoveFire : BehCityActor
{
  protected override void setupErrorChecks()
  {
    base.setupErrorChecks();
    this.null_check_tile_target = true;
  }

  public override BehResult execute(Actor pActor)
  {
    foreach (WorldTile pTile in pActor.current_tile.getTilesAround(3))
    {
      if (pTile != null)
        this.putOutFireForTile(pTile);
    }
    return BehResult.Continue;
  }

  private void putOutFireForTile(WorldTile pTile, bool pForceEffect = false)
  {
    bool flag = false;
    if (pTile.isOnFire())
    {
      pTile.stopFire();
      flag = true;
    }
    if (flag | pForceEffect)
      EffectsLibrary.spawnAt("fx_water_splash", Vector2Int.op_Implicit(pTile.pos), 0.1f);
    if (!pTile.hasBuilding())
      return;
    pTile.building.stopFire();
  }
}

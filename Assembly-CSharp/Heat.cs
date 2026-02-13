// Decompiled with JetBrains decompiler
// Type: Heat
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class Heat
{
  public const int MAX_HEAT = 404;
  private float tickTimer;
  private List<WorldTile> tilesToRemove = new List<WorldTile>();
  private HashSetWorldTile tiles;

  internal void clear()
  {
    if (this.tiles == null)
      this.tiles = new HashSetWorldTile();
    foreach (WorldTile tile in (HashSet<WorldTile>) this.tiles)
      tile.heat = 0;
    this.tiles.Clear();
  }

  internal void addTile(WorldTile pTile, int pHeat = 1)
  {
    if (pTile.heat == 0)
      this.tiles.Add(pTile);
    pTile.heat += pHeat;
    if (pTile.heat > 404)
      pTile.heat = 404;
    if (pTile.heat > 5)
    {
      if (pTile.hasBuilding() && pTile.building.isAlive())
        pTile.building.getHit(0.0f, true, AttackType.Other, (BaseSimObject) null, true, false, true);
      if (pTile.Type.layer_type == TileLayerType.Ocean)
      {
        MapAction.removeLiquid(pTile);
        World.world.particles_smoke.spawn(pTile.posV3);
      }
      if (pTile.isTemporaryFrozen())
        pTile.unfreeze();
      if (pTile.Type.grey_goo)
        pTile.startFire();
      pTile.setBurned();
    }
    if (pTile.heat > 10)
    {
      if (pTile.Type.burnable)
      {
        if (pTile.Type.IsType("tnt") || pTile.Type.IsType("tnt_timed"))
          AchievementLibrary.tnt_and_heat.check();
        pTile.startFire();
      }
      if (pTile.hasBuilding() && !pTile.building.isRuin())
        pTile.building.getHit((float) pTile.building.getMaxHealth() * 0.1f, true, AttackType.Divine, (BaseSimObject) null, false, false, true);
      if (pTile.hasBuilding())
        pTile.startFire();
      pTile.doUnits((Action<Actor>) (tActor =>
      {
        if (tActor.asset.very_high_flyer || tActor.isImmuneToFire())
          return;
        ActionLibrary.addBurningEffectOnTarget((BaseSimObject) null, (BaseSimObject) tActor);
        tActor.getHit(50f, pAttackType: AttackType.Fire, pSkipIfShake: false);
      }));
    }
    if (pTile.heat > 20)
    {
      if (pTile.Type.explodable && pTile.explosion_wave == 0)
        World.world.explosion_layer.explodeBomb(pTile);
      if (pTile.hasBuilding())
        pTile.startFire();
      if (pTile.top_type != null)
        MapAction.decreaseTile(pTile, true);
    }
    if (WorldLawLibrary.world_law_gaias_covenant.isEnabled())
      return;
    if (pTile.heat > 30)
    {
      if (pTile.Type.lava)
        LavaHelper.addLava(pTile);
      if (pTile.Type.IsType("soil_low") || pTile.Type.IsType("soil_high"))
        pTile.setTileType("sand");
      if (pTile.Type.road)
        pTile.setTileType("sand");
    }
    if (pTile.heat > 100 && pTile.Type.IsType("sand"))
      LavaHelper.addLava(pTile);
    if (pTile.heat <= 160 /*0xA0*/)
      return;
    if (pTile.Type.IsType("mountains"))
      LavaHelper.addLava(pTile);
    if (!pTile.Type.IsType("hills"))
      return;
    LavaHelper.addLava(pTile);
  }

  internal void update(float pElapsed)
  {
    if (World.world.isPaused() || this.tiles.Count == 0)
      return;
    if ((double) this.tickTimer > 0.0)
    {
      this.tickTimer -= pElapsed;
    }
    else
    {
      this.tickTimer = 1f;
      this.tilesToRemove.Clear();
      foreach (WorldTile tile in (HashSet<WorldTile>) this.tiles)
      {
        --tile.heat;
        if (tile.heat <= 0)
          this.tilesToRemove.Add(tile);
      }
      for (int index = 0; index < this.tilesToRemove.Count; ++index)
        this.tiles.Remove(this.tilesToRemove[index]);
      this.tilesToRemove.Clear();
    }
  }
}

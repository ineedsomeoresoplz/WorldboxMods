// Decompiled with JetBrains decompiler
// Type: TornadoEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityPools;

#nullable disable
public class TornadoEffect : BaseEffect
{
  private const float TORNADO_SCALE_DECREASE_SPLIT_MULTIPLIER = 0.5f;
  private const float TORNADO_SCALE_DECREASE_MULTIPLIER = 0.5f;
  private const float TORNADO_SCALE_INCREASE_MULTIPLIER = 1.5f;
  private const float TORNADO_SCALE_MIN = 0.1f;
  private const float TORNADO_SCALE_MAX = 5f;
  public const float TORNADO_SCALE_DEFAULT = 0.5f;
  private const int RANDOM_TILE_RANGE = 5;
  private const float ACTION_INTERVAL_FORCE = 0.1f;
  private const float ACTION_INTERVAL_TERRAFORM = 0.3f;
  private const float SHRINK_TIMER = 10f;
  private const float MOVE_SPEED = 0.15f;
  private float _tornado_timer_force;
  private float _tornado_timer_terraform;
  private float _shrink_timer;
  private float _target_scale = 0.5f;
  private WorldTile _target_tile;
  private static readonly Dictionary<WorldTile, HashSet<TornadoEffect>> _tornadoes_by_tiles = new Dictionary<WorldTile, HashSet<TornadoEffect>>();
  internal float colorEffect;
  internal Material colorMaterial;

  internal override void prepare(WorldTile pTile, float pScale = 0.5f)
  {
    base.prepare(pTile, pScale);
    this.current_tile = World.world.GetTileSimple((int) ((Component) this).transform.localPosition.x, (int) ((Component) this).transform.localPosition.y);
    this._target_tile = Toolbox.getRandomTileWithinDistance(this.current_tile, 5);
    this.setScale(0.110000007f);
    this._target_scale = pScale;
    this._tornado_timer_force = 0.1f;
    this._tornado_timer_terraform = 0.3f;
    this._shrink_timer = 10f;
    this.addTornadoToTile();
  }

  public override void update(float pElapsed)
  {
    base.update(pElapsed);
    this.updateChangeScale(pElapsed);
    if (World.world.isPaused() || this.isKilled())
      return;
    this.updateColorEffect(pElapsed);
    if (this.state == 2)
    {
      this.deathAnimation(pElapsed);
    }
    else
    {
      this.updateMovement();
      this.updateShrinking(pElapsed);
      if ((double) this._tornado_timer_force > 0.0)
      {
        this._tornado_timer_force -= pElapsed;
      }
      else
      {
        this._tornado_timer_force = 0.1f;
        this.tornadoActionForce(this.current_tile);
      }
      if ((double) this._tornado_timer_terraform > 0.0)
      {
        this._tornado_timer_terraform -= pElapsed;
      }
      else
      {
        this._tornado_timer_terraform = 0.3f;
        this.tornadoActionTerraform(this.current_tile, this.scale);
      }
    }
  }

  private void updateMovement()
  {
    WorldTile tileSimple = World.world.GetTileSimple((int) ((Component) this).transform.localPosition.x, (int) ((Component) this).transform.localPosition.y);
    if (tileSimple != this.current_tile)
    {
      this.removeTornadoFromTile();
      this.current_tile = tileSimple;
      this.addTornadoToTile();
    }
    if (this.current_tile == this._target_tile)
      this._target_tile = Toolbox.getRandomTileWithinDistance(this.current_tile, 5);
    Vector3 vector3 = Vector3.op_Subtraction(this._target_tile.posV3, ((Component) this).transform.localPosition);
    Vector3 normalized = ((Vector3) ref vector3).normalized;
    Transform transform = ((Component) this).transform;
    transform.localPosition = Vector3.op_Addition(transform.localPosition, Vector3.op_Multiply(normalized, 0.15f));
  }

  private void updateShrinking(float pElapsed)
  {
    if ((double) this._shrink_timer > 0.0)
    {
      this._shrink_timer -= pElapsed;
    }
    else
    {
      this._shrink_timer = Randy.randomFloat(7.5f, 12.5f);
      this.shrink();
    }
  }

  private void tornadoActionTerraform(WorldTile pTile, float pScale = 0.5f)
  {
    BrushData brushData = Brush.get((int) ((double) pScale * 6.0));
    bool flag = true;
    if (!MapAction.checkTileDamageGaiaCovenant(pTile, true))
      flag = false;
    for (int index = 0; index < brushData.pos.Length; ++index)
    {
      Vector2Int pos1 = pTile.pos;
      int pX = ((Vector2Int) ref pos1).x + brushData.pos[index].x;
      Vector2Int pos2 = pTile.pos;
      int pY = ((Vector2Int) ref pos2).y + brushData.pos[index].y;
      if (pX >= 0 && pX < MapBox.width && pY >= 0 && pY < MapBox.height)
      {
        WorldTile tileSimple = World.world.GetTileSimple(pX, pY);
        if (tileSimple.Type.ocean)
        {
          MapAction.removeLiquid(tileSimple);
          if (Randy.randomChance(0.15f))
            TornadoEffect.spawnBurst(tileSimple, "rain", pScale);
        }
        if (flag)
        {
          if (tileSimple.top_type != null || tileSimple.Type.life)
            MapAction.decreaseTile(tileSimple, false);
          if (tileSimple.Type.lava)
          {
            LavaHelper.removeLava(tileSimple);
            TornadoEffect.spawnBurst(tileSimple, "lava", pScale);
          }
        }
        if (tileSimple.hasBuilding() && tileSimple.building.asset.can_be_damaged_by_tornado)
          tileSimple.building.getHit(1f, true, AttackType.Other, (BaseSimObject) null, true, false, true);
        if (tileSimple.isTemporaryFrozen())
          tileSimple.unfreeze(10);
        if (tileSimple.isOnFire())
          tileSimple.stopFire();
      }
    }
  }

  private void tornadoActionForce(WorldTile pTile)
  {
    World.world.applyForceOnTile(pTile, pForceAmount: 3f, pForceOut: false);
  }

  private static void spawnBurst(WorldTile pTile, string pType, float pScale)
  {
    if (World.world.drop_manager.getActiveIndex() > 3000)
      return;
    World.world.drop_manager.spawnParabolicDrop(pTile, pType, pMinHeight: 0.62f, pMaxHeight: 104f * pScale, pMinRadius: 0.7f, pMaxRadius: 23.5f * pScale);
  }

  internal void shrink()
  {
    if (this.isKilled())
      return;
    float pScale = this.scale * 0.5f;
    if ((double) pScale < 0.10000000149011612)
      this.die();
    else
      this.resizeTornado(pScale);
  }

  internal void grow()
  {
    if (this.isKilled())
      return;
    float pScale = Mathf.Min(this.scale * 1.5f, 5f);
    if ((double) pScale >= 5.0)
      AchievementLibrary.tornado.check();
    this.resizeTornado(pScale);
  }

  internal bool split()
  {
    if (this.isKilled())
      return false;
    AchievementLibrary.baby_tornado.check();
    float pScale = this.scale * 0.5f;
    if ((double) pScale < 0.10000000149011612)
    {
      this.die();
      return true;
    }
    EffectsLibrary.spawnAtTile("fx_tornado", this.current_tile, pScale);
    this.resizeTornado(pScale);
    return true;
  }

  internal void resizeTornado(float pScale) => this._target_scale = pScale;

  public void startColorEffect(string pType = "red")
  {
    this.colorEffect = 0.3f;
    switch (pType)
    {
      case "red":
        this.colorMaterial = LibraryMaterials.instance.mat_damaged;
        break;
      case "white":
        this.colorMaterial = LibraryMaterials.instance.mat_highlighted;
        break;
    }
    this.updateColorEffect(0.0f);
  }

  private void updateColorEffect(float pElapsed)
  {
    if ((double) this.colorEffect == 0.0)
      return;
    this.colorEffect -= pElapsed;
    if ((double) this.colorEffect < 0.0)
      this.colorEffect = 0.0f;
    double colorEffect = (double) this.colorEffect;
  }

  internal static void growTornados(WorldTile pTile)
  {
    TornadoEffect.resizeOnTile(pTile, "grow");
    for (int index = 0; index < pTile.neighboursAll.Length; ++index)
      TornadoEffect.resizeOnTile(pTile.neighboursAll[index], "grow");
  }

  internal static void shrinkTornados(WorldTile pTile)
  {
    TornadoEffect.resizeOnTile(pTile, "shrink");
    for (int index = 0; index < pTile.neighboursAll.Length; ++index)
      TornadoEffect.resizeOnTile(pTile.neighboursAll[index], "shrink");
  }

  internal static void resizeOnTile(WorldTile pTile, string direction)
  {
    foreach (BaseEffect baseEffect in World.world.stack_effects.get("fx_tornado").getList())
    {
      if (baseEffect.active && baseEffect.current_tile == pTile)
      {
        TornadoEffect tornadoEffect = baseEffect as TornadoEffect;
        if (direction == "grow")
          tornadoEffect.grow();
        else
          tornadoEffect.shrink();
      }
    }
  }

  private void deathAnimation(float pElapsed)
  {
    if ((double) this.scale > 0.0)
      this.updateChangeScale(pElapsed);
    else
      this.kill();
  }

  public void die()
  {
    this.state = 2;
    this.resizeTornado(0.0f);
    this.removeTornadoFromTile();
  }

  internal void updateChangeScale(float pElapsed)
  {
    if ((double) this.scale == (double) this._target_scale)
      return;
    if ((double) this.scale < (double) this._target_scale)
      this.startColorEffect();
    else
      this.startColorEffect("white");
    if ((double) this.scale > (double) this._target_scale)
    {
      this.setScale(this.scale - 0.2f * pElapsed);
      if ((double) this.scale < (double) this._target_scale)
        this.setScale(this._target_scale);
    }
    else
    {
      this.setScale(this.scale + 0.2f * pElapsed);
      if ((double) this.scale > (double) this._target_scale)
        this.setScale(this._target_scale);
    }
    if ((double) this.scale > 0.10000000149011612)
      return;
    this.die();
  }

  private void addTornadoToTile()
  {
    HashSet<TornadoEffect> tornadoEffectSet;
    if (!TornadoEffect._tornadoes_by_tiles.TryGetValue(this.current_tile, out tornadoEffectSet))
    {
      tornadoEffectSet = UnsafeCollectionPool<HashSet<TornadoEffect>, TornadoEffect>.Get();
      TornadoEffect._tornadoes_by_tiles.Add(this.current_tile, tornadoEffectSet);
    }
    tornadoEffectSet.Add(this);
  }

  private void removeTornadoFromTile()
  {
    HashSet<TornadoEffect> tornadoEffectSet;
    if (!TornadoEffect._tornadoes_by_tiles.TryGetValue(this.current_tile, out tornadoEffectSet))
      return;
    tornadoEffectSet.Remove(this);
    if (tornadoEffectSet.Count != 0)
      return;
    UnsafeCollectionPool<HashSet<TornadoEffect>, TornadoEffect>.Release(tornadoEffectSet);
    TornadoEffect._tornadoes_by_tiles.Remove(this.current_tile);
  }

  public static HashSet<TornadoEffect> getTornadoesFromTile(WorldTile pTile)
  {
    HashSet<TornadoEffect> tornadoEffectSet;
    return !TornadoEffect._tornadoes_by_tiles.TryGetValue(pTile, out tornadoEffectSet) ? (HashSet<TornadoEffect>) null : tornadoEffectSet;
  }

  public static void Clear()
  {
    foreach (HashSet<TornadoEffect> tornadoEffectSet in TornadoEffect._tornadoes_by_tiles.Values)
      UnsafeCollectionPool<HashSet<TornadoEffect>, TornadoEffect>.Release(tornadoEffectSet);
    TornadoEffect._tornadoes_by_tiles.Clear();
  }
}

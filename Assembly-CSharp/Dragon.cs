// Decompiled with JetBrains decompiler
// Type: Dragon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Dragon : BaseActorComponent
{
  private DragonAsset dragonAsset;
  private DragonState state;
  internal float idle_time = -1f;
  internal float sleep_time = -1f;
  internal SpriteAnimation spriteAnimation;
  internal HashSet<Actor> aggroTargets = new HashSet<Actor>();
  internal WorldTile lastLanded;
  private HashSet<WorldTile> _landAttackTiles = new HashSet<WorldTile>();
  private WorldTile _landAttackPosCheck;
  internal int _landAttackCache;
  internal HashSet<WorldTile> _slideAttackTilesFlip = new HashSet<WorldTile>();
  internal HashSet<WorldTile> _slideAttackTilesNoFlip = new HashSet<WorldTile>();
  private WorldTile _slideAttackPosCheckFlip;
  private WorldTile _slideAttackPosCheckNoFlip;
  internal int _slideAttackTilesFlipCache;
  internal int _slideAttackTilesNoFlipCache;

  internal override void create(Actor pActor)
  {
    base.create(pActor);
    this.spriteAnimation = ((Component) this).GetComponent<SpriteAnimation>();
    this.dragonAsset = !(this.actor.asset.id == "zombie_dragon") ? PrefabLibrary.instance.dragonAsset : PrefabLibrary.instance.zombieDragonAsset;
    this.actor.setFlying(true);
    this.setFrames(DragonState.Fly, true);
  }

  private void playSound(DragonState pState)
  {
    switch (this.state)
    {
      case DragonState.LandAttack:
        MusicBox.playSound("event:/SFX/UNITS/dragon/fire_breath", ((Component) this).transform.localPosition.x, ((Component) this).transform.localPosition.y);
        break;
      case DragonState.Slide:
        MusicBox.playSound("event:/SFX/UNITS/dragon/swoop", ((Component) this).transform.localPosition.x, ((Component) this).transform.localPosition.y);
        break;
    }
  }

  internal static bool shouldFly(Actor pActor, WorldTile pTile = null)
  {
    if (pTile == null)
      pTile = pActor.current_tile;
    return !Dragon.canLand(pActor, pTile);
  }

  internal static bool canLand(Actor pActor, WorldTile pTile = null)
  {
    if (pTile == null)
      pTile = pActor.current_tile;
    if (pTile.Type.ground)
      return true;
    return pTile.Type.lava && !pActor.asset.die_in_lava;
  }

  internal void attackTile(WorldTile pTile)
  {
    if (pTile == null)
      return;
    bool flag = this.actor.hasTrait("zombie");
    if (flag)
    {
      DropsLibrary.action_acid(pTile);
      if (pTile.hasUnits() || Randy.randomBool())
        World.world.drop_manager.spawnParabolicDrop(pTile, "acid", pMinHeight: 0.1f, pMaxHeight: 3.5f, pMinRadius: 0.5f, pMaxRadius: 4f, pScale: Randy.randomFloat(0.025f, 0.2f));
    }
    else
    {
      pTile.startFire(true);
      if (pTile.hasBuilding())
        pTile.building.getHit(10f, true, AttackType.Other, (BaseSimObject) null, true, false, true);
      if (pTile.hasUnits() || Randy.randomBool())
        World.world.drop_manager.spawnParabolicDrop(pTile, "fire", pMinHeight: 0.1f, pMaxHeight: 3.5f, pMinRadius: 0.5f, pMaxRadius: 4f, pScale: Randy.randomFloat(0.025f, 0.2f));
    }
    if (!pTile.hasUnits())
      return;
    MapAction.damageWorld(pTile, 2, AssetManager.terraform.get(flag ? "zombie_dragon_attack" : "dragon_attack"), (BaseSimObject) this.actor);
  }

  internal bool hasTargetsForSlide()
  {
    if (WorldLawLibrary.world_law_peaceful_monsters.isEnabled())
      return false;
    this.attackRange(this.actor.flip);
    foreach (WorldTile tTile in this.actor.flip ? this._slideAttackTilesFlip : this._slideAttackTilesNoFlip)
    {
      if (Dragon.hasTarget(tTile, this.actor))
        return true;
    }
    return false;
  }

  internal bool targetWithinSlide(WorldTile pTargetTile)
  {
    if (WorldLawLibrary.world_law_peaceful_monsters.isEnabled())
      return false;
    this.attackRange(true);
    if (this._slideAttackTilesFlip.Contains(pTargetTile))
    {
      this.actor.setFlip(true);
      return true;
    }
    this.attackRange(false);
    if (!this._slideAttackTilesNoFlip.Contains(pTargetTile))
      return false;
    this.actor.setFlip(false);
    return true;
  }

  internal static Kingdom getIgnoredKingdom(Actor pActor)
  {
    return pActor.hasTrait("zombie") ? World.world.kingdoms_wild.get("undead") : World.world.kingdoms_wild.get("dragons");
  }

  internal bool targetsWithinLandAttackRange()
  {
    foreach (Actor aggroTarget in this.aggroTargets)
    {
      if (!aggroTarget.isRekt() && this.landAttackRange(aggroTarget.current_tile))
        return true;
    }
    return false;
  }

  internal bool landAttackRange(WorldTile pTargetTile)
  {
    Vector2Int pos = this.actor.current_tile.pos;
    int x1 = ((Vector2Int) ref pos).x;
    pos = this.actor.current_tile.pos;
    int y1 = ((Vector2Int) ref pos).y;
    pos = pTargetTile.pos;
    int x2 = ((Vector2Int) ref pos).x;
    pos = pTargetTile.pos;
    int y2 = ((Vector2Int) ref pos).y;
    if ((double) Toolbox.Dist(x1, y1, x2, y2) > 9.0)
      return false;
    this.landAttackTiles(this.actor.current_tile);
    return this._landAttackTiles.Contains(pTargetTile);
  }

  internal HashSet<WorldTile> landAttackTiles(WorldTile pTile)
  {
    if (this._landAttackPosCheck == pTile)
    {
      ++this._landAttackCache;
      return this._landAttackTiles;
    }
    this._landAttackCache = 0;
    this._landAttackTiles.Clear();
    this._landAttackPosCheck = pTile;
    for (int index1 = 0; index1 < 12; ++index1)
    {
      for (int index2 = 0; index2 < 20; ++index2)
      {
        MapBox world = World.world;
        Vector2Int pos = pTile.pos;
        int pX = ((Vector2Int) ref pos).x + index2 - 10;
        pos = pTile.pos;
        int pY = ((Vector2Int) ref pos).y - index1 + 1;
        WorldTile tile = world.GetTile(pX, pY);
        if (tile != null)
        {
          pos = pTile.pos;
          int x1 = ((Vector2Int) ref pos).x;
          pos = pTile.pos;
          int y1 = ((Vector2Int) ref pos).y;
          pos = tile.pos;
          int x2 = ((Vector2Int) ref pos).x;
          pos = tile.pos;
          int y2 = ((Vector2Int) ref pos).y;
          if ((double) Toolbox.Dist(x1, y1, x2, y2) <= 9.0)
            this._landAttackTiles.Add(tile);
        }
      }
    }
    return this._landAttackTiles;
  }

  internal WorldTile randomTileWithinLandAttackRange(WorldTile pTile)
  {
    Toolbox.temp_list_tiles.Clear();
    Vector2Int pos;
    for (int index = 9; index > 1; --index)
    {
      MapBox world = World.world;
      pos = pTile.pos;
      int x = ((Vector2Int) ref pos).x;
      pos = pTile.pos;
      int pY = ((Vector2Int) ref pos).y + index;
      WorldTile tile = world.GetTile(x, pY);
      if (tile != null)
      {
        pTile = tile;
        break;
      }
    }
    for (int index1 = 0; index1 < 12; ++index1)
    {
      for (int index2 = 0; index2 < 20; ++index2)
      {
        MapBox world = World.world;
        pos = pTile.pos;
        int pX = ((Vector2Int) ref pos).x + index2 - 10;
        pos = pTile.pos;
        int pY = ((Vector2Int) ref pos).y - index1 + 1;
        WorldTile tile = world.GetTile(pX, pY);
        if (tile != null)
        {
          pos = pTile.pos;
          int x1 = ((Vector2Int) ref pos).x;
          pos = pTile.pos;
          int y1 = ((Vector2Int) ref pos).y;
          pos = tile.pos;
          int x2 = ((Vector2Int) ref pos).x;
          pos = tile.pos;
          int y2 = ((Vector2Int) ref pos).y;
          if ((double) Toolbox.Dist(x1, y1, x2, y2) <= 9.0 && Dragon.canLand(this.actor, tile))
            Toolbox.temp_list_tiles.Add(tile);
        }
      }
    }
    return Toolbox.temp_list_tiles.Count == 0 ? pTile : Toolbox.temp_list_tiles.GetRandom<WorldTile>();
  }

  internal HashSet<WorldTile> attackRange(bool flip)
  {
    if (flip)
    {
      if (this._slideAttackPosCheckFlip == this.actor.current_tile)
      {
        ++this._slideAttackTilesFlipCache;
        return this._slideAttackTilesFlip;
      }
      this._slideAttackTilesFlipCache = 0;
      this._slideAttackTilesFlip.Clear();
      this._slideAttackPosCheckFlip = this.actor.current_tile;
    }
    else
    {
      if (this._slideAttackPosCheckNoFlip == this.actor.current_tile)
      {
        ++this._slideAttackTilesNoFlipCache;
        return this._slideAttackTilesNoFlip;
      }
      this._slideAttackTilesNoFlipCache = 0;
      this._slideAttackTilesNoFlip.Clear();
      this._slideAttackPosCheckNoFlip = this.actor.current_tile;
    }
    int num = !flip ? 20 : -25;
    for (int index1 = 0; index1 < 4; ++index1)
    {
      for (int index2 = 0; index2 < 35; ++index2)
      {
        WorldTile tile = World.world.GetTile(this.actor.current_tile.x + index2 - 15 + num, this.actor.current_tile.y - index1 + 2);
        if (tile != null)
        {
          if (flip)
            this._slideAttackTilesFlip.Add(tile);
          if (!flip)
            this._slideAttackTilesNoFlip.Add(tile);
        }
      }
    }
    return flip ? this._slideAttackTilesFlip : this._slideAttackTilesNoFlip;
  }

  private static bool hasTarget(WorldTile tTile, Actor pActor1)
  {
    if (tTile.hasBuilding() && tTile.building.isUsable())
      return true;
    if (!tTile.hasUnits())
      return false;
    Kingdom tIgnoredKingdom = Dragon.getIgnoredKingdom(pActor1);
    bool tTargetFound = false;
    tTile.doUnits((Func<Actor, bool>) (pActor2 =>
    {
      if ((double) pActor2.position_height > 0.0 || pActor2.kingdom == tIgnoredKingdom)
        return true;
      tTargetFound = true;
      return false;
    }));
    return tTargetFound;
  }

  public void setFrames(DragonState pDragonState, bool pForce = false)
  {
    if (this.state == pDragonState && !pForce)
      return;
    this.actor.setShowShadow(pDragonState == DragonState.Fly);
    this.state = pDragonState;
    this.playSound(this.state);
    DragonAssetContainer asset = this.dragonAsset.getAsset(pDragonState);
    this.spriteAnimation.setFrames(asset.frames);
    this.spriteAnimation.timeBetweenFrames = asset.speed;
    this.spriteAnimation.resetAnim();
    this.spriteAnimation.looped = true;
  }

  internal static bool clickToWakeup(BaseSimObject pTarget, WorldTile pTile = null)
  {
    if (!pTarget.a.isTask("dragon_sleep"))
      return false;
    pTarget.a.cancelAllBeh();
    pTarget.a.setTask("dragon_wakeup");
    return true;
  }

  internal static bool canFlip(BaseSimObject pTarget = null, WorldTile pTile = null)
  {
    switch (pTarget.a.getActorComponent<Dragon>().state)
    {
      case DragonState.Fly:
      case DragonState.Idle:
        return true;
      case DragonState.LandAttack:
      case DragonState.Death:
      case DragonState.SleepStart:
      case DragonState.SleepLoop:
      case DragonState.SleepUp:
      case DragonState.Landing:
      case DragonState.Slide:
      case DragonState.Up:
        return false;
      default:
        return true;
    }
  }

  internal static bool getHit(BaseSimObject pSelf, BaseSimObject pAttackedBy = null, WorldTile pTile = null)
  {
    Actor a = pSelf.a;
    Dragon actorComponent = a.getActorComponent<Dragon>();
    if (WorldLawLibrary.world_law_peaceful_monsters.isEnabled())
      return true;
    bool flag = false;
    actorComponent.aggroTargets.RemoveWhere((Predicate<Actor>) (tAttacker => tAttacker.isRekt()));
    if (pAttackedBy != null)
    {
      if (pAttackedBy.isActor() && actorComponent.aggroTargets.Add(pAttackedBy.a))
        flag = actorComponent.aggroTargets.Count == 1;
      if (pAttackedBy.hasCity())
      {
        a.data.set("cityToAttack", pAttackedBy.getCity().data.id);
        a.data.set("attacksForCity", Randy.randomInt(4, 12));
      }
    }
    switch (a.ai.task?.id)
    {
      case "dragon_sleep":
        a.data.set("justGotHit", true);
        a.cancelAllBeh();
        a.setTask("dragon_wakeup");
        break;
      case "dragon_idle":
        int pResult;
        a.data.get("landAttacks", out pResult);
        if (pResult > 2 || Dragon.shouldFly(a) || pAttackedBy == null)
        {
          a.data.set("justGotHit", true);
          a.cancelAllBeh();
          a.setTask("dragon_up");
          break;
        }
        if (!pAttackedBy.isFlying() && actorComponent.landAttackRange(pAttackedBy.current_tile) && Dragon.canLand(a))
        {
          a.cancelAllBeh();
          a.setTask("dragon_land_attack");
          break;
        }
        break;
      case "dragon_fly":
        if (flag)
        {
          a.cancelAllBeh();
          if (!pAttackedBy.isFlying() && actorComponent.landAttackRange(pAttackedBy.current_tile) && Dragon.canLand(a) && actorComponent.lastLanded != a.current_tile)
          {
            a.setTask("dragon_land");
            break;
          }
          if (actorComponent.targetWithinSlide(pAttackedBy.current_tile))
          {
            a.setTask("dragon_slide");
            break;
          }
          a.setTask("dragon_fly");
          break;
        }
        break;
      case "dragon_wakeup":
      case "dragon_up":
        a.data.set("justGotHit", true);
        break;
    }
    return true;
  }

  internal static bool dragonFall(BaseSimObject pTarget, WorldTile pTile, float pElapsed)
  {
    Dragon actorComponent = pTarget.a.getActorComponent<Dragon>();
    SpriteAnimation spriteAnimation = actorComponent.spriteAnimation;
    spriteAnimation.looped = false;
    spriteAnimation.ignorePause = true;
    if (pTarget.isFlying())
    {
      actorComponent.setFrames(DragonState.Landing);
      if (spriteAnimation.currentFrameIndex < spriteAnimation.frames.Length - 1)
        return true;
      pTarget.a.setFlying(false);
      return true;
    }
    actorComponent.setFrames(DragonState.Death);
    if (spriteAnimation.currentFrameIndex == spriteAnimation.frames.Length - 1)
      pTarget.a.updateDeadBlackAnimation(World.world.elapsed);
    return true;
  }

  public override void update(float pElapsed)
  {
    base.update(pElapsed);
    if (this.actor.isRekt() || World.world.isPaused())
      return;
    this.checkLiquid();
  }

  internal void checkLiquid()
  {
    if (this.actor.isFlying() || this.actor.is_moving || this.actor.isEgg() || !this.actor.current_tile.Type.liquid)
      return;
    if (this.actor.hasTask())
    {
      if (this.actor.isTask("dragon_up") || this.actor.isTask("dragon_wakeup"))
        return;
      if (this.actor.isTask("dragon_sleep"))
      {
        this.actor.cancelAllBeh();
        this.actor.setTask("dragon_wakeup");
        return;
      }
    }
    this.actor.cancelAllBeh();
    this.actor.setTask("dragon_up");
  }

  public HashSet<WorldTile> getLandAttackTiles() => this._landAttackTiles;
}

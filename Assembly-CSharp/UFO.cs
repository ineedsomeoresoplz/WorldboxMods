// Decompiled with JetBrains decompiler
// Type: UFO
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class UFO : BaseActorComponent
{
  private SpriteRenderer beamRnd;
  internal SpriteAnimation beamAnim;
  internal HashSet<Actor> aggroTargets = new HashSet<Actor>();

  internal override void create(Actor pActor)
  {
    base.create(pActor);
    this.beamRnd = ((Component) ((Component) this).transform.Find("Beam")).GetComponent<SpriteRenderer>();
    this.beamAnim = ((Component) ((Component) this).transform.Find("Beam")).GetComponent<SpriteAnimation>();
    this.actor.position_height = this.actor.asset.default_height;
    this.actor.getSpriteAnimation().forceUpdateFrame();
    this.hideBeam();
  }

  public static bool click(BaseSimObject pTarget, WorldTile pTile = null)
  {
    Actor a = pTarget.a;
    if (a.ai.task?.id == "ufo_attack")
      return false;
    a.cancelAllBeh();
    a.setTask("ufo_attack");
    return true;
  }

  internal void startBeam()
  {
    this.beamAnim.stopAt(pNow: true);
    this.beamAnim.isOn = true;
    ((Renderer) this.beamRnd).enabled = true;
    MusicBox.playSound(this.actor.asset.sound_attack, this.actor.current_tile);
  }

  public override void update(float pElapsed)
  {
    base.update(pElapsed);
    this.beamAnim.update(pElapsed);
    if (this.beamAnim.isOn)
      World.world.stack_effects.light_blobs.Add(new LightBlobData()
      {
        position = new Vector2(this.actor.current_position.x, this.actor.current_position.y),
        radius = 1f
      });
    if ((double) this.actor.stats["speed"] < 50.0 && this.actor.ai.task?.id == "ufo_fly")
      this.actor.stats["speed"] += pElapsed * 10f;
    if (World.world.isPaused() || !this.actor.isAlive() || (double) this.actor.position_height >= (double) this.actor.asset.default_height)
      return;
    this.actor.position_height += (float) ((double) this.actor.stats["speed"] * (double) pElapsed * 0.10000000149011612);
  }

  internal void hideBeam()
  {
    this.beamAnim.isOn = false;
    ((Renderer) this.beamRnd).enabled = false;
  }

  internal static bool getHit(BaseSimObject pSelf, BaseSimObject pAttackedBy = null, WorldTile pTile = null)
  {
    Actor a = pSelf.a;
    UFO actorComponent = a.getActorComponent<UFO>();
    actorComponent.aggroTargets.RemoveWhere((Predicate<Actor>) (tAttacker => tAttacker == null || !tAttacker.isAlive()));
    if (pAttackedBy != null && pAttackedBy.isActor())
      actorComponent.aggroTargets.Add(pAttackedBy?.a);
    string id = a.ai.task?.id;
    if (id == "ufo_fly" || id == "ufo_explore")
    {
      a.cancelAllBeh();
      if (pAttackedBy == null)
        a.setTask("ufo_flee");
      else
        a.setTask("ufo_hit");
    }
    return true;
  }

  public static bool ufoFall(BaseSimObject pTarget, WorldTile pTile, float pElapsed)
  {
    pTarget.a.updateFall();
    if ((double) pTarget.a.position_height == 0.0)
    {
      WorldTile tile = World.world.GetTile((int) pTarget.a.current_position.x, (int) pTarget.a.current_position.y);
      if (tile != null)
      {
        MapAction.damageWorld(tile, 5, AssetManager.terraform.get("ufo_explosion"), pTarget);
        EffectsLibrary.spawnAtTileRandomScale("fx_explosion_ufo", tile, 0.45f, 0.6f);
      }
      pTarget.a.dieAndDestroy(AttackType.Other);
    }
    return true;
  }
}

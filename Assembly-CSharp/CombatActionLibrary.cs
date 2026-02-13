// Decompiled with JetBrains decompiler
// Type: CombatActionLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai;
using System;
using UnityEngine;

#nullable disable
public class CombatActionLibrary : AssetLibrary<CombatActionAsset>
{
  public static CombatActionAsset combat_attack_melee;
  public static CombatActionAsset combat_attack_range;
  public static CombatActionAsset combat_cast_spell;
  public static CombatActionAsset combat_action_deflect;
  public static CombatActionAsset combat_action_dash;
  public static CombatActionAsset combat_action_backstep;

  public override void init()
  {
    base.init();
    CombatActionAsset pAsset1 = new CombatActionAsset();
    pAsset1.id = "combat_attack_melee";
    pAsset1.play_unit_attack_sounds = true;
    pAsset1.rate = 6;
    pAsset1.action = new CombatAction(this.attackMeleeAction);
    pAsset1.basic = true;
    CombatActionLibrary.combat_attack_melee = this.add(pAsset1);
    CombatActionAsset pAsset2 = new CombatActionAsset();
    pAsset2.id = "combat_attack_range";
    pAsset2.play_unit_attack_sounds = true;
    pAsset2.rate = 6;
    pAsset2.action = new CombatAction(this.attackRangeAction);
    pAsset2.basic = true;
    CombatActionLibrary.combat_attack_range = this.add(pAsset2);
    CombatActionAsset pAsset3 = new CombatActionAsset();
    pAsset3.id = "combat_cast_spell";
    pAsset3.play_unit_attack_sounds = true;
    pAsset3.cost_stamina = 5;
    pAsset3.is_spell_use = true;
    pAsset3.rate = 3;
    pAsset3.action = new CombatAction(CombatActionLibrary.tryToCastSpell);
    CombatActionLibrary.combat_cast_spell = this.add(pAsset3);
    CombatActionAsset pAsset4 = new CombatActionAsset();
    pAsset4.id = "combat_deflect_projectile";
    pAsset4.cost_stamina = 5;
    pAsset4.chance = 0.2f;
    pAsset4.pools = new CombatActionPool[1];
    pAsset4.action_actor = new CombatActionActor(this.doDeflect);
    CombatActionLibrary.combat_action_deflect = this.add(pAsset4);
    CombatActionAsset pAsset5 = new CombatActionAsset();
    pAsset5.id = "combat_dodge";
    pAsset5.chance = 0.2f;
    pAsset5.cost_stamina = 5;
    pAsset5.action_actor = new CombatActionActor(this.doDodgeAction);
    pAsset5.pools = AssetLibrary<CombatActionAsset>.a<CombatActionPool>(CombatActionPool.BEFORE_HIT);
    this.add(pAsset5);
    CombatActionAsset pAsset6 = new CombatActionAsset();
    pAsset6.id = "combat_block";
    pAsset6.chance = 0.2f;
    pAsset6.cost_stamina = 5;
    pAsset6.cooldown = 0.5f;
    pAsset6.action_actor = new CombatActionActor(this.doBlockAction);
    pAsset6.pools = AssetLibrary<CombatActionAsset>.a<CombatActionPool>(CombatActionPool.BEFORE_HIT_BLOCK);
    this.add(pAsset6);
    CombatActionAsset pAsset7 = new CombatActionAsset();
    pAsset7.id = "combat_random_jump";
    pAsset7.cost_stamina = 5;
    pAsset7.cooldown = 2f;
    this.add(pAsset7);
    CombatActionAsset pAsset8 = new CombatActionAsset();
    pAsset8.id = "combat_dash";
    pAsset8.cost_stamina = 10;
    pAsset8.chance = 0.2f;
    pAsset8.cooldown = 2f;
    pAsset8.action_actor_target_position = new CombatActionActorTargetPosition(this.doDashAction);
    pAsset8.pools = AssetLibrary<CombatActionAsset>.a<CombatActionPool>(CombatActionPool.BEFORE_ATTACK_MELEE);
    CombatActionLibrary.combat_action_dash = this.add(pAsset8);
    CombatActionAsset pAsset9 = new CombatActionAsset();
    pAsset9.id = "combat_backstep";
    pAsset9.cost_stamina = 10;
    pAsset9.chance = 0.2f;
    pAsset9.cooldown = 1f;
    pAsset9.can_do_action = (CombatActionCheckStart) ((pSelf, pAttackTarget) => !pSelf.current_tile.Type.block && (double) Toolbox.SquaredDistVec2Float(pSelf.current_position, pAttackTarget.current_position) < (double) (pSelf.getAttackRangeSquared() * 0.5f));
    pAsset9.action_actor_target_position = new CombatActionActorTargetPosition(this.doBackstepAction);
    pAsset9.pools = AssetLibrary<CombatActionAsset>.a<CombatActionPool>(CombatActionPool.BEFORE_ATTACK_RANGE);
    CombatActionLibrary.combat_action_backstep = this.add(pAsset9);
    CombatActionAsset pAsset10 = new CombatActionAsset();
    pAsset10.id = "combat_throw_bomb";
    pAsset10.cost_stamina = 5;
    pAsset10.chance = 0.2f;
    pAsset10.cooldown = 8f;
    pAsset10.action_actor_target_position = new CombatActionActorTargetPosition(this.doThrowBombAction);
    pAsset10.can_do_action = (CombatActionCheckStart) ((pSelf, pAttackTarget) =>
    {
      float num = Toolbox.SquaredDistVec2Float(pSelf.current_position, pAttackTarget.current_position);
      return (double) num > 36.0 && (double) num < 2500.0;
    });
    pAsset10.pools = AssetLibrary<CombatActionAsset>.a<CombatActionPool>(CombatActionPool.BEFORE_ATTACK_MELEE, CombatActionPool.BEFORE_ATTACK_RANGE);
    this.add(pAsset10);
    CombatActionAsset pAsset11 = new CombatActionAsset();
    pAsset11.id = "combat_throw_torch";
    pAsset11.cost_stamina = 30;
    pAsset11.chance = 0.2f;
    pAsset11.cooldown = 8f;
    pAsset11.action_actor_target_position = new CombatActionActorTargetPosition(this.doThrowTorchAction);
    pAsset11.can_do_action = (CombatActionCheckStart) ((pSelf, pAttackTarget) =>
    {
      float num = Toolbox.SquaredDistVec2Float(pSelf.current_position, pAttackTarget.current_position);
      return (double) num > 36.0 && (double) num < 2500.0;
    });
    pAsset11.pools = AssetLibrary<CombatActionAsset>.a<CombatActionPool>(CombatActionPool.BEFORE_ATTACK_MELEE, CombatActionPool.BEFORE_ATTACK_RANGE);
    this.add(pAsset11);
  }

  private bool doThrowBombAction(Actor pSelf, Vector2 pTarget, WorldTile pTile = null)
  {
    ActionLibrary.throwBombAtTile((BaseSimObject) pSelf, pTile);
    pSelf.punchTargetAnimation(Vector2.op_Implicit(pTarget), pAngle: 45f);
    return true;
  }

  private bool doThrowTorchAction(Actor pSelf, Vector2 pTarget, WorldTile pTile = null)
  {
    ActionLibrary.throwTorchAtTile((BaseSimObject) pSelf, pTile);
    pSelf.punchTargetAnimation(Vector2.op_Implicit(pTarget), pAngle: 45f);
    return true;
  }

  private bool doBackstepAction(Actor pActor, Vector2 pTarget, WorldTile pTile = null)
  {
    float pForceAmountDirection = 5f;
    float pForceHeight = 1.2f;
    Vector2 currentPosition1 = pActor.current_position;
    pActor.punchTargetAnimation(Vector2.op_Implicit(pTarget), false, pAngle: -20f);
    pActor.calculateForce(currentPosition1.x, currentPosition1.y, pTarget.x, pTarget.y, pForceAmountDirection, pForceHeight);
    Vector2 currentPosition2 = pActor.current_position;
    currentPosition2.y += pActor.getHeight();
    BaseEffect baseEffect = EffectsLibrary.spawnAt("fx_dodge", currentPosition2, pActor.actor_scale);
    if (Object.op_Inequality((Object) baseEffect, (Object) null))
      ((Component) baseEffect).transform.rotation = Toolbox.getEulerAngle(currentPosition1, pTarget);
    return true;
  }

  private bool doDashAction(Actor pActor, Vector2 pTarget, WorldTile pTile = null)
  {
    float pForceAmountDirection = 5f;
    float pForceHeight = 1.2f;
    Vector2 currentPosition1 = pActor.current_position;
    pActor.punchTargetAnimation(Vector2.op_Implicit(pTarget), pAngle: 50f);
    pActor.addStatusEffect("dash", pColorEffect: false);
    pActor.calculateForce(pTarget.x, pTarget.y, currentPosition1.x, currentPosition1.y, pForceAmountDirection, pForceHeight);
    Vector2 currentPosition2 = pActor.current_position;
    currentPosition2.y += pActor.getHeight();
    BaseEffect baseEffect = EffectsLibrary.spawnAt("fx_dodge", currentPosition2, pActor.actor_scale);
    if (Object.op_Inequality((Object) baseEffect, (Object) null))
      ((Component) baseEffect).transform.rotation = Toolbox.getEulerAngle(currentPosition1, pTarget);
    return true;
  }

  private bool doBlockAction(Actor pActor, AttackData pData, float pTargetX = 0.0f, float pTargetY = 0.0f)
  {
    ActorTool.applyForceToUnit(pData, (BaseSimObject) pActor, 0.1f);
    if (!pActor.is_visible)
      return true;
    Vector2 currentPosition = pActor.current_position;
    Vector2 pPos = Vector2.op_Implicit(pData.hit_position);
    pActor.punchTargetAnimation(Vector2.op_Implicit(pPos), false, pAngle: -40f);
    BaseEffect baseEffect = EffectsLibrary.spawnAt("fx_block", pPos, pActor.a.actor_scale);
    if (Object.op_Equality((Object) baseEffect, (Object) null))
      return true;
    ((Component) baseEffect).transform.rotation = Toolbox.getEulerAngle(currentPosition.x, currentPosition.y, pPos.x, pPos.y);
    return true;
  }

  private bool doDeflect(Actor pActor, AttackData pData, float pTargetX = 0.0f, float pTargetY = 0.0f)
  {
    Vector2 pTowardsPosition = Vector2.op_Implicit(pData.initiator_position);
    pActor.spawnSlashPunch(pTowardsPosition);
    pActor.stopMovement();
    pActor.punchTargetAnimation(Vector2.op_Implicit(pTowardsPosition), pReverse: pActor.hasRangeAttack());
    pActor.startAttackCooldown();
    return true;
  }

  private bool doDodgeAction(Actor pActor, AttackData pData, float pTargetX = 0.0f, float pTargetY = 0.0f)
  {
    float pForceAmountDirection = 3f;
    float pForceHeight = 1.5f;
    Vector2 pVec1 = Vector2.op_Implicit(pActor.cur_transform_position);
    Vector2 vector2 = Vector2.op_Implicit(pData.initiator_position);
    Vector2 pVector = Vector2.op_Subtraction(pVec1, vector2);
    Vector2 pVec2 = !Randy.randomBool() ? Vector2.op_Addition(pVec1, Vector2.op_Multiply(Toolbox.rotateVector(pVector, -90f), pForceAmountDirection)) : Vector2.op_Addition(pVec1, Vector2.op_Multiply(Toolbox.rotateVector(pVector, 90f), pForceAmountDirection));
    pActor.calculateForce(pVec1.x, pVec1.y, pVec2.x, pVec2.y, pForceAmountDirection, pForceHeight);
    pActor.addStatusEffect("dodge", pColorEffect: false);
    pActor.punchTargetAnimation(Vector2.op_Implicit(pVec1), false, pAngle: -60f);
    Vector2 currentPosition = pActor.current_position;
    currentPosition.y += pActor.getHeight();
    BaseEffect baseEffect = EffectsLibrary.spawnAt("fx_dodge", currentPosition, pActor.actor_scale);
    if (Object.op_Inequality((Object) baseEffect, (Object) null))
      ((Component) baseEffect).transform.rotation = Toolbox.getEulerAngle(pVec1, pVec2);
    return true;
  }

  public bool attackRangeAction(AttackData pData)
  {
    Actor a = pData.initiator.a;
    BaseSimObject target = pData.target;
    string projectileId = pData.projectile_id;
    double actorScale = (double) a.actor_scale;
    float scaleMod = a.getScaleMod();
    float stat1 = a.stats["size"];
    int stat2 = (int) a.stats["projectiles"];
    Vector2 vector2_1;
    if (target == null)
    {
      vector2_1 = Vector2.op_Implicit(pData.hit_position);
    }
    else
    {
      vector2_1 = this.getAttackTargetPosition(pData);
      vector2_1.y += 0.2f * scaleMod;
    }
    float stat3 = a.stats["accuracy"];
    float num1 = Mathf.Clamp(Randy.randomFloat(0.0f, (float) ((double) Toolbox.DistVec2Float(a.current_position, vector2_1) / (double) stat3 * 0.25)), 0.0f, 2f);
    float pStartPosZ = 0.6f * scaleMod;
    float pTargetZ = 0.0f;
    float num2 = 0.0f;
    for (int index = 0; index < stat2; ++index)
    {
      Vector2 vector2_2;
      // ISSUE: explicit constructor call
      ((Vector2) ref vector2_2).\u002Ector(vector2_1.x, vector2_1.y);
      if ((double) stat3 < 10.0)
      {
        Vector2 vector2_3 = Vector2.op_Multiply(this.getInnacuracyVector(stat3), num1);
        vector2_2 = Vector2.op_Addition(vector2_2, vector2_3);
      }
      Vector3 newPoint = Toolbox.getNewPoint(a.current_position.x, a.current_position.y, vector2_2.x, vector2_2.y, stat1 * scaleMod);
      newPoint.y += a.getHeight();
      if (target != null && target.isInAir())
        pTargetZ = target.getHeight();
      num2 = World.world.projectiles.spawn((BaseSimObject) a, target, projectileId, newPoint, Vector2.op_Implicit(vector2_2), pTargetZ, pStartPosZ, pData.kill_action, pData.kingdom).getLaunchAngle();
    }
    a.spawnSlash(vector2_1, pTargetZ: pTargetZ, pAngle: new float?(num2));
    return true;
  }

  public Vector2 getInnacuracyVector(float pAccuracyStat)
  {
    float num1 = (float) (1.0 * (10.0 - (double) pAccuracyStat) / 10.0);
    float num2 = (float) ((double) Randy.random() * 2.0 * Math.PI);
    return new Vector2(num1 * (float) Math.Cos((double) num2), num1 * (float) Math.Sin((double) num2));
  }

  public static bool tryToCastSpell(AttackData pData)
  {
    Actor a = pData.initiator.a;
    BaseSimObject pTarget = pData.target;
    SpellAsset randomSpell = a.getRandomSpell();
    if (!a.hasEnoughMana(randomSpell.cost_mana) || !Randy.randomChance(randomSpell.chance + randomSpell.chance * a.stats["skill_spell"]))
      return false;
    if (randomSpell.cast_target == CastTarget.Himself)
      pTarget = (BaseSimObject) a;
    if (randomSpell.cast_entity == CastEntity.BuildingsOnly)
    {
      if (pTarget.isActor())
        return false;
    }
    else if (randomSpell.cast_entity == CastEntity.UnitsOnly && pTarget.isBuilding())
      return false;
    if ((double) randomSpell.health_ratio > 0.0)
    {
      float healthRatio = a.getHealthRatio();
      if ((double) randomSpell.health_ratio <= (double) healthRatio)
        return false;
    }
    if ((double) randomSpell.min_distance > 0.0 && (double) Toolbox.SquaredDistTile(a.current_tile, pTarget.current_tile) < (double) randomSpell.min_distance * (double) randomSpell.min_distance)
      return false;
    bool castSpell = false;
    if (randomSpell.action != null)
      castSpell = randomSpell.action.RunAnyTrue((BaseSimObject) a, pTarget, pTarget.current_tile);
    if (castSpell)
    {
      a.doCastAnimation();
      a.addStatusEffect("recovery_spell");
    }
    return castSpell;
  }

  public bool attackMeleeAction(AttackData pData)
  {
    AttackDataResult attackDataResult = MapBox.newAttack(pData);
    if (pData.initiator.a.is_visible && EffectsLibrary.canShowSlashEffect())
      this.showMeleeSlashAttack(pData);
    Action killAction = pData.kill_action;
    if (killAction != null)
      killAction();
    return attackDataResult.state == ApplyAttackState.Hit;
  }

  public void showMeleeSlashAttack(AttackData pData)
  {
    pData.initiator.a.spawnSlash(Vector2.op_Implicit(pData.hit_position));
  }

  public Vector2 getAttackTargetPosition(AttackData pData)
  {
    BaseSimObject target = pData.target;
    Vector2 attackTargetPosition;
    // ISSUE: explicit constructor call
    ((Vector2) ref attackTargetPosition).\u002Ector(pData.hit_position.x, pData.hit_position.y);
    if (target == null)
      return attackTargetPosition;
    float stat = target.stats["size"];
    if (target.isActor() && target.a.is_moving && target.isFlying())
      attackTargetPosition = Vector2.MoveTowards(attackTargetPosition, target.a.next_step_position, stat * 3f);
    return attackTargetPosition;
  }
}

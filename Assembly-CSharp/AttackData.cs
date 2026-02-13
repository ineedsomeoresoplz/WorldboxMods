// Decompiled with JetBrains decompiler
// Type: AttackData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public readonly struct AttackData
{
  public readonly BaseSimObject initiator;
  public readonly Action kill_action;
  public readonly Kingdom kingdom;
  public readonly WorldTile hit_tile;
  public readonly Vector3 hit_position;
  public readonly Vector3 initiator_position;
  public readonly BaseSimObject target;
  public readonly AttackType attack_type;
  public readonly bool skip_shake;
  public readonly bool metallic_weapon;
  public readonly bool critical;
  public readonly int targets;
  public readonly int critical_damage_multiplier;
  public readonly float area_of_effect;
  public readonly int damage;
  public readonly float damage_range;
  public readonly bool is_projectile;
  public readonly string projectile_id;
  public readonly float knockback;

  public AttackData(
    BaseSimObject pInitiator,
    WorldTile pHitTile,
    Vector3 pHitPosition,
    Vector3 pInitiatorPosition,
    BaseSimObject pTarget,
    Kingdom pKingdom,
    AttackType pAttackType = AttackType.Other,
    bool pMetallicWeapon = false,
    bool pSkipShake = true,
    bool pProjectile = false,
    string pProjectileID = "",
    Action pKillAction = null,
    float pBonusAreOfEffect = 0.0f)
  {
    bool flag = false;
    float num1 = 0.0f;
    int num2 = 1;
    float num3 = 0.1f;
    int num4 = 1;
    float num5 = 1f;
    float num6 = 1f;
    if (pInitiator != null)
    {
      flag = Randy.randomChance(pInitiator.stats["critical_chance"]);
      num1 = pInitiator.stats[nameof (knockback)];
      num2 = (int) pInitiator.stats[nameof (targets)];
      num3 = pInitiator.stats[nameof (area_of_effect)];
      num4 = (int) pInitiator.stats[nameof (damage)];
      num5 = pInitiator.stats[nameof (damage_range)];
      num6 = pInitiator.stats[nameof (critical_damage_multiplier)];
    }
    float num7 = num3 + pBonusAreOfEffect;
    this.kill_action = pKillAction;
    this.initiator = pInitiator;
    this.kingdom = pKingdom;
    this.hit_tile = pHitTile;
    this.initiator_position = pInitiatorPosition;
    this.hit_position = pHitPosition;
    this.target = pTarget;
    this.attack_type = pAttackType;
    this.metallic_weapon = pMetallicWeapon;
    this.skip_shake = pSkipShake;
    this.is_projectile = pProjectile;
    this.projectile_id = pProjectileID;
    this.targets = num2;
    this.critical = flag;
    this.knockback = num1;
    this.area_of_effect = num7;
    this.damage = num4;
    this.damage_range = num5;
    this.critical_damage_multiplier = (int) num6;
  }
}

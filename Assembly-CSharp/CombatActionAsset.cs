// Decompiled with JetBrains decompiler
// Type: CombatActionAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public class CombatActionAsset : Asset
{
  public bool play_unit_attack_sounds;
  public CombatActionPool[] pools;
  public string tag_required;
  public int rate = 1;
  public float chance = 0.2f;
  public float cooldown = 1f;
  public bool is_spell_use;
  public int cost_stamina;
  public int cost_mana;
  public bool basic;
  public CombatAction action;
  public CombatActionActor action_actor;
  public CombatActionActorTargetPosition action_actor_target_position;
  public CombatActionCheckStart can_do_action;
}

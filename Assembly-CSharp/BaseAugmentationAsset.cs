// Decompiled with JetBrains decompiler
// Type: BaseAugmentationAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
[Serializable]
public class BaseAugmentationAsset : BaseUnlockableAsset
{
  [DefaultValue(true)]
  public bool can_be_given = true;
  [DefaultValue(true)]
  public bool can_be_removed = true;
  [DefaultValue(true)]
  public bool show_in_meta_editor = true;
  public WorldActionTrait action_on_object_remove;
  public WorldAction action_special_effect;
  public WorldActionTrait action_on_augmentation_add;
  public WorldActionTrait action_on_augmentation_remove;
  public WorldActionTrait action_on_augmentation_load;
  [DefaultValue(1f)]
  public float special_effect_interval = 1f;
  public AttackAction action_attack_target;
  public string group_id;
  public int priority;
  public string special_locale_id;
  [NonSerialized]
  public CombatActionHolder combat_actions;
  [NonSerialized]
  public List<SpellAsset> spells;
  public List<string> combat_actions_ids;
  [NonSerialized]
  public DecisionAsset[] decisions_assets;
  public List<string> decision_ids;

  public List<string> spells_ids { get; set; }

  public bool hasDecisions() => this.decisions_assets != null;

  public bool hasCombatActions() => this.combat_actions != null;

  public bool hasSpells() => this.spells != null;

  public void addDecision(string pID)
  {
    if (this.decision_ids == null)
      this.decision_ids = new List<string>();
    this.decision_ids.Add(pID);
  }

  public void addSpell(string pSpell)
  {
    if (this.spells_ids == null)
      this.spells_ids = new List<string>();
    this.spells_ids.Add(pSpell);
  }

  public void addCombatAction(string pCombatActionID)
  {
    if (this.combat_actions_ids == null)
      this.combat_actions_ids = new List<string>();
    this.combat_actions_ids.Add(pCombatActionID);
  }

  public void linkCombatActions()
  {
    if (this.combat_actions_ids == null || this.combat_actions_ids.Count == 0)
      return;
    this.combat_actions = new CombatActionHolder();
    this.combat_actions.fillFromIDS(this.combat_actions_ids);
  }

  public void linkSpells()
  {
    if (this.spells_ids == null || this.spells_ids.Count == 0)
      return;
    this.spells = new List<SpellAsset>();
    foreach (string spellsId in this.spells_ids)
    {
      SpellAsset spellAsset = AssetManager.spells.get(spellsId);
      if (spellAsset != null)
        this.spells.Add(spellAsset);
    }
  }

  public virtual BaseCategoryAsset getGroup() => throw new NotImplementedException();
}

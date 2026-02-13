// Decompiled with JetBrains decompiler
// Type: SpellAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class SpellAsset : Asset
{
  public float health_ratio;
  public int cost_mana;
  public float chance = 1f;
  public float min_distance;
  public CastTarget cast_target;
  public CastEntity cast_entity = CastEntity.Both;
  public AttackAction action;
  public List<string> decision_ids;
  [NonSerialized]
  public DecisionAsset[] decisions_assets;
  public bool can_be_used_in_combat;

  public bool hasDecisions() => this.decisions_assets != null;

  public void addDecision(string pID)
  {
    if (this.decision_ids == null)
      this.decision_ids = new List<string>();
    this.decision_ids.Add(pID);
  }
}

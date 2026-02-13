// Decompiled with JetBrains decompiler
// Type: ProfessionAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class ProfessionAsset : Asset
{
  public UnitProfession profession_id;
  public bool can_capture;
  public bool is_civilian;
  public bool cancel_when_no_city;
  public List<string> decision_ids;
  [NonSerialized]
  public DecisionAsset[] decisions_assets;

  public bool hasDecisions() => this.decision_ids != null;

  public bool hasDecision(string pID) => this.decision_ids.Contains(pID);

  public void linkDecisions()
  {
    if (this.decision_ids == null)
      return;
    this.decisions_assets = new DecisionAsset[this.decision_ids.Count];
    for (int index = 0; index < this.decision_ids.Count; ++index)
    {
      string decisionId = this.decision_ids[index];
      DecisionAsset decisionAsset = AssetManager.decisions_library.get(decisionId);
      this.decisions_assets[index] = decisionAsset;
    }
  }

  public void addDecision(string pID)
  {
    if (this.decision_ids == null)
      this.decision_ids = new List<string>();
    this.decision_ids.Add(pID);
  }
}

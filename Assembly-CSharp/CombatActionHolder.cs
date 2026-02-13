// Decompiled with JetBrains decompiler
// Type: CombatActionHolder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class CombatActionHolder
{
  private readonly List<CombatActionAsset>[] _combat_action_pools = new List<CombatActionAsset>[Enum.GetValues(typeof (CombatActionPool)).Length];
  private bool _has_combat_actions;

  public void fillFromIDS(List<string> pIDs)
  {
    foreach (string pId in pIDs)
    {
      CombatActionAsset combatActionAsset = AssetManager.combat_action_library.get(pId);
      if (combatActionAsset != null)
      {
        foreach (CombatActionPool pool in combatActionAsset.pools)
        {
          if (this._combat_action_pools[(int) pool] == null)
            this._combat_action_pools[(int) pool] = new List<CombatActionAsset>();
          this._combat_action_pools[(int) pool].Add(combatActionAsset);
        }
      }
    }
  }

  public List<CombatActionAsset> getPool(CombatActionPool pPool)
  {
    return this._combat_action_pools[(int) pPool];
  }

  public void reset()
  {
    if (!this._has_combat_actions)
      return;
    for (int index = 0; index < this._combat_action_pools.Length; ++index)
      this._combat_action_pools[index]?.Clear();
    this._has_combat_actions = false;
  }

  public void mergeWith(CombatActionHolder pCombatActions)
  {
    for (int index = 0; index < pCombatActions._combat_action_pools.Length; ++index)
    {
      List<CombatActionAsset> combatActionPool = pCombatActions._combat_action_pools[index];
      if (combatActionPool != null && combatActionPool.Count != 0)
      {
        if (this._combat_action_pools[index] == null)
          this._combat_action_pools[index] = new List<CombatActionAsset>();
        this._combat_action_pools[index].AddRange((IEnumerable<CombatActionAsset>) combatActionPool);
        this._has_combat_actions = true;
      }
    }
  }

  public bool isEmpty() => !this._has_combat_actions;

  public bool hasAny() => this._has_combat_actions;
}

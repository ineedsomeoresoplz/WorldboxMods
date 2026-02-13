// Decompiled with JetBrains decompiler
// Type: CrabLimbGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class CrabLimbGroup
{
  public CrabLimb crabLimb;
  private CrabLimbItem[] _list;
  private CrabLimbState _dmg_state;
  private Actor actor;
  private float _flicker_timer;
  private const float _flicker_interval = 0.15f;

  public CrabLimbGroup(CrabLimb pCrabLimb, Actor pActor)
  {
    this.actor = pActor;
    this.crabLimb = pCrabLimb;
    List<CrabLimbItem> crabLimbItemList = new List<CrabLimbItem>();
    foreach (CrabLimbItem componentsInChild in this.actor.avatar.GetComponentsInChildren<CrabLimbItem>(false))
    {
      if (componentsInChild.crabLimb == this.crabLimb)
        crabLimbItemList.Add(componentsInChild);
    }
    this._list = crabLimbItemList.ToArray();
    this._dmg_state = CrabLimbState.HighHP;
  }

  internal void update(float pElapsed)
  {
    if ((double) this._flicker_timer == 0.0)
      return;
    this._flicker_timer -= pElapsed;
    if ((double) this._flicker_timer < 0.0)
      this._flicker_timer = 0.0f;
    float pProgress = (float) (1.0 - (double) this._flicker_timer / 0.15000000596046448);
    foreach (CrabLimbItem crabLimbItem in this._list)
      crabLimbItem.flicker(pProgress);
  }

  internal void showDamage()
  {
    if (this.IsFlickering())
      return;
    int health = this.actor.getHealth();
    int maxHealth = this.actor.getMaxHealth();
    if ((double) health > (double) maxHealth * 0.699999988079071)
    {
      if (this._dmg_state == CrabLimbState.HighHP)
        return;
      this._dmg_state = CrabLimbState.HighHP;
    }
    else if ((double) health > (double) maxHealth * 0.34999999403953552)
    {
      if (this._dmg_state == CrabLimbState.MedHP)
        return;
      this._dmg_state = CrabLimbState.MedHP;
    }
    else
    {
      if (this._dmg_state == CrabLimbState.LowHP)
        return;
      this._dmg_state = CrabLimbState.LowHP;
    }
    foreach (CrabLimbItem crabLimbItem in this._list)
      crabLimbItem.stateChange(this._dmg_state);
    this._flicker_timer = 0.15f;
  }

  internal bool IsFlickering() => (double) this._flicker_timer > 0.0;
}

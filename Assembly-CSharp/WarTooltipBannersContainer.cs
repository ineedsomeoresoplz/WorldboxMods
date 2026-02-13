// Decompiled with JetBrains decompiler
// Type: WarTooltipBannersContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class WarTooltipBannersContainer : MonoBehaviour
{
  [SerializeField]
  private KingdomBanner _banner_left;
  [SerializeField]
  private KingdomBanner _banner_right;
  [SerializeField]
  private Image _total_war;

  public void load(War pWar)
  {
    ((Component) this._banner_right).gameObject.SetActive(false);
    ((Component) this._banner_left).gameObject.SetActive(false);
    ((Component) this._total_war).gameObject.SetActive(false);
    Kingdom mainAttacker = pWar.main_attacker;
    if (!mainAttacker.isRekt())
    {
      ((Component) this._banner_left).gameObject.SetActive(true);
      this._banner_left.load((NanoObject) mainAttacker);
    }
    if (pWar.isTotalWar())
    {
      ((Component) this._total_war).gameObject.SetActive(true);
    }
    else
    {
      Kingdom mainDefender = pWar.getMainDefender();
      if (mainDefender.isRekt())
        return;
      ((Component) this._banner_right).gameObject.SetActive(true);
      this._banner_right.load((NanoObject) mainDefender);
    }
  }
}

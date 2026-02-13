// Decompiled with JetBrains decompiler
// Type: UnitLoverElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class UnitLoverElement : UnitElement
{
  [SerializeField]
  private PrefabUnitElement _lover_element;
  [SerializeField]
  private GameObject _lover_title;

  protected override IEnumerator showContent()
  {
    UnitLoverElement unitLoverElement = this;
    if (unitLoverElement.actor.hasLover() && !unitLoverElement.actor.lover.isRekt())
    {
      unitLoverElement.track_objects.Add((NanoObject) unitLoverElement.actor.lover);
      unitLoverElement._lover_element.show(unitLoverElement.actor.lover);
      unitLoverElement._lover_title.SetActive(true);
      yield return (object) new WaitForSecondsRealtime(0.025f);
      ((Component) unitLoverElement._lover_element).gameObject.SetActive(true);
    }
  }

  protected override void clear()
  {
    this._lover_title.SetActive(false);
    ((Component) this._lover_element).gameObject.SetActive(false);
    base.clear();
  }

  public override bool checkRefreshWindow()
  {
    return ((Component) this._lover_element).gameObject.activeSelf && (!this.actor.hasLover() || this.actor.lover.isRekt()) || base.checkRefreshWindow();
  }
}

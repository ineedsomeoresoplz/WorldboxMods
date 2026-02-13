// Decompiled with JetBrains decompiler
// Type: CityLoyaltyElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class CityLoyaltyElement : MonoBehaviour
{
  private City _city;
  private TooltipData _tooltip_data;

  public void setCity(City pCity) => this._city = pCity;

  private void Start()
  {
    Button component = ((Component) this).GetComponent<Button>();
    // ISSUE: method pointer
    ((UnityEvent) component.onClick).AddListener(new UnityAction((object) this, __methodptr(showTooltip)));
    // ISSUE: method pointer
    component.OnHover(new UnityAction((object) this, __methodptr(showHoverTooltip)));
    // ISSUE: method pointer
    component.OnHoverOut(new UnityAction((object) null, __methodptr(hideTooltip)));
  }

  private void showHoverTooltip()
  {
    if (!Config.tooltips_active)
      return;
    this.showTooltip();
  }

  private void showTooltip()
  {
    this._tooltip_data = new TooltipData()
    {
      city = this._city
    };
    Tooltip.show((object) ((Component) this).gameObject, "loyalty", this._tooltip_data);
  }
}

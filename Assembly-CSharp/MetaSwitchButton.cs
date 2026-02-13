// Decompiled with JetBrains decompiler
// Type: MetaSwitchButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class MetaSwitchButton : 
  MonoBehaviour,
  IPointerEnterHandler,
  IEventSystemHandler,
  IPointerExitHandler
{
  public Button button;
  public Text meta_name;
  public Transform banner_parent;
  private IBanner _banner;
  private MultiBannerPool _pool;
  private MetaSwitchManager.Direction _direction;

  public void init(MetaSwitchManager.Direction pDirection, SwitchWindowsAction pAction)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    MetaSwitchButton.\u003C\u003Ec__DisplayClass6_0 cDisplayClass60 = new MetaSwitchButton.\u003C\u003Ec__DisplayClass6_0();
    // ISSUE: reference to a compiler-generated field
    cDisplayClass60.pAction = pAction;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass60.\u003C\u003E4__this = this;
    this._direction = pDirection;
    this._pool = new MultiBannerPool(this.banner_parent);
    // ISSUE: method pointer
    ((UnityEvent) this.button.onClick).AddListener(new UnityAction((object) cDisplayClass60, __methodptr(\u003Cinit\u003Eb__0)));
  }

  public void setBanner(IBanner pBanner) => this._banner = pBanner;

  public MultiBannerPool getPool() => this._pool;

  public void clear() => this._pool.clear();

  public void OnPointerEnter(PointerEventData pEventData)
  {
    if (!InputHelpers.mouseSupported)
      return;
    this.showTooltip();
  }

  public void OnPointerExit(PointerEventData pEventData)
  {
    if (!InputHelpers.mouseSupported)
      return;
    Tooltip.hideTooltip();
  }

  private void showTooltip()
  {
    if (!MetaSwitchManager.isSwitcherEnabled())
      return;
    this._banner.meta_type_asset.stat_hover(this._banner.GetNanoObject().getID(), (MonoBehaviour) this);
  }
}

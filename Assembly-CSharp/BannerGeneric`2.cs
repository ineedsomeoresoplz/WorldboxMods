// Decompiled with JetBrains decompiler
// Type: BannerGeneric`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public abstract class BannerGeneric<TMetaObject, TData> : BannerBase
  where TMetaObject : CoreSystemObject<TData>
  where TData : BaseSystemData
{
  protected TMetaObject meta_object;
  private bool _created;
  protected Image part_background;
  protected Image part_icon;
  protected Image part_frame;
  public bool enable_default_click = true;
  [SerializeField]
  private bool _enable_customize_click;
  public bool enable_tab_show_click;

  protected TData data => this.meta_object.data;

  protected virtual string tooltip_id => throw new NotImplementedException();

  private void Start() => this.create();

  private void create()
  {
    if (this._created)
      return;
    this._created = true;
    this.setupParts();
    this.setupClick();
    this.setupTooltip();
  }

  protected virtual void setupClick()
  {
    if (!this.enable_default_click && !this._enable_customize_click && !this.enable_tab_show_click)
      return;
    Button componentInChildren = ((Component) this).GetComponentInChildren<Button>();
    if (Object.op_Equality((Object) componentInChildren, (Object) null))
      return;
    if (this.enable_default_click)
    {
      Button.ButtonClickedEvent onClick = componentInChildren.onClick;
      BannerGeneric<TMetaObject, TData> bannerGeneric = this;
      // ISSUE: virtual method pointer
      UnityAction unityAction = new UnityAction((object) bannerGeneric, __vmethodptr(bannerGeneric, clickAction));
      ((UnityEvent) onClick).AddListener(unityAction);
    }
    else if (this._enable_customize_click)
    {
      Button.ButtonClickedEvent onClick = componentInChildren.onClick;
      BannerGeneric<TMetaObject, TData> bannerGeneric = this;
      // ISSUE: virtual method pointer
      UnityAction unityAction = new UnityAction((object) bannerGeneric, __vmethodptr(bannerGeneric, clickCustomize));
      ((UnityEvent) onClick).AddListener(unityAction);
    }
    else
    {
      if (!this.enable_tab_show_click)
        return;
      // ISSUE: method pointer
      ((UnityEvent) componentInChildren.onClick).AddListener(new UnityAction((object) this, __methodptr(clickShowTab)));
    }
  }

  protected virtual void clickAction()
  {
    if (this.meta_object.hasDied())
      return;
    if (!InputHelpers.mouseSupported)
      this.switchOnDoubleTap();
    else
      this.showMetaWindow();
  }

  private void switchOnDoubleTap()
  {
    if (!Tooltip.isShowingFor((object) this))
      this.tooltipAction();
    else
      this.showMetaWindow();
  }

  private void showMetaWindow()
  {
    this.meta_type_asset.set_selected((NanoObject) this.meta_object);
    string windowName = this.meta_type_asset.window_name;
    if (ScrollWindow.isCurrentWindow(windowName))
      ScrollWindow.get(windowName).showSameWindow();
    else
      ScrollWindow.showWindow(windowName);
  }

  protected virtual void clickCustomize()
  {
    string customizeWindowId = this.meta_asset.customize_window_id;
    if (customizeWindowId == string.Empty)
      Debug.LogError((object) $"var {customizeWindowId} is not set!");
    else
      ScrollWindow.showWindow(customizeWindowId);
  }

  private void clickShowTab()
  {
    if (HotkeyLibrary.isHoldingControlForSelection())
    {
      this.clickAction();
    }
    else
    {
      this.meta_type_asset.selectAndInspect((NanoObject) this.meta_object);
      Tooltip.blockTooltips(0.5f);
      Tooltip.hideTooltipNow();
    }
  }

  protected virtual void setupParts()
  {
    this.loadPartBackground();
    this.loadPartFrame();
    this.loadPartIcon();
  }

  protected virtual void loadPartFrame()
  {
    this.part_frame = ((Component) ((Component) this).transform.FindRecursive("Frame"))?.GetComponent<Image>();
  }

  protected virtual void loadPartBackground()
  {
    this.part_background = ((Component) ((Component) this).transform.FindRecursive("Background"))?.GetComponent<Image>();
  }

  protected virtual void loadPartIcon()
  {
    this.part_icon = ((Component) ((Component) this).transform.FindRecursive("Icon"))?.GetComponent<Image>();
  }

  protected virtual void setupBanner()
  {
  }

  public override void load(NanoObject pObject)
  {
    this.setMetaObject(pObject);
    this.create();
    this.setupBanner();
  }

  public override NanoObject GetNanoObject() => (NanoObject) this.meta_object;

  protected virtual void setupTooltip()
  {
    TipButton tipButton;
    if (!((Component) this).TryGetComponent<TipButton>(ref tipButton))
      return;
    tipButton.setHoverAction((TooltipAction) (() =>
    {
      if (!InputHelpers.mouseSupported)
        return;
      this.tooltipAction();
    }));
  }

  protected virtual void tooltipAction()
  {
    Tooltip.show((object) this, this.tooltip_id, this.getTooltipData());
  }

  protected virtual TooltipData getTooltipData()
  {
    return new TooltipData()
    {
      custom_data_bool = new CustomDataContainer<bool>()
      {
        ["tab_banner"] = this.enable_tab_show_click
      }
    };
  }

  public override void showTooltip() => this.tooltipAction();

  protected void setMetaObject(NanoObject pObject) => this.meta_object = (TMetaObject) pObject;

  internal virtual void normalize()
  {
    if (this.meta_asset.option_1_editable)
    {
      int num = this.meta_asset.option_1_count();
      if (this.option_1 < 0)
        this.option_1 += num;
      if (this.option_1 > num - 1)
        this.option_1 -= num;
      this.option_1 = Mathf.Clamp(this.option_1, 0, num - 1);
    }
    if (this.meta_asset.option_2_editable)
    {
      int num = this.meta_asset.option_2_count();
      if (this.option_2 < 0)
        this.option_2 += num;
      if (this.option_2 > num - 1)
        this.option_2 -= num;
      this.option_2 = Mathf.Clamp(this.option_2, 0, num - 1);
    }
    if (!this.meta_asset.color_editable)
      return;
    int num1 = this.meta_asset.color_count();
    if (this.color < 0)
      this.color += num1;
    if (this.color > num1 - 1)
      this.color -= num1;
    this.color = Mathf.Clamp(this.color, 0, num1 - 1);
  }

  internal virtual void updateColor()
  {
    if (!this.meta_object.updateColor(this.meta_asset.color_library().list[this.color]))
      return;
    this.meta_asset.on_new_color();
  }
}

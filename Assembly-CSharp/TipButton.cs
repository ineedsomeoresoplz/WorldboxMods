// Decompiled with JetBrains decompiler
// Type: TipButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
[DisallowMultipleComponent]
public class TipButton : MonoBehaviour
{
  private Vector3 _default_scale;
  private float _default_click_scale_increase = 1.1f;
  public const float SCALE_DURATION = 0.1f;
  public string textOnClick;
  public string textOnClickDescription;
  public string text_description_2;
  public string text_override_non_steam = string.Empty;
  public string description_override_non_steam = string.Empty;
  public TooltipAction hoverAction;
  public TooltipAction clickAction;
  public bool return_if_same_object;
  public string type = "tip";
  public bool showOnClick = true;
  public bool override_click_scale_animation;
  public float overridden_click_scale_animation = 1f;
  private Tweener _scale_anim;

  private void Awake()
  {
    if (this.hoverAction == null)
      this.setHoverAction(new TooltipAction(this.showTooltipDefault));
    this._default_scale = ((Component) this).gameObject.transform.localScale;
  }

  private void Start()
  {
    Button button;
    if (((Component) this).TryGetComponent<Button>(ref button))
    {
      if (this.showOnClick)
      {
        // ISSUE: method pointer
        ((UnityEvent) button.onClick).AddListener(new UnityAction((object) this, __methodptr(showTooltipOnClick)));
      }
      // ISSUE: method pointer
      button.OnHover(new UnityAction((object) this, __methodptr(showHoverTooltip)));
      // ISSUE: method pointer
      button.OnHoverOut(new UnityAction((object) null, __methodptr(hideTooltip)));
    }
    else
    {
      Slider component = ((Component) this).GetComponent<Slider>();
      if (!Object.op_Inequality((Object) component, (Object) null))
        return;
      // ISSUE: method pointer
      component.OnHover(new UnityAction((object) this, __methodptr(showHoverTooltip)));
      // ISSUE: method pointer
      component.OnHoverOut(new UnityAction((object) null, __methodptr(hideTooltip)));
    }
  }

  public void setDefaultScale(Vector3 pScale) => this._default_scale = pScale;

  public void setHoverAction(TooltipAction pAction, bool pAddAnimation = true)
  {
    this.hoverAction = pAction;
    if (!pAddAnimation)
      return;
    this.hoverAction += new TooltipAction(this.clickAnimation);
  }

  private void showTooltipOnClick()
  {
    if (this.clickAction != null)
    {
      this.clickAction();
    }
    else
    {
      TooltipAction hoverAction = this.hoverAction;
      if (hoverAction == null)
        return;
      hoverAction();
    }
  }

  private void showHoverTooltip()
  {
    if (!Config.tooltips_active)
      return;
    TooltipAction hoverAction = this.hoverAction;
    if (hoverAction == null)
      return;
    hoverAction();
  }

  public void showTooltipDefault()
  {
    if (Config.isMobile)
    {
      if (!string.IsNullOrEmpty(this.text_override_non_steam))
        this.textOnClick = this.text_override_non_steam;
      if (!string.IsNullOrEmpty(this.description_override_non_steam))
        this.textOnClickDescription = this.description_override_non_steam;
    }
    if (this.textOnClick == "" && this.textOnClickDescription == "")
      return;
    Tooltip.show((object) ((Component) this).gameObject, this.type, new TooltipData()
    {
      tip_name = this.textOnClick,
      tip_description = this.textOnClickDescription,
      tip_description_2 = this.text_description_2
    });
  }

  public void clickAnimation()
  {
    float num1 = this._default_click_scale_increase;
    if (this.override_click_scale_animation)
      num1 = this.overridden_click_scale_animation;
    float num2 = this._default_scale.x * num1;
    ((Component) this).transform.localScale = new Vector3(num2, num2, num2);
    TweenExtensions.Kill((Tween) this._scale_anim, false);
    this._scale_anim = (Tweener) TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOScale(((Component) this).transform, this._default_scale, 0.1f), (Ease) 26);
  }

  private void OnEnable() => this.resetAnimation();

  private void resetAnimation()
  {
    Tweener scaleAnim = this._scale_anim;
    if (scaleAnim != null)
      TweenExtensions.Kill((Tween) scaleAnim, false);
    ((Component) this).transform.localScale = this._default_scale;
  }

  private void OnDestroy() => TweenExtensions.Kill((Tween) this._scale_anim, false);
}

// Decompiled with JetBrains decompiler
// Type: ButtonResource
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
public class ButtonResource : MonoBehaviour
{
  public Text textAmount;
  public ResourceAsset asset;
  public static float scaleTime = 0.1f;

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

  internal void load(ResourceAsset pAsset, int pAmount)
  {
    this.asset = pAsset;
    if (this.asset == null)
      return;
    ((Component) this).GetComponent<Image>().sprite = pAsset.getSpriteIcon();
    this.textAmount.text = pAmount.ToString() ?? "";
  }

  private void showHoverTooltip()
  {
    if (!Config.tooltips_active)
      return;
    this.showTooltip();
  }

  private void showTooltip()
  {
    Tooltip.show((object) this, this.asset.tooltip, new TooltipData()
    {
      resource = this.asset
    });
    ((Component) this).transform.localScale = new Vector3(1f, 1f, 1f);
    ShortcutExtensions.DOKill((Component) ((Component) this).transform, false);
    TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOScale(((Component) this).transform, 0.8f, ButtonResource.scaleTime), (Ease) 26);
  }

  private void OnDestroy()
  {
    ShortcutExtensions.DOKill((Component) ((Component) this).transform, false);
  }
}

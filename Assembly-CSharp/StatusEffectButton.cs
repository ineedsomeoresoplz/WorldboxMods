// Decompiled with JetBrains decompiler
// Type: StatusEffectButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class StatusEffectButton : MonoBehaviour
{
  private Status _status;
  internal Image image;
  internal bool tooltip_enabled = true;
  internal Button button;
  private bool _updatable_tooltip;

  public Status status => this._status;

  private void Awake()
  {
    this.button = ((Component) this).GetComponent<Button>();
    this.image = ((Component) ((Component) this).transform.Find("icon")).GetComponent<Image>();
    DraggableLayoutElement draggableLayoutElement;
    if (!((Component) this).TryGetComponent<DraggableLayoutElement>(ref draggableLayoutElement))
      return;
    draggableLayoutElement.start_being_dragged += new Action<DraggableLayoutElement>(this.onStartDrag);
  }

  private void Start()
  {
    // ISSUE: method pointer
    ((UnityEvent) this.button.onClick).AddListener(new UnityAction((object) this, __methodptr(showTooltip)));
    // ISSUE: method pointer
    this.button.OnHover(new UnityAction((object) this, __methodptr(showHoverTooltip)));
    // ISSUE: method pointer
    this.button.OnHoverOut(new UnityAction((object) null, __methodptr(hideTooltip)));
  }

  internal void load(Status pData)
  {
    if (pData == null)
      return;
    this._status = pData;
    this.image.sprite = pData.asset.getSprite();
  }

  protected virtual void onStartDrag(DraggableLayoutElement pOriginalElement)
  {
    this.load(((Component) pOriginalElement).GetComponent<StatusEffectButton>()._status);
  }

  private void OnDisable() => Tooltip.hideTooltip();

  private void showHoverTooltip()
  {
    if (!Config.tooltips_active)
      return;
    this.showTooltip();
  }

  private void showTooltip()
  {
    if (!this.tooltip_enabled)
      return;
    string pType = this._updatable_tooltip ? "status_updatable" : "status";
    string localeId = this._status.asset.getLocaleID();
    string descriptionId = this._status.asset.getDescriptionID();
    Tooltip.show((object) this, pType, new TooltipData()
    {
      tip_name = localeId,
      tip_description = descriptionId,
      status = this._status
    });
    ((Component) this).transform.localScale = new Vector3(1f, 1f, 1f);
    ShortcutExtensions.DOKill((Component) ((Component) this).transform, false);
    TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOScale(((Component) this).transform, 0.8f, 0.1f), (Ease) 26);
  }

  public void setUpdatableTooltip(bool pState) => this._updatable_tooltip = pState;

  private void OnDestroy()
  {
    ShortcutExtensions.DOKill((Component) ((Component) this).transform, false);
  }
}

// Decompiled with JetBrains decompiler
// Type: OnomasticsAssetButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class OnomasticsAssetButton : MonoBehaviour
{
  private bool _created;
  internal Image image;
  internal bool tooltip_enabled = true;
  internal Button button;
  public OnomasticsAsset onomastics_asset;
  public OnomasticsActionUpdate onomastics_action_update;
  private GetCurrentOnomasticsData _get_current_onomastics_data;

  private void Awake()
  {
    this.create();
    DraggableLayoutElement draggableLayoutElement;
    if (!((Component) this).TryGetComponent<DraggableLayoutElement>(ref draggableLayoutElement))
      return;
    draggableLayoutElement.start_being_dragged += new Action<DraggableLayoutElement>(this.onStartDrag);
  }

  protected virtual void onStartDrag(DraggableLayoutElement pOriginalElement)
  {
    OnomasticsAssetButton component = ((Component) pOriginalElement).GetComponent<OnomasticsAssetButton>();
    this.setupButton(component.onomastics_asset, component._get_current_onomastics_data);
  }

  public void setupButton(OnomasticsAsset pAsset, GetCurrentOnomasticsData pDelegate)
  {
    this.loadAsset(pAsset);
    this.setOnomasticsGetter(pDelegate);
    this.checkSpriteButtonColor();
  }

  public RectTransform getRect() => ((Component) this).GetComponent<RectTransform>();

  private void Update() => this.checkSpriteButtonColor();

  public bool isGroupType() => this.onomastics_asset.isGroupType();

  private bool doesGroupHaveContent()
  {
    if (this._get_current_onomastics_data == null)
      return true;
    OnomasticsData onomasticsData = this._get_current_onomastics_data();
    if (onomasticsData == null || this.onomastics_asset == null)
      return false;
    return !this.isGroupType() || !onomasticsData.isGroupEmpty(this.onomastics_asset.id);
  }

  public void checkSpriteButtonColor()
  {
    if (this.doesGroupHaveContent())
      ((Graphic) this.image).color = Color.white;
    else
      ((Graphic) this.image).color = Color.gray;
  }

  public void setOnomasticsGetter(GetCurrentOnomasticsData pDelegate)
  {
    this._get_current_onomastics_data = pDelegate;
  }

  private void Start()
  {
    TipButton tipButton;
    if (!((Component) this).TryGetComponent<TipButton>(ref tipButton))
      return;
    tipButton.setHoverAction((TooltipAction) (() =>
    {
      if (!InputHelpers.mouseSupported)
        return;
      this.showTooltip();
    }));
  }

  public void loadAsset(OnomasticsAsset pAsset)
  {
    this.onomastics_asset = pAsset;
    this.image.sprite = this.onomastics_asset.getSprite();
  }

  public void showTooltip()
  {
    if (!this.tooltip_enabled)
      return;
    this.tooltipBuilder();
    ((Component) this).transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    ShortcutExtensions.DOKill((Component) ((Component) this).transform, false);
    TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOScale(((Component) this).transform, 1f, 0.1f), (Ease) 26);
  }

  private void tooltipBuilder()
  {
    Tooltip.show((object) this, "onomastics_asset", new TooltipData()
    {
      onomastics_asset = this.onomastics_asset,
      onomastics_data = this._get_current_onomastics_data()
    });
  }

  private void create()
  {
    if (this._created)
      return;
    this._created = true;
    this.button = ((Component) this).GetComponent<Button>();
    this.image = ((Component) ((Component) this).transform.Find("TiltEffect/icon")).GetComponent<Image>();
  }

  private void OnDestroy()
  {
    ShortcutExtensions.DOKill((Component) ((Component) this).transform, false);
  }
}

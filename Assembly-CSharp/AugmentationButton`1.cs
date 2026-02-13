// Decompiled with JetBrains decompiler
// Type: AugmentationButton`1
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
public class AugmentationButton<TAugmentation> : MonoBehaviour where TAugmentation : BaseAugmentationAsset
{
  [NonSerialized]
  public TAugmentation augmentation_asset;
  internal Image image;
  internal Image locked_bg;
  private IconOutline _outline;
  private Shadow _shadow;
  private bool _tooltip_enabled = true;
  internal Button button;
  internal bool is_editor_button;
  private AugmentationUnlockedAction _on_augmentation_unlocked;
  private AugmentationButtonClickAction _on_button_clicked;
  protected bool created;
  private bool _selected;

  public bool isSelected() => this._selected;

  protected virtual void Awake()
  {
    this.create();
    DraggableLayoutElement draggableLayoutElement;
    if (!((Component) this).TryGetComponent<DraggableLayoutElement>(ref draggableLayoutElement))
      return;
    draggableLayoutElement.start_being_dragged += new Action<DraggableLayoutElement>(this.onStartDrag);
  }

  protected virtual void onStartDrag(DraggableLayoutElement pOriginalElement)
  {
    AugmentationButton<TAugmentation> component = ((Component) pOriginalElement).GetComponent<AugmentationButton<TAugmentation>>();
    this.load(component.augmentation_asset);
    this.is_editor_button = component.is_editor_button;
  }

  protected virtual void create()
  {
    if (this.created)
      return;
    this.created = true;
    this.button = ((Component) this).GetComponent<Button>();
    this.image = ((Component) ((Component) this).transform.Find("TiltEffect/icon")).GetComponent<Image>();
    this.locked_bg = ((Component) ((Component) this).transform.Find("TiltEffect/locked_bg")).GetComponent<Image>();
    ((Component) this.locked_bg).gameObject.SetActive(false);
    this.initTooltip();
    this._outline = ((Component) ((Component) this).transform.FindRecursive("outline"))?.GetComponent<IconOutline>();
    this._shadow = ((Component) this.image).GetComponent<Shadow>();
    // ISSUE: method pointer
    ((UnityEvent) this.button.onClick).AddListener(new UnityAction((object) this, __methodptr(\u003Ccreate\u003Eb__15_0)));
  }

  public virtual void load(TAugmentation pElement) => throw new NotImplementedException();

  protected virtual void initTooltip()
  {
    TipButton tipButton;
    if (!((Component) this).TryGetComponent<TipButton>(ref tipButton))
      return;
    tipButton.setHoverAction(new TooltipAction(this.showTooltip));
  }

  protected virtual void Update() => throw new NotImplementedException();

  protected void loadLegendaryOutline()
  {
    ((Behaviour) this._shadow).enabled = true;
    if (Object.op_Equality((Object) this._outline, (Object) null))
      return;
    if (this.getRarity() == Rarity.R3_Legendary)
      this.showOutline(RarityLibrary.legendary.color_container);
    else
      ((Component) this._outline).gameObject.SetActive(false);
  }

  private void showOutline(ContainerItemColor pContainer)
  {
    if (Object.op_Equality((Object) this._outline, (Object) null))
      return;
    this._outline.show(pContainer);
    ((Behaviour) this._shadow).enabled = false;
  }

  public void showTooltip()
  {
    if (!this._tooltip_enabled)
      return;
    if (!this.is_editor_button && !this.augmentation_asset.unlocked_with_achievement && !this.isElementUnlocked() && !WorldLawLibrary.world_law_cursed_world.isEnabled() && this.unlockElement())
    {
      this.startSignal();
      AugmentationUnlockedAction augmentationUnlocked = this._on_augmentation_unlocked;
      if (augmentationUnlocked != null)
        augmentationUnlocked();
    }
    if (!this.is_editor_button || InputHelpers.mouseSupported || !Tooltip.isShowingFor((object) this))
      this.fillTooltipData(this.augmentation_asset);
    ((Component) this).transform.localScale = new Vector3(1f, 1f, 1f);
    ShortcutExtensions.DOKill((Component) ((Component) this).transform, false);
    TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOScale(((Component) this).transform, 0.8f, 0.1f), (Ease) 26);
  }

  public void addElementUnlockedAction(AugmentationUnlockedAction pAction)
  {
    this._on_augmentation_unlocked += pAction;
  }

  public void removeElementUnlockedAction(AugmentationUnlockedAction pAction)
  {
    this._on_augmentation_unlocked -= pAction;
  }

  protected virtual void clearActions()
  {
    this._on_augmentation_unlocked = (AugmentationUnlockedAction) null;
    this.clearClickActions();
  }

  public virtual void updateIconColor(bool pSelected)
  {
    this._selected = pSelected;
    if (!this.is_editor_button)
      return;
    if (!this.getElementAsset().isAvailable())
      ((Graphic) this.image).color = Toolbox.color_black;
    else if (pSelected)
      ((Graphic) this.image).color = Toolbox.color_augmentation_selected;
    else
      ((Graphic) this.image).color = Toolbox.color_augmentation_unselected;
  }

  public TAugmentation getElementAsset() => this.augmentation_asset;

  protected bool isElementUnlocked() => this.augmentation_asset.isAvailable();

  protected virtual bool unlockElement()
  {
    throw new NotImplementedException(((object) this).GetType().Name);
  }

  protected virtual void startSignal()
  {
  }

  protected virtual void fillTooltipData(TAugmentation pElement)
  {
    throw new NotImplementedException(((object) this).GetType().Name);
  }

  protected virtual string tooltip_type
  {
    get => throw new NotImplementedException(((object) this).GetType().Name);
  }

  protected virtual TooltipData tooltipDataBuilder()
  {
    throw new NotImplementedException(((object) this).GetType().Name);
  }

  protected virtual string getElementType()
  {
    throw new NotImplementedException(((object) this).GetType().Name);
  }

  public virtual string getElementId()
  {
    throw new NotImplementedException(((object) this).GetType().Name);
  }

  protected virtual Rarity getRarity()
  {
    throw new NotImplementedException(((object) this).GetType().Name);
  }

  protected virtual void disableTooltip() => this._tooltip_enabled = false;

  public void addClickAction(AugmentationButtonClickAction pAction)
  {
    this._on_button_clicked += pAction;
  }

  public void removeClickAction(AugmentationButtonClickAction pAction)
  {
    this._on_button_clicked -= pAction;
  }

  private void clearClickActions()
  {
    this._on_button_clicked = (AugmentationButtonClickAction) null;
  }

  private void OnDestroy()
  {
    ShortcutExtensions.DOKill((Component) ((Component) this).transform, false);
  }
}

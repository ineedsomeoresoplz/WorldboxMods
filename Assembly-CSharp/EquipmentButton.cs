// Decompiled with JetBrains decompiler
// Type: EquipmentButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class EquipmentButton : 
  AugmentationButton<EquipmentAsset>,
  IBanner,
  IBaseMono,
  IRefreshElement
{
  [SerializeField]
  private Image _favorited_icon;
  private Item _item;
  private bool _object_button;

  public MetaCustomizationAsset meta_asset
  {
    get => AssetManager.meta_customization_library.getAsset(MetaType.Item);
  }

  public MetaTypeAsset meta_type_asset => AssetManager.meta_type_library.getAsset(MetaType.Item);

  protected override void Update()
  {
    if (this.is_editor_button)
      return;
    if (this.augmentation_asset.unlocked_with_achievement)
      ((Component) this.locked_bg).gameObject.SetActive(false);
    else
      ((Component) this.locked_bg).gameObject.SetActive(!this.augmentation_asset.isAvailable() && this._object_button);
  }

  protected override void onStartDrag(DraggableLayoutElement pOriginalElement)
  {
    EquipmentEditorButton component1 = ((Component) pOriginalElement).GetComponent<EquipmentEditorButton>();
    if (Object.op_Inequality((Object) component1, (Object) null))
    {
      this.load(component1.augmentation_button.augmentation_asset);
      this.is_editor_button = component1.augmentation_button.is_editor_button;
    }
    else
    {
      EquipmentButton component2 = ((Component) pOriginalElement).GetComponent<EquipmentButton>();
      if (component2._object_button)
      {
        this.load(component2._item);
        this.is_editor_button = component2.is_editor_button;
      }
      else
      {
        this.load(component2.augmentation_asset);
        this.is_editor_button = component2.is_editor_button;
      }
    }
  }

  public void load(NanoObject pObject) => this.load((Item) pObject);

  internal void load(Item pItem)
  {
    this._object_button = true;
    this.create();
    this._item = pItem;
    this.augmentation_asset = this._item.getAsset();
    if (this.augmentation_asset == null)
      return;
    this.image.sprite = this._item.getSprite();
    this.loadLegendaryOutline();
    ((Object) ((Component) this).gameObject).name = $"{this.getElementType()}_{this._item.data.asset_id}";
    ((Component) this._favorited_icon).gameObject.SetActive(this._item.isFavorite());
  }

  public override void load(EquipmentAsset pItem)
  {
    this._object_button = false;
    this.create();
    this.augmentation_asset = pItem;
    if (this.augmentation_asset == null)
      return;
    this.image.sprite = this.augmentation_asset.getSprite();
    ((Object) ((Component) this).gameObject).name = $"{this.getElementType()}_{this.augmentation_asset.id}";
    ((Component) this._favorited_icon).gameObject.SetActive(false);
  }

  protected override void initTooltip()
  {
    base.initTooltip();
    TipButton tipButton;
    if (!((Component) this).TryGetComponent<TipButton>(ref tipButton))
      return;
    tipButton.clickAction += (TooltipAction) (() =>
    {
      if (InputHelpers.mouseSupported)
        this.openItemWindow();
      else if (Tooltip.isShowingFor((object) this) && !this.is_editor_button)
        this.openItemWindow();
      else
        this.showTooltip();
    });
  }

  private void openItemWindow()
  {
    SelectedMetas.selected_item = this._item;
    if (SelectedMetas.selected_item == null)
      return;
    ScrollWindow.showWindow("item");
  }

  protected override void fillTooltipData(EquipmentAsset pElement)
  {
    Tooltip.show((object) this, this.is_editor_button || !this._object_button ? "equipment_in_editor" : "equipment", this.tooltipDataBuilder());
  }

  protected override bool unlockElement() => this.augmentation_asset.unlock(true);

  protected override TooltipData tooltipDataBuilder()
  {
    if (!this.is_editor_button && this._object_button)
      return new TooltipData() { item = this._item };
    return new TooltipData()
    {
      item_asset = this.augmentation_asset
    };
  }

  protected override string tooltip_type => "equipment";

  protected override string getElementType() => "equip";

  protected override void startSignal() => AchievementLibrary.equipment_explorer.checkBySignal();

  public override string getElementId() => this.getElementAsset().id;

  private bool hasDivineRune()
  {
    return this._item != null && this._item.isAlive() && this._item.hasMod("divine_rune");
  }

  protected override Rarity getRarity() => this._item.getQuality();

  public string getName() => this._item.getName();

  public NanoObject GetNanoObject() => (NanoObject) this._item;

  Transform IBaseMono.get_transform() => ((Component) this).transform;

  GameObject IBaseMono.get_gameObject() => ((Component) this).gameObject;

  T IBaseMono.GetComponent<T>() => ((Component) this).GetComponent<T>();
}

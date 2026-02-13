// Decompiled with JetBrains decompiler
// Type: ItemWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class ItemWindow : WindowMetaGeneric<Item, ItemData>
{
  [SerializeField]
  private EquipmentBanner _item_banner;
  [SerializeField]
  private Text _text_item_type;
  [SerializeField]
  private Text _text_item_description;
  [SerializeField]
  private SwitchButton _button_cursed;
  [SerializeField]
  private SwitchButton _button_eternal;
  [SerializeField]
  private Sprite _frame_sprite_legendary;
  [SerializeField]
  private Sprite _frame_sprite_epic;
  private IconOutline _outline;

  public override MetaType meta_type => MetaType.Item;

  protected override Item meta_object => SelectedMetas.selected_item;

  public void clickReforge()
  {
    this.meta_object.reforge(1);
    this.meta_object.addMod("divine_rune");
    this.updateStates();
  }

  public void clickReforgeDivine()
  {
    this.meta_object.reforge(30);
    this.meta_object.addMod("divine_rune");
    this.updateStates();
  }

  public void clickCursed()
  {
    if (this.meta_object.hasMod("cursed"))
      this.meta_object.removeMod("cursed");
    else
      this.meta_object.addMod("cursed");
    this.meta_object.addMod("divine_rune");
    this.updateStates();
  }

  public void clickEternal()
  {
    if (this.meta_object.hasMod("eternal"))
      this.meta_object.removeMod("eternal");
    else
      this.meta_object.addMod("eternal");
    this.meta_object.addMod("divine_rune");
    this.updateStates();
  }

  public void clickTransmutation()
  {
    this.meta_object.transmute();
    this.meta_object.addMod("divine_rune");
    this.updateStates();
  }

  private void updateStates()
  {
    this.showTopPartInformation();
    this.loadNameInput();
    this.updateStatsRows();
    AchievementLibrary.godly_smithing.check();
    Actor actor = this.meta_object.getActor();
    if (actor.isRekt())
      return;
    actor.setStatsDirty();
  }

  protected override void showTopPartInformation()
  {
    base.showTopPartInformation();
    this.clear();
    Item metaObject = this.meta_object;
    this._item_banner.load((NanoObject) metaObject);
    EquipmentAsset asset = this.meta_object.getAsset();
    string str = "";
    if (asset.material != "basic")
      str = $"{str}({LocalizedTextManager.getText(asset.getMaterialID())}) ";
    ((Component) this._text_item_type).GetComponent<LocalizedText>().setKeyAndUpdate(metaObject.getItemKeyType());
    this._text_item_type.text = str + this._text_item_type.text;
    ((Graphic) this._text_item_type).color = Toolbox.makeColor(this.meta_object.getQualityColor());
    this._text_item_description.text = metaObject.getItemDescription();
    this._button_cursed.setEnabled(this.meta_object.hasMod("cursed"));
    this._button_eternal.setEnabled(this.meta_object.hasMod("eternal"));
  }

  internal override void showStatsRows()
  {
    Item metaObject = this.meta_object;
    BaseStatsHelper.showItemModsRows(new BaseStatsHelper.KeyValueFieldGetter(((StatsWindow) this).getStatRow), metaObject);
    BaseStatsHelper.showBaseStatsRows(new BaseStatsHelper.KeyValueFieldGetter(((StatsWindow) this).getStatRow), metaObject.getFullStats());
    this.showStatRow("durability", (object) metaObject.getDurabilityString());
    if (metaObject.data.kills <= 0)
      return;
    this.showStatRow("creature_statistics_kills", (object) metaObject.data.kills);
  }

  private void showOutline() => this._outline.show(RarityLibrary.legendary.color_container);

  protected override void loadNameInput()
  {
    // ISSUE: method pointer
    ((UnityEvent<string>) this._name_input.inputField.onEndEdit).AddListener(new UnityAction<string>((object) this, __methodptr(\u003CloadNameInput\u003Eb__21_0)));
    string pText = this.meta_object.getName(false).Trim();
    this._initial_name = pText;
    this._name_input.setText(pText);
    ((Graphic) this._name_input.textField).color = Toolbox.makeColor(this.meta_object.getQualityColor());
    if (!this.meta_object.data.custom_name)
      return;
    this._name_input.SetOutline();
  }
}

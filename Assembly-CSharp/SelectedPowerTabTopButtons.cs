// Decompiled with JetBrains decompiler
// Type: SelectedPowerTabTopButtons
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class SelectedPowerTabTopButtons : MonoBehaviour
{
  [SerializeField]
  private Image _favorite_icon;
  [SerializeField]
  private GameObject _button_possession;
  [SerializeField]
  private GameObject _button_spectate;
  [SerializeField]
  private GameObject _button_open_window;
  [SerializeField]
  private GameObject _button_favorite;
  [SerializeField]
  private UiMover _button_tab_back;
  [SerializeField]
  private Image _button_tab_back_icon;
  [SerializeField]
  private Text _button_tab_back_counter;

  private void Update()
  {
    if (SelectedUnit.isSet() || SelectedObjects.isNanoObjectSet())
    {
      this._button_open_window.SetActive(Screen.width < Screen.height);
      bool pVisible = SelectedTabsHistory.count() > 1;
      this._button_tab_back.setVisible(pVisible);
      if (!pVisible)
        return;
      this.updateTabBackButton();
    }
    else
    {
      this._button_open_window.SetActive(false);
      this._button_tab_back.setVisible(false);
    }
  }

  private void updateTabBackButton()
  {
    TabHistoryData? prevData = SelectedTabsHistory.getPrevData();
    if (!prevData.HasValue)
      return;
    MetaTypeAsset asset = AssetManager.meta_type_library.getAsset(prevData.Value.meta_type);
    NanoObject nanoObject = prevData.Value.getNanoObject();
    this._button_tab_back_icon.sprite = nanoObject.getMetaType() != MetaType.Unit ? asset.getIconSprite() : ((Actor) nanoObject).asset.getSpriteIcon();
    this._button_tab_back_counter.text = (SelectedTabsHistory.count() - 1).ToString();
  }

  public void clickButtonFavorite()
  {
    if (!SelectedObjects.isNanoObjectSet())
      return;
    if (SelectedUnit.isSet())
      this.clickFavoriteUnit();
    else
      this.clickFavoriteMeta();
  }

  public void clickLocate()
  {
  }

  public void clickOpenMain()
  {
    PowerTabAsset powerTabAsset = this.getPowerTabAsset();
    powerTabAsset.on_main_info_click(powerTabAsset);
  }

  private void clickFavoriteMeta()
  {
    ICoreObject coreObject = this.getPowerTabAsset().meta_type.getAsset().get_selected() as ICoreObject;
    coreObject.switchFavorite();
    if (coreObject.isFavorite())
      WorldTip.showNowTop("tip_favorite_icon");
    this.updateFavoriteIcon(coreObject.isFavorite());
  }

  private void clickFavoriteUnit()
  {
    Actor actor = (Actor) this.getPowerTabAsset().meta_type.getAsset().get_selected();
    actor.switchFavorite();
    if (actor.isFavorite())
      WorldTip.showNowTop("tip_favorite_icon");
    this.updateFavoriteIcon(actor.isFavorite());
  }

  private void updateFavoriteIcon(bool pStatus)
  {
    if (pStatus)
      ((Graphic) this._favorite_icon).color = ColorStyleLibrary.m.favorite_selected;
    else
      ((Graphic) this._favorite_icon).color = ColorStyleLibrary.m.favorite_not_selected;
  }

  private PowerTabAsset getPowerTabAsset() => PowersTab.getActiveTab().getAsset();
}

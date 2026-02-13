// Decompiled with JetBrains decompiler
// Type: SaveWindowIcons
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Steamworks.Ugc;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class SaveWindowIcons : MonoBehaviour
{
  [SerializeField]
  private bool _use_current_world_info;
  [SerializeField]
  private bool _allow_edit;
  [SerializeField]
  private bool _save_meta_data_on_close;
  [SerializeField]
  private bool _save_names_to_current_world;
  [SerializeField]
  private bool _clear_name_on_load;
  [SerializeField]
  private GameObject _icon_orc;
  [SerializeField]
  private GameObject _icon_human;
  [SerializeField]
  private GameObject _icon_elf;
  [SerializeField]
  private GameObject _icon_dwarf;
  [SerializeField]
  private Text _text_map_size;
  [SerializeField]
  private StatsIcon _map_age;
  [SerializeField]
  private StatsIcon _population;
  [SerializeField]
  private StatsIcon _mobs;
  [SerializeField]
  private StatsIcon _vegetation;
  [SerializeField]
  private StatsIcon _deaths;
  [SerializeField]
  private StatsIcon _kingdoms;
  [SerializeField]
  private StatsIcon _cities;
  [SerializeField]
  private StatsIcon _buildings;
  [SerializeField]
  private StatsIcon _equipment;
  [SerializeField]
  private StatsIcon _books;
  [SerializeField]
  private StatsIcon _wars;
  [SerializeField]
  private StatsIcon _alliances;
  [SerializeField]
  private StatsIcon _families;
  [SerializeField]
  private StatsIcon _clans;
  [SerializeField]
  private StatsIcon _cultures;
  [SerializeField]
  private StatsIcon _religions;
  [SerializeField]
  private StatsIcon _languages;
  [SerializeField]
  private StatsIcon _subspecies;
  [SerializeField]
  private StatsIcon _favorites;
  [SerializeField]
  private StatsIcon _favorite_items;
  [SerializeField]
  private GameObject _map_background_normal;
  [SerializeField]
  private GameObject _map_background_cursed;
  [SerializeField]
  private GameObject _map_overlay_cursed;
  [SerializeField]
  private GameObject _modded_icon;
  [SerializeField]
  private GameObject _cursed_icon;
  [SerializeField]
  private Text _map_name;
  [SerializeField]
  private Text _text_description;
  [SerializeField]
  private NameInput _name_input;
  [SerializeField]
  private NameInput _description_input;
  private string _save_path;
  private MapMetaData metaData;

  public void Awake()
  {
    // ISSUE: method pointer
    this._name_input.addListener(new UnityAction<string>((object) this, __methodptr(applyInputName)));
    // ISSUE: method pointer
    this._description_input.addListener(new UnityAction<string>((object) this, __methodptr(applyInputDescription)));
  }

  private void OnEnable()
  {
    if (!Config.game_loaded)
      return;
    ((Component) this._name_input).gameObject.SetActive(this._allow_edit);
    ((Component) this._description_input).gameObject.SetActive(this._allow_edit);
    if (this._use_current_world_info)
      this.metaData = SaveManager.currentWorldToSavedMap().getMeta();
    else if (SaveManager.currentWorkshopMapData != null)
    {
      this.metaData = SaveManager.currentWorkshopMapData.meta_data_map;
    }
    else
    {
      this.metaData = SaveManager.getCurrentMeta();
      this._save_path = SaveManager.currentSavePath;
    }
    if (this.metaData != null)
    {
      this.checkRaceIcons(this.metaData);
      MapStats mapStats = this.metaData.mapStats;
      if (this._allow_edit)
      {
        if (!this._clear_name_on_load)
          this._name_input.setText(mapStats.name);
        else
          this._name_input.setText("");
        this._description_input.setText(mapStats.description);
      }
      MapSizeAsset presetAsset = MapSizeLibrary.getPresetAsset(this.metaData.width);
      if (presetAsset != null)
        ((Component) this._text_map_size).GetComponent<LocalizedText>().setKeyAndUpdate(presetAsset.getLocaleID());
      else
        this._text_map_size.text = $"{this.metaData.width.ToString()}x{this.metaData.height.ToString()}";
      this._modded_icon.SetActive(this.metaData.modded);
      bool cursed = this.metaData.cursed;
      this._cursed_icon.SetActive(cursed);
      this._map_background_cursed.SetActive(cursed);
      this._map_overlay_cursed.SetActive(cursed);
      this._map_background_normal.SetActive(!cursed);
      this._map_age.setValue((float) Date.getYear(mapStats.world_time));
      this._population.setValue((float) this.metaData.population);
      this._mobs.setValue((float) this.metaData.mobs);
      this._vegetation.setValue((float) this.metaData.vegetation);
      this._deaths.setValue((float) this.metaData.deaths);
      this._kingdoms.setValue((float) this.metaData.kingdoms);
      this._cities.setValue((float) this.metaData.cities);
      this._buildings.setValue((float) this.metaData.buildings);
      this._equipment.setValue((float) this.metaData.equipment);
      this._books.setValue((float) this.metaData.books);
      this._wars.setValue((float) this.metaData.wars);
      this._alliances.setValue((float) this.metaData.alliances);
      this._families.setValue((float) this.metaData.families);
      this._clans.setValue((float) this.metaData.clans);
      this._cultures.setValue((float) this.metaData.cultures);
      this._religions.setValue((float) this.metaData.religions);
      this._languages.setValue((float) this.metaData.languages);
      this._subspecies.setValue((float) this.metaData.subspecies);
      this._favorites.setValue((float) this.metaData.favorites);
      this._favorite_items.setValue((float) this.metaData.favorite_items);
      this._map_name.text = mapStats.name;
      ((Graphic) this._map_name).color = mapStats.getArchitectMood().getColorText();
      ((Graphic) this._name_input.textField).color = mapStats.getArchitectMood().getColorText();
      this._text_description.text = mapStats.description;
      if (SaveManager.currentWorkshopMapData == null)
        return;
      Item workshopItem = SaveManager.currentWorkshopMapData.workshop_item;
      if (((Item) ref workshopItem).Owner.Id.ToString() == Config.steam_id)
        ((Graphic) this._map_name).color = Toolbox.makeColor("#3DDEFF");
      else
        ((Graphic) this._map_name).color = Toolbox.makeColor("#FF9B1C");
    }
    else
      ((Component) this._map_name).GetComponent<LocalizedText>().updateText();
  }

  private void applyInputName(string pInput)
  {
    if (string.IsNullOrEmpty(pInput))
      return;
    if (this._save_names_to_current_world)
      World.world.map_stats.name = pInput;
    else
      this.metaData.mapStats.name = pInput;
    if (!this._save_meta_data_on_close || this.metaData == null)
      return;
    SaveManager.saveMetaData(this.metaData, this._save_path);
  }

  private void applyInputDescription(string pInput)
  {
    if (pInput == null)
      return;
    if (this._save_names_to_current_world)
    {
      if (Object.op_Equality((Object) World.world, (Object) null) || World.world.map_stats == null)
        return;
      World.world.map_stats.description = pInput;
    }
    else
    {
      if (this.metaData == null || this.metaData.mapStats == null)
        return;
      this.metaData.mapStats.description = pInput;
    }
    if (!this._save_meta_data_on_close || this.metaData == null)
      return;
    SaveManager.saveMetaData(this.metaData, this._save_path);
  }

  private void OnDisable()
  {
    if (!this._save_meta_data_on_close || this.metaData == null)
      return;
    SaveManager.saveMetaData(this.metaData, this._save_path);
  }

  private void checkNameInput(bool pDeactivate = false)
  {
    if (!this._save_meta_data_on_close || this.metaData == null)
      return;
    this.metaData.mapStats.name = this._name_input.textField.text;
    this.metaData.mapStats.description = this._description_input.textField.text;
    SaveManager.saveMetaData(this.metaData, this._save_path);
  }

  private void checkRaceIcons(MapMetaData pData)
  {
    this._icon_orc.gameObject.SetActive(false);
    this._icon_human.gameObject.SetActive(false);
    this._icon_elf.gameObject.SetActive(false);
    this._icon_dwarf.gameObject.SetActive(false);
  }
}

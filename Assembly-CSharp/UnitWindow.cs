// Decompiled with JetBrains decompiler
// Type: UnitWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class UnitWindow : 
  StatsWindow,
  ITraitWindow<ActorTrait, ActorTraitButton>,
  IAugmentationsWindow<ITraitsEditor<ActorTrait>>,
  IEquipmentWindow,
  IAugmentationsWindow<IEquipmentEditor>,
  IPlotsWindow,
  IAugmentationsWindow<IPlotsEditor>
{
  [SerializeField]
  private Image _mood_sprite;
  [SerializeField]
  private Image _mood_bg;
  public NameInput name_input;
  [SerializeField]
  private UiUnitAvatarElement _avatar_element;
  [SerializeField]
  private Image _icon_favorite;
  [SerializeField]
  private WindowMetaTab _button_trait_editor;
  [SerializeField]
  private WindowMetaTab _button_equipment_editor;
  [SerializeField]
  private WindowMetaTab _button_mind_tab;
  [SerializeField]
  private WindowMetaTab _button_genealogy_tab;
  [SerializeField]
  private WindowMetaTab _button_plots_tab;
  [SerializeField]
  private WindowMetaTab _button_possession;
  [SerializeField]
  private Image _equipment_editor_icon;
  [SerializeField]
  private Sprite _equipment_sprite_normal;
  [SerializeField]
  private Sprite _equipment_sprite_firearm;
  [SerializeField]
  private LocalizedText _main_tab_title;
  [SerializeField]
  private UnitTextManager _unit_text;
  private string _initial_name;

  public override MetaType meta_type => MetaType.Unit;

  internal Actor actor => SelectedUnit.unit;

  protected virtual bool onNameChange(string pInput)
  {
    if (string.IsNullOrWhiteSpace(pInput) || this.actor.isRekt())
      return false;
    string pName = pInput.Trim();
    if (this._initial_name == pName)
      return false;
    this.actor.data.custom_name = true;
    this.actor.setName(pName);
    this._initial_name = pName;
    this.name_input.SetOutline();
    foreach (City city in (CoreSystemManager<City, CityData>) World.world.cities)
    {
      if (!city.isRekt())
      {
        city.updateRulers();
        if (city.data.founder_id == this.actor.getID())
          city.data.founder_name = this.actor.data.name;
      }
    }
    foreach (Army army in (CoreSystemManager<Army, ArmyData>) World.world.armies)
    {
      if (!army.isRekt())
        army.updateCaptains();
    }
    foreach (Kingdom kingdom in (CoreSystemManager<Kingdom, KingdomData>) World.world.kingdoms)
    {
      if (!kingdom.isRekt())
        kingdom.updateRulers();
    }
    foreach (War war in (CoreSystemManager<War, WarData>) World.world.wars)
    {
      if (!war.isRekt() && war.data.started_by_actor_id == this.actor.getID())
        war.data.started_by_actor_name = this.actor.data.name;
    }
    foreach (Alliance alliance in (CoreSystemManager<Alliance, AllianceData>) World.world.alliances)
    {
      if (!alliance.isRekt() && alliance.data.founder_actor_id == this.actor.getID())
        alliance.data.founder_actor_name = this.actor.data.name;
    }
    foreach (Religion religion in (CoreSystemManager<Religion, ReligionData>) World.world.religions)
    {
      if (!religion.isRekt() && religion.data.creator_id == this.actor.getID())
        religion.data.creator_name = this.actor.data.name;
    }
    foreach (Culture culture in (CoreSystemManager<Culture, CultureData>) World.world.cultures)
    {
      if (!culture.isRekt() && culture.data.creator_id == this.actor.getID())
        culture.data.creator_name = this.actor.data.name;
    }
    foreach (Clan clan in (CoreSystemManager<Clan, ClanData>) World.world.clans)
    {
      if (!clan.isRekt())
      {
        if (clan.data.founder_actor_id == this.actor.getID())
          clan.data.founder_actor_name = this.actor.data.name;
        clan.updateChiefs();
      }
    }
    foreach (Language language in (CoreSystemManager<Language, LanguageData>) World.world.languages)
    {
      if (!language.isRekt() && language.data.creator_id == this.actor.getID())
        language.data.creator_name = this.actor.data.name;
    }
    foreach (Family family in (CoreSystemManager<Family, FamilyData>) World.world.families)
    {
      if (!family.isRekt())
      {
        if (family.data.main_founder_id_1 == this.actor.getID())
          family.data.founder_actor_name_1 = this.actor.data.name;
        if (family.data.main_founder_id_2 == this.actor.getID())
          family.data.founder_actor_name_2 = this.actor.data.name;
      }
    }
    foreach (Book book in (CoreSystemManager<Book, BookData>) World.world.books)
    {
      if (!book.isRekt() && book.data.author_id == this.actor.getID())
        book.data.author_name = this.actor.data.name;
    }
    foreach (Plot plot in (CoreSystemManager<Plot, PlotData>) World.world.plots)
    {
      if (!plot.isRekt() && plot.data.founder_id == this.actor.getID())
        plot.data.founder_name = this.actor.data.name;
    }
    foreach (Item pObject in (CoreSystemManager<Item, ItemData>) World.world.items)
    {
      if (!pObject.isRekt() && pObject.data.creator_id == this.actor.getID())
        pObject.data.by = this.actor.data.name;
    }
    return true;
  }

  internal override bool checkCancelWindow()
  {
    return this.actor.isRekt() || !this.actor.hasHealth() || base.checkCancelWindow();
  }

  private void clear()
  {
    this._button_trait_editor.toggleActive(false);
    this._button_equipment_editor.toggleActive(false);
    this._button_plots_tab.toggleActive(false);
    this._button_mind_tab.toggleActive(false);
    this._button_genealogy_tab.toggleActive(false);
    this._button_possession.toggleActive(false);
    ((Component) ((Component) this._icon_favorite).transform.parent).gameObject.SetActive(false);
    ((Component) this._mood_bg).gameObject.SetActive(false);
    this.name_input.setText("");
  }

  protected override void OnEnable()
  {
    base.OnEnable();
    this.showInfo();
    if (!this.actor.asset.isAvailable() && !this.actor.asset.unlocked_with_achievement)
      this.actor.asset.unlock(true);
    AchievementLibrary.checkUnitAchievements(this.actor);
  }

  public void showInfo()
  {
    this.clear();
    this.loadNameInput();
    this.showMainInfo();
    this.checkShowMain();
  }

  private void checkShowMain()
  {
    WindowMetaTab activeTab = this.tabs.getActiveTab();
    if ((Object.op_Equality((Object) activeTab, (Object) this._button_trait_editor) && !this.isTraitsEditorAllowed() || Object.op_Equality((Object) activeTab, (Object) this._button_equipment_editor) && !this.isEquipmentEditorAllowed() || Object.op_Equality((Object) activeTab, (Object) this._button_mind_tab) && !this.isMindTabAllowed() || Object.op_Equality((Object) activeTab, (Object) this._button_genealogy_tab) && !this.isGenealogyTabAllowed() ? 1 : (!Object.op_Equality((Object) activeTab, (Object) this._button_plots_tab) ? 0 : (!this.isPlotsTabAllowed() ? 1 : 0))) == 0)
      return;
    this.showTab(this.tabs.tab_default);
  }

  internal override void showStatsRows()
  {
    this.tryShowPastNames();
    this.showStatRow("birthday", (object) this.actor.getBirthday());
    this.showStatRow("task", (object) this.actor.getTaskText());
    if (this.actor.asset.inspect_generation)
      this.showStatRow("generation", (object) this.actor.data.generation);
    if (this.actor.data.loot > 0)
      this.showStatRow("loot", (object) this.actor.data.loot, "#43FF43", pIconPath: "iconLoot");
    if (this.actor.data.money > 0)
      this.showStatRow("money", (object) this.actor.data.money, "#43FF43", pIconPath: "iconMoney");
    if (this.actor.inventory.hasResources())
      this.showStatRow("resources", (object) this.actor.inventory.getRandomResourceID().Localize(), MetaType.None, -1L, (string) null, "carrying_resources", new TooltipDataGetter(this.getTooltipCarryingResources));
    if (this.actor.hasBestFriend())
    {
      Actor bestFriend = this.actor.getBestFriend();
      if (!bestFriend.isRekt())
      {
        string pIconPath = "iconMale";
        if (bestFriend.isSexFemale())
          pIconPath = "iconFemale";
        this.tryToShowActor("best_friend", pObject: bestFriend, pIconPath: pIconPath);
      }
    }
    if (this.actor.hasLover())
    {
      Actor lover = this.actor.lover;
      if (!this.actor.lover.isRekt())
      {
        string pIconPath = "iconMale";
        if (lover.isSexFemale())
          pIconPath = "iconFemale";
        this.tryToShowActor("lover", pObject: lover, pIconPath: pIconPath);
      }
    }
    if (this.actor.hasFavoriteFood())
      this.showStatRow("creature_statistics_favorite_food", (object) this.actor.favorite_food_asset.getTranslatedName());
    if (this.actor.isSapient() && this.actor.s_personality != null)
      this.showStatRow("creature_statistics_personality", (object) this.actor.s_personality.getTranslatedName());
    if (this.actor.asset.is_boat)
    {
      Boat simpleComponent = this.actor.getSimpleComponent<Boat>();
      string pTooltipId;
      TooltipDataGetter pTooltipData;
      if (!simpleComponent.hasPassengers())
      {
        pTooltipId = (string) null;
        pTooltipData = (TooltipDataGetter) null;
      }
      else
      {
        pTooltipId = "passengers";
        pTooltipData = new TooltipDataGetter(this.getTooltipPassengers);
      }
      this.showStatRow("passengers", (object) simpleComponent.countPassengers(), MetaType.None, -1L, (string) null, pTooltipId, pTooltipData);
    }
    int massKg = this.actor.getMassKG();
    string pTooltipId1;
    TooltipDataGetter pTooltipData1;
    if (this.actor.asset.resources_given == null)
    {
      pTooltipId1 = (string) null;
      pTooltipData1 = (TooltipDataGetter) null;
    }
    else
    {
      pTooltipId1 = "mass";
      pTooltipData1 = new TooltipDataGetter(this.getTooltipMass);
    }
    this.showStatRow("mass", (object) $"{massKg} kg", MetaType.None, -1L, (string) null, pTooltipId1, pTooltipData1);
    if (!this.actor.asset.inspect_show_species)
      return;
    this.tryToShowMetaSpecies("species", this.actor.asset.id);
  }

  public override void showMetaRows()
  {
    if (this.actor.asset.inspect_home)
      this.meta_rows_container.tryToShowMetaCity("creature_statistics_home_village", pObject: this.actor.city);
    if (this.actor.hasKingdom() && this.actor.isKingdomCiv())
    {
      this.meta_rows_container.tryToShowMetaKingdom(pObject: this.actor.kingdom);
      if (this.actor.kingdom.hasAlliance())
        this.meta_rows_container.tryToShowMetaAlliance(pObject: this.actor.kingdom.getAlliance());
    }
    if (this.actor.hasClan())
      this.meta_rows_container.tryToShowMetaClan(pObject: this.actor.clan);
    if (this.actor.hasLanguage())
      this.meta_rows_container.tryToShowMetaLanguage(pObject: this.actor.language);
    if (this.actor.hasReligion())
      this.meta_rows_container.tryToShowMetaReligion(pObject: this.actor.religion);
    if (this.actor.hasFamily())
      this.meta_rows_container.tryToShowMetaFamily(pObject: this.actor.family);
    if (this.actor.hasCulture())
      this.meta_rows_container.tryToShowMetaCulture(pObject: this.actor.culture);
    if (!this.actor.asset.inspect_show_species || !this.actor.hasSubspecies())
      return;
    this.meta_rows_container.tryToShowMetaSubspecies("subspecies", pObject: this.actor.subspecies);
  }

  public TooltipData getTooltipMass()
  {
    return new TooltipData()
    {
      tip_name = "mass",
      actor = this.actor
    };
  }

  private TooltipData getTooltipPassengers()
  {
    return new TooltipData() { actor = this.actor };
  }

  public TooltipData getTooltipCarryingResources()
  {
    return new TooltipData()
    {
      tip_name = "carrying",
      actor = this.actor
    };
  }

  private void loadNameInput()
  {
    if (this.actor.isRekt())
      return;
    // ISSUE: method pointer
    ((UnityEvent<string>) this.name_input.inputField.onEndEdit).AddListener(new UnityAction<string>((object) this, __methodptr(\u003CloadNameInput\u003Eb__32_0)));
    string pText = this.actor.getName().Trim();
    this._initial_name = pText;
    this.name_input.setText(pText);
    if (!this.actor.data.custom_name)
      return;
    this.name_input.SetOutline();
  }

  public void showMainInfo()
  {
    if (this.actor.isRekt())
      return;
    this.checkMainTabTitle();
    if (this.isTraitsEditorAllowed())
      this._button_trait_editor.toggleActive(true);
    if (this.isEquipmentEditorAllowed())
      this._button_equipment_editor.toggleActive(true);
    if (this.isPlotsTabAllowed())
      this._button_plots_tab.toggleActive(true);
    if (this.isMindTabAllowed())
      this._button_mind_tab.toggleActive(true);
    if (this.isGenealogyTabAllowed())
      this._button_genealogy_tab.toggleActive(true);
    if (this.actor.canBePossessed())
      this._button_possession.toggleActive(true);
    if (this.actor.isKingdomCiv())
      ((Graphic) this.name_input.textField).color = this.actor.kingdom.getColor().getColorText();
    else
      ((Graphic) this.name_input.textField).color = Toolbox.color_log_neutral;
    this._avatar_element.show(this.actor);
    if (this.actor.isSapient())
      ((Component) this._mood_bg).gameObject.SetActive(false);
    if (this.actor.asset.can_be_favorited)
      ((Component) ((Component) this._icon_favorite).transform.parent).gameObject.SetActive(true);
    this.setIconValue("i_age", (float) this.actor.getAge());
    this.updateFavoriteIconFor(this.actor);
    this.checkEquipmentTabIcon();
  }

  private void checkMainTabTitle()
  {
    ActorAsset asset = this.actor.asset;
    asset.getSpriteIcon();
    if (asset.is_boat)
      this._main_tab_title.setKeyAndUpdate("boat");
    else
      this._main_tab_title.setKeyAndUpdate(asset.getLocaleID());
  }

  public void reloadBanner() => this._avatar_element.show(this.actor);

  private bool isTraitsEditorAllowed() => this.actor.asset.can_edit_traits;

  private bool isEquipmentEditorAllowed() => this.actor.canEditEquipment();

  private bool isMindTabAllowed() => this.actor.asset.inspect_mind;

  private bool isGenealogyTabAllowed() => this.actor.asset.inspect_genealogy;

  private bool isPlotsTabAllowed()
  {
    return this.actor.isCityLeader() || this.actor.isKing() || this.actor.hasClan();
  }

  public void locateActorHouse()
  {
    if (this.actor.getHomeBuilding() == null)
      return;
    World.world.locatePosition(this.actor.getHomeBuilding().current_tile.posV3);
  }

  private void updateFavoriteIconFor(Actor pUnit)
  {
    if (pUnit.isFavorite())
      ((Graphic) this._icon_favorite).color = ColorStyleLibrary.m.favorite_selected;
    else
      ((Graphic) this._icon_favorite).color = ColorStyleLibrary.m.favorite_not_selected;
  }

  public void pressFavorite()
  {
    if (this.actor.isRekt())
      return;
    this.actor.switchFavorite();
    this.updateFavoriteIconFor(this.actor);
    this.refreshMetaList();
    SpriteSwitcher.checkAllStates();
    if (!this.actor.data.favorite)
      return;
    WorldTip.showNowTop("tip_favorite_icon");
  }

  private void OnDisable() => this.name_input.inputField.DeactivateInputField();

  protected void setIconValue(
    string pName,
    float pMainVal,
    float? pMax = null,
    string pColor = "",
    bool pFloat = false,
    string pEnding = "",
    char pSeparator = '/')
  {
    Transform recursive = ((Component) this).transform.FindRecursive(pName);
    if (Object.op_Equality((Object) recursive, (Object) null))
      return;
    StatsIcon component = ((Component) recursive).GetComponent<StatsIcon>();
    ((Component) component).gameObject.SetActive(true);
    component.setValue(pMainVal, pMax, pColor, pFloat, pEnding, pSeparator);
  }

  public void clickOpenAssetDebug()
  {
    if (this.actor.isRekt())
      return;
    BaseDebugAssetElement<ActorAsset>.selected_asset = this.actor.asset;
    ScrollWindow.showWindow("actor_asset");
  }

  public void clickGetLLMPrompt()
  {
    if (this.actor.isRekt())
      return;
    GUIUtility.systemCopyBuffer = GenerateLLMPrompt.getText(this.actor);
  }

  public void checkEquipmentTabIcon()
  {
    if (!this.actor.hasEquipment())
      return;
    this._equipment_editor_icon.sprite = this.actor.isWeaponFirearm() ? this._equipment_sprite_firearm : this._equipment_sprite_normal;
  }

  public void avatarTouchScream()
  {
    if (!this.actor.isRekt())
      this.actor.pokeFromAvatarUI();
    this._unit_text.spawnAvatarText();
    this.scroll_window.shake();
    ScrollWindow.randomDropHoveringIcon(1, 2);
    this.reloadBanner();
    this.tabs.reloadActiveTab();
  }

  internal void tryShowPastNames()
  {
    List<NameEntry> pastNames1 = this.actor.data.past_names;
    // ISSUE: explicit non-virtual call
    if ((pastNames1 != null ? (__nonvirtual (pastNames1.Count) > 0 ? 1 : 0) : 0) == 0)
      return;
    List<NameEntry> pastNames2 = this.actor.data.past_names;
    // ISSUE: explicit non-virtual call
    this.showStatRow("past_names", (object) (pastNames2 != null ? __nonvirtual (pastNames2.Count) : 1), MetaType.None, -1L, "iconVillages", "past_names", new TooltipDataGetter(this.getTooltipPastNames));
  }

  private TooltipData getTooltipPastNames()
  {
    return new TooltipData()
    {
      tip_name = "past_names",
      past_names = new ListPool<NameEntry>((ICollection<NameEntry>) this.actor.data.past_names),
      meta_type = MetaType.Unit
    };
  }

  T IAugmentationsWindow<ITraitsEditor<ActorTrait>>.GetComponentInChildren<T>(bool includeInactive)
  {
    return ((Component) this).GetComponentInChildren<T>(includeInactive);
  }

  T IAugmentationsWindow<IEquipmentEditor>.GetComponentInChildren<T>(bool includeInactive)
  {
    return ((Component) this).GetComponentInChildren<T>(includeInactive);
  }

  T IAugmentationsWindow<IPlotsEditor>.GetComponentInChildren<T>(bool includeInactive)
  {
    return ((Component) this).GetComponentInChildren<T>(includeInactive);
  }
}

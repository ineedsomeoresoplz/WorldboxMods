// Decompiled with JetBrains decompiler
// Type: StatsWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class StatsWindow : TabbedWindow
{
  [SerializeField]
  protected StatsMetaRowsContainer meta_rows_container;
  private StatsRowsContainer _stats_rows_container;
  private IRefreshElement[] _refreshable_elements;
  private const string EMPTY_META_OBJECT_STRING = "???";

  public virtual MetaType meta_type => throw new NotImplementedException();

  protected virtual void OnEnable()
  {
    MetaTypeAsset asset = AssetManager.meta_type_library.getAsset(this.meta_type);
    if (string.IsNullOrEmpty(asset.power_tab_id))
      return;
    SelectedObjects.setNanoObject(asset.get_selected());
    PowerTabController.showWorldTipSelected(asset.power_tab_id, false);
    ScrollWindow.skip_worldtip_hide = true;
  }

  internal StatsRowsContainer stats_rows_container
  {
    get
    {
      if (Object.op_Equality((Object) this._stats_rows_container, (Object) null))
      {
        this._stats_rows_container = ((Component) ((Component) this).transform.FindRecursive("content_stats")).GetComponent<StatsRowsContainer>();
        if (Object.op_Equality((Object) this._stats_rows_container, (Object) null))
          Debug.LogError((object) ("No StatsRowsContainer 'content_stats' found in " + ((Object) this).name));
      }
      return this._stats_rows_container;
    }
  }

  protected override void create()
  {
    base.create();
    this._refreshable_elements = ((Component) this).GetComponentsInChildren<IRefreshElement>(true);
  }

  protected void refreshMetaList() => MetaSwitchManager.refresh();

  public void updateStats()
  {
    foreach (IRefreshElement refreshableElement in this._refreshable_elements)
      refreshableElement.refresh();
  }

  internal KeyValueField getStatRow(string pID) => this.stats_rows_container.getStatRow(pID);

  internal virtual void showStatsRows()
  {
    throw new NotImplementedException("showStatsRows not implemented");
  }

  public virtual void showMetaRows()
  {
  }

  internal KeyValueField showStatRow(
    string pId,
    object pValue,
    MetaType pMetaType = MetaType.None,
    long pMetaId = -1,
    string pIconPath = null,
    string pTooltipId = null,
    TooltipDataGetter pTooltipData = null)
  {
    return this.stats_rows_container.showStatRow(pId, pValue, (string) null, pMetaType, pMetaId, pIconPath: pIconPath, pTooltipId: pTooltipId, pTooltipData: pTooltipData);
  }

  internal KeyValueField showStatRow(
    string pId,
    object pValue,
    string pColor,
    MetaType pMetaType = MetaType.None,
    long pMetaId = -1,
    bool pColorText = false,
    string pIconPath = null,
    string pTooltipId = null,
    TooltipDataGetter pTooltipData = null,
    bool pLocalize = true)
  {
    return this.stats_rows_container.showStatRow(pId, pValue, pColor, pMetaType, pMetaId, pColorText, pIconPath, pTooltipId, pTooltipData, pLocalize);
  }

  internal void updateStatsRows()
  {
    ((Component) this.stats_rows_container).gameObject.SetActive(false);
    ((Component) this.stats_rows_container).gameObject.SetActive(true);
  }

  protected void showStatRowMeta(
    string pId,
    object pValue,
    string pColor,
    MetaType pMetaType,
    long pMetaId,
    bool pColorText = false,
    string pIconPath = null,
    string pTooltipId = null,
    TooltipDataGetter pTooltipData = null,
    bool pLocalize = true)
  {
    this.showStatRow(pId, pValue, pColor, pMetaType, pMetaId, true, pIconPath, pTooltipId, pTooltipData, pLocalize);
  }

  private void showStatsMetaCity(City pCity, string pTitle)
  {
    string colorText = pCity.kingdom.getColor()?.color_text;
    string pValue = pCity.name + Toolbox.coloredGreyPart((object) pCity.getPopulationPeople(), colorText);
    this.showStatRowMeta(pTitle, (object) pValue, colorText, MetaType.City, pCity.getID(), pIconPath: "iconCity");
  }

  private void showStatsMetaKingdom(Kingdom pKingdom, string pTitle = "kingdom")
  {
    string pValue = "???";
    string colorText = pKingdom?.getColor().color_text;
    if (pKingdom != null)
      pValue = pKingdom.data.name + Toolbox.coloredGreyPart((object) pKingdom.getPopulationPeople(), colorText);
    this.showStatRowMeta(pTitle, (object) pValue, colorText, MetaType.Kingdom, pKingdom != null ? pKingdom.getID() : -1L, pIconPath: "iconKingdom");
  }

  private void showStatsMetaUnit(Actor pActor, string pTitle, string pIconPath = null)
  {
    string pValue = "???";
    string colorText = pActor?.kingdom.getColor().color_text;
    if (pActor != null)
      pValue = pActor.getName() + Toolbox.coloredGreyPart((object) pActor.getAge(), colorText, true);
    this.showStatRowMeta(pTitle, (object) pValue, colorText, MetaType.Unit, pActor != null ? pActor.getID() : -1L, pIconPath: pIconPath);
  }

  private void showStatsMetaCulture(Culture pCulture, string pTitle = "culture")
  {
    string pValue = "???";
    string colorText = pCulture?.getColor().color_text;
    if (pCulture != null)
      pValue = pCulture.data.name + Toolbox.coloredGreyPart((object) pCulture.units.Count, colorText);
    this.showStatRowMeta(pTitle, (object) pValue, colorText, MetaType.Culture, pCulture != null ? pCulture.getID() : -1L, pIconPath: "iconCulture");
  }

  private void showStatsMetaLanguage(Language pLanguage, string pTitle = "language")
  {
    string pValue = "???";
    string colorText = pLanguage?.getColor().color_text;
    if (pLanguage != null)
      pValue = pLanguage.data.name + Toolbox.coloredGreyPart((object) pLanguage.units.Count, colorText);
    this.showStatRowMeta(pTitle, (object) pValue, colorText, MetaType.Language, pLanguage != null ? pLanguage.getID() : -1L, pIconPath: "iconLanguage");
  }

  private void showStatsMetaReligion(Religion pReligion, string pTitle = "religion")
  {
    string pValue = "???";
    string colorText = pReligion?.getColor().color_text;
    if (pReligion != null)
      pValue = pReligion.data.name + Toolbox.coloredGreyPart((object) pReligion.units.Count, colorText);
    this.showStatRowMeta(pTitle, (object) pValue, colorText, MetaType.Religion, pReligion != null ? pReligion.getID() : -1L, pIconPath: "iconReligion");
  }

  private void showStatsMetaClan(Clan pClan, string pTitle = "clan")
  {
    string pValue = "???";
    string colorText = pClan?.getColor().color_text;
    if (pClan != null)
      pValue = pClan.data.name + Toolbox.coloredGreyPart((object) pClan.units.Count, colorText);
    this.showStatRowMeta(pTitle, (object) pValue, colorText, MetaType.Clan, pClan != null ? pClan.getID() : -1L, pIconPath: "iconClan");
  }

  private void showStatsMetaSubspecies(Subspecies pSubspecies, string pTitle = "subspecies")
  {
    string pValue = "???";
    string colorText = pSubspecies?.getColor().color_text;
    if (pSubspecies != null)
      pValue = pSubspecies.data.name + Toolbox.coloredGreyPart((object) pSubspecies.units.Count, colorText);
    this.showStatRowMeta(pTitle, (object) pValue, colorText, MetaType.Subspecies, pSubspecies != null ? pSubspecies.getID() : -1L, pIconPath: "iconSpecies");
  }

  private void showStatsMetaFamily(Family pFamily, string pTitle = "family")
  {
    string colorText = pFamily.getColor().color_text;
    string pValue = pFamily.name + Toolbox.coloredGreyPart((object) pFamily.units.Count, colorText);
    this.showStatRowMeta(pTitle, (object) pValue, colorText, MetaType.Family, pFamily.getID(), pIconPath: "iconFamily");
  }

  private void showStatsMetaAlliance(Alliance pAlliance, string pTitle = "alliance")
  {
    string pValue = "???";
    string colorText = pAlliance?.getColor().color_text;
    if (pAlliance != null)
      pValue = pAlliance.data.name + Toolbox.coloredGreyPart((object) pAlliance.countPopulation(), colorText);
    this.showStatRowMeta(pTitle, (object) pValue, colorText, MetaType.Alliance, pAlliance != null ? pAlliance.getID() : -1L, pIconPath: "iconAlliance");
  }

  protected void tryToShowMetaFamily(string pTitle = "family", long pID = -1, string pName = null, Family pObject = null)
  {
    Family family = pObject != null ? pObject : World.world.families.get(pID);
    if (!family.isRekt())
      this.showStatsMetaFamily(family, pTitle);
    else
      this.showEmptyStatRow(pTitle, pName, "iconFamily");
  }

  protected void tryToShowMetaCulture(string pTitle = "culture", long pID = -1, string pName = null, Culture pObject = null)
  {
    Culture culture = pObject != null ? pObject : World.world.cultures.get(pID);
    if (!culture.isRekt())
      this.showStatsMetaCulture(culture, pTitle);
    else
      this.showEmptyStatRow(pTitle, pName, "iconCulture");
  }

  protected void tryToShowMetaLanguage(string pTitle = "language", long pID = -1, string pName = null, Language pObject = null)
  {
    Language language = pObject != null ? pObject : World.world.languages.get(pID);
    if (!language.isRekt())
      this.showStatsMetaLanguage(language, pTitle);
    else
      this.showEmptyStatRow(pTitle, pName, "iconLanguage");
  }

  protected void tryToShowMetaReligion(string pTitle = "religion", long pID = -1, string pName = null, Religion pObject = null)
  {
    Religion religion = pObject != null ? pObject : World.world.religions.get(pID);
    if (!religion.isRekt())
      this.showStatsMetaReligion(religion, pTitle);
    else
      this.showEmptyStatRow(pTitle, pName, "iconReligion");
  }

  protected void tryToShowMetaAlliance(string pTitle = "alliance", long pID = -1, string pName = null, Alliance pObject = null)
  {
    Alliance alliance = pObject != null ? pObject : World.world.alliances.get(pID);
    if (!alliance.isRekt())
      this.showStatsMetaAlliance(alliance, pTitle);
    else
      this.showEmptyStatRow(pTitle, pName, "iconAlliance");
  }

  protected void tryToShowMetaCity(
    string pTitle,
    long pID = -1,
    string pName = null,
    City pObject = null,
    string pIconPath = "iconCity")
  {
    City city = pObject != null ? pObject : World.world.cities.get(pID);
    if (!city.isRekt())
    {
      this.showStatsMetaCity(city, pTitle);
    }
    else
    {
      string pContent = pID == -1L || string.IsNullOrEmpty(pName) ? pName : "† " + pName;
      this.showEmptyStatRow(pTitle, pContent, pIconPath);
    }
  }

  protected void tryToShowMetaSubspecies(
    string pTitle = "main_subspecies",
    long pID = -1,
    string pName = null,
    Subspecies pObject = null)
  {
    Subspecies subspecies = pObject != null ? pObject : World.world.subspecies.get(pID);
    if (!subspecies.isRekt())
    {
      this.showStatsMetaSubspecies(subspecies, pTitle);
    }
    else
    {
      string pContent = pID == -1L || string.IsNullOrEmpty(pName) ? pName : "† " + pName;
      this.showEmptyStatRow(pTitle, pContent, "iconSpecies");
    }
  }

  protected void tryToShowMetaKingdom(string pTitle = "kingdom", long pID = -1, string pName = null, Kingdom pObject = null)
  {
    Kingdom kingdom = pObject != null ? pObject : World.world.kingdoms.get(pID) ?? (Kingdom) World.world.kingdoms.db_get(pID);
    if (!kingdom.isRekt())
    {
      this.showStatsMetaKingdom(kingdom, pTitle);
    }
    else
    {
      string pContent = pID == -1L || string.IsNullOrEmpty(pName) ? pName : "† " + pName;
      this.showEmptyStatRow(pTitle, pContent, "iconKingdom");
    }
  }

  protected void tryToShowMetaClan(string pTitle = "clan", long pID = -1, string pName = null, Clan pObject = null)
  {
    Clan clan = pObject != null ? pObject : World.world.clans.get(pID);
    if (!clan.isRekt())
    {
      this.showStatsMetaClan(clan, pTitle);
    }
    else
    {
      string pContent = pID == -1L || string.IsNullOrEmpty(pName) ? pName : "† " + pName;
      this.showEmptyStatRow(pTitle, pContent, "iconClan");
    }
  }

  protected void tryToShowActor(
    string pTitle,
    long pID = -1,
    string pName = null,
    Actor pObject = null,
    string pIconPath = null)
  {
    Actor actor = pObject != null ? pObject : World.world.units.get(pID);
    if (!actor.isRekt())
    {
      this.showStatsMetaUnit(actor, pTitle, pIconPath);
    }
    else
    {
      string pContent = pID == -1L || string.IsNullOrEmpty(pName) ? pName : "† " + pName;
      this.showEmptyStatRow(pTitle, pContent, pIconPath);
    }
  }

  private void showEmptyStatRow(string pTitle, string pContent = null, string pIconPath = null)
  {
    StatsWindow.showEmptyStatRow(pTitle, this.stats_rows_container, pContent, pIconPath);
  }

  private static void showEmptyStatRow(
    string pTitle,
    StatsRowsContainer pContainer,
    string pContent = null,
    string pIconPath = null)
  {
    if (string.IsNullOrEmpty(pContent))
      pContent = "???";
    if (pContent == "neutral")
      pContent = "???";
    pContainer.showStatRow(pTitle, (object) pContent, ColorStyleLibrary.m.color_dead_text, pIconPath: pIconPath);
  }

  protected void tryToShowMetaSpecies(string pTitle, string pId)
  {
    StatsWindow.tryToShowMetaSpecies(pTitle, pId, this.stats_rows_container);
  }

  public static void tryToShowMetaSpecies(string pTitle, string pId, StatsRowsContainer pContainer)
  {
    if (!string.IsNullOrEmpty(pId))
    {
      ActorAsset actorAsset = AssetManager.actor_library.get(pId);
      if (actorAsset != null)
      {
        string translatedName = actorAsset.getTranslatedName();
        pContainer.showStatRow(pTitle, (object) translatedName, (string) null, pIconPath: "iconGene", pTooltipId: "unit_species", pTooltipData: new TooltipDataGetter(actorAsset.getTooltip));
        return;
      }
    }
    StatsWindow.showEmptyStatRow(pTitle, pContainer);
  }
}

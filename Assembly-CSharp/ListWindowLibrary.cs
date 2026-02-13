// Decompiled with JetBrains decompiler
// Type: ListWindowLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ListWindowLibrary : AssetLibrary<ListWindowAsset>
{
  private Dictionary<MetaType, ListWindowAsset> _dict = new Dictionary<MetaType, ListWindowAsset>();

  public override void init()
  {
    ListWindowAsset pAsset1 = new ListWindowAsset();
    pAsset1.id = "list_alliances";
    pAsset1.meta_type = MetaType.Alliance;
    pAsset1.no_items_locale = "list_empty_alliances";
    pAsset1.art_path = "ui/illustrations/art_alliances";
    pAsset1.icon_path = "ui/Icons/iconAllianceList";
    pAsset1.set_list_component = (ListComponentSetter) (pTransform => (IComponentList) ((Component) pTransform).AddComponent<AllianceListComponent>());
    this.add(pAsset1);
    ListWindowAsset pAsset2 = new ListWindowAsset();
    pAsset2.id = "list_clans";
    pAsset2.meta_type = MetaType.Clan;
    pAsset2.no_items_locale = "list_empty_clans";
    pAsset2.art_path = "ui/illustrations/art_clans";
    pAsset2.icon_path = "ui/Icons/iconClanList";
    pAsset2.set_list_component = (ListComponentSetter) (pTransform => (IComponentList) ((Component) pTransform).AddComponent<ClanListComponent>());
    this.add(pAsset2);
    ListWindowAsset pAsset3 = new ListWindowAsset();
    pAsset3.id = "list_cultures";
    pAsset3.meta_type = MetaType.Culture;
    pAsset3.no_items_locale = "list_empty_cultures";
    pAsset3.art_path = "ui/illustrations/art_cultures";
    pAsset3.icon_path = "ui/Icons/iconCultureList";
    pAsset3.set_list_component = (ListComponentSetter) (pTransform => (IComponentList) ((Component) pTransform).AddComponent<CultureListComponent>());
    this.add(pAsset3);
    ListWindowAsset pAsset4 = new ListWindowAsset();
    pAsset4.id = "list_cities";
    pAsset4.meta_type = MetaType.City;
    pAsset4.no_items_locale = "list_empty_villages";
    pAsset4.art_path = "ui/illustrations/art_cities";
    pAsset4.icon_path = "ui/Icons/iconCityList";
    pAsset4.set_list_component = (ListComponentSetter) (pTransform => (IComponentList) ((Component) pTransform).AddComponent<CityListComponent>());
    this.add(pAsset4);
    ListWindowAsset pAsset5 = new ListWindowAsset();
    pAsset5.id = "list_families";
    pAsset5.meta_type = MetaType.Family;
    pAsset5.no_items_locale = "list_empty_families";
    pAsset5.art_path = "ui/illustrations/art_families";
    pAsset5.icon_path = "ui/Icons/iconFamilyList";
    pAsset5.set_list_component = (ListComponentSetter) (pTransform => (IComponentList) ((Component) pTransform).AddComponent<FamilyListComponent>());
    this.add(pAsset5);
    ListWindowAsset pAsset6 = new ListWindowAsset();
    pAsset6.id = "list_favorite_items";
    pAsset6.meta_type = MetaType.Item;
    pAsset6.no_items_locale = "list_empty_favorites_items";
    pAsset6.art_path = "ui/illustrations/art_favorite_items";
    pAsset6.icon_path = "ui/Icons/iconFavoriteItemsList";
    pAsset6.set_list_component = (ListComponentSetter) (pTransform => (IComponentList) ((Component) pTransform).AddComponent<FavoriteItemListComponent>());
    this.add(pAsset6);
    ListWindowAsset pAsset7 = new ListWindowAsset();
    pAsset7.id = "list_favorite_units";
    pAsset7.meta_type = MetaType.Unit;
    pAsset7.no_items_locale = "list_empty_favorite_units";
    pAsset7.art_path = "ui/illustrations/art_favorite_units";
    pAsset7.icon_path = "ui/Icons/iconFavoritesList";
    pAsset7.set_list_component = (ListComponentSetter) (pTransform => (IComponentList) ((Component) pTransform).AddComponent<WindowFavorites>());
    this.add(pAsset7);
    ListWindowAsset pAsset8 = new ListWindowAsset();
    pAsset8.id = "list_kingdoms";
    pAsset8.meta_type = MetaType.Kingdom;
    pAsset8.no_items_locale = "list_empty_kingdoms";
    pAsset8.art_path = "ui/illustrations/art_kingdoms";
    pAsset8.icon_path = "ui/Icons/iconKingdomList";
    pAsset8.set_list_component = (ListComponentSetter) (pTransform => (IComponentList) ((Component) pTransform).AddComponent<KingdomListComponent>());
    this.add(pAsset8);
    ListWindowAsset pAsset9 = new ListWindowAsset();
    pAsset9.id = "list_languages";
    pAsset9.meta_type = MetaType.Language;
    pAsset9.no_items_locale = "list_empty_languages";
    pAsset9.art_path = "ui/illustrations/art_languages";
    pAsset9.icon_path = "ui/Icons/iconLanguageList";
    pAsset9.set_list_component = (ListComponentSetter) (pTransform => (IComponentList) ((Component) pTransform).AddComponent<LanguageListComponent>());
    this.add(pAsset9);
    ListWindowAsset pAsset10 = new ListWindowAsset();
    pAsset10.id = "list_plots";
    pAsset10.meta_type = MetaType.Plot;
    pAsset10.no_items_locale = "list_empty_plots";
    pAsset10.art_path = "ui/illustrations/art_plots";
    pAsset10.icon_path = "ui/Icons/iconPlotList";
    pAsset10.set_list_component = (ListComponentSetter) (pTransform => (IComponentList) ((Component) pTransform).AddComponent<PlotListComponent>());
    this.add(pAsset10);
    ListWindowAsset pAsset11 = new ListWindowAsset();
    pAsset11.id = "list_religions";
    pAsset11.meta_type = MetaType.Religion;
    pAsset11.no_items_locale = "list_empty_religions";
    pAsset11.art_path = "ui/illustrations/art_religions";
    pAsset11.icon_path = "ui/Icons/iconReligionList";
    pAsset11.set_list_component = (ListComponentSetter) (pTransform => (IComponentList) ((Component) pTransform).AddComponent<ReligionListComponent>());
    this.add(pAsset11);
    ListWindowAsset pAsset12 = new ListWindowAsset();
    pAsset12.id = "list_subspecies";
    pAsset12.meta_type = MetaType.Subspecies;
    pAsset12.no_items_locale = "list_empty_subspecies";
    pAsset12.art_path = "ui/illustrations/art_subspecies";
    pAsset12.icon_path = "ui/Icons/iconSubspeciesList";
    pAsset12.set_list_component = (ListComponentSetter) (pTransform => (IComponentList) ((Component) pTransform).AddComponent<SubspeciesListComponent>());
    this.add(pAsset12);
    ListWindowAsset pAsset13 = new ListWindowAsset();
    pAsset13.id = "list_wars";
    pAsset13.meta_type = MetaType.War;
    pAsset13.no_items_locale = "list_empty_wars";
    pAsset13.no_dead_items_locale = "empty_past_wars_list";
    pAsset13.art_path = "ui/illustrations/art_wars";
    pAsset13.icon_path = "ui/Icons/iconWarList";
    pAsset13.set_list_component = (ListComponentSetter) (pTransform => (IComponentList) ((Component) pTransform).AddComponent<WarListComponent>());
    this.add(pAsset13);
    ListWindowAsset pAsset14 = new ListWindowAsset();
    pAsset14.id = "list_armies";
    pAsset14.meta_type = MetaType.Army;
    pAsset14.no_items_locale = "list_empty_armies";
    pAsset14.art_path = "ui/illustrations/art_armies";
    pAsset14.icon_path = "ui/Icons/iconArmyList";
    pAsset14.set_list_component = (ListComponentSetter) (pTransform => (IComponentList) ((Component) pTransform).AddComponent<ArmyListComponent>());
    this.add(pAsset14);
  }

  public override void linkAssets()
  {
    base.linkAssets();
    foreach (ListWindowAsset listWindowAsset in this.list)
      this._dict.Add(listWindowAsset.meta_type, listWindowAsset);
  }

  public override void editorDiagnosticLocales()
  {
    base.editorDiagnosticLocales();
    foreach (ListWindowAsset pAsset in this.list)
    {
      foreach (string localeId in pAsset.getLocaleIDs())
        this.checkLocale((Asset) pAsset, localeId);
    }
  }

  public ListWindowAsset getByMetaType(MetaType pType) => this._dict[pType];
}

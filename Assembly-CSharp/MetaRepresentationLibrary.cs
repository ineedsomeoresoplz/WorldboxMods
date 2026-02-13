// Decompiled with JetBrains decompiler
// Type: MetaRepresentationLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class MetaRepresentationLibrary : AssetLibrary<MetaRepresentationAsset>
{
  public override void init()
  {
    base.init();
    MetaRepresentationAsset pAsset1 = new MetaRepresentationAsset();
    pAsset1.id = "alliance";
    pAsset1.meta_type = MetaType.Alliance;
    pAsset1.title_locale = "statistics_breakdown_alliances";
    pAsset1.icon_getter = (IconPathGetter) (_ => "iconAlliance");
    pAsset1.check_has_meta = (CheckActorHasMeta) (pActor => pActor.hasKingdom() && pActor.kingdom.isCiv() && pActor.kingdom.hasAlliance());
    pAsset1.meta_getter = (GetMetaFromActor) (pActor => (IMetaObject) pActor.kingdom.getAlliance());
    pAsset1.meta_getter_total = (GetMetaTotalFromActor) (pActor => new ListPool<IMetaObject>()
    {
      (IMetaObject) pActor.kingdom.getAlliance()
    });
    pAsset1.world_units_getter = (GetWorldUnits) (() => World.world.units.units_only_civ);
    pAsset1.general_icon_path = "iconAlliance";
    pAsset1.show_species_icon = false;
    this.add(pAsset1);
    MetaRepresentationAsset pAsset2 = new MetaRepresentationAsset();
    pAsset2.id = "army";
    pAsset2.meta_type = MetaType.Army;
    pAsset2.title_locale = "statistics_breakdown_armies";
    pAsset2.icon_getter = (IconPathGetter) (_ => "iconArmy");
    pAsset2.check_has_meta = (CheckActorHasMeta) (pActor => pActor.hasArmy());
    pAsset2.meta_getter = (GetMetaFromActor) (pActor => (IMetaObject) pActor.city.getArmy());
    pAsset2.meta_getter_total = (GetMetaTotalFromActor) (pActor => new ListPool<IMetaObject>()
    {
      (IMetaObject) pActor.city.getArmy()
    });
    pAsset2.world_units_getter = (GetWorldUnits) (() => World.world.units.units_only_civ);
    pAsset2.general_icon_path = "iconArmy";
    this.add(pAsset2);
    MetaRepresentationAsset pAsset3 = new MetaRepresentationAsset();
    pAsset3.id = "city";
    pAsset3.meta_type = MetaType.City;
    pAsset3.title_locale = "statistics_breakdown_cities";
    pAsset3.icon_getter = (IconPathGetter) (_ => "iconCity");
    pAsset3.check_has_meta = (CheckActorHasMeta) (pActor => pActor.hasCity());
    pAsset3.meta_getter = (GetMetaFromActor) (pActor => (IMetaObject) pActor.city);
    pAsset3.meta_getter_total = (GetMetaTotalFromActor) (pActor => new ListPool<IMetaObject>()
    {
      (IMetaObject) pActor.city
    });
    pAsset3.world_units_getter = (GetWorldUnits) (() => World.world.units.units_only_civ);
    pAsset3.general_icon_path = "iconCity";
    this.add(pAsset3);
    MetaRepresentationAsset pAsset4 = new MetaRepresentationAsset();
    pAsset4.id = "clan";
    pAsset4.meta_type = MetaType.Clan;
    pAsset4.title_locale = "statistics_breakdown_clans";
    pAsset4.icon_getter = (IconPathGetter) (_ => "iconClan");
    pAsset4.check_has_meta = (CheckActorHasMeta) (pActor => pActor.hasClan());
    pAsset4.meta_getter = (GetMetaFromActor) (pActor => (IMetaObject) pActor.clan);
    pAsset4.meta_getter_total = (GetMetaTotalFromActor) (pActor => new ListPool<IMetaObject>()
    {
      (IMetaObject) pActor.clan
    });
    pAsset4.world_units_getter = (GetWorldUnits) (() => World.world.units.units_only_civ);
    pAsset4.general_icon_path = "iconClan";
    this.add(pAsset4);
    MetaRepresentationAsset pAsset5 = new MetaRepresentationAsset();
    pAsset5.id = "culture";
    pAsset5.meta_type = MetaType.Culture;
    pAsset5.title_locale = "statistics_breakdown_cultures";
    pAsset5.icon_getter = (IconPathGetter) (_ => "iconCulture");
    pAsset5.check_has_meta = (CheckActorHasMeta) (pActor => pActor.hasCulture());
    pAsset5.meta_getter = (GetMetaFromActor) (pActor => (IMetaObject) pActor.culture);
    pAsset5.meta_getter_total = (GetMetaTotalFromActor) (pActor => new ListPool<IMetaObject>()
    {
      (IMetaObject) pActor.culture
    });
    pAsset5.world_units_getter = (GetWorldUnits) (() => World.world.units.units_only_civ);
    pAsset5.general_icon_path = "iconCulture";
    pAsset5.show_none_percent = true;
    this.add(pAsset5);
    MetaRepresentationAsset pAsset6 = new MetaRepresentationAsset();
    pAsset6.id = "kingdom";
    pAsset6.meta_type = MetaType.Kingdom;
    pAsset6.title_locale = "statistics_breakdown_kingdoms";
    pAsset6.icon_getter = (IconPathGetter) (_ => "iconKingdom");
    pAsset6.check_has_meta = (CheckActorHasMeta) (pActor => pActor.hasKingdom() && pActor.kingdom.isCiv());
    pAsset6.meta_getter = (GetMetaFromActor) (pActor => (IMetaObject) pActor.kingdom);
    pAsset6.meta_getter_total = (GetMetaTotalFromActor) (pActor => new ListPool<IMetaObject>()
    {
      (IMetaObject) pActor.kingdom
    });
    pAsset6.world_units_getter = (GetWorldUnits) (() => World.world.units.units_only_civ);
    pAsset6.general_icon_path = "iconKingdom";
    this.add(pAsset6);
    MetaRepresentationAsset pAsset7 = new MetaRepresentationAsset();
    pAsset7.id = "language";
    pAsset7.meta_type = MetaType.Language;
    pAsset7.title_locale = "statistics_breakdown_languages";
    pAsset7.icon_getter = (IconPathGetter) (_ => "iconLanguage");
    pAsset7.check_has_meta = (CheckActorHasMeta) (pActor => pActor.hasLanguage());
    pAsset7.meta_getter = (GetMetaFromActor) (pActor => (IMetaObject) pActor.language);
    pAsset7.meta_getter_total = (GetMetaTotalFromActor) (pActor => new ListPool<IMetaObject>()
    {
      (IMetaObject) pActor.language
    });
    pAsset7.world_units_getter = (GetWorldUnits) (() => World.world.units.units_only_civ);
    pAsset7.general_icon_path = "iconLanguage";
    pAsset7.show_none_percent = true;
    this.add(pAsset7);
    MetaRepresentationAsset pAsset8 = new MetaRepresentationAsset();
    pAsset8.id = "religion";
    pAsset8.meta_type = MetaType.Religion;
    pAsset8.title_locale = "statistics_breakdown_religions";
    pAsset8.icon_getter = (IconPathGetter) (_ => "iconReligion");
    pAsset8.check_has_meta = (CheckActorHasMeta) (pActor => pActor.hasReligion());
    pAsset8.meta_getter = (GetMetaFromActor) (pActor => (IMetaObject) pActor.religion);
    pAsset8.meta_getter_total = (GetMetaTotalFromActor) (pActor => new ListPool<IMetaObject>()
    {
      (IMetaObject) pActor.religion
    });
    pAsset8.world_units_getter = (GetWorldUnits) (() => World.world.units.units_only_civ);
    pAsset8.general_icon_path = "iconReligion";
    pAsset8.show_none_percent = true;
    this.add(pAsset8);
    MetaRepresentationAsset pAsset9 = new MetaRepresentationAsset();
    pAsset9.id = "subspecies";
    pAsset9.meta_type = MetaType.Subspecies;
    pAsset9.title_locale = "statistics_breakdown_subspecies";
    pAsset9.icon_getter = (IconPathGetter) (pMeta => pMeta.getActorAsset().icon);
    pAsset9.check_has_meta = (CheckActorHasMeta) (pActor => pActor.hasSubspecies());
    pAsset9.meta_getter = (GetMetaFromActor) (pActor => (IMetaObject) pActor.subspecies);
    pAsset9.meta_getter_total = (GetMetaTotalFromActor) (pActor => new ListPool<IMetaObject>()
    {
      (IMetaObject) pActor.subspecies
    });
    pAsset9.world_units_getter = (GetWorldUnits) (() => World.world.units.units_only_alive);
    pAsset9.general_icon_path = "iconSubspecies";
    pAsset9.show_species_icon = false;
    pAsset9.show_none_percent_for_total = false;
    this.add(pAsset9);
    MetaRepresentationAsset pAsset10 = new MetaRepresentationAsset();
    pAsset10.id = "war";
    pAsset10.meta_type = MetaType.War;
    pAsset10.title_locale = "statistics_breakdown_wars";
    pAsset10.icon_getter = (IconPathGetter) (_ => "iconWar");
    pAsset10.check_has_meta = (CheckActorHasMeta) (pActor =>
    {
      if (!pActor.hasKingdom() || !pActor.kingdom.isCiv())
        return false;
      foreach (War war in pActor.kingdom.getWars())
      {
        if (!war.hasEnded())
          return true;
      }
      return false;
    });
    pAsset10.meta_getter_total = (GetMetaTotalFromActor) (pActor => new ListPool<IMetaObject>((IEnumerable<IMetaObject>) pActor.kingdom.getWars()));
    pAsset10.world_units_getter = (GetWorldUnits) (() => World.world.units.units_only_civ);
    pAsset10.general_icon_path = "iconWar";
    pAsset10.show_species_icon = false;
    this.add(pAsset10);
  }

  public MetaRepresentationAsset getAsset(MetaType pType) => this.get(pType.AsString());

  public bool hasAsset(MetaType pType) => this.has(pType.AsString());

  public override void editorDiagnosticLocales()
  {
    base.editorDiagnosticLocales();
    foreach (MetaRepresentationAsset pAsset in this.list)
      this.checkLocale((Asset) pAsset, pAsset.title_locale);
  }
}

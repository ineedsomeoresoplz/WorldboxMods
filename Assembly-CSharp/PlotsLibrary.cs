// Decompiled with JetBrains decompiler
// Type: PlotsLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class PlotsLibrary : BaseLibraryWithUnlockables<PlotAsset>
{
  public static PlotAsset rebellion;
  public static PlotAsset new_war;
  public static PlotAsset alliance_create;
  [NonSerialized]
  public readonly List<PlotAsset> basic_plots = new List<PlotAsset>();

  private static bool tryStartReligionRiteOnEnemyCity(
    Actor pActor,
    PlotAsset pPlotAsset,
    bool pForced)
  {
    using (ListPool<Kingdom> enemiesKingdoms = pActor.kingdom.getEnemiesKingdoms())
    {
      if (enemiesKingdoms.Count == 0)
        return false;
      Kingdom random = enemiesKingdoms.GetRandom<Kingdom>();
      if (!random.hasCities())
        return false;
      World.world.plots.newPlot(pActor, pPlotAsset, pForced).target_city = random.getCities().GetRandom<City>();
      return true;
    }
  }

  private static bool tryStartCauseRebellion(Actor pActor, PlotAsset pPlotAsset, bool pForced)
  {
    using (ListPool<Kingdom> enemiesKingdoms = pActor.kingdom.getEnemiesKingdoms())
    {
      if (enemiesKingdoms.Count == 0)
        return false;
      Kingdom random = enemiesKingdoms.GetRandom<Kingdom>();
      if (!random.hasCities())
        return false;
      using (ListPool<City> list = new ListPool<City>())
      {
        foreach (City city in random.getCities())
        {
          if (!city.isCapitalCity())
            list.Add(city);
        }
        if (list.Count == 0)
          return false;
        World.world.plots.newPlot(pActor, pPlotAsset, pForced).target_city = list.GetRandom<City>();
        return true;
      }
    }
  }

  private static bool tryStartReligionRiteOnSelfCity(
    Actor pActor,
    PlotAsset pPlotAsset,
    bool pForced)
  {
    World.world.plots.newPlot(pActor, pPlotAsset, pForced).target_city = pActor.city;
    return true;
  }

  private static bool tryStartReligionRiteOnSelfKingdom(
    Actor pActor,
    PlotAsset pPlotAsset,
    bool pForced)
  {
    World.world.plots.newPlot(pActor, pPlotAsset, pForced).target_kingdom = pActor.kingdom;
    return true;
  }

  private static bool checkShouldContinueReligionRiteOnEnemyCity(Actor pActor)
  {
    if (!pActor.hasKingdom() || !pActor.hasReligion() || !pActor.kingdom.hasEnemies())
      return false;
    City targetCity = pActor.plot.target_city;
    return targetCity != null && targetCity.isAlive() && targetCity.hasKingdom() && targetCity.kingdom.isEnemy(pActor.kingdom);
  }

  private static bool summonCloudAction(
    Actor pActor,
    string pCloud,
    int pMin,
    int pMax,
    float pLifespanMin,
    float pLifespanMax)
  {
    City targetCity = pActor.plot.target_city;
    int num1 = MapBox.width;
    int num2 = MapBox.height;
    int num3 = 0;
    foreach (TileZone zone in targetCity.zones)
    {
      if (zone.centerTile.x < num1)
        num1 = zone.centerTile.x;
      if (zone.centerTile.y < num2)
        num2 = zone.centerTile.y;
      if (zone.centerTile.y > num3)
        num3 = zone.centerTile.y;
    }
    int num4 = Randy.randomInt(pMin, pMax + 1);
    int num5 = (num3 - num2) / num4;
    for (int index = 0; index < num4; ++index)
    {
      int num6 = Randy.randomInt(-7, 8);
      int pX = Mathf.Clamp(num1 + num6, 0, MapBox.width - 1);
      int pY = num2 + num5 * index;
      ((Cloud) EffectsLibrary.spawn("fx_cloud", MapBox.instance.GetTileSimple(pX, pY), pCloud)).setLifespan(Randy.randomFloat(pLifespanMin, pLifespanMax));
    }
    return true;
  }

  private static bool summonUnitsAction(
    Actor pActor,
    string pActorAssetId,
    int pMin,
    int pMax,
    bool pJoinCasters)
  {
    City targetCity = pActor.plot.target_city;
    if (targetCity == null)
      return false;
    int num = Randy.randomInt(pMin, pMax + 1);
    for (int index = 0; index < num; ++index)
    {
      WorldTile randomTile = targetCity.zones.GetRandom<TileZone>().getRandomTile();
      Actor newUnit = World.world.units.createNewUnit(pActorAssetId, randomTile, pAdultAge: true, pSapientSubspecies: pJoinCasters);
      EffectsLibrary.spawn("fx_spawn", randomTile);
      if (pJoinCasters && newUnit != null && pActor.isKingdomCiv())
      {
        if (newUnit.subspecies.isJustCreated())
          newUnit.subspecies.addTrait("prefrontal_cortex");
        newUnit.joinKingdom(pActor.kingdom);
      }
    }
    return true;
  }

  private static bool afterRiteWorldTransform(Actor pActor)
  {
    if (WorldLawLibrary.world_law_gaias_covenant.isEnabled())
      return false;
    Religion religion = pActor.religion;
    if (religion == null)
      return false;
    using (ListPool<ReligionTrait> list1 = new ListPool<ReligionTrait>())
    {
      foreach (ReligionTrait trait in (IEnumerable<ReligionTrait>) religion.getTraits())
      {
        if (!(trait.group_id != "transformation"))
          list1.Add(trait);
      }
      if (list1.Count == 0)
        return false;
      ReligionTrait random1 = list1.GetRandom<ReligionTrait>();
      TileTypeBase tileHigh = (TileTypeBase) AssetManager.biome_library.get(random1.transformation_biome_id).getTileHigh();
      WorldTile randomTile = World.world.islands_calculator.getRandomIslandGround().getRandomTile();
      WorldBehaviourActionBiomes.trySpreadBiomeAround(randomTile, tileHigh);
      BrushData brushData = Brush.get(2, "special_");
      int length = brushData.pos.Length;
      int num = Randy.randomInt((int) ((double) length * 0.25), (int) ((double) length * 0.5));
      using (ListPool<BrushPixelData> list2 = new ListPool<BrushPixelData>(brushData.pos))
      {
        for (int index = 0; index < num; ++index)
        {
          BrushPixelData random2 = list2.GetRandom<BrushPixelData>();
          list2.Remove(random2);
          WorldTile tile = MapBox.instance.GetTile(randomTile.x + random2.x, randomTile.y + random2.y);
          if (tile != null)
            WorldBehaviourActionBiomes.trySpreadBiomeAround(tile, tileHigh, pForce: true);
        }
        return true;
      }
    }
  }

  public override void init()
  {
    Debug.Log((object) "Init PlotLibrary");
    base.init();
    this.addBasic();
    this.addBasicMetas();
    this.addMagicRites();
  }

  public override void linkAssets()
  {
    base.linkAssets();
    foreach (PlotAsset plotAsset in this.list)
    {
      if (plotAsset.is_basic_plot)
        this.basic_plots.Add(plotAsset);
    }
  }

  public override PlotAsset add(PlotAsset pAsset)
  {
    base.add(pAsset);
    pAsset.get_formatted_description = new PlotDescription(PlotsLibrary.getDescriptionGeneric);
    return pAsset;
  }

  public override void editorDiagnosticLocales()
  {
    foreach (PlotAsset pAsset in this.list)
    {
      this.checkLocale((Asset) pAsset, pAsset.getLocaleID());
      this.checkLocale((Asset) pAsset, pAsset.getDescriptionID());
      this.checkLocale((Asset) pAsset, pAsset.getDescriptionID2());
    }
    base.editorDiagnosticLocales();
  }

  private static string getDescriptionGeneric(Plot pPlot)
  {
    string pString = pPlot.getAsset().getDescriptionID().Localize();
    Actor author = pPlot.getAuthor();
    string name = author?.name;
    return PlotsLibrary.tryToReplace(PlotsLibrary.tryToReplace(PlotsLibrary.tryToReplace(PlotsLibrary.tryToReplace(PlotsLibrary.tryToReplace(PlotsLibrary.tryToReplace(pString, "$initiator_actor$", name), "$initiator_kingdom$", author?.kingdom?.name), "$initiator_city$", author?.city?.name), "$target_kingdom$", pPlot.target_kingdom?.name), "$target_alliance$", pPlot.target_alliance?.name), "$target_war$", pPlot.target_war?.name);
  }

  private static string tryToReplace(string pString, string pReplaceID, string pName)
  {
    if (pString.Contains(pReplaceID))
      pString = pString.Replace(pReplaceID, pName);
    return pString;
  }

  public void addBasic()
  {
    // ISSUE: unable to decompile the method.
  }

  public void addBasicMetas()
  {
    PlotAsset plotAsset1 = new PlotAsset();
    plotAsset1.id = "new_language";
    plotAsset1.is_basic_plot = true;
    plotAsset1.path_icon = "plots/icons/plot_new_language";
    plotAsset1.group_id = "language";
    plotAsset1.priority = 99;
    plotAsset1.min_level = 1;
    plotAsset1.money_cost = 0;
    plotAsset1.min_intelligence = 3;
    plotAsset1.can_be_done_by_king = true;
    plotAsset1.can_be_done_by_leader = true;
    plotAsset1.check_is_possible = (PlotCheckerDelegate) (pActor => !pActor.hasLanguage() && pActor.subspecies.has_advanced_communication);
    plotAsset1.check_should_continue = (PlotCheckerDelegate) (pActor => !pActor.hasLanguage() && pActor.subspecies.has_advanced_communication);
    plotAsset1.action = (PlotAction) (pActor =>
    {
      Language pLanguage = World.world.languages.newLanguage(pActor, true);
      pActor.joinLanguage(pLanguage);
      pLanguage.forceConvertSameSpeciesAroundUnit(pActor);
      return true;
    });
    PlotAsset pAsset1 = plotAsset1;
    this.t = plotAsset1;
    this.add(pAsset1);
    this.t.check_can_be_forced = (PlotCheckerDelegate) (pActor => pActor.subspecies.has_advanced_communication);
    PlotAsset plotAsset2 = new PlotAsset();
    plotAsset2.id = "new_religion";
    plotAsset2.path_icon = "plots/icons/plot_new_religion";
    plotAsset2.group_id = "religion";
    plotAsset2.priority = 99;
    plotAsset2.min_level = 5;
    plotAsset2.money_cost = 25;
    plotAsset2.min_intelligence = 8;
    plotAsset2.is_basic_plot = true;
    plotAsset2.can_be_done_by_king = true;
    plotAsset2.can_be_done_by_leader = true;
    plotAsset2.check_is_possible = (PlotCheckerDelegate) (pActor => !pActor.hasReligion() && pActor.hasCulture() && pActor.hasLanguage() && pActor.subspecies.has_advanced_memory && pActor.culture.countUnits() >= 50);
    plotAsset2.check_should_continue = (PlotCheckerDelegate) (pActor => !pActor.hasReligion() && pActor.subspecies.has_advanced_memory);
    plotAsset2.action = (PlotAction) (pActor =>
    {
      Religion pObject = World.world.religions.newReligion(pActor, true);
      pActor.setReligion(pObject);
      pObject.forceConvertSameSpeciesAroundUnit(pActor);
      return true;
    });
    PlotAsset pAsset2 = plotAsset2;
    this.t = plotAsset2;
    this.add(pAsset2);
    this.t.check_can_be_forced = (PlotCheckerDelegate) (pActor => pActor.subspecies.has_advanced_memory);
    PlotAsset plotAsset3 = new PlotAsset();
    plotAsset3.id = "new_culture";
    plotAsset3.is_basic_plot = true;
    plotAsset3.path_icon = "plots/icons/plot_new_culture";
    plotAsset3.group_id = "culture";
    plotAsset3.priority = 99;
    plotAsset3.min_level = 1;
    plotAsset3.money_cost = 0;
    plotAsset3.min_intelligence = 2;
    plotAsset3.can_be_done_by_king = true;
    plotAsset3.can_be_done_by_leader = true;
    plotAsset3.check_is_possible = (PlotCheckerDelegate) (pActor => !pActor.hasCulture() && pActor.subspecies.has_advanced_memory);
    plotAsset3.check_should_continue = (PlotCheckerDelegate) (pActor => !pActor.hasCulture() && pActor.subspecies.has_advanced_memory);
    plotAsset3.action = (PlotAction) (pActor =>
    {
      Culture pCulture = World.world.cultures.newCulture(pActor, true);
      pActor.setCulture(pCulture);
      pCulture.forceConvertSameSpeciesAroundUnit(pActor);
      return true;
    });
    PlotAsset pAsset3 = plotAsset3;
    this.t = plotAsset3;
    this.add(pAsset3);
    this.t.check_can_be_forced = (PlotCheckerDelegate) (pActor => pActor.subspecies.has_advanced_memory);
    PlotAsset plotAsset4 = new PlotAsset();
    plotAsset4.id = "clan_ascension";
    plotAsset4.path_icon = "plots/icons/plot_clan_ascension";
    plotAsset4.can_be_done_by_king = true;
    plotAsset4.group_id = "rites_various";
    plotAsset4.min_level = 7;
    plotAsset4.money_cost = 100;
    plotAsset4.min_intelligence = 5;
    plotAsset4.check_is_possible = (PlotCheckerDelegate) (pActor => pActor.hasReligion() && pActor.hasClan() && !pActor.clan.hasTrait("mark_of_becoming") && pActor.clan.hasChief() && pActor.clan.getChief() == pActor && pActor.kingdom.getPopulationPeople() >= 500 && pActor.hasTrait("evil") && !pActor.hasSubspeciesTrait("pure"));
    plotAsset4.check_should_continue = (PlotCheckerDelegate) (pActor => pActor.hasClan());
    plotAsset4.action = (PlotAction) (pActor =>
    {
      Clan clan = pActor.clan;
      Actor pEvolvedActorForm;
      if (!ActionLibrary.tryToEvolveUnitViaAscension(pActor, out pEvolvedActorForm))
        return false;
      clan.addTrait("mark_of_becoming");
      Subspecies subspecies = pEvolvedActorForm.subspecies;
      if (clan != null)
      {
        foreach (Actor unit in clan.units)
          unit.setSubspecies(subspecies);
      }
      return true;
    });
    plotAsset4.post_action = new PlotAction(PlotsLibrary.afterRiteWorldTransform);
    PlotAsset pAsset4 = plotAsset4;
    this.t = plotAsset4;
    this.add(pAsset4);
    this.t.check_can_be_forced = (PlotCheckerDelegate) (pActor => !pActor.hasSubspeciesTrait("pure"));
    PlotAsset plotAsset5 = new PlotAsset();
    plotAsset5.id = "culture_divide";
    plotAsset5.is_basic_plot = true;
    plotAsset5.path_icon = "plots/icons/plot_culture_divide";
    plotAsset5.group_id = "culture";
    plotAsset5.can_be_done_by_leader = true;
    plotAsset5.min_level = 5;
    plotAsset5.money_cost = 20;
    plotAsset5.min_intelligence = 6;
    plotAsset5.check_is_possible = (PlotCheckerDelegate) (pActor =>
    {
      if (!pActor.hasCity() || !pActor.hasCulture() || !pActor.hasLanguage() || pActor.isOneCityKingdom())
        return false;
      Culture culture = pActor.culture;
      return culture.countUnits() >= SimGlobals.m.people_before_meta_divide && culture.getAge() >= SimGlobals.m.years_before_meta_divide && !culture.hasTrait("legacy_keepers") && pActor.wantsToSplitMeta() && culture == pActor.kingdom.culture && pActor.subspecies.has_advanced_memory && culture.data.creator_kingdom_id != pActor.kingdom.id && culture.data.creator_clan_id != pActor.clan.id;
    });
    plotAsset5.check_should_continue = (PlotCheckerDelegate) (pActor => pActor.hasCulture() && pActor.subspecies.has_advanced_memory);
    plotAsset5.action = (PlotAction) (pActor =>
    {
      Culture culture = pActor.culture;
      Culture pCulture = World.world.cultures.newCulture(pActor, false);
      pCulture.cloneAndEvolveOnomastics(culture);
      pCulture.copyTraits(culture.getTraits());
      pActor.setCulture(pCulture);
      pCulture.forceConvertSameSpeciesAroundUnit(pActor);
      return true;
    });
    PlotAsset pAsset5 = plotAsset5;
    this.t = plotAsset5;
    this.add(pAsset5);
    this.t.check_can_be_forced = (PlotCheckerDelegate) (pActor => pActor.hasCity() && pActor.hasCulture() && pActor.hasLanguage() && pActor.subspecies.has_advanced_memory);
    PlotAsset plotAsset6 = new PlotAsset();
    plotAsset6.id = "religion_schism";
    plotAsset6.is_basic_plot = true;
    plotAsset6.path_icon = "plots/icons/plot_religion_schism";
    plotAsset6.can_be_done_by_leader = true;
    plotAsset6.group_id = "religion";
    plotAsset6.min_level = 5;
    plotAsset6.money_cost = 15;
    plotAsset6.min_intelligence = 6;
    plotAsset6.check_is_possible = (PlotCheckerDelegate) (pActor =>
    {
      if (!pActor.hasCity() || !pActor.hasCulture() || !pActor.hasLanguage() || !pActor.hasReligion() || pActor.isOneCityKingdom() || !pActor.wantsToSplitMeta() || pActor.culture.hasTrait("legacy_keepers"))
        return false;
      Religion religion = pActor.religion;
      return religion.countUnits() >= SimGlobals.m.people_before_meta_divide && religion.getAge() >= SimGlobals.m.years_before_meta_divide && religion == pActor.kingdom.religion && pActor.subspecies.has_advanced_memory && religion.data.creator_kingdom_id != pActor.kingdom.id && religion.data.creator_clan_id != pActor.clan.id;
    });
    plotAsset6.check_should_continue = (PlotCheckerDelegate) (pActor => pActor.hasReligion() && pActor.subspecies.has_advanced_memory);
    plotAsset6.action = (PlotAction) (pActor =>
    {
      Religion religion = pActor.religion;
      Religion pObject = World.world.religions.newReligion(pActor, false);
      pObject.copyTraits(religion.getTraits());
      pActor.setReligion(pObject);
      pObject.forceConvertSameSpeciesAroundUnit(pActor);
      return true;
    });
    PlotAsset pAsset6 = plotAsset6;
    this.t = plotAsset6;
    this.add(pAsset6);
    this.t.check_can_be_forced = (PlotCheckerDelegate) (pActor => pActor.hasCity() && pActor.hasCulture() && pActor.hasLanguage() && pActor.hasReligion() && pActor.religion == pActor.kingdom.religion && pActor.subspecies.has_advanced_memory);
    PlotAsset plotAsset7 = new PlotAsset();
    plotAsset7.id = "language_divergence";
    plotAsset7.is_basic_plot = true;
    plotAsset7.path_icon = "plots/icons/plot_language_divergence";
    plotAsset7.can_be_done_by_leader = true;
    plotAsset7.group_id = "language";
    plotAsset7.min_level = 5;
    plotAsset7.money_cost = 15;
    plotAsset7.min_intelligence = 8;
    plotAsset7.check_is_possible = (PlotCheckerDelegate) (pActor =>
    {
      if (!pActor.hasCity() || !pActor.hasCulture() || !pActor.hasLanguage() || !pActor.hasReligion() || pActor.isOneCityKingdom() || !pActor.wantsToSplitMeta() || pActor.culture.hasTrait("legacy_keepers"))
        return false;
      Language language = pActor.language;
      return language.countUnits() >= SimGlobals.m.people_before_meta_divide && language.getAge() >= SimGlobals.m.years_before_meta_divide && language == pActor.kingdom.language && pActor.subspecies.has_advanced_communication && language.data.creator_kingdom_id != pActor.kingdom.id && language.data.creator_clan_id != pActor.clan.id;
    });
    plotAsset7.check_should_continue = (PlotCheckerDelegate) (pActor => pActor.hasLanguage() && pActor.subspecies.has_advanced_communication);
    plotAsset7.action = (PlotAction) (pActor =>
    {
      Language language = pActor.language;
      Language pLanguage = World.world.languages.newLanguage(pActor, false);
      pLanguage.copyTraits(language.getTraits());
      pActor.joinLanguage(pLanguage);
      pLanguage.forceConvertSameSpeciesAroundUnit(pActor);
      return true;
    });
    PlotAsset pAsset7 = plotAsset7;
    this.t = plotAsset7;
    this.add(pAsset7);
    this.t.check_can_be_forced = (PlotCheckerDelegate) (pActor => pActor.hasCity() && pActor.hasCulture() && pActor.hasLanguage() && pActor.hasReligion() && pActor.subspecies.has_advanced_communication);
  }

  public void addMagicRites()
  {
    PlotAsset plotAsset1 = new PlotAsset();
    plotAsset1.id = "summon_meteor_rain";
    plotAsset1.path_icon = "plots/icons/plot_summon_meteor_rain";
    plotAsset1.group_id = "rites_wrathful";
    plotAsset1.can_be_done_by_king = true;
    plotAsset1.can_be_done_by_leader = true;
    plotAsset1.check_target_city = true;
    plotAsset1.progress_needed = 200f;
    plotAsset1.min_level = 13;
    plotAsset1.money_cost = 1000;
    plotAsset1.check_is_possible = (PlotCheckerDelegate) (pActor => pActor.kingdom.hasEnemies());
    plotAsset1.try_to_start_advanced = new PlotTryToStart(PlotsLibrary.tryStartReligionRiteOnEnemyCity);
    plotAsset1.check_should_continue = new PlotCheckerDelegate(PlotsLibrary.checkShouldContinueReligionRiteOnEnemyCity);
    plotAsset1.action = (PlotAction) (pActor =>
    {
      City targetCity = pActor.plot.target_city;
      int num = Randy.randomInt(3, 7 + 1);
      for (int index = 0; index < num; ++index)
        Meteorite.spawnMeteoriteDisaster(targetCity.zones.GetRandom<TileZone>().getRandomTile(), pActor);
      return true;
    });
    plotAsset1.post_action = new PlotAction(PlotsLibrary.afterRiteWorldTransform);
    PlotAsset pAsset1 = plotAsset1;
    this.t = plotAsset1;
    this.add(pAsset1);
    this.t.check_can_be_forced = this.t.check_is_possible;
    PlotAsset plotAsset2 = new PlotAsset();
    plotAsset2.id = "summon_earthquake";
    plotAsset2.path_icon = "plots/icons/plot_summon_earthquake";
    plotAsset2.group_id = "rites_wrathful";
    plotAsset2.can_be_done_by_king = true;
    plotAsset2.can_be_done_by_leader = true;
    plotAsset2.check_target_city = true;
    plotAsset2.progress_needed = 200f;
    plotAsset2.min_level = 13;
    plotAsset2.money_cost = 600;
    plotAsset2.pot_rate = 2;
    plotAsset2.check_is_possible = (PlotCheckerDelegate) (pActor => pActor.hasKingdom() && pActor.kingdom.hasEnemies());
    plotAsset2.try_to_start_advanced = new PlotTryToStart(PlotsLibrary.tryStartReligionRiteOnEnemyCity);
    plotAsset2.check_should_continue = new PlotCheckerDelegate(PlotsLibrary.checkShouldContinueReligionRiteOnEnemyCity);
    plotAsset2.action = (PlotAction) (pActor =>
    {
      Earthquake.startQuake(pActor.plot.target_city.zones.GetRandom<TileZone>().getRandomTile(), EarthquakeType.SmallDisaster);
      return true;
    });
    plotAsset2.post_action = new PlotAction(PlotsLibrary.afterRiteWorldTransform);
    PlotAsset pAsset2 = plotAsset2;
    this.t = plotAsset2;
    this.add(pAsset2);
    this.t.check_can_be_forced = this.t.check_is_possible;
    PlotAsset plotAsset3 = new PlotAsset();
    plotAsset3.id = "summon_thunderstorm";
    plotAsset3.path_icon = "plots/icons/plot_summon_thunderstorm";
    plotAsset3.group_id = "rites_wrathful";
    plotAsset3.can_be_done_by_king = true;
    plotAsset3.can_be_done_by_leader = true;
    plotAsset3.check_target_city = true;
    plotAsset3.progress_needed = 200f;
    plotAsset3.min_level = 13;
    plotAsset3.money_cost = 500;
    plotAsset3.pot_rate = 4;
    plotAsset3.check_is_possible = (PlotCheckerDelegate) (pActor => pActor.hasKingdom() && pActor.kingdom.hasEnemies());
    plotAsset3.try_to_start_advanced = new PlotTryToStart(PlotsLibrary.tryStartReligionRiteOnEnemyCity);
    plotAsset3.check_should_continue = new PlotCheckerDelegate(PlotsLibrary.checkShouldContinueReligionRiteOnEnemyCity);
    plotAsset3.action = (PlotAction) (pActor =>
    {
      City targetCity = pActor.plot.target_city;
      if (!targetCity.hasZones())
        return false;
      WorldTile randomTile = targetCity.zones.GetRandom<TileZone>().getRandomTile();
      MapBox.spawnLightningMedium(randomTile, 0.15f, pActor);
      int num = Randy.randomInt(5, 13);
      for (int index = 0; index < num; ++index)
      {
        WorldTile tTargetTileL = Toolbox.getRandomTileWithinDistance(randomTile, 10);
        DelayedActionsManager.addAction((Action) (() => MapBox.spawnLightningMedium(tTargetTileL, 0.15f, pActor)), Randy.randomFloat(0.1f, 0.75f) * (float) index);
      }
      return true;
    });
    plotAsset3.post_action = new PlotAction(PlotsLibrary.afterRiteWorldTransform);
    PlotAsset pAsset3 = plotAsset3;
    this.t = plotAsset3;
    this.add(pAsset3);
    this.t.check_can_be_forced = this.t.check_is_possible;
    PlotAsset plotAsset4 = new PlotAsset();
    plotAsset4.id = "summon_stormfront";
    plotAsset4.path_icon = "plots/icons/plot_summon_stormfront";
    plotAsset4.group_id = "rites_wrathful";
    plotAsset4.can_be_done_by_king = true;
    plotAsset4.can_be_done_by_leader = true;
    plotAsset4.check_target_city = true;
    plotAsset4.progress_needed = 100f;
    plotAsset4.min_level = 13;
    plotAsset4.money_cost = 200;
    plotAsset4.check_is_possible = (PlotCheckerDelegate) (pActor => pActor.hasKingdom() && pActor.kingdom.hasEnemies());
    plotAsset4.try_to_start_advanced = new PlotTryToStart(PlotsLibrary.tryStartReligionRiteOnEnemyCity);
    plotAsset4.check_should_continue = new PlotCheckerDelegate(PlotsLibrary.checkShouldContinueReligionRiteOnEnemyCity);
    plotAsset4.action = (PlotAction) (pActor => PlotsLibrary.summonCloudAction(pActor, "cloud_lightning", 5, 15, 20f, 60f));
    plotAsset4.post_action = new PlotAction(PlotsLibrary.afterRiteWorldTransform);
    PlotAsset pAsset4 = plotAsset4;
    this.t = plotAsset4;
    this.add(pAsset4);
    this.t.check_can_be_forced = this.t.check_is_possible;
    PlotAsset plotAsset5 = new PlotAsset();
    plotAsset5.id = "summon_hellstorm";
    plotAsset5.path_icon = "plots/icons/plot_summon_hellstorm";
    plotAsset5.group_id = "rites_wrathful";
    plotAsset5.can_be_done_by_king = true;
    plotAsset5.can_be_done_by_leader = true;
    plotAsset5.check_target_city = true;
    plotAsset5.progress_needed = 100f;
    plotAsset5.min_level = 13;
    plotAsset5.money_cost = 300;
    plotAsset5.pot_rate = 4;
    plotAsset5.check_is_possible = (PlotCheckerDelegate) (pActor => pActor.hasKingdom() && pActor.kingdom.hasEnemies());
    plotAsset5.try_to_start_advanced = new PlotTryToStart(PlotsLibrary.tryStartReligionRiteOnEnemyCity);
    plotAsset5.check_should_continue = new PlotCheckerDelegate(PlotsLibrary.checkShouldContinueReligionRiteOnEnemyCity);
    plotAsset5.action = (PlotAction) (pActor => PlotsLibrary.summonCloudAction(pActor, "cloud_fire", 3, 5, 20f, 60f));
    plotAsset5.post_action = new PlotAction(PlotsLibrary.afterRiteWorldTransform);
    PlotAsset pAsset5 = plotAsset5;
    this.t = plotAsset5;
    this.add(pAsset5);
    this.t.check_can_be_forced = this.t.check_is_possible;
    PlotAsset plotAsset6 = new PlotAsset();
    plotAsset6.id = "summon_demons";
    plotAsset6.path_icon = "plots/icons/plot_summon_demons";
    plotAsset6.group_id = "rites_summoning";
    plotAsset6.can_be_done_by_king = true;
    plotAsset6.can_be_done_by_leader = true;
    plotAsset6.check_target_city = true;
    plotAsset6.progress_needed = 100f;
    plotAsset6.min_level = 13;
    plotAsset6.money_cost = 300;
    plotAsset6.check_is_possible = (PlotCheckerDelegate) (pActor => pActor.hasKingdom() && pActor.kingdom.hasEnemies());
    plotAsset6.try_to_start_advanced = new PlotTryToStart(PlotsLibrary.tryStartReligionRiteOnEnemyCity);
    plotAsset6.check_should_continue = new PlotCheckerDelegate(PlotsLibrary.checkShouldContinueReligionRiteOnEnemyCity);
    plotAsset6.action = (PlotAction) (pActor =>
    {
      if (Randy.randomChance(0.0666f))
        pActor.plot.target_city = pActor.city;
      return PlotsLibrary.summonUnitsAction(pActor, "demon", 5, 10, false);
    });
    plotAsset6.post_action = new PlotAction(PlotsLibrary.afterRiteWorldTransform);
    PlotAsset pAsset6 = plotAsset6;
    this.t = plotAsset6;
    this.add(pAsset6);
    this.t.check_can_be_forced = this.t.check_is_possible;
    PlotAsset plotAsset7 = new PlotAsset();
    plotAsset7.id = "summon_angles";
    plotAsset7.path_icon = "plots/icons/plot_summon_angles";
    plotAsset7.group_id = "rites_summoning";
    plotAsset7.can_be_done_by_king = true;
    plotAsset7.can_be_done_by_leader = true;
    plotAsset7.check_target_city = true;
    plotAsset7.progress_needed = 100f;
    plotAsset7.min_level = 12;
    plotAsset7.money_cost = 200;
    plotAsset7.pot_rate = 2;
    plotAsset7.check_is_possible = (PlotCheckerDelegate) (pActor => pActor.hasKingdom() && pActor.kingdom.hasEnemies());
    plotAsset7.try_to_start_advanced = new PlotTryToStart(PlotsLibrary.tryStartReligionRiteOnEnemyCity);
    plotAsset7.check_should_continue = new PlotCheckerDelegate(PlotsLibrary.checkShouldContinueReligionRiteOnEnemyCity);
    plotAsset7.action = (PlotAction) (pActor => PlotsLibrary.summonUnitsAction(pActor, "angle", 3, 7, true));
    plotAsset7.post_action = new PlotAction(PlotsLibrary.afterRiteWorldTransform);
    PlotAsset pAsset7 = plotAsset7;
    this.t = plotAsset7;
    this.add(pAsset7);
    this.t.check_can_be_forced = this.t.check_is_possible;
    PlotAsset plotAsset8 = new PlotAsset();
    plotAsset8.id = "summon_skeletons";
    plotAsset8.path_icon = "plots/icons/plot_summon_skeletons";
    plotAsset8.group_id = "rites_summoning";
    plotAsset8.can_be_done_by_king = true;
    plotAsset8.can_be_done_by_leader = true;
    plotAsset8.check_target_city = true;
    plotAsset8.progress_needed = 100f;
    plotAsset8.min_level = 13;
    plotAsset8.money_cost = 150;
    plotAsset8.pot_rate = 2;
    plotAsset8.check_is_possible = (PlotCheckerDelegate) (pActor => pActor.hasKingdom() && pActor.kingdom.hasEnemies());
    plotAsset8.try_to_start_advanced = new PlotTryToStart(PlotsLibrary.tryStartReligionRiteOnEnemyCity);
    plotAsset8.check_should_continue = new PlotCheckerDelegate(PlotsLibrary.checkShouldContinueReligionRiteOnEnemyCity);
    plotAsset8.action = (PlotAction) (pActor => PlotsLibrary.summonUnitsAction(pActor, "skeleton", 6, 13, true));
    plotAsset8.post_action = new PlotAction(PlotsLibrary.afterRiteWorldTransform);
    PlotAsset pAsset8 = plotAsset8;
    this.t = plotAsset8;
    this.add(pAsset8);
    this.t.check_can_be_forced = this.t.check_is_possible;
    PlotAsset plotAsset9 = new PlotAsset();
    plotAsset9.id = "summon_living_plants";
    plotAsset9.path_icon = "plots/icons/plot_summon_living_plants";
    plotAsset9.group_id = "rites_summoning";
    plotAsset9.can_be_done_by_king = true;
    plotAsset9.can_be_done_by_leader = true;
    plotAsset9.check_target_city = true;
    plotAsset9.progress_needed = 100f;
    plotAsset9.min_level = 13;
    plotAsset9.money_cost = 400;
    plotAsset9.pot_rate = 2;
    plotAsset9.check_is_possible = (PlotCheckerDelegate) (pActor => pActor.hasKingdom() && pActor.kingdom.hasEnemies() && AssetManager.actor_library.get("living_plants").units.Count <= 100);
    plotAsset9.try_to_start_advanced = new PlotTryToStart(PlotsLibrary.tryStartReligionRiteOnEnemyCity);
    plotAsset9.check_should_continue = new PlotCheckerDelegate(PlotsLibrary.checkShouldContinueReligionRiteOnEnemyCity);
    plotAsset9.action = (PlotAction) (pActor =>
    {
      foreach (WorldTile calculatedFarmField in (ObjectContainer<WorldTile>) pActor.plot.target_city.calculated_farm_fields)
      {
        if (calculatedFarmField.hasBuilding())
          ActionLibrary.tryToMakeFloraAlive(calculatedFarmField.building, false);
      }
      return true;
    });
    plotAsset9.post_action = new PlotAction(PlotsLibrary.afterRiteWorldTransform);
    PlotAsset pAsset9 = plotAsset9;
    this.t = plotAsset9;
    this.add(pAsset9);
    this.t.check_can_be_forced = this.t.check_is_possible;
    PlotAsset plotAsset10 = new PlotAsset();
    plotAsset10.id = "big_cast_coffee";
    plotAsset10.path_icon = "plots/icons/plot_big_cast_coffee";
    plotAsset10.group_id = "rites_merciful";
    plotAsset10.can_be_done_by_king = true;
    plotAsset10.can_be_done_by_leader = true;
    plotAsset10.check_target_city = true;
    plotAsset10.progress_needed = 60f;
    plotAsset10.min_level = 9;
    plotAsset10.money_cost = 100;
    plotAsset10.pot_rate = 2;
    plotAsset10.check_is_possible = (PlotCheckerDelegate) (pActor => pActor.hasCity());
    plotAsset10.try_to_start_advanced = new PlotTryToStart(PlotsLibrary.tryStartReligionRiteOnSelfCity);
    plotAsset10.check_should_continue = (PlotCheckerDelegate) (pActor => pActor.hasCity());
    plotAsset10.action = (PlotAction) (pActor =>
    {
      foreach (BaseSimObject unit in pActor.plot.target_city.getUnits())
        unit.addStatusEffect("caffeinated");
      return true;
    });
    plotAsset10.post_action = new PlotAction(PlotsLibrary.afterRiteWorldTransform);
    PlotAsset pAsset10 = plotAsset10;
    this.t = plotAsset10;
    this.add(pAsset10);
    this.t.check_can_be_forced = this.t.check_is_possible;
    PlotAsset plotAsset11 = new PlotAsset();
    plotAsset11.id = "big_cast_bubble_shield";
    plotAsset11.path_icon = "plots/icons/plot_big_cast_bubble_shield";
    plotAsset11.group_id = "rites_merciful";
    plotAsset11.can_be_done_by_king = true;
    plotAsset11.can_be_done_by_leader = true;
    plotAsset11.check_target_kingdom = true;
    plotAsset11.progress_needed = 50f;
    plotAsset11.min_level = 11;
    plotAsset11.money_cost = 200;
    plotAsset11.pot_rate = 3;
    plotAsset11.check_is_possible = (PlotCheckerDelegate) (pActor => pActor.hasKingdom());
    plotAsset11.try_to_start_advanced = new PlotTryToStart(PlotsLibrary.tryStartReligionRiteOnSelfKingdom);
    plotAsset11.check_should_continue = (PlotCheckerDelegate) (pActor => pActor.hasKingdom());
    plotAsset11.action = (PlotAction) (pActor =>
    {
      foreach (Actor unit in pActor.plot.target_kingdom.getUnits())
      {
        if (unit.isProfession(UnitProfession.Warrior))
          unit.addStatusEffect("shield");
      }
      return true;
    });
    plotAsset11.post_action = new PlotAction(PlotsLibrary.afterRiteWorldTransform);
    PlotAsset pAsset11 = plotAsset11;
    this.t = plotAsset11;
    this.add(pAsset11);
    this.t.check_can_be_forced = this.t.check_is_possible;
    PlotAsset plotAsset12 = new PlotAsset();
    plotAsset12.id = "big_cast_madness";
    plotAsset12.path_icon = "plots/icons/plot_big_cast_madness";
    plotAsset12.group_id = "rites_wrathful";
    plotAsset12.can_be_done_by_king = true;
    plotAsset12.can_be_done_by_leader = true;
    plotAsset12.check_target_city = true;
    plotAsset12.progress_needed = 120f;
    plotAsset12.min_level = 11;
    plotAsset12.money_cost = 600;
    plotAsset12.check_is_possible = (PlotCheckerDelegate) (pActor => pActor.hasKingdom() && pActor.kingdom.hasEnemies());
    plotAsset12.try_to_start_advanced = new PlotTryToStart(PlotsLibrary.tryStartReligionRiteOnEnemyCity);
    plotAsset12.check_should_continue = new PlotCheckerDelegate(PlotsLibrary.checkShouldContinueReligionRiteOnEnemyCity);
    plotAsset12.action = (PlotAction) (pActor =>
    {
      City targetCity = pActor.plot.target_city;
      float pVal = 0.05f;
      foreach (Actor unit in targetCity.getUnits())
      {
        if (Randy.randomChance(pVal))
          unit.addTrait("madness");
      }
      return true;
    });
    plotAsset12.post_action = new PlotAction(PlotsLibrary.afterRiteWorldTransform);
    PlotAsset pAsset12 = plotAsset12;
    this.t = plotAsset12;
    this.add(pAsset12);
    this.t.check_can_be_forced = this.t.check_is_possible;
    PlotAsset plotAsset13 = new PlotAsset();
    plotAsset13.id = "big_cast_slowness";
    plotAsset13.path_icon = "plots/icons/plot_big_cast_slowness";
    plotAsset13.group_id = "rites_wrathful";
    plotAsset13.can_be_done_by_king = true;
    plotAsset13.can_be_done_by_leader = true;
    plotAsset13.check_target_city = true;
    plotAsset13.pot_rate = 3;
    plotAsset13.progress_needed = 70f;
    plotAsset13.min_level = 11;
    plotAsset13.money_cost = 150;
    plotAsset13.check_is_possible = (PlotCheckerDelegate) (pActor => pActor.hasKingdom() && pActor.kingdom.hasEnemies());
    plotAsset13.try_to_start_advanced = new PlotTryToStart(PlotsLibrary.tryStartReligionRiteOnEnemyCity);
    plotAsset13.check_should_continue = new PlotCheckerDelegate(PlotsLibrary.checkShouldContinueReligionRiteOnEnemyCity);
    plotAsset13.action = (PlotAction) (pActor =>
    {
      foreach (Actor unit in pActor.plot.target_city.getUnits())
      {
        if (unit.isProfession(UnitProfession.Warrior))
          unit.addStatusEffect("slowness");
      }
      return true;
    });
    plotAsset13.post_action = new PlotAction(PlotsLibrary.afterRiteWorldTransform);
    PlotAsset pAsset13 = plotAsset13;
    this.t = plotAsset13;
    this.add(pAsset13);
    this.t.check_can_be_forced = this.t.check_is_possible;
    PlotAsset plotAsset14 = new PlotAsset();
    plotAsset14.id = "cause_rebellion";
    plotAsset14.path_icon = "plots/icons/plot_cause_rebellion";
    plotAsset14.group_id = "rites_wrathful";
    plotAsset14.can_be_done_by_king = true;
    plotAsset14.can_be_done_by_leader = true;
    plotAsset14.check_target_city = true;
    plotAsset14.pot_rate = 2;
    plotAsset14.progress_needed = 70f;
    plotAsset14.min_level = 12;
    plotAsset14.money_cost = 400;
    plotAsset14.check_is_possible = (PlotCheckerDelegate) (pActor => pActor.hasKingdom() && pActor.kingdom.hasEnemies());
    plotAsset14.try_to_start_advanced = new PlotTryToStart(PlotsLibrary.tryStartCauseRebellion);
    plotAsset14.check_should_continue = new PlotCheckerDelegate(PlotsLibrary.checkShouldContinueReligionRiteOnEnemyCity);
    plotAsset14.action = (PlotAction) (pActor =>
    {
      City targetCity = pActor.plot.target_city;
      if (targetCity.isCapitalCity() || targetCity.units.Count == 0)
        return false;
      Actor random = targetCity.getUnits().GetRandom<Actor>();
      if (!random.isAlive())
        return false;
      DiplomacyHelpersRebellion.startRebellion(random, pActor.plot, false);
      return true;
    });
    plotAsset14.post_action = new PlotAction(PlotsLibrary.afterRiteWorldTransform);
    PlotAsset pAsset14 = plotAsset14;
    this.t = plotAsset14;
    this.add(pAsset14);
    this.t.check_can_be_forced = this.t.check_is_possible;
  }
}

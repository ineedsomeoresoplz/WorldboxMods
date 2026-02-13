// Decompiled with JetBrains decompiler
// Type: WorldLawLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class WorldLawLibrary : BaseLibraryWithUnlockables<WorldLawAsset>
{
  public static WorldLawAsset world_law_diplomacy;
  public static WorldLawAsset world_law_rites;
  public static WorldLawAsset world_law_peaceful_monsters;
  public static WorldLawAsset world_law_hunger;
  public static WorldLawAsset world_law_vegetation_random_seeds;
  public static WorldLawAsset world_law_roots_without_borders;
  public static WorldLawAsset world_law_spread_trees;
  public static WorldLawAsset world_law_spread_fungi;
  public static WorldLawAsset world_law_spread_plants;
  public static WorldLawAsset world_law_spread_fast_trees;
  public static WorldLawAsset world_law_spread_fast_fungi;
  public static WorldLawAsset world_law_spread_fast_plants;
  public static WorldLawAsset world_law_exploding_mushrooms;
  public static WorldLawAsset world_law_entanglewood;
  public static WorldLawAsset world_law_bark_bites_back;
  public static WorldLawAsset world_law_plants_tickles;
  public static WorldLawAsset world_law_root_pranks;
  public static WorldLawAsset world_law_nectar_nap;
  public static WorldLawAsset world_law_spread_density_high;
  public static WorldLawAsset world_law_grow_minerals;
  public static WorldLawAsset world_law_grow_grass;
  public static WorldLawAsset world_law_biome_overgrowth;
  public static WorldLawAsset world_law_kingdom_expansion;
  public static WorldLawAsset world_law_old_age;
  public static WorldLawAsset world_law_animals_spawn;
  public static WorldLawAsset world_law_animals_babies;
  public static WorldLawAsset world_law_rebellions;
  public static WorldLawAsset world_law_border_stealing;
  public static WorldLawAsset world_law_erosion;
  public static WorldLawAsset world_law_forever_lava;
  public static WorldLawAsset world_law_forever_cold;
  public static WorldLawAsset world_law_disasters_nature;
  public static WorldLawAsset world_law_disasters_other;
  public static WorldLawAsset world_law_rat_plague;
  public static WorldLawAsset world_law_angry_civilians;
  public static WorldLawAsset world_law_civ_babies;
  public static WorldLawAsset world_law_civ_migrants;
  public static WorldLawAsset world_law_forever_tumor_creep;
  public static WorldLawAsset world_law_civ_army;
  public static WorldLawAsset world_law_civ_limit_population_100;
  public static WorldLawAsset world_law_gaias_covenant;
  public static WorldLawAsset world_law_clouds;
  public static WorldLawAsset world_law_evolution_events;
  public static WorldLawAsset world_law_terramorphing;
  public static WorldLawAsset world_law_gene_spaghetti;
  public static WorldLawAsset world_law_mutant_box;
  public static WorldLawAsset world_law_glitched_noosphere;
  public static WorldLawAsset world_law_drop_of_thoughts;
  public static WorldLawAsset world_law_cursed_world;
  public static WorldLawAsset world_law_army_no_cross_foreign;
  public static WorldLawAsset world_law_keep_off_my_land;

  public override void init()
  {
    base.init();
    this.addFlora();
    WorldLawAsset pAsset1 = new WorldLawAsset();
    pAsset1.id = "world_law_gene_spaghetti";
    pAsset1.group_id = "units";
    pAsset1.icon_path = "ui/Icons/worldrules/icon_gene_spaghetti";
    pAsset1.default_state = false;
    WorldLawLibrary.world_law_gene_spaghetti = this.add(pAsset1);
    WorldLawAsset pAsset2 = new WorldLawAsset();
    pAsset2.id = "world_law_mutant_box";
    pAsset2.group_id = "units";
    pAsset2.icon_path = "ui/Icons/worldrules/icon_mutant_box";
    pAsset2.default_state = false;
    WorldLawLibrary.world_law_mutant_box = this.add(pAsset2);
    WorldLawAsset pAsset3 = new WorldLawAsset();
    pAsset3.id = "world_law_glitched_noosphere";
    pAsset3.group_id = "civilizations";
    pAsset3.icon_path = "ui/Icons/worldrules/icon_glitched_noosphere";
    pAsset3.default_state = false;
    WorldLawLibrary.world_law_glitched_noosphere = this.add(pAsset3);
    WorldLawAsset pAsset4 = new WorldLawAsset();
    pAsset4.id = "world_law_drop_of_thoughts";
    pAsset4.group_id = "spawn";
    pAsset4.icon_path = "ui/Icons/worldrules/icon_drop_of_thoughts";
    pAsset4.default_state = false;
    WorldLawLibrary.world_law_drop_of_thoughts = this.add(pAsset4);
    WorldLawAsset pAsset5 = new WorldLawAsset();
    pAsset5.id = "world_law_diplomacy";
    pAsset5.group_id = "diplomacy";
    pAsset5.icon_path = "ui/Icons/worldrules/icon_diplomacy";
    pAsset5.default_state = true;
    pAsset5.on_state_change = new PlayerOptionAction(this.checkDiplomacy);
    WorldLawLibrary.world_law_diplomacy = this.add(pAsset5);
    WorldLawAsset pAsset6 = new WorldLawAsset();
    pAsset6.id = "world_law_rites";
    pAsset6.group_id = "diplomacy";
    pAsset6.icon_path = "ui/Icons/worldrules/icon_rites";
    pAsset6.default_state = true;
    WorldLawLibrary.world_law_rites = this.add(pAsset6);
    WorldLawAsset pAsset7 = new WorldLawAsset();
    pAsset7.id = "world_law_peaceful_monsters";
    pAsset7.group_id = "mobs";
    pAsset7.icon_path = "ui/Icons/worldrules/icon_peacefulanimals";
    pAsset7.default_state = false;
    pAsset7.on_state_change = new PlayerOptionAction(this.checkPeacefulMonsters);
    WorldLawLibrary.world_law_peaceful_monsters = this.add(pAsset7);
    WorldLawAsset pAsset8 = new WorldLawAsset();
    pAsset8.id = "world_law_hunger";
    pAsset8.group_id = "units";
    pAsset8.icon_path = "ui/Icons/worldrules/icon_hunger";
    pAsset8.default_state = true;
    WorldLawLibrary.world_law_hunger = this.add(pAsset8);
    WorldLawAsset pAsset9 = new WorldLawAsset();
    pAsset9.id = "world_law_vegetation_random_seeds";
    pAsset9.group_id = "nature";
    pAsset9.icon_path = "ui/Icons/worldrules/icon_random_seeds";
    pAsset9.default_state = true;
    WorldLawLibrary.world_law_vegetation_random_seeds = this.add(pAsset9);
    WorldLawAsset pAsset10 = new WorldLawAsset();
    pAsset10.id = "world_law_roots_without_borders";
    pAsset10.group_id = "nature";
    pAsset10.icon_path = "ui/Icons/worldrules/icon_roots_without_borders";
    pAsset10.default_state = false;
    WorldLawLibrary.world_law_roots_without_borders = this.add(pAsset10);
    WorldLawAsset pAsset11 = new WorldLawAsset();
    pAsset11.id = "world_law_grow_minerals";
    pAsset11.group_id = "nature";
    pAsset11.icon_path = "ui/Icons/iconStone";
    pAsset11.default_state = true;
    WorldLawLibrary.world_law_grow_minerals = this.add(pAsset11);
    WorldLawAsset pAsset12 = new WorldLawAsset();
    pAsset12.id = "world_law_grow_grass";
    pAsset12.group_id = "biomes";
    pAsset12.icon_path = "ui/Icons/worldrules/icon_growgrass";
    pAsset12.default_state = true;
    WorldLawLibrary.world_law_grow_grass = this.add(pAsset12);
    WorldLawAsset pAsset13 = new WorldLawAsset();
    pAsset13.id = "world_law_biome_overgrowth";
    pAsset13.group_id = "biomes";
    pAsset13.icon_path = "ui/Icons/worldrules/icon_overgrowth";
    pAsset13.default_state = true;
    WorldLawLibrary.world_law_biome_overgrowth = this.add(pAsset13);
    WorldLawAsset pAsset14 = new WorldLawAsset();
    pAsset14.id = "world_law_terramorphing";
    pAsset14.group_id = "civilizations";
    pAsset14.icon_path = "ui/Icons/worldrules/icon_terramorphing";
    pAsset14.default_state = true;
    WorldLawLibrary.world_law_terramorphing = this.add(pAsset14);
    WorldLawAsset pAsset15 = new WorldLawAsset();
    pAsset15.id = "world_law_kingdom_expansion";
    pAsset15.group_id = "civilizations";
    pAsset15.icon_path = "ui/Icons/worldrules/icon_kingdomexpansion";
    pAsset15.default_state = true;
    WorldLawLibrary.world_law_kingdom_expansion = this.add(pAsset15);
    WorldLawAsset pAsset16 = new WorldLawAsset();
    pAsset16.id = "world_law_old_age";
    pAsset16.group_id = "units";
    pAsset16.icon_path = "ui/Icons/worldrules/icon_oldage";
    pAsset16.default_state = true;
    WorldLawLibrary.world_law_old_age = this.add(pAsset16);
    WorldLawAsset pAsset17 = new WorldLawAsset();
    pAsset17.id = "world_law_animals_spawn";
    pAsset17.group_id = "spawn";
    pAsset17.icon_path = "ui/Icons/worldrules/icon_animalspawn";
    pAsset17.default_state = true;
    WorldLawLibrary.world_law_animals_spawn = this.add(pAsset17);
    WorldLawAsset pAsset18 = new WorldLawAsset();
    pAsset18.id = "world_law_animals_babies";
    pAsset18.group_id = "mobs";
    pAsset18.icon_path = "ui/Icons/iconChicken";
    pAsset18.default_state = true;
    WorldLawLibrary.world_law_animals_babies = this.add(pAsset18);
    WorldLawAsset pAsset19 = new WorldLawAsset();
    pAsset19.id = "world_law_rebellions";
    pAsset19.group_id = "diplomacy";
    pAsset19.icon_path = "ui/Icons/worldrules/icon_rebellion";
    pAsset19.default_state = true;
    WorldLawLibrary.world_law_rebellions = this.add(pAsset19);
    WorldLawAsset pAsset20 = new WorldLawAsset();
    pAsset20.id = "world_law_border_stealing";
    pAsset20.group_id = "diplomacy";
    pAsset20.icon_path = "ui/Icons/worldrules/icon_borderstealing";
    pAsset20.default_state = true;
    WorldLawLibrary.world_law_border_stealing = this.add(pAsset20);
    WorldLawAsset pAsset21 = new WorldLawAsset();
    pAsset21.id = "world_law_erosion";
    pAsset21.group_id = "nature";
    pAsset21.icon_path = "ui/Icons/worldrules/icon_erosion";
    pAsset21.default_state = true;
    WorldLawLibrary.world_law_erosion = this.add(pAsset21);
    WorldLawAsset pAsset22 = new WorldLawAsset();
    pAsset22.id = "world_law_forever_lava";
    pAsset22.group_id = "weather";
    pAsset22.icon_path = "ui/Icons/worldrules/icon_foreverlava";
    pAsset22.default_state = false;
    WorldLawLibrary.world_law_forever_lava = this.add(pAsset22);
    WorldLawAsset pAsset23 = new WorldLawAsset();
    pAsset23.id = "world_law_forever_cold";
    pAsset23.group_id = "weather";
    pAsset23.icon_path = "ui/Icons/iconSnow";
    pAsset23.default_state = false;
    WorldLawLibrary.world_law_forever_cold = this.add(pAsset23);
    WorldLawAsset pAsset24 = new WorldLawAsset();
    pAsset24.id = "world_law_disasters_nature";
    pAsset24.group_id = "disasters";
    pAsset24.icon_path = "ui/Icons/worldrules/icon_disasters";
    pAsset24.default_state = true;
    WorldLawLibrary.world_law_disasters_nature = this.add(pAsset24);
    WorldLawAsset pAsset25 = new WorldLawAsset();
    pAsset25.id = "world_law_clouds";
    pAsset25.group_id = "spawn";
    pAsset25.icon_path = "ui/Icons/iconCloud";
    pAsset25.default_state = true;
    WorldLawLibrary.world_law_clouds = this.add(pAsset25);
    WorldLawAsset pAsset26 = new WorldLawAsset();
    pAsset26.id = "world_law_evolution_events";
    pAsset26.group_id = "other";
    pAsset26.icon_path = "ui/Icons/iconMonolith";
    pAsset26.default_state = true;
    WorldLawLibrary.world_law_evolution_events = this.add(pAsset26);
    WorldLawAsset pAsset27 = new WorldLawAsset();
    pAsset27.id = "world_law_disasters_other";
    pAsset27.group_id = "disasters";
    pAsset27.icon_path = "ui/Icons/iconEvilMage";
    pAsset27.default_state = true;
    WorldLawLibrary.world_law_disasters_other = this.add(pAsset27);
    WorldLawAsset pAsset28 = new WorldLawAsset();
    pAsset28.id = "world_law_rat_plague";
    pAsset28.group_id = "disasters";
    pAsset28.icon_path = "ui/Icons/iconRatKing";
    pAsset28.default_state = false;
    WorldLawLibrary.world_law_rat_plague = this.add(pAsset28);
    WorldLawAsset pAsset29 = new WorldLawAsset();
    pAsset29.id = "world_law_angry_civilians";
    pAsset29.group_id = "civilizations";
    pAsset29.icon_path = "ui/Icons/worldrules/icon_angryvillagers";
    pAsset29.default_state = false;
    WorldLawLibrary.world_law_angry_civilians = this.add(pAsset29);
    WorldLawAsset pAsset30 = new WorldLawAsset();
    pAsset30.id = "world_law_civ_babies";
    pAsset30.group_id = "civilizations";
    pAsset30.icon_path = "ui/Icons/worldrules/icon_lastofus";
    pAsset30.default_state = true;
    WorldLawLibrary.world_law_civ_babies = this.add(pAsset30);
    WorldLawAsset pAsset31 = new WorldLawAsset();
    pAsset31.id = "world_law_civ_migrants";
    pAsset31.group_id = "civilizations";
    pAsset31.icon_path = "ui/Icons/worldrules/icon_migrants";
    pAsset31.default_state = true;
    WorldLawLibrary.world_law_civ_migrants = this.add(pAsset31);
    WorldLawAsset pAsset32 = new WorldLawAsset();
    pAsset32.id = "world_law_forever_tumor_creep";
    pAsset32.group_id = "mobs";
    pAsset32.icon_path = "ui/Icons/iconTumor";
    pAsset32.default_state = false;
    WorldLawLibrary.world_law_forever_tumor_creep = this.add(pAsset32);
    WorldLawAsset pAsset33 = new WorldLawAsset();
    pAsset33.id = "world_law_civ_army";
    pAsset33.group_id = "civilizations";
    pAsset33.icon_path = "ui/Icons/iconArmyList";
    pAsset33.default_state = true;
    WorldLawLibrary.world_law_civ_army = this.add(pAsset33);
    WorldLawAsset pAsset34 = new WorldLawAsset();
    pAsset34.id = "world_law_civ_limit_population_100";
    pAsset34.group_id = "harmony";
    pAsset34.icon_path = "ui/Icons/iconPopulation100";
    pAsset34.default_state = false;
    WorldLawLibrary.world_law_civ_limit_population_100 = this.add(pAsset34);
    WorldLawAsset pAsset35 = new WorldLawAsset();
    pAsset35.id = "world_law_gaias_covenant";
    pAsset35.group_id = "harmony";
    pAsset35.icon_path = "ui/Icons/worldrules/icon_gaias_covenant";
    pAsset35.default_state = false;
    WorldLawLibrary.world_law_gaias_covenant = this.add(pAsset35);
    WorldLawAsset pAsset36 = new WorldLawAsset();
    pAsset36.id = "world_law_cursed_world";
    pAsset36.icon_path = "ui/Icons/worldrules/icon_cursed_world";
    pAsset36.default_state = false;
    pAsset36.can_turn_off = false;
    pAsset36.requires_premium = true;
    pAsset36.on_state_change = new PlayerOptionAction(this.checkCursedWorld);
    pAsset36.on_state_enabled = new PlayerOptionAction(this.turnOnCursedWorld);
    pAsset36.on_world_load = new OnWorldLoadAction(this.checkAlreadyCursed);
    WorldLawLibrary.world_law_cursed_world = this.add(pAsset36);
    WorldLawAsset pAsset37 = new WorldLawAsset();
    pAsset37.id = "world_law_army_no_cross_foreign";
    pAsset37.group_id = "diplomacy";
    pAsset37.icon_path = "ui/Icons/worldrules/icon_army_no_cross";
    pAsset37.default_state = false;
    WorldLawLibrary.world_law_army_no_cross_foreign = this.add(pAsset37);
    WorldLawAsset pAsset38 = new WorldLawAsset();
    pAsset38.id = "world_law_keep_off_my_land";
    pAsset38.group_id = "diplomacy";
    pAsset38.icon_path = "ui/Icons/worldrules/icon_keep_off_my_land";
    pAsset38.default_state = false;
    WorldLawLibrary.world_law_keep_off_my_land = this.add(pAsset38);
  }

  private void addFlora()
  {
    WorldLawAsset pAsset1 = new WorldLawAsset();
    pAsset1.id = "world_law_spread_trees";
    pAsset1.group_id = "trees";
    pAsset1.icon_path = "ui/Icons/worldrules/icon_grow_trees";
    pAsset1.default_state = true;
    WorldLawLibrary.world_law_spread_trees = this.add(pAsset1);
    WorldLawAsset pAsset2 = new WorldLawAsset();
    pAsset2.id = "world_law_spread_fungi";
    pAsset2.group_id = "fungi";
    pAsset2.icon_path = "ui/Icons/worldrules/icon_grow_fungi";
    pAsset2.default_state = true;
    WorldLawLibrary.world_law_spread_fungi = this.add(pAsset2);
    WorldLawAsset pAsset3 = new WorldLawAsset();
    pAsset3.id = "world_law_spread_plants";
    pAsset3.group_id = "plants";
    pAsset3.icon_path = "ui/Icons/worldrules/icon_grow_plants";
    pAsset3.default_state = true;
    WorldLawLibrary.world_law_spread_plants = this.add(pAsset3);
    WorldLawAsset pAsset4 = new WorldLawAsset();
    pAsset4.id = "world_law_spread_fast_trees";
    pAsset4.group_id = "trees";
    pAsset4.icon_path = "ui/Icons/worldrules/icon_grow_trees_fast";
    pAsset4.default_state = false;
    WorldLawLibrary.world_law_spread_fast_trees = this.add(pAsset4);
    WorldLawAsset pAsset5 = new WorldLawAsset();
    pAsset5.id = "world_law_spread_fast_fungi";
    pAsset5.group_id = "fungi";
    pAsset5.icon_path = "ui/Icons/worldrules/icon_grow_fungi_fast";
    pAsset5.default_state = false;
    WorldLawLibrary.world_law_spread_fast_fungi = this.add(pAsset5);
    WorldLawAsset pAsset6 = new WorldLawAsset();
    pAsset6.id = "world_law_spread_fast_plants";
    pAsset6.group_id = "plants";
    pAsset6.icon_path = "ui/Icons/worldrules/icon_grow_plants_fast";
    pAsset6.default_state = false;
    WorldLawLibrary.world_law_spread_fast_plants = this.add(pAsset6);
    WorldLawAsset pAsset7 = new WorldLawAsset();
    pAsset7.id = "world_law_spread_density_high";
    pAsset7.group_id = "nature";
    pAsset7.icon_path = "ui/Icons/worldrules/icon_flora_density_high";
    pAsset7.default_state = false;
    WorldLawLibrary.world_law_spread_density_high = this.add(pAsset7);
    WorldLawAsset pAsset8 = new WorldLawAsset();
    pAsset8.id = "world_law_exploding_mushrooms";
    pAsset8.group_id = "fungi";
    pAsset8.icon_path = "ui/Icons/worldrules/icon_exploding_mushrooms";
    pAsset8.default_state = false;
    pAsset8.on_state_change = (PlayerOptionAction) (pOption =>
    {
      if (!pOption.boolVal)
        return;
      World.world.map_stats.exploding_mushrooms_enabled_at = World.world.getCurWorldTime();
    });
    WorldLawLibrary.world_law_exploding_mushrooms = this.add(pAsset8);
    WorldLawAsset pAsset9 = new WorldLawAsset();
    pAsset9.id = "world_law_entanglewood";
    pAsset9.group_id = "trees";
    pAsset9.icon_path = "ui/Icons/worldrules/icon_entanglewood";
    pAsset9.default_state = true;
    WorldLawLibrary.world_law_entanglewood = this.add(pAsset9);
    WorldLawAsset pAsset10 = new WorldLawAsset();
    pAsset10.id = "world_law_bark_bites_back";
    pAsset10.group_id = "trees";
    pAsset10.icon_path = "ui/Icons/worldrules/icon_bark_bites_back";
    pAsset10.default_state = false;
    WorldLawLibrary.world_law_bark_bites_back = this.add(pAsset10);
    WorldLawAsset pAsset11 = new WorldLawAsset();
    pAsset11.id = "world_law_plants_tickles";
    pAsset11.group_id = "plants";
    pAsset11.icon_path = "ui/Icons/worldrules/icon_plants_tickles";
    pAsset11.default_state = false;
    WorldLawLibrary.world_law_plants_tickles = this.add(pAsset11);
    WorldLawAsset pAsset12 = new WorldLawAsset();
    pAsset12.id = "world_law_root_pranks";
    pAsset12.group_id = "plants";
    pAsset12.icon_path = "ui/Icons/worldrules/icon_root_pranks";
    pAsset12.default_state = false;
    WorldLawLibrary.world_law_root_pranks = this.add(pAsset12);
    WorldLawAsset pAsset13 = new WorldLawAsset();
    pAsset13.id = "world_law_nectar_nap";
    pAsset13.group_id = "plants";
    pAsset13.icon_path = "ui/Icons/worldrules/icon_nectar_nap";
    pAsset13.default_state = false;
    WorldLawLibrary.world_law_nectar_nap = this.add(pAsset13);
  }

  public override void editorDiagnostic()
  {
    base.editorDiagnostic();
    foreach (WorldLawAsset pAsset in this.list)
      this.checkSpriteExists("icon_path", pAsset.icon_path, (Asset) pAsset);
  }

  public override void editorDiagnosticLocales()
  {
    foreach (WorldLawAsset pAsset in this.list)
    {
      this.checkLocale((Asset) pAsset, pAsset.getLocaleID());
      this.checkLocale((Asset) pAsset, pAsset.getDescriptionID());
      this.checkLocale((Asset) pAsset, pAsset.getDescriptionID2());
    }
    base.editorDiagnosticLocales();
  }

  private void checkDiplomacy(PlayerOptionData pOption)
  {
    if (WorldLawLibrary.world_law_diplomacy.isEnabled())
      return;
    World.world.stopAttacksFor(false);
    World.world.wars.stopAllWars();
  }

  private void checkPeacefulMonsters(PlayerOptionData pOption)
  {
    if (!WorldLawLibrary.world_law_peaceful_monsters.isEnabled())
      return;
    World.world.stopAttacksFor(true);
  }

  private void turnOnCursedWorld(PlayerOptionData pOption) => CursedSacrifice.justCursedWorld();

  private void checkCursedWorld(PlayerOptionData pOption) => PowerButton.checkActorSpawnButtons();

  private void checkAlreadyCursed()
  {
    if (!WorldLawLibrary.world_law_cursed_world.isEnabled())
      return;
    CursedSacrifice.loadAlreadyCursedState();
  }

  public static float getIntervalSpreadTrees()
  {
    return WorldLawLibrary.world_law_spread_fast_trees.isEnabled() ? 10f : 50f;
  }

  public static float getIntervalSpreadPlants()
  {
    return WorldLawLibrary.world_law_spread_fast_plants.isEnabled() ? 10f : 40f;
  }

  public static float getIntervalSpreadFungi()
  {
    return WorldLawLibrary.world_law_spread_fast_fungi.isEnabled() ? 10f : 30f;
  }

  public string addToGameplayReport(string pWhatFor)
  {
    string str1 = $"{string.Empty}{pWhatFor}\n";
    foreach (WorldLawAsset worldLawAsset in this.list)
    {
      string translatedName = worldLawAsset.getTranslatedName();
      string translatedDescription = worldLawAsset.getTranslatedDescription();
      string translatedDescription2 = worldLawAsset.getTranslatedDescription2();
      string str2 = "\n" + translatedName + "\n";
      if (!string.IsNullOrEmpty(translatedDescription))
        str2 = $"{str2}1: {translatedDescription}";
      if (!string.IsNullOrEmpty(translatedDescription2))
        str2 = $"{str2}\n2: {translatedDescription2}";
      str1 += str2;
    }
    return str1 + "\n\n";
  }
}

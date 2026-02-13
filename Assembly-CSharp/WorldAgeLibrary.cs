// Decompiled with JetBrains decompiler
// Type: WorldAgeLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class WorldAgeLibrary : AssetLibrary<WorldAgeAsset>
{
  [NonSerialized]
  public List<WorldAgeAsset> list_only_normal;
  [NonSerialized]
  public Dictionary<int, List<WorldAgeAsset>> pool_by_slots = new Dictionary<int, List<WorldAgeAsset>>();
  public static WorldAgeAsset hope;

  public override void init()
  {
    base.init();
    WorldAgeAsset pAsset1 = new WorldAgeAsset();
    pAsset1.id = "age_hope";
    pAsset1.path_icon = "ui/Icons/ages/iconAgeHope";
    pAsset1.rate = 10;
    pAsset1.flag_light_age = true;
    pAsset1.global_unfreeze_world = true;
    pAsset1.cloud_interval = 25f;
    pAsset1.temperature_damage_bonus = 7;
    pAsset1.bonus_loyalty = 15;
    pAsset1.bonus_opinion = 15;
    pAsset1.title_color = Toolbox.makeColor("#86BC4E");
    pAsset1.flag_light_damage = true;
    WorldAgeLibrary.hope = this.add(pAsset1);
    this.t.biomes = AssetLibrary<WorldAgeAsset>.h<string>("biome_grass");
    this.t.clouds = AssetLibrary<WorldAgeAsset>.l<string>("cloud_normal");
    this.t.default_slots = AssetLibrary<WorldAgeAsset>.l<int>(1, 3, 5, 6);
    WorldAgeAsset pAsset2 = new WorldAgeAsset();
    pAsset2.id = "age_sun";
    pAsset2.path_icon = "ui/Icons/ages/iconAgeSun";
    pAsset2.rate = 5;
    pAsset2.overlay_sun = true;
    pAsset2.era_disaster_fire_elemental_spawn_on_fire = true;
    pAsset2.fire_elemental_spawn_chance = 0.01f;
    pAsset2.era_effect_overlay_alpha = 0.1f;
    pAsset2.temperature_damage_bonus = 15;
    pAsset2.global_unfreeze_world = true;
    pAsset2.global_unfreeze_world_mountains = true;
    pAsset2.particles_sun = true;
    pAsset2.title_color = Toolbox.makeColor("#F7A42A");
    pAsset2.bonus_loyalty = 5;
    pAsset2.bonus_opinion = 5;
    pAsset2.bonus_biomes_growth = 3;
    pAsset2.grow_vegetation = false;
    pAsset2.fire_spread_rate_bonus = 2f;
    pAsset2.flag_light_damage = true;
    pAsset2.special_effect_interval = 7f;
    this.add(pAsset2);
    this.t.special_effect_action = new WorldAgeAction(this.droughtAction);
    this.t.biomes = AssetLibrary<WorldAgeAsset>.h<string>("biome_desert", "biome_savanna");
    this.t.clouds = AssetLibrary<WorldAgeAsset>.l<string>("cloud_normal");
    WorldAgeAsset pAsset3 = new WorldAgeAsset();
    pAsset3.id = "age_dark";
    pAsset3.overlay_darkness = true;
    pAsset3.path_icon = "ui/Icons/ages/iconAgeDark";
    pAsset3.era_effect_overlay_alpha = 0.3f;
    pAsset3.overlay_night = true;
    pAsset3.conditions = AssetLibrary<WorldAgeAsset>.a<string>("age_hope", "age_sun", "age_wonders", "age_tears");
    pAsset3.rate = 3;
    pAsset3.global_unfreeze_world = true;
    pAsset3.temperature_damage_bonus = -4;
    pAsset3.flag_crops_grow = false;
    pAsset3.flag_night = true;
    pAsset3.title_color = Toolbox.makeColor("#A4A9B2");
    pAsset3.range_weapons_multiplier = 0.5f;
    pAsset3.bonus_loyalty = -5;
    pAsset3.bonus_opinion = -5;
    pAsset3.bonus_biomes_growth = -2;
    this.add(pAsset3);
    this.t.biomes = AssetLibrary<WorldAgeAsset>.h<string>("biome_corrupted");
    this.t.clouds = AssetLibrary<WorldAgeAsset>.l<string>("cloud_normal");
    this.t.default_slots = AssetLibrary<WorldAgeAsset>.l<int>(2, 7);
    this.t.link_default_slots = true;
    WorldAgeAsset pAsset4 = new WorldAgeAsset();
    pAsset4.id = "age_tears";
    pAsset4.particles_rain = true;
    pAsset4.path_icon = "ui/Icons/ages/iconAgeTears";
    pAsset4.conditions = AssetLibrary<WorldAgeAsset>.a<string>("age_hope", "age_sun", "age_wonders");
    pAsset4.rate = 3;
    pAsset4.global_unfreeze_world = true;
    pAsset4.overlay_rain = true;
    pAsset4.era_effect_overlay_alpha = 0.3f;
    pAsset4.cloud_interval = 20f;
    pAsset4.special_effect_interval = 5f;
    pAsset4.title_color = Toolbox.makeColor("#6C97D8");
    pAsset4.bonus_biomes_growth = 5;
    this.add(pAsset4);
    this.t.special_effect_action = new WorldAgeAction(this.globalRainAction);
    this.t.special_effect_action += new WorldAgeAction(this.trySpawnThunder);
    this.t.special_effect_action += (WorldAgeAction) (() => this.damageHydrophobicUnits());
    this.t.biomes = AssetLibrary<WorldAgeAsset>.h<string>("biome_swamp", "biome_mushroom", "biome_jungle");
    this.t.clouds = Toolbox.splitStringIntoList("cloud_rain#5", "cloud_lightning#1", "cloud_normal#2");
    this.t.default_slots = AssetLibrary<WorldAgeAsset>.l<int>(2, 7);
    this.t.link_default_slots = true;
    WorldAgeAsset pAsset5 = new WorldAgeAsset();
    pAsset5.id = "age_moon";
    pAsset5.path_icon = "ui/Icons/ages/iconAgeMoon";
    pAsset5.overlay_darkness = true;
    pAsset5.flag_moon = true;
    pAsset5.overlay_moon = true;
    pAsset5.global_unfreeze_world = true;
    pAsset5.temperature_damage_bonus = -3;
    pAsset5.conditions = AssetLibrary<WorldAgeAsset>.a<string>("age_hope", "age_sun");
    pAsset5.rate = 3;
    pAsset5.light_color = Toolbox.makeColor("#8DFFF3");
    pAsset5.title_color = Toolbox.makeColor("#B5FAFF");
    this.add(pAsset5);
    this.t.default_slots = AssetLibrary<WorldAgeAsset>.l<int>(2, 7);
    this.t.biomes = AssetLibrary<WorldAgeAsset>.h<string>("biome_crystal");
    this.t.clouds = Toolbox.splitStringIntoList("cloud_rain#5", "cloud_normal#5");
    WorldAgeAsset pAsset6 = new WorldAgeAsset();
    pAsset6.id = "age_chaos";
    pAsset6.overlay_chaos = true;
    pAsset6.flag_chaos = true;
    pAsset6.era_disaster_rage_brings_demons = true;
    pAsset6.path_icon = "ui/Icons/ages/iconAgeChaos";
    pAsset6.conditions = AssetLibrary<WorldAgeAsset>.a<string>("age_hope", "age_sun", "age_wonders");
    pAsset6.era_effect_overlay_alpha = 0.35f;
    pAsset6.rate = 2;
    pAsset6.global_unfreeze_world = true;
    pAsset6.global_unfreeze_world_mountains = true;
    pAsset6.title_color = Toolbox.makeColor("#E6503A");
    pAsset6.bonus_loyalty = -55;
    pAsset6.bonus_opinion = -35;
    pAsset6.flag_light_damage = true;
    this.add(pAsset6);
    this.t.biomes = AssetLibrary<WorldAgeAsset>.h<string>("biome_infernal");
    this.t.clouds = AssetLibrary<WorldAgeAsset>.l<string>("cloud_rage");
    WorldAgeAsset pAsset7 = new WorldAgeAsset();
    pAsset7.id = "age_wonders";
    pAsset7.path_icon = "ui/Icons/ages/iconAgeWonders";
    pAsset7.rate = 2;
    pAsset7.flag_light_age = true;
    pAsset7.particles_magic = true;
    pAsset7.overlay_magic = true;
    pAsset7.era_effect_overlay_alpha = 0.15f;
    pAsset7.global_unfreeze_world = true;
    pAsset7.global_unfreeze_world_mountains = true;
    pAsset7.title_color = Toolbox.makeColor("#D6559C");
    this.add(pAsset7);
    this.t.biomes = AssetLibrary<WorldAgeAsset>.h<string>("biome_enchanted", "biome_candy");
    this.t.clouds = Toolbox.splitStringIntoList("cloud_magic#5", "cloud_normal#1");
    this.t.default_slots = AssetLibrary<WorldAgeAsset>.l<int>(2, 7);
    this.t.link_default_slots = true;
    WorldAgeAsset pAsset8 = new WorldAgeAsset();
    pAsset8.id = "age_ice";
    pAsset8.particles_snow = true;
    pAsset8.path_icon = "ui/Icons/ages/iconAgeIce";
    pAsset8.conditions = AssetLibrary<WorldAgeAsset>.a<string>("age_hope", "age_sun", "age_wonders");
    pAsset8.rate = 2;
    pAsset8.global_freeze_world = true;
    pAsset8.overlay_winter = true;
    pAsset8.flag_winter = true;
    pAsset8.era_effect_overlay_alpha = 0.2f;
    pAsset8.temperature_damage_bonus = 5;
    pAsset8.flag_crops_grow = false;
    pAsset8.cloud_interval = 20f;
    pAsset8.title_color = Toolbox.makeColor("#C1FAFF");
    pAsset8.bonus_biomes_growth = -20;
    pAsset8.years_min = 30;
    pAsset8.years_max = 40;
    this.add(pAsset8);
    this.t.special_effect_action = (WorldAgeAction) (() => this.damageHydrophobicUnits(true));
    this.t.special_effect_action += new WorldAgeAction(this.frostingAction);
    this.t.biomes = AssetLibrary<WorldAgeAsset>.h<string>("biome_permafrost");
    this.t.clouds = AssetLibrary<WorldAgeAsset>.l<string>("cloud_snow");
    WorldAgeAsset pAsset9 = new WorldAgeAsset();
    pAsset9.id = "age_ash";
    pAsset9.path_icon = "ui/Icons/ages/iconAgeAsh";
    pAsset9.conditions = AssetLibrary<WorldAgeAsset>.a<string>("age_hope", "age_sun", "age_wonders");
    pAsset9.rate = 2;
    pAsset9.overlay_ash = true;
    pAsset9.particles_ash = true;
    pAsset9.era_effect_overlay_alpha = 0.4f;
    pAsset9.global_unfreeze_world = true;
    pAsset9.cloud_interval = 15f;
    pAsset9.title_color = Toolbox.makeColor("#DDC49D");
    pAsset9.bonus_loyalty = -25;
    pAsset9.bonus_opinion = -10;
    pAsset9.bonus_biomes_growth = -4;
    this.add(pAsset9);
    this.t.clouds = AssetLibrary<WorldAgeAsset>.l<string>("cloud_ash");
    WorldAgeAsset pAsset10 = new WorldAgeAsset();
    pAsset10.id = "age_despair";
    pAsset10.overlay_darkness = true;
    pAsset10.particles_snow = true;
    pAsset10.path_icon = "ui/Icons/ages/iconAgeDespair";
    pAsset10.conditions = AssetLibrary<WorldAgeAsset>.a<string>("age_dark", "age_ice");
    pAsset10.force_next = "age_hope";
    pAsset10.rate = 4;
    pAsset10.overlay_winter = true;
    pAsset10.flag_winter = true;
    pAsset10.overlay_night = true;
    pAsset10.era_effect_overlay_alpha = 0.25f;
    pAsset10.global_freeze_world = true;
    pAsset10.temperature_damage_bonus = 3;
    pAsset10.flag_crops_grow = false;
    pAsset10.flag_night = true;
    pAsset10.cloud_interval = 16f;
    pAsset10.title_color = Toolbox.makeColor("#728599");
    pAsset10.era_disaster_snow_turns_babies_into_ice_ones = true;
    pAsset10.range_weapons_multiplier = 0.5f;
    pAsset10.bonus_loyalty = -10;
    pAsset10.bonus_opinion = -10;
    pAsset10.bonus_biomes_growth = -2;
    pAsset10.years_min = 30;
    pAsset10.years_max = 40;
    this.add(pAsset10);
    this.t.special_effect_action += (WorldAgeAction) (() => this.damageHydrophobicUnits(true));
    this.t.special_effect_action += new WorldAgeAction(this.frostingAction);
    this.t.biomes = AssetLibrary<WorldAgeAsset>.h<string>("biome_permafrost", "biome_corrupted");
    this.t.clouds = AssetLibrary<WorldAgeAsset>.l<string>("cloud_snow");
    WorldAgeAsset pAsset11 = new WorldAgeAsset();
    pAsset11.id = "age_unknown";
    pAsset11.path_icon = "ui/Icons/ages/iconAgeUnknown";
    pAsset11.title_color = Toolbox.makeColor("#AAAAAA");
    this.add(pAsset11);
    this.t.default_slots = AssetLibrary<WorldAgeAsset>.l<int>(4, 8);
  }

  public override void post_init()
  {
    base.post_init();
    foreach (WorldAgeAsset worldAgeAsset in this.list)
      worldAgeAsset.path_background = $"ui/AgeWheel/backgrounds/{worldAgeAsset.id}_background";
  }

  public override void linkAssets()
  {
    base.linkAssets();
    foreach (WorldAgeAsset worldAgeAsset in this.list)
    {
      foreach (int defaultSlot in worldAgeAsset.default_slots)
      {
        if (!this.pool_by_slots.ContainsKey(defaultSlot))
          this.pool_by_slots.Add(defaultSlot, new List<WorldAgeAsset>());
        this.pool_by_slots[defaultSlot].Add(worldAgeAsset);
      }
    }
    this.list_only_normal = this.list.FindAll((Predicate<WorldAgeAsset>) (pAsset => pAsset.id != "age_unknown"));
  }

  public void damageHydrophobicUnits(bool pFrost = false)
  {
    foreach (Subspecies subspecies in World.world.subspecies.list)
    {
      if (subspecies.is_damaged_by_water)
      {
        foreach (Actor unit in subspecies.units)
        {
          if (unit.isAlive() && !unit.isInsideSomething() && (!pFrost || !unit.has_tag_immunity_cold) && Randy.randomChance(0.5f))
            unit.getHit((float) unit.getWaterDamage(), pAttackType: AttackType.Water);
        }
      }
    }
  }

  public void droughtAction()
  {
    this.extremeEnvironmentAction(new EnvironmentAction(this.droughtCheck));
  }

  private bool droughtCheck(BuildingAsset pAsset)
  {
    return pAsset.affected_by_drought && !(pAsset.type != "type_tree");
  }

  public void frostingAction()
  {
    this.extremeEnvironmentAction(new EnvironmentAction(this.frostCheck));
  }

  private bool frostCheck(BuildingAsset pAsset)
  {
    return pAsset.affected_by_cold_temperature && (!(pAsset.type != "type_tree") || !(pAsset.type != "type_vegetation"));
  }

  private void extremeEnvironmentAction(EnvironmentAction pCheckAsset)
  {
    foreach (Building building in World.world.kingdoms_wild.get("nature").buildings.LoopRandom<Building>(5))
    {
      if (building.isAlive() && !building.isRuin() && pCheckAsset(building.asset))
        building.startMakingRuins();
    }
  }

  public void globalRainAction()
  {
    if (WorldBehaviourActionFire.hasFires())
      return;
    List<Actor> simpleList1 = World.world.units.getSimpleList();
    for (int index = 0; index < simpleList1.Count; ++index)
    {
      Actor actor = simpleList1[index];
      if (actor.isAlive() && actor.hasStatus("burning") && Randy.randomChance(0.9f))
        actor.finishStatusEffect("burning");
    }
    List<Building> simpleList2 = World.world.buildings.getSimpleList();
    for (int index = 0; index < simpleList2.Count; ++index)
    {
      Building building = simpleList2[index];
      if (building.isAlive() && Randy.randomChance(0.9f))
        building.stopFire();
    }
  }

  public void trySpawnThunder()
  {
    if (!MapBox.isRenderGameplay() || !Randy.randomChance(0.1f))
      return;
    double timeElapsedSince = (double) World.world.getWorldTimeElapsedSince(StackEffects.last_thunder_timestamp);
    float num1 = 100f * Config.time_scale_asset.multiplier;
    if ((double) num1 > 600.0)
      num1 = 600f;
    double num2 = (double) num1;
    if (timeElapsedSince <= num2 && StackEffects.last_thunder_timestamp != 0.0)
      return;
    EffectsLibrary.spawn("fx_thunder_flash");
    StackEffects.last_thunder_timestamp = World.world.getCurWorldTime();
  }

  public override void editorDiagnosticLocales()
  {
    base.editorDiagnosticLocales();
    foreach (WorldAgeAsset pAsset in this.list)
    {
      this.checkLocale((Asset) pAsset, pAsset.getLocaleID());
      this.checkLocale((Asset) pAsset, pAsset.getDescriptionID());
    }
  }
}

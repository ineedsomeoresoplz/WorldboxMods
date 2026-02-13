// Decompiled with JetBrains decompiler
// Type: ResourceLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class ResourceLibrary : AssetLibrary<ResourceAsset>
{
  private const string TEMPLATE_FOOD = "$TEMPLATE_FOOD$";
  private const string TEMPLATE_STRATEGIC_MINERAL = "$TEMPLATE_STRATEGIC_MINERAL$";
  public static ResourceAsset herbs;
  public static ResourceAsset mushrooms;
  public static ResourceAsset desert_berries;
  public static ResourceAsset berries;
  public static ResourceAsset peppers;
  public static ResourceAsset bananas;
  public static ResourceAsset crystal_salt;
  public static ResourceAsset coconut;
  public static ResourceAsset evil_beets;
  public static ResourceAsset lemons;
  public static ResourceAsset meat;
  public static ResourceAsset sushi;
  public static ResourceAsset jam;
  public static ResourceAsset burger;
  public static ResourceAsset cider;
  public static ResourceAsset ale;
  public static ResourceAsset honey;
  public static ResourceAsset tea;
  public static ResourceAsset pie;
  public static ResourceAsset wheat;
  public static ResourceAsset bread;
  public static ResourceAsset fish;
  public static ResourceAsset candy;
  public static ResourceAsset worms;
  public static ResourceAsset pine_cones;
  public static ResourceAsset snow_cucumbers;
  public static ResourceAsset celestial_avocado;
  public static ResourceAsset wood;
  public static ResourceAsset stone;
  public static ResourceAsset common_metals;
  public static ResourceAsset gold;
  public static ResourceAsset fertilizer;
  [NonSerialized]
  public List<ResourceAsset> strategic_resource_assets = new List<ResourceAsset>();
  [NonSerialized]
  public Dictionary<string, List<string>> diet_food_pools = new Dictionary<string, List<string>>();

  public override void init()
  {
    base.init();
    this.initTemplates();
    this.initOther();
    this.initStrategic();
    this.initFoodIngredients();
    this.initFood();
    this.initFoodRecipes();
  }

  private void initTemplates()
  {
    ResourceAsset pAsset1 = new ResourceAsset();
    pAsset1.id = "$TEMPLATE_FOOD$";
    pAsset1.type = ResType.Food;
    pAsset1.path_gameplay_sprite = "bag_food";
    pAsset1.tooltip = "city_resource_food";
    pAsset1.food = true;
    this.add(pAsset1);
    ResourceAsset pAsset2 = new ResourceAsset();
    pAsset2.id = "$TEMPLATE_STRATEGIC_MINERAL$";
    pAsset2.type = ResType.Strategic;
    pAsset2.mineral = true;
    this.add(pAsset2);
    this.t.diet = AssetLibrary<ResourceAsset>.a<string>("diet_minerals");
  }

  private void initFoodIngredients()
  {
    ResourceAsset pAsset = new ResourceAsset();
    pAsset.id = "wheat";
    pAsset.type = ResType.Ingredient_Food;
    pAsset.path_gameplay_sprite = "wheat";
    pAsset.path_icon = "iconResWheat";
    pAsset.supply_bound_give = 100;
    pAsset.supply_bound_take = 20;
    pAsset.restore_nutrition = 50;
    pAsset.restore_health = 0.05f;
    pAsset.restore_mana = 5;
    pAsset.restore_stamina = 5;
    pAsset.favorite_food_chance = 0.1f;
    pAsset.tastiness = 0.1f;
    pAsset.restore_happiness = 5;
    ResourceLibrary.wheat = this.add(pAsset);
    this.t.food = true;
    this.t.diet = AssetLibrary<ResourceAsset>.a<string>("diet_crops");
  }

  private void initFoodRecipes()
  {
    ResourceLibrary.bread = this.clone("bread", "$TEMPLATE_FOOD$");
    this.t.path_icon = "iconResBread";
    this.t.path_gameplay_sprite = "bread";
    this.t.ingredients_amount = 1;
    this.t.restore_nutrition = 100;
    this.t.restore_health = 0.1f;
    this.t.restore_mana = 10;
    this.t.restore_stamina = 10;
    this.t.restore_happiness = 10;
    this.t.give_experience = 10;
    this.t.ingredients = AssetLibrary<ResourceAsset>.a<string>("wheat");
    this.t.trade_bound = 50;
    this.t.produce_min = 50;
    this.t.maximum = 999;
    this.t.trade_give = 5;
    this.t.tastiness = 0.5f;
    this.t.diet = AssetLibrary<ResourceAsset>.a<string>("diet_crops");
    ResourceLibrary.sushi = this.clone("sushi", "$TEMPLATE_FOOD$");
    this.t.path_icon = "iconResSushi";
    this.t.restore_nutrition = 70;
    this.t.restore_health = 0.2f;
    this.t.restore_mana = 20;
    this.t.restore_stamina = 20;
    this.t.restore_happiness = 20;
    this.t.give_experience = 5;
    this.t.ingredients_amount = 1;
    this.t.ingredients = AssetLibrary<ResourceAsset>.a<string>("fish", "herbs");
    this.t.trade_bound = 50;
    this.t.trade_give = 5;
    this.t.give_trait_id = Toolbox.splitStringIntoArray("fast#5");
    this.t.give_chance = 0.1f;
    this.t.tastiness = 0.6f;
    this.t.diet = AssetLibrary<ResourceAsset>.a<string>("diet_fish", "diet_meat");
    ResourceLibrary.jam = this.clone("jam", "$TEMPLATE_FOOD$");
    this.t.path_icon = "iconResJam";
    this.t.restore_nutrition = 100;
    this.t.restore_health = 0.4f;
    this.t.restore_mana = 40;
    this.t.restore_stamina = 40;
    this.t.restore_happiness = 25;
    this.t.ingredients_amount = 2;
    this.t.give_experience = 15;
    this.t.ingredients = AssetLibrary<ResourceAsset>.a<string>("berries", "herbs");
    this.t.trade_bound = 50;
    this.t.trade_give = 5;
    this.t.give_trait_id = Toolbox.splitStringIntoArray("regeneration#3");
    this.t.give_chance = 0.1f;
    this.t.tastiness = 0.9f;
    this.t.diet = AssetLibrary<ResourceAsset>.a<string>("diet_fruits", "diet_vegetation");
    ResourceLibrary.cider = this.clone("cider", "$TEMPLATE_FOOD$");
    this.t.path_icon = "iconResCider";
    this.t.restore_nutrition = 55;
    this.t.restore_health = 0.25f;
    this.t.restore_mana = 25;
    this.t.restore_stamina = 25;
    this.t.restore_happiness = 20;
    this.t.ingredients_amount = 3;
    this.t.give_experience = 10;
    this.t.ingredients = AssetLibrary<ResourceAsset>.a<string>("berries");
    this.t.trade_bound = 50;
    this.t.trade_give = 5;
    this.t.tastiness = 0.7f;
    this.t.diet = AssetLibrary<ResourceAsset>.a<string>("diet_fruits");
    ResourceLibrary.ale = this.clone("ale", "$TEMPLATE_FOOD$");
    this.t.path_icon = "iconResAle";
    this.t.restore_nutrition = 45;
    this.t.restore_health = 0.2f;
    this.t.restore_mana = 20;
    this.t.restore_stamina = 20;
    this.t.restore_happiness = 20;
    this.t.ingredients_amount = 3;
    this.t.give_experience = 10;
    this.t.ingredients = AssetLibrary<ResourceAsset>.a<string>("wheat");
    this.t.trade_bound = 50;
    this.t.trade_give = 5;
    this.t.give_status_id = AssetLibrary<ResourceAsset>.a<string>("slowness");
    this.t.give_trait_id = Toolbox.splitStringIntoArray("poison_immune#1");
    this.t.give_chance = 0.4f;
    this.t.tastiness = 0.6f;
    this.t.diet = AssetLibrary<ResourceAsset>.a<string>("diet_crops");
    ResourceLibrary.burger = this.clone("burger", "$TEMPLATE_FOOD$");
    this.t.path_icon = "iconResBurger";
    this.t.restore_nutrition = 100;
    this.t.restore_health = 0.4f;
    this.t.restore_mana = 25;
    this.t.restore_stamina = 35;
    this.t.restore_happiness = 25;
    this.t.ingredients_amount = 1;
    this.t.give_experience = 9;
    this.t.ingredients = AssetLibrary<ResourceAsset>.a<string>("meat", "bread", "herbs");
    this.t.trade_bound = 50;
    this.t.trade_give = 5;
    this.t.give_status_id = AssetLibrary<ResourceAsset>.a<string>("slowness");
    this.t.give_trait_id = Toolbox.splitStringIntoArray("fat#1");
    this.t.give_chance = 0.05f;
    this.t.tastiness = 0.85f;
    this.t.diet = AssetLibrary<ResourceAsset>.a<string>("diet_meat", "diet_crops", "diet_vegetation");
    ResourceLibrary.pie = this.clone("pie", "$TEMPLATE_FOOD$");
    this.t.path_icon = "iconResPie";
    this.t.restore_nutrition = 100;
    this.t.restore_health = 0.4f;
    this.t.restore_mana = 35;
    this.t.restore_stamina = 25;
    this.t.restore_happiness = 25;
    this.t.ingredients_amount = 1;
    this.t.give_experience = 10;
    this.t.ingredients = AssetLibrary<ResourceAsset>.a<string>("meat", "wheat");
    this.t.trade_bound = 50;
    this.t.trade_give = 5;
    this.t.give_status_id = AssetLibrary<ResourceAsset>.a<string>("slowness");
    this.t.give_trait_id = Toolbox.splitStringIntoArray("fat#1");
    this.t.give_chance = 0.05f;
    this.t.tastiness = 0.9f;
    this.t.diet = AssetLibrary<ResourceAsset>.a<string>("diet_meat", "diet_crops");
    ResourceLibrary.tea = this.clone("tea", "$TEMPLATE_FOOD$");
    this.t.restore_nutrition = 20;
    this.t.restore_health = 0.25f;
    this.t.restore_mana = 40;
    this.t.restore_stamina = 10;
    this.t.restore_happiness = 10;
    this.t.give_experience = 10;
    this.t.path_icon = "iconResTea";
    this.t.path_gameplay_sprite = "box_tea";
    this.t.trade_bound = 50;
    this.t.trade_give = 5;
    this.t.ingredients = AssetLibrary<ResourceAsset>.a<string>("herbs");
    this.t.give_status_id = AssetLibrary<ResourceAsset>.a<string>("caffeinated");
    this.t.give_chance = 0.1f;
    this.t.tastiness = 0.3f;
    this.t.diet = AssetLibrary<ResourceAsset>.a<string>("diet_vegetation");
  }

  private void initFood()
  {
    ResourceLibrary.berries = this.clone("berries", "$TEMPLATE_FOOD$");
    this.t.path_gameplay_sprite = "berries";
    this.t.path_icon = "iconResBerries";
    this.t.restore_nutrition = 30;
    this.t.restore_health = 0.35f;
    this.t.restore_mana = 15;
    this.t.restore_stamina = 10;
    this.t.restore_happiness = 12;
    this.t.give_experience = 5;
    this.t.trade_bound = 50;
    this.t.trade_give = 5;
    this.t.storage_max = 100;
    this.t.tastiness = 0.55f;
    this.t.diet = AssetLibrary<ResourceAsset>.a<string>("diet_fruits");
    ResourceLibrary.bananas = this.clone("bananas", "$TEMPLATE_FOOD$");
    this.t.path_gameplay_sprite = "bananas";
    this.t.path_icon = "iconResBanana";
    this.t.restore_nutrition = 25;
    this.t.restore_health = 0.4f;
    this.t.restore_mana = 15;
    this.t.restore_stamina = 10;
    this.t.restore_happiness = 10;
    this.t.give_experience = 5;
    this.t.trade_bound = 50;
    this.t.trade_give = 5;
    this.t.storage_max = 100;
    this.t.give_trait_id = Toolbox.splitStringIntoArray("unlucky");
    this.t.give_chance = 0.05f;
    this.t.tastiness = 0.55f;
    this.t.diet = AssetLibrary<ResourceAsset>.a<string>("diet_fruits");
    ResourceLibrary.coconut = this.clone("coconut", "$TEMPLATE_FOOD$");
    this.t.path_icon = "iconResCoconut";
    this.t.path_gameplay_sprite = "coconut";
    this.t.restore_nutrition = 30;
    this.t.restore_health = 0.45f;
    this.t.restore_mana = 15;
    this.t.restore_stamina = 10;
    this.t.restore_happiness = 15;
    this.t.give_experience = 10;
    this.t.trade_bound = 50;
    this.t.trade_give = 5;
    this.t.storage_max = 100;
    this.t.give_status_id = Toolbox.splitStringIntoArray("shield#1");
    this.t.give_chance = 0.1f;
    this.t.tastiness = 0.55f;
    this.t.diet = AssetLibrary<ResourceAsset>.a<string>("diet_fruits");
    ResourceLibrary.crystal_salt = this.clone("crystal_salt", "$TEMPLATE_FOOD$");
    this.t.drop_per_mass = 150;
    this.t.path_icon = "iconResCrystalSalt";
    this.t.path_gameplay_sprite = "crystal_salt";
    this.t.restore_nutrition = 20;
    this.t.restore_health = 0.2f;
    this.t.restore_mana = 20;
    this.t.restore_stamina = 10;
    this.t.restore_happiness = 10;
    this.t.give_experience = 5;
    this.t.trade_bound = 50;
    this.t.trade_give = 5;
    this.t.storage_max = 100;
    this.t.give_status_id = Toolbox.splitStringIntoArray("caffeinated#3", "slowness#6");
    this.t.give_trait_id = Toolbox.splitStringIntoArray("madness#10", "strong_minded#1");
    this.t.give_chance = 0.1f;
    this.t.tastiness = 0.2f;
    this.t.diet = AssetLibrary<ResourceAsset>.a<string>("diet_minerals");
    ResourceLibrary.desert_berries = this.clone("desert_berries", "$TEMPLATE_FOOD$");
    this.t.path_icon = "iconResDesertBerry";
    this.t.path_gameplay_sprite = "desert_berries";
    this.t.restore_nutrition = 25;
    this.t.restore_health = 0.35f;
    this.t.restore_mana = 15;
    this.t.restore_stamina = 10;
    this.t.restore_happiness = 12;
    this.t.give_experience = 5;
    this.t.trade_bound = 50;
    this.t.trade_give = 5;
    this.t.storage_max = 100;
    this.t.give_status_id = Toolbox.splitStringIntoArray("poisoned#2", "slowness#2");
    this.t.give_trait_id = Toolbox.splitStringIntoArray("poison_immune#1");
    this.t.give_chance = 0.1f;
    this.t.tastiness = 0.55f;
    this.t.diet = AssetLibrary<ResourceAsset>.a<string>("diet_fruits");
    ResourceLibrary.evil_beets = this.clone("evil_beets", "$TEMPLATE_FOOD$");
    this.t.path_icon = "iconResEvilBeets";
    this.t.path_gameplay_sprite = "evil_beets";
    this.t.restore_nutrition = 25;
    this.t.restore_health = 0.4f;
    this.t.restore_mana = 15;
    this.t.restore_stamina = 25;
    this.t.restore_happiness = 15;
    this.t.give_experience = 5;
    this.t.trade_bound = 50;
    this.t.trade_give = 5;
    this.t.storage_max = 100;
    this.t.give_trait_id = Toolbox.splitStringIntoArray("pyromaniac#1", "evil#3");
    this.t.give_chance = 0.1f;
    this.t.tastiness = 0.1f;
    this.t.diet = AssetLibrary<ResourceAsset>.a<string>("diet_vegetation");
    ResourceLibrary.mushrooms = this.clone("mushrooms", "$TEMPLATE_FOOD$");
    this.t.drop_per_mass = 40;
    this.t.path_gameplay_sprite = "mushrooms";
    this.t.path_icon = "iconResMushrooms";
    this.t.restore_nutrition = 20;
    this.t.restore_health = 0.4f;
    this.t.restore_mana = 15;
    this.t.restore_stamina = 15;
    this.t.restore_happiness = 15;
    this.t.give_experience = 5;
    this.t.trade_bound = 50;
    this.t.trade_give = 5;
    this.t.storage_max = 100;
    this.t.give_status_id = Toolbox.splitStringIntoArray("powerup#10", "slowness#3");
    this.t.give_trait_id = Toolbox.splitStringIntoArray("madness#1", "strong_minded#1", "paranoid#5", "content#10");
    this.t.give_chance = 0.1f;
    this.t.tastiness = 0.2f;
    this.t.diet = AssetLibrary<ResourceAsset>.a<string>("diet_vegetation");
    ResourceLibrary.peppers = this.clone("peppers", "$TEMPLATE_FOOD$");
    this.t.drop_per_mass = 40;
    this.t.path_icon = "iconResPeppers";
    this.t.path_gameplay_sprite = "peppers";
    this.t.restore_nutrition = 30;
    this.t.restore_health = 0.4f;
    this.t.restore_mana = 25;
    this.t.restore_stamina = 15;
    this.t.restore_happiness = 20;
    this.t.give_experience = 5;
    this.t.trade_bound = 50;
    this.t.trade_give = 5;
    this.t.storage_max = 100;
    this.t.give_status_id = Toolbox.splitStringIntoArray("burning");
    this.t.give_trait_id = Toolbox.splitStringIntoArray("fire_proof#5", "fire_blood#5");
    this.t.give_chance = 0.1f;
    this.t.tastiness = 0.3f;
    this.t.diet = AssetLibrary<ResourceAsset>.a<string>("diet_vegetation");
    ResourceLibrary.herbs = this.clone("herbs", "$TEMPLATE_FOOD$");
    this.t.path_icon = "iconResHerbs";
    this.t.path_gameplay_sprite = "herbs";
    this.t.restore_nutrition = 25;
    this.t.restore_health = 0.5f;
    this.t.restore_mana = 25;
    this.t.restore_stamina = 10;
    this.t.restore_happiness = 5;
    this.t.give_experience = 5;
    this.t.trade_bound = 30;
    this.t.trade_give = 10;
    this.t.storage_max = 100;
    this.t.tastiness = 0.1f;
    this.t.diet = AssetLibrary<ResourceAsset>.a<string>("diet_vegetation");
    ResourceLibrary.fish = this.clone("fish", "$TEMPLATE_FOOD$");
    this.t.path_gameplay_sprite = "fish";
    this.t.restore_nutrition = 50;
    this.t.restore_health = 0.3f;
    this.t.restore_mana = 25;
    this.t.restore_stamina = 10;
    this.t.restore_happiness = 15;
    this.t.path_icon = "iconResFish";
    this.t.give_experience = 5;
    this.t.trade_bound = 50;
    this.t.trade_give = 5;
    this.t.give_trait_id = Toolbox.splitStringIntoArray("tough#5", "strong#5");
    this.t.give_chance = 0.1f;
    this.t.tastiness = 0.25f;
    this.t.diet = AssetLibrary<ResourceAsset>.a<string>("diet_fish", "diet_meat");
    ResourceLibrary.candy = this.clone("candy", "$TEMPLATE_FOOD$");
    this.t.drop_per_mass = 50;
    this.t.path_icon = "iconResCandy";
    this.t.path_gameplay_sprite = "candy";
    this.t.restore_nutrition = 40;
    this.t.restore_health = 0.4f;
    this.t.restore_mana = 25;
    this.t.restore_stamina = 10;
    this.t.restore_happiness = 40;
    this.t.give_experience = 5;
    this.t.trade_bound = 50;
    this.t.trade_give = 5;
    this.t.give_trait_id = Toolbox.splitStringIntoArray("fat#10", "tiny#5", "giant#5", "bloodlust#1");
    this.t.give_chance = 0.1f;
    this.t.tastiness = 1f;
    this.t.diet = AssetLibrary<ResourceAsset>.a<string>("diet_fruits");
    ResourceLibrary.worms = this.clone("worms", "$TEMPLATE_FOOD$");
    this.t.path_icon = "iconResWorms";
    this.t.path_gameplay_sprite = "worms";
    this.t.restore_nutrition = 10;
    this.t.restore_health = 0.2f;
    this.t.restore_mana = 10;
    this.t.restore_stamina = 10;
    this.t.restore_happiness = -5;
    this.t.give_experience = 5;
    this.t.trade_bound = 50;
    this.t.trade_give = 5;
    this.t.tastiness = 0.1f;
    this.t.diet = AssetLibrary<ResourceAsset>.a<string>("diet_meat");
    ResourceLibrary.snow_cucumbers = this.clone("snow_cucumbers", "$TEMPLATE_FOOD$");
    this.t.path_icon = "iconResSnowCucumbers";
    this.t.path_gameplay_sprite = "snow_cucumbers";
    this.t.restore_nutrition = 30;
    this.t.restore_health = 0.35f;
    this.t.restore_mana = 25;
    this.t.restore_stamina = 10;
    this.t.restore_happiness = 10;
    this.t.give_experience = 5;
    this.t.trade_bound = 50;
    this.t.trade_give = 5;
    this.t.give_status_id = Toolbox.splitStringIntoArray("frozen");
    this.t.give_trait_id = Toolbox.splitStringIntoArray("freeze_proof#1");
    this.t.give_chance = 0.1f;
    this.t.tastiness = 0.2f;
    this.t.diet = AssetLibrary<ResourceAsset>.a<string>("diet_vegetation");
    ResourceLibrary.celestial_avocado = this.clone("celestial_avocado", "$TEMPLATE_FOOD$");
    this.t.path_icon = "iconResCelestialAvocado";
    this.t.path_gameplay_sprite = "celestial_avocado";
    this.t.restore_nutrition = 100;
    this.t.restore_health = 0.55f;
    this.t.restore_mana = 50;
    this.t.restore_stamina = 50;
    this.t.restore_happiness = 50;
    this.t.give_experience = 10;
    this.t.trade_bound = 50;
    this.t.trade_give = 5;
    this.t.give_trait_id = Toolbox.splitStringIntoArray("sunblessed#1");
    this.t.give_chance = 0.1f;
    this.t.tastiness = 1.1f;
    this.t.diet = AssetLibrary<ResourceAsset>.a<string>("diet_fruits");
    ResourceLibrary.pine_cones = this.clone("pine_cones", "$TEMPLATE_FOOD$");
    this.t.path_icon = "iconResPineCones";
    this.t.path_gameplay_sprite = "pine_cones";
    this.t.restore_nutrition = 15;
    this.t.restore_health = 0.15f;
    this.t.restore_mana = 10;
    this.t.restore_stamina = 10;
    this.t.restore_happiness = 10;
    this.t.give_experience = 5;
    this.t.trade_bound = 50;
    this.t.trade_give = 5;
    this.t.give_status_id = Toolbox.splitStringIntoArray("frozen");
    this.t.give_trait_id = Toolbox.splitStringIntoArray("freeze_proof#10", "tough#1", "strong#1", "regeneration#1");
    this.t.give_chance = 0.1f;
    this.t.tastiness = 0.1f;
    this.t.diet = AssetLibrary<ResourceAsset>.a<string>("diet_crops");
    ResourceLibrary.lemons = this.clone("lemons", "$TEMPLATE_FOOD$");
    this.t.path_icon = "iconResLemons";
    this.t.path_gameplay_sprite = "lemons";
    this.t.restore_nutrition = 20;
    this.t.restore_health = 0.3f;
    this.t.restore_mana = 10;
    this.t.restore_stamina = 10;
    this.t.restore_happiness = 10;
    this.t.give_experience = 5;
    this.t.trade_bound = 50;
    this.t.trade_give = 5;
    this.t.give_trait_id = Toolbox.splitStringIntoArray("eagle_eyed#5", "regeneration#3");
    this.t.give_chance = 0.1f;
    this.t.tastiness = 0.2f;
    this.t.diet = AssetLibrary<ResourceAsset>.a<string>("diet_fruits");
    ResourceLibrary.meat = this.clone("meat", "$TEMPLATE_FOOD$");
    this.t.drop_per_mass = 50;
    this.t.restore_nutrition = 60;
    this.t.restore_health = 0.5f;
    this.t.restore_mana = 10;
    this.t.restore_stamina = 25;
    this.t.restore_happiness = 20;
    this.t.path_icon = "iconResMeat";
    this.t.path_gameplay_sprite = "meat";
    this.t.give_experience = 5;
    this.t.trade_bound = 50;
    this.t.trade_give = 5;
    this.t.give_trait_id = Toolbox.splitStringIntoArray("tough#5", "strong#5");
    this.t.give_chance = 0.1f;
    this.t.tastiness = 0.5f;
    this.t.diet = AssetLibrary<ResourceAsset>.a<string>("diet_meat");
    ResourceLibrary.honey = this.clone("honey", "$TEMPLATE_FOOD$");
    this.t.path_icon = "iconResHoney";
    this.t.path_gameplay_sprite = "honey";
    this.t.restore_nutrition = 15;
    this.t.restore_health = 0.45f;
    this.t.restore_mana = 10;
    this.t.restore_stamina = 25;
    this.t.restore_happiness = 35;
    this.t.give_experience = 10;
    this.t.trade_bound = 50;
    this.t.trade_give = 10;
    this.t.storage_max = 100;
    this.t.tastiness = 0.5f;
    this.t.diet = AssetLibrary<ResourceAsset>.a<string>("diet_fruits", "diet_flowers");
  }

  private void initStrategic()
  {
    ResourceAsset pAsset1 = new ResourceAsset();
    pAsset1.id = "wood";
    pAsset1.path_icon = "iconResWood";
    pAsset1.path_gameplay_sprite = "wood";
    pAsset1.type = ResType.Strategic;
    pAsset1.wood = true;
    ResourceLibrary.wood = this.add(pAsset1);
    this.t.restore_nutrition = 50;
    this.t.restore_health = 0.45f;
    this.t.restore_mana = 15;
    this.t.restore_stamina = 15;
    this.t.restore_happiness = 10;
    this.t.tastiness = 0.2f;
    this.t.diet = AssetLibrary<ResourceAsset>.a<string>("diet_wood");
    ResourceLibrary.stone = this.clone("stone", "$TEMPLATE_STRATEGIC_MINERAL$");
    this.t.drop_per_mass = 150;
    this.t.path_icon = "iconResStone";
    this.t.path_gameplay_sprite = "stone";
    this.t.mine_rate = 25;
    this.t.restore_nutrition = 20;
    this.t.restore_health = 0.2f;
    this.t.restore_mana = 10;
    this.t.restore_stamina = 10;
    this.t.restore_happiness = 5;
    this.t.tastiness = 0.1f;
    this.clone("silver", "$TEMPLATE_STRATEGIC_MINERAL$");
    this.t.path_icon = "iconResSilver";
    this.t.path_gameplay_sprite = "silver";
    this.t.restore_nutrition = 35;
    this.t.restore_health = 0.3f;
    this.t.restore_mana = 25;
    this.t.restore_stamina = 15;
    this.t.restore_happiness = 20;
    this.t.tastiness = 0.4f;
    this.clone("mythril", "$TEMPLATE_STRATEGIC_MINERAL$");
    this.t.path_icon = "iconResMythril";
    this.t.path_gameplay_sprite = "mythril";
    this.t.restore_nutrition = 45;
    this.t.restore_health = 0.4f;
    this.t.restore_mana = 30;
    this.t.restore_stamina = 25;
    this.t.restore_happiness = 25;
    this.t.tastiness = 0.6f;
    this.clone("adamantine", "$TEMPLATE_STRATEGIC_MINERAL$");
    this.t.path_icon = "iconResAdamantine";
    this.t.path_gameplay_sprite = "adamantine";
    this.t.restore_nutrition = 55;
    this.t.restore_health = 0.5f;
    this.t.restore_mana = 45;
    this.t.restore_stamina = 40;
    this.t.restore_happiness = 35;
    this.t.tastiness = 0.8f;
    ResourceAsset pAsset2 = new ResourceAsset();
    pAsset2.id = "dragon_scales";
    pAsset2.path_icon = "iconResDragonScales";
    pAsset2.path_gameplay_sprite = "dragon_scales";
    pAsset2.type = ResType.Strategic;
    this.add(pAsset2);
    ResourceLibrary.common_metals = this.clone("common_metals", "$TEMPLATE_STRATEGIC_MINERAL$");
    this.t.path_icon = "iconResCommonMetals";
    this.t.path_gameplay_sprite = "common_metals";
    this.t.storage_max = 100;
    this.t.restore_nutrition = 35;
    this.t.restore_health = 0.3f;
    this.t.restore_mana = 15;
    this.t.restore_stamina = 15;
    this.t.restore_happiness = 10;
    this.t.tastiness = 0.2f;
    this.clone("bones", "$TEMPLATE_STRATEGIC_MINERAL$");
    this.t.drop_per_mass = 100;
    this.t.path_icon = "iconResBones";
    this.t.path_gameplay_sprite = "bones";
    this.t.tastiness = 0.3f;
    ResourceAsset pAsset3 = new ResourceAsset();
    pAsset3.id = "leather";
    pAsset3.path_icon = "iconResLeather";
    pAsset3.path_gameplay_sprite = "leather";
    pAsset3.type = ResType.Strategic;
    pAsset3.drop_per_mass = 100;
    this.add(pAsset3);
    this.clone("gems", "$TEMPLATE_STRATEGIC_MINERAL$");
    this.t.drop_per_mass = 200;
    this.t.path_icon = "iconResGems";
    this.t.path_gameplay_sprite = "gems";
    this.t.mine_rate = 1;
    this.t.restore_nutrition = 35;
    this.t.restore_health = 0.3f;
    this.t.restore_mana = 15;
    this.t.restore_stamina = 25;
    this.t.restore_happiness = 15;
    this.t.tastiness = 1f;
    ResourceAsset pAsset4 = new ResourceAsset();
    pAsset4.id = "fertilizer";
    pAsset4.path_icon = "iconFertilizer";
    pAsset4.path_gameplay_sprite = "fertilizer";
    pAsset4.type = ResType.Strategic;
    ResourceLibrary.fertilizer = this.add(pAsset4);
  }

  private void initOther()
  {
    ResourceAsset pAsset = new ResourceAsset();
    pAsset.id = "gold";
    pAsset.path_icon = "iconResGold";
    pAsset.path_gameplay_sprite = "gold";
    pAsset.maximum = 999;
    pAsset.supply_bound_give = 600;
    pAsset.supply_bound_take = 10;
    pAsset.supply_give = 100;
    pAsset.type = ResType.Currency;
    ResourceLibrary.gold = this.add(pAsset);
  }

  public override void post_init()
  {
    base.post_init();
    int num = 0;
    foreach (ResourceAsset resourceAsset in this.list)
    {
      resourceAsset.order = num++;
      resourceAsset.full_sprite_path = "items/resources/" + resourceAsset.path_gameplay_sprite;
    }
  }

  public void loadSprites()
  {
    foreach (ResourceAsset resourceAsset in this.list)
      resourceAsset.gameplay_sprites = SpriteTextureLoader.getSpriteList(resourceAsset.full_sprite_path);
  }

  public override void linkAssets()
  {
    base.linkAssets();
    foreach (ResourceAsset resourceAsset in this.list)
    {
      if (resourceAsset.type == ResType.Strategic)
        this.strategic_resource_assets.Add(resourceAsset);
      if (resourceAsset.give_trait_id != null)
      {
        resourceAsset.give_trait = new ActorTrait[resourceAsset.give_trait_id.Length];
        for (int index = 0; index < resourceAsset.give_trait_id.Length; ++index)
        {
          string pID = resourceAsset.give_trait_id[index];
          resourceAsset.give_trait[index] = AssetManager.traits.get(pID);
        }
      }
      if (resourceAsset.give_status_id != null)
      {
        resourceAsset.give_status = new StatusAsset[resourceAsset.give_status_id.Length];
        for (int index = 0; index < resourceAsset.give_status_id.Length; ++index)
        {
          string pID = resourceAsset.give_status_id[index];
          resourceAsset.give_status[index] = AssetManager.status.get(pID);
        }
      }
      if (resourceAsset.diet != null)
      {
        foreach (SubspeciesTrait subspeciesTrait in AssetManager.subspecies_traits.list)
        {
          if (subspeciesTrait.is_diet_related)
          {
            string id = subspeciesTrait.id;
            foreach (string pTag in resourceAsset.diet)
            {
              if (subspeciesTrait.base_stats_meta.hasTag(pTag))
              {
                List<string> stringList;
                if (!this.diet_food_pools.TryGetValue(id, out stringList))
                {
                  stringList = new List<string>();
                  this.diet_food_pools.Add(id, stringList);
                }
                stringList.Add(resourceAsset.id);
              }
            }
          }
        }
      }
    }
  }

  public override void editorDiagnostic()
  {
    foreach (ResourceAsset pAsset in this.list)
    {
      if (!pAsset.isTemplateAsset())
        this.checkSpriteExists("path_gameplay_sprite", pAsset.full_sprite_path, (Asset) pAsset);
    }
    base.editorDiagnostic();
  }

  public override void editorDiagnosticLocales()
  {
    base.editorDiagnosticLocales();
    foreach (ResourceAsset pAsset in this.list)
    {
      if (!pAsset.isTemplateAsset())
        this.checkLocale((Asset) pAsset, pAsset.getLocaleID());
    }
  }
}

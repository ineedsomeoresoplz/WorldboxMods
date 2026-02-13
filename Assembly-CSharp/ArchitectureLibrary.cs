// Decompiled with JetBrains decompiler
// Type: ArchitectureLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class ArchitectureLibrary : AssetLibrary<ArchitectureAsset>
{
  private const string TEMPLATE_WITH_GENERATED_BUILDINGS = "$template_with_generated_buildings$";

  public override void init()
  {
    base.init();
    this.addTemplates();
    this.addClassic();
    this.addUnique();
    this.addAnimal();
  }

  private void addTemplates()
  {
    ArchitectureAsset pAsset = new ArchitectureAsset();
    pAsset.id = "$template_with_generated_buildings$";
    pAsset.generation_target = "human";
    pAsset.generate_buildings = true;
    this.add(pAsset);
    this.t.styled_building_orders = new string[9]
    {
      "order_docks_0",
      "order_docks_1",
      "order_house_0",
      "order_hall_0",
      "order_windmill_0",
      "order_watch_tower",
      "order_temple",
      "order_library",
      "order_barracks"
    };
    this.t.shared_building_orders = new (string, string)[6]
    {
      ("order_bonfire", "bonfire"),
      ("order_statue", "statue"),
      ("order_well", "well"),
      ("order_stockpile", "stockpile"),
      ("order_mine", "mine"),
      ("order_training_dummy", "training_dummy")
    };
    this.t.actor_asset_id_trading = "boat_trading_human";
    this.t.actor_asset_id_transport = "boat_transport_human";
  }

  private void addClassic()
  {
    ArchitectureAsset pAsset = new ArchitectureAsset();
    pAsset.id = "human";
    this.add(pAsset);
    this.t.actor_asset_id_trading = "boat_trading_human";
    this.t.actor_asset_id_transport = "boat_transport_human";
    this.t.styled_building_orders = new string[19]
    {
      "order_docks_0",
      "order_tent",
      "order_house_0",
      "order_house_1",
      "order_house_2",
      "order_house_3",
      "order_house_4",
      "order_house_5",
      "order_hall_0",
      "order_hall_1",
      "order_hall_2",
      "order_windmill_0",
      "order_windmill_1",
      "order_docks_1",
      "order_watch_tower",
      "order_temple",
      "order_library",
      "order_market",
      "order_barracks"
    };
    this.t.shared_building_orders = new (string, string)[6]
    {
      ("order_bonfire", "bonfire"),
      ("order_statue", "statue"),
      ("order_well", "well"),
      ("order_stockpile", "stockpile"),
      ("order_mine", "mine"),
      ("order_training_dummy", "training_dummy")
    };
    this.clone("orc", "human");
    this.t.actor_asset_id_trading = "boat_trading_orc";
    this.t.actor_asset_id_transport = "boat_transport_orc";
    this.clone("elf", "human");
    this.t.actor_asset_id_trading = "boat_trading_elf";
    this.t.actor_asset_id_transport = "boat_transport_elf";
    this.clone("dwarf", "human");
    this.t.actor_asset_id_trading = "boat_trading_dwarf";
    this.t.actor_asset_id_transport = "boat_transport_dwarf";
  }

  private void addUnique()
  {
    this.clone("civ_necromancer", "$template_with_generated_buildings$");
    this.t.actor_asset_id_trading = "boat_trading_necromancer";
    this.t.actor_asset_id_transport = "boat_transport_necromancer";
    this.t.spread_biome_id = "biome_corrupted";
    this.clone("civ_alien", "$template_with_generated_buildings$");
    this.t.actor_asset_id_trading = "boat_trading_alien";
    this.t.actor_asset_id_transport = "boat_transport_alien";
    this.clone("civ_druid", "$template_with_generated_buildings$");
    this.t.spread_biome_id = "biome_jungle";
    this.t.actor_asset_id_trading = "boat_trading_druid";
    this.t.actor_asset_id_transport = "boat_transport_druid";
    this.clone("civ_bee", "$template_with_generated_buildings$");
    this.t.actor_asset_id_trading = "boat_trading_bee";
    this.t.actor_asset_id_transport = "boat_transport_bee";
    this.clone("civ_beetle", "$template_with_generated_buildings$");
    this.t.actor_asset_id_trading = "boat_trading_beetle";
    this.t.actor_asset_id_transport = "boat_transport_beetle";
    this.clone("civ_seal", "$template_with_generated_buildings$");
    this.t.actor_asset_id_trading = "boat_trading_seal";
    this.t.actor_asset_id_transport = "boat_transport_seal";
    this.clone("civ_unicorn", "$template_with_generated_buildings$");
    this.t.actor_asset_id_trading = "boat_trading_unicorn";
    this.t.actor_asset_id_transport = "boat_transport_unicorn";
    this.clone("civ_ghost", "$template_with_generated_buildings$");
    this.t.has_shadows = false;
    this.t.material = "jelly";
    this.t.actor_asset_id_trading = "boat_trading_ghost";
    this.t.actor_asset_id_transport = "boat_transport_ghost";
    this.clone("civ_fairy", "$template_with_generated_buildings$");
    this.t.actor_asset_id_trading = "boat_trading_fairy";
    this.t.actor_asset_id_transport = "boat_transport_fairy";
    this.clone("civ_evil_mage", "$template_with_generated_buildings$");
    this.t.actor_asset_id_trading = "boat_trading_evil_mage";
    this.t.actor_asset_id_transport = "boat_transport_evil_mage";
    this.t.replaceSharedID("order_stockpile", "stockpile_fireproof");
    this.t.burnable_buildings = false;
    this.clone("civ_white_mage", "$template_with_generated_buildings$");
    this.t.actor_asset_id_trading = "boat_trading_white_mage";
    this.t.actor_asset_id_transport = "boat_transport_white_mage";
    this.clone("civ_bandit", "$template_with_generated_buildings$");
    this.t.actor_asset_id_trading = "boat_trading_bandit";
    this.t.actor_asset_id_transport = "boat_transport_bandit";
    this.clone("civ_demon", "$template_with_generated_buildings$");
    this.t.actor_asset_id_trading = "boat_trading_demon";
    this.t.actor_asset_id_transport = "boat_transport_demon";
    this.t.burnable_buildings = false;
    this.t.spread_biome_id = "biome_infernal";
    this.t.replaceSharedID("order_stockpile", "stockpile_fireproof");
    this.clone("civ_cold_one", "$template_with_generated_buildings$");
    this.t.spread_biome_id = "biome_permafrost";
    this.t.actor_asset_id_trading = "boat_trading_cold_one";
    this.t.actor_asset_id_transport = "boat_transport_cold_one";
    this.clone("civ_angle", "$template_with_generated_buildings$");
    this.t.actor_asset_id_trading = "boat_trading_angle";
    this.t.actor_asset_id_transport = "boat_transport_angle";
    this.clone("civ_snowman", "$template_with_generated_buildings$");
    this.t.spread_biome_id = "biome_permafrost";
    this.t.actor_asset_id_trading = "boat_trading_snowman";
    this.t.actor_asset_id_transport = "boat_transport_snowman";
    this.clone("civ_garlic_man", "$template_with_generated_buildings$");
    this.t.actor_asset_id_trading = "boat_trading_garlic_man";
    this.t.actor_asset_id_transport = "boat_transport_garlic_man";
    this.t.spread_biome_id = "biome_garlic";
    this.clone("civ_lemon_man", "$template_with_generated_buildings$");
    this.t.actor_asset_id_trading = "boat_trading_lemon_man";
    this.t.actor_asset_id_transport = "boat_transport_lemon_man";
    this.t.spread_biome_id = "biome_lemon";
    this.clone("civ_acid_gentleman", "$template_with_generated_buildings$");
    this.t.actor_asset_id_trading = "boat_trading_acid_gentleman";
    this.t.actor_asset_id_transport = "boat_transport_acid_gentleman";
    this.t.spread_biome_id = "biome_wasteland";
    this.t.acid_affected_buildings = false;
    this.t.replaceSharedID("order_stockpile", "stockpile_acidproof");
    this.clone("civ_crystal_golem", "$template_with_generated_buildings$");
    this.t.actor_asset_id_trading = "boat_trading_crystal_golem";
    this.t.actor_asset_id_transport = "boat_transport_crystal_golem";
    this.t.spread_biome_id = "biome_crystal";
    this.t.burnable_buildings = false;
    this.clone("civ_candy_man", "$template_with_generated_buildings$");
    this.t.actor_asset_id_trading = "boat_trading_candy_man";
    this.t.actor_asset_id_transport = "boat_transport_candy_man";
    this.t.spread_biome_id = "biome_candy";
    this.clone("civ_liliar", "$template_with_generated_buildings$");
    this.t.actor_asset_id_trading = "boat_trading_liliar";
    this.t.actor_asset_id_transport = "boat_transport_liliar";
    this.t.spread_biome_id = "biome_flower";
    this.clone("civ_greg", "$template_with_generated_buildings$");
    this.t.actor_asset_id_trading = "boat_trading_greg";
    this.t.actor_asset_id_transport = "boat_transport_greg";
  }

  private void addAnimal()
  {
    this.clone("civ_cat", "$template_with_generated_buildings$");
    this.t.actor_asset_id_trading = "boat_trading_cat";
    this.t.actor_asset_id_transport = "boat_transport_cat";
    this.clone("civ_dog", "$template_with_generated_buildings$");
    this.t.actor_asset_id_trading = "boat_trading_dog";
    this.t.actor_asset_id_transport = "boat_transport_dog";
    this.clone("civ_chicken", "$template_with_generated_buildings$");
    this.t.actor_asset_id_trading = "boat_trading_chicken";
    this.t.actor_asset_id_transport = "boat_transport_chicken";
    this.clone("civ_rabbit", "$template_with_generated_buildings$");
    this.t.actor_asset_id_trading = "boat_trading_rabbit";
    this.t.actor_asset_id_transport = "boat_transport_rabbit";
    this.clone("civ_monkey", "$template_with_generated_buildings$");
    this.t.actor_asset_id_trading = "boat_trading_monkey";
    this.t.actor_asset_id_transport = "boat_transport_monkey";
    this.clone("civ_fox", "$template_with_generated_buildings$");
    this.t.actor_asset_id_trading = "boat_trading_fox";
    this.t.actor_asset_id_transport = "boat_transport_fox";
    this.clone("civ_sheep", "$template_with_generated_buildings$");
    this.t.actor_asset_id_trading = "boat_trading_sheep";
    this.t.actor_asset_id_transport = "boat_transport_sheep";
    this.clone("civ_cow", "$template_with_generated_buildings$");
    this.t.actor_asset_id_trading = "boat_trading_cow";
    this.t.actor_asset_id_transport = "boat_transport_cow";
    this.clone("civ_armadillo", "$template_with_generated_buildings$");
    this.t.actor_asset_id_trading = "boat_trading_armadillo";
    this.t.actor_asset_id_transport = "boat_transport_armadillo";
    this.clone("civ_wolf", "$template_with_generated_buildings$");
    this.t.actor_asset_id_trading = "boat_trading_wolf";
    this.t.actor_asset_id_transport = "boat_transport_wolf";
    this.clone("civ_bear", "$template_with_generated_buildings$");
    this.t.actor_asset_id_trading = "boat_trading_bear";
    this.t.actor_asset_id_transport = "boat_transport_bear";
    this.clone("civ_rhino", "$template_with_generated_buildings$");
    this.t.actor_asset_id_trading = "boat_trading_rhino";
    this.t.actor_asset_id_transport = "boat_transport_rhino";
    this.clone("civ_buffalo", "$template_with_generated_buildings$");
    this.t.actor_asset_id_trading = "boat_trading_buffalo";
    this.t.actor_asset_id_transport = "boat_transport_buffalo";
    this.clone("civ_hyena", "$template_with_generated_buildings$");
    this.t.actor_asset_id_trading = "boat_trading_hyena";
    this.t.actor_asset_id_transport = "boat_transport_hyena";
    this.clone("civ_rat", "$template_with_generated_buildings$");
    this.t.actor_asset_id_trading = "boat_trading_rat";
    this.t.actor_asset_id_transport = "boat_transport_rat";
    this.clone("civ_alpaca", "$template_with_generated_buildings$");
    this.t.actor_asset_id_trading = "boat_trading_alpaca";
    this.t.actor_asset_id_transport = "boat_transport_alpaca";
    this.clone("civ_capybara", "$template_with_generated_buildings$");
    this.t.actor_asset_id_trading = "boat_trading_capybara";
    this.t.actor_asset_id_transport = "boat_transport_capybara";
    this.clone("civ_goat", "$template_with_generated_buildings$");
    this.t.actor_asset_id_trading = "boat_trading_goat";
    this.t.actor_asset_id_transport = "boat_transport_goat";
    this.clone("civ_scorpion", "$template_with_generated_buildings$");
    this.t.actor_asset_id_trading = "boat_trading_scorpion";
    this.t.actor_asset_id_transport = "boat_transport_scorpion";
    this.clone("civ_crab", "$template_with_generated_buildings$");
    this.t.actor_asset_id_trading = "boat_trading_crab";
    this.t.actor_asset_id_transport = "boat_transport_crab";
    this.clone("civ_penguin", "$template_with_generated_buildings$");
    this.t.actor_asset_id_trading = "boat_trading_penguin";
    this.t.actor_asset_id_transport = "boat_transport_penguin";
    this.t.spread_biome_id = "biome_permafrost";
    this.clone("civ_turtle", "$template_with_generated_buildings$");
    this.t.actor_asset_id_trading = "boat_trading_turtle";
    this.t.actor_asset_id_transport = "boat_transport_turtle";
    this.clone("civ_crocodile", "$template_with_generated_buildings$");
    this.t.actor_asset_id_trading = "boat_trading_crocodile";
    this.t.actor_asset_id_transport = "boat_transport_crocodile";
    this.clone("civ_snake", "$template_with_generated_buildings$");
    this.t.actor_asset_id_trading = "boat_trading_snake";
    this.t.actor_asset_id_transport = "boat_transport_snake";
    this.clone("civ_frog", "$template_with_generated_buildings$");
    this.t.actor_asset_id_trading = "boat_trading_frog";
    this.t.actor_asset_id_transport = "boat_transport_frog";
    this.clone("civ_piranha", "$template_with_generated_buildings$");
    this.t.actor_asset_id_trading = "boat_trading_piranha";
    this.t.actor_asset_id_transport = "boat_transport_piranha";
  }

  public override void post_init()
  {
    base.post_init();
    foreach (ArchitectureAsset architectureAsset in this.list)
    {
      if (!string.IsNullOrEmpty(architectureAsset.spread_biome_id))
        architectureAsset.spread_biome = true;
    }
    this.initBuildingKeys();
  }

  private void initBuildingKeys()
  {
    foreach (ArchitectureAsset pAsset in this.list)
    {
      if (!pAsset.isTemplateAsset())
      {
        this.loadAutoBuildingsForAsset(pAsset);
        foreach ((string, string) sharedBuildingOrder in pAsset.shared_building_orders)
          pAsset.addBuildingOrderKey(sharedBuildingOrder.Item1, sharedBuildingOrder.Item2);
      }
    }
  }

  private void loadAutoBuildingsForAsset(ArchitectureAsset pAsset)
  {
    string id = pAsset.id;
    foreach (string styledBuildingOrder in pAsset.styled_building_orders)
    {
      string pID = (string) null;
      switch (styledBuildingOrder)
      {
        case "order_barracks":
          pID = "barracks_" + id;
          break;
        case "order_bonfire":
          pID = "bonfire";
          break;
        case "order_docks_0":
          pID = "fishing_docks_" + id;
          break;
        case "order_docks_1":
          pID = "docks_" + id;
          break;
        case "order_hall_0":
        case "order_hall_1":
        case "order_hall_2":
          pID = $"hall_{id}_{styledBuildingOrder.Substring(styledBuildingOrder.Length - 1)}";
          break;
        case "order_house_0":
        case "order_house_1":
        case "order_house_2":
        case "order_house_3":
        case "order_house_4":
        case "order_house_5":
          pID = $"house_{id}_{styledBuildingOrder.Substring(styledBuildingOrder.Length - 1)}";
          break;
        case "order_library":
          pID = "library_" + id;
          break;
        case "order_market":
          pID = "market_" + id;
          break;
        case "order_mine":
          pID = "mine";
          break;
        case "order_statue":
          pID = "statue";
          break;
        case "order_stockpile":
          pID = "stockpile";
          break;
        case "order_temple":
          pID = "temple_" + id;
          break;
        case "order_tent":
          pID = "tent_" + id;
          break;
        case "order_training_dummy":
          pID = "training_dummy";
          break;
        case "order_watch_tower":
          pID = "watch_tower_" + id;
          break;
        case "order_well":
          pID = "well";
          break;
        case "order_windmill_0":
        case "order_windmill_1":
          pID = $"windmill_{id}_{styledBuildingOrder.Substring(styledBuildingOrder.Length - 1)}";
          break;
      }
      if (pID != null)
        pAsset.addBuildingOrderKey(styledBuildingOrder, pID);
    }
  }
}

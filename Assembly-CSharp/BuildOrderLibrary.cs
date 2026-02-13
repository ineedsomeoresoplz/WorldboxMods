// Decompiled with JetBrains decompiler
// Type: BuildOrderLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class BuildOrderLibrary : AssetLibrary<CityBuildOrderAsset>
{
  public static BuildOrder b;

  public override void init()
  {
    base.init();
    this.initCivsBasic();
    this.initCivsBasic2();
    this.initCivsAdvanced();
  }

  private void initCivsBasic()
  {
    CityBuildOrderAsset pAsset = new CityBuildOrderAsset();
    pAsset.id = "build_order_basic";
    this.add(pAsset);
    this.t.addBuilding("order_bonfire", 1);
    this.t.addBuilding("order_stockpile", 1);
    this.t.addBuilding("order_hall_0", 1);
    BuildOrderLibrary.b.requirements_orders = AssetLibrary<CityBuildOrderAsset>.a<string>("order_bonfire");
    this.t.addBuilding("order_house_0", pCheckHouseLimit: true);
    BuildOrderLibrary.b.requirements_orders = AssetLibrary<CityBuildOrderAsset>.a<string>("order_bonfire");
    this.t.addBuilding("order_watch_tower", 1, 30, 10);
    BuildOrderLibrary.b.requirements_orders = AssetLibrary<CityBuildOrderAsset>.a<string>("order_bonfire", "order_hall_0");
    this.t.addBuilding("order_temple", 1, 90, 20, pMinZones: 20);
    BuildOrderLibrary.b.requirements_orders = AssetLibrary<CityBuildOrderAsset>.a<string>("order_bonfire", "order_hall_0", "order_statue");
    this.t.addBuilding("order_statue", 1, 70, 15);
    BuildOrderLibrary.b.requirements_types = AssetLibrary<CityBuildOrderAsset>.a<string>("type_hall");
    this.t.addBuilding("order_well", 1, 20, 10);
    BuildOrderLibrary.b.requirements_types = AssetLibrary<CityBuildOrderAsset>.a<string>("type_hall");
    this.t.addBuilding("order_mine", 1, 20, 10);
    BuildOrderLibrary.b.requirements_types = AssetLibrary<CityBuildOrderAsset>.a<string>("type_bonfire", "type_hall");
    this.t.addBuilding("order_library", 1, 50, 15);
    BuildOrderLibrary.b.requirements_types = AssetLibrary<CityBuildOrderAsset>.a<string>("type_bonfire", "type_hall");
    this.t.addBuilding("order_docks_0", 5, pBuildings: 2);
    BuildOrderLibrary.b.requirements_orders = AssetLibrary<CityBuildOrderAsset>.a<string>("order_bonfire");
    this.t.addUpgrade("order_docks_0");
    BuildOrderLibrary.b.requirements_orders = AssetLibrary<CityBuildOrderAsset>.a<string>("order_docks_0");
    this.t.addBuilding("order_windmill_0", 1, 6, 5);
    BuildOrderLibrary.b.requirements_orders = AssetLibrary<CityBuildOrderAsset>.a<string>("order_bonfire");
    this.t.addBuilding("order_barracks", 1, 50, 16 /*0x10*/, pMinZones: 20);
    BuildOrderLibrary.b.requirements_types = AssetLibrary<CityBuildOrderAsset>.a<string>("type_bonfire", "type_hall");
    this.t.addBuilding("order_training_dummy", 3, 50, 16 /*0x10*/, pMinZones: 20);
    BuildOrderLibrary.b.requirements_types = AssetLibrary<CityBuildOrderAsset>.a<string>("type_barracks");
  }

  private void initCivsBasic2()
  {
    CityBuildOrderAsset pAsset = new CityBuildOrderAsset();
    pAsset.id = "build_order_basic_2";
    this.add(pAsset);
    this.t.addBuilding("order_bonfire", 1);
    this.t.addBuilding("order_stockpile", 1);
    this.t.addBuilding("order_hall_0", 1);
    BuildOrderLibrary.b.requirements_orders = AssetLibrary<CityBuildOrderAsset>.a<string>("order_bonfire");
    this.t.addBuilding("order_house_0", pCheckHouseLimit: true);
    BuildOrderLibrary.b.requirements_orders = AssetLibrary<CityBuildOrderAsset>.a<string>("order_bonfire");
    this.t.addBuilding("order_watch_tower", 1, 30, 10);
    BuildOrderLibrary.b.requirements_orders = AssetLibrary<CityBuildOrderAsset>.a<string>("order_bonfire", "order_hall_0");
    this.t.addBuilding("order_temple", 1, 90, 20, pMinZones: 20);
    BuildOrderLibrary.b.requirements_orders = AssetLibrary<CityBuildOrderAsset>.a<string>("order_bonfire", "order_hall_0");
    this.t.addBuilding("order_mine", 1, 20, 10);
    BuildOrderLibrary.b.requirements_types = AssetLibrary<CityBuildOrderAsset>.a<string>("type_bonfire", "type_hall");
    this.t.addBuilding("order_library", 1, 50, 15);
    BuildOrderLibrary.b.requirements_types = AssetLibrary<CityBuildOrderAsset>.a<string>("type_bonfire", "type_hall");
    this.t.addBuilding("order_docks_0", 5, pBuildings: 2);
    BuildOrderLibrary.b.requirements_orders = AssetLibrary<CityBuildOrderAsset>.a<string>("order_bonfire");
    this.t.addUpgrade("order_docks_0");
    BuildOrderLibrary.b.requirements_orders = AssetLibrary<CityBuildOrderAsset>.a<string>("order_docks_0");
    this.t.addBuilding("order_windmill_0", 1, 6, 5);
    BuildOrderLibrary.b.requirements_orders = AssetLibrary<CityBuildOrderAsset>.a<string>("order_bonfire");
    this.t.addBuilding("order_barracks", 1, 50, 16 /*0x10*/, pMinZones: 20);
    BuildOrderLibrary.b.requirements_types = AssetLibrary<CityBuildOrderAsset>.a<string>("type_bonfire", "type_hall");
    this.t.addBuilding("order_training_dummy", 3, 50, 16 /*0x10*/, pMinZones: 20);
    BuildOrderLibrary.b.requirements_types = AssetLibrary<CityBuildOrderAsset>.a<string>("type_barracks");
  }

  private void initCivsAdvanced()
  {
    CityBuildOrderAsset pAsset = new CityBuildOrderAsset();
    pAsset.id = "build_order_advanced";
    this.add(pAsset);
    this.t.addBuilding("order_hall_0", 1);
    BuildOrderLibrary.b.requirements_orders = AssetLibrary<CityBuildOrderAsset>.a<string>("order_bonfire");
    this.t.addBuilding("order_bonfire", 1);
    this.t.addBuilding("order_stockpile", 1);
    this.t.addBuilding("order_house_0", pCheckHouseLimit: true);
    BuildOrderLibrary.b.requirements_orders = AssetLibrary<CityBuildOrderAsset>.a<string>("order_bonfire");
    this.t.addBuilding("order_tent", pCheckHouseLimit: true);
    BuildOrderLibrary.b.requirements_orders = AssetLibrary<CityBuildOrderAsset>.a<string>("order_bonfire");
    this.t.addUpgrade("order_tent");
    BuildOrderLibrary.b.requirements_orders = AssetLibrary<CityBuildOrderAsset>.a<string>("order_tent");
    this.t.addUpgrade("order_house_0");
    BuildOrderLibrary.b.requirements_orders = AssetLibrary<CityBuildOrderAsset>.a<string>("order_hall_0", "order_house_0");
    this.t.addUpgrade("order_house_1");
    BuildOrderLibrary.b.requirements_orders = AssetLibrary<CityBuildOrderAsset>.a<string>("order_hall_1", "order_house_1");
    this.t.addUpgrade("order_house_2");
    BuildOrderLibrary.b.requirements_orders = AssetLibrary<CityBuildOrderAsset>.a<string>("order_hall_1", "order_house_2");
    this.t.addUpgrade("order_house_3");
    BuildOrderLibrary.b.requirements_orders = AssetLibrary<CityBuildOrderAsset>.a<string>("order_hall_2", "order_house_3");
    this.t.addUpgrade("order_house_4");
    BuildOrderLibrary.b.requirements_orders = AssetLibrary<CityBuildOrderAsset>.a<string>("order_hall_2", "order_house_4");
    this.t.addUpgrade("order_hall_0", pPop: 30, pBuildings: 8);
    BuildOrderLibrary.b.requirements_orders = AssetLibrary<CityBuildOrderAsset>.a<string>("order_house_1");
    this.t.addUpgrade("order_hall_1", pPop: 100, pBuildings: 20);
    BuildOrderLibrary.b.requirements_orders = AssetLibrary<CityBuildOrderAsset>.a<string>("order_statue", "order_mine", "order_barracks");
    this.t.addBuilding("order_windmill_0", 1, 6, 5);
    BuildOrderLibrary.b.requirements_orders = AssetLibrary<CityBuildOrderAsset>.a<string>("order_bonfire");
    this.t.addUpgrade("order_windmill_0", pPop: 40, pBuildings: 10);
    this.t.addBuilding("order_docks_0", 5, pBuildings: 2);
    BuildOrderLibrary.b.requirements_orders = AssetLibrary<CityBuildOrderAsset>.a<string>("order_bonfire");
    this.t.addUpgrade("order_docks_0");
    BuildOrderLibrary.b.requirements_orders = AssetLibrary<CityBuildOrderAsset>.a<string>("order_docks_0");
    this.t.addBuilding("order_well", 1, 20, 10);
    BuildOrderLibrary.b.requirements_types = AssetLibrary<CityBuildOrderAsset>.a<string>("type_hall");
    this.t.addBuilding("order_mine", 1, 20, 10);
    BuildOrderLibrary.b.requirements_orders = AssetLibrary<CityBuildOrderAsset>.a<string>("order_bonfire", "order_hall_0");
    this.t.addBuilding("order_barracks", 1, 50, 16 /*0x10*/, pMinZones: 20);
    BuildOrderLibrary.b.requirements_orders = AssetLibrary<CityBuildOrderAsset>.a<string>("order_hall_1");
    this.t.addBuilding("order_training_dummy", 3, 50, 16 /*0x10*/, pMinZones: 20);
    BuildOrderLibrary.b.requirements_types = AssetLibrary<CityBuildOrderAsset>.a<string>("type_barracks");
    this.t.addBuilding("order_watch_tower", 1, 30, 10);
    BuildOrderLibrary.b.requirements_orders = AssetLibrary<CityBuildOrderAsset>.a<string>("order_bonfire", "order_hall_0");
    this.t.addBuilding("order_temple", 1, 90, 20, pMinZones: 20);
    BuildOrderLibrary.b.requirements_orders = AssetLibrary<CityBuildOrderAsset>.a<string>("order_bonfire", "order_hall_1", "order_statue");
    this.t.addBuilding("order_statue", 1, 70, 15);
    BuildOrderLibrary.b.requirements_orders = AssetLibrary<CityBuildOrderAsset>.a<string>("order_hall_1");
    this.t.addBuilding("order_library", 1, 50, 15);
    BuildOrderLibrary.b.requirements_types = AssetLibrary<CityBuildOrderAsset>.a<string>("type_bonfire", "type_hall");
    this.t.addBuilding("order_market", 1, 60, 15);
    BuildOrderLibrary.b.requirements_types = AssetLibrary<CityBuildOrderAsset>.a<string>("type_bonfire", "type_hall");
  }

  public override void linkAssets()
  {
    base.linkAssets();
    foreach (CityBuildOrderAsset cityBuildOrderAsset in this.list)
      cityBuildOrderAsset.prepareForAssetGeneration();
  }
}

// Decompiled with JetBrains decompiler
// Type: WorldLawGroupLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class WorldLawGroupLibrary : BaseCategoryLibrary<WorldLawGroupAsset>
{
  public override void init()
  {
    base.init();
    WorldLawGroupAsset pAsset1 = new WorldLawGroupAsset();
    pAsset1.id = "harmony";
    pAsset1.name = "world_laws_tab_harmony";
    pAsset1.color = "#FFFAA3";
    this.add(pAsset1);
    WorldLawGroupAsset pAsset2 = new WorldLawGroupAsset();
    pAsset2.id = "diplomacy";
    pAsset2.name = "world_laws_tab_diplomacy";
    pAsset2.color = "#BAF0F4";
    this.add(pAsset2);
    WorldLawGroupAsset pAsset3 = new WorldLawGroupAsset();
    pAsset3.id = "civilizations";
    pAsset3.name = "world_laws_tab_civilizations";
    pAsset3.color = "#BAF0F4";
    this.add(pAsset3);
    WorldLawGroupAsset pAsset4 = new WorldLawGroupAsset();
    pAsset4.id = "units";
    pAsset4.name = "world_laws_tab_units";
    pAsset4.color = "#FF6B86";
    this.add(pAsset4);
    WorldLawGroupAsset pAsset5 = new WorldLawGroupAsset();
    pAsset5.id = "mobs";
    pAsset5.name = "world_laws_tab_mobs";
    pAsset5.color = "#BAFFC2";
    this.add(pAsset5);
    WorldLawGroupAsset pAsset6 = new WorldLawGroupAsset();
    pAsset6.id = "spawn";
    pAsset6.name = "world_laws_tab_spawn";
    pAsset6.color = "#F482FF";
    this.add(pAsset6);
    WorldLawGroupAsset pAsset7 = new WorldLawGroupAsset();
    pAsset7.id = "nature";
    pAsset7.name = "world_laws_tab_nature";
    pAsset7.color = "#68FF77";
    this.add(pAsset7);
    WorldLawGroupAsset pAsset8 = new WorldLawGroupAsset();
    pAsset8.id = "trees";
    pAsset8.name = "world_laws_tab_trees";
    pAsset8.color = "#DEEA5D";
    this.add(pAsset8);
    WorldLawGroupAsset pAsset9 = new WorldLawGroupAsset();
    pAsset9.id = "plants";
    pAsset9.name = "world_laws_tab_plants";
    pAsset9.color = "#E1E894";
    this.add(pAsset9);
    WorldLawGroupAsset pAsset10 = new WorldLawGroupAsset();
    pAsset10.id = "fungi";
    pAsset10.name = "world_laws_tab_fungi";
    pAsset10.color = "#FF9699";
    this.add(pAsset10);
    WorldLawGroupAsset pAsset11 = new WorldLawGroupAsset();
    pAsset11.id = "biomes";
    pAsset11.name = "world_laws_tab_biomes";
    pAsset11.color = "#FFDD32";
    this.add(pAsset11);
    WorldLawGroupAsset pAsset12 = new WorldLawGroupAsset();
    pAsset12.id = "weather";
    pAsset12.name = "world_laws_tab_weather";
    pAsset12.color = "#59B9FF";
    this.add(pAsset12);
    WorldLawGroupAsset pAsset13 = new WorldLawGroupAsset();
    pAsset13.id = "disasters";
    pAsset13.name = "world_laws_tab_disasters";
    pAsset13.color = "#FF6B86";
    this.add(pAsset13);
    WorldLawGroupAsset pAsset14 = new WorldLawGroupAsset();
    pAsset14.id = "other";
    pAsset14.name = "world_laws_tab_other";
    pAsset14.color = "#D8D8D8";
    this.add(pAsset14);
  }
}

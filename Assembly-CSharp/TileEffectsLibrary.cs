// Decompiled with JetBrains decompiler
// Type: TileEffectsLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class TileEffectsLibrary : AssetLibrary<TileEffectAsset>
{
  private static Dictionary<string, List<TileEffectAsset>> _dict_effects = new Dictionary<string, List<TileEffectAsset>>();

  public override void init()
  {
    base.init();
    TileEffectAsset pAsset1 = new TileEffectAsset();
    pAsset1.id = "wave_normal";
    pAsset1.rate = 100;
    pAsset1.chance = 1f;
    pAsset1.path_sprite = "effects/tile_effects/waves/wave_normal";
    this.add(pAsset1);
    this.t.addTileTypes("close_ocean", "deep_ocean", "shallow_waters");
    TileEffectAsset pAsset2 = new TileEffectAsset();
    pAsset2.id = "wave_mermaid";
    pAsset2.chance = 0.05f;
    pAsset2.path_sprite = "effects/tile_effects/waves/wave_mermaid";
    this.add(pAsset2);
    this.t.addTileTypes("close_ocean", "deep_ocean");
    TileEffectAsset pAsset3 = new TileEffectAsset();
    pAsset3.id = "wave_dolphin";
    pAsset3.chance = 0.05f;
    pAsset3.path_sprite = "effects/tile_effects/waves/wave_dolphin";
    this.add(pAsset3);
    this.t.addTileTypes("close_ocean", "deep_ocean");
    TileEffectAsset pAsset4 = new TileEffectAsset();
    pAsset4.id = "wave_tentacle";
    pAsset4.chance = 0.05f;
    pAsset4.path_sprite = "effects/tile_effects/waves/wave_tentacle";
    this.add(pAsset4);
    this.t.addTileTypes("close_ocean", "deep_ocean");
    TileEffectAsset pAsset5 = new TileEffectAsset();
    pAsset5.id = "wave_shark_fin";
    pAsset5.chance = 0.05f;
    pAsset5.path_sprite = "effects/tile_effects/waves/wave_shark_fin";
    this.add(pAsset5);
    this.t.addTileTypes("close_ocean", "deep_ocean");
    TileEffectAsset pAsset6 = new TileEffectAsset();
    pAsset6.id = "wave_bubbles";
    pAsset6.chance = 0.05f;
    pAsset6.path_sprite = "effects/tile_effects/waves/wave_bubbles";
    this.add(pAsset6);
    this.t.addTileTypes("close_ocean", "deep_ocean", "shallow_waters");
    TileEffectAsset pAsset7 = new TileEffectAsset();
    pAsset7.id = "enchanted_sparkle";
    pAsset7.chance = 0.3f;
    pAsset7.path_sprite = "effects/tile_effects/enchanted_sparkle";
    this.add(pAsset7);
    this.t.addTileTypes("enchanted_high", "enchanted_low");
    TileEffectAsset pAsset8 = new TileEffectAsset();
    pAsset8.id = "paradox_effect";
    pAsset8.chance = 0.3f;
    pAsset8.path_sprite = "effects/tile_effects/paradox_effect";
    this.add(pAsset8);
    this.t.addTileTypes("paradox_high", "paradox_low");
    TileEffectAsset pAsset9 = new TileEffectAsset();
    pAsset9.id = "desert_effect";
    pAsset9.chance = 0.3f;
    pAsset9.path_sprite = "effects/tile_effects/desert_effect";
    this.add(pAsset9);
    this.t.addTileTypes("desert_high", "desert_low");
    TileEffectAsset pAsset10 = new TileEffectAsset();
    pAsset10.id = "celestial_effect";
    pAsset10.chance = 0.3f;
    pAsset10.path_sprite = "effects/tile_effects/celestial_effect";
    this.add(pAsset10);
    this.t.addTileTypes("celestial_low", "celestial_high");
    TileEffectAsset pAsset11 = new TileEffectAsset();
    pAsset11.id = "singularity_effect";
    pAsset11.chance = 0.3f;
    pAsset11.path_sprite = "effects/tile_effects/singularity_effect";
    this.add(pAsset11);
    this.t.addTileTypes("singularity_high", "singularity_low");
    TileEffectAsset pAsset12 = new TileEffectAsset();
    pAsset12.id = "corrupted_effect";
    pAsset12.chance = 0.3f;
    pAsset12.path_sprite = "effects/tile_effects/corrupted_effect";
    this.add(pAsset12);
    this.t.addTileTypes("corrupted_low", "corrupted_high");
    TileEffectAsset pAsset13 = new TileEffectAsset();
    pAsset13.id = "infernal_effect";
    pAsset13.chance = 0.3f;
    pAsset13.path_sprite = "effects/tile_effects/infernal_effect";
    this.add(pAsset13);
    this.t.addTileTypes("infernal_low", "infernal_high");
    TileEffectAsset pAsset14 = new TileEffectAsset();
    pAsset14.id = "wind_effect";
    pAsset14.chance = 0.3f;
    pAsset14.path_sprite = "effects/tile_effects/wind_effect";
    this.add(pAsset14);
    this.t.addTileTypes("savanna_high", "savanna_low");
    TileEffectAsset pAsset15 = new TileEffectAsset();
    pAsset15.id = "birch_effect";
    pAsset15.chance = 0.3f;
    pAsset15.path_sprite = "effects/tile_effects/birch_effect";
    this.add(pAsset15);
    this.t.addTileTypes("birch_high", "birch_low");
    TileEffectAsset pAsset16 = new TileEffectAsset();
    pAsset16.id = "maple_effect";
    pAsset16.chance = 0.3f;
    pAsset16.path_sprite = "effects/tile_effects/maple_effect";
    this.add(pAsset16);
    this.t.addTileTypes("maple_high", "maple_low");
    TileEffectAsset pAsset17 = new TileEffectAsset();
    pAsset17.id = "lava_effect";
    pAsset17.chance = 0.8f;
    pAsset17.path_sprite = "effects/tile_effects/lava_effect";
    this.add(pAsset17);
    this.t.addTileTypes("lava3", "lava2");
    TileEffectAsset pAsset18 = new TileEffectAsset();
    pAsset18.id = "permafrost_effect";
    pAsset18.chance = 0.3f;
    pAsset18.path_sprite = "effects/tile_effects/permafrost_effect";
    this.add(pAsset18);
    this.t.addTileTypes("permafrost_high", "permafrost_low");
    TileEffectAsset pAsset19 = new TileEffectAsset();
    pAsset19.id = "swamp_effect";
    pAsset19.chance = 0.3f;
    pAsset19.path_sprite = "effects/tile_effects/swamp_effect";
    this.add(pAsset19);
    this.t.addTileTypes("swamp_high", "swamp_low");
    TileEffectAsset pAsset20 = new TileEffectAsset();
    pAsset20.id = "sand_effect";
    pAsset20.chance = 0.3f;
    pAsset20.path_sprite = "effects/tile_effects/desert_effect";
    this.add(pAsset20);
    this.t.addTileTypes("sand");
    TileEffectAsset pAsset21 = new TileEffectAsset();
    pAsset21.id = "mountain_effect";
    pAsset21.chance = 0.3f;
    pAsset21.path_sprite = "effects/tile_effects/wind_effect";
    this.add(pAsset21);
    this.t.addTileTypes("hills", "mountains");
    TileEffectAsset pAsset22 = new TileEffectAsset();
    pAsset22.id = "soil_effect";
    pAsset22.chance = 0.3f;
    pAsset22.path_sprite = "effects/tile_effects/wind_effect";
    this.add(pAsset22);
    this.t.addTileTypes("soil_high", "soil_low");
    TileEffectAsset pAsset23 = new TileEffectAsset();
    pAsset23.id = "clover_effect";
    pAsset23.chance = 0.3f;
    pAsset23.path_sprite = "effects/tile_effects/clover_effect";
    this.add(pAsset23);
    this.t.addTileTypes("clover_high", "clover_low");
    TileEffectAsset pAsset24 = new TileEffectAsset();
    pAsset24.id = "garlic_effect";
    pAsset24.chance = 0.3f;
    pAsset24.path_sprite = "effects/tile_effects/infernal_effect";
    this.add(pAsset24);
    this.t.addTileTypes("garlic_high", "garlic_low");
    TileEffectAsset pAsset25 = new TileEffectAsset();
    pAsset25.id = "flower_effect";
    pAsset25.chance = 0.1f;
    pAsset25.path_sprite = "effects/tile_effects/flower_effect";
    this.add(pAsset25);
    this.t.addTileTypes("flower_high", "flower_low");
    TileEffectAsset pAsset26 = new TileEffectAsset();
    pAsset26.id = "crystal_effect";
    pAsset26.chance = 0.3f;
    pAsset26.path_sprite = "effects/tile_effects/crystal_effect";
    this.add(pAsset26);
    this.t.addTileTypes("crystal_high", "crystal_low");
    TileEffectAsset pAsset27 = new TileEffectAsset();
    pAsset27.id = "jungle_effect";
    pAsset27.chance = 0.3f;
    pAsset27.path_sprite = "effects/tile_effects/jungle_effect";
    this.add(pAsset27);
    this.t.addTileTypes("jungle_high", "jungle_low");
    TileEffectAsset pAsset28 = new TileEffectAsset();
    pAsset28.id = "mushroom_effect";
    pAsset28.chance = 0.3f;
    pAsset28.path_sprite = "effects/tile_effects/mushroom_effect";
    this.add(pAsset28);
    this.t.addTileTypes("mushroom_high", "mushroom_low");
    TileEffectAsset pAsset29 = new TileEffectAsset();
    pAsset29.id = "lemon_effect";
    pAsset29.chance = 0.3f;
    pAsset29.path_sprite = "effects/tile_effects/lemon_effect";
    this.add(pAsset29);
    this.t.addTileTypes("lemon_high", "lemon_low");
    TileEffectAsset pAsset30 = new TileEffectAsset();
    pAsset30.id = "candy_effect";
    pAsset30.chance = 0.1f;
    pAsset30.path_sprite = "effects/tile_effects/candy_effect";
    this.add(pAsset30);
    this.t.addTileTypes("candy_high", "candy_low");
    TileEffectAsset pAsset31 = new TileEffectAsset();
    pAsset31.id = "grass_effect";
    pAsset31.chance = 0.3f;
    pAsset31.path_sprite = "effects/tile_effects/birch_effect";
    this.add(pAsset31);
    this.t.addTileTypes("grass_high", "grass_low");
    TileEffectAsset pAsset32 = new TileEffectAsset();
    pAsset32.id = "wasteland_effect";
    pAsset32.chance = 0.3f;
    pAsset32.path_sprite = "effects/tile_effects/wasteland_effect";
    this.add(pAsset32);
    this.t.addTileTypes("wasteland_high", "wasteland_low");
    TileEffectAsset pAsset33 = new TileEffectAsset();
    pAsset33.id = "rocklands_effect";
    pAsset33.chance = 0.3f;
    pAsset33.path_sprite = "effects/tile_effects/rocklands_effect";
    this.add(pAsset33);
    this.t.addTileTypes("rocklands_high", "rocklands_low");
  }

  public override void linkAssets()
  {
    base.linkAssets();
    this.fillPool();
  }

  private void fillPool()
  {
    foreach (TileEffectAsset pAsset in this.list)
    {
      foreach (string tileType in pAsset.tile_types)
        this.addToDict(tileType, pAsset);
    }
  }

  private void addToDict(string pTileTypeID, TileEffectAsset pAsset)
  {
    List<TileEffectAsset> pList;
    if (!TileEffectsLibrary._dict_effects.TryGetValue(pTileTypeID, out pList))
    {
      pList = new List<TileEffectAsset>();
      TileEffectsLibrary._dict_effects.Add(pTileTypeID, pList);
    }
    pList.AddTimes<TileEffectAsset>(pAsset.rate, pAsset);
  }

  public static TileEffectAsset getRandomEffect(WorldTile pTile)
  {
    List<TileEffectAsset> list;
    return !TileEffectsLibrary._dict_effects.TryGetValue(pTile.Type.id, out list) ? (TileEffectAsset) null : list.GetRandom<TileEffectAsset>();
  }
}

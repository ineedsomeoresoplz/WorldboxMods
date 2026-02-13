// Decompiled with JetBrains decompiler
// Type: PreloadHelpers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

#nullable disable
public static class PreloadHelpers
{
  private static bool _initiated = false;
  public static int total_building_sprite_containers = 0;
  public static int total_building_sprites = 0;
  public static List<Sprite> all_preloaded_sprites_buildings = new List<Sprite>();

  public static void init()
  {
    if (PreloadHelpers._initiated)
      return;
    PreloadHelpers._initiated = true;
    WindowPreloader.addWindowPreloadResources();
    SmoothLoader.add((MapLoaderAction) (() => HandRendererTexturePreloader.launch()), "Preload hand item textures...", pNewWaitTimerValue: 0.2f);
    SmoothLoader.add((MapLoaderAction) (() =>
    {
      AssetManager.actor_library.preloadMainUnitSprites();
      Debug.Log((object) ("Loaded unit related sprites: " + ActorTextureSubAsset.all_preloaded_sprites_units.Count.ToString()));
    }), "Preload main unit textures...", pNewWaitTimerValue: 0.1f);
    SmoothLoader.add((MapLoaderAction) (() =>
    {
      AssetManager.subspecies_traits.preloadMainUnitSprites();
      Debug.Log((object) ("Loaded unit related sprites: " + ActorTextureSubAsset.all_preloaded_sprites_units.Count.ToString()));
    }), "Preload unit trait textures...", pNewWaitTimerValue: 0.1f);
    SmoothLoader.add((MapLoaderAction) (() => PreloadHelpers.preloadBuildingSpriteLists()), "Preload building sprite lists...", pNewWaitTimerValue: 0.1f);
    SmoothLoader.add((MapLoaderAction) (() => PreloadHelpers.preloadBuildingSprites()), "Preload building sprites...", pNewWaitTimerValue: 0.1f);
    SmoothLoader.add((MapLoaderAction) (() => PreloadHelpers.preloadBoatAnimations()), "Preload boat animations...", pNewWaitTimerValue: 0.1f);
    SmoothLoader.add((MapLoaderAction) (() => PreloadHelpers.preloadActorAnimations()), "Preload unit animations...", pNewWaitTimerValue: 0.1f);
    SmoothLoader.add((MapLoaderAction) (() => PreloadHelpers.preloadPixelBagsUnits()), "Preload pixel bags units...", pNewWaitTimerValue: 0.1f);
    SmoothLoader.add((MapLoaderAction) (() => PreloadHelpers.preloadPixelBagsBuildings()), "Preload pixel bags buildings...", pNewWaitTimerValue: 0.1f);
  }

  private static void preloadBuildingSpriteLists()
  {
    if (!Config.preload_buildings)
      return;
    foreach (BuildingAsset buildingAsset in AssetManager.buildings.list)
      buildingAsset.loadBuildingSpriteList();
  }

  private static void preloadBuildingSprites()
  {
    if (!Config.preload_buildings)
      return;
    foreach (BuildingAsset buildingAsset in AssetManager.buildings.list)
      buildingAsset.loadBuildingSprites();
    AssetManager.dynamic_sprites_library.checkDirty();
  }

  private static void preloadBoatAnimations()
  {
    if (!Config.preload_units)
      return;
    foreach (ActorAsset listOnlyBoatAsset in AssetManager.actor_library.list_only_boat_assets)
      ActorAnimationLoader.loadAnimationBoat(listOnlyBoatAsset.boat_texture_id);
  }

  private static void preloadActorAnimations()
  {
    if (!Config.preload_units)
      return;
    foreach (ActorAsset pAsset in AssetManager.actor_library.list)
    {
      if (!pAsset.isTemplateAsset() && !pAsset.is_boat && !pAsset.ignore_generic_render && pAsset.texture_asset != null)
      {
        foreach (KeyValuePair<string, Sprite[]> dictMain in pAsset.texture_asset.dict_mains)
          ActorAnimationLoader.getAnimationContainer(dictMain.Key, pAsset);
      }
    }
  }

  private static void preloadPixelBagsUnits()
  {
    if (!Config.preload_units)
      return;
    foreach (Sprite preloadedSpritesUnit in ActorTextureSubAsset.all_preloaded_sprites_units)
      PixelBagManager.preloadPixelBagUnit(preloadedSpritesUnit);
  }

  private static void preloadPixelBagsBuildings()
  {
    if (!Config.preload_buildings)
      return;
    foreach (Sprite preloadedSpritesBuilding in PreloadHelpers.all_preloaded_sprites_buildings)
      PixelBagManager.preloadPixelBagUnit(preloadedSpritesBuilding);
  }

  private static void createPreloadReport()
  {
    string str1 = "GenAssets/wbdiag/preloaded_assets_report.txt";
    using (StringBuilderPool stringBuilderPool1 = new StringBuilderPool())
    {
      int num1 = 0 + ActorTextureSubAsset.all_preloaded_sprites_units.Count + PixelBagManager.total + SpriteTextureLoader.total_sprites + SpriteTextureLoader.total_sprites_list + SpriteTextureLoader.total_sprites_list_single_sprites + ActorAnimationLoader.count_units + ActorAnimationLoader.count_heads + ActorAnimationLoader.count_boats + ActorTextureSubAsset.getTotal() + PreloadHelpers.total_building_sprite_containers + PreloadHelpers.total_building_sprites + HandRendererTexturePreloader.getTotal();
      int num2 = 0;
      foreach (BaseAssetLibrary baseAssetLibrary in AssetManager.getList())
        num2 += baseAssetLibrary.total_items;
      stringBuilderPool1.AppendLine("# Preloaded Assets Report");
      stringBuilderPool1.AppendLine();
      stringBuilderPool1.AppendLine("Total Preloaded Graphical Stuff: " + num1.ToString());
      stringBuilderPool1.AppendLine("Total Preloaded Assets: " + num2.ToString());
      stringBuilderPool1.AppendLine();
      stringBuilderPool1.AppendLine("[Sprites] Preloaded Sprites: " + ActorTextureSubAsset.all_preloaded_sprites_units.Count.ToString());
      stringBuilderPool1.AppendLine("[Sprites] Preloaded Building Sprites: " + PreloadHelpers.total_building_sprites.ToString());
      stringBuilderPool1.AppendLine("[Sprites] Hand Renderer Sprites: " + HandRendererTexturePreloader.getTotal().ToString());
      stringBuilderPool1.AppendLine("[Objects] Preloaded Building Sprite Containers: " + PreloadHelpers.total_building_sprite_containers.ToString());
      stringBuilderPool1.AppendLine("[Objects] Preloaded Pixel Bags: " + PixelBagManager.total.ToString());
      stringBuilderPool1.AppendLine("[Objects] Texture Sub Assets: " + ActorTextureSubAsset.getTotal().ToString());
      stringBuilderPool1.AppendLine();
      stringBuilderPool1.AppendLine("[SpriteTextureLoader] Total Single Sprites: " + SpriteTextureLoader.total_sprites.ToString());
      StringBuilderPool stringBuilderPool2 = stringBuilderPool1;
      int num3 = SpriteTextureLoader.total_sprites_list;
      string str2 = "[SpriteTextureLoader] Total Sprites Lists: " + num3.ToString();
      stringBuilderPool2.AppendLine(str2);
      StringBuilderPool stringBuilderPool3 = stringBuilderPool1;
      num3 = SpriteTextureLoader.total_sprites_list_single_sprites;
      string str3 = "[SpriteTextureLoader] Total Sprites Lists Single Sprites: " + num3.ToString();
      stringBuilderPool3.AppendLine(str3);
      stringBuilderPool1.AppendLine();
      StringBuilderPool stringBuilderPool4 = stringBuilderPool1;
      num3 = ActorAnimationLoader.count_units;
      string str4 = "[ActorAnimationLoader] Total Units: " + num3.ToString();
      stringBuilderPool4.AppendLine(str4);
      StringBuilderPool stringBuilderPool5 = stringBuilderPool1;
      num3 = ActorAnimationLoader.count_heads;
      string str5 = "[ActorAnimationLoader] Total Heads: " + num3.ToString();
      stringBuilderPool5.AppendLine(str5);
      StringBuilderPool stringBuilderPool6 = stringBuilderPool1;
      num3 = ActorAnimationLoader.count_boats;
      string str6 = "[ActorAnimationLoader] Total Boats: " + num3.ToString();
      stringBuilderPool6.AppendLine(str6);
      stringBuilderPool1.AppendLine();
      foreach (BaseAssetLibrary baseAssetLibrary in AssetManager.getList())
      {
        StringBuilderPool stringBuilderPool7 = stringBuilderPool1;
        string id = baseAssetLibrary.id;
        num3 = baseAssetLibrary.total_items;
        string str7 = num3.ToString();
        string str8 = $"{id}: {str7}";
        stringBuilderPool7.AppendLine(str8);
      }
      File.WriteAllTextAsync(str1, stringBuilderPool1.ToString(), new CancellationToken());
    }
  }
}

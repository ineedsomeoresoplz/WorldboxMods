// Decompiled with JetBrains decompiler
// Type: HandRendererTexturePreloader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class HandRendererTexturePreloader
{
  private static int _preloaded_items_counter;

  public static void launch()
  {
    AssetManager.items.loadSprites();
    AssetManager.unit_hand_tools.loadSprites();
    AssetManager.resources.loadSprites();
    HandRendererTexturePreloader.preloadItemsIntoAtlas();
  }

  private static void preloadItemsIntoAtlas()
  {
    foreach (UnitHandToolAsset unitHandToolAsset in AssetManager.unit_hand_tools.list)
    {
      bool isColored = unitHandToolAsset.is_colored;
      HandRendererTexturePreloader.preloadSpritesUnitHands(unitHandToolAsset.getSprites(), isColored);
    }
    foreach (EquipmentAsset equipmentAsset in AssetManager.items.list)
    {
      bool isColored = equipmentAsset.is_colored;
      HandRendererTexturePreloader.preloadSpritesUnitHands(equipmentAsset.getSprites(), isColored);
    }
    foreach (ResourceAsset resourceAsset in AssetManager.resources.list)
    {
      bool isColored = resourceAsset.is_colored;
      HandRendererTexturePreloader.preloadSpritesUnitHands(resourceAsset.getSprites(), isColored);
    }
    Debug.Log((object) $"Total Preloaded Hand Renderer Sprites : {HandRendererTexturePreloader._preloaded_items_counter.ToString()} with colors {ColorAsset.getAllColorsList().Count.ToString()}");
  }

  private static void preloadSpritesUnitHands(Sprite[] pSprites, bool pUseColors)
  {
    if (pSprites == null)
      return;
    if (pUseColors)
    {
      foreach (ColorAsset allColors in ColorAsset.getAllColorsList())
      {
        foreach (Sprite pSprite in pSprites)
        {
          DynamicSprites.preloadItemSprite(pSprite, allColors);
          ++HandRendererTexturePreloader._preloaded_items_counter;
        }
      }
    }
    else
    {
      foreach (Sprite pSprite in pSprites)
      {
        DynamicSprites.preloadItemSprite(pSprite);
        ++HandRendererTexturePreloader._preloaded_items_counter;
      }
    }
  }

  public static int getTotal() => HandRendererTexturePreloader._preloaded_items_counter;
}

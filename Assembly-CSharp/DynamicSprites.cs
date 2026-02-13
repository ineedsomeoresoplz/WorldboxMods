// Decompiled with JetBrains decompiler
// Type: DynamicSprites
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class DynamicSprites
{
  public const int NO_COLOR_ID = -900000;

  public static Sprite getIconWithColors(
    Sprite pSprite,
    PhenotypeAsset pPhenotype,
    ColorAsset pKingdomColor)
  {
    DynamicSpritesAsset icons = DynamicSpritesLibrary.icons;
    long num = (long) (pSprite.GetHashCode() * 10000 + (pPhenotype != null ? pPhenotype.GetHashCode() : 0) * 100 + (pKingdomColor != null ? pKingdomColor.GetHashCode() : 0));
    Sprite pSprite1 = icons.getSprite(num);
    if (pSprite1 == null)
    {
      pSprite1 = DynamicSpriteCreator.createNewIcon(icons, pSprite, pKingdomColor, pPhenotype);
      icons.addSprite(num, pSprite1);
    }
    return pSprite1;
  }

  public static Sprite getRecoloredBuilding(
    Sprite pBuildingSprite,
    ColorAsset pColor,
    DynamicSpritesAsset pAtlasAsset)
  {
    long buildingSpriteId = DynamicSprites.getBuildingSpriteID(pBuildingSprite.GetHashCode(), pColor);
    Sprite pSprite = pAtlasAsset.getSprite(buildingSpriteId);
    if (pSprite == null)
    {
      pSprite = DynamicSpriteCreator.createNewSpriteBuilding(pAtlasAsset, buildingSpriteId, pBuildingSprite, pColor);
      pAtlasAsset.addSprite(buildingSpriteId, pSprite);
    }
    return pSprite;
  }

  private static long getBuildingSpriteID(int pBaseSpriteID, ColorAsset pColor)
  {
    return ((pColor != null ? (long) (pColor.index_id + 1) : -1000000L) + 1L) * 10000000L + (long) pBaseSpriteID;
  }

  public static Sprite getBuildingLight(Building pBuilding)
  {
    return DynamicSpritesLibrary.building_lights.getSprite((long) pBuilding.last_main_sprite.GetHashCode());
  }

  public static Sprite getIcon(Sprite pSprite, ColorAsset pColorAsset)
  {
    DynamicSpritesAsset icons = DynamicSpritesLibrary.icons;
    long num = (long) (pSprite.GetHashCode() * 10000 + pColorAsset.GetHashCode());
    Sprite pSprite1 = icons.getSprite(num);
    if (pSprite1 == null)
    {
      pSprite1 = DynamicSpriteCreator.createNewIcon(icons, pSprite, pColorAsset);
      icons.addSprite(num, pSprite1);
    }
    return pSprite1;
  }

  public static Sprite getShadowBuilding(BuildingAsset pAsset, Sprite pSprite)
  {
    if (!pAsset.shadow)
      return (Sprite) null;
    int hashCode = pSprite.GetHashCode();
    return DynamicSpritesLibrary.building_shadows.getSprite((long) hashCode);
  }

  public static Sprite getShadowUnit(Sprite pSprite, int pHashCode)
  {
    DynamicSpritesAsset unitsShadows = DynamicSpritesLibrary.units_shadows;
    Sprite pSprite1 = unitsShadows.getSprite((long) pHashCode);
    if (pSprite1 == null)
    {
      pSprite1 = DynamicSpriteCreator.createNewUnitShadow(unitsShadows, pSprite);
      unitsShadows.addSprite((long) pHashCode, pSprite1);
    }
    return pSprite1;
  }

  public static void preloadItemSprite(Sprite pSprite, ColorAsset pColorAsset = null)
  {
    long itemSpriteId = DynamicSprites.getItemSpriteID(pSprite, pColorAsset);
    DynamicSpritesAsset items = DynamicSpritesLibrary.items;
    Sprite newItemSprite = DynamicSpriteCreator.createNewItemSprite(items, pSprite, pColorAsset);
    items.addSprite(itemSpriteId, newItemSprite);
  }

  public static long getItemSpriteID(Sprite pSprite, ColorAsset pColor)
  {
    int pColorID = pColor == null ? -900000 : pColor.GetHashCode();
    return DynamicSprites.getItemSpriteID(pSprite, pColorID);
  }

  public static long getItemSpriteID(Sprite pSprite, int pColorID = -900000)
  {
    return (long) (pSprite.GetHashCode() * 10000 + pColorID);
  }

  public static Sprite getCachedAtlasItemSprite(long pID, Sprite pSpriteSource)
  {
    Sprite sprite = DynamicSpritesLibrary.items.getSprite(pID);
    if (sprite != null)
      return sprite;
    Debug.LogError((object) $"[getCachedAtlasItemSprite]Dynamic sprite not found: {pID.ToString()} {pSpriteSource?.ToString()}");
    return pSpriteSource;
  }

  public static Sprite getCachedAtlasItemSprite(
    long pID,
    Sprite pSpriteSource,
    ColorAsset pColorAsset)
  {
    Sprite sprite = DynamicSpritesLibrary.items.getSprite(pID);
    if (sprite != null)
      return sprite;
    Debug.LogError((object) $"[getCachedAtlasItemSprite]Dynamic sprite not found: {pID.ToString()} {pSpriteSource?.ToString()} {(pColorAsset != null ? $"{pColorAsset.index_id.ToString()} {pColorAsset.color_main}" : "null")}");
    return pSpriteSource;
  }
}

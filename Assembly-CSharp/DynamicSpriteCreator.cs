// Decompiled with JetBrains decompiler
// Type: DynamicSpriteCreator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class DynamicSpriteCreator
{
  public static Actor debug_actor;
  private static Dictionary<Sprite, int> _int_ids_body = new Dictionary<Sprite, int>();
  private static readonly Color32 _placeholder_color_skin = Color32.op_Implicit(Toolbox.makeColor("#00FF00"));
  private static readonly List<Vector2Int> _light_colors = new List<Vector2Int>();

  public static Sprite createNewItemSprite(
    DynamicSpritesAsset pAsset,
    Sprite pSource,
    ColorAsset pKingdomColor)
  {
    UnitSpriteConstructorAtlas atlas = pAsset.getAtlas();
    Rect rect = pSource.rect;
    int width1 = (int) ((Rect) ref rect).width;
    int height1 = (int) ((Rect) ref rect).height;
    atlas.checkBounds(width1, height1);
    int width2 = ((Texture) atlas.texture).width;
    int height2 = ((Texture) atlas.texture).height;
    Color32[] pixels32 = pSource.texture.GetPixels32();
    int width3 = ((Texture) pSource.texture).width;
    for (int index1 = 0; (double) index1 < (double) ((Rect) ref rect).width; ++index1)
    {
      for (int index2 = 0; (double) index2 < (double) ((Rect) ref rect).height; ++index2)
      {
        int index3 = index1 + (int) ((Rect) ref rect).x + (index2 + (int) ((Rect) ref rect).y) * width3;
        Color32 pColor = pixels32[index3];
        if (pColor.a != (byte) 0)
        {
          Color32 color32 = DynamicColorPixelTool.checkSpecialColors(pColor, pKingdomColor, true);
          int num1 = index1 + atlas.last_x;
          int num2 = index2 + atlas.last_y;
          if (num1 < 0)
            num1 = 0;
          if (num2 < 0)
            num2 = 0;
          int index4 = num1 + num2 * width2;
          atlas.pixels[index4] = color32;
        }
      }
    }
    DynamicSpriteCreator.setAtlasDirty(atlas);
    return DynamicSpriteCreator.createFinalSprite(atlas, pSource, width1, height1);
  }

  private static Sprite createFinalSprite(
    UnitSpriteConstructorAtlas pAtlasTexture,
    Sprite pMain,
    int pWidth,
    int pHeight,
    int pResizeX = 0,
    int pResizeY = 0)
  {
    Rect rect;
    // ISSUE: explicit constructor call
    ((Rect) ref rect).\u002Ector((float) pAtlasTexture.last_x, (float) pAtlasTexture.last_y, (float) pWidth, (float) pHeight);
    Vector2 vector2;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2).\u002Ector((pMain.pivot.x + (float) pResizeX) / (float) pWidth, pMain.pivot.y / (float) pHeight);
    Sprite finalSprite = Sprite.Create(pAtlasTexture.texture, rect, vector2, 1f);
    ((Object) finalSprite).name = "gen_" + ((Object) pMain).name;
    pAtlasTexture.last_x += pWidth + 1;
    return finalSprite;
  }

  private static Sprite createNewSpriteBuildingShadow(
    DynamicSpritesAsset pDynamicSpritesAsset,
    BuildingAsset tAsset,
    Sprite pSource,
    bool pIsContructionSprite)
  {
    UnitSpriteConstructorAtlas atlas = pDynamicSpritesAsset.getAtlas();
    Rect rect = pSource.rect;
    int num1 = 3;
    int width1 = (int) ((Rect) ref rect).width;
    int height1 = (int) ((Rect) ref rect).height;
    int x = (int) ((Rect) ref rect).x;
    int y = (int) ((Rect) ref rect).y;
    atlas.checkBounds(width1 + num1, height1);
    int width2 = ((Texture) atlas.texture).width;
    int height2 = ((Texture) atlas.texture).height;
    Color32[] pixels32 = pSource.texture.GetPixels32();
    Vector2 vector2;
    float num2;
    if (pIsContructionSprite)
    {
      vector2 = BuildingLibrary.shadow_under_construction_bound;
      num2 = BuildingLibrary.shadow_under_construction_distortion;
    }
    else
    {
      vector2 = tAsset.shadow_bound;
      num2 = tAsset.shadow_distortion;
    }
    int num3 = (int) ((double) vector2.x * (double) width1);
    int num4 = (int) ((double) height1 * (double) vector2.y);
    List<Vector2Int> vector2IntList = new List<Vector2Int>();
    Color32 color32_1 = Color32.op_Implicit(Color.black);
    int width3 = ((Texture) pSource.texture).width;
    for (int index1 = 0; index1 < width1; ++index1)
    {
      for (int index2 = 0; index2 < height1; ++index2)
      {
        int index3 = index1 + x + (index2 + y) * width3;
        if (pixels32[index3].a != (byte) 0)
        {
          Color32 color32_2 = color32_1;
          if (index1 >= num3)
          {
            int num5 = index1 + atlas.last_x;
            int num6 = index2 + atlas.last_y;
            if (index2 > num4)
              num6 = (int) ((double) index2 * (double) num2) + atlas.last_y;
            if (num5 < 0)
              num5 = 0;
            if (num6 < 0)
              num6 = 0;
            vector2IntList.Add(new Vector2Int(num5, num6));
            int index4 = num5 + num6 * width2;
            atlas.pixels[index4] = color32_2;
          }
        }
      }
    }
    DynamicSpriteCreator.setAtlasDirty(atlas);
    int pWidth = width1 + num1;
    foreach (Vector2Int vector2Int in vector2IntList)
    {
      int index5 = ((Vector2Int) ref vector2Int).x + 1 + ((Vector2Int) ref vector2Int).y * width2;
      atlas.pixels[index5] = color32_1;
      int index6 = ((Vector2Int) ref vector2Int).x + 2 + ((Vector2Int) ref vector2Int).y * width2;
      atlas.pixels[index6] = color32_1;
      int index7 = ((Vector2Int) ref vector2Int).x + 1 + (((Vector2Int) ref vector2Int).y + 1) * width2;
      atlas.pixels[index7] = color32_1;
    }
    return DynamicSpriteCreator.createFinalSprite(atlas, pSource, pWidth, height1);
  }

  public static Sprite createNewUnitShadow(DynamicSpritesAsset pAsset, Sprite pSource)
  {
    UnitSpriteConstructorAtlas atlas = pAsset.getAtlas();
    Rect rect = pSource.rect;
    int num1 = 1;
    int width1 = (int) ((Rect) ref rect).width;
    int height1 = (int) ((Rect) ref rect).height;
    int x = (int) ((Rect) ref rect).x;
    int y = (int) ((Rect) ref rect).y;
    atlas.checkBounds(width1 + num1, height1);
    int width2 = ((Texture) atlas.texture).width;
    int height2 = ((Texture) atlas.texture).height;
    Color32[] pixels32 = pSource.texture.GetPixels32();
    int width3 = ((Texture) pSource.texture).width;
    for (int index1 = 0; index1 < width1; ++index1)
    {
      for (int index2 = 0; index2 < height1; ++index2)
      {
        int index3 = index1 + x + (index2 + y) * width3;
        Color32 color32 = pixels32[index3];
        if (color32.a != (byte) 0)
        {
          int num2 = index1 + atlas.last_x;
          int num3 = index2 + atlas.last_y;
          if (num2 < 0)
            num2 = 0;
          if (num3 < 0)
            num3 = 0;
          int index4 = num2 + num3 * width2;
          atlas.pixels[index4] = color32;
        }
      }
    }
    int pWidth = width1 + num1;
    DynamicSpriteCreator.setAtlasDirty(atlas);
    return DynamicSpriteCreator.createFinalSprite(atlas, pSource, pWidth, height1);
  }

  public static void createBuildingShadow(
    BuildingAsset pAsset,
    Sprite pSprite,
    bool pIsContructionSprite)
  {
    DynamicSpritesAsset buildingShadows = DynamicSpritesLibrary.building_shadows;
    buildingShadows.addSprite((long) pSprite.GetHashCode(), DynamicSpriteCreator.createNewSpriteBuildingShadow(buildingShadows, pAsset, pSprite, pIsContructionSprite));
  }

  public static Sprite createNewIcon(
    DynamicSpritesAsset pAsset,
    Sprite pSource,
    ColorAsset pKingdomColor,
    PhenotypeAsset pPhenotype = null)
  {
    UnitSpriteConstructorAtlas atlas = pAsset.getAtlas();
    if (pPhenotype != null)
      DynamicColorPixelTool.loadSkinColorsPreview(pPhenotype, 0);
    Rect rect = pSource.rect;
    int width1 = (int) ((Rect) ref rect).width;
    int height1 = (int) ((Rect) ref rect).height;
    atlas.checkBounds(width1, height1);
    int width2 = ((Texture) atlas.texture).width;
    int height2 = ((Texture) atlas.texture).height;
    Color32[] pixels32 = pSource.texture.GetPixels32();
    int width3 = ((Texture) pSource.texture).width;
    for (int index1 = 0; (double) index1 < (double) ((Rect) ref rect).width; ++index1)
    {
      for (int index2 = 0; (double) index2 < (double) ((Rect) ref rect).height; ++index2)
      {
        int index3 = index1 + (int) ((Rect) ref rect).x + (index2 + (int) ((Rect) ref rect).y) * width3;
        Color32 pColor = pixels32[index3];
        if (pColor.a != (byte) 0)
        {
          Color32 color32 = DynamicColorPixelTool.checkSpecialColors(pColor, pKingdomColor, true);
          int num1 = index1 + atlas.last_x;
          int num2 = index2 + atlas.last_y;
          if (num1 < 0)
            num1 = 0;
          if (num2 < 0)
            num2 = 0;
          int index4 = num1 + num2 * width2;
          atlas.pixels[index4] = color32;
        }
      }
    }
    DynamicSpriteCreator.setAtlasDirty(atlas);
    return DynamicSpriteCreator.createFinalSprite(atlas, pSource, width1, height1);
  }

  public static Sprite createNewSpriteBuilding(
    DynamicSpritesAsset pAssetAtlas,
    long pID,
    Sprite pSource,
    ColorAsset pKingdomColor)
  {
    UnitSpriteConstructorAtlas atlas = pAssetAtlas.getAtlas();
    Rect rect = pSource.rect;
    int width1 = (int) ((Rect) ref rect).width;
    int height1 = (int) ((Rect) ref rect).height;
    atlas.checkBounds(width1, height1);
    int width2 = ((Texture) atlas.texture).width;
    int height2 = ((Texture) atlas.texture).height;
    Color32[] pixels32 = pSource.texture.GetPixels32();
    DynamicSpriteCreator._light_colors.Clear();
    int width3 = ((Texture) pSource.texture).width;
    for (int index1 = 0; (double) index1 < (double) ((Rect) ref rect).width; ++index1)
    {
      for (int index2 = 0; (double) index2 < (double) ((Rect) ref rect).height; ++index2)
      {
        int index3 = index1 + (int) ((Rect) ref rect).x + (index2 + (int) ((Rect) ref rect).y) * width3;
        Color32 color32_1 = pixels32[index3];
        if (color32_1.a != (byte) 0)
        {
          if (Toolbox.areColorsEqual(color32_1, Toolbox.color_light))
            DynamicSpriteCreator._light_colors.Add(new Vector2Int(index1, index2));
          Color32 color32_2 = DynamicColorPixelTool.checkSpecialColors(color32_1, pKingdomColor, true);
          int num1 = index1 + atlas.last_x;
          int num2 = index2 + atlas.last_y;
          if (num1 < 0)
            num1 = 0;
          if (num2 < 0)
            num2 = 0;
          int index4 = num1 + num2 * width2;
          atlas.pixels[index4] = color32_2;
        }
      }
    }
    DynamicSpriteCreator.setAtlasDirty(atlas);
    Sprite finalSprite = DynamicSpriteCreator.createFinalSprite(atlas, pSource, width1, height1);
    if (DynamicSpriteCreator._light_colors.Count <= 0)
      return finalSprite;
    DynamicSpriteCreator.checkBuildingLightSprite(DynamicSpritesLibrary.building_lights, (long) pSource.GetHashCode(), pSource);
    return finalSprite;
  }

  private static void checkBuildingLightSprite(
    DynamicSpritesAsset pQuantumAsset,
    long pHashcodeMainSprite,
    Sprite pSprite)
  {
    if (pQuantumAsset.getSprite(pHashcodeMainSprite) != null)
      return;
    Sprite spriteBuildingLight = DynamicSpriteCreator.createNewSpriteBuildingLight(pQuantumAsset, pSprite);
    pQuantumAsset.addSprite(pHashcodeMainSprite, spriteBuildingLight);
  }

  public static Sprite createNewSpriteBuildingLight(DynamicSpritesAsset pAsset, Sprite pSource)
  {
    UnitSpriteConstructorAtlas atlas = pAsset.getAtlas();
    Rect rect = pSource.rect;
    int width1 = (int) ((Rect) ref rect).width;
    int height = (int) ((Rect) ref rect).height;
    atlas.checkBounds(width1, height);
    int width2 = ((Texture) pSource.texture).width;
    for (int index = 0; index < DynamicSpriteCreator._light_colors.Count; ++index)
    {
      Vector2Int lightColor = DynamicSpriteCreator._light_colors[index];
      DynamicSpriteCreator.drawLightPixel(atlas, ((Vector2Int) ref lightColor).x, ((Vector2Int) ref lightColor).y, width1, height, width2, Toolbox.color_light_100);
      DynamicSpriteCreator.drawLightPixel(atlas, ((Vector2Int) ref lightColor).x, ((Vector2Int) ref lightColor).y - 1, width1, height, width2, Toolbox.color_light_10);
      DynamicSpriteCreator.drawLightPixel(atlas, ((Vector2Int) ref lightColor).x - 1, ((Vector2Int) ref lightColor).y, width1, height, width2, Toolbox.color_light_10);
      DynamicSpriteCreator.drawLightPixel(atlas, ((Vector2Int) ref lightColor).x + 1, ((Vector2Int) ref lightColor).y, width1, height, width2, Toolbox.color_light_10);
      DynamicSpriteCreator.drawLightPixel(atlas, ((Vector2Int) ref lightColor).x, ((Vector2Int) ref lightColor).y + 1, width1, height, width2, Toolbox.color_light_10);
    }
    DynamicSpriteCreator.setAtlasDirty(atlas);
    return DynamicSpriteCreator.createFinalSprite(atlas, pSource, width1, height);
  }

  private static void drawLightPixel(
    UnitSpriteConstructorAtlas pAtlas,
    int pColorCoordsX,
    int pColorCoordsY,
    int pWidth,
    int pHeight,
    int pBodyTextureWidth,
    Color32 pColor)
  {
    int num1 = pColorCoordsX + pAtlas.last_x;
    int num2 = pColorCoordsY + pAtlas.last_y;
    if (num1 < 0)
      num1 = 0;
    if (num2 < 0)
      num2 = 0;
    int index = num1 + num2 * ((Texture) pAtlas.texture).width;
    if ((int) pAtlas.pixels[index].a >= (int) pColor.a)
      return;
    pAtlas.pixels[index] = pColor;
  }

  public static Sprite createNewSpriteForDebug(Sprite pSpriteSource, ColorAsset pKingdomColor)
  {
    Rect rect = pSpriteSource.rect;
    int width1 = (int) ((Rect) ref rect).width;
    int height = (int) ((Rect) ref rect).height;
    Color32[] pixels32_1 = pSpriteSource.texture.GetPixels32();
    int num = height;
    Texture2D texture2D = new Texture2D(width1, num);
    ((Texture) texture2D).filterMode = (FilterMode) 0;
    ((Texture) texture2D).wrapMode = (TextureWrapMode) 1;
    Color32[] pixels32_2 = texture2D.GetPixels32();
    int width2 = ((Texture) pSpriteSource.texture).width;
    for (int index1 = 0; (double) index1 < (double) ((Rect) ref rect).width; ++index1)
    {
      for (int index2 = 0; (double) index2 < (double) ((Rect) ref rect).height; ++index2)
      {
        int index3 = index1 + (int) ((Rect) ref rect).x + (index2 + (int) ((Rect) ref rect).y) * width2;
        Color32 pColor = pixels32_1[index3];
        if (pColor.a == (byte) 0)
        {
          pixels32_2[index3] = pColor;
        }
        else
        {
          Color32 color32 = DynamicColorPixelTool.checkSpecialColors(pColor, pKingdomColor, true);
          pixels32_2[index3] = color32;
        }
      }
    }
    texture2D.SetPixels32(pixels32_2);
    texture2D.Apply();
    Sprite newSpriteForDebug = Sprite.Create(texture2D, rect, pSpriteSource.pivot, 1f);
    ((Object) newSpriteForDebug).name = "gen_" + ((Object) pSpriteSource).name;
    return newSpriteForDebug;
  }

  public static Sprite createNewSpriteUnit(
    AnimationFrameData pFrameData,
    Sprite pSourceBody,
    Sprite pSourceHead,
    ColorAsset pKingdomColor,
    ActorAsset pAsset,
    int pPhenotypeIndex,
    int pPhenotypeShade,
    UnitTextureAtlasID pAtlasID)
  {
    UnitSpriteConstructorAtlas constructorAtlas = (UnitSpriteConstructorAtlas) null;
    switch (pAtlasID)
    {
      case UnitTextureAtlasID.Units:
        constructorAtlas = DynamicSpritesLibrary.units.getAtlas();
        break;
      case UnitTextureAtlasID.Boats:
        constructorAtlas = DynamicSpritesLibrary.boats.getAtlas();
        break;
    }
    PixelBag pixelBag1 = PixelBagManager.getPixelBag(pSourceBody, true);
    int textureRectWidth = pixelBag1.texture_rect_width;
    int textureRectHeight = pixelBag1.texture_rect_height;
    int num1 = 0;
    int num2 = 0;
    DynamicColorPixelTool.setPlaceholderSkinColor(DynamicSpriteCreator._placeholder_color_skin);
    DynamicColorPixelTool.resetSkinColors();
    if (pPhenotypeIndex != 0)
      DynamicColorPixelTool.loadPhenotype(pPhenotypeIndex, pPhenotypeShade);
    if (pSourceHead != null && pFrameData != null)
    {
      Rect rect = pSourceHead.rect;
      Vector2 posHeadNew = pFrameData.pos_head_new;
      int num3 = (int) posHeadNew.y + (int) ((Rect) ref rect).height - textureRectHeight;
      if (num3 > 0)
        num2 = num3;
      int num4 = (int) posHeadNew.x + (int) ((Rect) ref rect).width - textureRectWidth;
      if (num4 > 0)
        num1 = num4;
      else if ((double) posHeadNew.x < 0.0)
        num1 = -(int) posHeadNew.x;
    }
    int pResizeX = num1;
    int num5 = num2;
    int pWidth = textureRectWidth + pResizeX;
    int pHeight = textureRectHeight + num5;
    constructorAtlas.checkBounds(pWidth, pHeight);
    DynamicSpriteCreator.fillDebugColor(pWidth, pHeight, constructorAtlas);
    bool dynamicSpriteZombie = pAsset.dynamic_sprite_zombie;
    int pPartX1 = pResizeX + constructorAtlas.last_x;
    int lastY = constructorAtlas.last_y;
    DynamicSpriteCreator.drawPixelsAll(pixelBag1, constructorAtlas, pKingdomColor, pPartX1, lastY, dynamicSpriteZombie, pAsset);
    if (pSourceHead != null && pFrameData != null)
    {
      PixelBag pixelBag2 = PixelBagManager.getPixelBag(pSourceHead, true);
      Vector2 posHeadNew = pFrameData.pos_head_new;
      Vector2 pivot = pSourceHead.pivot;
      int num6 = (int) posHeadNew.x - (int) pivot.x;
      int num7 = (int) posHeadNew.y - (int) pivot.y;
      int num8 = pPartX1 + num6;
      int num9 = lastY + num7;
      UnitSpriteConstructorAtlas pAtlas = constructorAtlas;
      ColorAsset pKingdomColor1 = pKingdomColor;
      int pPartX2 = num8;
      int pPartY = num9;
      int num10 = dynamicSpriteZombie ? 1 : 0;
      ActorAsset pActorAsset = pAsset;
      DynamicSpriteCreator.drawPixelsAll(pixelBag2, pAtlas, pKingdomColor1, pPartX2, pPartY, num10 != 0, pActorAsset, true);
    }
    DynamicSpriteCreator.setAtlasDirty(constructorAtlas);
    return DynamicSpriteCreator.createFinalSprite(constructorAtlas, pSourceBody, pWidth, pHeight, pResizeX);
  }

  private static void fillDebugColor(int pWidth, int pHeight, UnitSpriteConstructorAtlas pAtlas)
  {
  }

  private static void drawPixelsAll(
    PixelBag pBag,
    UnitSpriteConstructorAtlas pAtlas,
    ColorAsset pKingdomColor,
    int pPartX,
    int pPartY,
    bool pDynamicZombie,
    ActorAsset pActorAsset,
    bool pHead = false)
  {
    Color32[] pixels = pAtlas.pixels;
    int width = ((Texture) pAtlas.texture).width;
    DynamicSpriteCreator.drawPixels(pixels, width, pBag.arr_pixels_k1_0, pKingdomColor.k_color_0, pPartX, pPartY, pDynamicZombie, pActorAsset, pHead: pHead);
    DynamicSpriteCreator.drawPixels(pixels, width, pBag.arr_pixels_k1_1, pKingdomColor.k_color_1, pPartX, pPartY, pDynamicZombie, pActorAsset, pHead: pHead);
    DynamicSpriteCreator.drawPixels(pixels, width, pBag.arr_pixels_k1_2, pKingdomColor.k_color_2, pPartX, pPartY, pDynamicZombie, pActorAsset, pHead: pHead);
    DynamicSpriteCreator.drawPixels(pixels, width, pBag.arr_pixels_k1_3, pKingdomColor.k_color_3, pPartX, pPartY, pDynamicZombie, pActorAsset, pHead: pHead);
    DynamicSpriteCreator.drawPixels(pixels, width, pBag.arr_pixels_k1_4, pKingdomColor.k_color_4, pPartX, pPartY, pDynamicZombie, pActorAsset, pHead: pHead);
    DynamicSpriteCreator.drawPixels(pixels, width, pBag.arr_pixels_k2_0, pKingdomColor.k2_color_0, pPartX, pPartY, pDynamicZombie, pActorAsset, pHead: pHead);
    DynamicSpriteCreator.drawPixels(pixels, width, pBag.arr_pixels_k2_1, pKingdomColor.k2_color_1, pPartX, pPartY, pDynamicZombie, pActorAsset, pHead: pHead);
    DynamicSpriteCreator.drawPixels(pixels, width, pBag.arr_pixels_k2_2, pKingdomColor.k2_color_2, pPartX, pPartY, pDynamicZombie, pActorAsset, pHead: pHead);
    DynamicSpriteCreator.drawPixels(pixels, width, pBag.arr_pixels_k2_3, pKingdomColor.k2_color_3, pPartX, pPartY, pDynamicZombie, pActorAsset, pHead: pHead);
    DynamicSpriteCreator.drawPixels(pixels, width, pBag.arr_pixels_k2_4, pKingdomColor.k2_color_4, pPartX, pPartY, pDynamicZombie, pActorAsset, pHead: pHead);
    DynamicSpriteCreator.drawPixels(pixels, width, pBag.arr_pixels_light, Toolbox.color_light_replace, pPartX, pPartY, pDynamicZombie, pActorAsset, pHead: pHead);
    DynamicSpriteCreator.drawPixels(pixels, width, pBag.arr_pixels_normal, Toolbox.color_magenta_1, pPartX, pPartY, pDynamicZombie, pActorAsset, true, pHead);
    DynamicSpriteCreator.drawPixels(pixels, width, pBag.arr_pixels_phenotype_shade_0, DynamicColorPixelTool.phenotype_shade_0, pPartX, pPartY, pDynamicZombie, pActorAsset, pHead: pHead);
    DynamicSpriteCreator.drawPixels(pixels, width, pBag.arr_pixels_phenotype_shade_1, DynamicColorPixelTool.phenotype_shade_1, pPartX, pPartY, pDynamicZombie, pActorAsset, pHead: pHead);
    DynamicSpriteCreator.drawPixels(pixels, width, pBag.arr_pixels_phenotype_shade_2, DynamicColorPixelTool.phenotype_shade_2, pPartX, pPartY, pDynamicZombie, pActorAsset, pHead: pHead);
    DynamicSpriteCreator.drawPixels(pixels, width, pBag.arr_pixels_phenotype_shade_3, DynamicColorPixelTool.phenotype_shade_3, pPartX, pPartY, pDynamicZombie, pActorAsset, pHead: pHead);
  }

  private static void drawPixels(
    Color32[] pPixels,
    int pAtlasWidth,
    Pixel[] pListSourcePixels,
    Color32 pNewColor,
    int pPartX,
    int pPartY,
    bool pDrawDynamicZombie,
    ActorAsset pActorAsset,
    bool pUseNormal = false,
    bool pHead = false)
  {
    if (pListSourcePixels == null)
      return;
    for (int index1 = 0; index1 < pListSourcePixels.Length; ++index1)
    {
      Pixel pListSourcePixel = pListSourcePixels[index1];
      Color32 pColor = pNewColor;
      int num1 = pListSourcePixel.x + pPartX;
      int num2 = pListSourcePixel.y + pPartY;
      if (num1 < 0)
        num1 = 0;
      if (num2 < 0)
        num2 = 0;
      int index2 = num1 + num2 * pAtlasWidth;
      if (pUseNormal)
        pColor = pListSourcePixel.color;
      if (pDrawDynamicZombie)
        pColor = DynamicColorPixelTool.checkZombieColors(pActorAsset, pColor, index2 / 3 + num1, pHead);
      pPixels[index2] = pColor;
    }
  }

  public static Sprite getSpriteUnit(
    AnimationFrameData pFrameData,
    Sprite pMainSprite,
    Actor pActor,
    ColorAsset pKingdomColor,
    int pPhenotypeIndex,
    int pPhenotypeShade,
    UnitTextureAtlasID pTextureAtlasID)
  {
    long pKingdomColorID = 0;
    long pShadeID = 0;
    long pPhenotypeIndex1 = (long) pPhenotypeIndex;
    long pHeadID = 0;
    long bodySpriteSmallId = (long) DynamicSpriteCreator.getBodySpriteSmallID(pMainSprite);
    if (pActor.has_rendered_sprite_head)
    {
      int num1;
      ActorAnimationLoader.int_ids_heads.TryGetValue(pActor.cached_sprite_head, out num1);
      if (num1 == 0)
      {
        int num2 = ActorAnimationLoader.int_ids_heads.Count + 1;
        ActorAnimationLoader.int_ids_heads.Add(pActor.cached_sprite_head, num2);
        num1 = num2;
      }
      pHeadID = (long) num1;
    }
    if (pPhenotypeIndex1 != 0L)
      pShadeID = (long) (pPhenotypeShade + 1);
    if (pKingdomColor != null)
      pKingdomColorID = (long) (pKingdomColor.index_id + 1);
    long num = pKingdomColorID * 1000000000000L + pHeadID * 1000000000L + bodySpriteSmallId * 1000000L + pPhenotypeIndex1 * 1000L + pShadeID;
    if (DynamicSpriteCreator.debug_actor == pActor)
      AssetManager.dynamic_sprites_library.setDebugActor(num, pKingdomColorID, pHeadID, bodySpriteSmallId, pPhenotypeIndex1, pShadeID);
    DynamicSpritesAsset units = DynamicSpritesLibrary.units;
    Sprite pSprite = units.getSprite(num);
    if (pSprite == null)
    {
      pSprite = DynamicSpriteCreator.createNewSpriteUnit(pFrameData, pMainSprite, pActor.cached_sprite_head, pKingdomColor, pActor.asset, pPhenotypeIndex, pPhenotypeShade, pTextureAtlasID);
      units.addSprite(num, pSprite);
    }
    return pSprite;
  }

  public static void setAtlasDirty(UnitSpriteConstructorAtlas pAtlas)
  {
    AssetManager.dynamic_sprites_library.setDirty();
    pAtlas.dirty = true;
    if (pAtlas.isBigSpriteSheetAtlas())
      return;
    pAtlas.checkDirty();
  }

  public static int getBodySpriteSmallID(Sprite pSprite)
  {
    int bodySpriteSmallId;
    if (!DynamicSpriteCreator._int_ids_body.TryGetValue(pSprite, out bodySpriteSmallId))
    {
      bodySpriteSmallId = DynamicSpriteCreator._int_ids_body.Count + 1;
      DynamicSpriteCreator._int_ids_body.Add(pSprite, bodySpriteSmallId);
    }
    return bodySpriteSmallId;
  }
}

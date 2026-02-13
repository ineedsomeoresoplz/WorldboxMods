// Decompiled with JetBrains decompiler
// Type: GeneratorTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class GeneratorTool : ScriptableObject
{
  private static WorldTile[,] _tiles_map;
  private static Texture2D[] _textures;
  private static List<WorldTile> _neighbours = new List<WorldTile>(4);
  private static List<WorldTile> _neighbours_all = new List<WorldTile>(8);

  internal static void Setup(WorldTile[,] pTilesMap) => GeneratorTool._tiles_map = pTilesMap;

  public static void Init() => GeneratorTool.LoadGenShapeTextures();

  internal static void applyTemplate(string pID, float pMod = 1f)
  {
    Texture2D tex = TextureRotator.Rotate(Resources.LoadAll<Texture2D>("map_gen/" + pID).GetRandom<Texture2D>(), Randy.randomInt(0, 360), new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
    TextureScale.Bilinear(tex, MapBox.width, MapBox.height);
    float num1 = (float) byte.MaxValue * pMod;
    for (int pX = 0; pX < ((Texture) tex).width; ++pX)
    {
      for (int pY = 0; pY < ((Texture) tex).height; ++pY)
      {
        WorldTile tile = World.world.GetTile(pX, pY);
        if (tile != null)
        {
          tex.GetPixel(pX, pY);
          int num2 = (int) ((1.0 - (double) tex.GetPixel(pX, pY).g) * (double) num1);
          tile.Height += num2;
        }
      }
    }
  }

  internal static void ApplyRandomShape(string pWhat = "height", float tDistMax = 2f, float pMod = 0.7f, bool pSubtract = false)
  {
    Texture2D originTexture = Object.Instantiate<Texture2D>(GeneratorTool._textures.GetRandom<Texture2D>());
    ((Object) originTexture).name = "random_shape";
    int newWidth = (int) ((double) ((Texture) originTexture).width * (double) Randy.randomFloat(0.3f, 2f));
    int newHeight = (int) ((double) ((Texture) originTexture).height * (double) Randy.randomFloat(0.3f, 2f));
    Texture2D tex = TextureRotator.Rotate(originTexture, Randy.randomInt(0, 360), new Color32((byte) 0, (byte) 0, (byte) 0, (byte) 0));
    TextureScale.Bilinear(tex, newWidth, newHeight);
    int width = ((Texture) tex).width;
    int height = ((Texture) tex).height;
    int num1 = MapBox.width / 2 - width / 2 - (int) Randy.randomFloat((float) -width * tDistMax, (float) width * tDistMax);
    int num2 = MapBox.height / 2 - height / 2 - (int) Randy.randomFloat((float) -height * tDistMax, (float) height * tDistMax);
    if (num1 < 0)
      num1 = 0;
    if (num2 < 0)
      num2 = 0;
    if (num1 + width > MapBox.width)
      num1 = MapBox.width - width;
    if (num2 + height > MapBox.height)
      num2 = MapBox.height - height;
    float num3 = (float) byte.MaxValue * pMod;
    for (int index1 = 0; index1 < ((Texture) tex).width; ++index1)
    {
      for (int index2 = 0; index2 < ((Texture) tex).height; ++index2)
      {
        WorldTile tile = World.world.GetTile(num1 + index1, num2 + index2);
        if (tile != null)
        {
          int num4 = (int) ((double) tex.GetPixel(index1, index2).a * (double) num3);
          if (pSubtract)
            num4 = -num4;
          tile.Height += num4;
        }
      }
    }
    Object.Destroy((Object) tex);
  }

  private static void LoadGenShapeTextures()
  {
    if (GeneratorTool._textures != null)
      return;
    GeneratorTool._textures = Resources.LoadAll<Texture2D>("gen_shapes");
  }

  public static void ApplyWaterLevel(WorldTile[,] tilesMap, int width, int height, int pVal)
  {
    for (int index1 = 0; index1 < width; ++index1)
    {
      for (int index2 = 0; index2 < height; ++index2)
        tilesMap[index1, index2].Height -= pVal;
    }
  }

  public static void ApplyPerlinNoise(
    WorldTile[,] tilesMap,
    int width,
    int height,
    float pPosX,
    float pPosY,
    float pAlphaMod,
    float pScaleMod,
    bool pSubtract = false,
    GeneratorTarget pTarget = GeneratorTarget.Height)
  {
    float num1 = (float) byte.MaxValue * pAlphaMod;
    float num2 = 1f;
    float num3 = 1f;
    if (width > height)
      num2 = (float) (width / height);
    else
      num3 = (float) (height / width);
    for (int index1 = 0; index1 < width; ++index1)
    {
      for (int index2 = 0; index2 < height; ++index2)
      {
        double num4 = ((double) pPosX + (double) index1) / (double) width;
        float num5 = (pPosY + (float) index2) / (float) height;
        double num6 = (double) pScaleMod;
        int num7 = (int) ((double) Mathf.PerlinNoise((float) (num4 * num6) * num2, num5 * pScaleMod * num3) * (double) num1);
        if (pSubtract)
          num7 = -num7;
        if (pTarget == GeneratorTarget.Height)
          tilesMap[index1, index2].Height += num7;
      }
    }
  }

  public static void ApplyPerlinReplace(PerlinReplaceContainer pContainer)
  {
    float num1 = (float) Randy.randomInt(0, 15000);
    float num2 = (float) Randy.randomInt(0, 15000);
    int width = MapBox.width;
    int height = MapBox.height;
    float scale = (float) pContainer.scale;
    float maxValue = (float) byte.MaxValue;
    float num3 = 1f;
    float num4 = 1f;
    if (width > height)
      num3 = (float) (width / height);
    else
      num4 = (float) (height / width);
    WorldTile[,] tilesMap = GeneratorTool._tiles_map;
    for (int index1 = 0; index1 < width; ++index1)
    {
      for (int index2 = 0; index2 < height; ++index2)
      {
        WorldTile worldTile = tilesMap[index1, index2];
        double num5 = ((double) num1 + (double) index1) / (double) width;
        float num6 = (num2 + (float) index2) / (float) height;
        double num7 = (double) scale;
        int num8 = (int) ((double) Mathf.PerlinNoise((float) (num5 * num7) * num3, num6 * scale * num4) * (double) maxValue);
        for (int index3 = 0; index3 < pContainer.options.Count; ++index3)
        {
          PerlinReplaceOption option = pContainer.options[index3];
          if (num8 > option.replace_height_value && worldTile.main_type.IsType(option.from))
            worldTile.setTileType(option.to);
        }
      }
    }
  }

  public static void UpdateTileTypes(bool pGeneratorStage = false, int pStartIndex = 0, int pAmount = 0)
  {
    int num = pStartIndex + pAmount;
    for (int index = pStartIndex; index < num; ++index)
    {
      WorldTile tiles = World.world.tiles_list[index];
      TileType typeByDepth = AssetManager.tiles.getTypeByDepth(tiles);
      tiles.setTileType(typeByDepth);
    }
  }

  public static void GenerateTileNeighbours(WorldTile[] pTilesList)
  {
    WorldTile[] worldTileArray = pTilesList;
    int length = worldTileArray.Length;
    for (int index = 0; index < length; ++index)
      GeneratorTool.generateTileNeighbours(worldTileArray[index]);
  }

  public static void generateTileNeighbours(WorldTile pTile)
  {
    WorldTile tile1 = GeneratorTool.getTile(pTile.x - 1, pTile.y);
    pTile.addNeighbour(tile1, TileDirection.Left, GeneratorTool._neighbours, GeneratorTool._neighbours_all);
    WorldTile tile2 = GeneratorTool.getTile(pTile.x + 1, pTile.y);
    pTile.addNeighbour(tile2, TileDirection.Right, GeneratorTool._neighbours, GeneratorTool._neighbours_all);
    WorldTile tile3 = GeneratorTool.getTile(pTile.x, pTile.y - 1);
    pTile.addNeighbour(tile3, TileDirection.Down, GeneratorTool._neighbours, GeneratorTool._neighbours_all);
    WorldTile tile4 = GeneratorTool.getTile(pTile.x, pTile.y + 1);
    pTile.addNeighbour(tile4, TileDirection.Up, GeneratorTool._neighbours, GeneratorTool._neighbours_all);
    WorldTile tile5 = GeneratorTool.getTile(pTile.x - 1, pTile.y - 1);
    pTile.addNeighbour(tile5, TileDirection.Null, GeneratorTool._neighbours, GeneratorTool._neighbours_all, true);
    WorldTile tile6 = GeneratorTool.getTile(pTile.x - 1, pTile.y + 1);
    pTile.addNeighbour(tile6, TileDirection.Null, GeneratorTool._neighbours, GeneratorTool._neighbours_all, true);
    WorldTile tile7 = GeneratorTool.getTile(pTile.x + 1, pTile.y - 1);
    pTile.addNeighbour(tile7, TileDirection.Null, GeneratorTool._neighbours, GeneratorTool._neighbours_all, true);
    WorldTile tile8 = GeneratorTool.getTile(pTile.x + 1, pTile.y + 1);
    pTile.addNeighbour(tile8, TileDirection.Null, GeneratorTool._neighbours, GeneratorTool._neighbours_all, true);
    pTile.neighbours = GeneratorTool._neighbours.ToArray();
    pTile.neighboursAll = GeneratorTool._neighbours_all.ToArray();
    GeneratorTool._neighbours.Clear();
    GeneratorTool._neighbours_all.Clear();
  }

  public static void ApplyRingEffect()
  {
    WorldTile[,] tilesMap = GeneratorTool._tiles_map;
    for (int index1 = 0; index1 < MapBox.width; ++index1)
    {
      for (int index2 = 0; index2 < MapBox.height; ++index2)
      {
        for (int index3 = 0; index3 < AssetManager.tiles.list.Count; ++index3)
        {
          TileType tileType = AssetManager.tiles.list[index3];
          if (tileType.additional_height != null)
          {
            bool flag = false;
            for (int index4 = 0; index4 < tileType.additional_height.Length; ++index4)
            {
              WorldTile worldTile = tilesMap[index1, index2];
              if (worldTile.Height == tileType.height_min - tileType.additional_height[index4])
              {
                worldTile.Height = tileType.height_min;
                flag = true;
                break;
              }
            }
            if (flag)
              break;
          }
        }
      }
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static WorldTile getTile(int pX, int pY)
  {
    if (pX < 0 || pX >= MapBox.width)
      return (WorldTile) null;
    return pY < 0 || pY >= MapBox.height ? (WorldTile) null : GeneratorTool._tiles_map[pX, pY];
  }
}

// Decompiled with JetBrains decompiler
// Type: MapEdges
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class MapEdges
{
  private static Texture2D textureLeft;
  private static Texture2D textureRight;
  private static Texture2D textureUp;
  private static Texture2D textureDown;
  private static Texture2D textureTempUp;
  private static Texture2D textureTempDown;
  private static int edgeSize;

  internal static void AddEdgeGradientCircle(WorldTile[,] pMap, string pWhat)
  {
    WorldTile p = pMap[MapBox.width / 2, MapBox.height / 2];
    float num1 = 0.99f;
    float num2 = 0.85f;
    float num3 = (float) (MapBox.width / 2) * num1;
    float num4 = (float) (MapBox.width / 2) * num2;
    float num5 = num3 - num4;
    foreach (WorldTile tiles in World.world.tiles_list)
    {
      float num6 = Toolbox.DistTile(tiles, p);
      if ((double) num6 > (double) num3)
        tiles.Height = 0;
      else if ((double) num6 < (double) num3 && (double) num6 > (double) num4)
      {
        float num7 = (num3 - num6) / num5;
        int num8 = (int) ((double) tiles.Height * (double) num7);
        tiles.Height = num8;
      }
    }
  }

  internal static void AddEdgeSquare(WorldTile[,] pMap, string pWhat)
  {
    MapEdges.edgeSize = 64 /*0x40*/;
    if (Object.op_Equality((Object) MapEdges.textureLeft, (Object) null))
    {
      MapEdges.textureLeft = (Texture2D) Resources.Load("edges/edge100xLeft");
      MapEdges.textureRight = (Texture2D) Resources.Load("edges/edge100xRight");
      MapEdges.textureUp = (Texture2D) Resources.Load("edges/edge100xUp");
      MapEdges.textureDown = (Texture2D) Resources.Load("edges/edge100xDown");
      MapEdges.textureTempUp = (Texture2D) Resources.Load("edges/edgeTempUp");
      MapEdges.textureTempDown = (Texture2D) Resources.Load("edges/edgeTempDown");
    }
    int num1 = (int) ((double) MapBox.width / (double) MapEdges.edgeSize) + 1;
    int num2 = (int) ((double) MapBox.height / (double) MapEdges.edgeSize) + 1;
    if (pWhat == "temperature")
    {
      for (int pX = 0; pX < num1; ++pX)
        MapEdges.fill(pX, 0, MapEdges.textureTempDown, pMap, pWhat);
      for (int pX = 0; pX < num1; ++pX)
        MapEdges.fill(pX, num2 - 2, MapEdges.textureTempUp, pMap, pWhat);
    }
    else
    {
      for (int pY = 0; pY < num2; ++pY)
        MapEdges.fill(0, pY, MapEdges.textureLeft, pMap, pWhat);
      for (int pY = 0; pY < num2; ++pY)
        MapEdges.fill(num1 - 2, pY, MapEdges.textureRight, pMap, pWhat);
      for (int pX = 0; pX < num1; ++pX)
        MapEdges.fill(pX, 0, MapEdges.textureDown, pMap, pWhat);
      for (int pX = 0; pX < num1; ++pX)
        MapEdges.fill(pX, num2 - 2, MapEdges.textureUp, pMap, pWhat);
    }
  }

  internal static void fill(
    int pX,
    int pY,
    Texture2D pTexture,
    WorldTile[,] tilesMap,
    string pWhat)
  {
    for (int index1 = 0; index1 < ((Texture) pTexture).height; ++index1)
    {
      for (int index2 = 0; index2 < ((Texture) pTexture).width; ++index2)
      {
        int num = (int) ((double) pTexture.GetPixel(index2, index1).a * (double) byte.MaxValue);
        int index3 = index2 + pX * MapEdges.edgeSize;
        int index4 = index1 + pY * MapEdges.edgeSize;
        if (index3 < MapBox.width && index4 < MapBox.height)
        {
          WorldTile tiles = tilesMap[index3, index4];
          if (tiles != null && pWhat == "height")
            tiles.Height -= num;
        }
      }
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: TownPlans
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class TownPlans
{
  public static bool isInPassableRingMap(TileZone pZone, TileZone pCenterZone = null)
  {
    TileZone mapCenterZone = World.world.zone_calculator.getMapCenterZone();
    return TownPlans.isInPassableRing(pZone, mapCenterZone);
  }

  public static bool isInPassableRing(TileZone pZone, TileZone pCityCenterZone)
  {
    double num1 = (double) Toolbox.Dist(pZone.x, pZone.y, pCityCenterZone.x, pCityCenterZone.y);
    float num2 = 1f;
    float num3 = 1f;
    double num4 = (double) (num2 + num3);
    return num1 % num4 >= (double) num2;
  }

  public static bool isPassableCross(TileZone pZone, TileZone pCityZone)
  {
    int num1 = Mathf.Abs(pCityZone.x - pZone.x);
    int num2 = Mathf.Abs(pCityZone.y - pZone.y);
    return num1 <= 1 || num2 <= 1;
  }

  public static bool isPassableLineHorizontal(TileZone pZone, TileZone _ = null)
  {
    return pZone.y % 2 == 0;
  }

  public static bool isPassableLineVertical(TileZone pZone, TileZone _ = null) => pZone.x % 2 == 0;

  public static bool isPassableDiagonal(TileZone pZone, TileZone _ = null)
  {
    return (pZone.x + pZone.y) % 3 != 0;
  }

  public static bool isPassableDiamond(TileZone pZone, TileZone _ = null)
  {
    return (pZone.x + pZone.y) % 2 != 0;
  }

  public static bool isPassableDiamondCluster(TileZone pZone, TileZone _ = null)
  {
    return (pZone.x / 2 + pZone.y / 2) % 2 == 0;
  }

  public static bool isPassableHoneycomb(TileZone pZone, TileZone _ = null)
  {
    int num = pZone.y % 2 == 0 ? 2 : 0;
    return (pZone.x + num) % 4 == 0;
  }

  public static bool isPassableBrickHorizontal(TileZone pZone, TileZone _ = null)
  {
    return TownPlans.isPassableLineVertical(pZone) && TownPlans.isPassableDiagonal(pZone);
  }

  public static bool isPassableBrickVertical(TileZone pZone, TileZone _ = null)
  {
    return TownPlans.isPassableLineHorizontal(pZone) && TownPlans.isPassableDiagonal(pZone);
  }

  public static bool isPassableLatticeSmall(TileZone pZone, TileZone _ = null)
  {
    return TownPlans.isPassableLattice(pZone, 2, 1);
  }

  public static bool isPassableLatticeMedium(TileZone pZone, TileZone _ = null)
  {
    return TownPlans.isPassableLattice(pZone, 3, 1);
  }

  public static bool isPassableLatticeBig(TileZone pZone, TileZone _ = null)
  {
    return TownPlans.isPassableLattice(pZone, 4, 2);
  }

  public static bool isPassableMadmanLabyrinth(TileZone pZone, TileZone _ = null)
  {
    float num1 = 0.7f;
    float num2 = 0.4f;
    return (double) Mathf.PerlinNoise((float) pZone.x * num1, (float) pZone.y * num1) > (double) num2;
  }

  private static bool isPassableLattice(TileZone pZone, int pSpacing, int pWidth)
  {
    int num1 = pSpacing;
    int num2 = pWidth;
    return pZone.x % num1 < num2 | pZone.y % num1 < num2;
  }

  private static bool isPassableClusters(TileZone pZone, int pSpacing, int pWidth)
  {
    int num1 = pSpacing;
    int num2 = pWidth;
    return !(pZone.x % num1 < num2 | pZone.y % num1 < num2);
  }

  public static bool isPassableClustersSmall(TileZone pZone, TileZone _ = null)
  {
    return TownPlans.isPassableClusters(pZone, 3, 1);
  }

  public static bool isPassableClustersMedium(TileZone pZone, TileZone _ = null)
  {
    return TownPlans.isPassableClusters(pZone, 4, 1);
  }

  public static bool isPassableClustersBig(TileZone pZone, TileZone _ = null)
  {
    return TownPlans.isPassableClusters(pZone, 5, 1);
  }

  public static bool debugVisualizeZone(TileZone pZone, TileZone pCursorZone = null)
  {
    DebugVariables instance = DebugVariables.instance;
    if (Object.op_Equality((Object) instance, (Object) null))
      return false;
    bool flag = true;
    if (instance.layout_cross && !TownPlans.isPassableCross(pZone, pCursorZone))
      flag = false;
    if (instance.layout_ring && !TownPlans.isInPassableRing(pZone, pCursorZone))
      flag = false;
    if (instance.layout_lines_horizontal && !TownPlans.isPassableLineHorizontal(pZone))
      flag = false;
    if (instance.layout_lines_vertical && !TownPlans.isPassableLineVertical(pZone))
      flag = false;
    if (instance.layout_diagonal && !TownPlans.isPassableDiagonal(pZone))
      flag = false;
    if (instance.layout_diamond && !TownPlans.isPassableDiamond(pZone))
      flag = false;
    if (instance.layout_diamond_cluster && !TownPlans.isPassableDiamondCluster(pZone))
      flag = false;
    if (instance.layout_lattice_small && !TownPlans.isPassableLatticeSmall(pZone))
      flag = false;
    if (instance.layout_lattice_medium && !TownPlans.isPassableLatticeMedium(pZone))
      flag = false;
    if (instance.layout_lattice_big && !TownPlans.isPassableLatticeBig(pZone))
      flag = false;
    if (instance.layout_clusters_small && !TownPlans.isPassableClustersSmall(pZone))
      flag = false;
    if (instance.layout_clusters_medium && !TownPlans.isPassableClustersMedium(pZone))
      flag = false;
    if (instance.layout_clusters_big && !TownPlans.isPassableClustersBig(pZone))
      flag = false;
    if (instance.layout_map_ring && !TownPlans.isInPassableRingMap(pZone))
      flag = false;
    if (instance.layout_honeycomb && !TownPlans.isPassableHoneycomb(pZone))
      flag = false;
    if (instance.layout_brick_horizontal && !TownPlans.isPassableBrickHorizontal(pZone))
      flag = false;
    if (instance.layout_brick_vertical && !TownPlans.isPassableBrickVertical(pZone))
      flag = false;
    if (instance.layout_madman_labyrinth && !TownPlans.isPassableMadmanLabyrinth(pZone))
      flag = false;
    return flag;
  }
}

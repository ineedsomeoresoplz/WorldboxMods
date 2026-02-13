// Decompiled with JetBrains decompiler
// Type: LavaHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public static class LavaHelper
{
  public static void heatUpLava(WorldTile pTile)
  {
    if (string.IsNullOrEmpty(pTile.Type.lava_increase))
      return;
    LavaHelper.changeLavaTile(pTile, pTile.Type.lava_increase);
  }

  public static void coolDownLava(WorldTile pTile)
  {
    if (!string.IsNullOrEmpty(pTile.Type.lava_decrease))
      LavaHelper.changeLavaTile(pTile, pTile.Type.lava_decrease);
    else
      LavaHelper.putOut(pTile);
  }

  public static void addLava(WorldTile pTile, string pType = "lava3")
  {
    if (pTile.Type.lava && string.IsNullOrEmpty(pTile.Type.lava_increase))
      return;
    pTile.startFire();
    MapAction.terraformMain(pTile, AssetManager.tiles.get(pType), TerraformLibrary.lava_damage);
  }

  private static void changeLavaTile(WorldTile pTile, string pType)
  {
    MapAction.terraformMain(pTile, AssetManager.tiles.get(pType), TerraformLibrary.lava_damage);
  }

  public static void putOut(WorldTile pTile)
  {
    if (WorldLawLibrary.world_law_forever_lava.isEnabled() || WorldLawLibrary.world_law_gaias_covenant.isEnabled())
      return;
    MapAction.increaseTile(pTile, false);
  }

  public static void removeLava(WorldTile pTile) => MapAction.decreaseTile(pTile, false);
}

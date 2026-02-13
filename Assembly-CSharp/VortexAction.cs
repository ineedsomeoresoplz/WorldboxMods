// Decompiled with JetBrains decompiler
// Type: VortexAction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public static class VortexAction
{
  private static List<VortexSwitchHelper> newTiles = new List<VortexSwitchHelper>();

  internal static void moveTiles(WorldTile pCenter, BrushData pBrush)
  {
    VortexAction.clear();
    foreach (BrushPixelData po in pBrush.pos)
    {
      WorldTile tile = World.world.GetTile(pCenter.x + po.x, pCenter.y + po.y);
      if (tile != null)
      {
        World.world.flash_effects.flashPixel(tile, 10);
        if (!Randy.randomChance(0.8f))
        {
          WorldTile random = Randy.getRandom<WorldTile>(tile.neighbours);
          if (random != null)
            VortexAction.newTiles.Add(new VortexSwitchHelper()
            {
              tile = random,
              newType = tile.main_type,
              newTopType = tile.top_type
            });
        }
      }
    }
    foreach (VortexSwitchHelper newTile in VortexAction.newTiles)
      MapAction.terraformTile(newTile.tile, newTile.newType, newTile.newTopType, TerraformLibrary.flash);
  }

  private static void clear() => VortexAction.newTiles.Clear();
}

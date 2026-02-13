// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehWormDigEat
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace ai.behaviours;

public class BehWormDigEat : BehaviourActionActor
{
  private static List<BrushPixelData> myRange = new List<BrushPixelData>();

  public override BehResult execute(Actor pActor)
  {
    int pResult;
    pActor.data.get("worm_size", out pResult, 1);
    if (pActor.current_tile.Height < 220)
      this.loopWithBrush(pActor.current_tile, pResult, new PowerActionWithID(BehWormDigEat.tileDrawWorm));
    BehWormDigEat.checkForWorms(pActor.current_tile, pResult, pActor);
    return BehResult.Continue;
  }

  public static void checkForWorms(WorldTile pCenterTile, int pBrushSize, Actor pActor)
  {
    BrushData brushData = Brush.get(pBrushSize, "hcirc_");
    for (int index = 0; index < brushData.pos.Length; ++index)
    {
      int pX = pCenterTile.x + brushData.pos[index].x;
      int pY = pCenterTile.y + brushData.pos[index].y;
      if (pX >= 0 && pX < MapBox.width && pY >= 0 && pY < MapBox.height)
      {
        WorldTile tileSimple = BehaviourActionBase<Actor>.world.GetTileSimple(pX, pY);
        BehWormDigEat.checkWorms(tileSimple, pActor);
        BehaviourActionBase<Actor>.world.flash_effects.flashPixel(tileSimple, 10, ColorType.Purple);
      }
    }
  }

  public void loopWithBrush(
    WorldTile pCenterTile,
    int pBrushSize,
    PowerActionWithID pAction,
    string pPowerID = null)
  {
    BrushData brushData = Brush.get(pBrushSize, "hcirc_");
    for (int index = 0; index < brushData.pos.Length; ++index)
    {
      int pX = pCenterTile.x + brushData.pos[index].x;
      int pY = pCenterTile.y + brushData.pos[index].y;
      if (pX >= 0 && pX < MapBox.width && pY >= 0 && pY < MapBox.height)
      {
        WorldTile tileSimple = BehaviourActionBase<Actor>.world.GetTileSimple(pX, pY);
        int num = pAction(tileSimple, pPowerID) ? 1 : 0;
      }
    }
  }

  public static void checkWorms(WorldTile pTile, Actor pActor)
  {
    pTile.doUnits((Action<Actor>) (tActor =>
    {
      if (pActor.data.id == tActor.data.id || !(tActor.asset.id == "worm"))
        return;
      int pResult1;
      pActor.data.get("worm_size", out pResult1, 1);
      int pResult2;
      tActor.data.get("worm_size", out pResult2, 1);
      tActor.dieSimpleNone();
      pActor.data.set("worm_size", pResult1 + pResult2);
    }));
  }

  public static bool tileDrawWorm(WorldTile pTile, string pPowerID)
  {
    if (pTile == null)
      return false;
    BehWormDig.wormTile(pTile);
    if (pTile.Type.ocean && pTile.Type.liquid && Randy.randomChance(0.25f))
      BehWormDigEat.spawnBurst(pTile, "rain", false);
    if (pTile.Type.lava)
    {
      LavaHelper.removeLava(pTile);
      if (Randy.randomChance(0.25f))
        BehWormDigEat.spawnBurst(pTile, "lava");
    }
    if (pTile.isOnFire())
      pTile.stopFire();
    if (Randy.randomChance(0.25f))
    {
      if (pTile.Type.IsType("sand"))
        BehWormDigEat.spawnBurst(pTile, "pixel", false);
      else if (pTile.Type.can_be_farm)
        BehWormDigEat.spawnBurst(pTile, "pixel", false);
    }
    return true;
  }

  private static void spawnBurst(WorldTile pTile, string pType, bool pCreateGround = true)
  {
    if (BehaviourActionBase<Actor>.world.drop_manager.getActiveIndex() > 300)
      return;
    BehaviourActionBase<Actor>.world.drop_manager.spawnParabolicDrop(pTile, pType, pMinHeight: 0.62f, pMaxHeight: 104f, pMinRadius: 0.7f, pMaxRadius: 23.5f);
  }
}

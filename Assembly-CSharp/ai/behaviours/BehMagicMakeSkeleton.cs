// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehMagicMakeSkeleton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehMagicMakeSkeleton : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    int num = 0;
    foreach (Actor actor in Finder.findSpeciesAroundTileChunk(pActor.current_tile, "skeleton"))
    {
      if (num++ > 6)
        return BehResult.Stop;
    }
    WorldTile currentTile = pActor.current_tile;
    WorldTile worldTile;
    if (currentTile == null)
    {
      worldTile = (WorldTile) null;
    }
    else
    {
      MapRegion region = currentTile.region;
      worldTile = region != null ? region.tiles.GetRandom<WorldTile>() : (WorldTile) null;
    }
    WorldTile pTile = worldTile;
    if (pTile == null || pTile.hasUnits())
      return BehResult.Stop;
    pActor.doCastAnimation();
    ActionLibrary.spawnSkeleton((BaseSimObject) pActor, pTile);
    return BehResult.Continue;
  }
}

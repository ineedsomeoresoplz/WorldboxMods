// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehFindTileNearbyGroupLeader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
namespace ai.behaviours;

public class BehFindTileNearbyGroupLeader : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    if (!pActor.hasArmy() || !pActor.army.hasCaptain())
      return BehResult.Stop;
    Actor captain = pActor.army.getCaptain();
    List<WorldTile> currentPath = captain.current_path;
    WorldTile random;
    if (currentPath != null && currentPath.Count > 0)
    {
      random = currentPath[currentPath.Count - 1].region.tiles.GetRandom<WorldTile>();
    }
    else
    {
      MapRegion mapRegion = captain.current_tile.region;
      if (mapRegion.tiles.Count < 20 && mapRegion.neighbours.Count > 0)
        mapRegion = mapRegion.neighbours.GetRandom<MapRegion>();
      random = mapRegion.tiles.GetRandom<WorldTile>();
    }
    pActor.beh_tile_target = random;
    return BehResult.Continue;
  }
}

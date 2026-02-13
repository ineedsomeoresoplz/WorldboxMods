// Decompiled with JetBrains decompiler
// Type: BehCheckCuriosityTile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class BehCheckCuriosityTile : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    if (pActor.scheduled_tile_target == null)
      return BehResult.Stop;
    WorldTile scheduledTileTarget = pActor.scheduled_tile_target;
    pActor.scheduled_tile_target = (WorldTile) null;
    float pVal = 0.6f;
    if (pActor.hasSubspecies() && pActor.subspecies.has_trait_curious)
      pVal += 0.3f;
    if (!Randy.randomChance(pVal))
      return BehResult.Stop;
    WorldTile walkableTileAround = scheduledTileTarget.getWalkableTileAround(pActor.current_tile);
    if (walkableTileAround == null)
      return BehResult.Stop;
    pActor.beh_tile_target = walkableTileAround;
    return BehResult.Continue;
  }
}

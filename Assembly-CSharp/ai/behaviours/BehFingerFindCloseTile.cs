// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehFingerFindCloseTile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehFingerFindCloseTile : BehFinger
{
  public override BehResult execute(Actor pActor)
  {
    pActor.findCurrentTile(false);
    if (this.finger.target_tiles.Count == 0)
      return BehResult.Stop;
    pActor.beh_tile_target = this.finger.target_tiles.GetRandom<WorldTile>();
    if (this.finger.target_tiles.Contains(pActor.current_tile))
    {
      while (pActor.beh_tile_target.region != pActor.current_tile.region)
        pActor.beh_tile_target = this.finger.target_tiles.GetRandom<WorldTile>();
    }
    return BehResult.Continue;
  }
}

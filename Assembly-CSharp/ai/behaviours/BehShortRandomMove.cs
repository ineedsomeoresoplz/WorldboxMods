// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehShortRandomMove
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehShortRandomMove : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    foreach (WorldTile worldTile in pActor.current_tile.neighboursAll.LoopRandom<WorldTile>())
    {
      if (worldTile.Type.layer_type == pActor.current_tile.Type.layer_type)
      {
        pActor.beh_tile_target = worldTile;
        return BehResult.Continue;
      }
    }
    return BehResult.Stop;
  }
}

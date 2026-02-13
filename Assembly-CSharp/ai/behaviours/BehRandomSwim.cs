// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehRandomSwim
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehRandomSwim : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    BehaviourActionActor.possible_moves.Clear();
    foreach (WorldTile worldTile in pActor.current_tile.neighboursAll)
    {
      if (worldTile.Type.liquid)
        BehaviourActionActor.possible_moves.Add(worldTile);
    }
    if (BehaviourActionActor.possible_moves.Count > 0)
    {
      WorldTile random = BehaviourActionActor.possible_moves.GetRandom<WorldTile>();
      BehaviourActionActor.possible_moves.Clear();
      pActor.moveTo(random);
      pActor.setTileTarget(random);
    }
    return BehResult.Continue;
  }
}

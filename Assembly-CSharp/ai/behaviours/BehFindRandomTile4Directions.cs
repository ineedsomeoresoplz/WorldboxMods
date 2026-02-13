// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehFindRandomTile4Directions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehFindRandomTile4Directions : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    int pResult;
    pActor.data.get("direction", out pResult, -1);
    ActorDirection actorDirection;
    if (pResult == -1)
    {
      actorDirection = Randy.getRandom<ActorDirection>(Toolbox.directions);
      int pData = Toolbox.directions.IndexOf<ActorDirection>(actorDirection);
      pActor.data.set("direction", pData);
    }
    else
      actorDirection = Toolbox.directions[pResult];
    pActor.beh_tile_target = Ant.getNextTile(pActor.current_tile, actorDirection);
    if (pActor.beh_tile_target == null)
      pActor.beh_tile_target = Ant.randomNeighbour(pActor.current_tile);
    return BehResult.Continue;
  }
}

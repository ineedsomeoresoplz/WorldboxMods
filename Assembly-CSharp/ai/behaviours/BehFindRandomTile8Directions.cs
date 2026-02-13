// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehFindRandomTile8Directions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehFindRandomTile8Directions : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    int pResult1;
    pActor.data.get("random_steps", out pResult1);
    int pResult2;
    pActor.data.get("direction", out pResult2, -1);
    ActorDirection random;
    if (pResult1 > 0)
    {
      if (pActor.beh_tile_target != null && pActor.current_tile != pActor.beh_tile_target)
        return BehResult.Continue;
      random = Toolbox.directions_all[pResult2];
    }
    else
    {
      pResult1 = Randy.randomInt(Randy.randomInt(1, 6), Randy.randomInt(10, 60));
      int pData;
      if (pResult2 < 0)
      {
        random = Randy.getRandom<ActorDirection>(Toolbox.directions_all);
        pData = Toolbox.directions_all.IndexOf<ActorDirection>(random);
      }
      else
      {
        ActorDirection key = Toolbox.directions_all[pResult2];
        random = Randy.getRandom<ActorDirection>(Toolbox.directions_all_turns[key]);
        pData = Toolbox.directions_all.IndexOf<ActorDirection>(random);
      }
      pActor.data.set("direction", pData);
    }
    int pData1 = pResult1 - 1;
    pActor.beh_tile_target = Ant.getNextTile(pActor.current_tile, random);
    if (pActor.beh_tile_target == null)
    {
      pActor.data.set("random_steps", 0);
      pActor.data.set("direction", -1);
      return BehResult.RepeatStep;
    }
    pActor.data.set("random_steps", pData1);
    return BehResult.Continue;
  }
}

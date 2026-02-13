// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehCheckCure
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehCheckCure : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    if (!pActor.current_tile.Type.ground)
      return BehResult.Stop;
    Actor pTarget = (Actor) null;
    foreach (Actor pActor1 in Finder.getUnitsFromChunk(pActor.current_tile, 1))
    {
      if (ActorTool.canBeCuredFromTraitsOrStatus(pActor1))
      {
        pTarget = pActor1;
        break;
      }
    }
    if (pTarget == null)
      return BehResult.Stop;
    AttackAction action = AssetManager.spells.get("cast_cure").action;
    if (action != null)
    {
      int num = action((BaseSimObject) pActor, (BaseSimObject) pTarget, pTarget.current_tile) ? 1 : 0;
    }
    pActor.doCastAnimation();
    return BehResult.Continue;
  }
}

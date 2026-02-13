// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehFamilyFollowAlpha
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehFamilyFollowAlpha : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    if (!pActor.hasFamily() || pActor.family.isAlpha(pActor))
      return BehResult.Stop;
    pActor.family.checkAlpha();
    Actor pBaseSimObject = pActor.family.getAlpha();
    if (pActor.family.isAlpha(pActor))
      return BehResult.Stop;
    if (pBaseSimObject != null && !pBaseSimObject.current_tile.isSameIsland(pActor.current_tile))
      pBaseSimObject = (Actor) null;
    if (pBaseSimObject == null || (double) pActor.distanceToObjectTarget((BaseSimObject) pBaseSimObject) > 400.0)
      return BehResult.Stop;
    pActor.beh_actor_target = (BaseSimObject) pBaseSimObject;
    return BehResult.Continue;
  }
}

// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehHeal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehHeal : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    if (pActor.hasMaxHealth())
      return BehResult.Stop;
    AttackAction action = AssetManager.spells.get("cast_blood_rain").action;
    if (action != null)
    {
      int num = action((BaseSimObject) pActor, (BaseSimObject) pActor, pActor.current_tile) ? 1 : 0;
    }
    pActor.doCastAnimation();
    return BehResult.Continue;
  }
}

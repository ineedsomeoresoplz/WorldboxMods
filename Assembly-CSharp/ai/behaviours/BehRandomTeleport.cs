// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehRandomTeleport
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehRandomTeleport : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    if (!pActor.hasMaxHealth() || !Randy.randomChance(0.3f))
      return BehResult.Stop;
    SpellAsset spellAsset = AssetManager.spells.get("teleport");
    bool flag = false;
    if (spellAsset.action != null)
      flag = spellAsset.action.RunAnyTrue((BaseSimObject) pActor, (BaseSimObject) pActor, pActor.current_tile);
    if (!flag)
      return BehResult.Stop;
    pActor.doCastAnimation();
    return BehResult.Continue;
  }
}

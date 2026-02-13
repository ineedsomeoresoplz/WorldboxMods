// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehActorSetFlip
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehActorSetFlip : BehaviourActionActor
{
  private bool flip;

  public BehActorSetFlip(bool pFlip) => this.flip = pFlip;

  public override BehResult execute(Actor pActor)
  {
    pActor.setFlip(this.flip);
    return BehResult.Continue;
  }
}

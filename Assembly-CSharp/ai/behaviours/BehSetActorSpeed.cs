// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehSetActorSpeed
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehSetActorSpeed : BehaviourActionActor
{
  private float speed;

  public BehSetActorSpeed(float pSpeed = 0.0f) => this.speed = pSpeed;

  public override BehResult execute(Actor pActor)
  {
    pActor.stats["speed"] = this.speed;
    return BehResult.Continue;
  }
}

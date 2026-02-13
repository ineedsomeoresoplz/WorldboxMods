// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehBoat
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehBoat : BehaviourActionActor
{
  internal Boat boat;

  internal void checkHomeDocks(Actor pActor) => ActorTool.checkHomeDocks(pActor);

  public override void prepare(Actor pActor)
  {
    base.prepare(pActor);
    this.boat = pActor.getSimpleComponent<Boat>();
  }

  public override BehResult execute(Actor pActor) => BehResult.Continue;
}

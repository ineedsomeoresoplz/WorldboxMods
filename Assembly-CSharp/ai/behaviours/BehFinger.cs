// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehFinger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehFinger : BehaviourActionActor
{
  protected GodFinger finger;
  public bool drawing_action;

  public override void prepare(Actor pActor)
  {
    this.finger = pActor.children_special[0] as GodFinger;
    base.prepare(pActor);
  }
}

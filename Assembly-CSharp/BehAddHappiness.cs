// Decompiled with JetBrains decompiler
// Type: BehAddHappiness
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class BehAddHappiness : BehaviourActionActor
{
  private string _happiness_id;

  public BehAddHappiness(string pHappinessID) => this._happiness_id = pHappinessID;

  public override BehResult execute(Actor pActor)
  {
    pActor.a.changeHappiness(this._happiness_id);
    return BehResult.Continue;
  }
}

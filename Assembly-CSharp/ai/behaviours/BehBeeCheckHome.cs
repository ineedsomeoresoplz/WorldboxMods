// Decompiled with JetBrains decompiler
// Type: ai.behaviours.BehBeeCheckHome
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
namespace ai.behaviours;

public class BehBeeCheckHome : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    return pActor.asset.id != "bee" || pActor.getHomeBuilding() != null ? BehResult.Continue : BehResult.Stop;
  }
}

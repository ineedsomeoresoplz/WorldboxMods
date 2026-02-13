// Decompiled with JetBrains decompiler
// Type: BehDecideWhereToSleep
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class BehDecideWhereToSleep : BehaviourActionActor
{
  public override BehResult execute(Actor pObject)
  {
    return pObject.hasHouseCityInBordersAndSameIsland() ? this.forceTask(pObject, "sleep_inside", false, true) : this.forceTask(pObject, "sleep_outside", false, true);
  }
}

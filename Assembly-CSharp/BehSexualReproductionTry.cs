// Decompiled with JetBrains decompiler
// Type: BehSexualReproductionTry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class BehSexualReproductionTry : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    int num = (int) base.execute(pActor);
    bool flag = pActor.hasHouseCityInBordersAndSameIsland();
    pActor.subspecies.counter_reproduction_sexual_try?.registerEvent();
    return flag ? this.forceTask(pActor, "sexual_reproduction_inside", false, true) : this.forceTask(pActor, "sexual_reproduction_check_outside", false, true);
  }
}

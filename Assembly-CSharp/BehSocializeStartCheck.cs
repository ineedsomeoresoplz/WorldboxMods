// Decompiled with JetBrains decompiler
// Type: BehSocializeStartCheck
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class BehSocializeStartCheck : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    int num = (int) base.execute(pActor);
    if (pActor.hasTelepathicLink())
      return this.forceTask(pActor, "socialize_try_to_start_immediate", false);
    return pActor.hasCity() && pActor.city.hasBuildingType("type_bonfire", pLimitIsland: pActor.current_island) ? this.forceTask(pActor, "socialize_try_to_start_near_bonfire", false) : this.forceTask(pActor, "socialize_try_to_start_immediate", false);
  }
}

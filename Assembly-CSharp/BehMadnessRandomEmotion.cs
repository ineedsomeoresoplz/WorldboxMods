// Decompiled with JetBrains decompiler
// Type: BehMadnessRandomEmotion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class BehMadnessRandomEmotion : BehaviourActionActor
{
  private const int STATUS_DURATION = 10;

  public override BehResult execute(Actor pActor)
  {
    if (Randy.randomBool())
    {
      using (ListPool<string> list = new ListPool<string>())
      {
        list.Add("laughing");
        list.Add("crying");
        list.Add("swearing");
        string random = list.GetRandom<string>();
        pActor.addStatusEffect(random, 10f, false);
        return BehResult.Continue;
      }
    }
    using (ListPool<string> list = new ListPool<string>())
    {
      list.Add("happy_laughing");
      list.Add("crying");
      list.Add("swearing");
      if (list.Count == 0)
        return BehResult.Stop;
      string random = list.GetRandom<string>();
      return this.forceTask(pActor, random, false, true);
    }
  }
}

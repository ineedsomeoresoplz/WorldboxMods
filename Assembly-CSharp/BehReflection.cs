// Decompiled with JetBrains decompiler
// Type: BehReflection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;

#nullable disable
public class BehReflection : BehaviourActionActor
{
  public override BehResult execute(Actor pActor)
  {
    int happiness = pActor.getHappiness();
    bool flag1 = happiness > 50;
    bool flag2 = happiness < -70;
    using (ListPool<string> listPool = new ListPool<string>())
    {
      if (pActor.subspecies.hasTrait("super_positivity"))
      {
        this.fillFromSuperPositivity(pActor, listPool);
      }
      else
      {
        this.fillFromHappinessHistory(pActor, listPool);
        if (pActor.hasTrait("hotheaded"))
        {
          listPool.Add("swearing");
          listPool.Add("swearing");
          listPool.Add("swearing");
          listPool.Add("swearing");
        }
        if (pActor.subspecies.hasTrait("aggressive") && happiness < 0)
          listPool.Add("start_tantrum");
        if (pActor.hasTrait("hotheaded") && happiness < 0)
          listPool.Add("start_tantrum");
        if (flag1)
        {
          listPool.Add("happy_laughing");
          listPool.Add("happy_laughing");
          listPool.Add("happy_laughing");
          listPool.Add("singing");
          if (pActor.hasLanguage() && pActor.language.hasTrait("melodic"))
          {
            listPool.Add("singing");
            listPool.Add("singing");
            listPool.Add("singing");
            listPool.Add("singing");
          }
          if (pActor.isBaby())
          {
            listPool.Add("child_random_flips");
            listPool.Add("child_random_flips");
            listPool.Add("child_play_at_one_spot");
            listPool.Add("child_play_at_one_spot");
            listPool.Add("child_random_jump");
            listPool.Add("child_random_jump");
          }
          else
            listPool.Add("wait5");
        }
        else if (flag2)
        {
          if (!pActor.hasTag("strong_mind"))
          {
            listPool.Add("crying");
            listPool.Add("crying");
            if (happiness <= -100)
            {
              listPool.Add("crying");
              listPool.Add("crying");
              listPool.Add("crying");
              listPool.Add("start_tantrum");
            }
            listPool.Add("start_tantrum");
          }
          listPool.Add("swearing");
          listPool.Add("swearing");
          listPool.Add("punch_a_tree");
          listPool.Add("punch_a_tree");
          listPool.Add("punch_a_building");
          listPool.Add("punch_a_building");
          listPool.Add("wait5");
          if (pActor.hasTrait("pyromaniac"))
            listPool.Add("start_fire");
        }
        else
          listPool.Add("wait5");
      }
      if (listPool.Count == 0)
        return BehResult.Stop;
      string random = listPool.GetRandom<string>();
      return this.forceTask(pActor, random, false, true);
    }
  }

  private void fillFromHappinessHistory(Actor pActor, ListPool<string> pPot)
  {
    if (!pActor.hasHappinessHistory())
      return;
    foreach (HappinessHistory happinessHistory in pActor.happiness_change_history)
    {
      HappinessAsset asset = happinessHistory.asset;
      if (asset.pot_task_id != null)
      {
        for (int index = 0; index < asset.pot_amount; ++index)
          pPot.Add(asset.pot_task_id);
      }
    }
  }

  private void fillFromSuperPositivity(Actor pActor, ListPool<string> pPot)
  {
    pPot.Add("happy_laughing");
    pPot.Add("happy_laughing");
    pPot.Add("happy_laughing");
    pPot.Add("singing");
    pPot.Add("child_random_flips");
    pPot.Add("child_play_at_one_spot");
    pPot.Add("child_random_jump");
  }
}

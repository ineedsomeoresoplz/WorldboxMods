// Decompiled with JetBrains decompiler
// Type: RewardTester
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
internal sealed class RewardTester
{
  internal bool haveRew(string pPowID)
  {
    RewardedPower rewardedPower1 = (RewardedPower) null;
    foreach (RewardedPower rewardedPower2 in PlayerConfig.instance.data.rewardedPowers)
    {
      if (rewardedPower2.name == pPowID)
      {
        rewardedPower1 = rewardedPower2;
        break;
      }
    }
    return rewardedPower1 != null;
  }

  internal bool checkRew()
  {
    if (PlayerConfig.instance.data.rewardedPowers.Count == 0)
      return false;
    double num1 = Epoch.Current();
    int num2 = 1860;
    bool flag1 = false;
    int index = 0;
    while (index < PlayerConfig.instance.data.rewardedPowers.Count)
    {
      RewardedPower rewardedPower = PlayerConfig.instance.data.rewardedPowers[index];
      bool flag2 = false;
      if (rewardedPower.timeStamp > num1)
        flag2 = true;
      if (num1 - rewardedPower.timeStamp > (double) num2)
        flag2 = true;
      if (flag2)
      {
        PlayerConfig.instance.data.rewardedPowers.RemoveAt(index);
        flag1 = true;
      }
      else
        ++index;
    }
    if (!flag1)
      return false;
    PlayerConfig.saveData();
    return true;
  }
}

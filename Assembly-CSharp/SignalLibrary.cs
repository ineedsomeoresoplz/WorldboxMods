// Decompiled with JetBrains decompiler
// Type: SignalLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class SignalLibrary : AssetLibrary<SignalAsset>
{
  public override void post_init()
  {
    base.post_init();
    foreach (Achievement achievement in AssetManager.achievements.list)
    {
      Achievement tAchievement = achievement;
      if (!tAchievement.has_signal)
      {
        SignalAsset pAsset = new SignalAsset();
        pAsset.id = tAchievement.id + "_signal";
        pAsset.action_achievement = new AchievementCheck(tAchievement.check);
        pAsset.ban_check_action = (SignalBanCheckAction) (_ => tAchievement.isUnlocked());
        SignalAsset pSignal = this.add(pAsset);
        tAchievement.setSignal(pSignal);
      }
    }
    foreach (SignalAsset signalAsset in this.list)
    {
      if (signalAsset.action != null)
        signalAsset.has_action = true;
      if (signalAsset.action_achievement != null)
        signalAsset.has_action_achievement = true;
      if (signalAsset.ban_check_action != null)
      {
        signalAsset.has_ban_check_action = true;
        if (signalAsset.ban_check_action())
          signalAsset.ban();
      }
    }
  }
}

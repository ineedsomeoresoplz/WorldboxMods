// Decompiled with JetBrains decompiler
// Type: MoodLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class MoodLibrary : AssetLibrary<MoodAsset>
{
  public override void init()
  {
    base.init();
    MoodAsset pAsset1 = new MoodAsset();
    pAsset1.id = "sad";
    pAsset1.icon = "iconMoodSad";
    this.add(pAsset1);
    this.t.base_stats["multiplier_speed"] = -0.2f;
    this.t.base_stats["multiplier_diplomacy"] = -0.1f;
    this.t.base_stats["loyalty_mood"] = -5f;
    this.t.base_stats["opinion"] = -5f;
    MoodAsset pAsset2 = new MoodAsset();
    pAsset2.id = "normal";
    pAsset2.icon = "iconMoodNormal";
    this.add(pAsset2);
    MoodAsset pAsset3 = new MoodAsset();
    pAsset3.id = "happy";
    pAsset3.icon = "iconMoodHappy";
    this.add(pAsset3);
    this.t.base_stats["multiplier_speed"] = 0.1f;
    this.t.base_stats["multiplier_diplomacy"] = 0.1f;
    this.t.base_stats["loyalty_mood"] = 10f;
    this.t.base_stats["opinion"] = 10f;
    MoodAsset pAsset4 = new MoodAsset();
    pAsset4.id = "angry";
    pAsset4.icon = "iconMoodAngry";
    this.add(pAsset4);
    this.t.base_stats["multiplier_speed"] = 0.1f;
    this.t.base_stats["multiplier_diplomacy"] = -0.3f;
    this.t.base_stats["loyalty_mood"] = -15f;
    this.t.base_stats["opinion"] = -15f;
    MoodAsset pAsset5 = new MoodAsset();
    pAsset5.id = "dark";
    this.add(pAsset5);
    this.t.base_stats["loyalty_mood"] = -20f;
    this.t.base_stats["opinion"] = -20f;
  }
}

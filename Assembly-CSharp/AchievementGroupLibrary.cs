// Decompiled with JetBrains decompiler
// Type: AchievementGroupLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Beebyte.Obfuscator;
using System;

#nullable disable
[ObfuscateLiterals]
[Serializable]
public class AchievementGroupLibrary : AssetLibrary<AchievementGroupAsset>
{
  public override void init()
  {
    base.init();
    AchievementGroupAsset pAsset1 = new AchievementGroupAsset();
    pAsset1.id = "creation";
    pAsset1.color = "#68B3FF";
    this.add(pAsset1);
    AchievementGroupAsset pAsset2 = new AchievementGroupAsset();
    pAsset2.id = "worlds";
    pAsset2.color = "#BAFFC2";
    this.add(pAsset2);
    AchievementGroupAsset pAsset3 = new AchievementGroupAsset();
    pAsset3.id = "civilizations";
    pAsset3.color = "#BAF0F4";
    this.add(pAsset3);
    AchievementGroupAsset pAsset4 = new AchievementGroupAsset();
    pAsset4.id = "creatures";
    pAsset4.color = "#42FF61";
    this.add(pAsset4);
    AchievementGroupAsset pAsset5 = new AchievementGroupAsset();
    pAsset5.id = "destruction";
    pAsset5.color = "#FF6B86";
    this.add(pAsset5);
    AchievementGroupAsset pAsset6 = new AchievementGroupAsset();
    pAsset6.id = "nature";
    pAsset6.color = "#BAFFC2";
    this.add(pAsset6);
    AchievementGroupAsset pAsset7 = new AchievementGroupAsset();
    pAsset7.id = "experiments";
    pAsset7.color = "#FF8F44";
    this.add(pAsset7);
    AchievementGroupAsset pAsset8 = new AchievementGroupAsset();
    pAsset8.id = "collection";
    pAsset8.color = "#46DCE3";
    this.add(pAsset8);
    AchievementGroupAsset pAsset9 = new AchievementGroupAsset();
    pAsset9.id = "exploration";
    pAsset9.color = "#EFCB00";
    this.add(pAsset9);
    AchievementGroupAsset pAsset10 = new AchievementGroupAsset();
    pAsset10.id = "forbidden";
    pAsset10.color = "#C98CFF";
    this.add(pAsset10);
    AchievementGroupAsset pAsset11 = new AchievementGroupAsset();
    pAsset11.id = "miscellaneous";
    pAsset11.color = "#B4C4C0";
    this.add(pAsset11);
  }

  public override void linkAssets()
  {
    foreach (Achievement achievement in AssetManager.achievements.list)
      this.dict[achievement.group].achievements_list.Add(achievement);
  }

  public override void editorDiagnosticLocales()
  {
    base.editorDiagnosticLocales();
    foreach (AchievementGroupAsset pAsset in this.list)
      this.checkLocale((Asset) pAsset, pAsset.getLocaleID());
  }
}

// Decompiled with JetBrains decompiler
// Type: AchievementWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class AchievementWindow : MonoBehaviour
{
  public AchievementGroup achievementGroupPrefab;
  private List<AchievementGroup> _elements = new List<AchievementGroup>();
  public Transform transformContent;
  public StatBar achievementBar;

  private void OnEnable() => this.showList();

  internal void showList()
  {
    if (!Config.game_loaded)
      return;
    for (int index = 0; index < this._elements.Count; ++index)
      Object.Destroy((Object) ((Component) this._elements[index]).gameObject);
    this._elements.Clear();
    foreach (AchievementGroupAsset pAchievementGroup in AssetManager.achievement_groups.list)
      this.showElement(pAchievementGroup);
    this.updateTotalBar();
  }

  private void updateTotalBar()
  {
    int count = AssetManager.achievements.list.Count;
    this.achievementBar.setBar((float) AchievementLibrary.countUnlocked(), (float) count, "/" + count.ToText());
  }

  private void showElement(AchievementGroupAsset pAchievementGroup)
  {
    AchievementGroup achievementGroup = Object.Instantiate<AchievementGroup>(this.achievementGroupPrefab, this.transformContent);
    achievementGroup.showGroup(pAchievementGroup);
    this._elements.Add(achievementGroup);
  }
}

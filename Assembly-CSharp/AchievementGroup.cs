// Decompiled with JetBrains decompiler
// Type: AchievementGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class AchievementGroup : MonoBehaviour
{
  public AchievementButton achievementButtonPrefab;
  private List<AchievementButton> _elements = new List<AchievementButton>();
  public Text title;
  public Text counter;
  public Transform transformContent;

  public void showGroup(AchievementGroupAsset pAchievementGroup)
  {
    ((Component) this.title).GetComponent<LocalizedText>().setKeyAndUpdate(pAchievementGroup.getLocaleID());
    ((Graphic) this.title).color = pAchievementGroup.getColor();
    if (pAchievementGroup.achievements_list.Count <= 0)
      return;
    int num = 0;
    foreach (Achievement achievements in pAchievementGroup.achievements_list)
    {
      AchievementButton achievementButton = Object.Instantiate<AchievementButton>(this.achievementButtonPrefab, this.transformContent);
      achievementButton.Load(achievements);
      if (AchievementLibrary.isUnlocked(achievements))
        ++num;
      this._elements.Add(achievementButton);
    }
    this.counter.text = $"{num.ToString()} / {pAchievementGroup.achievements_list.Count.ToString()}";
  }
}

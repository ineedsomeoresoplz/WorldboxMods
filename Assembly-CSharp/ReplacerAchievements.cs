// Decompiled with JetBrains decompiler
// Type: ReplacerAchievements
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ReplacerAchievements : MonoBehaviour
{
  [SerializeField]
  private Image _target_icon;
  public Sprite icon_gold;
  public Sprite icon_silver;
  private BuildingAsset _asset;

  private void OnEnable()
  {
    if (!Config.game_loaded || SmoothLoader.isLoading())
      return;
    this.checkIcon();
  }

  private void Start() => this.checkIcon();

  private void checkIcon()
  {
    if (AchievementLibrary.isAllUnlocked())
      this._target_icon.sprite = this.icon_gold;
    else
      this._target_icon.sprite = this.icon_silver;
  }
}

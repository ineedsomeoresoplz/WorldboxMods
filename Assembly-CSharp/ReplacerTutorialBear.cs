// Decompiled with JetBrains decompiler
// Type: ReplacerTutorialBear
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ReplacerTutorialBear : MonoBehaviour
{
  [SerializeField]
  private Image _target_icon;
  public Sprite icon_animal;
  public Sprite icon_civ;
  private BuildingAsset _asset;

  private void OnEnable()
  {
    if (!Config.game_loaded || SmoothLoader.isLoading())
      return;
    if (this._asset == null)
      this._asset = AssetManager.buildings.get("monolith");
    if (this._asset.buildings.Count > 0)
      this._target_icon.sprite = this.icon_civ;
    else
      this._target_icon.sprite = this.icon_animal;
  }
}

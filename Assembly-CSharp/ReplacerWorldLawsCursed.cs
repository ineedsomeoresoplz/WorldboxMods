// Decompiled with JetBrains decompiler
// Type: ReplacerWorldLawsCursed
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ReplacerWorldLawsCursed : MonoBehaviour
{
  [SerializeField]
  private Image _target_icon;
  public Sprite icon_normal;
  public Sprite icon_world_cursed;

  private void OnEnable()
  {
    if (!Config.game_loaded)
      return;
    if (WorldLawLibrary.world_law_cursed_world.isEnabled())
      this._target_icon.sprite = this.icon_world_cursed;
    else
      this._target_icon.sprite = this.icon_normal;
  }
}

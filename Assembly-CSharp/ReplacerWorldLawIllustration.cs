// Decompiled with JetBrains decompiler
// Type: ReplacerWorldLawIllustration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ReplacerWorldLawIllustration : MonoBehaviour
{
  private Image _target_image;
  public Sprite image_normal;
  public Sprite image_cursed;

  private void Awake() => this._target_image = ((Component) this).GetComponent<Image>();

  private void OnEnable()
  {
    if (!Config.game_loaded)
      return;
    if (WorldLawLibrary.world_law_cursed_world.isEnabled())
      this._target_image.sprite = this.image_cursed;
    else
      this._target_image.sprite = this.image_normal;
  }
}

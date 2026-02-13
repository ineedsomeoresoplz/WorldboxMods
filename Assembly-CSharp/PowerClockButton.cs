// Decompiled with JetBrains decompiler
// Type: PowerClockButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class PowerClockButton : MonoBehaviour
{
  public Image currentSpeedIcon;
  private string _latest_used = string.Empty;

  private void Update()
  {
    if (Config.time_scale_asset == null || !(Config.time_scale_asset.id != this._latest_used))
      return;
    this._latest_used = Config.time_scale_asset.id;
    this.currentSpeedIcon.sprite = SpriteTextureLoader.getSprite(Config.time_scale_asset.path_icon);
  }
}

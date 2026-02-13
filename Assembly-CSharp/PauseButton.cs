// Decompiled with JetBrains decompiler
// Type: PauseButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class PauseButton : MonoBehaviour
{
  public Sprite pause;
  public Sprite play;
  private Image icon;

  private void Start()
  {
    this.icon = ((Component) ((Component) this).transform.Find("Icon")).GetComponent<Image>();
  }

  private void Update() => this.updateSprite();

  internal void press()
  {
    Config.paused = !Config.paused;
    if (Config.paused)
      WorldTip.instance.setText("game_paused");
    else
      WorldTip.instance.setText("game_unpaused");
  }

  private void updateSprite()
  {
    if (Config.paused)
      this.icon.sprite = this.play;
    else
      this.icon.sprite = this.pause;
  }
}

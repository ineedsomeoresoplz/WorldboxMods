// Decompiled with JetBrains decompiler
// Type: BuildingDebugAnimationVariation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine.UI;

#nullable disable
public class BuildingDebugAnimationVariation : DebugAnimationVariation
{
  public Image shadow;
  public SpriteAnimation shadow_animation;

  public void update(float pElapsed)
  {
    this.sprite_animation.update(pElapsed);
    this.shadow_animation.update(pElapsed);
  }

  public void toggleAnimation(bool pState)
  {
    if (pState)
    {
      this.sprite_animation.isOn = true;
      this.shadow_animation.isOn = true;
    }
    else
    {
      this.sprite_animation.stopAnimations();
      this.shadow_animation.stopAnimations();
    }
  }

  public void setFrame(int pIndex)
  {
    this.sprite_animation.currentFrameIndex = pIndex;
    this.sprite_animation.updateFrame();
    this.shadow_animation.currentFrameIndex = pIndex;
    this.shadow_animation.updateFrame();
  }
}

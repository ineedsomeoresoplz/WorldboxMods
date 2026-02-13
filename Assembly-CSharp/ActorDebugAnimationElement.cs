// Decompiled with JetBrains decompiler
// Type: ActorDebugAnimationElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ActorDebugAnimationElement : BaseDebugAnimationElement<ActorAsset>
{
  public SpriteAnimation adult;
  public SpriteAnimation baby;

  protected override void Start()
  {
    base.Start();
    this.adult.create();
    this.baby.create();
  }

  public override void update()
  {
    if (!this.is_playing)
      return;
    this.adult.update(Time.deltaTime);
    if (this.asset.has_baby_form)
      this.baby.update(Time.deltaTime);
    this.frame_number_text.text = this.adult.currentFrameIndex.ToString();
  }

  public override void setData(ActorAsset pAsset)
  {
    base.setData(pAsset);
    if (this.asset.has_baby_form)
      return;
    ((Behaviour) this.baby).enabled = false;
    this.baby.image.sprite = (Sprite) null;
    ((Graphic) this.baby.image).color = Color.clear;
  }

  protected override void clear()
  {
    ((Behaviour) this.adult).enabled = true;
    ((Graphic) this.adult.image).color = Color.white;
    this.adult.frames = Array.Empty<Sprite>();
    this.adult.resetAnim();
    ((Behaviour) this.baby).enabled = true;
    ((Graphic) this.baby.image).color = Color.white;
    this.baby.frames = Array.Empty<Sprite>();
    this.baby.resetAnim();
  }

  public override void stopAnimations()
  {
    base.stopAnimations();
    this.adult.isOn = false;
    this.baby.isOn = false;
    this.frame_number_text.text = this.adult.currentFrameIndex.ToString();
  }

  public override void startAnimations()
  {
    base.startAnimations();
    this.adult.isOn = true;
    this.baby.isOn = true;
  }

  protected override void clickNextFrame()
  {
    if (this.is_playing)
      return;
    int length = this.adult.frames.Length;
    ++this.adult.currentFrameIndex;
    ++this.baby.currentFrameIndex;
    if (this.adult.currentFrameIndex > length - 1)
    {
      this.adult.currentFrameIndex = 0;
      this.baby.currentFrameIndex = 0;
    }
    this.frame_number_text.text = this.adult.currentFrameIndex.ToString();
    this.adult.updateFrame();
    this.baby.updateFrame();
  }
}

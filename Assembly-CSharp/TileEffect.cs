// Decompiled with JetBrains decompiler
// Type: TileEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class TileEffect : BaseEffect
{
  public void load(TileEffectAsset pAsset)
  {
    this.sprite_animation.setFrames(pAsset.getSprites());
    this.sprite_animation.resetAnim();
    this.sprite_animation.timeBetweenFrames = pAsset.time_between_frames;
  }
}

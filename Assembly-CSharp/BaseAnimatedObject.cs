// Decompiled with JetBrains decompiler
// Type: BaseAnimatedObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BaseAnimatedObject : BaseMapObject
{
  internal SpriteAnimation sprite_animation;
  private bool _has_sprite_animation;

  public virtual void Awake()
  {
    this.sprite_animation = ((Component) this).gameObject.GetComponent<SpriteAnimation>();
    this._has_sprite_animation = Object.op_Inequality((Object) this.sprite_animation, (Object) null);
  }

  internal override void create()
  {
    base.create();
    if (!this._has_sprite_animation)
      return;
    this.sprite_animation.create();
  }

  public override void update(float pElapsed)
  {
    base.update(pElapsed);
    this.updateSpriteAnimation(pElapsed);
  }

  internal void resetAnim()
  {
    if (!this._has_sprite_animation)
      return;
    this.sprite_animation.resetAnim();
  }

  internal void updateSpriteAnimation(float pElapsed, bool pForce = false)
  {
    if (!this._has_sprite_animation)
      return;
    this.sprite_animation.update(pElapsed);
  }

  public override void Dispose()
  {
    this.sprite_animation = (SpriteAnimation) null;
    base.Dispose();
  }
}

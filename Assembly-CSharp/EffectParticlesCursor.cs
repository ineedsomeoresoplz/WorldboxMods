// Decompiled with JetBrains decompiler
// Type: EffectParticlesCursor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class EffectParticlesCursor : MonoBehaviour
{
  private SpriteAnimationSimple _sprite_animation;
  private float _speed = 50f;

  private void Awake()
  {
    this._sprite_animation = ((Component) this).GetComponent<SpriteAnimationSimple>();
  }

  public void launch()
  {
    this._sprite_animation.resetAnim();
    this._speed = 50f + Randy.randomFloat(-10f, 10f);
  }

  public void update()
  {
    this._sprite_animation.update(Time.deltaTime);
    Transform transform = ((Component) this).transform;
    transform.position = Vector3.op_Addition(transform.position, new Vector3(0.0f, this._speed * Time.deltaTime, 0.0f));
  }

  public SpriteAnimationSimple getAnimation() => this._sprite_animation;

  public void setFrames(Sprite[] pFrames) => this._sprite_animation.setFrames(pFrames);
}

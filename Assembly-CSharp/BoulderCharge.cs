// Decompiled with JetBrains decompiler
// Type: BoulderCharge
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BoulderCharge : BaseEffect
{
  private const float BASE_ALPHA = 1f;
  private const float ALPHA_CHANGE = 0.001f;
  private const float RANDOM_OFFSET = 20f;
  private const float BASE_TIME_BETWEEN_FRAMES = 0.2f;
  [SerializeField]
  private List<SpriteSet> _sprite_sets;
  private Vector2 _direction;

  internal override void prepare(Vector2 pVector, float pScale = 1f)
  {
    base.prepare(pVector, pScale);
    this._direction = Boulder.chargeVector();
    this._direction.x += Randy.randomFloat(-20f, 20f);
    this._direction.y += Randy.randomFloat(-20f, 20f);
    this.setAlpha(1f);
    this.sprite_animation.setFrames(this._sprite_sets.GetRandom<SpriteSet>().sprites);
    this.sprite_animation.timeBetweenFrames = 0.2f / ((Vector2) ref this._direction).magnitude;
  }

  public override void update(float pElapsed)
  {
    base.update(pElapsed);
    Transform transform = ((Component) this).transform;
    transform.position = Vector3.op_Addition(transform.position, new Vector3((this._direction.x + Randy.randomFloat(-20f, 20f)) * Time.deltaTime, this._direction.y * Time.deltaTime, 0.0f));
    this.setAlpha(this.alpha - 1f / 1000f);
  }
}

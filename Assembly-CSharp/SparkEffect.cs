// Decompiled with JetBrains decompiler
// Type: SparkEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SparkEffect : BaseEffect
{
  private const float BASE_ALPHA = 1f;
  private const float BASE_SPEED = 10f;
  private const float RANDOM_OFFSET = 5f;
  [SerializeField]
  private List<SpriteSet> _sprite_sets;
  private float _speed = 10f;

  internal override void prepare(Vector2 pVector, float pScale = 1f)
  {
    base.prepare(pVector, pScale);
    this.setAlpha(1f);
    this.sprite_animation.setFrames(this._sprite_sets.GetRandom<SpriteSet>().sprites);
    this._speed = 10f + Randy.randomFloat(-5f, 5f);
  }

  public override void update(float pElapsed)
  {
    base.update(pElapsed);
    Transform transform = ((Component) this).transform;
    transform.position = Vector3.op_Addition(transform.position, new Vector3(0.0f, this._speed * Time.deltaTime, 0.0f));
  }
}

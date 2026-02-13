// Decompiled with JetBrains decompiler
// Type: ExplosionFlash
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ExplosionFlash : BaseEffect
{
  private float speed;

  public void start(Vector3 pVector, float pRadius, float pSpeed = 1f)
  {
    this.speed = pSpeed;
    ((Component) this).transform.position = new Vector3(pVector.x, pVector.y);
    this.setScale(0.005f * pRadius);
    this.setAlpha(1f);
  }

  public override void update(float pElapsed)
  {
    this.setAlpha(this.alpha - (float) ((double) pElapsed * (double) this.speed * 0.5));
    this.setScale(this.scale += (float) ((double) pElapsed * (double) this.speed * 0.10000000149011612));
    if ((double) this.alpha > 0.0)
      return;
    this.kill();
  }
}

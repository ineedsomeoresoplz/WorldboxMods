// Decompiled with JetBrains decompiler
// Type: StatusParticle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class StatusParticle : BaseEffect
{
  public void spawnParticle(Vector3 pVector, Color pColor, float pScale = 0.25f)
  {
    this.prepare(Vector2.op_Implicit(pVector), pScale);
    ((Component) this).GetComponent<SpriteRenderer>().color = pColor;
  }

  public override void update(float pElapsed)
  {
    base.update(pElapsed);
    this.setScale(this.scale - pElapsed * 0.2f);
    if ((double) this.scale > 0.0)
      return;
    this.kill();
  }
}

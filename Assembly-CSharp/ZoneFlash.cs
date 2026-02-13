// Decompiled with JetBrains decompiler
// Type: ZoneFlash
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ZoneFlash : BaseEffect
{
  public void start(Color pColor, float pAlpha = 0.2f)
  {
    this.sprite_renderer.color = pColor;
    this.setAlpha(pAlpha);
  }

  public override void update(float pElapsed)
  {
    this.setAlpha(this.alpha - pElapsed * 0.1f);
    if ((double) this.alpha > 0.0)
      return;
    this.kill();
  }
}

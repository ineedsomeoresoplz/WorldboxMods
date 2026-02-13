// Decompiled with JetBrains decompiler
// Type: Smoke
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Smoke : BaseEffect
{
  private float timer_scale;

  private void Update()
  {
    if ((double) this.timer_scale > 0.0)
    {
      this.timer_scale -= World.world.elapsed;
    }
    else
    {
      this.timer_scale = 0.01f;
      this.setAlpha(this.alpha - 0.01f);
      if ((double) this.alpha <= 0.0)
      {
        this.controller.killObject((BaseEffect) this);
      }
      else
      {
        if ((double) ((Component) this).transform.localScale.x < 4.0)
          ((Component) this).transform.localScale = new Vector3(((Component) this).transform.localScale.x + 0.03f, ((Component) this).transform.localScale.y + 0.03f);
        ((Component) this).transform.localPosition = new Vector3(((Component) this).transform.localPosition.x + World.world.wind_direction.x * 0.5f, ((Component) this).transform.localPosition.y + World.world.wind_direction.y * 0.5f);
      }
    }
  }
}

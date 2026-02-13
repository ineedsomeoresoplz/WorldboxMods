// Decompiled with JetBrains decompiler
// Type: EffectHearts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class EffectHearts : BaseEffect
{
  internal override void spawnOnTile(WorldTile pTile)
  {
    float pScale = Randy.randomFloat(0.3f, 0.5f);
    this.prepare(pTile, pScale);
  }

  public override void update(float pElapsed)
  {
    base.update(pElapsed);
    ((Component) this).transform.position = new Vector3(((Component) this).transform.position.x, ((Component) this).transform.position.y + pElapsed * 3f / Config.time_scale_asset.multiplier);
  }
}

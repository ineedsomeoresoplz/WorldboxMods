// Decompiled with JetBrains decompiler
// Type: Fireworks
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Fireworks : BaseEffect
{
  internal override void spawnOnTile(WorldTile pTile)
  {
    float pScale = Randy.randomFloat(0.3f, 1f);
    this.prepare(pTile, pScale);
    if (Randy.randomBool())
      this.loadSprites("effects/fireworks1");
    else
      this.loadSprites("effects/fireworks2");
    this.sprite_renderer.flipX = Randy.randomBool();
    this.sprite_renderer.color = new Color()
    {
      a = 1f,
      r = Randy.randomFloat(0.0f, 1f),
      b = Randy.randomFloat(0.0f, 1f),
      g = Randy.randomFloat(0.0f, 1f)
    };
    ((Component) this).transform.localEulerAngles = new Vector3(0.0f, 0.0f, Randy.randomFloat(-15f, 15f));
  }

  private void loadSprites(string pPath)
  {
    this.sprite_animation.frames = SpriteTextureLoader.getSpriteList(pPath);
  }
}

// Decompiled with JetBrains decompiler
// Type: MoodAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Beebyte.Obfuscator;
using System;
using UnityEngine;

#nullable disable
[ObfuscateLiterals]
[Serializable]
public class MoodAsset : Asset
{
  public BaseStats base_stats = new BaseStats();
  public string icon;
  [NonSerialized]
  public Sprite sprite;

  public Sprite getSprite()
  {
    if (Object.op_Equality((Object) this.sprite, (Object) null))
      this.sprite = SpriteTextureLoader.getSprite("ui/Icons/" + this.icon);
    return this.sprite;
  }
}

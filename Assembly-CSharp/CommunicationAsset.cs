// Decompiled with JetBrains decompiler
// Type: CommunicationAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class CommunicationAsset : Asset
{
  public string icon_path;
  public bool show_topic;
  public float rate;
  public TopicCheck check;
  public TopicPotFill pot_fill;
  [NonSerialized]
  private Sprite _sprite_cache;

  public Sprite getSpriteBubble()
  {
    if (Object.op_Equality((Object) this._sprite_cache, (Object) null))
      this._sprite_cache = SpriteTextureLoader.getSprite(this.icon_path);
    return this._sprite_cache;
  }
}

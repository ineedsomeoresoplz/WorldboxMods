// Decompiled with JetBrains decompiler
// Type: AnimationContainerUnit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class AnimationContainerUnit
{
  public bool child;
  internal readonly string id;
  internal readonly Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
  internal readonly Dictionary<string, AnimationFrameData> dict_frame_data = new Dictionary<string, AnimationFrameData>();
  internal ActorAnimation idle;
  internal ActorAnimation walking;
  internal ActorAnimation swimming;
  public bool has_swimming;
  public bool has_idle;
  public bool has_walking;
  public bool render_heads_for_children;
  internal Sprite[] heads;
  internal Sprite[] heads_male;
  internal Sprite[] heads_female;

  public AnimationContainerUnit(string pTexturePath)
  {
    this.id = pTexturePath;
    foreach (Sprite sprite in SpriteTextureLoader.getSpriteList(pTexturePath))
      this.sprites.Add(((Object) sprite).name, sprite);
  }
}

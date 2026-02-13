// Decompiled with JetBrains decompiler
// Type: UnitHandToolAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class UnitHandToolAsset : Asset, IHandRenderer
{
  public bool animated;
  public string path_gameplay_sprite;
  public bool colored;
  [NonSerialized]
  public Sprite[] gameplay_sprites;

  public Sprite[] getSprites() => this.gameplay_sprites;

  public bool is_colored => this.colored;

  public bool is_animated => this.animated;
}

// Decompiled with JetBrains decompiler
// Type: ItemRendering
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class ItemRendering
{
  public static Sprite getItemMainSpriteFrame(IHandRenderer pHandRendererAsset)
  {
    if (pHandRendererAsset == null)
      return (Sprite) null;
    Sprite[] sprites = pHandRendererAsset.getSprites();
    return sprites.Length > 1 ? AnimationHelper.getSpriteFromList(0, (IList<Sprite>) sprites, 5f) : sprites[0];
  }
}

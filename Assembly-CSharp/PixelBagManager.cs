// Decompiled with JetBrains decompiler
// Type: PixelBagManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class PixelBagManager
{
  private static readonly Dictionary<Sprite, PixelBag> _dict = new Dictionary<Sprite, PixelBag>();

  public static PixelBag getPixelBag(
    Sprite pSpriteSource,
    bool pCheckPhenotypes = false,
    bool pCheckLights = false)
  {
    PixelBag pixelBag;
    PixelBagManager._dict.TryGetValue(pSpriteSource, out pixelBag);
    if (pixelBag == null)
      PixelBagManager.createPixelBag(pSpriteSource, pCheckPhenotypes, pCheckLights);
    return PixelBagManager._dict[pSpriteSource];
  }

  private static void createPixelBag(
    Sprite pSpriteSource,
    bool pCheckPhenotypes,
    bool pCheckLights)
  {
    PixelBag pixelBag = new PixelBag(pSpriteSource, pCheckPhenotypes, pCheckLights);
    PixelBagManager._dict.Add(pSpriteSource, pixelBag);
  }

  public static void preloadPixelBagUnit(Sprite pSpriteSource)
  {
    PixelBagManager.getPixelBag(pSpriteSource, true);
  }

  public static int total => PixelBagManager._dict.Count;
}

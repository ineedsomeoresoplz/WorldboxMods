// Decompiled with JetBrains decompiler
// Type: GenericBannerLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public abstract class GenericBannerLibrary : AssetLibrary<BannerAsset>
{
  public BannerAsset main;

  public int getNewIndexBackground()
  {
    return Randy.randomInt(0, this.getCurrentAsset().backgrounds.Count);
  }

  public int getNewIndexIcon() => Randy.randomInt(0, this.getCurrentAsset().icons.Count);

  public int getNewIndexFrame() => Randy.randomInt(0, this.getCurrentAsset().frames.Count);

  public Sprite getSpriteBackground(int pIndex)
  {
    return this.loadSpriteFromAsset(this.getCurrentAsset().backgrounds, pIndex);
  }

  public Sprite getSpriteBackground(int pIndex, string pAssetID)
  {
    return this.loadSpriteFromAsset(this.get(pAssetID).backgrounds, pIndex);
  }

  public Sprite getSpriteIcon(int pIndex)
  {
    return this.loadSpriteFromAsset(this.getCurrentAsset().icons, pIndex);
  }

  public Sprite getSpriteIcon(int pIndex, string pAssetID)
  {
    return this.loadSpriteFromAsset(this.get(pAssetID).icons, pIndex);
  }

  public Sprite getSpriteFrame(int pID)
  {
    BannerAsset currentAsset = this.getCurrentAsset();
    if (pID >= currentAsset.frames.Count)
      pID = 0;
    return SpriteTextureLoader.getSprite(currentAsset.frames[pID]);
  }

  private Sprite loadSpriteFromAsset(List<string> pSpriteList, int pIndex)
  {
    if (pIndex >= pSpriteList.Count)
      pIndex = 0;
    return SpriteTextureLoader.getSprite(pSpriteList[pIndex]);
  }

  public BannerAsset getCurrentAsset() => this.main;
}

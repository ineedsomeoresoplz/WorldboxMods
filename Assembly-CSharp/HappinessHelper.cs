// Decompiled with JetBrains decompiler
// Type: HappinessHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class HappinessHelper
{
  public static Sprite getSpriteBasedOnHappinessValue(int pValue)
  {
    return pValue >= -80 ? (pValue >= -20 ? (pValue >= 20 ? (pValue >= 40 ? (pValue >= 80 /*0x50*/ ? SpriteTextureLoader.getSprite("ui/Icons/iconHappiness_6") : SpriteTextureLoader.getSprite("ui/Icons/iconHappiness_5")) : SpriteTextureLoader.getSprite("ui/Icons/iconHappiness_4")) : SpriteTextureLoader.getSprite("ui/Icons/iconHappiness_3")) : SpriteTextureLoader.getSprite("ui/Icons/iconHappiness_2")) : SpriteTextureLoader.getSprite("ui/Icons/iconHappiness_1");
  }
}

// Decompiled with JetBrains decompiler
// Type: DebugAnimatedVariation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DebugAnimatedVariation
{
  public bool animated;
  public Sprite[] frames;

  public DebugAnimatedVariation(Sprite[] pFrames, bool pAnimated)
  {
    this.animated = pAnimated;
    this.frames = pFrames;
  }
}

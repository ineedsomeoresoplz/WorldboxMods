// Decompiled with JetBrains decompiler
// Type: AnimationHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public static class AnimationHelper
{
  private static float animationTimeMax = 100f;
  private static float _time_simulation;
  private static float _time_session;

  public static float getTime() => AnimationHelper._time_simulation;

  public static void updateTime(float pElapsedScaled, float pElapsedSession)
  {
    AnimationHelper.updateTimeSimulation(pElapsedScaled);
    AnimationHelper.updateTimeSession(pElapsedSession);
  }

  private static void updateTimeSimulation(float pElapsed)
  {
    if (World.world.isPaused())
      return;
    AnimationHelper._time_simulation += pElapsed;
    if ((double) AnimationHelper._time_simulation < (double) AnimationHelper.animationTimeMax)
      return;
    AnimationHelper._time_simulation -= AnimationHelper.animationTimeMax;
  }

  private static void updateTimeSession(float pElapsed)
  {
    AnimationHelper._time_session += pElapsed;
    if ((double) AnimationHelper._time_session < (double) AnimationHelper.animationTimeMax)
      return;
    AnimationHelper._time_session -= AnimationHelper.animationTimeMax;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static float getAnimationGlobalTime(float pAnimationSpeed)
  {
    return AnimationHelper._time_simulation * pAnimationSpeed;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Sprite getSpriteFromListSessionTime(
    int pHashCodeOffset,
    IList<Sprite> pFrames,
    float pAnimationSpeed)
  {
    return AnimationHelper.getSpriteFromList(AnimationHelper._time_session * pAnimationSpeed, pHashCodeOffset, pFrames);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Sprite getSpriteFromList(
    int pHashCodeOffset,
    IList<Sprite> pFrames,
    float pAnimationSpeed)
  {
    return AnimationHelper.getSpriteFromList(AnimationHelper.getAnimationGlobalTime(pAnimationSpeed), pHashCodeOffset, pFrames);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Sprite getSpriteFromList(float pTime, int pHashCodeOffset, IList<Sprite> pFrames)
  {
    int spriteIndex = AnimationHelper.getSpriteIndex(pTime, pHashCodeOffset, pFrames.Count);
    return pFrames[spriteIndex];
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static int getSpriteIndex(float pTime, int pHashCodeOffset, int pFrameCount)
  {
    if (pHashCodeOffset < 0)
      pHashCodeOffset = -pHashCodeOffset;
    return (int) ((double) pTime + (double) (pHashCodeOffset * 100)) % pFrameCount;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static int getSpriteIndex(long pHashCodeOffset, int pFrameCount)
  {
    if (pHashCodeOffset < 0L)
      pHashCodeOffset = -pHashCodeOffset;
    return (int) ((double) (1L + pHashCodeOffset * 100L) % (double) pFrameCount);
  }
}

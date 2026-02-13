// Decompiled with JetBrains decompiler
// Type: Randy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
public static class Randy
{
  internal static Random rnd = new Random(123);
  internal static Random rand = new Random(123U);
  internal static Random debug_rand = new Random(123U);
  private static long _seed = 1;

  [RuntimeInitializeOnLoadMethod]
  private static void OnBeforeSplashScreen() => Randy.fullReset();

  [RuntimeInitializeOnLoadMethod]
  private static void OnBeforeSceneLoad() => Randy.fullReset();

  [RuntimeInitializeOnLoadMethod]
  private static void OnAfterSceneLoad() => Randy.fullReset();

  internal static void fullReset()
  {
    DateTime now = DateTime.Now;
    int year = now.Year;
    now = DateTime.Now;
    int month = now.Month;
    now = DateTime.Now;
    int day = now.Day;
    now = DateTime.Now;
    int hour = now.Hour;
    Randy._seed = (long) (year * 100000 + month * 1000 + day * 10 + hour / 3);
    Randy.nextSeed();
  }

  internal static void nextSeed()
  {
    Randy._seed += 543L;
    Randy.resetSeed(Randy._seed);
  }

  public static void resetSeed(long pLongValue)
  {
    if (pLongValue == 0L)
      pLongValue = 1L;
    int Seed = (int) pLongValue;
    uint num = (uint) pLongValue;
    Random.InitState(Seed);
    Randy.rnd = new Random(Seed);
    Randy.rand = new Random(num);
    ((Random) ref Randy.rand).NextBool();
  }

  public static void resetSeed(int pIntValue)
  {
    if (pIntValue == 0)
      pIntValue = 1;
    Random.InitState(pIntValue);
    uint num = (uint) pIntValue;
    Randy.rnd = new Random(pIntValue);
    Randy.rand = new Random(num);
    ((Random) ref Randy.rand).NextBool();
  }

  [Pure]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static int randomInt(int pMinInclusive, int pMaxExclusive)
  {
    return ((Random) ref Randy.rand).NextInt(pMinInclusive, pMaxExclusive);
  }

  [Pure]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool randomBool() => ((Random) ref Randy.rand).NextBool();

  [Pure]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool randomChance(float pVal)
  {
    float num = Randy.random();
    return (double) pVal >= (double) num;
  }

  [Pure]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static float random() => ((Random) ref Randy.rand).NextFloat();

  [Pure]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static float randomFloat(float pMinInclusive, float pMaxExclusive)
  {
    return ((Random) ref Randy.rand).NextFloat(pMinInclusive, pMaxExclusive);
  }

  [Pure]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector2 randomPointOnCircle(float pRadiusMinInclusive, float pRadiusMaxExclusive)
  {
    Vector2 pointInUnitCircle = Randy.getRandomPointInUnitCircle();
    return Vector2.op_Multiply(((Vector2) ref pointInUnitCircle).normalized, Randy.randomFloat(pRadiusMinInclusive, pRadiusMaxExclusive));
  }

  [Pure]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Vector2 getRandomPointInUnitCircle()
  {
    float num1 = Mathf.Sqrt(Randy.random());
    float num2 = (float) ((double) Randy.random() * 2.0 * 3.1415927410125732);
    return new Vector2(num1 * Mathf.Cos(num2), num1 * Mathf.Sin(num2));
  }

  [Pure]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static T getRandom<T>(T[] pArray) => pArray.GetRandom<T>();

  [Pure]
  [CanBeNull]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static T getRandom<T>(List<T> pList)
  {
    return pList.Count == 0 ? default (T) : pList.GetRandom<T>();
  }

  [Pure]
  [CanBeNull]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static T getRandom<T>(ListPool<T> pList)
  {
    return pList.Count == 0 ? default (T) : pList.GetRandom<T>();
  }

  [Pure]
  public static T RandomEnumValue<T>() where T : Enum
  {
    Array values = Enum.GetValues(typeof (T));
    return (T) values.GetValue(Randy.randomInt(0, values.Length));
  }

  [Pure]
  public static Color getRandomColor()
  {
    return new Color(Randy.random(), Randy.random(), Randy.random(), 1f);
  }

  [Pure]
  public static Color ColorHSV() => Randy.ColorHSV(0.0f, 1f, 0.0f, 1f, 0.0f, 1f, 1f, 1f);

  [Pure]
  public static Color ColorHSV(
    float hueMin,
    float hueMax,
    float saturationMin,
    float saturationMax,
    float valueMin,
    float valueMax,
    float alphaMin,
    float alphaMax)
  {
    double num1 = (double) Mathf.Lerp(hueMin, hueMax, ((Random) ref Randy.debug_rand).NextFloat());
    float num2 = Mathf.Lerp(saturationMin, saturationMax, ((Random) ref Randy.debug_rand).NextFloat());
    float num3 = Mathf.Lerp(valueMin, valueMax, ((Random) ref Randy.debug_rand).NextFloat());
    double num4 = (double) num2;
    double num5 = (double) num3;
    Color rgb = Color.HSVToRGB((float) num1, (float) num4, (float) num5, true);
    rgb.a = Mathf.Lerp(alphaMin, alphaMax, ((Random) ref Randy.debug_rand).NextFloat());
    return rgb;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static IEnumerable<T> LoopRandom<T>(this List<T> list)
  {
    return (IEnumerable<T>) new RandomListEnumerator<T>(list);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static IEnumerable<T> LoopRandom<T>(this List<T> list, int pMax)
  {
    return (IEnumerable<T>) new RandomListEnumerator<T>(list, list.Count, pMax);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static IEnumerable<T> LoopRandom<T>(this T[] array)
  {
    return (IEnumerable<T>) new RandomArrayEnumerator<T>(array);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static IEnumerable<T> LoopRandom<T>(this T[] array, int pLength)
  {
    return (IEnumerable<T>) new RandomArrayEnumerator<T>(array, pLength);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static IEnumerable<T> LoopRandom<T>(this T[] array, int pLength, int pMax)
  {
    return (IEnumerable<T>) new RandomArrayEnumerator<T>(array, pLength, pMax);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static IEnumerable<T> LoopRandom<T>(this ListPool<T> list)
  {
    return (IEnumerable<T>) new RandomArrayEnumerator<T>(list.GetRawBuffer(), list.Count);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static IEnumerable<T> LoopRandom<T>(this ListPool<T> list, int pMax)
  {
    return (IEnumerable<T>) new RandomArrayEnumerator<T>(list.GetRawBuffer(), list.Count, pMax);
  }

  public static IEnumerable<T> LoopRandom<T>(this IEnumerable<T> pEnumerable)
  {
    ListPool<T> tPool = (ListPool<T>) null;
    IEnumerable<T> objs;
    switch (pEnumerable)
    {
      case List<T> list1:
        objs = list1.LoopRandom<T>();
        break;
      case T[] array:
        objs = array.LoopRandom<T>(array.Length);
        break;
      case ListPool<T> list2:
        objs = list2.LoopRandom<T>();
        break;
      default:
        tPool = new ListPool<T>(pEnumerable);
        objs = tPool.LoopRandom<T>();
        break;
    }
    try
    {
      foreach (T obj in objs)
        yield return obj;
    }
    finally
    {
      tPool?.Dispose();
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static IEnumerable<T> LoopRandom<T>(this IEnumerable<T> pEnumerable, int pMax)
  {
    ListPool<T> tPool = (ListPool<T>) null;
    IEnumerable<T> objs;
    switch (pEnumerable)
    {
      case List<T> list1:
        objs = list1.LoopRandom<T>(pMax);
        break;
      case T[] array:
        objs = array.LoopRandom<T>(array.Length, pMax);
        break;
      case ListPool<T> list2:
        objs = list2.LoopRandom<T>(pMax);
        break;
      default:
        tPool = new ListPool<T>(pEnumerable);
        objs = tPool.LoopRandom<T>(pMax);
        break;
    }
    try
    {
      foreach (T obj in objs)
        yield return obj;
    }
    finally
    {
      tPool?.Dispose();
    }
  }
}

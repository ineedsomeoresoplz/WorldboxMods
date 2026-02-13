// Decompiled with JetBrains decompiler
// Type: Epoch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public static class Epoch
{
  private static DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

  public static double Current() => (DateTime.UtcNow - Epoch.epochStart).TotalSeconds;

  public static double SecondsElapsed(double t1)
  {
    return (double) Mathf.Abs((float) (Epoch.Current() - t1));
  }

  public static int SecondsElapsed(int t1, int t2) => Mathf.Abs(t1 - t2);

  internal static DateTime toDateTime(double epoch)
  {
    return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(epoch);
  }
}

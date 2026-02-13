// Decompiled with JetBrains decompiler
// Type: LongExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Runtime.CompilerServices;

#nullable disable
public static class LongExtension
{
  public const long NULL = -1;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool hasValue(this long pLong) => pLong != -1L;

  public static long? toNullLong(this long pLong)
  {
    return !pLong.hasValue() ? new long?() : new long?(pLong);
  }

  public static long toLong(this long? pLong) => pLong ?? -1L;
}

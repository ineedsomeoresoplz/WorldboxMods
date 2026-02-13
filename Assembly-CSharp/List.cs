// Decompiled with JetBrains decompiler
// Type: List
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public static class List
{
  public static List<T> Of<T>(params T[] pArgs) => new List<T>((IEnumerable<T>) pArgs);
}

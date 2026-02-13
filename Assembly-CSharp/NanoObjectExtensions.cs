// Decompiled with JetBrains decompiler
// Type: NanoObjectExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using JetBrains.Annotations;
using System.Runtime.CompilerServices;

#nullable disable
public static class NanoObjectExtensions
{
  [ContractAnnotation("null => true")]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool isRekt(this NanoObject pObject) => pObject == null || !pObject.isAlive();
}

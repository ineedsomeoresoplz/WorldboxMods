// Decompiled with JetBrains decompiler
// Type: ExceptionHelpers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Runtime.ExceptionServices;

#nullable disable
internal static class ExceptionHelpers
{
  public static Exception PrepareForRethrow(Exception exception)
  {
    ExceptionDispatchInfo.Capture(exception).Throw();
    return exception;
  }
}

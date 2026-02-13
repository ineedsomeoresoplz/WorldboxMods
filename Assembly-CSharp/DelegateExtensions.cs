// Decompiled with JetBrains decompiler
// Type: DelegateExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
public static class DelegateExtensions
{
  public static string AsString<T>(this T pDelegate) where T : Delegate
  {
    if ((Delegate) pDelegate == (Delegate) null)
      return "";
    using (ListPool<string> list = new ListPool<string>(pDelegate.GetInvocationList().Length))
    {
      foreach (T invocation in pDelegate.GetInvocationList())
        list.Add(invocation.Method.Name);
      return string.Join(", ", list.ToArray<string>());
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: TypeExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
public static class TypeExtensions
{
  public static bool hasField(this Type pStaticType, string pFieldName)
  {
    return pStaticType.GetField(pFieldName, BindingFlags.Static | BindingFlags.Public) != (FieldInfo) null;
  }

  public static IEnumerable<string> getFields(this Type pStaticType)
  {
    FieldInfo[] fieldInfoArray = pStaticType.GetFields(BindingFlags.Static | BindingFlags.Public);
    for (int index = 0; index < fieldInfoArray.Length; ++index)
      yield return fieldInfoArray[index].Name;
    fieldInfoArray = (FieldInfo[]) null;
  }
}

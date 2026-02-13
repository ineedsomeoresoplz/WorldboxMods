// Decompiled with JetBrains decompiler
// Type: FieldInfoExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Reflection;

#nullable disable
public static class FieldInfoExtensions
{
  public static bool isString(this FieldInfo pField) => pField.FieldType == typeof (string);

  public static bool isCollection(this FieldInfo pField)
  {
    Type fieldType = pField.FieldType;
    return !pField.isString() && typeof (ICollection).IsAssignableFrom(fieldType);
  }

  public static bool isEnumerable(this FieldInfo pField)
  {
    Type fieldType = pField.FieldType;
    return !pField.isString() && typeof (IEnumerable).IsAssignableFrom(fieldType);
  }

  public static bool isCloneable(this FieldInfo pField)
  {
    Type fieldType = pField.FieldType;
    return !pField.isString() && typeof (ICloneable).IsAssignableFrom(fieldType);
  }
}

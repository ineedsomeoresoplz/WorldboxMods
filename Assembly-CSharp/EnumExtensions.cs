// Decompiled with JetBrains decompiler
// Type: EnumExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
public static class EnumExtensions
{
  public static int Count<TEnum>(this TEnum pEnum) where TEnum : Enum
  {
    int num = 0;
    int int32 = Convert.ToInt32((object) pEnum);
    while (int32 != 0)
    {
      int32 &= int32 - 1;
      ++num;
    }
    return num;
  }
}

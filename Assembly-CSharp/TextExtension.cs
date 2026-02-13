// Decompiled with JetBrains decompiler
// Type: TextExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

#nullable disable
public static class TextExtension
{
  private static Font krutiDev;
  private static List<string> colors = new List<string>();

  public static void SetHindiText(this UnityEngine.UI.Text text, string value)
  {
    if (Object.op_Equality((Object) TextExtension.krutiDev, (Object) null))
      TextExtension.krutiDev = Resources.Load("CD_Kruti_Dev_010") as Font;
    bool flag = value.IndexOf("</color>", StringComparison.Ordinal) > -1;
    if (flag)
    {
      TextExtension.colors.Clear();
      value = value.Replace("</color>", "END_COLOR");
      int num = 0;
      foreach (object match in Regex.Matches(value, "<color.*?>"))
      {
        TextExtension.colors.Add(match.ToString());
        value = value.Replace(match.ToString(), "COLOR_" + num++.ToString());
      }
    }
    if (value.IndexOf("'", StringComparison.Ordinal) > -1)
      value = value.Replace("'", "SINGLE_QUOTE");
    value = HindiCorrector.GetCorrectedHindiText(value);
    if (value.IndexOf("SINGLE_QUOTE", StringComparison.Ordinal) > -1)
      value = value.Replace("SINGLE_QUOTE", "'");
    if (flag)
    {
      value = value.Replace("END_COLOR", "</color>");
      int num = 0;
      foreach (string color in TextExtension.colors)
        value = value.Replace("COLOR_" + num++.ToString(), color);
    }
    text.font = TextExtension.krutiDev;
    text.text = value;
  }
}

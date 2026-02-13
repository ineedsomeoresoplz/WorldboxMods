// Decompiled with JetBrains decompiler
// Type: HotkeysLocalized
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class HotkeysLocalized
{
  private static Dictionary<KeyCode, string> _dictionary;

  private static void init()
  {
    HotkeysLocalized._dictionary = new Dictionary<KeyCode, string>();
    HotkeysLocalized._dictionary.Add((KeyCode) 48 /*0x30*/, "0");
    HotkeysLocalized._dictionary.Add((KeyCode) 49, "1");
    HotkeysLocalized._dictionary.Add((KeyCode) 50, "2");
    HotkeysLocalized._dictionary.Add((KeyCode) 51, "3");
    HotkeysLocalized._dictionary.Add((KeyCode) 52, "4");
    HotkeysLocalized._dictionary.Add((KeyCode) 53, "5");
    HotkeysLocalized._dictionary.Add((KeyCode) 54, "6");
    HotkeysLocalized._dictionary.Add((KeyCode) 55, "7");
    HotkeysLocalized._dictionary.Add((KeyCode) 56, "8");
    HotkeysLocalized._dictionary.Add((KeyCode) 57, "9");
    HotkeysLocalized._dictionary.Add((KeyCode) 256 /*0x0100*/, "0");
    HotkeysLocalized._dictionary.Add((KeyCode) 257, "1");
    HotkeysLocalized._dictionary.Add((KeyCode) 258, "2");
    HotkeysLocalized._dictionary.Add((KeyCode) 259, "3");
    HotkeysLocalized._dictionary.Add((KeyCode) 260, "4");
    HotkeysLocalized._dictionary.Add((KeyCode) 261, "5");
    HotkeysLocalized._dictionary.Add((KeyCode) 262, "6");
    HotkeysLocalized._dictionary.Add((KeyCode) 263, "7");
    HotkeysLocalized._dictionary.Add((KeyCode) 264, "8");
    HotkeysLocalized._dictionary.Add((KeyCode) 265, "9");
    HotkeysLocalized._dictionary.Add((KeyCode) 32 /*0x20*/, "SPACE");
    HotkeysLocalized._dictionary.Add((KeyCode) 304, "SHIFT");
    HotkeysLocalized._dictionary.Add((KeyCode) 303, "SHIFT");
    HotkeysLocalized._dictionary.Add((KeyCode) 308, "ALT");
    HotkeysLocalized._dictionary.Add((KeyCode) 307, "ALT");
    HotkeysLocalized._dictionary.Add((KeyCode) 306, "CONTROL");
    HotkeysLocalized._dictionary.Add((KeyCode) 305, "CONTROL");
    HotkeysLocalized._dictionary.Add((KeyCode) 310, "");
    HotkeysLocalized._dictionary.Add((KeyCode) 309, "");
  }

  public static string getLocalizedKey(KeyCode pCode)
  {
    if (HotkeysLocalized._dictionary == null)
      HotkeysLocalized.init();
    if (pCode == null)
      return string.Empty;
    string empty = string.Empty;
    string pText = !HotkeysLocalized._dictionary.ContainsKey(pCode) ? pCode.ToString() : HotkeysLocalized._dictionary[pCode];
    return string.IsNullOrEmpty(pText) ? string.Empty : Toolbox.coloredText(pText, "#95DD5D");
  }
}

// Decompiled with JetBrains decompiler
// Type: ConstantineConventer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#nullable disable
public class ConstantineConventer
{
  private static bool enabled;

  public static void init()
  {
    if (!ConstantineConventer.enabled)
      return;
    string[] strArray1 = Resources.Load<TextAsset>("texts/fmod_sheet").text.Split('\n', StringSplitOptions.None);
    Debug.Log((object) strArray1[0]);
    List<string> stringList = new List<string>();
    string contents = "";
    foreach (string str1 in strArray1)
    {
      string str2 = str1.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
      string[] strArray2 = str2.Split('/', StringSplitOptions.None);
      string str3 = "\tpublic const string " + strArray2[strArray2.Length - 1] + " = " + "\"" + str2 + "\"" + ";";
      stringList.Add(str3);
      contents = $"{contents}{str3}\n";
    }
    File.WriteAllText(Application.dataPath + "/Resources/texts/fmod_sheet_converted.txt", contents);
  }

  public static void init2()
  {
    string[] strArray = Resources.Load<TextAsset>("texts/fmod_sheet").text.Split('\n', StringSplitOptions.None);
    Debug.Log((object) strArray[0]);
    List<string> stringList = new List<string>();
    string contents = "";
    string str1 = "";
    foreach (string str2 in strArray)
    {
      string str3 = str2.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
      if (str3.Contains("$"))
        str1 = str3.Replace("$ ", "").Replace("$", "");
      else if (!str3.Contains("WB_SFX_"))
      {
        contents += "\n";
      }
      else
      {
        string str4 = $"{"\tpublic const string " + str3 + " = "}{str1} + " + "\"" + str3 + "\"" + ";";
        stringList.Add(str4);
        contents = $"{contents}{str4}\n";
      }
    }
    File.WriteAllText(Application.dataPath + "/Resources/texts/fmod_sheet_converted.txt", contents);
  }
}

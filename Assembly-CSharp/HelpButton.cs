// Decompiled with JetBrains decompiler
// Type: HelpButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class HelpButton : MonoBehaviour
{
  public void clickHelp()
  {
    string stringVal = PlayerConfig.dict["language"].stringVal;
    Analytics.LogEvent("open_help");
    string str;
    if (Application.platform == 11)
      str = "https://support.google.com/googleplay/answer/1050566?hl=" + stringVal;
    else
      str = $"https://support.apple.com/{stringVal}-{stringVal}/HT203005";
    Application.OpenURL(str);
  }
}

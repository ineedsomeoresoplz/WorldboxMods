// Decompiled with JetBrains decompiler
// Type: ButtonVote
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ButtonVote : MonoBehaviour
{
  public void openLink()
  {
    Analytics.LogEvent("click_vote");
    if (Config.isAndroid)
    {
      Application.OpenURL("https://play.google.com/store/apps/details?id=com.mkarpenko.worldbox");
    }
    else
    {
      if (!Config.isIos)
        return;
      Application.OpenURL("https://itunes.apple.com/app/id1450941371");
    }
  }
}

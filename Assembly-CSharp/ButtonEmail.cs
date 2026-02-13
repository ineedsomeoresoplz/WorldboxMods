// Decompiled with JetBrains decompiler
// Type: ButtonEmail
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Networking;

#nullable disable
public class ButtonEmail : MonoBehaviour
{
  public void SendEmail()
  {
    Application.OpenURL($"mailto:supworldbox@gmail.com?subject={this.convert($"WorldBox Feedback ( {Application.version} )")}&body={this.convert("Yo!\r\n")}");
    Analytics.LogEvent("clicked_send_email");
  }

  public void SendEmailLogs()
  {
    Application.OpenURL($"mailto:supworldbox+errors@gmail.com?subject={this.convert($"WorldBox Error Logs ( {Application.version} )")}&body={this.convert("Please take a look at this error :\r\n" + LogHandler.log.Substring(Math.Max(0, LogHandler.log.Length - 4000)))}");
    Analytics.LogEvent("clicked_send_error_email");
  }

  private string convert(string url) => UnityWebRequest.EscapeURL(url).Replace("+", "%20");
}

// Decompiled with JetBrains decompiler
// Type: ButtonTranslate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ButtonTranslate : MonoBehaviour
{
  public void openLink()
  {
    Analytics.LogEvent("click_translate");
    Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSeL8sirqSFbHa_dHipgu-2QiRSNHqEn2l7ApodM8qD5xm010A/viewform");
  }
}

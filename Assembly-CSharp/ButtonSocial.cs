// Decompiled with JetBrains decompiler
// Type: ButtonSocial
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ButtonSocial : MonoBehaviour
{
  [SerializeField]
  private SocialType _social_type;
  [SerializeField]
  private Text _text;

  private void Awake()
  {
    switch (this._social_type)
    {
      case SocialType.Facebook:
        this._text.text = 82.ToString() + "k+";
        break;
      case SocialType.Twitter:
        this._text.text = 56.ToString() + "k+";
        break;
      case SocialType.Discord:
        this._text.text = 560.ToString() + "k+";
        break;
      case SocialType.Reddit:
        this._text.text = 140.ToString() + "k+";
        break;
    }
  }

  public void openFacebook()
  {
    Analytics.LogEvent("open_link_facebook");
    Application.OpenURL("https://www.facebook.com/superworldbox");
  }

  public void openTwitter()
  {
    Analytics.LogEvent("open_link_twitter");
    Application.OpenURL("http://twitter.com/mixamko");
  }

  public void openDiscord()
  {
    Analytics.LogEvent("open_link_discord");
    Application.OpenURL("https://discordapp.com/invite/worldbox");
    AchievementLibrary.social_network.check();
  }

  public void openLinkReddit()
  {
    Analytics.LogEvent("open_link_reddit");
    Application.OpenURL("https://www.reddit.com/r/worldbox");
  }

  public void openLinkMoonBox()
  {
    Analytics.LogEvent("open_link_moonbox");
    if (Config.isIos)
      Application.OpenURL("https://bit.ly/moonbox_wb_ap");
    else
      Application.OpenURL("https://bit.ly/moonbox_wb");
  }

  public void openLinkSteam()
  {
    Analytics.LogEvent("open_link_steam");
    Application.OpenURL($"{$"https://store.steampowered.com/app/{(ValueType) 1206560U}/" + "?utm_source=game_bar" + "&utm_campaign=get_wishlists"}&utm_medium={Application.platform.ToString()}");
  }
}

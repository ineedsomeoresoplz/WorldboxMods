// Decompiled with JetBrains decompiler
// Type: SteamTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Steamworks;
using System;
using UnityEngine;

#nullable disable
public class SteamTracker : MonoBehaviour, IRichTracker
{
  private static bool steam_initialized = false;
  private SteamTracker instance;
  private static float timer = 10f;

  private void Start()
  {
    this.instance = this;
    SteamSDK.steamInitialized.Then((Action) (() => SteamTracker.init())).Catch((Action<Exception>) (_ => Object.Destroy((Object) this.instance)));
  }

  private void OnDestroy()
  {
    this.instance = (SteamTracker) null;
    PowerTracker.steamTracker = (SteamTracker) null;
  }

  private static bool init()
  {
    if (SteamSDK.steamInitialized != null && SteamSDK.steamInitialized.CurState == 2)
      SteamTracker.steam_initialized = true;
    return SteamTracker.steam_initialized;
  }

  public void trackViewing(string pText)
  {
    if (!SteamTracker.steam_initialized && !SteamTracker.init())
      return;
    if (pText == "" || !LocalizedTextManager.stringExists(pText))
    {
      this.trackActivity("#Status_browsing");
    }
    else
    {
      SteamFriends.SetRichPresence("window", LocalizedTextManager.getText(pText));
      this.trackActivity("#Status_viewing");
    }
  }

  public void trackWatching() => this.trackActivity("#Status_watching");

  public void trackUsing(string pPower)
  {
    if (!SteamTracker.steam_initialized && !SteamTracker.init())
      return;
    SteamFriends.SetRichPresence("power", LocalizedTextManager.getText(pPower));
    this.trackActivity("#Status_using");
  }

  public void updateUsing(int pAmount, string pPower = "")
  {
    if (!SteamTracker.steam_initialized && !SteamTracker.init())
      return;
    if (pPower != "")
      SteamFriends.SetRichPresence("power", LocalizedTextManager.getText(pPower));
    SteamFriends.SetRichPresence("amount", pAmount.ToString());
    this.trackActivity("#Status_using_num");
  }

  public void inspectKingdom(string pKingdom)
  {
    if (!SteamTracker.steam_initialized && !SteamTracker.init())
      return;
    SteamFriends.SetRichPresence("kingdom", pKingdom);
    this.trackActivity("#Status_kingdom");
  }

  public void inspectVillage(string pVillage)
  {
    if (!SteamTracker.steam_initialized && !SteamTracker.init())
      return;
    SteamFriends.SetRichPresence("village", pVillage);
    this.trackActivity("#Status_village");
  }

  public void inspectUnit(string pUnit)
  {
    if (!SteamTracker.steam_initialized && !SteamTracker.init())
      return;
    SteamFriends.SetRichPresence("unit", pUnit);
    this.trackActivity("#Status_unit");
  }

  public void spectatingUnit(string pUnit)
  {
    if (!SteamTracker.steam_initialized && !SteamTracker.init())
      return;
    SteamFriends.SetRichPresence("unit", pUnit);
    this.trackActivity("#Status_spectating");
  }

  public void updateDetails(StatisticsAsset pStat)
  {
    if (!SteamTracker.steam_initialized && !SteamTracker.init())
      return;
    string localeId = pStat.getLocaleID();
    if (!string.IsNullOrEmpty(localeId))
      SteamFriends.SetRichPresence("stat", localeId);
    SteamFriends.SetRichPresence("value", pStat.last_value);
    this.trackActivity(pStat.steam_activity);
  }

  public void trackActivity(string pText)
  {
    if (!SteamTracker.steam_initialized && !SteamTracker.init())
      return;
    SteamTracker.timer = 10f;
    try
    {
      if (pText.Substring(0, 1) != "#")
        Debug.LogError((object) pText);
      else
        SteamFriends.SetRichPresence("steam_display", pText);
    }
    catch (Exception ex)
    {
      Debug.LogError((object) "Could not set Steam Rich Presence (Steam not running, or game not run as Administrator)");
      Debug.LogError((object) ex);
    }
  }

  private void Update()
  {
    if (!SteamTracker.steam_initialized)
      return;
    if ((double) SteamTracker.timer > 0.0)
    {
      SteamTracker.timer -= Time.deltaTime;
    }
    else
    {
      SteamTracker.timer = 10f;
      try
      {
        this.updateDetails(PowerTracker.activeStat);
      }
      catch (Exception ex)
      {
        Debug.Log((object) "Steam Failed or Disabled");
        Debug.Log((object) ex);
        Object.Destroy((Object) this.instance);
      }
    }
  }
}

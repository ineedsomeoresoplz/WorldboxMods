// Decompiled with JetBrains decompiler
// Type: DiscordTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Discord;
using Proyecto26;
using System;
using UnityEngine;

#nullable disable
public class DiscordTracker : MonoBehaviour, IRichTracker
{
  private const long DISCORD_GAME_ID = 816251591299432468;
  private const ulong DISCORD_FLAGS = 1;
  private static Discord.Discord _discord;
  private static ActivityManager _activity_manager;
  private bool _initiated;
  private static DiscordTracker _instance;
  private static Activity _activity;
  private static bool _have_user = false;
  private static int _user_tries = 10;
  private static float _timer = 10f;

  private void Start()
  {
    if (this._initiated)
      return;
    this._initiated = true;
    bool flag = false;
    try
    {
      DiscordTracker._instance = this;
      DiscordTracker._discord = new Discord.Discord(816251591299432468L, 1UL);
      DiscordTracker._activity_manager = DiscordTracker._discord.GetActivityManager();
      DiscordTracker._activity = new Activity()
      {
        State = LocalizedTextManager.getText("discord_browsing"),
        Assets = {
          LargeImage = "worldboxlogo",
          LargeText = "WorldBox"
        },
        Timestamps = {
          Start = (long) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds
        },
        Instance = true
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      DiscordTracker._activity_manager?.UpdateActivity(DiscordTracker._activity, DiscordTracker.\u003C\u003Ec.\u003C\u003E9__10_0 ?? (DiscordTracker.\u003C\u003Ec.\u003C\u003E9__10_0 = new ActivityManager.UpdateActivityHandler((object) DiscordTracker.\u003C\u003Ec.\u003C\u003E9, __methodptr(\u003CStart\u003Eb__10_0))));
    }
    catch (ResultException ex)
    {
      Debug.Log((object) "Disabling Discord Integration (Discord not running, or game not run as Administrator)");
      Debug.Log((object) ex);
      flag = true;
    }
    catch (Exception ex)
    {
      Debug.Log((object) "Disabling Discord Integration (Discord not running, or game not run as Administrator)");
      Debug.Log((object) ex);
      flag = true;
    }
    if (!flag)
      return;
    Object.Destroy((Object) DiscordTracker._instance);
  }

  private static void tryGetUser()
  {
    try
    {
      --DiscordTracker._user_tries;
      User currentUser = DiscordTracker._discord.GetUserManager().GetCurrentUser();
      string str = currentUser.Id.ToString();
      if (!string.IsNullOrEmpty(str))
      {
        Config.discordId = str;
        RestClient.DefaultRequestHeaders["wb-dsc"] = str;
        DiscordTracker._have_user = true;
        Debug.Log((object) ("D:" + Config.discordId));
      }
      else
        Debug.Log((object) "D:nf");
      string username = currentUser.Username;
      if (!string.IsNullOrEmpty(username))
        Config.discordName = username;
      string discriminator = currentUser.Discriminator;
      if (!string.IsNullOrEmpty(discriminator))
        Config.discordDiscriminator = discriminator;
      VersionCheck.checkVersion();
    }
    catch (Exception ex)
    {
      Debug.Log((object) "D:F");
    }
  }

  private void Update()
  {
    if (!this._initiated)
      return;
    try
    {
      DiscordTracker._discord.RunCallbacks();
    }
    catch (Exception ex)
    {
      Debug.Log((object) "Disabling Discord");
      Debug.Log((object) ex);
      Object.Destroy((Object) DiscordTracker._instance);
      return;
    }
    if ((double) DiscordTracker._timer > 0.0)
    {
      DiscordTracker._timer -= Time.deltaTime;
    }
    else
    {
      DiscordTracker._timer = 10f;
      try
      {
        if (!DiscordTracker._have_user && DiscordTracker._user_tries > 0)
          DiscordTracker.tryGetUser();
        this.updateDetails(PowerTracker.activeStat);
      }
      catch (Exception ex)
      {
        Debug.Log((object) "Disabling Discord");
        Debug.Log((object) ex);
        Object.Destroy((Object) DiscordTracker._instance);
      }
    }
  }

  private void OnDisable() => DiscordTracker._discord?.Dispose();

  private void OnDestroy()
  {
    DiscordTracker._instance = (DiscordTracker) null;
    DiscordTracker._activity_manager = (ActivityManager) null;
    PowerTracker.discordTracker = (DiscordTracker) null;
  }

  public void trackViewing(string pString)
  {
    if (Object.op_Equality((Object) DiscordTracker._instance, (Object) null))
      return;
    if (pString != "" && LocalizedTextManager.stringExists(pString))
    {
      pString = LocalizedTextManager.getText("discord_viewing").Replace("$window$", LocalizedTextManager.getText(pString));
    }
    else
    {
      if (pString != "")
        Debug.Log((object) ("Missing translation for " + pString));
      pString = LocalizedTextManager.getText("discord_browsing");
    }
    this.trackActivity(pString);
  }

  public void trackWatching()
  {
    if (Object.op_Equality((Object) DiscordTracker._instance, (Object) null))
      return;
    this.trackActivity(LocalizedTextManager.getText("discord_watching"));
  }

  public void trackUsing(string pPower)
  {
    if (Object.op_Equality((Object) DiscordTracker._instance, (Object) null))
      return;
    this.trackActivity(LocalizedTextManager.getText("discord_using").Replace("$power$", LocalizedTextManager.getText(pPower)));
  }

  public void updateUsing(int pAmount, string pPower = "")
  {
    this.trackActivity($"{LocalizedTextManager.getText(pPower)} ({pAmount.ToString()})");
  }

  public void inspectKingdom(string pKingdom)
  {
    this.trackActivity($"{LocalizedTextManager.getText("village_statistics_kingdom")}: {pKingdom}");
  }

  public void inspectVillage(string pVillage)
  {
    this.trackActivity($"{LocalizedTextManager.getText("village")}: {pVillage}");
  }

  public void inspectUnit(string pUnit) => this.trackActivity($"{"inspect".Localize()}: {pUnit}");

  public void spectatingUnit(string pUnit)
  {
    this.trackActivity(LocalizedTextManager.getText("tip_following_unit").Replace("$name$", pUnit));
  }

  public void trackActivity(string pState = "")
  {
    if (Object.op_Equality((Object) DiscordTracker._instance, (Object) null))
      return;
    DiscordTracker._activity.State = pState;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: method pointer
    DiscordTracker._activity_manager?.UpdateActivity(DiscordTracker._activity, DiscordTracker.\u003C\u003Ec.\u003C\u003E9__23_0 ?? (DiscordTracker.\u003C\u003Ec.\u003C\u003E9__23_0 = new ActivityManager.UpdateActivityHandler((object) DiscordTracker.\u003C\u003Ec.\u003C\u003E9, __methodptr(\u003CtrackActivity\u003Eb__23_0))));
  }

  public void updateDetails(StatisticsAsset pStat)
  {
    if (Object.op_Equality((Object) DiscordTracker._instance, (Object) null))
      return;
    string localeId = pStat.getLocaleID();
    DiscordTracker._activity.Details = string.IsNullOrEmpty(localeId) ? pStat.last_value : $"{LocalizedTextManager.getText(localeId)}: {pStat.last_value}";
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: method pointer
    DiscordTracker._activity_manager?.UpdateActivity(DiscordTracker._activity, DiscordTracker.\u003C\u003Ec.\u003C\u003E9__24_0 ?? (DiscordTracker.\u003C\u003Ec.\u003C\u003E9__24_0 = new ActivityManager.UpdateActivityHandler((object) DiscordTracker.\u003C\u003Ec.\u003C\u003E9, __methodptr(\u003CupdateDetails\u003Eb__24_0))));
  }
}

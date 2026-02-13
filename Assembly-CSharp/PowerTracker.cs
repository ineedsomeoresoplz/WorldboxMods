// Decompiled with JetBrains decompiler
// Type: PowerTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
internal class PowerTracker : MonoBehaviour
{
  private static int amount = 0;
  private static PowerTracker instance;
  private static bool frameDone = false;
  internal static SteamTracker steamTracker;
  internal static DiscordTracker discordTracker;
  private static bool initiated = false;
  internal static StatisticsAsset activeStat;
  internal static string statValue = "";
  private static float timer = 10f;
  private static float secTimer = 1f;
  private static int currentIndex = 0;

  private static List<StatisticsAsset> rotateStats => StatisticsLibrary.power_tracker_pool;

  private void Start()
  {
    PowerTracker.instance = this;
    if (!Config.disable_discord)
      PowerTracker.discordTracker = ((Component) this).gameObject.AddComponent<DiscordTracker>();
    if (!Config.disable_steam)
      PowerTracker.steamTracker = ((Component) this).gameObject.AddComponent<SteamTracker>();
    PowerTracker.initiated = true;
  }

  internal static void PlusOne(GodPower pPower = null)
  {
    if (Object.op_Equality((Object) PowerTracker.instance, (Object) null) || MoveCamera.hasFocusUnit() || pPower == null)
      return;
    PowerTracker.frameDone = true;
    ++PowerTracker.amount;
    PowerTracker.discordTracker?.updateUsing(PowerTracker.amount, pPower.getLocaleID());
    PowerTracker.steamTracker?.updateUsing(PowerTracker.amount, pPower.getLocaleID());
  }

  internal static void trackPower(string pString = "")
  {
    if (Object.op_Equality((Object) PowerTracker.instance, (Object) null) || MoveCamera.hasFocusUnit())
      return;
    switch (pString)
    {
      case "Button Close":
        break;
      case "ButtonLeader":
        break;
      case "ButtonKingdom":
        break;
      case "ButtonCapital":
        break;
      default:
        PowerTracker.amount = 0;
        if (PowerTracker.frameDone)
          break;
        PowerTracker.steamTracker?.trackViewing(pString);
        PowerTracker.discordTracker?.trackViewing(pString);
        break;
    }
  }

  internal static void setPower(GodPower pPower)
  {
    if (Object.op_Equality((Object) PowerTracker.instance, (Object) null) || MoveCamera.hasFocusUnit())
      return;
    PowerTracker.frameDone = true;
    if (pPower == null)
    {
      PowerTracker.trackWatching();
    }
    else
    {
      if (!pPower.track_activity)
        return;
      if (LocalizedTextManager.stringExists(pPower.getLocaleID()))
      {
        PowerTracker.discordTracker?.trackUsing(pPower.getLocaleID());
        PowerTracker.steamTracker?.trackUsing(pPower.getLocaleID());
      }
    }
    PowerTracker.amount = 0;
  }

  internal static void trackWatching()
  {
    if (Object.op_Equality((Object) PowerTracker.instance, (Object) null))
      return;
    PowerTracker.discordTracker?.trackWatching();
    PowerTracker.steamTracker?.trackWatching();
  }

  internal static void spectatingUnit(string pUnit)
  {
    if (Object.op_Equality((Object) PowerTracker.instance, (Object) null))
      return;
    PowerTracker.frameDone = true;
    PowerTracker.steamTracker?.spectatingUnit(pUnit);
    PowerTracker.discordTracker?.spectatingUnit(pUnit);
  }

  internal static void trackWindow(string screen_id, ScrollWindow pWindow)
  {
    if (Object.op_Equality((Object) PowerTracker.instance, (Object) null))
      return;
    PowerTracker.frameDone = true;
    switch (screen_id)
    {
      case "":
        break;
      case "kingdom":
        PowerTracker.steamTracker?.inspectKingdom(SelectedMetas.selected_kingdom.name);
        PowerTracker.discordTracker?.inspectKingdom(SelectedMetas.selected_kingdom.name);
        break;
      case "city":
        PowerTracker.steamTracker?.inspectVillage(SelectedMetas.selected_city.name);
        PowerTracker.discordTracker?.inspectVillage(SelectedMetas.selected_city.name);
        break;
      case "unit":
        PowerTracker.steamTracker?.inspectUnit(SelectedUnit.unit.getName());
        PowerTracker.discordTracker?.inspectUnit(SelectedUnit.unit.getName());
        break;
      default:
        Transform recursive = ((Component) pWindow).transform.Find("Background/Title");
        if (Object.op_Equality((Object) recursive, (Object) null))
          recursive = ((Component) pWindow).transform.FindRecursive("Title");
        if (Object.op_Inequality((Object) recursive, (Object) null) && ((Component) recursive).HasComponent<LocalizedText>() && ((Component) recursive).GetComponent<LocalizedText>().key != "??????")
        {
          PowerTracker.steamTracker?.trackViewing(((Component) recursive).GetComponent<LocalizedText>().key);
          PowerTracker.discordTracker?.trackViewing(((Component) recursive).GetComponent<LocalizedText>().key);
          break;
        }
        Debug.Log((object) ("[PT] Not found " + screen_id));
        PowerTracker.steamTracker?.trackViewing(screen_id);
        PowerTracker.discordTracker?.trackViewing(screen_id);
        break;
    }
  }

  private static void resetTimer() => PowerTracker.timer = 9f;

  private static void nextStat()
  {
    PowerTracker.currentIndex = Randy.randomInt(0, PowerTracker.rotateStats.Count);
    if (PowerTracker.currentIndex < PowerTracker.rotateStats.Count)
      return;
    PowerTracker.currentIndex = 0;
  }

  private void updateStat()
  {
    if (Object.op_Equality((Object) PowerTracker.instance, (Object) null))
      return;
    StatisticsAsset rotateStat = PowerTracker.rotateStats[PowerTracker.currentIndex];
    string str = rotateStat.string_action(rotateStat);
    if (str != "0" && !string.IsNullOrEmpty(str))
    {
      rotateStat.last_value = str;
      PowerTracker.activeStat = rotateStat;
    }
    else
    {
      PowerTracker.nextStat();
      this.updateStat();
    }
  }

  private void OnDestroy() => PowerTracker.instance = (PowerTracker) null;

  private void Update()
  {
    if (PowerTracker.initiated && Object.op_Equality((Object) PowerTracker.discordTracker, (Object) null) && Object.op_Equality((Object) PowerTracker.steamTracker, (Object) null))
    {
      Object.Destroy((Object) this);
      Debug.Log((object) "[PT] Destroying...");
    }
    else
    {
      if ((double) PowerTracker.secTimer > 0.0)
      {
        PowerTracker.secTimer -= Time.deltaTime;
      }
      else
      {
        PowerTracker.secTimer = 1f;
        this.updateStat();
      }
      if ((double) PowerTracker.timer > 0.0)
      {
        PowerTracker.timer -= Time.deltaTime;
      }
      else
      {
        PowerTracker.resetTimer();
        PowerTracker.nextStat();
      }
    }
  }

  private void LateUpdate() => PowerTracker.frameDone = false;
}

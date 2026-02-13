// Decompiled with JetBrains decompiler
// Type: RewardedAds
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class RewardedAds : MonoBehaviour
{
  internal static RewardedAds instance;
  internal static List<IWorldBoxAd> adProviders = new List<IWorldBoxAd>();
  internal static IWorldBoxAd adProvider;
  public static bool initiated = false;
  private float timeout;
  private int failed;
  private bool should_switch = true;
  private const int AD_TIMEOUT = 8;
  private const int AD_REQUEST_TIMEOUT = 10;
  private const int LOST_FOCUS_TIMEOUT = 3;
  private static string adType;
  public static string _debug = "";
  private static List<PowerButton> rewardPowers = new List<PowerButton>();
  private static List<PowerButton> unlockButtons = new List<PowerButton>();
  internal static bool _isShowing = false;

  public static string debug
  {
    get => RewardedAds._debug;
    set
    {
      RewardedAds._debug = value == null || value.Length <= 50 ? value : value.Substring(value.Length - 50, 50);
    }
  }

  private void Awake() => RewardedAds.instance = this;

  public void switchProvider()
  {
    if (!this.should_switch)
      return;
    this.should_switch = false;
    using (ListPool<IWorldBoxAd> list = new ListPool<IWorldBoxAd>(RewardedAds.adProviders.Count))
    {
      foreach (IWorldBoxAd adProvider in RewardedAds.adProviders)
      {
        if (adProvider.IsInitialized() && adProvider != RewardedAds.adProvider)
          list.Add(adProvider);
      }
      if (list.Count == 0)
      {
        foreach (IWorldBoxAd adProvider in RewardedAds.adProviders)
        {
          if (adProvider.IsInitialized())
            list.Add(adProvider);
        }
      }
      RewardedAds.adProvider = list.GetRandom<IWorldBoxAd>();
      RewardedAds.adProvider.Reset();
      this.log("Switched provider: " + RewardedAds.adProvider.GetProviderName());
    }
  }

  public void unloadAd()
  {
    RewardedAds.debug += "u_";
    RewardedAds.adProvider.KillAd();
    RewardedAds.debug += "u2_";
  }

  private void RequestRewardBasedVideo()
  {
    RewardedAds.debug += "h8_";
    this.timeout = 18f;
    this.unloadAd();
    this.switchProvider();
    RewardedAds.adProvider.RequestAd();
    RewardedAds.debug += "h9_";
  }

  private static void logEvent(string pEvent)
  {
    Analytics.LogEvent(pEvent);
    if (string.IsNullOrEmpty(RewardedAds.adType))
      return;
    Analytics.LogEvent($"{pEvent}_{RewardedAds.adType}");
  }

  private void log(string pString) => Debug.Log((object) $"<color=cyan>[R] {pString}</color>");

  private void adReset()
  {
    this.failed = 0;
    this.timeout = 2f;
  }

  private void adStarted()
  {
    this.failed = 0;
    this.timeout = 2f;
    RewardedAds.logEvent("ad_reward_started");
    RewardedAds._isShowing = true;
  }

  private void adFailed()
  {
    ++this.failed;
    this.timeout = (float) (8 * this.failed);
    RewardedAds.logEvent("ad_reward_failed");
    RewardedAds._isShowing = false;
    this.should_switch = this.failed > 1;
  }

  private void adFinished()
  {
    this.failed = 0;
    this.timeout = 2f;
    RewardedAds.logEvent("ad_reward_finished");
    RewardedAds._isShowing = false;
  }

  private PowerButton generateRandomReward() => (PowerButton) null;

  private bool hasRewardAvailable() => false;

  private void generateReward()
  {
  }

  internal static bool isReady()
  {
    return Config.adsInitialized && RewardedAds.initiated && RewardedAds.adProvider != null && RewardedAds.adProvider.IsReady();
  }

  public static bool hasAd()
  {
    return Config.adsInitialized && RewardedAds.initiated && RewardedAds.adProvider != null && RewardedAds.adProvider.HasAd();
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal static bool isShowing() => RewardedAds._isShowing;

  public void ShowRewardedAd(string pAdType = "")
  {
    RewardedAds.adType = pAdType;
    RewardedAds.debug += "h10_";
    if (RewardedAds.isReady())
    {
      this.log("Show rewarded video");
      this.log("Active ad provider: " + RewardedAds.adProvider.GetProviderName());
      RewardedAds.logEvent("ad_reward_start");
      RewardedAds.adProvider.ShowAd();
    }
    else
    {
      ScrollWindow.get("ad_loading_error").clickShow();
      RewardedAds.logEvent("ad_reward_loading_error");
    }
  }

  public static void trimTimeout()
  {
    if (Object.op_Equality((Object) RewardedAds.instance, (Object) null) || (double) RewardedAds.instance.timeout <= 2.0 || RewardedAds.instance.failed <= 0)
      return;
    RewardedAds.instance.timeout = 2f;
    RewardedAds.instance.failed = 0;
  }

  public void handleRewards()
  {
  }
}

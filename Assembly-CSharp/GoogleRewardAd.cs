// Decompiled with JetBrains decompiler
// Type: GoogleRewardAd
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Beebyte.Obfuscator;
using GoogleMobileAds.Api;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[ObfuscateLiterals]
public class GoogleRewardAd : IWorldBoxAd
{
  private RewardedAd rewardBasedVideo;
  private static int loaded = 0;
  private static int failed = 0;
  internal static int default_current = 2;
  private static int current = 2;
  private const int max_current = 3;
  private static string _admob_id = string.Empty;
  private bool started;
  private static string lastError = "";
  private static string lastID = "";

  public Action<string> logger { get; set; }

  public Action adResetCallback { get; set; }

  public Action adFailedCallback { get; set; }

  public Action adFinishedCallback { get; set; }

  public Action adStartedCallback { get; set; }

  public void Reset()
  {
    this.log("reset to " + GoogleRewardAd.default_current.ToString());
    GoogleRewardAd.current = GoogleRewardAd.default_current;
    GoogleRewardAd.failed = 0;
    GoogleRewardAd.loaded = 0;
    GoogleRewardAd.lastError = "";
    GoogleRewardAd.lastID = "";
  }

  private string getRewardAdUnitID()
  {
    this.log("[prerew] " + GoogleRewardAd.current.ToString());
    if (GoogleRewardAd.failed > 1 && GoogleRewardAd.loaded == 0)
    {
      GoogleRewardAd.failed = 0;
      ++GoogleRewardAd.current;
      GoogleRewardAd.current = Mathf.Clamp(GoogleRewardAd.current, 0, 3);
      this.log("Level " + GoogleRewardAd.current.ToString());
    }
    else if (GoogleRewardAd.loaded > 2 && GoogleRewardAd.current > 0)
    {
      --GoogleRewardAd.current;
      GoogleRewardAd.current = Mathf.Clamp(GoogleRewardAd.current, 0, 3);
      this.log("Level " + GoogleRewardAd.current.ToString());
    }
    return "unexpected_platform";
  }

  public void RequestAd()
  {
    if (!Config.isMobile || Config.hasPremium || this.rewardBasedVideo != null && !this.started)
      return;
    this.KillAd();
    this.started = false;
    GoogleRewardAd._admob_id = this.getRewardAdUnitID();
    AdRequest adRequest;
    if (Config.testAds)
    {
      this.log("Requesting Test Ad");
      adRequest = new AdRequest();
      List<string> stringList = new List<string>()
      {
        "38469EF1320047F75C548E8477B3583B",
        "6b80482efcca7c0f3f07a95f8be98fe6"
      };
      MobileAds.SetRequestConfiguration(new RequestConfiguration()
      {
        TestDeviceIds = stringList
      });
    }
    else
    {
      this.log("Requesting Ad");
      adRequest = new AdRequest();
    }
    RewardedAd.Load(GoogleRewardAd._admob_id, adRequest, (Action<RewardedAd, LoadAdError>) ((ad, error) => ThreadHelper.ExecuteInUpdate((Action) (() =>
    {
      if (error != null || ad == null)
      {
        this.log("Callback error");
        this.HandleRewardBasedVideoFailedToLoad(error);
      }
      else
      {
        this.HandleRewardBasedVideoLoaded();
        this.rewardBasedVideo = ad;
        this.rewardBasedVideo.OnAdFullScreenContentOpened += new Action(this.HandleRewardBasedVideoOpened);
        this.rewardBasedVideo.OnAdFullScreenContentFailed += new Action<AdError>(this.HandleRewardedAdFailedToShow);
        this.rewardBasedVideo.OnAdPaid += new Action<AdValue>(this.HandleOnPaidEvent);
        this.rewardBasedVideo.OnAdFullScreenContentClosed += new Action(this.HandleRewardBasedVideoClosed);
      }
    }))));
  }

  public void HandleRewardBasedVideoLoaded()
  {
    ThreadHelper.ExecuteInUpdate((Action) (() =>
    {
      ++GoogleRewardAd.loaded;
      GoogleRewardAd.failed = 0;
      this.log("Ad Loaded");
      RewardedAds.debug += "h1_";
    }));
  }

  public void HandleRewardBasedVideoFailedToLoad(LoadAdError pLoadAdError)
  {
    string tLoadError = "Failed to load ad";
    if (pLoadAdError != null)
      tLoadError = ((AdError) pLoadAdError).GetMessage();
    ThreadHelper.ExecuteInUpdate((Action) (() =>
    {
      GoogleRewardAd.loaded = 0;
      ++GoogleRewardAd.failed;
      if (GoogleRewardAd.lastError != tLoadError || GoogleRewardAd._admob_id != GoogleRewardAd.lastID)
      {
        this.log($"{GoogleRewardAd.current.ToString()} {GoogleRewardAd._admob_id}");
        this.log($"<color=red>Ad Failed to Load: {tLoadError}</color>");
        GoogleRewardAd.lastError = tLoadError;
        GoogleRewardAd.lastID = GoogleRewardAd._admob_id;
      }
      else
        this.log("<color=red>Ad Failed to Load</color>");
      this.started = true;
      RewardedAds.debug += "h2_";
      this.KillAd();
      if (this.adFailedCallback != null)
        this.adFailedCallback();
      if (!tLoadError.Contains("floor") && !tLoadError.Contains("fill") && !tLoadError.Contains("configured"))
        return;
      ++GoogleRewardAd.failed;
      if (GoogleRewardAd.current >= 3 || this.adResetCallback == null)
        return;
      this.adResetCallback();
    }));
  }

  public void HandleRewardBasedVideoOpened()
  {
    ThreadHelper.ExecuteInUpdate((Action) (() =>
    {
      ++GoogleRewardAd.loaded;
      GoogleRewardAd.failed = 0;
      this.log("Ad Opened");
      this.started = true;
      RewardedAds.debug += "h3_";
      if (this.adStartedCallback == null)
        return;
      this.adStartedCallback();
    }));
  }

  public void HandleRewardedAdFailedToShow(AdError pAdError)
  {
    string tLoadError = "Failed to show ad";
    if (pAdError != null)
      tLoadError = pAdError.GetMessage();
    ThreadHelper.ExecuteInUpdate((Action) (() =>
    {
      GoogleRewardAd.loaded = 0;
      ++GoogleRewardAd.failed;
      if (GoogleRewardAd.lastError != tLoadError)
      {
        this.log($"<color=red>Ad Failed to Show: {tLoadError}</color>");
        GoogleRewardAd.lastError = tLoadError;
      }
      else
        this.log("<color=red>Ad Failed to Show</color>");
      this.started = true;
      RewardedAds.debug += "h4_";
      this.KillAd();
      if (this.adFailedCallback == null)
        return;
      this.adFailedCallback();
    }));
  }

  public void HandleRewardBasedVideoRewarded(Reward pAdReward)
  {
    ThreadHelper.ExecuteInUpdate((Action) (() =>
    {
      ++GoogleRewardAd.loaded;
      GoogleRewardAd.failed = 0;
      this.log("Ad Rewarded");
      this.started = true;
      if (Object.op_Inequality((Object) World.world, (Object) null))
        this.log("is worldbox on focus " + World.world.has_focus.ToString());
      RewardedAds.instance.handleRewards();
      RewardedAds.debug += "h5_";
    }));
  }

  public void HandleRewardBasedVideoClosed()
  {
    ThreadHelper.ExecuteInUpdate((Action) (() =>
    {
      ++GoogleRewardAd.loaded;
      GoogleRewardAd.failed = 0;
      this.log("Ad Closed");
      this.started = true;
      RewardedAds.debug += "h6_";
      this.KillAd();
      if (this.adFinishedCallback == null)
        return;
      this.adFinishedCallback();
    }));
  }

  public void HandleOnPaidEvent(AdValue pAdValue)
  {
    string tLog1 = "Rewarded interstitial ad has received a paid event. " + pAdValue.ToString();
    string tLog2 = $"Values: {pAdValue.Precision.ToString()} {pAdValue.Value.ToString()} {pAdValue.CurrencyCode}";
    ThreadHelper.ExecuteInUpdate((Action) (() =>
    {
      ++GoogleRewardAd.loaded;
      GoogleRewardAd.failed = 0;
      this.log(tLog1);
      this.log(tLog2);
      this.started = true;
      RewardedAds.debug += "h7_";
    }));
  }

  public void KillAd()
  {
    if (this.rewardBasedVideo == null || !this.started)
      return;
    this.rewardBasedVideo.OnAdFullScreenContentOpened -= new Action(this.HandleRewardBasedVideoOpened);
    this.rewardBasedVideo.OnAdFullScreenContentFailed -= new Action<AdError>(this.HandleRewardedAdFailedToShow);
    this.rewardBasedVideo.OnAdPaid -= new Action<AdValue>(this.HandleOnPaidEvent);
    this.rewardBasedVideo.OnAdFullScreenContentClosed -= new Action(this.HandleRewardBasedVideoClosed);
    this.rewardBasedVideo.Destroy();
    this.rewardBasedVideo = (RewardedAd) null;
  }

  public bool IsReady() => this.rewardBasedVideo != null && this.rewardBasedVideo.CanShowAd();

  public void ShowAd()
  {
    if (!this.IsReady())
      return;
    this.started = true;
    this.rewardBasedVideo.Show(new Action<Reward>(this.HandleRewardBasedVideoRewarded));
  }

  public bool HasAd() => this.IsInitialized() && this.rewardBasedVideo != null;

  public string GetProviderName() => "AdMob Rewarded Ad";

  public string GetColor() => GoogleMobileAdsLoader.GetColor();

  private void log(string pLog) => this.logger($"{this.GetColor()} {pLog}");

  public bool IsInitialized() => GoogleMobileAdsLoader.initialized;
}

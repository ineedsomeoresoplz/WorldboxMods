// Decompiled with JetBrains decompiler
// Type: IronSourceRewardAd
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Beebyte.Obfuscator;
using com.unity3d.mediation;
using System;
using Unity.Services.LevelPlay;
using UnityEngine;

#nullable disable
[ObfuscateLiterals]
public class IronSourceRewardAd : IWorldBoxAd
{
  private const string LEVELPLAY_REWARDED = "none";
  private static int loaded = 0;
  private static int failed = 0;
  private static bool initialized = false;
  private static LevelPlayRewardedAd _rewarded_ad;
  private static bool started = false;
  private static string lastError = "";

  public Action<string> logger { get; set; }

  public Action adResetCallback { get; set; }

  public Action adFailedCallback { get; set; }

  public Action adFinishedCallback { get; set; }

  public Action adStartedCallback { get; set; }

  public void Reset()
  {
    IronSourceRewardAd.failed = 0;
    IronSourceRewardAd.loaded = 0;
    IronSourceRewardAd.lastError = "";
  }

  public void RequestAd()
  {
    if (!Config.isMobile || Config.hasPremium || !IronSourceMobileAdsLoader.initialized || this.HasAd() && !IronSourceRewardAd.started)
      return;
    this.KillAd();
    IronSourceRewardAd.started = false;
    if (!IronSourceRewardAd.initialized)
    {
      IronSourceRewardAd.initialized = true;
      IronSourceRewardAd._rewarded_ad = new LevelPlayRewardedAd("none");
      IronSourceRewardAd._rewarded_ad.OnAdLoaded += new Action<LevelPlayAdInfo>(this.HandleRewardedAdAvailable);
      IronSourceRewardAd._rewarded_ad.OnAdLoadFailed += new Action<LevelPlayAdError>(this.HandleRewardedAdUnavailable);
      IronSourceRewardAd._rewarded_ad.OnAdDisplayed += new Action<LevelPlayAdInfo>(this.HandleRewardBasedVideoOpened);
      IronSourceRewardAd._rewarded_ad.OnAdDisplayFailed += new Action<LevelPlayAdDisplayInfoError>(this.HandleRewardedAdFailedToShow);
      IronSourceRewardAd._rewarded_ad.OnAdRewarded += new Action<LevelPlayAdInfo, LevelPlayReward>(this.HandleRewardBasedVideoRewarded);
      IronSourceRewardAd._rewarded_ad.OnAdClosed += new Action<LevelPlayAdInfo>(this.HandleRewardBasedVideoClosed);
      IronSourceRewardAd._rewarded_ad.OnAdLoadFailed += new Action<LevelPlayAdError>(this.HandleRewardedAdFailedToLoad);
    }
    this.log("Requesting Ad");
    IronSourceRewardAd._rewarded_ad.LoadAd();
  }

  public void HandleRewardedAdAvailable(LevelPlayAdInfo pAdInfo)
  {
    ThreadHelper.ExecuteInUpdate((Action) (() =>
    {
      ++IronSourceRewardAd.loaded;
      IronSourceRewardAd.failed = 0;
      this.log("Ad Available");
    }));
  }

  public void HandleRewardedAdUnavailable(LevelPlayAdError pError)
  {
    ThreadHelper.ExecuteInUpdate((Action) (() => this.log("Ad Unavailable")));
  }

  public void HandleRewardBasedVideoOpened(LevelPlayAdInfo pAdInfo)
  {
    ThreadHelper.ExecuteInUpdate((Action) (() =>
    {
      IronSourceRewardAd.started = true;
      ++IronSourceRewardAd.loaded;
      IronSourceRewardAd.failed = 0;
      this.log("Ad Opened");
      RewardedAds.debug += "h3_";
      if (this.adStartedCallback == null)
        return;
      this.adStartedCallback();
    }));
  }

  public void HandleRewardedAdFailedToShow(LevelPlayAdDisplayInfoError pAdError)
  {
    string tLoadError = "Failed to show ad";
    if (pAdError != null)
      tLoadError = pAdError.ToString();
    ThreadHelper.ExecuteInUpdate((Action) (() =>
    {
      IronSourceRewardAd.started = true;
      IronSourceRewardAd.loaded = 0;
      ++IronSourceRewardAd.failed;
      if (IronSourceRewardAd.lastError != tLoadError)
      {
        this.log($"<color=red>Ad Failed to Show: {tLoadError}</color>");
        IronSourceRewardAd.lastError = tLoadError;
      }
      else
        this.log("<color=red>Ad Failed to Show</color>");
      RewardedAds.debug += "h4_";
      this.KillAd();
      if (this.adFailedCallback == null)
        return;
      this.adFailedCallback();
    }));
  }

  public void HandleRewardedAdFailedToLoad(LevelPlayAdError pAdError)
  {
    string tLoadError = "Failed to load ad";
    if (pAdError != null)
      tLoadError = pAdError.ToString();
    ThreadHelper.ExecuteInUpdate((Action) (() =>
    {
      IronSourceRewardAd.started = true;
      IronSourceRewardAd.loaded = 0;
      ++IronSourceRewardAd.failed;
      if (IronSourceRewardAd.lastError != tLoadError)
      {
        this.log($"<color=red>Ad Failed to Load: {tLoadError}</color>");
        IronSourceRewardAd.lastError = tLoadError;
      }
      else
        this.log("<color=red>Ad Failed to Load</color>");
      RewardedAds.debug += "h4_";
      this.KillAd();
      if (this.adFailedCallback == null)
        return;
      this.adFailedCallback();
    }));
  }

  public void HandleRewardBasedVideoRewarded(LevelPlayAdInfo pAdInfo, LevelPlayReward pReward)
  {
    ThreadHelper.ExecuteInUpdate((Action) (() =>
    {
      IronSourceRewardAd.started = true;
      ++IronSourceRewardAd.loaded;
      IronSourceRewardAd.failed = 0;
      this.log("Ad Rewarded");
      if (Object.op_Inequality((Object) World.world, (Object) null))
        this.log("is worldbox on focus " + World.world.has_focus.ToString());
      RewardedAds.instance.handleRewards();
      RewardedAds.debug += "h5_";
    }));
  }

  public void HandleRewardBasedVideoClosed(LevelPlayAdInfo pAdInfo)
  {
    ThreadHelper.ExecuteInUpdate((Action) (() =>
    {
      IronSourceRewardAd.started = true;
      ++IronSourceRewardAd.loaded;
      IronSourceRewardAd.failed = 0;
      this.log("Ad Closed");
      RewardedAds.debug += "h6_";
      this.KillAd();
      if (this.adFinishedCallback == null)
        return;
      this.adFinishedCallback();
    }));
  }

  public void KillAd()
  {
    int num = IronSourceRewardAd.started ? 1 : 0;
  }

  public bool IsReady()
  {
    if (!this.IsInitialized())
      return false;
    LevelPlayRewardedAd rewardedAd = IronSourceRewardAd._rewarded_ad;
    return rewardedAd != null && rewardedAd.IsAdReady();
  }

  public void ShowAd()
  {
    if (!this.IsReady())
      return;
    IronSourceRewardAd.started = true;
    IronSourceRewardAd._rewarded_ad.ShowAd((string) null);
  }

  public bool HasAd() => this.IsInitialized() && this.IsReady();

  public string GetProviderName() => "IronSource Rewarded Ad";

  public string GetColor() => IronSourceMobileAdsLoader.GetColor();

  private void log(string pLog) => this.logger($"{this.GetColor()} {pLog}");

  public bool IsInitialized() => IronSourceMobileAdsLoader.initialized;

  public void showAdInfo(LevelPlayAdInfo pAdInfo)
  {
  }
}

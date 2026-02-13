// Decompiled with JetBrains decompiler
// Type: IronSourceInterstitialAd
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Beebyte.Obfuscator;
using com.unity3d.mediation;
using System;
using Unity.Services.LevelPlay;

#nullable disable
[ObfuscateLiterals]
public class IronSourceInterstitialAd : IWorldBoxAd
{
  private const string LEVELPLAY_INTERSTITIAL = "none";
  private static string lastError = "";
  private static int loaded = 0;
  private static int failed = 0;
  private static bool _initialized = false;
  private static LevelPlayInterstitialAd _interstitial_ad;

  public Action<string> logger { get; set; }

  public Action adResetCallback { get; set; }

  public Action adFailedCallback { get; set; }

  public Action adFinishedCallback { get; set; }

  public Action adStartedCallback { get; set; }

  public void Reset()
  {
    IronSourceInterstitialAd.failed = 0;
    IronSourceInterstitialAd.loaded = 0;
    IronSourceInterstitialAd.lastError = "";
  }

  public void RequestAd()
  {
    if (!Config.isMobile || Config.hasPremium || !IronSourceMobileAdsLoader.initialized)
      return;
    if (!IronSourceInterstitialAd._initialized)
    {
      IronSourceInterstitialAd._initialized = true;
      IronSourceInterstitialAd._interstitial_ad = new LevelPlayInterstitialAd("none");
      IronSourceInterstitialAd._interstitial_ad.OnAdLoaded += new Action<LevelPlayAdInfo>(this.HandleOnAdReady);
      IronSourceInterstitialAd._interstitial_ad.OnAdLoadFailed += new Action<LevelPlayAdError>(this.HandleOnAdFailedToLoad);
      IronSourceInterstitialAd._interstitial_ad.OnAdDisplayed += new Action<LevelPlayAdInfo>(this.HandleOnAdOpened);
      IronSourceInterstitialAd._interstitial_ad.OnAdDisplayFailed += new Action<LevelPlayAdDisplayInfoError>(this.HandleOnAdFailedToShow);
      IronSourceInterstitialAd._interstitial_ad.OnAdClosed += new Action<LevelPlayAdInfo>(this.HandleOnAdClosed);
    }
    this.KillAd();
    this.log("Requesting Ad");
    IronSourceInterstitialAd._interstitial_ad.LoadAd();
  }

  public void HandleOnAdLoaded()
  {
    ThreadHelper.ExecuteInUpdate((Action) (() =>
    {
      ++IronSourceInterstitialAd.loaded;
      IronSourceInterstitialAd.failed = 0;
      this.log("Ad Loaded");
    }));
  }

  public void HandleOnAdFailedToLoad(LevelPlayAdError pLoadAdError)
  {
    string tLoadError = "Failed to load ad";
    if (pLoadAdError != null)
      tLoadError = pLoadAdError.ToString();
    ThreadHelper.ExecuteInUpdate((Action) (() =>
    {
      IronSourceInterstitialAd.loaded = 0;
      ++IronSourceInterstitialAd.failed;
      if (IronSourceInterstitialAd.lastError != tLoadError)
      {
        this.log($"<color=red>Ad Failed to Load: {tLoadError}</color>");
        IronSourceInterstitialAd.lastError = tLoadError;
      }
      else
        this.log("<color=red>Ad Failed to Load</color>");
      this.KillAd();
      if (this.adFailedCallback == null)
        return;
      this.adFailedCallback();
    }));
  }

  public void HandleOnAdFailedToShow(LevelPlayAdDisplayInfoError pLoadAdInfoError)
  {
    LevelPlayAdInfo displayLevelPlayAdInfo = pLoadAdInfoError.DisplayLevelPlayAdInfo;
    LevelPlayAdError levelPlayError = pLoadAdInfoError.LevelPlayError;
    string tLoadError = "Failed to show ad";
    if (levelPlayError != null)
      tLoadError = levelPlayError.ToString();
    ThreadHelper.ExecuteInUpdate((Action) (() =>
    {
      IronSourceInterstitialAd.loaded = 0;
      ++IronSourceInterstitialAd.failed;
      if (IronSourceInterstitialAd.lastError != tLoadError)
      {
        this.log($"<color=red>Ad Failed to Show: {tLoadError}</color>");
        IronSourceInterstitialAd.lastError = tLoadError;
      }
      else
        this.log("<color=red>Ad Failed to Show</color>");
      this.KillAd();
      if (this.adFailedCallback == null)
        return;
      this.adFailedCallback();
    }));
  }

  public void HandleOnAdReady(LevelPlayAdInfo pAdInfo)
  {
    ThreadHelper.ExecuteInUpdate((Action) (() =>
    {
      ++IronSourceInterstitialAd.loaded;
      IronSourceInterstitialAd.failed = 0;
      this.log("Ad Ready");
    }));
  }

  public void HandleOnAdOpened(LevelPlayAdInfo pAdInfo)
  {
    ThreadHelper.ExecuteInUpdate((Action) (() =>
    {
      ++IronSourceInterstitialAd.loaded;
      IronSourceInterstitialAd.failed = 0;
      this.log("Ad Opened");
      if (this.adStartedCallback == null)
        return;
      this.adStartedCallback();
    }));
  }

  public void HandleOnAdClosed(LevelPlayAdInfo pAdInfo)
  {
    ThreadHelper.ExecuteInUpdate((Action) (() =>
    {
      ++IronSourceInterstitialAd.loaded;
      IronSourceInterstitialAd.failed = 0;
      this.log("Ad Closed");
      this.KillAd();
      if (this.adFinishedCallback == null)
        return;
      this.adFinishedCallback();
    }));
  }

  public void KillAd()
  {
  }

  public bool IsReady()
  {
    if (!IronSourceMobileAdsLoader.initialized)
      return false;
    LevelPlayInterstitialAd interstitialAd = IronSourceInterstitialAd._interstitial_ad;
    return interstitialAd != null && interstitialAd.IsAdReady();
  }

  public void ShowAd()
  {
    if (!this.IsReady())
      return;
    IronSourceInterstitialAd._interstitial_ad.ShowAd((string) null);
  }

  public bool HasAd()
  {
    if (!this.IsInitialized())
      return false;
    LevelPlayInterstitialAd interstitialAd = IronSourceInterstitialAd._interstitial_ad;
    return interstitialAd != null && interstitialAd.IsAdReady();
  }

  public string GetProviderName() => "IronSource Interstitial Ad";

  public string GetColor() => IronSourceMobileAdsLoader.GetColor();

  private void log(string pLog) => this.logger($"{this.GetColor()} {pLog}");

  public bool IsInitialized() => IronSourceMobileAdsLoader.initialized;

  public void showAdInfo(LevelPlayAdInfo pAdInfo)
  {
  }
}

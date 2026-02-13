// Decompiled with JetBrains decompiler
// Type: GoogleInterstitialAd
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
public class GoogleInterstitialAd : IWorldBoxAd
{
  private InterstitialAd interstitial;
  private static string _last_error = "";
  private static string _last_id = "";
  private static int _loaded = 0;
  private static int _failed = 0;
  internal static int default_current = 2;
  private static int _current = 2;
  private const int max_current = 3;
  private static string _admob_id = string.Empty;

  public Action<string> logger { get; set; }

  public Action adResetCallback { get; set; }

  public Action adFailedCallback { get; set; }

  public Action adFinishedCallback { get; set; }

  public Action adStartedCallback { get; set; }

  public void Reset()
  {
    this.log("reset to " + GoogleInterstitialAd.default_current.ToString());
    GoogleInterstitialAd._current = GoogleInterstitialAd.default_current;
    GoogleInterstitialAd._failed = 0;
    GoogleInterstitialAd._loaded = 0;
    GoogleInterstitialAd._last_error = "";
    GoogleInterstitialAd._last_id = "";
  }

  private string getInterstitialAdUnitID()
  {
    this.log("[preint] " + GoogleInterstitialAd._current.ToString());
    if (GoogleInterstitialAd._failed > 1 && GoogleInterstitialAd._loaded == 0)
    {
      GoogleInterstitialAd._failed = 0;
      ++GoogleInterstitialAd._current;
      GoogleInterstitialAd._current = Mathf.Clamp(GoogleInterstitialAd._current, 0, 3);
      this.log("Level " + GoogleInterstitialAd._current.ToString());
    }
    else if (GoogleInterstitialAd._loaded > 2 && GoogleInterstitialAd._current > 0)
    {
      --GoogleInterstitialAd._current;
      GoogleInterstitialAd._current = Mathf.Clamp(GoogleInterstitialAd._current, 0, 3);
      this.log("Level " + GoogleInterstitialAd._current.ToString());
    }
    return "unexpected_platform";
  }

  public void RequestAd()
  {
    if (!Config.isMobile || Config.hasPremium)
      return;
    GoogleInterstitialAd._admob_id = this.getInterstitialAdUnitID();
    this.KillAd();
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
    InterstitialAd.Load(GoogleInterstitialAd._admob_id, adRequest, (Action<InterstitialAd, LoadAdError>) ((ad, error) => ThreadHelper.ExecuteInUpdate((Action) (() =>
    {
      if (error != null || ad == null)
      {
        this.log("Callback error");
        this.HandleOnAdFailedToLoad(error);
      }
      else
      {
        this.HandleOnAdLoaded();
        this.interstitial = ad;
        this.interstitial.OnAdFullScreenContentOpened += new Action(this.HandleOnAdOpened);
        this.interstitial.OnAdFullScreenContentClosed += new Action(this.HandleOnAdClosed);
        this.interstitial.OnAdFullScreenContentFailed += new Action<AdError>(this.HandleOnAdFailed);
        this.interstitial.OnAdPaid += new Action<AdValue>(this.HandleOnPaidEvent);
      }
    }))));
  }

  public void HandleOnAdLoaded()
  {
    ThreadHelper.ExecuteInUpdate((Action) (() =>
    {
      ++GoogleInterstitialAd._loaded;
      GoogleInterstitialAd._failed = 0;
      this.log("Ad Loaded");
    }));
  }

  public void HandleOnAdFailedToLoad(LoadAdError pLoadAdError = null)
  {
    string tLoadError = "Failed to load ad";
    if (pLoadAdError != null)
      tLoadError = ((AdError) pLoadAdError).GetMessage();
    ThreadHelper.ExecuteInUpdate((Action) (() =>
    {
      GoogleInterstitialAd._loaded = 0;
      ++GoogleInterstitialAd._failed;
      if (GoogleInterstitialAd._last_error != tLoadError || GoogleInterstitialAd._admob_id != GoogleInterstitialAd._last_id)
      {
        this.log($"{GoogleInterstitialAd._current.ToString()} {GoogleInterstitialAd._admob_id}");
        this.log($"<color=red>Failed Load: {tLoadError}</color>");
        GoogleInterstitialAd._last_error = tLoadError;
        GoogleInterstitialAd._last_id = GoogleInterstitialAd._admob_id;
      }
      else
        this.log("<color=red>Failed Load</color>");
      this.KillAd();
      if (this.adFailedCallback != null)
        this.adFailedCallback();
      if (!tLoadError.Contains("floor") && !tLoadError.Contains("fill") && !tLoadError.Contains("configured"))
        return;
      ++GoogleInterstitialAd._failed;
      if (GoogleInterstitialAd._current >= 3 || this.adResetCallback == null)
        return;
      this.adResetCallback();
    }));
  }

  public void HandleOnAdFailed(AdError pLoadAdError)
  {
    string tLoadError = "Failed to show ad";
    if (pLoadAdError != null)
      tLoadError = pLoadAdError.GetMessage();
    ThreadHelper.ExecuteInUpdate((Action) (() =>
    {
      GoogleInterstitialAd._loaded = 0;
      ++GoogleInterstitialAd._failed;
      if (GoogleInterstitialAd._last_error != tLoadError || GoogleInterstitialAd._admob_id != GoogleInterstitialAd._last_id)
      {
        this.log($"{GoogleInterstitialAd._current.ToString()} {GoogleInterstitialAd._admob_id}");
        this.log($"<color=red>Ad Failed: {tLoadError}</color>");
        GoogleInterstitialAd._last_error = tLoadError;
        GoogleInterstitialAd._last_id = GoogleInterstitialAd._admob_id;
      }
      else
        this.log("<color=red>Ad Failed</color>");
      this.KillAd();
      if (this.adFailedCallback != null)
        this.adFailedCallback();
      if (!tLoadError.Contains("floor") && !tLoadError.Contains("fill") && !tLoadError.Contains("configured"))
        return;
      ++GoogleInterstitialAd._failed;
      if (GoogleInterstitialAd._current >= 3 || this.adResetCallback == null)
        return;
      this.adResetCallback();
    }));
  }

  public void HandleOnAdOpened()
  {
    ThreadHelper.ExecuteInUpdate((Action) (() =>
    {
      ++GoogleInterstitialAd._loaded;
      GoogleInterstitialAd._failed = 0;
      this.log("Ad Opened");
      if (this.adStartedCallback == null)
        return;
      this.adStartedCallback();
    }));
  }

  public void HandleOnAdClosed()
  {
    ThreadHelper.ExecuteInUpdate((Action) (() =>
    {
      ++GoogleInterstitialAd._loaded;
      GoogleInterstitialAd._failed = 0;
      this.log("Ad Closed");
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
      ++GoogleInterstitialAd._loaded;
      GoogleInterstitialAd._failed = 0;
      this.log(tLog1);
      this.log(tLog2);
    }));
  }

  public void KillAd()
  {
    if (this.interstitial == null)
      return;
    this.interstitial.OnAdFullScreenContentOpened -= new Action(this.HandleOnAdOpened);
    this.interstitial.OnAdFullScreenContentClosed -= new Action(this.HandleOnAdClosed);
    this.interstitial.OnAdPaid -= new Action<AdValue>(this.HandleOnPaidEvent);
    this.interstitial.Destroy();
    this.interstitial = (InterstitialAd) null;
  }

  public bool IsReady() => this.interstitial != null && this.interstitial.CanShowAd();

  public void ShowAd()
  {
    if (!this.IsReady())
      return;
    this.interstitial.Show();
  }

  public bool HasAd() => this.IsInitialized() && this.interstitial != null;

  public string GetProviderName() => "AdMob Interstitial Ad";

  public string GetColor() => GoogleMobileAdsLoader.GetColor();

  private void log(string pLog) => this.logger($"{this.GetColor()} {pLog}");

  public bool IsInitialized() => GoogleMobileAdsLoader.initialized;
}

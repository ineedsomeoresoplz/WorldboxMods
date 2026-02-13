// Decompiled with JetBrains decompiler
// Type: PlayInterstitialAd
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class PlayInterstitialAd : MonoBehaviour
{
  public static PlayInterstitialAd instance;
  internal static List<IWorldBoxAd> adProviders = new List<IWorldBoxAd>();
  public static IWorldBoxAd adProvider;
  public static bool initiated = false;
  public float timeout;
  private int failed;
  private bool should_switch = true;
  private const int AD_TIMEOUT = 8;
  private const int LOST_FOCUS_TIMEOUT = 3;
  internal static bool _isShowing = false;

  private void Awake() => PlayInterstitialAd.instance = this;

  private void Update()
  {
  }

  public void initAds()
  {
  }

  public void realInit()
  {
    PlayInterstitialAd.initiated = true;
    PlayInterstitialAd.adProviders.Add((IWorldBoxAd) new IronSourceInterstitialAd());
    PlayInterstitialAd.adProviders.Add((IWorldBoxAd) new GoogleInterstitialAd());
    foreach (IWorldBoxAd adProvider in PlayInterstitialAd.adProviders)
    {
      adProvider.adResetCallback = new Action(this.adReset);
      adProvider.adFinishedCallback = new Action(this.adFinished);
      adProvider.adFailedCallback = new Action(this.adFailed);
      adProvider.adStartedCallback = new Action(this.adStarted);
      adProvider.logger = new Action<string>(this.log);
    }
    this.switchProvider();
    this.scheduleAd(8f);
  }

  public void switchProvider()
  {
    if (!this.should_switch)
      return;
    this.should_switch = false;
    using (ListPool<IWorldBoxAd> list = new ListPool<IWorldBoxAd>(PlayInterstitialAd.adProviders.Count))
    {
      foreach (IWorldBoxAd adProvider in PlayInterstitialAd.adProviders)
      {
        if (adProvider.IsInitialized() && adProvider != PlayInterstitialAd.adProvider)
          list.Add(adProvider);
      }
      if (list.Count == 0)
      {
        foreach (IWorldBoxAd adProvider in PlayInterstitialAd.adProviders)
        {
          if (adProvider.IsInitialized())
            list.Add(adProvider);
        }
      }
      PlayInterstitialAd.adProvider = list.GetRandom<IWorldBoxAd>();
      PlayInterstitialAd.adProvider.Reset();
      this.log("Switched provider: " + PlayInterstitialAd.adProvider.GetProviderName());
    }
  }

  internal bool isReady() => false;

  public static bool hasAd() => PlayInterstitialAd.adProvider.HasAd();

  internal void showAd()
  {
    this.log("Show interstitial ad");
    this.log("Active ad provider: " + PlayInterstitialAd.adProvider.GetProviderName());
    MonoBehaviour.print((object) ("- Show interstitial ad " + this.isReady().ToString()));
    PlayInterstitialAd.logEvent("interstitial_ad_show");
    PlayInterstitialAd.adProvider.ShowAd();
  }

  public static void forceShowAd()
  {
    try
    {
      PlayInterstitialAd.logEvent("interstitial_ad_force_show");
      if (!PlayInterstitialAd.initiated)
      {
        PlayInterstitialAd.instance.realInit();
        PlayInterstitialAd.adProvider.RequestAd();
      }
      if (PlayInterstitialAd.adProvider.IsReady())
        PlayInterstitialAd.adProvider.ShowAd();
      else
        PlayInterstitialAd.adProvider.RequestAd();
    }
    catch (Exception ex)
    {
    }
  }

  private void scheduleAd(float pTimer = 60f)
  {
    if ((double) this.timeout > 0.0)
      return;
    PlayInterstitialAd.adProvider.KillAd();
    this.timeout = pTimer;
    this.switchProvider();
  }

  private static void logEvent(string pEvent) => Analytics.LogEvent(pEvent);

  private void log(string pString) => Debug.Log((object) $"<color=yellow>[I] {pString}</color>");

  private void adReset()
  {
    this.failed = 0;
    this.timeout = 2f;
  }

  private void adStarted()
  {
    this.failed = 0;
    this.timeout = 8f;
    PlayInterstitialAd.logEvent("interstitial_ad_started");
    PlayInterstitialAd._isShowing = true;
  }

  private void adFailed()
  {
    ++this.failed;
    this.timeout = (float) (8 * this.failed);
    PlayInterstitialAd.logEvent("interstitial_ad_failed");
    PlayInterstitialAd._isShowing = false;
    this.should_switch = this.failed > 1;
  }

  private void adFinished()
  {
    this.failed = 0;
    this.timeout = 8f;
    PlayInterstitialAd.logEvent("interstitial_ad_finished");
    PlayInterstitialAd._isShowing = false;
  }

  internal static bool isShowing() => PlayInterstitialAd._isShowing;

  internal static void setActive(bool pActive = false)
  {
    if (Object.op_Equality((Object) PlayInterstitialAd.instance, (Object) null))
      PlayInterstitialAd.instance = GameObject.Find("Services").GetComponentInChildren<PlayInterstitialAd>(true);
    ((Component) PlayInterstitialAd.instance).gameObject.SetActive(pActive);
  }
}

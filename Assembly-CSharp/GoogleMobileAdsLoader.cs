// Decompiled with JetBrains decompiler
// Type: GoogleMobileAdsLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Beebyte.Obfuscator;
using GoogleMobileAds.Api;
using System;
using UnityEngine;

#nullable disable
[ObfuscateLiterals]
public class GoogleMobileAdsLoader : MonoBehaviour
{
  private static GoogleMobileAdsLoader instance;
  internal static bool initialized;

  public static void initAds()
  {
    if (Object.op_Inequality((Object) GoogleMobileAdsLoader.instance, (Object) null) || !GoogleMobileAdsLoader.shouldLoad())
      return;
    GameObject gameObject = new GameObject(nameof (GoogleMobileAdsLoader));
    ((Object) gameObject).hideFlags = (HideFlags) 52;
    Object.DontDestroyOnLoad((Object) gameObject);
    gameObject.transform.SetParent(GameObject.Find("Services").transform);
    GoogleMobileAdsLoader.instance = gameObject.AddComponent<GoogleMobileAdsLoader>();
  }

  public void Start()
  {
    if (DebugConfig.isOn(DebugOption.TestAds))
      Config.testAds = true;
    if (!Config.isMobile)
      return;
    if (Config.hasPremium)
      return;
    try
    {
      string region = PreciseLocale.GetRegion();
      if (region.ToLower().Contains("us") || region.ToLower().Contains("gb"))
      {
        GoogleInterstitialAd.default_current = 1;
        GoogleRewardAd.default_current = 1;
      }
      string currencyCode = PreciseLocale.GetCurrencyCode();
      if (!(currencyCode == "USD"))
      {
        if (!(currencyCode == "GBP"))
          goto label_11;
      }
      GoogleInterstitialAd.default_current = 1;
      GoogleRewardAd.default_current = 1;
    }
    catch (Exception ex)
    {
    }
label_11:
    try
    {
      GoogleMobileAdsLoader.log("Initializing");
      MobileAds.DisableMediationInitialization();
      MobileAds.Initialize((Action<InitializationStatus>) (_ => ThreadHelper.ExecuteInUpdate((Action) (() =>
      {
        GoogleMobileAdsLoader.log("Initialized");
        GoogleMobileAdsLoader.initialized = true;
        Config.adsInitialized = true;
      }))));
    }
    catch (Exception ex)
    {
      GoogleMobileAdsLoader.log("Could not initialize ads");
      Debug.Log((object) ex);
    }
  }

  private static void log(string pLog)
  {
    Debug.Log((object) $"{GoogleMobileAdsLoader.GetColor()} <color=#fbbc04>{pLog}</color>");
  }

  public static string GetColor()
  {
    return "[<color=#ea4335>A</color><color=#fbbc04>D</color><color=#4285f4>M</color>]";
  }

  public static bool shouldLoad() => false;
}

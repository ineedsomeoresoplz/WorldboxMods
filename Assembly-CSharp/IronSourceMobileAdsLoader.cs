// Decompiled with JetBrains decompiler
// Type: IronSourceMobileAdsLoader
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
public class IronSourceMobileAdsLoader : MonoBehaviour
{
  private const string APP_KEY = "unexpected_platform";
  private static IronSourceMobileAdsLoader instance;
  internal static bool initialized;

  public static void initAds()
  {
    if (Object.op_Inequality((Object) IronSourceMobileAdsLoader.instance, (Object) null))
      return;
    GameObject gameObject = new GameObject(nameof (IronSourceMobileAdsLoader));
    ((Object) gameObject).hideFlags = (HideFlags) 52;
    Object.DontDestroyOnLoad((Object) gameObject);
    gameObject.transform.SetParent(GameObject.Find("Services").transform);
    IronSourceMobileAdsLoader.instance = gameObject.AddComponent<IronSourceMobileAdsLoader>();
  }

  internal void Start()
  {
    if (DebugConfig.isOn(DebugOption.TestAds))
      Config.testAds = true;
    if (!Config.isMobile)
      return;
    if (Config.hasPremium)
      return;
    try
    {
      IronSourceMobileAdsLoader.log("Initializing");
      LevelPlayAdFormat[] levelPlayAdFormatArray = new LevelPlayAdFormat[1]
      {
        (LevelPlayAdFormat) 2
      };
      Unity.Services.LevelPlay.LevelPlay.OnInitSuccess += new Action<LevelPlayConfiguration>(this.SdkInitializationCompletedEvent);
      Unity.Services.LevelPlay.LevelPlay.OnInitFailed += new Action<LevelPlayInitError>(this.SdkInitializationFailedEvent);
      Unity.Services.LevelPlay.LevelPlay.Init("unexpected_platform", (string) null, levelPlayAdFormatArray);
      IronSourceMobileAdsLoader.log("Version " + Unity.Services.LevelPlay.LevelPlay.UnityVersion);
    }
    catch (Exception ex)
    {
      IronSourceMobileAdsLoader.log("Could not initialize ads");
      Debug.Log((object) ex);
    }
  }

  private void OnApplicationPause(bool isPaused)
  {
    IronSourceMobileAdsLoader.log("OnApplicationPause = " + isPaused.ToString());
  }

  private void SdkInitializationCompletedEvent(LevelPlayConfiguration pConfig)
  {
    ThreadHelper.ExecuteInUpdate((Action) (() =>
    {
      IronSourceMobileAdsLoader.log("Initialized");
      IronSourceMobileAdsLoader.initialized = true;
      Config.adsInitialized = true;
    }));
  }

  private void SdkInitializationFailedEvent(LevelPlayInitError pConfig)
  {
    ThreadHelper.ExecuteInUpdate((Action) (() =>
    {
      IronSourceMobileAdsLoader.log("Failed to initialize ads");
      IronSourceMobileAdsLoader.initialized = false;
    }));
  }

  private static void log(string pLog)
  {
    Debug.Log((object) $"{IronSourceMobileAdsLoader.GetColor()} <color=#abe0c3>{pLog}</color>");
  }

  public static string GetColor() => "[<color=#abe0c3>IS</color>]";
}

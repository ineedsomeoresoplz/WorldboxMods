// Decompiled with JetBrains decompiler
// Type: InitStuff
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using db;
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using Proyecto26;
using System;
using System.Threading.Tasks;
using UnityEngine;

#nullable disable
public class InitStuff : MonoBehaviour
{
  private static bool initiated = false;
  private static bool restinitiated = false;
  private float elapsedSeconds;
  public static float targetSeconds = 900f;
  private float checkInitTimeOut = 1f;
  private float servicesInitTimeOut = 3f;
  private float adsInitTimeOut = 8f;

  private void Start()
  {
    ThreadHelper.Initialize();
    InitStuff.initDB();
    InitStuff.initSteam();
  }

  public static void initRest()
  {
    if (InitStuff.restinitiated)
      return;
    InitStuff.restinitiated = true;
    try
    {
      RestClient.DefaultRequestHeaders["salt"] = RequestHelper.salt ?? "na";
      RestClient.DefaultRequestHeaders["wb-version"] = Application.version ?? "na";
      RestClient.DefaultRequestHeaders["wb-identifier"] = Application.identifier ?? "na";
      RestClient.DefaultRequestHeaders["wb-platform"] = Application.platform.ToString() ?? "na";
      RestClient.DefaultRequestHeaders["wb-language"] = LocalizedTextManager.instance.language ?? "na";
      RestClient.DefaultRequestHeaders["wb-prem"] = Config.hasPremium ? "y" : "n";
      RestClient.DefaultRequestHeaders["wb-build"] = Config.versionCodeText ?? "na";
      RestClient.DefaultRequestHeaders["wb-gen"] = Config.gen.HasValue ? (Config.gen.Value ? "y" : "n") : "na";
      RestClient.DefaultRequestHeaders["wb-git"] = Config.gitCodeText ?? "na";
    }
    catch (Exception ex)
    {
      Debug.Log((object) "RestClient initialization Error");
      Debug.Log((object) ex.Message);
    }
  }

  public static void initOnlineServices()
  {
    if (InitStuff.initiated)
      return;
    InitStuff.initiated = true;
    if (Config.isEditor)
      return;
    try
    {
      if (Config.firebaseEnabled)
        InitStuff.initFirebase();
    }
    catch (Exception ex)
    {
      Debug.Log((object) "Firebase Init Error");
      Debug.Log((object) ex.Message);
    }
    try
    {
      VersionCheck.checkVersion();
    }
    catch (Exception ex)
    {
      Debug.Log((object) "Version Error");
      Debug.Log((object) ex.Message);
    }
    InitStuff.initRichPresence();
  }

  private void Update()
  {
    if ((double) this.checkInitTimeOut > 0.0)
    {
      this.checkInitTimeOut -= Time.fixedDeltaTime;
      if ((double) this.checkInitTimeOut < 0.0)
      {
        if (Config.firebaseEnabled)
        {
          if (!Config.firebaseChecked)
          {
            try
            {
              InitStuff.checkFirebase();
            }
            catch (Exception ex)
            {
              Debug.Log((object) "Firebase Check Error");
              Debug.Log((object) ex.Message);
            }
          }
        }
        else
          Config.firebaseChecked = true;
      }
    }
    if (!Config.firebaseChecked)
      return;
    if (!InitStuff.restinitiated)
      InitStuff.initRest();
    if ((double) this.servicesInitTimeOut > 0.0)
    {
      this.servicesInitTimeOut -= Time.fixedDeltaTime;
      if ((double) this.servicesInitTimeOut < 0.0)
        InitStuff.initOnlineServices();
    }
    if (Config.firebaseInitiating)
      return;
    if ((double) this.adsInitTimeOut > 0.0)
    {
      this.adsInitTimeOut -= Time.fixedDeltaTime;
      if ((double) this.adsInitTimeOut < 0.0)
        InitAds.initAdProviders();
    }
    if ((double) this.elapsedSeconds <= (double) InitStuff.targetSeconds)
    {
      this.elapsedSeconds += Time.deltaTime;
    }
    else
    {
      this.elapsedSeconds = 0.0f;
      try
      {
        if (!Config.hasPremium && (double) InitStuff.targetSeconds == 900.0)
          return;
        VersionCheck.checkVersion();
      }
      catch (Exception ex)
      {
      }
    }
  }

  private static void checkFirebase()
  {
    Debug.Log((object) "Firebase Starting Check");
    Config.firebaseChecked = false;
    TaskExtension.ContinueWithOnMainThread<DependencyStatus>(FirebaseApp.CheckDependenciesAsync(), (Action<Task<DependencyStatus>>) (pTask =>
    {
      Debug.Log((object) "Firebase check continuing on thread");
      DependencyStatus dependencyStatus = pTask.Result;
      ThreadHelper.ExecuteInUpdate((Action) (() =>
      {
        Debug.Log((object) "Firebase check status");
        if (dependencyStatus != null)
        {
          Debug.Log((object) "Firebase is not available");
          Debug.Log((object) dependencyStatus);
        }
        else
        {
          Debug.Log((object) "Firebase is available");
          Debug.Log((object) dependencyStatus);
        }
        Config.firebaseChecked = true;
      }));
    }));
  }

  private static void initFirebase()
  {
    Debug.Log((object) "Firebase init");
    Config.firebaseInitiating = true;
    TaskExtension.ContinueWithOnMainThread<DependencyStatus>(FirebaseApp.CheckAndFixDependenciesAsync(), (Action<Task<DependencyStatus>>) (pTask =>
    {
      Debug.Log((object) "Firebase init continuing on thread");
      DependencyStatus dependencyStatus = pTask.Result;
      Debug.Log((object) dependencyStatus);
      ThreadHelper.ExecuteInUpdate((Action) (() =>
      {
        Debug.Log((object) "Firebase task status");
        if (dependencyStatus == null)
        {
          try
          {
            Config.firebaseInitiating = false;
            Config.firebase_available = true;
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLogin);
            Debug.Log((object) "Firebase loaded");
            FirebaseAnalytics.LogEvent("data", "installerName", Config.iname ?? "");
          }
          catch (Exception ex)
          {
            Debug.Log((object) "Firebase Error");
            Debug.Log((object) ex.Message);
            Config.authEnabled = false;
            Config.firebase_available = false;
            Config.firebaseInitiating = false;
          }
        }
        else
        {
          Debug.Log((object) $"Could not resolve all Firebase dependencies: {dependencyStatus}");
          Config.authEnabled = false;
          Config.firebase_available = false;
        }
      }));
    }));
  }

  private static void initDB()
  {
    GameObject gameObject = new GameObject("DB");
    ((Object) gameObject).hideFlags = (HideFlags) 52;
    Object.DontDestroyOnLoad((Object) gameObject);
    gameObject.AddComponent<DBManager>();
    gameObject.transform.SetParent(GameObject.Find("Services").transform);
  }

  private static void initRichPresence()
  {
    if (Config.disable_steam && Config.disable_discord)
      return;
    GameObject gameObject = new GameObject("PowerTracker");
    ((Object) gameObject).hideFlags = (HideFlags) 52;
    Object.DontDestroyOnLoad((Object) gameObject);
    gameObject.AddComponent<PowerTracker>();
    gameObject.transform.SetParent(GameObject.Find("Services").transform);
  }

  internal static void initSteam()
  {
    if (Config.disable_steam)
      return;
    GameObject gameObject = new GameObject("Steam");
    ((Object) gameObject).hideFlags = (HideFlags) 52;
    Object.DontDestroyOnLoad((Object) gameObject);
    gameObject.AddComponent<SteamSDK>();
    gameObject.transform.SetParent(GameObject.Find("Services").transform);
    SteamAchievements.InitAchievements();
  }
}

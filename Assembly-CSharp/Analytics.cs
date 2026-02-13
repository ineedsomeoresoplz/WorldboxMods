// Decompiled with JetBrains decompiler
// Type: Analytics
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Firebase.Analytics;
using System.Collections.Generic;

#nullable disable
public class Analytics
{
  private static Dictionary<string, string> _event_slugs = new Dictionary<string, string>();

  public static void trackWindow(string pName)
  {
    if (Config.isEditor || Config.isComputer)
      return;
    string pName1 = global::Analytics.slugify(pName);
    if (!Config.firebase_available)
      return;
    FirebaseAnalytics.LogEvent("open_window", "window_id", pName1);
    global::Analytics.logScreen("ScrollWindow", pName1);
  }

  public static void hideWindow() => global::Analytics.logScreen("GamePlay", "gameplay");

  public static void worldLoaded() => global::Analytics.logScreen("GamePlay", "gameplay");

  public static void worldLoading() => global::Analytics.logScreen("LoadingScreen", "loading");

  private static void logScreen(string pClass, string pName)
  {
    if (!Config.firebase_available)
      return;
    Parameter[] parameterArray = new Parameter[2]
    {
      new Parameter(FirebaseAnalytics.ParameterScreenClass, pClass),
      new Parameter(FirebaseAnalytics.ParameterScreenName, pName)
    };
    FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventScreenView, parameterArray);
  }

  public static void LogEvent(string pName, bool pFirebase = true, bool pFacebook = true)
  {
    if (Config.isEditor || Config.isComputer)
      return;
    MapBox world = World.world;
    int num;
    if (world == null)
    {
      num = 0;
    }
    else
    {
      bool? active = world.auto_tester?.active;
      bool flag = true;
      num = active.GetValueOrDefault() == flag & active.HasValue ? 1 : 0;
    }
    if (num != 0)
      return;
    string str = global::Analytics.slugify(pName);
    if (!(Config.firebase_available & pFirebase))
      return;
    FirebaseAnalytics.LogEvent(str);
  }

  public static void LogEvent(string pName, string parameterName, string parameterValue)
  {
    if (Config.isEditor || Config.isComputer)
      return;
    MapBox world = World.world;
    int num;
    if (world == null)
    {
      num = 0;
    }
    else
    {
      bool? active = world.auto_tester?.active;
      bool flag = true;
      num = active.GetValueOrDefault() == flag & active.HasValue ? 1 : 0;
    }
    if (num != 0)
      return;
    string str = global::Analytics.slugify(pName);
    if (!Config.firebase_available)
      return;
    FirebaseAnalytics.LogEvent(str, parameterName, parameterValue);
  }

  public static string slugify(string pPhrase)
  {
    string lower;
    if (!global::Analytics._event_slugs.TryGetValue(pPhrase, out lower))
    {
      lower = pPhrase.Trim().Replace(" ", "_").ToLower();
      global::Analytics._event_slugs[pPhrase] = lower;
    }
    return lower;
  }
}

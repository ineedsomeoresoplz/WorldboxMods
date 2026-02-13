// Decompiled with JetBrains decompiler
// Type: SteamAchievements
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using RSG;
using Steamworks;
using Steamworks.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
internal static class SteamAchievements
{
  private static Promise initialized = new Promise();
  private static HashSet<string> achievements_hashset = new HashSet<string>();

  public static void InitAchievements()
  {
    SteamSDK.steamInitialized.Then((Action) (() =>
    {
      foreach (Achievement achievement in SteamUserStats.Achievements)
      {
        if (((Achievement) ref achievement).State)
        {
          SteamAchievements.unlockAchievement(((Achievement) ref achievement).Identifier);
          if (!AchievementLibrary.isUnlocked(((Achievement) ref achievement).Identifier))
          {
            Debug.Log((object) ("Was unlocked in Steam already, unlocking in the game: " + ((Achievement) ref achievement).Identifier));
            AchievementLibrary.unlock(((Achievement) ref achievement).Identifier);
          }
        }
        if (!((Achievement) ref achievement).State && AchievementLibrary.isUnlocked(((Achievement) ref achievement).Identifier))
        {
          Debug.Log((object) ("Was not unlocked in Steam yet, unlocking: " + ((Achievement) ref achievement).Identifier));
          SteamAchievements.TriggerAchievement(((Achievement) ref achievement).Identifier);
        }
      }
      SteamAchievements.initialized.Resolve();
    })).Catch((Action<Exception>) (err =>
    {
      Debug.Log((object) "Error happened while getting Steam Achievement");
      Debug.Log((object) err);
      SteamAchievements.initialized.Reject(new Exception("Steam Achievements not available"));
    }));
  }

  public static void TriggerAchievement(string id)
  {
    if (SteamAchievements.isSteamAchievementUnlocked(id))
      return;
    SteamAchievements.initialized.Then((Action) (() =>
    {
      if (SteamAchievements.isSteamAchievementUnlocked(id))
        return;
      Debug.Log((object) ("Unlocking in Steam: " + id));
      Achievement achievement;
      // ISSUE: explicit constructor call
      ((Achievement) ref achievement).\u002Ector(id);
      ((Achievement) ref achievement).Trigger(true);
      SteamAchievements.unlockAchievement(id);
    }));
  }

  public static void unlockAchievement(string pName)
  {
    SteamAchievements.achievements_hashset.Add(pName);
  }

  public static bool isSteamAchievementUnlocked(string pName)
  {
    return SteamAchievements.achievements_hashset.Contains(pName);
  }
}

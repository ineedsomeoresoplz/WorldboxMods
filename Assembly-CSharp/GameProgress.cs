// Decompiled with JetBrains decompiler
// Type: GameProgress
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#nullable disable
public class GameProgress
{
  public static GameProgress instance;
  private string dataPath;
  internal GameProgressData data;

  public static void init()
  {
    if (GameProgress.instance != null)
      return;
    Debug.Log((object) "INIT Progress");
    GameProgress.instance = new GameProgress();
    GameProgress.instance.create();
  }

  public void create()
  {
    this.setNewDataPath();
    if (File.Exists(this.dataPath))
    {
      try
      {
        this.loadData();
      }
      catch (Exception ex)
      {
        this.initNewSave();
      }
    }
    else
      this.initNewSave();
  }

  private void setNewDataPath()
  {
    this.dataPath = Application.persistentDataPath + "/worldboxProgress";
  }

  private void initNewSave()
  {
    this.data = new GameProgressData();
    GameProgress.saveData();
  }

  public static bool unlockAchievement(string pName)
  {
    if (GameProgress.instance == null || string.IsNullOrEmpty(pName) || GameProgress.isAchievementUnlocked(pName))
      return false;
    GameProgress.instance.data.achievements.Add(pName);
    GameProgress.saveData();
    return true;
  }

  public static bool isAchievementUnlocked(string pName)
  {
    return GameProgress.instance != null && GameProgress.instance.data.achievements.Contains(pName);
  }

  public static void saveData()
  {
    JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
    {
      DefaultValueHandling = (DefaultValueHandling) 3,
      Formatting = (Formatting) 1
    };
    string pStringData = Toolbox.encode(JsonConvert.SerializeObject((object) GameProgress.instance.data, serializerSettings));
    Toolbox.WriteSafely("Game Progress", GameProgress.instance.dataPath, ref pStringData);
  }

  private void loadData()
  {
    if (!File.Exists(this.dataPath))
      return;
    string pString = File.ReadAllText(this.dataPath);
    try
    {
      string str = Toolbox.decode(pString);
      if (!string.IsNullOrEmpty(str))
        pString = str;
    }
    catch (Exception ex)
    {
    }
    try
    {
      this.data = JsonConvert.DeserializeObject<GameProgressData>(pString);
      this.data.setDefaultValues();
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ("Error loading game progress data from " + this.dataPath));
      Debug.LogError((object) ex);
      this.initNewSave();
    }
  }

  public void debugClearAllAchievements()
  {
    this.data.achievements.Clear();
    GameProgress.saveData();
  }

  public void unlockAllAchievements()
  {
    foreach (Asset asset in AssetManager.achievements.list)
      GameProgress.unlockAchievement(asset.id);
  }

  public void debugClearAll()
  {
    this.data.prepare();
    foreach (HashSet<string> allHashset in this.data.all_hashsets)
      allHashset.Clear();
    GameProgress.saveData();
  }

  public void debugUnlockAll()
  {
    foreach (BaseUnlockableAsset baseUnlockableAsset in AssetManager.traits.list)
      baseUnlockableAsset.unlock(false);
    foreach (BaseUnlockableAsset baseUnlockableAsset in AssetManager.culture_traits.list)
      baseUnlockableAsset.unlock(false);
    foreach (BaseUnlockableAsset baseUnlockableAsset in AssetManager.language_traits.list)
      baseUnlockableAsset.unlock(false);
    foreach (BaseUnlockableAsset baseUnlockableAsset in AssetManager.subspecies_traits.list)
      baseUnlockableAsset.unlock(false);
    foreach (BaseUnlockableAsset baseUnlockableAsset in AssetManager.clan_traits.list)
      baseUnlockableAsset.unlock(false);
    foreach (BaseUnlockableAsset baseUnlockableAsset in AssetManager.religion_traits.list)
      baseUnlockableAsset.unlock(false);
    foreach (BaseUnlockableAsset baseUnlockableAsset in AssetManager.kingdoms_traits.list)
      baseUnlockableAsset.unlock(false);
    foreach (BaseUnlockableAsset baseUnlockableAsset in AssetManager.items.list)
      baseUnlockableAsset.unlock(false);
    foreach (BaseUnlockableAsset baseUnlockableAsset in AssetManager.gene_library.list)
      baseUnlockableAsset.unlock(false);
    foreach (BaseUnlockableAsset baseUnlockableAsset in AssetManager.actor_library.list)
      baseUnlockableAsset.unlock(false);
    foreach (BaseUnlockableAsset baseUnlockableAsset in AssetManager.plots_library.list)
      baseUnlockableAsset.unlock(false);
    GameProgress.saveData();
  }
}

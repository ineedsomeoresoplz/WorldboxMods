// Decompiled with JetBrains decompiler
// Type: GameStats
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

#nullable disable
public class GameStats : MonoBehaviour
{
  internal GameStatsData data;
  private string dataPath;
  private WorldTimer saveTimer;

  private void Start()
  {
    this.dataPath = Application.persistentDataPath + "/stats.json";
    this.loadData();
    if (this.data == null)
      this.data = new GameStatsData();
    else
      this.checkDataForErrors();
    this.saveTimer = new WorldTimer(30f, new Action(this.saveData));
    ++this.data.gameLaunches;
  }

  internal bool goodForAds() => true;

  private void saveData()
  {
    string str1 = "Stats";
    bool flag = false;
    string dataPath = this.dataPath;
    string str2 = this.dataPath + ".tmp";
    try
    {
      if (!Directory.Exists(Application.persistentDataPath))
        Directory.CreateDirectory(Application.persistentDataPath);
    }
    catch (Exception ex)
    {
      WorldTip.showNow("Error creating directory to save stats in! Check console for details", false, "top");
      Debug.Log((object) ("Error creating directory: " + Application.persistentDataPath));
      Debug.Log((object) ex);
    }
    try
    {
      using (FileStream fileStream = new FileStream(str2, FileMode.Create, FileAccess.Write))
      {
        using (StreamWriter streamWriter = new StreamWriter((Stream) fileStream))
        {
          using (JsonWriter jsonWriter = (JsonWriter) new JsonTextWriter((TextWriter) streamWriter))
            new JsonSerializer().Serialize(jsonWriter, (object) this.data);
        }
      }
    }
    catch (IOException ex)
    {
      if (Toolbox.IsDiskFull(ex))
      {
        WorldTip.showNow($"Error saving {str1} : Disk full!", false, "top");
      }
      else
      {
        Debug.Log((object) $"Could not save {str1} due to hard drive / IO Error : ");
        Debug.Log((object) ex);
        WorldTip.showNow($"Error saving {str1} due to IOError! Check console for details", false, "top");
      }
      flag = true;
    }
    catch (Exception ex)
    {
      Debug.Log((object) $"Could not save {str1} due to error : ");
      Debug.Log((object) ex);
      WorldTip.showNow($"Error saving {str1}! Check console for errors", false, "top");
      flag = true;
    }
    if (flag)
    {
      if (File.Exists(str2))
        File.Delete(str2);
    }
    else
      Toolbox.MoveSafely(str2, dataPath);
    AchievementLibrary.life_is_a_sim.check();
  }

  private void checkDataForErrors()
  {
    if (double.IsNaN(this.data.gameTime) || double.IsInfinity(this.data.gameTime) || this.data.gameTime < 0.0)
    {
      Debug.Log((object) this.data.gameTime);
      Debug.LogError((object) "Game time is NaN or Infinity! Resetting to 0");
      this.data.gameTime = 0.0;
    }
    if (this.data.creaturesBorn >= 0L)
      return;
    this.data.creaturesBorn = Math.Max(0L, this.data.creaturesDied - this.data.creaturesCreated);
  }

  private void loadData()
  {
    if (!File.Exists(this.dataPath))
      return;
    try
    {
      using (FileStream fileStream = new FileStream(this.dataPath, FileMode.Open, FileAccess.Read))
      {
        using (StreamReader streamReader = new StreamReader((Stream) fileStream))
        {
          using (JsonReader jsonReader = (JsonReader) new JsonTextReader((TextReader) streamReader))
            this.data = new JsonSerializer().Deserialize<GameStatsData>(jsonReader);
        }
      }
    }
    catch (Exception ex)
    {
      Debug.Log((object) "exception caught when loading stats");
      Debug.LogError((object) ex);
    }
    if (this.data != null)
      return;
    Debug.LogError((object) "(!) stats not has been loaded");
  }

  public void updateStats(float pTime)
  {
    this.data.gameTime += (double) pTime;
    this.saveTimer.update();
  }
}

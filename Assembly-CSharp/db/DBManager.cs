// Decompiled with JetBrains decompiler
// Type: db.DBManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using SQLite;
using System;
using System.IO;
using UnityEngine;

#nullable disable
namespace db;

public class DBManager : MonoBehaviour
{
  private static SQLiteAsyncConnection _dbconn;
  private static string _dbpath;

  private static void resetDataPath()
  {
    DBManager._dbpath = Application.persistentDataPath + "/stats.s3db";
  }

  public static bool loadDBFrom(string pPath)
  {
    try
    {
      DBManager.closeDB();
      if (!File.Exists(pPath))
        return false;
      DBManager.resetDataPath();
      if (File.Exists(DBManager._dbpath))
        File.Delete(DBManager._dbpath);
      File.Copy(pPath, DBManager._dbpath);
      DBManager.openDB();
      return true;
    }
    catch (Exception ex)
    {
      Debug.Log((object) "[SQLITE] error loading db");
      Debug.LogError((object) ex);
      return false;
    }
  }

  public static void createDB()
  {
    try
    {
      DBManager.closeDB();
      DBManager.resetDataPath();
      if (File.Exists(DBManager._dbpath))
        File.Delete(DBManager._dbpath);
      Debug.Log((object) ("[SQLITE] new db " + DBManager._dbpath));
      DBManager.openDB();
    }
    catch (Exception ex)
    {
      Debug.Log((object) "[SQLITE] error creating db");
      Debug.Log((object) ex);
    }
  }

  public static void openDB()
  {
    if (Config.disable_db || DBManager._dbconn != null)
      return;
    DBManager._dbconn = new SQLiteAsyncConnection(DBManager._dbpath, (SQLiteOpenFlags) 32774, true);
    Debug.Log((object) ("[SQLITE] opening db " + DBManager._dbconn.LibVersionNumber.ToString()));
    DBManager._dbconn.Trace = false;
    SQLiteConnectionWithLock connection = DBManager._dbconn.GetConnection();
    using (connection.Lock())
    {
      ((SQLiteConnection) connection).ExecuteScalar<string>("PRAGMA temp_store=MEMORY;", Array.Empty<object>());
      ((SQLiteConnection) connection).ExecuteScalar<string>("PRAGMA synchronous=OFF;", Array.Empty<object>());
      ((SQLiteConnection) connection).ExecuteScalar<string>("PRAGMA cache_size=4000;", Array.Empty<object>());
      ((SQLiteConnection) connection).ExecuteScalar<string>("PRAGMA journal_mode=MEMORY;", Array.Empty<object>());
    }
  }

  public static SQLiteAsyncConnection getAsyncConnection()
  {
    DBManager.openDB();
    return DBManager._dbconn;
  }

  public static SQLiteConnectionWithLock getSyncConnection()
  {
    DBManager.openDB();
    return DBManager._dbconn.GetConnection();
  }

  public static void clearAndClose()
  {
    DBInserter.waitForAsync();
    DBInserter.clearCommands();
    DBManager.closeDB();
  }

  public static void closeDB()
  {
    if (Config.disable_db || DBManager._dbconn == null)
      return;
    Debug.Log((object) "[SQLITE] closing db");
    try
    {
      DBManager._dbconn.CloseAsync().WaitAndUnwrapException();
    }
    catch (Exception ex)
    {
      Debug.LogError((object) "[SQLITE] error closing db");
      Debug.LogError((object) ex);
    }
    DBManager._dbconn = (SQLiteAsyncConnection) null;
    Debug.Log((object) "[SQLITE] db closed");
  }

  private static void vacuum()
  {
    DBManager.openDB();
    SQLiteConnectionWithLock connection = DBManager._dbconn.GetConnection();
    using (connection.Lock())
      ((SQLiteConnection) connection).Execute(nameof (vacuum), Array.Empty<object>());
  }

  private static void backupTo(string pPath)
  {
    DBManager.openDB();
    SQLiteConnectionWithLock connection = DBManager._dbconn.GetConnection();
    using (connection.Lock())
      ((SQLiteConnection) connection).Backup(pPath, "main");
  }

  public static void saveToPath(string pPath)
  {
    if (File.Exists(pPath))
      File.Delete(pPath);
    if (Config.disable_db)
      return;
    string str1 = "Stats DB";
    string str2 = pPath + ".bak";
    bool flag = false;
    try
    {
      DBInserter.executeCommands();
      DBManager.vacuum();
      DBManager.backupTo(str2);
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
      if (!File.Exists(str2))
        return;
      File.Delete(str2);
    }
    else
      Toolbox.MoveSafely(str2, pPath);
  }

  private void Awake()
  {
    ScrollWindow.addCallbackShowStarted((ScrollWindowNameAction) (_ => DBInserter.executeCommands()));
  }

  private void OnApplicationQuit()
  {
    DBInserter.quitting();
    DBManager.closeDB();
  }
}

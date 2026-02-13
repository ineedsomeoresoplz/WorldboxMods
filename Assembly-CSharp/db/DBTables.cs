// Decompiled with JetBrains decompiler
// Type: db.DBTables
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using SQLite;
using System;
using UnityEngine;

#nullable disable
namespace db;

public static class DBTables
{
  public static void createOrMigrateTables()
  {
    DBTables.createTable<WorldLogMessage>();
    DBTables.createTable<KingdomData>();
  }

  public static void createOrMigrateTable(Type pType)
  {
    SQLiteConnectionWithLock syncConnection = DBManager.getSyncConnection();
    using (syncConnection.Lock())
    {
      if (((SQLiteConnection) syncConnection).CreateTable(pType, (CreateFlags) 0) == 1)
        return;
      TableMapping mapping = ((SQLiteConnection) syncConnection).GetMapping(pType, (CreateFlags) 0);
      string str1 = "SELECT sql FROM sqlite_master WHERE type='table' AND name=?";
      string str2 = ((SQLiteConnection) syncConnection).ExecuteScalar<string>(str1, new object[1]
      {
        (object) mapping.TableName
      });
      ((SQLiteConnection) syncConnection).DropTable(mapping);
      int length = str2.LastIndexOf(')');
      string str3 = str2.Substring(0, length).Replace(" integer ", " INT ").Trim().Replace("  ", " ").Replace(" ,", ",").Replace(", ", ",").Replace("\"", "") + ",\nauto INT" + ",\nPRIMARY KEY(id, timestamp)" + "\n)";
      ((SQLiteConnection) syncConnection).Execute(str3, Array.Empty<object>());
    }
  }

  public static void checkTablesOK(bool pDropTable = false)
  {
    int currentYear = Date.getCurrentYear();
    bool flag = true;
    SQLiteConnectionWithLock syncConnection = DBManager.getSyncConnection();
    using (syncConnection.Lock())
    {
      foreach (HistoryMetaDataAsset historyMetaDataAsset in AssetManager.history_meta_data_library.list)
      {
        if (flag)
        {
          foreach (Type pType in historyMetaDataAsset.table_types.Values)
          {
            if (DBTables.checkTableExists(pType) && !DBTables.checkTableOK(pType, currentYear))
            {
              flag = false;
              break;
            }
          }
        }
        else
          break;
      }
      if (flag)
        return;
      if (pDropTable)
      {
        Debug.Log((object) "Statistics have future data, dropping...");
        foreach (HistoryMetaDataAsset historyMetaDataAsset in AssetManager.history_meta_data_library.list)
        {
          foreach (Type pType in historyMetaDataAsset.table_types.Values)
          {
            if (DBTables.checkTableExists(pType))
            {
              TableMapping mapping = ((SQLiteConnection) syncConnection).GetMapping(pType, (CreateFlags) 0);
              ((SQLiteConnection) syncConnection).DropTable(mapping);
            }
          }
        }
        if (DBTables.checkTableExists<KingdomData>())
          ((SQLiteConnection) syncConnection).DropTable<KingdomData>();
        if (!DBTables.checkTableExists<WorldLogMessage>())
          return;
        ((SQLiteConnection) syncConnection).DropTable<WorldLogMessage>();
      }
      else
      {
        Debug.Log((object) "Statistics have future data, clearing...");
        foreach (HistoryMetaDataAsset historyMetaDataAsset in AssetManager.history_meta_data_library.list)
        {
          foreach (Type pType in historyMetaDataAsset.table_types.Values)
          {
            if (DBTables.checkTableExists(pType))
            {
              TableMapping mapping = ((SQLiteConnection) syncConnection).GetMapping(pType, (CreateFlags) 0);
              ((SQLiteConnection) syncConnection).DeleteAll(mapping);
            }
          }
        }
        if (DBTables.checkTableExists<KingdomData>())
          ((SQLiteConnection) syncConnection).DeleteAll<KingdomData>();
        if (!DBTables.checkTableExists<WorldLogMessage>())
          return;
        ((SQLiteConnection) syncConnection).DeleteAll<WorldLogMessage>();
      }
    }
  }

  public static bool checkTableExists(Type pType)
  {
    SQLiteConnectionWithLock syncConnection = DBManager.getSyncConnection();
    using (syncConnection.Lock())
    {
      TableMapping mapping = ((SQLiteConnection) syncConnection).GetMapping(pType, (CreateFlags) 0);
      string str = "SELECT count(1) FROM sqlite_master WHERE type='table' AND name=?";
      return ((SQLiteConnection) syncConnection).ExecuteScalar<int>(str, new object[1]
      {
        (object) mapping.TableName
      }) != 0;
    }
  }

  public static bool checkTableOK(Type pType, int pTimestamp)
  {
    SQLiteConnectionWithLock syncConnection = DBManager.getSyncConnection();
    using (syncConnection.Lock())
    {
      string str = $"SELECT count(1) FROM '{((SQLiteConnection) syncConnection).GetMapping(pType, (CreateFlags) 0).TableName}' WHERE timestamp>?";
      return ((SQLiteConnection) syncConnection).ExecuteScalar<int>(str, new object[1]
      {
        (object) pTimestamp
      }) == 0;
    }
  }

  public static bool checkTableExists<T>() => DBTables.checkTableExists(typeof (T));

  public static void createTableIfNotExists<T>()
  {
    if (DBTables.checkTableExists<T>())
      return;
    DBTables.createTable<T>();
  }

  public static void createTable<T>()
  {
    SQLiteConnectionWithLock syncConnection = DBManager.getSyncConnection();
    using (syncConnection.Lock())
      ((SQLiteConnection) syncConnection).CreateTable<T>((CreateFlags) 0);
  }

  public static void createOrMigrateTablesLoader(bool pCreating = true)
  {
    string str = pCreating ? "Creating" : "Migrating";
    if (!pCreating)
      SmoothLoader.add((MapLoaderAction) (() =>
      {
        SQLiteConnectionWithLock syncConnection = DBManager.getSyncConnection();
        foreach (HistoryMetaDataAsset tHistoryAsset in AssetManager.history_meta_data_library.list)
          DBTriggers.dropTrigger(syncConnection, tHistoryAsset);
      }), "Dropping Triggers");
    SmoothLoader.add((MapLoaderAction) (() => DBTables.createOrMigrateTables()), str + " Stats");
    foreach (HistoryMetaDataAsset historyMetaDataAsset in AssetManager.history_meta_data_library.list)
    {
      HistoryMetaDataAsset tHistoryAsset = historyMetaDataAsset;
      SmoothLoader.add((MapLoaderAction) (() =>
      {
        foreach (Type pType in tHistoryAsset.table_types.Values)
          DBTables.createOrMigrateTable(pType);
      }), $"{str} Stats ({tHistoryAsset.table_type.Name})");
    }
    SmoothLoader.add((MapLoaderAction) (() =>
    {
      SQLiteConnectionWithLock syncConnection = DBManager.getSyncConnection();
      foreach (HistoryMetaDataAsset tHistoryAsset in AssetManager.history_meta_data_library.list)
        DBTriggers.createTrigger(syncConnection, tHistoryAsset);
    }), str + " Triggers");
  }
}

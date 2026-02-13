// Decompiled with JetBrains decompiler
// Type: db.DBInserter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using db.tables;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

#nullable disable
namespace db;

public static class DBInserter
{
  internal static int _insert_commands_count = 0;
  internal static readonly List<(long MetaID, Type MetaType)> _delete_commands = new List<(long, Type)>(4096 /*0x1000*/);
  internal static readonly Dictionary<string, ListPool<HistoryTable>> _insert_commands = new Dictionary<string, ListPool<HistoryTable>>(64 /*0x40*/);
  internal static readonly Dictionary<string, ListPool<BaseSystemData>> _insert_metas = new Dictionary<string, ListPool<BaseSystemData>>(64 /*0x40*/);
  internal static readonly List<WorldLogMessage> _insert_logs = new List<WorldLogMessage>(64 /*0x40*/);
  private const float SQL_TIMEOUT_TIME = 10f;
  private static float _sql_timeout = 10f;
  private static Task _thread;
  private static bool _locked = false;

  public static void Lock() => DBInserter._locked = true;

  public static void Unlock() => DBInserter._locked = false;

  public static bool isLocked() => DBInserter._locked || Config.disable_db;

  public static void deleteData(long pID, string pMetaType)
  {
    if (DBInserter.isLocked())
      return;
    foreach (Type type in AssetManager.history_meta_data_library.get(pMetaType).table_types.Values)
      DBInserter._delete_commands.Add((pID, type));
  }

  public static void insertLog(WorldLogMessage pObject)
  {
    if (DBInserter.isLocked())
      return;
    DBInserter._insert_logs.Add(pObject);
    ++DBInserter._insert_commands_count;
  }

  public static void insertData(BaseSystemData pObject, string tMetaType)
  {
    if (DBInserter.isLocked())
      return;
    ListPool<BaseSystemData> listPool;
    if (!DBInserter._insert_metas.TryGetValue(tMetaType, out listPool))
    {
      listPool = new ListPool<BaseSystemData>();
      DBInserter._insert_metas.Add(tMetaType, listPool);
    }
    listPool.Add(pObject);
    ++DBInserter._insert_commands_count;
    if (!ScrollWindow.isWindowActive())
      return;
    DBInserter.executeCommands();
  }

  public static void insertData(HistoryTable pObject, string tMetaType)
  {
    if (DBInserter.isLocked())
      return;
    ListPool<HistoryTable> listPool;
    if (!DBInserter._insert_commands.TryGetValue(tMetaType, out listPool))
    {
      listPool = new ListPool<HistoryTable>();
      DBInserter._insert_commands.Add(tMetaType, listPool);
    }
    listPool.Add(pObject);
    ++DBInserter._insert_commands_count;
  }

  public static bool hasCommands()
  {
    return DBInserter._insert_commands_count > 0 || DBInserter._delete_commands.Count > 0;
  }

  public static void clearCommands()
  {
    DBInserter._insert_logs.Clear();
    DBInserter._insert_commands.Clear();
    DBInserter._insert_metas.Clear();
    DBInserter._insert_commands_count = 0;
    DBInserter._delete_commands.Clear();
  }

  public static void executeCommands()
  {
    DBInserter.waitForAsync();
    if (DBInserter.isLocked() || !DBInserter.hasCommands())
      return;
    SQLiteConnectionWithLock tDBConn = DBManager.getSyncConnection();
    using (tDBConn.Lock())
    {
      if (!DBInserter.hasCommands())
        return;
      ListPool<ListPool<BaseSystemData>> tMetasList = DBInserter._insert_metas.Values.Count > 0 ? new ListPool<ListPool<BaseSystemData>>(DBInserter._insert_metas.Values.Count) : (ListPool<ListPool<BaseSystemData>>) null;
      ListPool<ListPool<HistoryTable>> tCommandsList = DBInserter._insert_commands.Values.Count > 0 ? new ListPool<ListPool<HistoryTable>>(DBInserter._insert_commands.Values.Count) : (ListPool<ListPool<HistoryTable>>) null;
      ListPool<WorldLogMessage> tInsertLogsList = DBInserter._insert_logs.Count > 0 ? new ListPool<WorldLogMessage>((ICollection<WorldLogMessage>) DBInserter._insert_logs) : (ListPool<WorldLogMessage>) null;
      ListPool<(long, Type)> tDeleteCommandsList = DBInserter._delete_commands.Count > 0 ? new ListPool<(long, Type)>((ICollection<(long, Type)>) DBInserter._delete_commands) : (ListPool<(long, Type)>) null;
      foreach (ListPool<HistoryTable> listPool in DBInserter._insert_commands.Values)
      {
        if (listPool.Count == 0)
          listPool.Dispose();
        else
          tCommandsList.Add(listPool);
      }
      foreach (ListPool<BaseSystemData> listPool in DBInserter._insert_metas.Values)
      {
        if (listPool.Count == 0)
          listPool.Dispose();
        else
          tMetasList.Add(listPool);
      }
      DBInserter.clearCommands();
      ((SQLiteConnection) tDBConn).RunInTransaction((Action) (() => DBInserter.sendToDB((SQLiteConnection) tDBConn, tMetasList, tCommandsList, tDeleteCommandsList, tInsertLogsList)));
    }
  }

  public static void executeCommandsAsync()
  {
    if (DBInserter.isLocked())
      return;
    if ((double) DBInserter._sql_timeout > 0.0)
    {
      DBInserter._sql_timeout -= Time.deltaTime;
    }
    else
    {
      DBInserter._sql_timeout = 10f;
      if (DBInserter._thread != null && !DBInserter._thread.IsCompleted || !DBInserter.hasCommands())
        return;
      SQLiteAsyncConnection asyncConnection = DBManager.getAsyncConnection();
      if (!DBInserter.hasCommands())
        return;
      ListPool<ListPool<BaseSystemData>> tMetasList = DBInserter._insert_metas.Values.Count > 0 ? new ListPool<ListPool<BaseSystemData>>(DBInserter._insert_metas.Values.Count) : (ListPool<ListPool<BaseSystemData>>) null;
      ListPool<ListPool<HistoryTable>> tCommandsList = DBInserter._insert_commands.Values.Count > 0 ? new ListPool<ListPool<HistoryTable>>(DBInserter._insert_commands.Values.Count) : (ListPool<ListPool<HistoryTable>>) null;
      ListPool<WorldLogMessage> tInsertLogsList = DBInserter._insert_logs.Count > 0 ? new ListPool<WorldLogMessage>((ICollection<WorldLogMessage>) DBInserter._insert_logs) : (ListPool<WorldLogMessage>) null;
      ListPool<(long, Type)> tDeleteCommandsList = DBInserter._delete_commands.Count > 0 ? new ListPool<(long, Type)>((ICollection<(long, Type)>) DBInserter._delete_commands) : (ListPool<(long, Type)>) null;
      foreach (ListPool<HistoryTable> listPool in DBInserter._insert_commands.Values)
      {
        if (listPool.Count == 0)
          listPool.Dispose();
        else
          tCommandsList.Add(listPool);
      }
      foreach (ListPool<BaseSystemData> listPool in DBInserter._insert_metas.Values)
      {
        if (listPool.Count == 0)
          listPool.Dispose();
        else
          tMetasList.Add(listPool);
      }
      DBInserter.clearCommands();
      DBInserter._thread = asyncConnection.RunInTransactionAsync((Action<SQLiteConnection>) (pDBConn => DBInserter.sendToDB(pDBConn, tMetasList, tCommandsList, tDeleteCommandsList, tInsertLogsList)));
    }
  }

  private static void sendToDB(
    SQLiteConnection pDBConn,
    ListPool<ListPool<BaseSystemData>> tMetasList = null,
    ListPool<ListPool<HistoryTable>> tCommandsList = null,
    ListPool<(long MetaID, Type MetaType)> tDeleteCommandsList = null,
    ListPool<WorldLogMessage> tInsertLogsList = null)
  {
    // ISSUE: unable to decompile the method.
  }

  public static void quitting()
  {
    DBInserter._sql_timeout = float.MaxValue;
    DBInserter.waitForAsync();
  }

  public static void waitForAsync()
  {
    if (DBInserter._thread == null || DBInserter._thread.IsCompleted)
      return;
    Debug.Log((object) "DBInserter thread is still running");
    DBInserter._thread.WaitAndUnwrapException();
    Debug.Log((object) "DBInserter closed");
    DBInserter._thread = (Task) null;
  }
}

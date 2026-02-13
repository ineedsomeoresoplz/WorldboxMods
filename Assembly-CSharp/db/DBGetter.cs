// Decompiled with JetBrains decompiler
// Type: db.DBGetter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using db.tables;
using SQLite;
using System.Collections.Generic;
using UnityPools;

#nullable disable
namespace db;

public static class DBGetter
{
  public static ListPool<GraphTimeScale> getTimeScales(NanoObject pObject)
  {
    return Config.disable_db ? new ListPool<GraphTimeScale>() : DBGetter.getTimeScales(pObject.getID(), pObject.getType());
  }

  public static ListPool<GraphTimeScale> getTimeScales(long pID, string pMetaType)
  {
    // ISSUE: unable to decompile the method.
  }

  public static ListPool<WorldLogMessage> getWorldLogMessages()
  {
    DBInserter.executeCommands();
    SQLiteConnectionWithLock syncConnection = DBManager.getSyncConnection();
    using (syncConnection.Lock())
    {
      TableMapping mapping = ((SQLiteConnection) syncConnection).GetMapping<WorldLogMessage>((CreateFlags) 0);
      return ((SQLiteConnection) syncConnection).QueryPool<WorldLogMessage>($"select * from {mapping.TableName} order by timestamp DESC, ROWID DESC LIMIT {2000}");
    }
  }

  public static bool getData(
    CategoryData pData,
    NanoObject pObject,
    HistoryInterval pInterval,
    HistoryTable pExtraData)
  {
    return DBGetter.getData(pData, pObject.getID(), pObject.getType(), pInterval, pExtraData);
  }

  public static bool getData(
    CategoryData pData,
    long pID,
    string pMetaType,
    HistoryInterval pInterval,
    HistoryTable pExtraData)
  {
    // ISSUE: unable to decompile the method.
  }

  public static Dictionary<string, long?> parseValues(object pItem, TableMapping pTableMapping)
  {
    TableMapping.Column[] columns = pTableMapping.Columns;
    Dictionary<string, long?> values = UnsafeCollectionPool<Dictionary<string, long?>, KeyValuePair<string, long?>>.Get();
    foreach (TableMapping.Column column in columns)
    {
      if (!(column.Name == "id"))
      {
        object obj = column.GetValue(pItem);
        if (obj == null)
        {
          values.Add(column.Name, new long?());
        }
        else
        {
          long num = (long) obj;
          values.Add(column.Name, new long?(num));
        }
      }
    }
    return values;
  }
}

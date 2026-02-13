// Decompiled with JetBrains decompiler
// Type: db.DBTriggers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace db;

public static class DBTriggers
{
  public static void createTrigger(
    SQLiteConnectionWithLock pDBConn,
    HistoryMetaDataAsset tHistoryAsset)
  {
    List<string> pColumns1 = new List<string>();
    List<HistoryDataAsset> pColumns2 = new List<HistoryDataAsset>();
    foreach (HistoryDataAsset category in tHistoryAsset.categories)
    {
      pColumns2.Add(category);
      pColumns1.Add(category.id);
    }
    foreach (HistoryInterval key in tHistoryAsset.table_types.Keys)
    {
      Type tableType1 = tHistoryAsset.table_types[key];
      (int num, HistoryInterval historyInterval) = key.fillFrom();
      if (historyInterval != HistoryInterval.None)
      {
        Type tableType2 = tHistoryAsset.table_types[historyInterval];
        DBTriggers.createInsertionTrigger(pDBConn, tableType1.Name, pColumns2, tableType2.Name, num);
      }
      DBTriggers.createNullDuplicateValuesTrigger(pDBConn, tableType1.Name, pColumns1);
      int maxTimeFrame = key.getMaxTimeFrame();
      DBTriggers.createTrimTableTrigger(pDBConn, tableType1.Name, pColumns1, maxTimeFrame);
    }
  }

  public static void dropTrigger(
    SQLiteConnectionWithLock pDBConn,
    HistoryMetaDataAsset tHistoryAsset)
  {
    foreach (HistoryInterval key in tHistoryAsset.table_types.Keys)
    {
      Type tableType = tHistoryAsset.table_types[key];
      (int tEveryYears, HistoryInterval tFromInterval) tuple = key.fillFrom();
      int tEveryYears = tuple.tEveryYears;
      if (tuple.tFromInterval != HistoryInterval.None)
        DBTriggers.dropInsertionTrigger(pDBConn, tableType.Name, tEveryYears);
      DBTriggers.dropNullDuplicateValuesTrigger(pDBConn, tableType.Name);
      DBTriggers.dropTrimTableTrigger(pDBConn, tableType.Name);
    }
  }

  public static void dropTrimTableTrigger(SQLiteConnectionWithLock pDBConn, string pTableName)
  {
    ((SQLiteConnection) pDBConn).ExecuteScalar<string>("DROP TRIGGER IF EXISTS DELETE_OLD_" + pTableName, Array.Empty<object>());
  }

  public static void createTrimTableTrigger(
    SQLiteConnectionWithLock pDBConn,
    string pTableName,
    List<string> pColumns,
    int pMaxYears)
  {
    string str = $"CREATE TRIGGER IF NOT EXISTS DELETE_OLD_{pTableName}\n\t\t\tAFTER INSERT ON {pTableName}\n\t\t\tWHEN\n\t\t\t\tNEW.timestamp % {pMaxYears} = 0\n\t\t\tAND\n\t\t\t\tEXISTS (\n    \t\t\t\tSELECT 1 FROM {pTableName}\n    \t\t\t\tWHERE\n\t\t\t\t\t\tid = NEW.id AND\n\t\t\t\t\t\ttimestamp < (NEW.timestamp - {pMaxYears})\n\t\t\t\t\tLIMIT 1\n\t\t\t\t)\n\t\t\tBEGIN\n\t\t\t\tINSERT OR REPLACE INTO {pTableName}\n\t\t\t\t(\n\t\t\t\t\tid,\n\t\t\t\t\ttimestamp,\n\t\t\t\t\t{string.Join(", ", (IEnumerable<string>) pColumns)},\n\t\t\t\t\tauto\n\t\t\t\t) VALUES (\n\t\t\t\t\tNEW.id,\n\t\t\t\t\tNEW.timestamp - {pMaxYears},\n\t\t\t\t\t{string.Join(", ", pColumns.Select<string, string>((Func<string, string>) (x => $"(SELECT {x} FROM {pTableName} WHERE id = NEW.id AND timestamp <= (NEW.timestamp - {pMaxYears}) AND {x} IS NOT NULL ORDER BY timestamp DESC LIMIT 1)")))},\n\t\t\t\t\t1\n\t\t\t\t);\n\n-- \t\t\t\tUPDATE {pTableName}\n-- \t\t\t\tSET\n-- \t\t\t\t\t{string.Join(", ", pColumns.Select<string, string>((Func<string, string>) (x => $"{x} = CASE WHEN {x} IS NULL THEN (SELECT {x} FROM {pTableName} WHERE id = NEW.id AND timestamp <= (NEW.timestamp - {pMaxYears}) AND {x} IS NOT NULL ORDER BY timestamp DESC LIMIT 1) ELSE {x} END")))}\n-- \t\t\t\tWHERE\n-- \t\t\t\t\tid = NEW.id AND\n-- \t\t\t\t\ttimestamp = (NEW.timestamp - {pMaxYears})\n-- \t\t\t\t;\n\n\t\t\t\tDELETE FROM {pTableName}\n\t\t\t\tWHERE\n\t\t\t\t\tid = NEW.id AND\n\t\t\t\t\ttimestamp < (NEW.timestamp - {pMaxYears})\n\t\t\t\t;\n\t\t\tEND;";
    ((SQLiteConnection) pDBConn).ExecuteScalar<string>(str, Array.Empty<object>());
  }

  public static void dropNullDuplicateValuesTrigger(
    SQLiteConnectionWithLock pDBConn,
    string pTableName)
  {
    ((SQLiteConnection) pDBConn).ExecuteScalar<string>("DROP TRIGGER IF EXISTS NULL_DUPLICATES_" + pTableName, Array.Empty<object>());
  }

  public static void createNullDuplicateValuesTrigger(
    SQLiteConnectionWithLock pDBConn,
    string pTableName,
    List<string> pColumns)
  {
    string str = $"CREATE TRIGGER IF NOT EXISTS NULL_DUPLICATES_{pTableName}\n\t\t\tAFTER INSERT ON {pTableName}\n\t\t\t\tWHEN NOT EXISTS (\n\t\t\t\t\tSELECT 1 FROM {pTableName} WHERE id = NEW.id AND timestamp > NEW.timestamp LIMIT 1\n\t\t\t\t)\n\t\t\tBEGIN\n\t\t\t\tUPDATE {pTableName}\n\t\t\t\tSET {string.Join(", ", pColumns.Select<string, string>((Func<string, string>) (x => $"{x} = CASE WHEN (SELECT {x} FROM {pTableName} WHERE id = NEW.id AND {x} IS NOT NULL ORDER BY timestamp DESC LIMIT 1,1) = NEW.{x} THEN NULL ELSE NEW.{x} END")))}\n\t\t\t\tWHERE rowid = NEW.rowid;\n\n\t\t\t\tDELETE FROM {pTableName} WHERE rowid = NEW.rowid AND {string.Join(" AND ", pColumns.Select<string, string>((Func<string, string>) (x => x + " IS NULL")))};\n\t\t\tEND;";
    ((SQLiteConnection) pDBConn).ExecuteScalar<string>(str, Array.Empty<object>());
  }

  public static void dropInsertionTrigger(
    SQLiteConnectionWithLock pDBConn,
    string pTargetTable,
    int pYearDiviver)
  {
    ((SQLiteConnection) pDBConn).ExecuteScalar<string>($"DROP TRIGGER IF EXISTS FILL_{pTargetTable}_{pYearDiviver}", Array.Empty<object>());
  }

  public static void createInsertionTrigger(
    SQLiteConnectionWithLock pDBConn,
    string pTargetTable,
    List<HistoryDataAsset> pColumns,
    string pSourceTable,
    int pYearDiviver)
  {
    string str = $"CREATE TRIGGER IF NOT EXISTS FILL_{pTargetTable}_{pYearDiviver}\n\t\t\tAFTER INSERT ON {pSourceTable}\n\t\t\t\tWHEN NEW.timestamp % {pYearDiviver} = 0 AND NEW.auto IS NOT 1\n\t\t\tBEGIN\n\t\t\tINSERT INTO\n\t\t\t\t{pTargetTable}(\n\t\t\t\t\t{string.Join(", ", pColumns.Select<HistoryDataAsset, string>((Func<HistoryDataAsset, string>) (x => x.id)))},\n\t\t\t\t\tid,\n\t\t\t\t\ttimestamp\n\t\t\t\t)\n\t\t\tSELECT\n\t\t\t\t{string.Join(", ", pColumns.Select<HistoryDataAsset, string>((Func<HistoryDataAsset, string>) (x =>
    {
      if (x.max)
        return $"MAX({x.id})";
      if (x.sum)
        return $"SUM({x.id})";
      return x.average ? $"CAST(AVG({x.id})+1-1e-1 AS INT)" : $"ROUND(AVG({x.id}))";
    })))},\n\t\t\t\tNEW.id,\n\t\t\t\tNEW.timestamp\n\t\t\tFROM\n\t\t\t\t(\n\t\t\t\t\tSELECT\n\t\t\t\t\t\t*\n\t\t\t\t\tFROM\n\t\t\t\t\t\t{pSourceTable}\n\t\t\t\t\tWHERE\n\t\t\t\t\t\tid = NEW.id\n\t\t\t\t\tAND\n\t\t\t\t\t\ttimestamp >= NEW.timestamp - {pYearDiviver}\n\t\t\t\t\tAND\n\t\t\t\t\t\ttimestamp < NEW.timestamp\n\t\t\t\t\tORDER BY\n\t\t\t\t\t\ttimestamp DESC\n\t\t\t\t);\n\n\t\t\tEND;";
    ((SQLiteConnection) pDBConn).ExecuteScalar<string>(str, Array.Empty<object>());
  }
}

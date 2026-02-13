// Decompiled with JetBrains decompiler
// Type: SQLite.SQLiteExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace SQLite;

public static class SQLiteExtensions
{
  public static ListPool<Dictionary<string, long>> Query(
    this SQLiteConnection conn,
    string query,
    params object[] args)
  {
    return new ListPool<Dictionary<string, long>>(conn.CreateCommand(query, args).ExecuteDeferredQuery());
  }

  public static IEnumerable<Dictionary<string, long>> ExecuteDeferredQuery(
    this SQLiteConnection conn,
    string query,
    params object[] args)
  {
    return conn.CreateCommand(query, args).ExecuteDeferredQuery();
  }

  public static int Delete(
    this SQLiteConnection conn,
    string columnName,
    object obj,
    Type objType)
  {
    return conn.Delete(columnName, obj, conn.GetMapping(objType, (CreateFlags) 0));
  }

  public static int Delete(
    this SQLiteConnection conn,
    string columnName,
    object columnValue,
    TableMapping map)
  {
    TableMapping.Column column = map.FindColumn(columnName);
    if (column == null)
      throw new NotSupportedException($"Cannot delete {map.TableName}: it has no column named {columnName}");
    string str = $"delete from \"{map.TableName}\" where \"{column.Name}\" = ?";
    int num = conn.Execute(str, new object[1]{ columnValue });
    if (num <= 0)
      return num;
    conn.OnTableChanged(map, (NotifyTableChangedAction) 2);
    return num;
  }

  public static ListPool<T> ExecuteQueryPool<T>(this SQLiteCommand cmd, TableMapping map)
  {
    return new ListPool<T>(cmd.ExecuteDeferredQuery<T>(map));
  }

  public static ListPool<T> ExecuteQueryPool<T>(this SQLiteCommand cmd, SQLiteConnection conn)
  {
    return new ListPool<T>(cmd.ExecuteDeferredQuery<T>(conn.GetMapping(typeof (T), (CreateFlags) 0)));
  }

  public static ListPool<object> QueryPool(
    this SQLiteConnection conn,
    TableMapping map,
    string query,
    params object[] args)
  {
    return conn.CreateCommand(query, args).ExecuteQueryPool<object>(map);
  }

  public static ListPool<T> QueryPool<T>(
    this SQLiteConnection conn,
    string query,
    params object[] args)
    where T : new()
  {
    return conn.CreateCommand(query, args).ExecuteQueryPool<T>(conn);
  }
}

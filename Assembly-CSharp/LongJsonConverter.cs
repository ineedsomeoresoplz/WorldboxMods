// Decompiled with JetBrains decompiler
// Type: LongJsonConverter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class LongJsonConverter : JsonConverter
{
  internal static long next_long = 100000000;
  internal static Dictionary<string, long> longs = new Dictionary<string, long>();

  public virtual bool CanWrite => false;

  public virtual bool CanRead => true;

  public static void reset()
  {
    LongJsonConverter.next_long = 100000000L;
    LongJsonConverter.longs.Clear();
  }

  public static long getLong(string pString, JsonReader pReader)
  {
    if (string.IsNullOrEmpty(pString))
      return -1;
    string s = pString;
    if (pString.IndexOf('_') > 0)
    {
      string[] strArray = pString.Split('_', StringSplitOptions.None);
      if (strArray.Length == 2)
      {
        string pValue = strArray[0] + "_";
        if (MapStats.possible_formats.IndexOf<string>(pValue) > -1)
          s = strArray[1];
      }
    }
    long result;
    if (long.TryParse(s, out result))
      return result;
    bool flag = pString.Length == 8 || pString.Length == 36 && pString[8] == '-' && pString[13] == '-' && pString[18] == '-' && pString[23] == '-';
    long num;
    if (!LongJsonConverter.longs.TryGetValue(pString, out num))
    {
      num = LongJsonConverter.next_long++;
      LongJsonConverter.longs[pString] = num;
      if (!flag)
        Debug.LogWarning((object) $"{pReader.Path} Failed to parse long <b>{pString}</b> {pString.Length.ToString()} -> {num.ToString()}");
    }
    else if (!flag)
      Debug.LogWarning((object) $"{pReader.Path} Failed to parse long <b>{pString}</b> {pString.Length.ToString()} -> {num.ToString()} already had it");
    return num;
  }

  public virtual object ReadJson(
    JsonReader reader,
    Type objectType,
    object existingValue,
    JsonSerializer serializer)
  {
    switch (reader.TokenType - 7)
    {
      case 0:
        return (object) Convert.ToInt64(reader.Value);
      case 2:
        return (object) LongJsonConverter.getLong((string) reader.Value, reader);
      case 4:
        return (object) -1L;
      default:
        Debug.LogWarning((object) $"Unhandled type {reader.Path} {reader.Value?.ToString()} {reader.TokenType.ToString()} -> {-1L.ToString()}");
        return (object) -1L;
    }
  }

  public virtual void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
  {
    writer.WriteValue(value);
  }

  public virtual bool CanConvert(Type objectType) => objectType == typeof (long);
}

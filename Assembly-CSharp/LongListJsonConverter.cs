// Decompiled with JetBrains decompiler
// Type: LongListJsonConverter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class LongListJsonConverter : JsonConverter
{
  public virtual bool CanWrite => false;

  public virtual bool CanRead => true;

  public virtual object ReadJson(
    JsonReader reader,
    Type objectType,
    object existingValue,
    JsonSerializer serializer)
  {
    if (reader.TokenType == 11)
      return (object) null;
    if (reader.TokenType == 2)
    {
      using (ListPool<long> collection = new ListPool<long>())
      {
        while (reader.Read())
        {
          JsonToken tokenType = reader.TokenType;
          switch (tokenType - 7)
          {
            case 0:
              collection.Add(Convert.ToInt64(reader.Value));
              continue;
            case 1:
            case 3:
              continue;
            case 2:
              string pString = (string) reader.Value;
              collection.Add(LongJsonConverter.getLong(pString, reader));
              continue;
            case 4:
              collection.Add(-1L);
              continue;
            default:
              if (tokenType == 14)
                return (object) new List<long>((IEnumerable<long>) collection);
              continue;
          }
        }
      }
    }
    Debug.LogWarning((object) $"Unhandled type {reader.Path} {reader.Value?.ToString()} {reader.TokenType.ToString()} -> null");
    return (object) null;
  }

  public virtual void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
  {
    writer.WriteValue(value);
  }

  public virtual bool CanConvert(Type objectType) => objectType == typeof (List<long>);
}

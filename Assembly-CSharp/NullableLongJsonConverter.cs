// Decompiled with JetBrains decompiler
// Type: NullableLongJsonConverter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Newtonsoft.Json;
using System;
using UnityEngine;

#nullable disable
public class NullableLongJsonConverter : JsonConverter
{
  public virtual bool CanWrite => false;

  public virtual bool CanRead => true;

  public static long? getLong(string pString, JsonReader pReader)
  {
    return string.IsNullOrEmpty(pString) ? new long?() : new long?(LongJsonConverter.getLong(pString, pReader));
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
        return (object) NullableLongJsonConverter.getLong((string) reader.Value, reader);
      case 4:
        return (object) null;
      default:
        Debug.LogWarning((object) $"Unhandled type {reader.Path} {reader.Value?.ToString()} {reader.TokenType.ToString()} -> null");
        return (object) null;
    }
  }

  public virtual void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
  {
    writer.WriteValue(value);
  }

  public virtual bool CanConvert(Type objectType) => objectType == typeof (long?);
}

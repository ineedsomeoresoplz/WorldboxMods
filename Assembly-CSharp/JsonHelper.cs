// Decompiled with JetBrains decompiler
// Type: JsonHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Newtonsoft.Json;

#nullable disable
public static class JsonHelper
{
  private static JsonSerializer _writer;
  private static JsonSerializer _reader;
  private static JsonSerializerSettings _settings;

  public static JsonSerializer writer
  {
    get
    {
      if (JsonHelper._writer == null)
        JsonHelper._writer = JsonSerializer.Create(new JsonSerializerSettings()
        {
          DefaultValueHandling = (DefaultValueHandling) 3
        });
      return JsonHelper._writer;
    }
  }

  public static JsonSerializer reader
  {
    get
    {
      if (JsonHelper._reader == null)
        JsonHelper._reader = JsonSerializer.Create(JsonHelper.read_settings);
      return JsonHelper._reader;
    }
  }

  public static JsonSerializerSettings read_settings
  {
    get
    {
      if (JsonHelper._settings == null)
      {
        JsonHelper._settings = new JsonSerializerSettings();
        JsonHelper._settings.DefaultValueHandling = (DefaultValueHandling) 3;
        JsonHelper._settings.Converters.Add((JsonConverter) new LongJsonConverter());
        JsonHelper._settings.Converters.Add((JsonConverter) new LongListJsonConverter());
        JsonHelper._settings.Converters.Add((JsonConverter) new NullableLongJsonConverter());
        JsonHelper._settings.Converters.Add((JsonConverter) new NullableLongListJsonConverter());
      }
      return JsonHelper._settings;
    }
  }
}

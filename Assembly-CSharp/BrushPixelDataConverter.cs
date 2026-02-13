// Decompiled with JetBrains decompiler
// Type: BrushPixelDataConverter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Newtonsoft.Json;
using System;

#nullable disable
public class BrushPixelDataConverter : JsonConverter
{
  public virtual void WriteJson(JsonWriter pWriter, object pValue, JsonSerializer pSerializer)
  {
    BrushPixelData brushPixelData = (BrushPixelData) pValue;
    string str = $"{brushPixelData.x.ToString()},{brushPixelData.y.ToString()},{brushPixelData.dist.ToString()}";
    pSerializer.Serialize(pWriter, (object) str, typeof (string));
  }

  public virtual object ReadJson(
    JsonReader pReader,
    Type pObjectType,
    object pExistingValue,
    JsonSerializer pSerializer)
  {
    string str = pSerializer.Deserialize<string>(pReader);
    if (string.IsNullOrEmpty(str))
      return (object) null;
    int[] numArray = Array.ConvertAll<string, int>(str.Split(',', StringSplitOptions.None), new Converter<string, int>(int.Parse));
    return (object) new BrushPixelData(numArray[0], numArray[1], numArray[2]);
  }

  public virtual bool CanConvert(Type pObjectType)
  {
    return pObjectType != (Type) null && pObjectType == typeof (BrushPixelData);
  }
}

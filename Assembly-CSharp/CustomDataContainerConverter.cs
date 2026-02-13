// Decompiled with JetBrains decompiler
// Type: CustomDataContainerConverter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Newtonsoft.Json;
using System;
using System.Reflection;

#nullable disable
public class CustomDataContainerConverter : JsonConverter
{
  public virtual void WriteJson(JsonWriter pWriter, object pValue, JsonSerializer pSerializer)
  {
    FieldInfo field = pValue.GetType().GetField("dict", BindingFlags.Instance | BindingFlags.NonPublic);
    Type fieldType = field.FieldType;
    object obj = field.GetValue(pValue);
    pSerializer.Serialize(pWriter, obj, fieldType);
  }

  public virtual object ReadJson(
    JsonReader pReader,
    Type pObjectType,
    object pExistingValue,
    JsonSerializer pSerializer)
  {
    object instance = Activator.CreateInstance(pObjectType);
    FieldInfo field = pObjectType.GetField("dict", BindingFlags.Instance | BindingFlags.NonPublic);
    Type fieldType = field.FieldType;
    field.SetValue(instance, pSerializer.Deserialize(pReader, fieldType));
    return instance;
  }

  public virtual bool CanConvert(Type pObjectType) => false;
}

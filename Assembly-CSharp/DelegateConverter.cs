// Decompiled with JetBrains decompiler
// Type: DelegateConverter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Newtonsoft.Json;
using System;

#nullable disable
public class DelegateConverter : JsonConverter
{
  public virtual void WriteJson(JsonWriter pWriter, object pValue, JsonSerializer pSerializer)
  {
    if (pValue == null)
      return;
    Delegate[] invocationList = ((Delegate) pValue).GetInvocationList();
    string[] strArray = new string[invocationList.Length];
    for (int index = 0; index < invocationList.Length; ++index)
      strArray[index] = $"{invocationList[index].Method.DeclaringType?.ToString()}.{invocationList[index].Method.Name}";
    pSerializer.Serialize(pWriter, (object) strArray, typeof (string[]));
  }

  public virtual object ReadJson(
    JsonReader pReader,
    Type pObjectType,
    object pExistingValue,
    JsonSerializer pSerializer)
  {
    return (object) null;
  }

  public virtual bool CanConvert(Type pObjectType)
  {
    if (!(pObjectType != (Type) null))
      return false;
    return pObjectType == typeof (Delegate) || pObjectType.IsSubclassOf(typeof (Delegate));
  }
}

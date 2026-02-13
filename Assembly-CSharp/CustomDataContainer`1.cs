// Decompiled with JetBrains decompiler
// Type: CustomDataContainer`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityPools;

#nullable disable
[JsonConverter(typeof (CustomDataContainerConverter))]
[Serializable]
public class CustomDataContainer<TType> : IDisposable
{
  [NonSerialized]
  internal Dictionary<string, TType> dict = UnsafeCollectionPool<Dictionary<string, TType>, KeyValuePair<string, TType>>.Get();

  public bool TryGetValue(string pKey, out TType pValue) => this.dict.TryGetValue(pKey, out pValue);

  public TType this[string pKey]
  {
    get => this.dict[pKey];
    set => this.dict[pKey] = value;
  }

  public void Remove(string pKey) => this.dict.Remove(pKey);

  public IEnumerable<string> Keys => (IEnumerable<string>) this.dict.Keys;

  public void Dispose()
  {
    if (this.dict != null)
    {
      this.dict.Clear();
      UnsafeCollectionPool<Dictionary<string, TType>, KeyValuePair<string, TType>>.Release(this.dict);
    }
    this.dict = (Dictionary<string, TType>) null;
  }
}

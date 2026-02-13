// Decompiled with JetBrains decompiler
// Type: ResourceContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Newtonsoft.Json;
using System;

#nullable disable
[Serializable]
public struct ResourceContainer(string pID, int pAmount)
{
  [JsonProperty]
  public readonly string id = pID;
  public int amount = pAmount;

  [JsonIgnore]
  public ResourceAsset asset => AssetManager.resources.get(this.id);
}

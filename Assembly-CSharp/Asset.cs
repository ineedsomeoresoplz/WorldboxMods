// Decompiled with JetBrains decompiler
// Type: Asset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Newtonsoft.Json;
using System;

#nullable disable
[Serializable]
public abstract class Asset : IEquatable<Asset>
{
  public const string DEFAULT_ASSET_ID = "ASSET_ID";
  [JsonProperty(Order = -1)]
  public string id = "ASSET_ID";
  private int _hashcode;
  private int _index;

  public virtual void create()
  {
  }

  public void setHash(int pHash) => this._hashcode = pHash;

  public void setIndexID(int pValue) => this._index = pValue;

  public int getIndexID() => this._index;

  public bool Equals(Asset pAsset) => this._hashcode == pAsset.GetHashCode();

  public override int GetHashCode() => this._hashcode;

  public bool isTemplateAsset() => this.id.StartsWith("$") || this.id.StartsWith("_");
}

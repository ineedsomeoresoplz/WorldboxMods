// Decompiled with JetBrains decompiler
// Type: BuildingData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Newtonsoft.Json;
using System;
using System.ComponentModel;
using UnityEngine.Scripting;

#nullable disable
[Serializable]
public class BuildingData : BaseObjectData
{
  [DefaultValue(BuildingState.Normal)]
  public BuildingState state = BuildingState.Normal;
  public int mainX;
  public int mainY;
  [JsonProperty]
  public string asset_id;
  [DefaultValue(-1)]
  public long cityID = -1;
  public float grow_time;
  public CityResources resources;
  public StorageBooks books;
  [DefaultValue(-1)]
  public int frameID = -1;

  [Preserve]
  [Obsolete("use .id instead", true)]
  public long objectID
  {
    set
    {
      if (!value.hasValue() || this.id.hasValue())
        return;
      this.id = value;
    }
  }

  [Preserve]
  [Obsolete("use .asset_id instead", true)]
  public string templateID
  {
    set
    {
      if (string.IsNullOrEmpty(value) || !string.IsNullOrEmpty(this.asset_id))
        return;
      this.asset_id = value;
    }
  }

  public override void Dispose()
  {
    base.Dispose();
    this.resources?.Dispose();
    this.resources = (CityResources) null;
    this.books?.Dispose();
    this.books = (StorageBooks) null;
  }
}

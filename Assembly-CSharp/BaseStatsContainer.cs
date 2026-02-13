// Decompiled with JetBrains decompiler
// Type: BaseStatsContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Newtonsoft.Json;
using UnityEngine;

#nullable disable
public class BaseStatsContainer
{
  public string id;
  public float value;

  public void normalize()
  {
    BaseStatAsset asset = this.asset;
    if (!asset.normalize)
      return;
    this.value = Mathf.Clamp(this.value, asset.normalize_min, asset.normalize_max);
  }

  [JsonIgnore]
  public BaseStatAsset asset => AssetManager.base_stats_library.get(this.id);
}

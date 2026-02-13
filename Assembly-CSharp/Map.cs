// Decompiled with JetBrains decompiler
// Type: Map
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class Map
{
  public string mapId;
  public string language;
  public string timestamp;
  public string userId;
  public string username;
  public string version;
  public string mapName;
  public string mapDescription;
  public int size;
  public int sortIndex;
  public List<MapTagType> mapTags;
  public MapMetaData mapMeta;
  public OnlineStats onlineStats = new OnlineStats()
  {
    downloads = 0,
    plays = 0,
    favs = 0,
    reports = 0
  };

  public string formattedMapId
  {
    get
    {
      if (string.IsNullOrEmpty(this.mapId) || this.mapId.Length != 12)
        return this.mapId;
      return $"WB-{this.mapId.Substring(0, 4)}-{this.mapId.Substring(4, 4)}-{this.mapId.Substring(8, 4)}";
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: WorldLaws
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class WorldLaws
{
  public List<PlayerOptionData> list;
  [NonSerialized]
  public Dictionary<string, PlayerOptionData> dict;

  public PlayerOptionData add(PlayerOptionData pData)
  {
    foreach (PlayerOptionData playerOptionData in this.list)
    {
      if (string.Equals(pData.name, playerOptionData.name))
      {
        this.dict.TryAdd(playerOptionData.name, playerOptionData);
        playerOptionData.on_switch = pData.on_switch;
        return playerOptionData;
      }
    }
    this.list.Add(pData);
    this.dict.Add(pData.name, pData);
    return pData;
  }

  public void check() => this.init();

  public void updateCaches()
  {
    foreach (WorldLawAsset worldLawAsset in AssetManager.world_laws_library.list)
      worldLawAsset.updateCachedEnabled(this);
  }

  public void init(bool pUpdateCaches = true)
  {
    if (this.list == null)
      this.list = new List<PlayerOptionData>();
    if (this.dict == null)
      this.dict = new Dictionary<string, PlayerOptionData>();
    foreach (WorldLawAsset worldLawAsset in AssetManager.world_laws_library.list)
      this.add(new PlayerOptionData(worldLawAsset.id)
      {
        boolVal = worldLawAsset.default_state,
        on_switch = worldLawAsset.on_state_change
      });
    foreach (Asset asset in AssetManager.era_library.list)
      this.add(new PlayerOptionData(asset.id)
      {
        boolVal = true
      });
    if (pUpdateCaches)
      this.updateCaches();
    PowerButton.checkActorSpawnButtons();
  }

  public bool isAgeEnabled(string pID) => this.dict[pID].boolVal;

  public void setAgeEnabled(string pID, bool pValue) => this.dict[pID].boolVal = pValue;

  public bool isEnabled(string pId)
  {
    PlayerOptionData playerOptionData;
    return this.dict.TryGetValue(pId, out playerOptionData) && playerOptionData.boolVal;
  }

  public void enable(string pID)
  {
    PlayerOptionData playerOptionData;
    if (!this.dict.TryGetValue(pID, out playerOptionData))
      return;
    playerOptionData.boolVal = true;
    this.updateCaches();
  }
}

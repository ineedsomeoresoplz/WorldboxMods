// Decompiled with JetBrains decompiler
// Type: ResourceThrowManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ResourceThrowManager
{
  private List<ResourceThrowData> _list = new List<ResourceThrowData>();

  public void update(float pElapsed) => this.updateRemoval();

  private void updateRemoval()
  {
    List<ResourceThrowData> list = this._list;
    for (int index = list.Count - 1; index >= 0; --index)
    {
      ResourceThrowData resourceThrowData = list[index];
      if (resourceThrowData.isFinished())
      {
        list.RemoveAt(index);
        World.world.buildings.get(resourceThrowData.building_target_id)?.startShake(0.3f);
      }
    }
  }

  public void addNew(
    Vector2 pStart,
    Vector2 pEnd,
    float pDuration,
    string pResourceAssetId,
    int pResourceAmount,
    float pHeight,
    Building pBuildingTarget)
  {
    this._list.Add(new ResourceThrowData(pStart, pEnd, pDuration, pResourceAssetId, pResourceAmount, pBuildingTarget.getID(), pHeight));
  }

  public List<ResourceThrowData> getList() => this._list;

  public void clear() => this._list.Clear();
}

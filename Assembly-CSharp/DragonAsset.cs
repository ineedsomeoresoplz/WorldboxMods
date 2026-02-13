// Decompiled with JetBrains decompiler
// Type: DragonAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DragonAsset : ScriptableObject
{
  private Dictionary<DragonState, DragonAssetContainer> _dict;
  public DragonAssetContainer[] list;

  public DragonAssetContainer getAsset(DragonState pState)
  {
    if (this._dict == null)
    {
      this._dict = new Dictionary<DragonState, DragonAssetContainer>();
      foreach (DragonAssetContainer dragonAssetContainer in this.list)
        this._dict.Add(dragonAssetContainer.id, dragonAssetContainer);
    }
    return this._dict[pState];
  }
}

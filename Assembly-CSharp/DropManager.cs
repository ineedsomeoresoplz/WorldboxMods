// Decompiled with JetBrains decompiler
// Type: DropManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DropManager
{
  private List<Drop> _drops = new List<Drop>();
  private float _timeout_timer;
  private int _activeIndex;
  private GameObject _original_drop;
  private Transform _dropContainer;

  public DropManager(Transform pDropContainer)
  {
    this._dropContainer = pDropContainer;
    this._original_drop = (GameObject) Resources.Load("effects/p_drop", typeof (GameObject));
  }

  public Drop spawn(WorldTile pTile, string pDropID, float zHeight = -1f, float pScale = -1f, long pCasterId = -1)
  {
    DropAsset pAsset = AssetManager.drops.get(pDropID);
    return this.spawn(pTile, pAsset, zHeight, pScale, pCasterId: pCasterId);
  }

  public Drop spawn(
    WorldTile pTile,
    DropAsset pAsset,
    float zHeight = -1f,
    float pScale = -1f,
    bool pForceSurprise = false,
    long pCasterId = -1)
  {
    Drop drop = this.getObject();
    if (pForceSurprise)
      drop.setForceSurprise();
    drop.launchStraight(pTile, pAsset, zHeight);
    if ((double) pScale == -1.0)
      pScale = pAsset.default_scale;
    drop.setScale(new Vector3(pScale, pScale, ((Component) drop).transform.localScale.z));
    drop.setCasterId(pCasterId);
    return drop;
  }

  public void spawnParabolicDrop(
    WorldTile pTile,
    string pDropID,
    float pStartHeight = 0.0f,
    float pMinHeight = 0.0f,
    float pMaxHeight = 0.0f,
    float pMinRadius = 0.0f,
    float pMaxRadius = 0.0f,
    float pScale = -1f)
  {
    this.spawn(pTile, pDropID, pMinHeight, pScale).launchParabolic(pStartHeight, pMinHeight, pMaxHeight, pMinRadius, pMaxRadius);
  }

  public void clear()
  {
    List<Drop> drops = this._drops;
    for (int index = 0; index < drops.Count; ++index)
      drops[index].makeInactive();
    this._activeIndex = 0;
  }

  private void killObject(Drop pObject)
  {
    pObject.makeInactive();
    int index1 = pObject.drop_index - 1;
    int index2 = this._activeIndex - 1;
    List<Drop> drops = this._drops;
    if (index1 != index2)
    {
      Drop drop = drops[index2];
      drops[index2] = pObject;
      drops[index1] = drop;
      pObject.drop_index = index2 + 1;
      drop.drop_index = index1 + 1;
    }
    if (this._activeIndex <= 0)
      return;
    --this._activeIndex;
  }

  public void landDrop(Drop pObject)
  {
    WorldTile currentTile = pObject.current_tile;
    this.killObject(pObject);
    if (currentTile == null)
      return;
    World.world.flash_effects.flashPixel(currentTile, 14);
  }

  public Drop getObject()
  {
    List<Drop> drops = this._drops;
    Drop component;
    if (drops.Count > this._activeIndex)
    {
      component = drops[this._activeIndex];
    }
    else
    {
      component = Object.Instantiate<GameObject>(this._original_drop, this._dropContainer).GetComponent<Drop>();
      ((Component) component).gameObject.layer = ((Component) this._dropContainer).gameObject.layer;
      ((Component) component).transform.parent = this._dropContainer;
      drops.Add(component);
      component.drop_index = drops.Count;
    }
    ++this._activeIndex;
    component.prepare();
    return component;
  }

  public void update(float pElapsed)
  {
    Bench.bench("drops", "game_total");
    if ((double) this._timeout_timer > 0.0)
      this._timeout_timer -= World.world.delta_time;
    List<Drop> drops = this._drops;
    for (int index = this._activeIndex - 1; index >= 0; --index)
    {
      Drop drop = drops[index];
      if (drop.created && drop.active)
        drop.update(pElapsed);
      else if (this._activeIndex == drop.drop_index)
      {
        --this._activeIndex;
        Debug.LogError((object) ("do we ever hit this??? " + this._activeIndex.ToString()));
      }
    }
    Bench.benchEnd("drops", "game_total");
  }

  public void debug(DebugTool pTool)
  {
    pTool.setText("drops total", (object) (this._drops.Count.ToString() ?? ""));
    pTool.setText("drops active", (object) (this._activeIndex.ToString() ?? ""));
  }

  public int getActiveIndex() => this._activeIndex;

  private void debugString()
  {
    string str = "";
    for (int index = 0; index < this._drops.Count; ++index)
      str = !this._drops[index].active ? str + "x" : str + "O";
    Debug.Log((object) $"{str} ::: {this._activeIndex.ToString()}");
  }
}

// Decompiled with JetBrains decompiler
// Type: BaseEffectController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BaseEffectController : BaseMapObject
{
  public Transform prefab;
  private int _active_index;
  private readonly List<BaseEffect> _list = new List<BaseEffect>();
  private float _timer;
  private float _timer_interval = 1f;
  private bool _object_limit_used;
  private int _object_limit;
  private bool _limit_unload;
  public bool useInterval = true;
  public EffectAsset asset;

  internal override void create()
  {
    base.create();
    this._timer_interval = 0.9f;
  }

  public void setLimits(int pLimitObjects, bool pLimitUnload)
  {
    if (pLimitObjects > 0)
      this._object_limit_used = true;
    this._object_limit = pLimitObjects;
    this._limit_unload = pLimitUnload;
  }

  public BaseEffect GetObject()
  {
    List<BaseEffect> list = this._list;
    BaseEffect component;
    if (list.Count > this._active_index)
    {
      component = list[this._active_index];
    }
    else
    {
      component = ((Component) Object.Instantiate<Transform>(this.prefab)).gameObject.GetComponent<BaseEffect>();
      this.addNewObject(component);
      if (!component.created)
        component.create();
      list.Add(component);
      component.effectIndex = list.Count;
    }
    ++this._active_index;
    component.activate();
    return component;
  }

  public int getActiveIndex() => this._active_index;

  internal void addNewObject(BaseEffect pEffect)
  {
    pEffect.controller = this;
    ((Component) pEffect).transform.parent = ((Component) this).transform;
  }

  public void killObject(BaseEffect pObject)
  {
    if (!pObject.active)
      return;
    this.makeInactive(pObject);
    List<BaseEffect> list = this._list;
    int index1 = pObject.effectIndex - 1;
    int index2 = this._active_index - 1;
    if (index1 != index2)
    {
      BaseEffect baseEffect = list[index2];
      list[index2] = pObject;
      list[index1] = baseEffect;
      pObject.effectIndex = index2 + 1;
      baseEffect.effectIndex = index1 + 1;
    }
    if (this._active_index <= 0)
      return;
    --this._active_index;
  }

  private void makeInactive(BaseEffect pObject) => pObject.deactivate();

  private void debugString()
  {
    string str = "";
    List<BaseEffect> list = this._list;
    for (int index = 0; index < list.Count; ++index)
      str = !list[index].active ? str + "x" : str + "O";
    Debug.Log((object) $"{str} ::: {this._active_index.ToString()}");
  }

  public override void update(float pElapsed)
  {
    base.update(pElapsed);
    this.updateChildren(pElapsed);
    this.updateSpawn(pElapsed);
  }

  private void updateSpawn(float pElapsed)
  {
    if (World.world.isPaused() || !this.useInterval)
      return;
    if ((double) this._timer > 0.0)
    {
      this._timer -= pElapsed;
    }
    else
    {
      this._timer = this._timer_interval;
      this.spawn();
    }
  }

  private void updateChildren(float pElapsed)
  {
    List<BaseEffect> list = this._list;
    for (int index = this._active_index - 1; index >= 0; --index)
    {
      BaseEffect baseEffect = list[index];
      if (baseEffect.created && baseEffect.active)
        baseEffect.update(pElapsed);
    }
  }

  public virtual void spawn()
  {
  }

  public BaseEffect spawnNew()
  {
    if (this.isLimitReached())
    {
      if (!this._limit_unload)
        return (BaseEffect) null;
      this.killOldest();
    }
    BaseEffect baseEffect = this.GetObject();
    if (Object.op_Inequality((Object) baseEffect.sprite_animation, (Object) null))
      baseEffect.sprite_animation.resetAnim();
    return baseEffect;
  }

  private void killOldest()
  {
    if (this._list.Count == 0)
      return;
    BaseEffect pObject = this._list[0];
    double num = double.MaxValue;
    foreach (BaseEffect baseEffect in this._list)
    {
      if (baseEffect.timestamp_spawned < num)
      {
        pObject = baseEffect;
        num = baseEffect.timestamp_spawned;
      }
    }
    this.killObject(pObject);
  }

  internal bool isLimitReached()
  {
    return this._object_limit_used && this._active_index >= this._object_limit;
  }

  internal void clear()
  {
    List<BaseEffect> list = this._list;
    for (int index = 0; index < list.Count; ++index)
      this.makeInactive(list[index]);
    this._active_index = 0;
  }

  public bool isAnyActive() => this._active_index > 0;

  internal void debug(DebugTool pTool)
  {
    pTool.setText(((Object) this).name, (object) $"{this._active_index.ToString()}/{this._list.Count.ToString()}");
  }

  internal List<BaseEffect> getList() => this._list;
}

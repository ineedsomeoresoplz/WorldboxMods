// Decompiled with JetBrains decompiler
// Type: SpriteGroupSystem`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class SpriteGroupSystem<T> : MonoBehaviour where T : GroupSpriteObject
{
  private T[] _sprites = new T[32 /*0x20*/];
  private int _total;
  internal T prefab;
  internal int count_active_debug;
  private int _count_total_debug;
  private int _used_index;
  private int _active_index;
  public bool turn_off_renderer;

  public virtual void create()
  {
    ((Object) ((Component) this).transform).name = "GroupSpriteController";
    ((Component) this).transform.parent = ((Component) World.world).transform;
  }

  public T[] getAll() => this._sprites;

  public void prepare() => this._used_index = 0;

  public virtual void update(float pElapsed) => this.finale();

  private void finale()
  {
    this.clearLast();
    this.count_active_debug = this._active_index;
    this._count_total_debug = this._total;
  }

  public void clearFull()
  {
    for (int index = 0; index < this._active_index; ++index)
      this.disableSprite((GroupSpriteObject) this._sprites[index]);
    this._active_index = 0;
    this._used_index = 0;
  }

  public void clearLast()
  {
    int num = this._active_index - this._used_index;
    for (int index = 0; index < num; ++index)
    {
      T sprite = this._sprites[this._active_index - 1 - index];
      this.disableSprite((GroupSpriteObject) sprite);
      this.deactivate(sprite);
    }
    this._active_index -= num;
  }

  private void disableSprite(GroupSpriteObject pQ)
  {
    if (this.turn_off_renderer)
    {
      ((Renderer) pQ.sprite_renderer).enabled = false;
    }
    else
    {
      if (!((Component) pQ).gameObject.activeSelf)
        return;
      ((Component) pQ).gameObject.SetActive(false);
    }
  }

  private void enableSprite(GroupSpriteObject pQ)
  {
    if (this.turn_off_renderer)
    {
      ((Renderer) pQ.sprite_renderer).enabled = true;
    }
    else
    {
      if (((Component) pQ).gameObject.activeSelf)
        return;
      ((Component) pQ).gameObject.SetActive(true);
    }
  }

  public virtual void deactivate(T pObject)
  {
  }

  public virtual void checkActiveAction(T pObject)
  {
  }

  internal T getNext()
  {
    T sprite;
    if (this.is_within_active_index)
    {
      sprite = this._sprites[this._used_index];
    }
    else
    {
      if (this._active_index < this._total)
      {
        sprite = this._sprites[this._active_index];
        this.enableSprite((GroupSpriteObject) sprite);
      }
      else
        sprite = this.createNew();
      ++this._active_index;
    }
    this.checkActiveAction(sprite);
    ++this._used_index;
    return sprite;
  }

  internal bool is_within_active_index => this._active_index > this._used_index;

  public T[] getFastActiveList(int pPlannedSize)
  {
    int activeIndex = this._active_index;
    if (activeIndex < pPlannedSize)
    {
      this._used_index = activeIndex;
      int num = pPlannedSize - activeIndex;
      for (int index = 0; index < num; ++index)
        this.getNext();
    }
    else
      this._used_index = pPlannedSize;
    return this._sprites;
  }

  protected virtual T createNew()
  {
    T obj = Object.Instantiate<T>(this.prefab, ((Component) this).gameObject.transform);
    if (this._total >= this._sprites.Length)
    {
      T[] destinationArray = new T[this._sprites.Length * 2];
      Array.Copy((Array) this._sprites, (Array) destinationArray, this._sprites.Length);
      this._sprites = destinationArray;
    }
    this._sprites[this._total++] = obj;
    return obj;
  }

  public int countActive() => this._active_index;

  public void debug(DebugTool pTool)
  {
    pTool.setSeparator();
    pTool.setText("count_active", (object) this.count_active_debug);
    pTool.setText("count_total", (object) this._count_total_debug);
    pTool.setSeparator();
    pTool.setText("active_len", (object) this._active_index);
    pTool.setText("used_index", (object) this._used_index);
  }
}

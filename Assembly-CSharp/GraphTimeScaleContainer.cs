// Decompiled with JetBrains decompiler
// Type: GraphTimeScaleContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using db;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class GraphTimeScaleContainer : MonoBehaviour
{
  public GraphTimeScale current_scale;
  private List<GraphTimeScale> _available_time_scales = new List<GraphTimeScale>();
  private GraphController _controller;

  public void calcBounds()
  {
    // ISSUE: unable to decompile the method.
  }

  public bool resetTimeScale()
  {
    this.calcBounds();
    if (this._available_time_scales.Contains(this.current_scale))
      return false;
    this.current_scale = this._available_time_scales.Last<GraphTimeScale>();
    return true;
  }

  public void setTimeScale(GraphTimeScale pScale) => this.current_scale = pScale;

  public ListPool<GraphTimeScale> sharedTimeScales()
  {
    ListPool<GraphTimeScale> listPool = new ListPool<GraphTimeScale>((GraphTimeScale[]) Enum.GetValues(typeof (GraphTimeScale)));
    foreach (NanoObject pObject in this._controller.getObjects())
    {
      using (ListPool<GraphTimeScale> tAvailableTimeScales = DBGetter.getTimeScales(pObject))
        listPool.RemoveAll((Predicate<GraphTimeScale>) (tScale => !tAvailableTimeScales.Contains(tScale)));
    }
    return listPool;
  }

  public bool randomizeTimeScale()
  {
    if (this._available_time_scales.Count < 2)
      return false;
    using (ListPool<GraphTimeScale> list = this.sharedTimeScales())
    {
      if (list.Count == 0)
        return false;
      if (list.Count > 2)
      {
        int num = (int) list.Shift<GraphTimeScale>();
      }
      GraphTimeScale random = list.GetRandom<GraphTimeScale>();
      if (random == this.current_scale)
        return false;
      this.current_scale = random;
      return true;
    }
  }

  public void timeScaleMinus()
  {
    int currentScale = (int) this.current_scale;
    if (currentScale > 0)
      this.current_scale = (GraphTimeScale) (currentScale - 1);
    else
      this.current_scale = (GraphTimeScale) (this._available_time_scales.Count - 1);
  }

  public void timeScalePlus()
  {
    int currentScale = (int) this.current_scale;
    if (currentScale < this._available_time_scales.Count - 1)
      this.current_scale = (GraphTimeScale) (currentScale + 1);
    else
      this.current_scale = GraphTimeScale.year_10;
  }

  public string getIndexString()
  {
    if (this._available_time_scales.Count == 0)
      return "";
    return $" ({((int) (this.current_scale + 1)).ToString()}/{this._available_time_scales.Count.ToString()})";
  }

  public GraphTimeScale getCurrentScale() => this.current_scale;
}

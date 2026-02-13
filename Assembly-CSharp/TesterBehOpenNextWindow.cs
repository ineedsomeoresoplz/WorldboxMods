// Decompiled with JetBrains decompiler
// Type: TesterBehOpenNextWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class TesterBehOpenNextWindow : BehaviourActionTester
{
  private int _current_window;
  private bool _only_meta;
  private bool _random;
  private List<WindowAsset> _windows;

  public TesterBehOpenNextWindow(bool pOnlyMeta = false, bool pRandom = false)
  {
    this._only_meta = pOnlyMeta;
    this._random = pRandom;
  }

  public override BehResult execute(AutoTesterBot pObject)
  {
    if (this._windows == null)
    {
      this._windows = AssetManager.window_library.getTestableWindows();
      if (this._only_meta)
      {
        this._windows = this._windows.FindAll((Predicate<WindowAsset>) (pWindow => pWindow.meta_type_asset != null));
        this._windows = this._windows.FindAll((Predicate<WindowAsset>) (pWindow => !pWindow.id.EndsWith("_customize")));
      }
    }
    this._current_window = !this._random ? Toolbox.loopIndex(this._current_window + 1, this._windows.Count) : Random.Range(0, this._windows.Count);
    WindowAsset window = this._windows[this._current_window];
    if (this._only_meta && window.meta_type_asset == null)
      return BehResult.RepeatStep;
    if (window.meta_type_asset != null)
    {
      NanoObject nanoObject = window.meta_type_asset.get_selected();
      if (nanoObject == null || !nanoObject.isAlive())
        return BehResult.RepeatStep;
    }
    Config.debug_window_stats.setCurrent(window.id);
    ScrollWindow.get(window.id).show(pSkipAnimation: true);
    pObject.wait = 0.1f;
    return BehResult.Continue;
  }
}

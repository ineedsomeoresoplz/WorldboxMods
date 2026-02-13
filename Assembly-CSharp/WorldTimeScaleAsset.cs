// Decompiled with JetBrains decompiler
// Type: WorldTimeScaleAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.ComponentModel;

#nullable disable
[Serializable]
public class WorldTimeScaleAsset : Asset, ILocalizedAsset
{
  public float multiplier;
  [DefaultValue(1)]
  public int ticks = 1;
  public int conway_ticks;
  public bool sonic;
  public bool render_skip;
  public string path_icon;
  public string locale_key;

  public string getLocaleID() => this.locale_key;

  public WorldTimeScaleAsset getNext(bool pCycle = false)
  {
    int num = AssetManager.time_scales.list.Count - 2;
    if (DebugConfig.debug_enabled)
      num = AssetManager.time_scales.list.Count - 1;
    int index;
    if ((index = AssetManager.time_scales.list.IndexOf(this) + 1) > num)
    {
      if (!pCycle)
        return this;
      index = 0;
    }
    return AssetManager.time_scales.list[index];
  }

  public WorldTimeScaleAsset getPrevious(bool pCycle = false)
  {
    int index;
    if ((index = AssetManager.time_scales.list.IndexOf(this) - 1) < 0)
    {
      if (!pCycle)
        return this;
      index = AssetManager.time_scales.list.Count - 1;
    }
    return AssetManager.time_scales.list[index];
  }
}

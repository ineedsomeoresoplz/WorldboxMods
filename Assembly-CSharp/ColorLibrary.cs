// Decompiled with JetBrains decompiler
// Type: ColorLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class ColorLibrary : AssetLibrary<ColorAsset>
{
  private readonly List<ColorAsset> _free_colors_main = new List<ColorAsset>();
  private readonly List<ColorAsset> _free_colors_bonus = new List<ColorAsset>();
  private readonly List<ColorAsset> _free_colors_preferred = new List<ColorAsset>();
  internal bool must_be_global;

  public override void post_init()
  {
    base.post_init();
    foreach (ColorAsset colorAsset in this.list)
      colorAsset.initColor();
  }

  public ColorAsset getColorByIndex(int pIndex)
  {
    return pIndex >= this.list.Count ? this.list[0] : this.list[pIndex];
  }

  public ColorAsset getNextColor(ActorAsset pActorAsset)
  {
    this._free_colors_bonus.Clear();
    this._free_colors_main.Clear();
    this._free_colors_preferred.Clear();
    for (int index = 0; index < this.list.Count; ++index)
    {
      ColorAsset pAsset = this.list[index];
      if (!this.isColorUsedInWorld(pAsset))
      {
        if (pActorAsset != null && pActorAsset.preferred_colors != null && pActorAsset.preferred_colors.Contains(pAsset.id))
          this._free_colors_preferred.Add(pAsset);
        if (pAsset.favorite)
          this._free_colors_main.Add(pAsset);
        else
          this._free_colors_bonus.Add(pAsset);
      }
    }
    if (this._free_colors_preferred.Count > 0)
      return this._free_colors_preferred.GetRandom<ColorAsset>();
    if (this._free_colors_main.Count > 0)
      return this._free_colors_main.GetRandom<ColorAsset>();
    return this._free_colors_bonus.Count > 0 ? this._free_colors_bonus.GetRandom<ColorAsset>() : this.list.GetRandom<ColorAsset>();
  }

  public int getNextColorIndex(ActorAsset pActorAsset)
  {
    return this.list.IndexOf(this.getNextColor(pActorAsset));
  }

  public virtual bool isColorUsedInWorld(ColorAsset pAsset) => false;

  protected bool checkColor(ColorAsset pAsset, int pColorIndex) => pColorIndex == pAsset.index_id;

  public override ColorAsset add(ColorAsset pAsset)
  {
    ColorAsset pAsset1 = base.add(pAsset);
    ColorAsset.saveToGlobalList(pAsset1, this.must_be_global);
    return pAsset1;
  }

  public void useSameColorsFrom(ColorLibrary pSource)
  {
    this.list = pSource.list;
    this.dict = pSource.dict;
  }
}

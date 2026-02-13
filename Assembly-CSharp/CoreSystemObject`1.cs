// Decompiled with JetBrains decompiler
// Type: CoreSystemObject`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable disable
public abstract class CoreSystemObject<TData> : 
  NanoObject,
  ICoreObject,
  ILoadable<TData>,
  IFavoriteable
  where TData : BaseSystemData
{
  public TData data;

  public virtual BaseSystemManager manager => throw new NotImplementedException();

  public bool isFavorite() => this.data.favorite;

  public void switchFavorite() => this.data.favorite = !this.data.favorite;

  public virtual void setFavorite(bool pState) => this.data.favorite = pState;

  public virtual bool updateColor(ColorAsset pColor) => false;

  public virtual void save() => this.data.save();

  public virtual void loadData(TData pData)
  {
    this.setData(pData);
    this.data.load();
  }

  public virtual void setData(TData pData) => this.data = pData;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public override void setAlive(bool pValue)
  {
    this._alive = pValue;
    if (pValue || this.data.died_time != 0.0)
      return;
    this.data.died_time = World.world.getCurWorldTime();
  }

  public override bool hasDied() => this.data.died_time > 0.0;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public virtual int getAge() => Date.getYearsSince(this.data.created_time);

  public override double getFoundedTimestamp() => this.data.created_time;

  public bool isJustCreated()
  {
    return Math.Abs(this.data.created_time - World.world.getCurWorldTime()) <= 0.05000000074505806;
  }

  public string getFoundedDate() => Date.getDate(this.data.created_time);

  public string getDiedDate() => Date.getDate(this.data.died_time);

  public string getFoundedYear() => Date.getYearDate(this.data.created_time);

  public string getDiedYear() => Date.getYearDate(this.data.died_time);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public int getAgeMonths() => Date.getMonthsSince(this.data.created_time);

  public override string name
  {
    get => this.data.name;
    protected set => this.data.name = value;
  }

  public override void trackName(bool pPostChange = false)
  {
    if (string.IsNullOrEmpty(this.data.name) || pPostChange && (this.data.past_names == null || this.data.past_names.Count == 0))
      return;
    BaseSystemData data = (BaseSystemData) this.data;
    if (data.past_names == null)
      data.past_names = new List<NameEntry>();
    if (this.data.past_names.Count == 0)
    {
      this.data.past_names.Add(new NameEntry(this.data.name, false, -1, this.data.created_time));
    }
    else
    {
      if (this.data.past_names.Last<NameEntry>().name == this.data.name)
        return;
      this.data.past_names.Add(new NameEntry(this.data.name, this.data.custom_name));
    }
  }

  public override void Dispose()
  {
    this.data?.Dispose();
    this.data = default (TData);
    base.Dispose();
  }

  public string obsidian_name_id => this.data.obsidian_name_id;

  public sealed override long getID() => this.data.id;

  public string getBirthday() => Date.getDate(this.data.created_time);
}

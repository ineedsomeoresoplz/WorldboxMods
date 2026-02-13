// Decompiled with JetBrains decompiler
// Type: NanoObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Newtonsoft.Json;
using System;
using System.Runtime.CompilerServices;

#nullable disable
public class NanoObject : IComparable<NanoObject>, IEquatable<NanoObject>, IDisposable
{
  protected bool _alive;
  public bool exists;
  protected int _hashcode;
  protected int stats_dirty_version;

  protected virtual MetaType meta_type => throw new NotImplementedException(this.GetType().Name);

  public double created_time_unscaled { get; set; }

  public NanoObject() => this.setDefaultValues();

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void revive() => this.setDefaultValues();

  protected virtual void setDefaultValues()
  {
    this.exists = true;
    this._alive = true;
    this.stats_dirty_version = 0;
    this.created_time_unscaled = 0.0;
  }

  public virtual long getID() => throw new NotImplementedException(this.GetType().Name);

  [JsonProperty(Order = -1)]
  public long id => this.getID();

  public virtual string getType() => this.meta_type.AsString();

  public virtual MetaType getMetaType() => this.meta_type;

  public MetaTypeAsset getMetaTypeAsset() => this.meta_type.getAsset();

  public virtual string getTypeID() => $"{this.getType()}_{this.getID().ToString()}";

  public virtual string name
  {
    get => throw new NotImplementedException(this.GetType().Name);
    protected set => throw new NotImplementedException(this.GetType().Name);
  }

  public void setName(string pName, bool pTrack = true)
  {
    if (pTrack)
      this.trackName();
    this.name = pName;
    if (!pTrack)
      return;
    this.trackName(true);
  }

  public virtual void trackName(bool pPostChange = false)
  {
    throw new NotImplementedException(this.GetType().Name);
  }

  public virtual ColorAsset getColor() => (ColorAsset) null;

  public virtual double getFoundedTimestamp() => 0.0;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isAlive() => this._alive;

  public virtual bool hasDied() => throw new NotImplementedException(this.GetType().Name);

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public virtual void setAlive(bool pValue) => this._alive = pValue;

  public int getStatsDirtyVersion() => this.stats_dirty_version;

  public void setHash(int pHash) => this._hashcode = pHash;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool Equals(NanoObject pObject) => this._hashcode == pObject.GetHashCode();

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public int CompareTo(NanoObject pTarget) => this.GetHashCode().CompareTo(pTarget.GetHashCode());

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public override int GetHashCode() => this._hashcode;

  public virtual void Dispose()
  {
  }
}

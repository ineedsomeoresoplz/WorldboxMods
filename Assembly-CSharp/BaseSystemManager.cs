// Decompiled with JetBrains decompiler
// Type: BaseSystemManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
public abstract class BaseSystemManager
{
  protected static int _latest_hash = 1;
  internal static bool anything_changed = false;

  public virtual void ClearAllDisposed()
  {
  }

  public virtual void parallelDirtyUnitsCheck()
  {
  }

  public virtual void checkLists()
  {
  }

  public virtual void clear()
  {
    this.ClearAllDisposed();
    BaseSystemManager.anything_changed = false;
  }

  public virtual void checkDeadObjects()
  {
  }

  public virtual bool isUnitsDirty() => false;

  public virtual bool isLocked() => this.isUnitsDirty();

  public virtual void startCollectHistoryData()
  {
  }

  public virtual void clearLastYearStats()
  {
  }

  public virtual void showDebugTool(DebugTool pTool)
  {
  }

  public virtual bool hasAny() => this.Count > 0;

  public virtual int Count => throw new NotImplementedException();

  public virtual string debugShort() => $"[c:{this.Count}]";
}

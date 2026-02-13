// Decompiled with JetBrains decompiler
// Type: MultiStackPool`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class MultiStackPool<T> where T : new()
{
  private Dictionary<Type, StackPool<T>> _pools = new Dictionary<Type, StackPool<T>>();

  public U get<U>() where U : T, new()
  {
    Type key = typeof (U);
    StackPool<T> stackPool;
    if (!this._pools.TryGetValue(key, out stackPool))
    {
      stackPool = new StackPool<T>();
      this._pools.Add(key, stackPool);
    }
    return stackPool.get<U>();
  }

  public void release(T pObject)
  {
    StackPool<T> stackPool;
    if (!this._pools.TryGetValue(pObject.GetType(), out stackPool))
      return;
    stackPool.release(pObject);
  }

  public void clear()
  {
    foreach (StackPool<T> stackPool in this._pools.Values)
      stackPool.clear();
    this._pools.Clear();
  }
}

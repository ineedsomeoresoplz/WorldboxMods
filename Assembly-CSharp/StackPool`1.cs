// Decompiled with JetBrains decompiler
// Type: StackPool`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class StackPool<T> where T : new()
{
  public Stack<T> pool = new Stack<T>();

  public T get() => this.pool.Count <= 0 ? new T() : this.pool.Pop();

  public U get<U>() where U : T, new()
  {
    return this.pool.Count <= 0 ? new U() : (U) (object) this.pool.Pop();
  }

  public void release(T pObject) => this.pool.Push(pObject);

  public void clear() => this.pool.Clear();
}

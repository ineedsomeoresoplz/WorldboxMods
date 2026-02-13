// Decompiled with JetBrains decompiler
// Type: ActorVisibleDataArray
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections;

#nullable disable
public class ActorVisibleDataArray
{
  public Actor[] array = new Actor[0];
  public int count;

  public void prepare(int pTargetSize)
  {
    this.array = Toolbox.checkArraySize<Actor>(this.array, pTargetSize);
  }

  public void addFromCollection(ICollection pList)
  {
    if (pList.Count == 0)
      return;
    pList.CopyTo((Array) this.array, this.count);
    this.count += pList.Count;
  }
}

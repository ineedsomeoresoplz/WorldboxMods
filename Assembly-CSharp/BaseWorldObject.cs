// Decompiled with JetBrains decompiler
// Type: BaseWorldObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class BaseWorldObject : MonoBehaviour, IDisposable
{
  internal bool created;
  internal Transform m_transform;

  private void Start()
  {
    if (this.created)
      return;
    this.create();
  }

  public virtual void update(float pElapsed)
  {
  }

  internal virtual void create()
  {
    this.created = true;
    this.m_transform = ((Component) this).gameObject.transform;
  }

  public virtual void Dispose() => this.m_transform = (Transform) null;
}

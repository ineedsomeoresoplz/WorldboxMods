// Decompiled with JetBrains decompiler
// Type: WindowMetaElementBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public abstract class WindowMetaElementBase : MonoBehaviour, IShouldRefreshWindow
{
  protected readonly List<NanoObject> track_objects = new List<NanoObject>();

  protected virtual void Awake()
  {
    this.clearInitial();
    this.clear();
  }

  protected virtual void OnEnable()
  {
    this.clear();
    this.StartCoroutine(this.showContent());
  }

  public virtual void refresh()
  {
    if (!((Component) this).gameObject.activeInHierarchy)
      return;
    this.StopAllCoroutines();
    this.clear();
    this.StartCoroutine(this.showContent());
  }

  protected virtual IEnumerator showContent()
  {
    yield break;
  }

  protected virtual void OnDisable() => this.clear();

  protected virtual void clear() => this.track_objects.Clear();

  protected virtual void clearInitial()
  {
  }

  public virtual bool checkRefreshWindow()
  {
    foreach (NanoObject trackObject in this.track_objects)
    {
      if (trackObject.isRekt())
        return true;
    }
    return false;
  }
}

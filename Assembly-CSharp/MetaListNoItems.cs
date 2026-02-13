// Decompiled with JetBrains decompiler
// Type: MetaListNoItems
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class MetaListNoItems : MonoBehaviour
{
  private GameObject _inner;
  private IMetaWindow _window;

  protected IMetaObject meta_object => this._window.getCoreObject() as IMetaObject;

  private void Awake()
  {
    this._inner = ((Component) ((Component) this).transform.GetChild(0)).gameObject;
    this._window = ((Component) this).GetComponentInParent<IMetaWindow>();
  }

  private void OnEnable() => this._inner.SetActive(!this.hasMetas());

  protected virtual bool hasMetas() => throw new NotImplementedException();
}

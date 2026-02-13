// Decompiled with JetBrains decompiler
// Type: FamiliesNoItems
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FamiliesNoItems : MonoBehaviour
{
  private GameObject _inner;
  private IMetaWithFamiliesWindow _families_window;

  private void Awake()
  {
    this._inner = ((Component) ((Component) this).transform.GetChild(0)).gameObject;
    this._families_window = ((Component) this).GetComponentInParent<IMetaWithFamiliesWindow>();
  }

  private void OnEnable() => this._inner.SetActive(!this._families_window.hasFamilies());
}

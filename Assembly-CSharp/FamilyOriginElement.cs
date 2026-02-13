// Decompiled with JetBrains decompiler
// Type: FamilyOriginElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class FamilyOriginElement : FamilyElement
{
  [SerializeField]
  private GameObject _family_origin_title;
  [SerializeField]
  private FamilyListElement _prefab;
  private ObjectPoolGenericMono<FamilyListElement> _pool_elements;
  [SerializeField]
  private Transform _container;

  protected override void Awake()
  {
    this._pool_elements = new ObjectPoolGenericMono<FamilyListElement>(this._prefab, this._container);
    base.Awake();
  }

  protected override IEnumerator showContent()
  {
    // ISSUE: unable to decompile the method.
  }

  protected override void clear()
  {
    this._pool_elements.clear();
    this._family_origin_title.SetActive(false);
    base.clear();
  }

  protected override void clearInitial()
  {
    for (int index = 0; index < this._container.childCount; ++index)
      Object.Destroy((Object) ((Component) this._container.GetChild(index)).gameObject);
    base.clearInitial();
  }
}

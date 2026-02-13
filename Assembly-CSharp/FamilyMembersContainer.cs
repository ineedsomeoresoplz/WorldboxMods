// Decompiled with JetBrains decompiler
// Type: FamilyMembersContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class FamilyMembersContainer : FamilyElement
{
  private ObjectPoolGenericMono<PrefabUnitElement> _pool_parents;
  private ObjectPoolGenericMono<PrefabUnitElement> _pool_children;
  [SerializeField]
  private RectTransform _list_parents;
  [SerializeField]
  private RectTransform _list_children;
  [SerializeField]
  private LocalizedText _title_parents;
  [SerializeField]
  private LocalizedText _title_children;
  [SerializeField]
  private PrefabUnitElement _prefab;

  protected override void Awake()
  {
    this._pool_children = new ObjectPoolGenericMono<PrefabUnitElement>(this._prefab, (Transform) this._list_children);
    this._pool_parents = new ObjectPoolGenericMono<PrefabUnitElement>(this._prefab, (Transform) this._list_parents);
    base.Awake();
  }

  protected override IEnumerator showContent()
  {
    // ISSUE: unable to decompile the method.
  }

  private void showParents()
  {
    ((Component) this._title_parents).gameObject.SetActive(true);
    ((Component) this._list_parents).gameObject.SetActive(true);
  }

  private void showChildren()
  {
    ((Component) this._title_children).gameObject.SetActive(true);
    ((Component) this._list_children).gameObject.SetActive(true);
  }

  private int sortByMainParent(Actor pActor1, Actor pActor2)
  {
    if (this.family.isMainFounder(pActor1) && !this.family.isMainFounder(pActor2))
      return -1;
    return !this.family.isMainFounder(pActor1) && this.family.isMainFounder(pActor2) ? 1 : 0;
  }

  private void showMember(Actor pActor, ObjectPoolGenericMono<PrefabUnitElement> pPool)
  {
    PrefabUnitElement next = pPool.getNext();
    ((Component) next).transform.localScale = new Vector3(0.9f, 0.9f, 1f);
    next.show(pActor);
  }

  protected override void clear()
  {
    ((Component) this._title_parents).gameObject.SetActive(false);
    ((Component) this._list_parents).gameObject.SetActive(false);
    ((Component) this._title_children).gameObject.SetActive(false);
    ((Component) this._list_children).gameObject.SetActive(false);
    this._pool_children.clear();
    this._pool_parents.clear();
    base.clear();
  }
}

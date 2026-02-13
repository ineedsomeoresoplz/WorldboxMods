// Decompiled with JetBrains decompiler
// Type: ClanMembersContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ClanMembersContainer : ClanElement
{
  private ObjectPoolGenericMono<PrefabUnitElement> _pool_members;
  [SerializeField]
  private RectTransform _list_members;
  [SerializeField]
  private LocalizedText _title_members;
  [SerializeField]
  private PrefabUnitElement _prefab;
  [SerializeField]
  private Text _members_counter;

  protected override void Awake()
  {
    this._pool_members = new ObjectPoolGenericMono<PrefabUnitElement>(this._prefab, (Transform) this._list_members);
    base.Awake();
  }

  protected override IEnumerator showContent()
  {
    // ISSUE: unable to decompile the method.
  }

  private void showMember(Actor pActor)
  {
    PrefabUnitElement next = this._pool_members.getNext();
    ((Component) next).transform.localScale = new Vector3(0.9f, 0.9f, 1f);
    next.show(pActor);
  }

  protected override void clear()
  {
    ((Component) this._title_members).gameObject.SetActive(false);
    ((Component) this._list_members).gameObject.SetActive(false);
    this._pool_members.clear();
    base.clear();
  }
}

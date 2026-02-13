// Decompiled with JetBrains decompiler
// Type: PlotMembers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class PlotMembers : PlotElement
{
  [SerializeField]
  private UiUnitAvatarElement _prefab_avatar;
  [SerializeField]
  private Transform _transform_members;
  private ObjectPoolGenericMono<UiUnitAvatarElement> _pool_members;

  protected override void Awake()
  {
    this._pool_members = new ObjectPoolGenericMono<UiUnitAvatarElement>(this._prefab_avatar, this._transform_members);
    base.Awake();
  }

  protected override IEnumerator showContent()
  {
    // ISSUE: unable to decompile the method.
  }

  private IEnumerator showMember(Actor pActor)
  {
    if (pActor != null)
    {
      yield return (object) new WaitForSecondsRealtime(0.025f);
      UiUnitAvatarElement next = this._pool_members.getNext();
      ((Component) next).transform.localScale = new Vector3(0.6f, 0.6f, 1f);
      next.show(pActor);
    }
  }

  protected override void clear()
  {
    this._pool_members.clear();
    base.clear();
  }
}

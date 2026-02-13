// Decompiled with JetBrains decompiler
// Type: UnitStatusContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class UnitStatusContainer : UnitElement
{
  [SerializeField]
  private StatusEffectButton _prefab_status;
  [SerializeField]
  private Transform _grid;
  private ObjectPoolGenericMono<StatusEffectButton> _pool_status;

  protected override void Awake()
  {
    this._pool_status = new ObjectPoolGenericMono<StatusEffectButton>(this._prefab_status, this._grid);
    base.Awake();
  }

  protected override IEnumerator showContent()
  {
    UnitStatusContainer unitStatusContainer = this;
    if (unitStatusContainer.actor != null && unitStatusContainer.actor.isAlive() && unitStatusContainer.actor.hasAnyStatusEffect())
    {
      ((Component) unitStatusContainer._grid).gameObject.SetActive(true);
      yield return (object) new WaitForSecondsRealtime(0.025f);
      foreach (Status statuse in unitStatusContainer.actor.getStatuses())
      {
        Status tData = statuse;
        if (!tData.is_finished)
        {
          yield return (object) CoroutineHelper.wait_for_next_frame;
          unitStatusContainer.loadStatusButton(tData);
          tData = (Status) null;
        }
      }
    }
  }

  private void loadStatusButton(Status pStatus) => this._pool_status.getNext().load(pStatus);

  protected override void clear()
  {
    this._pool_status?.clear();
    base.clear();
  }
}

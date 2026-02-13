// Decompiled with JetBrains decompiler
// Type: ActorSelectedContainerStatus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ActorSelectedContainerStatus : SelectedElementBase<StatusEffectButton>
{
  [SerializeField]
  private StatusEffectButton _prefab_status;

  private void Awake()
  {
    this._pool = new ObjectPoolGenericMono<StatusEffectButton>(this._prefab_status, this._grid);
  }

  public void update(NanoObject pNano) => this.refresh(pNano);

  protected override void refresh(NanoObject pNano)
  {
    this.clear();
    foreach (Status statuse in ((BaseSimObject) pNano).getStatuses())
    {
      if (!statuse.is_finished)
        this.loadStatusButton(statuse);
    }
  }

  private void loadStatusButton(Status pStatus)
  {
    StatusEffectButton next = this._pool.getNext();
    next.load(pStatus);
    next.setUpdatableTooltip(true);
  }
}

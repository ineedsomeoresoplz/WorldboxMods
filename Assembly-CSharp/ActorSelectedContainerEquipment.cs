// Decompiled with JetBrains decompiler
// Type: ActorSelectedContainerEquipment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ActorSelectedContainerEquipment : SelectedElementBase<EquipmentButton>
{
  [SerializeField]
  private EquipmentButton _prefab_equipment;

  private void Awake()
  {
    this._pool = new ObjectPoolGenericMono<EquipmentButton>(this._prefab_equipment, this._grid);
  }

  public void update(Actor pActor)
  {
    if (!pActor.hasEquipment())
      this.clear();
    else
      this.refresh((NanoObject) pActor);
  }

  protected override void refresh(NanoObject pNano)
  {
    this.clear();
    foreach (Item pItem in ((Actor) pNano).equipment.getItems())
      this.loadEquipmentButton(pItem);
  }

  private void loadEquipmentButton(Item pItem) => this._pool.getNext().load(pItem);
}

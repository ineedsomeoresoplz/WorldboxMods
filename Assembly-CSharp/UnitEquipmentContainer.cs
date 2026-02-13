// Decompiled with JetBrains decompiler
// Type: UnitEquipmentContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class UnitEquipmentContainer : UnitElement
{
  [SerializeField]
  private EquipmentButton _prefab_equipment;
  [SerializeField]
  private Transform _grid;
  private ObjectPoolGenericMono<EquipmentButton> _pool_equipment;
  private Dictionary<Item, EquipmentButton> _items = new Dictionary<Item, EquipmentButton>();

  protected override void Awake()
  {
    this._pool_equipment = new ObjectPoolGenericMono<EquipmentButton>(this._prefab_equipment, this._grid);
    base.Awake();
  }

  protected override IEnumerator showContent()
  {
    UnitEquipmentContainer equipmentContainer = this;
    if (equipmentContainer.actor != null && equipmentContainer.actor.isAlive())
      yield return (object) equipmentContainer.loadEquipment();
  }

  private IEnumerator loadEquipment(bool pAnimated = true)
  {
    // ISSUE: unable to decompile the method.
  }

  private void loadEquipmentButton(Item pItem)
  {
    EquipmentButton next = this._pool_equipment.getNext();
    next.load(pItem);
    this._items[pItem] = next;
    AugmentationUnlockedAction pAction = new AugmentationUnlockedAction(((IAugmentationsEditor) this.unit_window.getEditor()).reloadButtons);
    next.removeElementUnlockedAction(pAction);
    next.addElementUnlockedAction(pAction);
  }

  protected override void clear()
  {
    this._items.Clear();
    this._pool_equipment?.clear();
    base.clear();
  }

  protected override void clearInitial()
  {
    for (int index = 0; index < this._grid.childCount; ++index)
    {
      Transform child = this._grid.GetChild(index);
      if (!(((Object) child).name == "Title"))
        Object.Destroy((Object) ((Component) child).gameObject);
    }
    base.clearInitial();
  }

  public void reloadEquipment(bool pAnimated)
  {
    this.StopAllCoroutines();
    this.StartCoroutine(this.loadEquipment(pAnimated));
  }
}

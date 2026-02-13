// Decompiled with JetBrains decompiler
// Type: UiCityEquipment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class UiCityEquipment : CitySortableElement
{
  [SerializeField]
  private EquipmentType _equipment_type;
  [SerializeField]
  private EquipmentButton _prefab_equipment;
  private ObjectPoolGenericMono<EquipmentButton> _pool_equipment;
  private Dictionary<long, EquipmentButton> _equipment = new Dictionary<long, EquipmentButton>();

  protected override void Awake()
  {
    this._pool_equipment = new ObjectPoolGenericMono<EquipmentButton>(this._prefab_equipment, ((Component) this).transform);
    base.Awake();
  }

  protected override IEnumerator showContent()
  {
    // ISSUE: unable to decompile the method.
  }

  private void loadEquipmentButton(Item pItem, long pItemID)
  {
    EquipmentButton next = this._pool_equipment.getNext();
    next.load(pItem);
    this._equipment[pItemID] = next;
  }

  protected override void onListChange()
  {
    List<long> equipmentList = this.city.getEquipmentList(this._equipment_type);
    if (!equipmentList.SetEquals<long>((IEnumerable<long>) this._equipment.Keys))
      return;
    equipmentList.Sort((Comparison<long>) ((a, b) => ((Component) this._equipment[a]).transform.GetSiblingIndex().CompareTo(((Component) this._equipment[b]).transform.GetSiblingIndex())));
  }

  protected override void clear()
  {
    this._equipment.Clear();
    this._pool_equipment.clear();
    base.clear();
  }

  protected override void clearInitial()
  {
    for (int index = 0; index < ((Component) this).transform.childCount; ++index)
    {
      Transform child = ((Component) this).transform.GetChild(index);
      if (!(((Object) child).name == "Title"))
        Object.Destroy((Object) ((Component) child).gameObject);
    }
    base.clearInitial();
  }
}

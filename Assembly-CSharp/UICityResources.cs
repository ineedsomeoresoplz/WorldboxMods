// Decompiled with JetBrains decompiler
// Type: UICityResources
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class UICityResources : CitySortableElement
{
  [SerializeField]
  private ResType[] _res_types;
  [SerializeField]
  private ButtonResource _prefab_resource;
  private ObjectPoolGenericMono<ButtonResource> _pool_resources;
  private Dictionary<CityStorageSlot, ButtonResource> _loaded_slots = new Dictionary<CityStorageSlot, ButtonResource>();

  protected override void Awake()
  {
    this._pool_resources = new ObjectPoolGenericMono<ButtonResource>(this._prefab_resource, ((Component) this).transform);
    base.Awake();
  }

  protected override IEnumerator showContent()
  {
    this.showResources();
    yield return (object) new WaitForEndOfFrame();
  }

  protected void showResources()
  {
    // ISSUE: unable to decompile the method.
  }

  private void loadResource(CityStorageSlot pSlot)
  {
    ButtonResource next = this._pool_resources.getNext();
    next.load(pSlot.asset, pSlot.amount);
    this._loaded_slots[pSlot] = next;
  }

  protected override void onListChange()
  {
    // ISSUE: unable to decompile the method.
  }

  protected override void clear()
  {
    this._loaded_slots.Clear();
    this._pool_resources.clear();
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

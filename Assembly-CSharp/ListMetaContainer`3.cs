// Decompiled with JetBrains decompiler
// Type: ListMetaContainer`3
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ListMetaContainer<TListElement, TMetaObject, TMetaData> : WindowMetaElementBase
  where TListElement : WindowListElementBase<TMetaObject, TMetaData>
  where TMetaObject : CoreSystemObject<TMetaData>
  where TMetaData : BaseSystemData
{
  [SerializeField]
  private TListElement _prefab;
  [SerializeField]
  private Transform _container;
  private StatsWindow _window;
  private ObjectPoolGenericMono<TListElement> _pool_elements;

  protected override void Awake()
  {
    this._window = ((Component) this).GetComponentInParent<StatsWindow>();
    this._pool_elements = new ObjectPoolGenericMono<TListElement>(this._prefab, this._container);
    base.Awake();
  }

  protected override IEnumerator showContent()
  {
    ListMetaContainer<TListElement, TMetaObject, TMetaData> listMetaContainer = this;
    using (ListPool<TMetaObject> tListObjects = new ListPool<TMetaObject>(listMetaContainer.getMetaList()))
    {
      listMetaContainer.track_objects.AddRange((IEnumerable<NanoObject>) tListObjects);
      tListObjects.Sort(listMetaContainer.getSorting());
      for (int i = 0; i < tListObjects.Count; ++i)
      {
        TMetaObject tObject = tListObjects[i];
        yield return (object) new WaitForSecondsRealtime(0.025f);
        listMetaContainer.showElement(tObject);
        tObject = default (TMetaObject);
      }
    }
  }

  private void showElement(TMetaObject pMeta) => this._pool_elements.getNext().show(pMeta);

  protected override void clear()
  {
    this._pool_elements.clear();
    base.clear();
  }

  protected IMetaObject getMeta()
  {
    return AssetManager.meta_type_library.getAsset(this._window.meta_type).get_selected() as IMetaObject;
  }

  protected virtual IEnumerable<TMetaObject> getMetaList() => throw new NotImplementedException();

  protected virtual Comparison<TMetaObject> getSorting() => throw new NotImplementedException();
}

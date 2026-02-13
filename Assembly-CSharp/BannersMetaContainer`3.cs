// Decompiled with JetBrains decompiler
// Type: BannersMetaContainer`3
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BannersMetaContainer<TMetaBanner, TMetaObject, TMetaData> : WindowMetaElementBase
  where TMetaBanner : BannerGeneric<TMetaObject, TMetaData>
  where TMetaObject : CoreSystemObject<TMetaData>
  where TMetaData : BaseSystemData
{
  [SerializeField]
  private TMetaBanner _prefab;
  [SerializeField]
  private Transform _container;
  private StatsWindow _window;
  private ObjectPoolGenericMono<TMetaBanner> _pool_elements;

  protected override void Awake()
  {
    base.Awake();
    this._pool_elements = new ObjectPoolGenericMono<TMetaBanner>(this._prefab, this._container);
  }

  protected override void OnEnable()
  {
  }

  public void update(NanoObject pNano)
  {
    this.clear();
    this._pool_elements.clear();
    this.showContent(pNano);
  }

  private void showContent(NanoObject pNano)
  {
    using (ListPool<TMetaObject> listPool = new ListPool<TMetaObject>(this.getMetaList(pNano as IMetaObject)))
    {
      for (int index = 0; index < listPool.Count; ++index)
      {
        TMetaObject pMeta = listPool[index];
        this.track_objects.Add((NanoObject) pMeta);
        this.showElement(pMeta);
      }
    }
  }

  private void showElement(TMetaObject pMeta)
  {
    TMetaBanner next = this._pool_elements.getNext();
    next.enable_tab_show_click = true;
    next.enable_default_click = false;
    if (!((Component) (object) next).HasComponent<DraggableLayoutElement>())
      ((Component) (object) next).AddComponent<DraggableLayoutElement>();
    next.load((NanoObject) pMeta);
  }

  protected virtual IEnumerable<TMetaObject> getMetaList(IMetaObject pMeta)
  {
    throw new NotImplementedException();
  }
}

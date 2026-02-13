// Decompiled with JetBrains decompiler
// Type: MultiBannerPool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MultiBannerPool
{
  private Dictionary<string, ObjectPoolGenericMono<MonoBehaviour>> _pool_banners;
  private Transform _pool_container;
  private Transform _prefab_area;

  public MultiBannerPool(Transform pPoolContainer)
  {
    this._pool_banners = new Dictionary<string, ObjectPoolGenericMono<MonoBehaviour>>();
    this._pool_container = pPoolContainer;
    GameObject gameObject = new GameObject("PrefabArea", new Type[1]
    {
      typeof (RectTransform)
    });
    gameObject.transform.SetParent(this._pool_container);
    this._prefab_area = gameObject.transform;
    ((Component) this._prefab_area).gameObject.SetActive(false);
  }

  public IBanner getNext(NanoObject pObject)
  {
    string type = pObject.getType();
    MetaCustomizationAsset pAsset = AssetManager.meta_customization_library.get(type);
    ObjectPoolGenericMono<MonoBehaviour> poolBanner;
    if (!this._pool_banners.TryGetValue(type, out poolBanner))
    {
      GameObject gameObject = new GameObject("BannerArea " + type, new Type[1]
      {
        typeof (RectTransform)
      });
      gameObject.transform.SetParent(this._pool_container, false);
      MonoBehaviour pPrefab = (MonoBehaviour) pAsset.get_banner(pAsset, pObject, this._prefab_area);
      ((Object) ((Component) pPrefab).gameObject).name = type;
      this._pool_banners.Add(type, new ObjectPoolGenericMono<MonoBehaviour>(pPrefab, gameObject.transform));
      poolBanner = this._pool_banners[type];
    }
    return poolBanner.getNext() as IBanner;
  }

  public void release(IBanner pItem) => this.getItemPool(pItem).release(pItem as MonoBehaviour);

  public void resetParent(IBanner pItem)
  {
    this.getItemPool(pItem).resetParent(pItem as MonoBehaviour);
  }

  private ObjectPoolGenericMono<MonoBehaviour> getItemPool(IBanner pItem)
  {
    ObjectPoolGenericMono<MonoBehaviour> objectPoolGenericMono;
    return this._pool_banners.TryGetValue(pItem.meta_asset.id, out objectPoolGenericMono) ? objectPoolGenericMono : (ObjectPoolGenericMono<MonoBehaviour>) null;
  }

  public void clear()
  {
    foreach (ObjectPoolGenericMono<MonoBehaviour> objectPoolGenericMono in this._pool_banners.Values)
    {
      objectPoolGenericMono.clear();
      objectPoolGenericMono.resetParent();
    }
  }
}

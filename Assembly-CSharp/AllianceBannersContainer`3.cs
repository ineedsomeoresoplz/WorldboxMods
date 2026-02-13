// Decompiled with JetBrains decompiler
// Type: AllianceBannersContainer`3
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class AllianceBannersContainer<TBanner, TMetaObject, TData> : AllianceElement
  where TBanner : BannerGeneric<TMetaObject, TData>
  where TMetaObject : CoreSystemObject<TData>
  where TData : BaseSystemData
{
  protected ObjectPoolGenericMono<TBanner> pool_elements;
  [SerializeField]
  private TBanner _prefab;
  [SerializeField]
  private Transform _container;

  protected override void Awake()
  {
    this.pool_elements = new ObjectPoolGenericMono<TBanner>(this._prefab, this._container);
    base.Awake();
  }

  protected override void clear()
  {
    this.pool_elements.clear();
    base.clear();
  }
}

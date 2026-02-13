// Decompiled with JetBrains decompiler
// Type: KingdomSelectedContainerCities
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class KingdomSelectedContainerCities : SelectedElementBase<CityBanner>
{
  [SerializeField]
  private CityBanner _banner_prefab;

  private void Awake()
  {
    this._pool = new ObjectPoolGenericMono<CityBanner>(this._banner_prefab, this._grid);
    ((Component) this._grid).gameObject.AddOrGetComponent<TraitsGrid>();
  }

  public void update(NanoObject pNano) => this.refresh(pNano);

  protected override void refresh(NanoObject pNano)
  {
    this.clear();
    foreach (City city in ((MetaObject<KingdomData>) pNano).getCities())
      this.addBanner(city);
  }

  private void addBanner(City pCity) => this._pool.getNext().load((NanoObject) pCity);
}

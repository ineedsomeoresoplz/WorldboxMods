// Decompiled with JetBrains decompiler
// Type: KingdomCitiesContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class KingdomCitiesContainer : KingdomElement
{
  private ObjectPoolGenericMono<CityListElement> _pool_elements;
  [SerializeField]
  private CityListElement _prefab;

  protected override void Awake()
  {
    this._pool_elements = new ObjectPoolGenericMono<CityListElement>(this._prefab, ((Component) this).transform);
    base.Awake();
  }

  protected override IEnumerator showContent()
  {
    KingdomCitiesContainer kingdomCitiesContainer = this;
    using (ListPool<City> tListCities = new ListPool<City>(kingdomCitiesContainer.kingdom.getCities()))
    {
      kingdomCitiesContainer.track_objects.AddRange((IEnumerable<NanoObject>) tListCities);
      tListCities.Sort(new Comparison<City>(ComponentListBase<CityListElement, City, CityData, CityListComponent>.sortByPopulation));
      for (int i = 0; i < tListCities.Count; ++i)
      {
        City tCity = tListCities[i];
        if (!tCity.isCapitalCity())
        {
          yield return (object) new WaitForSecondsRealtime(0.025f);
          kingdomCitiesContainer.showCityElement(tCity);
          tCity = (City) null;
        }
      }
    }
  }

  private void showCityElement(City pCity) => this._pool_elements.getNext().show(pCity);

  protected override void clear()
  {
    this._pool_elements.clear();
    base.clear();
  }
}

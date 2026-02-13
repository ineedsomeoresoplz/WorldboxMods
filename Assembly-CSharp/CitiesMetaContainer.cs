// Decompiled with JetBrains decompiler
// Type: CitiesMetaContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class CitiesMetaContainer : ListMetaContainer<CityListElement, City, CityData>
{
  protected override IEnumerable<City> getMetaList() => this.getMeta().getCities();

  protected override Comparison<City> getSorting()
  {
    return new Comparison<City>(ComponentListBase<CityListElement, City, CityData, CityListComponent>.sortByPopulation);
  }
}

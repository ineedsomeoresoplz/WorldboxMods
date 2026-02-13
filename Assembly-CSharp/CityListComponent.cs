// Decompiled with JetBrains decompiler
// Type: CityListComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
public class CityListComponent : 
  ComponentListBase<CityListElement, City, CityData, CityListComponent>
{
  protected override MetaType meta_type => MetaType.City;

  protected override void setupSortingTabs()
  {
    this.genericMetaSortByAge(new Comparison<City>(((ComponentListBase<CityListElement, City, CityData, CityListComponent>) this).sortByAge));
    this.genericMetaSortByRenown(new Comparison<City>(((ComponentListBase<CityListElement, City, CityData, CityListComponent>) this).sortByRenown));
    this.genericMetaSortByPopulation(new Comparison<City>(ComponentListBase<CityListElement, City, CityData, CityListComponent>.sortByPopulation));
    this.sorting_tab.tryAddButton("ui/Icons/iconLoyalty", "sort_by_loyalty", new SortButtonAction(((ComponentListBase<CityListElement, City, CityData, CityListComponent>) this).show), (SortButtonAction) (() => this.current_sort = new Comparison<City>(CityListComponent.sortByLoyalty)));
    this.sorting_tab.tryAddButton("ui/Icons/iconZones", "sort_by_area", new SortButtonAction(((ComponentListBase<CityListElement, City, CityData, CityListComponent>) this).show), (SortButtonAction) (() => this.current_sort = new Comparison<City>(CityListComponent.sortByArea)));
    this.sorting_tab.tryAddButton("ui/Icons/iconArmy", "sort_by_army", new SortButtonAction(((ComponentListBase<CityListElement, City, CityData, CityListComponent>) this).show), (SortButtonAction) (() => this.current_sort = new Comparison<City>(CityListComponent.sortByArmy)));
    this.sorting_tab.tryAddButton("ui/Icons/iconKingdom", "sort_by_kingdom", new SortButtonAction(((ComponentListBase<CityListElement, City, CityData, CityListComponent>) this).show), (SortButtonAction) (() => this.current_sort = new Comparison<City>(CityListComponent.sortByKingdom)));
  }

  private static int sortByKingdom(City p1, City p2)
  {
    return p2.kingdom.CompareTo((NanoObject) p1.kingdom);
  }

  private static int sortByArmy(City p1, City p2)
  {
    return p2.countWarriors().CompareTo(p1.countWarriors());
  }

  private static int sortByLoyalty(City p1, City p2)
  {
    return p2.getCachedLoyalty().CompareTo(p1.getCachedLoyalty());
  }

  private static int sortByArea(City p1, City p2) => p2.zones.Count.CompareTo(p1.zones.Count);
}

// Decompiled with JetBrains decompiler
// Type: ReligionListComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
public class ReligionListComponent : 
  ComponentListBase<ReligionListElement, Religion, ReligionData, ReligionListComponent>
{
  protected override MetaType meta_type => MetaType.Religion;

  protected override void setupSortingTabs()
  {
    this.genericMetaSortByAge(new Comparison<Religion>(((ComponentListBase<ReligionListElement, Religion, ReligionData, ReligionListComponent>) this).sortByAge));
    this.genericMetaSortByRenown(new Comparison<Religion>(((ComponentListBase<ReligionListElement, Religion, ReligionData, ReligionListComponent>) this).sortByRenown));
    this.genericMetaSortByPopulation(new Comparison<Religion>(ComponentListBase<ReligionListElement, Religion, ReligionData, ReligionListComponent>.sortByPopulation));
    this.genericMetaSortByKills(new Comparison<Religion>(ComponentListBase<ReligionListElement, Religion, ReligionData, ReligionListComponent>.sortByKills));
    this.genericMetaSortByDeath(new Comparison<Religion>(ComponentListBase<ReligionListElement, Religion, ReligionData, ReligionListComponent>.sortByDeaths));
    this.sorting_tab.tryAddButton("ui/Icons/iconVillages", "sort_by_villages", new SortButtonAction(((ComponentListBase<ReligionListElement, Religion, ReligionData, ReligionListComponent>) this).show), (SortButtonAction) (() => this.current_sort = new Comparison<Religion>(ReligionListComponent.sortByVillages)));
  }

  public static int sortByVillages(Religion p1, Religion p2)
  {
    return p2.cities.Count.CompareTo(p1.cities.Count);
  }
}

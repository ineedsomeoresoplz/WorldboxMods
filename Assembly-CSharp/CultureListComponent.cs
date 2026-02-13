// Decompiled with JetBrains decompiler
// Type: CultureListComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
public class CultureListComponent : 
  ComponentListBase<CultureListElement, Culture, CultureData, CultureListComponent>
{
  protected override MetaType meta_type => MetaType.Culture;

  protected override void setupSortingTabs()
  {
    this.genericMetaSortByAge(new Comparison<Culture>(((ComponentListBase<CultureListElement, Culture, CultureData, CultureListComponent>) this).sortByAge));
    this.genericMetaSortByRenown(new Comparison<Culture>(((ComponentListBase<CultureListElement, Culture, CultureData, CultureListComponent>) this).sortByRenown));
    this.genericMetaSortByPopulation(new Comparison<Culture>(ComponentListBase<CultureListElement, Culture, CultureData, CultureListComponent>.sortByPopulation));
    this.genericMetaSortByKills(new Comparison<Culture>(ComponentListBase<CultureListElement, Culture, CultureData, CultureListComponent>.sortByKills));
    this.genericMetaSortByDeath(new Comparison<Culture>(ComponentListBase<CultureListElement, Culture, CultureData, CultureListComponent>.sortByDeaths));
    this.sorting_tab.tryAddButton("ui/Icons/iconVillages", "sort_by_villages", new SortButtonAction(((ComponentListBase<CultureListElement, Culture, CultureData, CultureListComponent>) this).show), (SortButtonAction) (() => this.current_sort = new Comparison<Culture>(CultureListComponent.sortByVillages)));
  }

  public static int sortByVillages(Culture pCulture1, Culture pCulture2)
  {
    return pCulture2.countCities().CompareTo(pCulture1.countCities());
  }
}

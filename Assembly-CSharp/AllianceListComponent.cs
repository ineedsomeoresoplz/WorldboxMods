// Decompiled with JetBrains decompiler
// Type: AllianceListComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
public class AllianceListComponent : 
  ComponentListBase<AllianceListElement, Alliance, AllianceData, AllianceListComponent>
{
  protected override MetaType meta_type => MetaType.Alliance;

  protected override void setupSortingTabs()
  {
    this.genericMetaSortByAge(new Comparison<Alliance>(((ComponentListBase<AllianceListElement, Alliance, AllianceData, AllianceListComponent>) this).sortByAge));
    this.genericMetaSortByRenown(new Comparison<Alliance>(((ComponentListBase<AllianceListElement, Alliance, AllianceData, AllianceListComponent>) this).sortByRenown));
    this.genericMetaSortByPopulation(new Comparison<Alliance>(ComponentListBase<AllianceListElement, Alliance, AllianceData, AllianceListComponent>.sortByPopulation));
    this.sorting_tab.tryAddButton("ui/Icons/iconArmy", "sort_by_army", new SortButtonAction(((ComponentListBase<AllianceListElement, Alliance, AllianceData, AllianceListComponent>) this).show), (SortButtonAction) (() => this.current_sort = new Comparison<Alliance>(AllianceListComponent.sortByArmy)));
    this.sorting_tab.tryAddButton("ui/Icons/iconKingdomList", "sort_by_kingdoms", new SortButtonAction(((ComponentListBase<AllianceListElement, Alliance, AllianceData, AllianceListComponent>) this).show), (SortButtonAction) (() => this.current_sort = new Comparison<Alliance>(AllianceListComponent.sortByKingdoms)));
    this.sorting_tab.tryAddButton("ui/Icons/iconVillages", "sort_by_villages", new SortButtonAction(((ComponentListBase<AllianceListElement, Alliance, AllianceData, AllianceListComponent>) this).show), (SortButtonAction) (() => this.current_sort = new Comparison<Alliance>(AllianceListComponent.sortByVillages)));
  }

  public static int sortByArmy(Alliance pAlliance1, Alliance pAlliance2)
  {
    return pAlliance2.countWarriors().CompareTo(pAlliance1.countWarriors());
  }

  public static int sortByKingdoms(Alliance pAlliance1, Alliance pAlliance2)
  {
    return pAlliance2.countKingdoms().CompareTo(pAlliance1.countKingdoms());
  }

  public static int sortByVillages(Alliance pAlliance1, Alliance pAlliance2)
  {
    return pAlliance2.countCities().CompareTo(pAlliance1.countCities());
  }
}

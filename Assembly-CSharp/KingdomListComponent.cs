// Decompiled with JetBrains decompiler
// Type: KingdomListComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
public class KingdomListComponent : 
  ComponentListBase<KingdomListElement, Kingdom, KingdomData, KingdomListComponent>
{
  protected override MetaType meta_type => MetaType.Kingdom;

  protected override void setupSortingTabs()
  {
    this.genericMetaSortByAge(new Comparison<Kingdom>(((ComponentListBase<KingdomListElement, Kingdom, KingdomData, KingdomListComponent>) this).sortByAge));
    this.genericMetaSortByRenown(new Comparison<Kingdom>(((ComponentListBase<KingdomListElement, Kingdom, KingdomData, KingdomListComponent>) this).sortByRenown));
    this.genericMetaSortByPopulation(new Comparison<Kingdom>(ComponentListBase<KingdomListElement, Kingdom, KingdomData, KingdomListComponent>.sortByPopulation));
    this.sorting_tab.tryAddButton("ui/Icons/iconArmy", "sort_by_army", new SortButtonAction(((ComponentListBase<KingdomListElement, Kingdom, KingdomData, KingdomListComponent>) this).show), (SortButtonAction) (() => this.current_sort = new Comparison<Kingdom>(KingdomListComponent.sortByArmy)));
    this.sorting_tab.tryAddButton("ui/Icons/iconChildren", "sort_by_children", new SortButtonAction(((ComponentListBase<KingdomListElement, Kingdom, KingdomData, KingdomListComponent>) this).show), (SortButtonAction) (() => this.current_sort = new Comparison<Kingdom>(KingdomListComponent.sortByChildren)));
    this.sorting_tab.tryAddButton("ui/Icons/iconVillages", "sort_by_villages", new SortButtonAction(((ComponentListBase<KingdomListElement, Kingdom, KingdomData, KingdomListComponent>) this).show), (SortButtonAction) (() => this.current_sort = new Comparison<Kingdom>(KingdomListComponent.sortByCities)));
    this.sorting_tab.tryAddButton("ui/Icons/iconZones", "sort_by_area", new SortButtonAction(((ComponentListBase<KingdomListElement, Kingdom, KingdomData, KingdomListComponent>) this).show), (SortButtonAction) (() => this.current_sort = new Comparison<Kingdom>(KingdomListComponent.sortByArea)));
  }

  private static int sortByArea(Kingdom p1, Kingdom p2)
  {
    return p2.countZones().CompareTo(p1.countZones());
  }

  public static int sortByArmy(Kingdom p1, Kingdom p2)
  {
    return p2.countTotalWarriors().CompareTo(p1.countTotalWarriors());
  }

  private static int sortByChildren(Kingdom p1, Kingdom p2)
  {
    return p2.countChildren().CompareTo(p1.countChildren());
  }

  private static int sortByCities(Kingdom p1, Kingdom p2)
  {
    return p2.countCities().CompareTo(p1.countCities());
  }
}

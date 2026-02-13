// Decompiled with JetBrains decompiler
// Type: ArmyListComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
public class ArmyListComponent : 
  ComponentListBase<ArmyListElement, Army, ArmyData, ArmyListComponent>
{
  protected override MetaType meta_type => MetaType.Army;

  protected override void setupSortingTabs()
  {
    this.genericMetaSortByAge(new Comparison<Army>(((ComponentListBase<ArmyListElement, Army, ArmyData, ArmyListComponent>) this).sortByAge));
    this.genericMetaSortByRenown(new Comparison<Army>(((ComponentListBase<ArmyListElement, Army, ArmyData, ArmyListComponent>) this).sortByRenown));
    this.genericMetaSortByPopulation(new Comparison<Army>(ComponentListBase<ArmyListElement, Army, ArmyData, ArmyListComponent>.sortByPopulation));
    this.genericMetaSortByKills(new Comparison<Army>(ComponentListBase<ArmyListElement, Army, ArmyData, ArmyListComponent>.sortByKills));
    this.genericMetaSortByDeath(new Comparison<Army>(ComponentListBase<ArmyListElement, Army, ArmyData, ArmyListComponent>.sortByDeaths));
    this.sorting_tab.tryAddButton("ui/Icons/iconKingdom", "sort_by_kingdom", new SortButtonAction(((ComponentListBase<ArmyListElement, Army, ArmyData, ArmyListComponent>) this).show), (SortButtonAction) (() => this.current_sort = new Comparison<Army>(ArmyListComponent.sortByKingdom)));
  }

  private static int sortByKingdom(Army p1, Army p2)
  {
    return p2.getKingdom().CompareTo((NanoObject) p1.getKingdom());
  }
}

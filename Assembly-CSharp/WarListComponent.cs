// Decompiled with JetBrains decompiler
// Type: WarListComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class WarListComponent : ComponentListBase<WarListElement, War, WarData, WarListComponent>
{
  protected override MetaType meta_type => MetaType.War;

  protected override void setupSortingTabs()
  {
    SortButton sortButton = this.sorting_tab.tryAddButton("ui/Icons/iconAge", "sort_by_age", new SortButtonAction(((ComponentListBase<WarListElement, War, WarData, WarListComponent>) this).show), (SortButtonAction) (() => this.current_sort = new Comparison<War>(WarListComponent.sortByAge)));
    this.sorting_tab.tryAddButton("ui/Icons/iconClock", "sort_by_duration", new SortButtonAction(((ComponentListBase<WarListElement, War, WarData, WarListComponent>) this).show), (SortButtonAction) (() => this.current_sort = new Comparison<War>(WarListComponent.sortByDuration)));
    if (this.getCurrentFilter() != ListItemsFilter.OnlyAlive)
      this.sorting_tab.tryAddButton("ui/Icons/iconDeadKingdom", "sort_by_ended", new SortButtonAction(((ComponentListBase<WarListElement, War, WarData, WarListComponent>) this).show), (SortButtonAction) (() => this.current_sort = new Comparison<War>(WarListComponent.sortByEndedTime)));
    this.sorting_tab.tryAddButton("ui/Icons/iconRenown", "sort_by_renown", new SortButtonAction(((ComponentListBase<WarListElement, War, WarData, WarListComponent>) this).show), (SortButtonAction) (() => this.current_sort = new Comparison<War>(WarListComponent.sortByRenown)));
    this.sorting_tab.tryAddButton("ui/Icons/iconArmy", "sort_by_army", new SortButtonAction(((ComponentListBase<WarListElement, War, WarData, WarListComponent>) this).show), (SortButtonAction) (() => this.current_sort = new Comparison<War>(WarListComponent.sortByArmy)));
    this.sorting_tab.tryAddButton("ui/Icons/iconKills", "sort_by_dead", new SortButtonAction(((ComponentListBase<WarListElement, War, WarData, WarListComponent>) this).show), (SortButtonAction) (() => this.current_sort = new Comparison<War>(WarListComponent.sortByDead)));
    this.sorting_tab.tryAddButton("ui/Icons/iconPopulation", "sort_by_population", new SortButtonAction(((ComponentListBase<WarListElement, War, WarData, WarListComponent>) this).show), (SortButtonAction) (() => this.current_sort = new Comparison<War>(WarListComponent.sortByPopulation)));
    if (!Object.op_Inequality((Object) sortButton, (Object) null))
      return;
    sortButton.click();
    sortButton.click();
  }

  public static int sortByRenown(War pWar1, War pWar2)
  {
    return WarListComponent.sortByEnded(pWar1, pWar2) != 0 ? WarListComponent.sortByEnded(pWar1, pWar2) : pWar2.getRenown().CompareTo(pWar1.getRenown());
  }

  public static int sortByDuration(War pWar1, War pWar2)
  {
    return WarListComponent.sortByEnded(pWar1, pWar2) != 0 ? WarListComponent.sortByEnded(pWar1, pWar2) : -pWar2.getDuration().CompareTo(pWar1.getDuration());
  }

  public static int sortByAge(War pWar1, War pWar2)
  {
    return WarListComponent.sortByEnded(pWar1, pWar2) != 0 ? WarListComponent.sortByEnded(pWar1, pWar2) : -pWar2.data.created_time.CompareTo(pWar1.data.created_time);
  }

  public static int sortByArmy(War pWar1, War pWar2)
  {
    return WarListComponent.sortByEnded(pWar1, pWar2) != 0 ? WarListComponent.sortByEnded(pWar1, pWar2) : pWar1.countTotalArmy().CompareTo(pWar2.countTotalArmy());
  }

  public static int sortByPopulation(War pWar1, War pWar2)
  {
    return WarListComponent.sortByEnded(pWar1, pWar2) != 0 ? WarListComponent.sortByEnded(pWar1, pWar2) : pWar1.countTotalPopulation().CompareTo(pWar2.countTotalPopulation());
  }

  public static int sortByEndedTime(War pWar1, War pWar2)
  {
    return WarListComponent.sortByEnded(pWar1, pWar2) == 0 ? WarListComponent.sortByAge(pWar1, pWar2) : pWar2.data.died_time.CompareTo(pWar1.data.died_time);
  }

  public static int sortByDead(War pWar1, War pWar2)
  {
    return WarListComponent.sortByEnded(pWar1, pWar2) != 0 ? WarListComponent.sortByEnded(pWar1, pWar2) : pWar2.data.total_deaths.CompareTo(pWar1.getTotalDeaths());
  }

  private static int sortByEnded(War pWar1, War pWar2)
  {
    return pWar1.hasEnded().CompareTo(pWar2.hasEnded());
  }
}

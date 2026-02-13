// Decompiled with JetBrains decompiler
// Type: PlotListComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
public class PlotListComponent : 
  ComponentListBase<PlotListElement, Plot, PlotData, PlotListComponent>
{
  protected override MetaType meta_type => MetaType.Plot;

  protected override void setupSortingTabs()
  {
    this.sorting_tab.tryAddButton("ui/Icons/iconAge", "sort_by_age", new SortButtonAction(((ComponentListBase<PlotListElement, Plot, PlotData, PlotListComponent>) this).show), (SortButtonAction) (() => this.current_sort = new Comparison<Plot>(PlotListComponent.sortByAge)));
    this.sorting_tab.tryAddButton("ui/Icons/iconPopulation", "sort_by_members", new SortButtonAction(((ComponentListBase<PlotListElement, Plot, PlotData, PlotListComponent>) this).show), (SortButtonAction) (() => this.current_sort = new Comparison<Plot>(PlotListComponent.sortBySupporters)));
  }

  public static int sortByAge(Plot pPlot1, Plot pPlot2)
  {
    return -pPlot2.data.created_time.CompareTo(pPlot1.data.created_time);
  }

  public static int sortBySupporters(Plot pPlot1, Plot pPlot2)
  {
    return pPlot2.units.Count.CompareTo(pPlot1.units.Count);
  }
}

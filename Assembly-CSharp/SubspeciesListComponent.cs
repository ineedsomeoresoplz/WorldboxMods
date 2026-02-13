// Decompiled with JetBrains decompiler
// Type: SubspeciesListComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
public class SubspeciesListComponent : 
  ComponentListSapient<SubspeciesListElement, Subspecies, SubspeciesData, SubspeciesListComponent>
{
  protected override MetaType meta_type => MetaType.Subspecies;

  protected override void setupSortingTabs()
  {
    this.genericMetaSortByAge(new Comparison<Subspecies>(((ComponentListBase<SubspeciesListElement, Subspecies, SubspeciesData, SubspeciesListComponent>) this).sortByAge));
    this.genericMetaSortByRenown(new Comparison<Subspecies>(((ComponentListBase<SubspeciesListElement, Subspecies, SubspeciesData, SubspeciesListComponent>) this).sortByRenown));
    this.genericMetaSortByPopulation(new Comparison<Subspecies>(ComponentListBase<SubspeciesListElement, Subspecies, SubspeciesData, SubspeciesListComponent>.sortByPopulation));
    this.genericMetaSortByKills(new Comparison<Subspecies>(ComponentListBase<SubspeciesListElement, Subspecies, SubspeciesData, SubspeciesListComponent>.sortByKills));
    this.genericMetaSortByDeath(new Comparison<Subspecies>(ComponentListBase<SubspeciesListElement, Subspecies, SubspeciesData, SubspeciesListComponent>.sortByDeaths));
    this.sorting_tab.tryAddButton("ui/Icons/iconChildren", "sort_by_children", new SortButtonAction(((ComponentListBase<SubspeciesListElement, Subspecies, SubspeciesData, SubspeciesListComponent>) this).show), (SortButtonAction) (() => this.current_sort = new Comparison<Subspecies>(SubspeciesListComponent.sortByChildren)));
    this.sorting_tab.tryAddButton("ui/Icons/iconHelixDNA", "sort_by_species", new SortButtonAction(((ComponentListBase<SubspeciesListElement, Subspecies, SubspeciesData, SubspeciesListComponent>) this).show), (SortButtonAction) (() => this.current_sort = new Comparison<Subspecies>(SubspeciesListComponent.sortBySpecies)));
    this.sorting_tab.tryAddButton("ui/Icons/iconFamily", "sort_by_families", new SortButtonAction(((ComponentListBase<SubspeciesListElement, Subspecies, SubspeciesData, SubspeciesListComponent>) this).show), (SortButtonAction) (() => this.current_sort = new Comparison<Subspecies>(SubspeciesListComponent.sortByFamilies)));
  }

  public static int sortByChildren(Subspecies pObject1, Subspecies pObject2)
  {
    return pObject2.countChildren().CompareTo(pObject1.countChildren());
  }

  public static int sortBySpecies(Subspecies pObject1, Subspecies pObject2)
  {
    return pObject2.getActorAsset().GetHashCode().CompareTo(pObject1.getActorAsset().GetHashCode());
  }

  public static int sortByDead(Subspecies pObject1, Subspecies pObject2)
  {
    return pObject2.data.total_deaths.CompareTo(pObject1.data.total_deaths);
  }

  public static int sortByFamilies(Subspecies pObject1, Subspecies pObject2)
  {
    return pObject2.countCurrentFamilies().CompareTo(pObject1.countCurrentFamilies());
  }
}

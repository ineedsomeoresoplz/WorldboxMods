// Decompiled with JetBrains decompiler
// Type: FamilyListComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class FamilyListComponent : 
  ComponentListSapient<FamilyListElement, Family, FamilyData, FamilyListComponent>
{
  private IMetaWithFamiliesWindow _families_window;

  protected override MetaType meta_type => MetaType.Family;

  protected override bool change_asset_sort_order => this._families_window == null;

  protected override void create()
  {
    base.create();
    this._families_window = ((Component) this).GetComponentInParent<IMetaWithFamiliesWindow>();
    if (this._families_window == null)
      return;
    this.get_objects_delegate = (GetListOfObjectsFunc<FamilyListElement, Family, FamilyData, FamilyListComponent>) (_ => this._families_window.getFamilies());
  }

  protected override void setupSortingTabs()
  {
    this.genericMetaSortByAge(new Comparison<Family>(((ComponentListBase<FamilyListElement, Family, FamilyData, FamilyListComponent>) this).sortByAge));
    this.genericMetaSortByRenown(new Comparison<Family>(((ComponentListBase<FamilyListElement, Family, FamilyData, FamilyListComponent>) this).sortByRenown));
    this.genericMetaSortByPopulation(new Comparison<Family>(ComponentListBase<FamilyListElement, Family, FamilyData, FamilyListComponent>.sortByPopulation));
    this.genericMetaSortByKills(new Comparison<Family>(ComponentListBase<FamilyListElement, Family, FamilyData, FamilyListComponent>.sortByKills));
    this.genericMetaSortByDeath(new Comparison<Family>(ComponentListBase<FamilyListElement, Family, FamilyData, FamilyListComponent>.sortByDeaths));
    this.sorting_tab.tryAddButton("ui/Icons/iconAdults", "sort_by_adults", new SortButtonAction(((ComponentListBase<FamilyListElement, Family, FamilyData, FamilyListComponent>) this).show), (SortButtonAction) (() => this.current_sort = new Comparison<Family>(FamilyListComponent.sortByAdults)));
    this.sorting_tab.tryAddButton("ui/Icons/iconChildren", "sort_by_children", new SortButtonAction(((ComponentListBase<FamilyListElement, Family, FamilyData, FamilyListComponent>) this).show), (SortButtonAction) (() => this.current_sort = new Comparison<Family>(FamilyListComponent.sortByChildren)));
    this.sorting_tab.tryAddButton("ui/Icons/iconHelixDNA", "sort_by_species", new SortButtonAction(((ComponentListBase<FamilyListElement, Family, FamilyData, FamilyListComponent>) this).show), (SortButtonAction) (() => this.current_sort = new Comparison<Family>(FamilyListComponent.sortBySpecies)));
  }

  public override bool isEmpty()
  {
    return this._families_window != null ? !this._families_window.hasFamilies() : base.isEmpty();
  }

  public static int sortByAdults(Family pObject1, Family pObject2)
  {
    return pObject2.countAdults().CompareTo(pObject1.countAdults());
  }

  public static int sortByChildren(Family pObject1, Family pObject2)
  {
    return pObject2.countChildren().CompareTo(pObject1.countChildren());
  }

  public static int sortBySpecies(Family pObject1, Family pObject2)
  {
    return pObject2.getActorAsset().GetHashCode().CompareTo(pObject1.getActorAsset().GetHashCode());
  }
}

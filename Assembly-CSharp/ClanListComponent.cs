// Decompiled with JetBrains decompiler
// Type: ClanListComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
public class ClanListComponent : 
  ComponentListBase<ClanListElement, Clan, ClanData, ClanListComponent>
{
  protected override MetaType meta_type => MetaType.Clan;

  protected override void setupSortingTabs()
  {
    this.genericMetaSortByAge(new Comparison<Clan>(((ComponentListBase<ClanListElement, Clan, ClanData, ClanListComponent>) this).sortByAge));
    this.genericMetaSortByRenown(new Comparison<Clan>(((ComponentListBase<ClanListElement, Clan, ClanData, ClanListComponent>) this).sortByRenown));
    this.genericMetaSortByPopulation(new Comparison<Clan>(ComponentListBase<ClanListElement, Clan, ClanData, ClanListComponent>.sortByPopulation));
    this.genericMetaSortByKills(new Comparison<Clan>(ComponentListBase<ClanListElement, Clan, ClanData, ClanListComponent>.sortByKills));
    this.genericMetaSortByDeath(new Comparison<Clan>(ComponentListBase<ClanListElement, Clan, ClanData, ClanListComponent>.sortByDeaths));
    this.sorting_tab.tryAddButton("ui/Icons/iconKingdom", "sort_by_kingdom", new SortButtonAction(((ComponentListBase<ClanListElement, Clan, ClanData, ClanListComponent>) this).show), (SortButtonAction) (() => this.current_sort = new Comparison<Clan>(ClanListComponent.sortByKingdom)));
  }

  private static int sortByKingdom(Clan p1, Clan p2)
  {
    Actor chief = p1.getChief();
    return p2.getChief().kingdom.CompareTo((NanoObject) chief.kingdom);
  }
}

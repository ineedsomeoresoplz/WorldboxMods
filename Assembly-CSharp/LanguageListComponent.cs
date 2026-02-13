// Decompiled with JetBrains decompiler
// Type: LanguageListComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
public class LanguageListComponent : 
  ComponentListBase<LanguageListElement, Language, LanguageData, LanguageListComponent>
{
  protected override MetaType meta_type => MetaType.Language;

  protected override void setupSortingTabs()
  {
    this.genericMetaSortByAge(new Comparison<Language>(((ComponentListBase<LanguageListElement, Language, LanguageData, LanguageListComponent>) this).sortByAge));
    this.genericMetaSortByRenown(new Comparison<Language>(((ComponentListBase<LanguageListElement, Language, LanguageData, LanguageListComponent>) this).sortByRenown));
    this.genericMetaSortByPopulation(new Comparison<Language>(ComponentListBase<LanguageListElement, Language, LanguageData, LanguageListComponent>.sortByPopulation));
    this.genericMetaSortByKills(new Comparison<Language>(ComponentListBase<LanguageListElement, Language, LanguageData, LanguageListComponent>.sortByKills));
    this.genericMetaSortByDeath(new Comparison<Language>(ComponentListBase<LanguageListElement, Language, LanguageData, LanguageListComponent>.sortByDeaths));
    this.sorting_tab.tryAddButton("ui/Icons/iconVillages", "sort_by_villages", new SortButtonAction(((ComponentListBase<LanguageListElement, Language, LanguageData, LanguageListComponent>) this).show), (SortButtonAction) (() => this.current_sort = new Comparison<Language>(LanguageListComponent.sortByVillages)));
  }

  public static int sortByVillages(Language pObject1, Language pObject2)
  {
    return pObject2.cities.Count.CompareTo(pObject1.cities.Count);
  }
}

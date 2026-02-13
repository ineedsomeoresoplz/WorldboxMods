// Decompiled with JetBrains decompiler
// Type: CultureWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class CultureWindow : 
  WindowMetaGeneric<Culture, CultureData>,
  ITraitWindow<CultureTrait, CultureTraitButton>,
  IAugmentationsWindow<ITraitsEditor<CultureTrait>>,
  IBooksWindow
{
  public StatBar experienceBar;

  public override MetaType meta_type => MetaType.Culture;

  protected override Culture meta_object => SelectedMetas.selected_culture;

  public void testDebugNewBook()
  {
    this.meta_object.testDebugNewBook();
    this.startShowingWindow();
    this.scroll_window.tabs.showTab(this.scroll_window.tabs.getActiveTab());
  }

  public List<long> getBooks() => this.meta_object.books.getList();

  protected override void showTopPartInformation()
  {
    base.showTopPartInformation();
    Culture metaObject = this.meta_object;
  }

  internal override void showStatsRows()
  {
    Culture metaObject = this.meta_object;
    this.tryShowPastNames();
    this.showStatRow("founded", (object) metaObject.getFoundedDate());
    this.tryToShowActor("founder", metaObject.data.creator_id, metaObject.data.creator_name, pIconPath: "actor_traits/iconStupid");
    this.tryToShowMetaClan("founder_clan", metaObject.data.creator_clan_id, metaObject.data.creator_clan_name);
    this.tryToShowMetaKingdom("origin", metaObject.data.creator_kingdom_id, metaObject.data.creator_kingdom_name);
    this.tryToShowMetaCity("birthplace", metaObject.data.creator_city_id, metaObject.data.creator_city_name);
    this.tryToShowMetaSubspecies("founder_subspecies", metaObject.data.creator_subspecies_id, metaObject.data.creator_subspecies_name);
    this.tryToShowMetaSpecies("founder_species", metaObject.data.creator_species_id);
  }

  protected override bool onNameChange(string pInput)
  {
    if (!base.onNameChange(pInput))
      return false;
    long id = this.meta_object.getID();
    string name = this.meta_object.data.name;
    foreach (Book book in (CoreSystemManager<Book, BookData>) World.world.books)
    {
      if (!book.isRekt() && book.data.culture_id == id)
        book.data.culture_name = name;
    }
    return true;
  }

  T IAugmentationsWindow<ITraitsEditor<CultureTrait>>.GetComponentInChildren<T>(bool includeInactive)
  {
    return ((Component) this).GetComponentInChildren<T>(includeInactive);
  }
}

// Decompiled with JetBrains decompiler
// Type: LanguageWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class LanguageWindow : 
  WindowMetaGeneric<Language, LanguageData>,
  ITraitWindow<LanguageTrait, LanguageTraitButton>,
  IAugmentationsWindow<ITraitsEditor<LanguageTrait>>,
  IBooksWindow
{
  public override MetaType meta_type => MetaType.Language;

  protected override Language meta_object => SelectedMetas.selected_language;

  protected override void showTopPartInformation()
  {
    base.showTopPartInformation();
    AchievementLibrary.multiply_spoken.checkBySignal((object) this.meta_object);
  }

  internal override void showStatsRows()
  {
    Language metaObject = this.meta_object;
    this.tryShowPastNames();
    this.showStatRow("founded", (object) metaObject.getFoundedDate(), MetaType.None, -1L, "iconAge", (string) null, (TooltipDataGetter) null);
    this.tryToShowActor("creator", metaObject.data.creator_id, metaObject.data.creator_name, pIconPath: "actor_traits/iconStupid");
    this.tryToShowMetaClan("creators_clan", metaObject.data.creator_clan_id, metaObject.data.creator_clan_name);
    this.tryToShowMetaKingdom("origin", metaObject.data.creator_kingdom_id, metaObject.data.creator_kingdom_name);
    this.tryToShowMetaCity("birthplace", metaObject.data.creator_city_id, metaObject.data.creator_city_name);
    this.tryToShowMetaSubspecies("creator_subspecies", metaObject.data.creator_subspecies_id, metaObject.data.creator_subspecies_name);
    this.tryToShowMetaSpecies("creator_species", metaObject.data.creator_species_id);
  }

  public List<long> getBooks() => this.meta_object.books.getList();

  protected override bool onNameChange(string pInput)
  {
    if (!base.onNameChange(pInput))
      return false;
    long id = this.meta_object.getID();
    string name = this.meta_object.data.name;
    foreach (Book book in (CoreSystemManager<Book, BookData>) World.world.books)
    {
      if (!book.isRekt() && book.data.language_id == id)
        book.data.language_name = name;
    }
    return true;
  }

  T IAugmentationsWindow<ITraitsEditor<LanguageTrait>>.GetComponentInChildren<T>(
    bool includeInactive)
  {
    return ((Component) this).GetComponentInChildren<T>(includeInactive);
  }
}

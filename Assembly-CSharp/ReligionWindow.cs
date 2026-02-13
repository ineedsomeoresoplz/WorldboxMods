// Decompiled with JetBrains decompiler
// Type: ReligionWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ReligionWindow : 
  WindowMetaGeneric<Religion, ReligionData>,
  ITraitWindow<ReligionTrait, ReligionTraitButton>,
  IAugmentationsWindow<ITraitsEditor<ReligionTrait>>,
  IBooksWindow
{
  public StatBar experienceBar;

  public override MetaType meta_type => MetaType.Religion;

  protected override Religion meta_object => SelectedMetas.selected_religion;

  public List<long> getBooks() => this.meta_object.books.getList();

  protected override void showTopPartInformation()
  {
    base.showTopPartInformation();
    Religion metaObject = this.meta_object;
    AchievementLibrary.not_just_a_cult.checkBySignal((object) this.meta_object);
  }

  internal override void showStatsRows()
  {
    Religion metaObject = this.meta_object;
    this.tryShowPastNames();
    this.showStatRow("founded", (object) metaObject.getFoundedDate(), MetaType.None, -1L, "iconAge", (string) null, (TooltipDataGetter) null);
    this.tryToShowActor("founder", metaObject.data.creator_id, metaObject.data.creator_name, pIconPath: "actor_traits/iconStupid");
    this.tryToShowMetaClan("founder_clan", metaObject.data.creator_clan_id, metaObject.data.creator_clan_name);
    this.tryToShowMetaKingdom("origin", metaObject.data.creator_kingdom_id, metaObject.data.creator_kingdom_name);
    this.tryToShowMetaCity("birthplace", metaObject.data.creator_city_id, metaObject.data.creator_city_name);
    this.tryToShowMetaSubspecies("founder_subspecies", metaObject.data.creator_subspecies_id, metaObject.data.creator_subspecies_name);
    this.tryToShowMetaSpecies("founder_species", metaObject.data.creator_species_id);
    this.showStatRow("deity", (object) "??", ColorStyleLibrary.m.color_dead_text, pIconPath: "iconDivineLight");
  }

  public void testDebugNewBook()
  {
    if (this.meta_object.units.Count == 0)
      return;
    Actor random = this.meta_object.units.GetRandom<Actor>();
    if (random.getCity() == null || !random.city.hasBookSlots())
      return;
    World.world.books.generateNewBook(random);
    this.startShowingWindow();
    this.scroll_window.tabs.showTab(this.scroll_window.tabs.getActiveTab());
  }

  protected override bool onNameChange(string pInput)
  {
    if (!base.onNameChange(pInput))
      return false;
    long id = this.meta_object.getID();
    string name = this.meta_object.data.name;
    foreach (Book book in (CoreSystemManager<Book, BookData>) World.world.books)
    {
      if (!book.isRekt() && book.data.religion_id == id)
        book.data.religion_name = name;
    }
    return true;
  }

  T IAugmentationsWindow<ITraitsEditor<ReligionTrait>>.GetComponentInChildren<T>(
    bool includeInactive)
  {
    return ((Component) this).GetComponentInChildren<T>(includeInactive);
  }
}

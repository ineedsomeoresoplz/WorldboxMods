// Decompiled with JetBrains decompiler
// Type: ClanWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class ClanWindow : 
  WindowMetaGeneric<Clan, ClanData>,
  ITraitWindow<ClanTrait, ClanTraitButton>,
  IAugmentationsWindow<ITraitsEditor<ClanTrait>>
{
  public NameInput nameInput;
  public NameInput mottoInput;

  public override MetaType meta_type => MetaType.Clan;

  protected override Clan meta_object => SelectedMetas.selected_clan;

  protected override void initNameInput()
  {
    base.initNameInput();
    // ISSUE: method pointer
    this.mottoInput.addListener(new UnityAction<string>((object) this, __methodptr(applyInputMotto)));
  }

  protected override bool onNameChange(string pInput)
  {
    if (!base.onNameChange(pInput))
      return false;
    long id = this.meta_object.getID();
    string name = this.meta_object.data.name;
    foreach (Culture culture in (CoreSystemManager<Culture, CultureData>) World.world.cultures)
    {
      if (!culture.isRekt() && culture.data.creator_clan_id == id)
        culture.data.creator_clan_name = name;
    }
    foreach (Religion religion in (CoreSystemManager<Religion, ReligionData>) World.world.religions)
    {
      if (!religion.isRekt() && religion.data.creator_clan_id == id)
        religion.data.creator_clan_name = name;
    }
    foreach (Language language in (CoreSystemManager<Language, LanguageData>) World.world.languages)
    {
      if (!language.isRekt() && language.data.creator_clan_id == id)
        language.data.creator_clan_name = name;
    }
    foreach (Book book in (CoreSystemManager<Book, BookData>) World.world.books)
    {
      if (!book.isRekt() && book.data.author_clan_id == id)
        book.data.author_clan_name = name;
    }
    return true;
  }

  private void applyInputMotto(string pInput)
  {
    if (pInput == null || this.meta_object == null)
      return;
    this.meta_object.data.motto = pInput;
  }

  protected override void showTopPartInformation()
  {
    base.showTopPartInformation();
    Clan metaObject = this.meta_object;
    if (metaObject == null)
      return;
    this.mottoInput.setText(metaObject.getMotto());
    ((Graphic) this.mottoInput.textField).color = metaObject.getColor().getColorText();
  }

  internal override void showStatsRows()
  {
    Clan metaObject = this.meta_object;
    this.tryShowPastNames();
    this.showStatRow("founded", (object) metaObject.getFoundedDate(), MetaType.None, -1L, "iconAge", (string) null, (TooltipDataGetter) null);
    this.tryToShowActor("clan_founder", metaObject.data.founder_actor_id, metaObject.data.founder_actor_name, pIconPath: "actor_traits/iconStupid");
    this.tryToShowActor("clan_chief_title", pObject: metaObject.getChief(), pIconPath: "iconClan");
    this.tryShowPastChiefs();
    this.tryToShowActor("clan_heir", pObject: metaObject.getNextChief(), pIconPath: "iconClanList");
    this.tryToShowMetaCulture(pObject: metaObject.getClanCulture());
    this.tryToShowMetaKingdom("origin", metaObject.data.founder_kingdom_id, metaObject.data.founder_kingdom_name);
    this.tryToShowMetaCity("birthplace", metaObject.data.founder_city_id, metaObject.data.founder_city_name);
    this.tryToShowMetaSubspecies("original_subspecies", metaObject.data.creator_subspecies_id, metaObject.data.creator_subspecies_name);
    this.tryToShowMetaSpecies("species", metaObject.data.creator_species_id);
  }

  private void tryShowPastChiefs()
  {
    List<LeaderEntry> pastChiefs = this.meta_object.data.past_chiefs;
    // ISSUE: explicit non-virtual call
    if ((pastChiefs != null ? (__nonvirtual (pastChiefs.Count) > 1 ? 1 : 0) : 0) == 0)
      return;
    this.showStatRow("past_chiefs", (object) this.meta_object.data.past_chiefs.Count, MetaType.None, -1L, "iconCaptain", "past_rulers", new TooltipDataGetter(this.getTooltipPastChiefs));
  }

  private TooltipData getTooltipPastChiefs()
  {
    return new TooltipData()
    {
      tip_name = "past_chiefs",
      meta_type = MetaType.Clan,
      past_rulers = new ListPool<LeaderEntry>((ICollection<LeaderEntry>) this.meta_object.data.past_chiefs)
    };
  }

  protected override void OnDisable()
  {
    base.OnDisable();
    this.mottoInput.inputField.DeactivateInputField();
  }

  public void debugClearExpLevel() => this.OnEnable();

  T IAugmentationsWindow<ITraitsEditor<ClanTrait>>.GetComponentInChildren<T>(bool includeInactive)
  {
    return ((Component) this).GetComponentInChildren<T>(includeInactive);
  }
}

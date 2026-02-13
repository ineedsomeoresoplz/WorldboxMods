// Decompiled with JetBrains decompiler
// Type: KingdomWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class KingdomWindow : 
  WindowMetaGeneric<Kingdom, KingdomData>,
  ITraitWindow<KingdomTrait, KingdomTraitButton>,
  IAugmentationsWindow<ITraitsEditor<KingdomTrait>>
{
  public Image raceTopIcon1;
  public Image raceTopIcon2;
  public NameInput mottoInput;

  public override MetaType meta_type => MetaType.Kingdom;

  protected override Kingdom meta_object => SelectedMetas.selected_kingdom;

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
    foreach (War war in (CoreSystemManager<War, WarData>) World.world.wars)
    {
      if (!war.isRekt() && war.data.started_by_kingdom_id == id)
        war.data.started_by_kingdom_name = name;
    }
    foreach (Alliance alliance in (CoreSystemManager<Alliance, AllianceData>) World.world.alliances)
    {
      if (!alliance.isRekt() && alliance.data.founder_kingdom_id == id)
        alliance.data.founder_kingdom_name = name;
    }
    foreach (Religion religion in (CoreSystemManager<Religion, ReligionData>) World.world.religions)
    {
      if (!religion.isRekt() && religion.data.creator_kingdom_id == id)
        religion.data.creator_kingdom_name = name;
    }
    foreach (Culture culture in (CoreSystemManager<Culture, CultureData>) World.world.cultures)
    {
      if (!culture.isRekt() && culture.data.creator_kingdom_id == id)
        culture.data.creator_kingdom_name = name;
    }
    foreach (Clan clan in (CoreSystemManager<Clan, ClanData>) World.world.clans)
    {
      if (!clan.isRekt() && clan.data.founder_kingdom_id == id)
        clan.data.founder_kingdom_name = name;
    }
    foreach (Language language in (CoreSystemManager<Language, LanguageData>) World.world.languages)
    {
      if (!language.isRekt() && language.data.creator_kingdom_id == id)
        language.data.creator_kingdom_name = name;
    }
    foreach (Family family in (CoreSystemManager<Family, FamilyData>) World.world.families)
    {
      if (!family.isRekt() && family.data.founder_kingdom_id == id)
        family.data.founder_kingdom_name = name;
    }
    foreach (Book book in (CoreSystemManager<Book, BookData>) World.world.books)
    {
      if (!book.isRekt() && book.data.author_kingdom_id == id)
        book.data.author_kingdom_name = name;
    }
    foreach (Item pObject in (CoreSystemManager<Item, ItemData>) World.world.items)
    {
      if (!pObject.isRekt() && pObject.data.creator_kingdom_id == id)
        pObject.data.from = name;
    }
    foreach (Army army in (CoreSystemManager<Army, ArmyData>) World.world.armies)
    {
      if (!army.isRekt() && army.getKingdom() == this.meta_object)
        army.onKingdomNameChange();
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
    Kingdom metaObject = this.meta_object;
    if (metaObject == null)
      return;
    this.raceTopIcon1.sprite = metaObject.getSpriteIcon();
    this.raceTopIcon2.sprite = metaObject.getSpriteIcon();
    this.mottoInput.setText(metaObject.getMotto());
    ((Graphic) this.mottoInput.textField).color = metaObject.getColor().getColorText();
  }

  private void tryShowPastRulers()
  {
    List<LeaderEntry> pastRulers = this.meta_object.data.past_rulers;
    // ISSUE: explicit non-virtual call
    if ((pastRulers != null ? (__nonvirtual (pastRulers.Count) > 1 ? 1 : 0) : 0) == 0)
      return;
    this.showStatRow("past_kings", (object) this.meta_object.data.past_rulers.Count, MetaType.None, -1L, "iconKingdomList", "past_rulers", new TooltipDataGetter(this.getTooltipPastRulers));
  }

  private TooltipData getTooltipPastRulers()
  {
    return new TooltipData()
    {
      tip_name = "past_kings",
      meta_type = MetaType.Kingdom,
      past_rulers = new ListPool<LeaderEntry>((ICollection<LeaderEntry>) this.meta_object.data.past_rulers)
    };
  }

  internal override void showStatsRows()
  {
    Kingdom metaObject = this.meta_object;
    this.tryShowPastNames();
    this.showStatRow("founded", (object) metaObject.getFoundedDate(), MetaType.None, -1L, "iconAge", (string) null, (TooltipDataGetter) null);
    this.tryShowPastRulers();
    this.tryToShowActor("king", pObject: metaObject.king, pIconPath: "iconKings");
    this.tryToShowActor("heir", pObject: SuccessionTool.findNextHeir(metaObject, metaObject.king), pIconPath: "iconChildren");
    if (metaObject.hasKing())
    {
      if (metaObject.king.s_personality != null)
        this.showStatRow("creature_statistics_personality", (object) metaObject.king.s_personality.getTranslatedName(), MetaType.None, -1L, "actor_traits/iconStupid", (string) null, (TooltipDataGetter) null);
      this.showStatRow("kingdom_statistics_king_ruled", (object) Date.getYearsSince(metaObject.data.timestamp_king_rule), MetaType.None, -1L, "iconClock", (string) null, (TooltipDataGetter) null);
      this.showStatRow("ruler_money", (object) metaObject.king.money, "#43FF43", pIconPath: "iconMoney");
    }
    this.showStatRow("tribute", (object) metaObject.getTaxRateTribute().ToString("0%"), "#43FF43", pIconPath: "kingdom_traits/kingdom_trait_tax_rate_tribute_high");
    this.tryToShowMetaSpecies("founder_species", metaObject.getFounderSpecies().id);
  }

  public override void showMetaRows()
  {
    Kingdom metaObject = this.meta_object;
    this.meta_rows_container.tryToShowMetaAlliance(pObject: metaObject.getAlliance());
    this.meta_rows_container.tryToShowMetaCity("kingdom_statistics_capital", pObject: metaObject.capital, pIconPath: "iconKingdom");
    this.meta_rows_container.tryToShowMetaClan(pObject: metaObject.king?.clan);
    this.meta_rows_container.tryToShowMetaCulture(pObject: metaObject.culture);
    this.meta_rows_container.tryToShowMetaLanguage(pObject: metaObject.language);
    this.meta_rows_container.tryToShowMetaReligion(pObject: metaObject.religion);
    this.meta_rows_container.tryToShowMetaSubspecies(pObject: metaObject.getMainSubspecies());
  }

  protected override void OnDisable()
  {
    base.OnDisable();
    this.mottoInput.inputField.DeactivateInputField();
  }

  public void clickCapital()
  {
    SelectedMetas.selected_city = this.meta_object.capital;
    ScrollWindow.showWindow("city");
  }

  T IAugmentationsWindow<ITraitsEditor<KingdomTrait>>.GetComponentInChildren<T>(bool includeInactive)
  {
    return ((Component) this).GetComponentInChildren<T>(includeInactive);
  }
}

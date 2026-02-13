// Decompiled with JetBrains decompiler
// Type: CityWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine.UI;

#nullable disable
public class CityWindow : WindowMetaGeneric<City, CityData>, IBooksWindow
{
  public Image raceTopIcon1;
  public Image raceTopIcon2;
  public LocalizedText village_title;

  public override MetaType meta_type => MetaType.City;

  protected override City meta_object => SelectedMetas.selected_city;

  public List<long> getBooks() => this.meta_object.getBooks();

  protected override void showTopPartInformation()
  {
    base.showTopPartInformation();
    City metaObject = this.meta_object;
    if (metaObject == null)
      return;
    this.raceTopIcon1.sprite = metaObject.getSpriteIcon();
    this.raceTopIcon2.sprite = metaObject.getSpriteIcon();
  }

  public override void startShowingWindow()
  {
    base.startShowingWindow();
    AchievementLibrary.checkCityAchievements(this.meta_object);
  }

  private void tryShowPastRulers()
  {
    List<LeaderEntry> pastRulers = this.meta_object.data.past_rulers;
    // ISSUE: explicit non-virtual call
    if ((pastRulers != null ? (__nonvirtual (pastRulers.Count) > 1 ? 1 : 0) : 0) == 0)
      return;
    this.showStatRow("past_leaders", (object) this.meta_object.data.past_rulers.Count, MetaType.None, -1L, "iconVillages", "past_rulers", new TooltipDataGetter(this.getTooltipPastRulers));
  }

  private TooltipData getTooltipPastRulers()
  {
    return new TooltipData()
    {
      tip_name = "past_leaders",
      meta_type = MetaType.City,
      past_rulers = new ListPool<LeaderEntry>((ICollection<LeaderEntry>) this.meta_object.data.past_rulers)
    };
  }

  protected override bool onNameChange(string pInput)
  {
    if (!base.onNameChange(pInput))
      return false;
    foreach (Religion religion in (CoreSystemManager<Religion, ReligionData>) World.world.religions)
    {
      if (!religion.isRekt() && religion.data.creator_city_id == this.meta_object.getID())
        religion.data.creator_city_name = this.meta_object.data.name;
    }
    foreach (Culture culture in (CoreSystemManager<Culture, CultureData>) World.world.cultures)
    {
      if (!culture.isRekt() && culture.data.creator_city_id == this.meta_object.getID())
        culture.data.creator_city_name = this.meta_object.data.name;
    }
    foreach (Clan clan in (CoreSystemManager<Clan, ClanData>) World.world.clans)
    {
      if (!clan.isRekt() && clan.data.founder_city_id == this.meta_object.getID())
        clan.data.founder_city_name = this.meta_object.data.name;
    }
    foreach (Language language in (CoreSystemManager<Language, LanguageData>) World.world.languages)
    {
      if (!language.isRekt() && language.data.creator_city_id == this.meta_object.getID())
        language.data.creator_city_name = this.meta_object.data.name;
    }
    foreach (Family family in (CoreSystemManager<Family, FamilyData>) World.world.families)
    {
      if (!family.isRekt() && family.data.founder_city_id == this.meta_object.getID())
        family.data.founder_city_name = this.meta_object.data.name;
    }
    foreach (Book book in (CoreSystemManager<Book, BookData>) World.world.books)
    {
      if (!book.isRekt() && book.data.author_city_id == this.meta_object.getID())
        book.data.author_city_name = this.meta_object.data.name;
    }
    return true;
  }

  internal override void showStatsRows()
  {
    City metaObject = this.meta_object;
    if (metaObject == null)
      return;
    if (metaObject.kingdom.isNeutral())
      this.village_title.setKeyAndUpdate("village_dying");
    else
      this.village_title.setKeyAndUpdate("village");
    this.tryShowPastNames();
    this.showStatRow("founded", (object) metaObject.getFoundedDate(), MetaType.None, -1L, "iconAge", (string) null, (TooltipDataGetter) null);
    this.tryToShowActor("founder", metaObject.data.founder_id, metaObject.data.founder_name, pIconPath: "actor_traits/iconStupid");
    this.tryShowPastRulers();
    this.tryToShowActor("village_statistics_leader", pObject: metaObject.leader, pIconPath: "iconLeaders");
    if (metaObject.hasLeader())
      this.showStatRow("ruler_money", (object) metaObject.leader.money, "#43FF43", pIconPath: "iconMoney");
    this.showStatRow("tax", (object) metaObject.kingdom.getTaxRateLocal().ToString("0%"), "#43FF43", pIconPath: "kingdom_traits/kingdom_trait_tax_rate_local_low");
    this.showStatRow("tribute", (object) metaObject.kingdom.getTaxRateTribute().ToString("0%"), "#43FF43", pIconPath: "kingdom_traits/kingdom_trait_tax_rate_tribute_high");
    this.tryToShowActor("king", pObject: metaObject.kingdom.king, pIconPath: "iconKings");
    this.tryToShowMetaSpecies("founder_species", metaObject.getFounderSpecies()?.id);
  }

  public override void showMetaRows()
  {
    City metaObject = this.meta_object;
    if (metaObject == null || metaObject.kingdom.isNeutral())
      return;
    this.meta_rows_container.tryToShowMetaClan(pObject: metaObject.leader?.clan);
    this.meta_rows_container.tryToShowMetaKingdom(pObject: metaObject.kingdom);
    this.meta_rows_container.tryToShowMetaAlliance(pObject: metaObject.kingdom.getAlliance());
    this.meta_rows_container.tryToShowMetaCulture(pObject: metaObject.culture);
    this.meta_rows_container.tryToShowMetaLanguage(pObject: metaObject.language);
    this.meta_rows_container.tryToShowMetaReligion(pObject: metaObject.religion);
    this.meta_rows_container.tryToShowMetaSubspecies(pObject: metaObject.getMainSubspecies());
    this.meta_rows_container.tryToShowMetaArmy(pObject: metaObject.army);
  }

  public void clickTestItemProduction()
  {
    ItemCrafting.tryToCraftRandomWeapon(this.meta_object.units.GetRandom<Actor>(), this.meta_object);
    this.scroll_window.tabs.showTab(this.scroll_window.tabs.getActiveTab());
  }

  public void clickTestClearItems()
  {
    this.meta_object.data.equipment.clearItems();
    this.scroll_window.tabs.showTab(this.scroll_window.tabs.getActiveTab());
  }

  public void clickTestNewBook()
  {
    if (!this.meta_object.hasLeader() || !this.meta_object.leader.hasCulture() || !this.meta_object.leader.hasLanguage())
      return;
    World.world.books.generateNewBook(this.meta_object.leader);
    this.meta_object.forceDoChecks();
    this.scroll_window.tabs.showTab(this.scroll_window.tabs.getActiveTab());
  }
}

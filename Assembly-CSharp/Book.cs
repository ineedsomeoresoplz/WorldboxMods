// Decompiled with JetBrains decompiler
// Type: Book
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class Book : CoreSystemObject<BookData>
{
  private BaseStats _base_stats_read_action = new BaseStats();

  public override BaseSystemManager manager => (BaseSystemManager) World.world.books;

  public void newBook(
    Actor pByActor,
    BookTypeAsset pBookType,
    ActorTrait pTraitActor,
    CultureTrait pTraitCulture,
    LanguageTrait pTraitLanguage,
    ReligionTrait pTraitReligion)
  {
    this.setName(NameGenerator.generateNameFromTemplate(pBookType.name_template, pByActor));
    this.data.book_type = pBookType.id;
    this.data.path_cover = World.world.books.getNewCoverPath();
    this.data.path_icon = pBookType.getNewIconPath();
    this.data.author_name = pByActor.getName();
    this.data.author_id = pByActor.getID();
    this.data.author_clan_name = pByActor.clan?.name;
    BookData data1 = this.data;
    Clan clan = pByActor.clan;
    long num1 = clan != null ? clan.id : -1L;
    data1.author_clan_id = num1;
    this.data.author_kingdom_name = pByActor.kingdom?.name;
    BookData data2 = this.data;
    Kingdom kingdom = pByActor.kingdom;
    long num2 = kingdom != null ? kingdom.id : -1L;
    data2.author_kingdom_id = num2;
    this.data.author_city_name = pByActor.city?.name;
    BookData data3 = this.data;
    City city = pByActor.city;
    long num3 = city != null ? city.id : -1L;
    data3.author_city_id = num3;
    this.data.language_id = pByActor.language.id;
    this.data.language_name = pByActor.language.name;
    this.data.trait_id_actor = pTraitActor?.id;
    this.data.trait_id_language = pTraitLanguage?.id;
    this.data.trait_id_culture = pTraitCulture?.id;
    this.data.trait_id_religion = pTraitReligion?.id;
    pByActor.language.books.setDirty();
    if (pBookType.save_culture)
    {
      this.data.culture_id = pByActor.culture.id;
      this.data.culture_name = pByActor.culture.name;
      pByActor.culture.books.setDirty();
    }
    if (pBookType.save_religion)
    {
      BookData data4 = this.data;
      Religion religion = pByActor.religion;
      long num4 = religion != null ? religion.id : -1L;
      data4.religion_id = num4;
      this.data.religion_name = pByActor.religion?.name;
      pByActor.religion?.books.setDirty();
    }
    ++pByActor.language.data.books_written;
    if (pByActor.hasClan())
      ++pByActor.clan.data.books_written;
    this.recalcBaseStats();
  }

  public BaseStats getBaseStats() => this._base_stats_read_action;

  private void recalcBaseStats()
  {
    this._base_stats_read_action.clear();
    this._base_stats_read_action.mergeStats(this.getAsset().base_stats);
  }

  public bool isReadyToBeRead()
  {
    return (double) World.world.getWorldTimeElapsedSince(this.data.timestamp_read_last_time) > 10.0;
  }

  public override void loadData(BookData pData)
  {
    base.loadData(pData);
    this.recalcBaseStats();
  }

  public Religion getReligion() => World.world.religions.get(this.data.religion_id);

  public Language getLanguage() => World.world.languages.get(this.data.language_id);

  public Culture getCulture() => World.world.cultures.get(this.data.culture_id);

  public ActorTrait getBookTraitActor()
  {
    return string.IsNullOrEmpty(this.data.trait_id_actor) ? (ActorTrait) null : AssetManager.traits.get(this.data.trait_id_actor);
  }

  public LanguageTrait getBookTraitLanguage()
  {
    return string.IsNullOrEmpty(this.data.trait_id_language) ? (LanguageTrait) null : AssetManager.language_traits.get(this.data.trait_id_language);
  }

  public CultureTrait getBookTraitCulture()
  {
    return string.IsNullOrEmpty(this.data.trait_id_culture) ? (CultureTrait) null : AssetManager.culture_traits.get(this.data.trait_id_culture);
  }

  public ReligionTrait getBookTraitReligion()
  {
    return string.IsNullOrEmpty(this.data.trait_id_religion) ? (ReligionTrait) null : AssetManager.religion_traits.get(this.data.trait_id_religion);
  }

  public int getHappiness()
  {
    int happiness = (int) this._base_stats_read_action["happiness"];
    if (this.getLanguage().hasTrait("beautiful_calligraphy"))
      happiness = (int) ((double) happiness * (double) LanguageTraitLibrary.getValueFloat("beautiful_calligraphy"));
    return happiness;
  }

  public int getExperience()
  {
    int experience = (int) this._base_stats_read_action["experience"];
    Language language = this.getLanguage();
    if (language.hasTrait("scribble"))
    {
      if (experience > 1)
        experience = 1;
    }
    else if (language.hasTrait("nicely_structured_grammar"))
      experience = (int) ((double) experience * (double) LanguageTraitLibrary.getValueFloat("nicely_structured_grammar"));
    return experience;
  }

  public int getMana() => (int) this._base_stats_read_action["mana"];

  protected sealed override void setDefaultValues() => base.setDefaultValues();

  public BookTypeAsset getAsset() => AssetManager.book_types.get(this.data.book_type);

  public void readIt() => this.data.timestamp_read_last_time = World.world.getCurWorldTime();

  public void increaseReadTimes()
  {
    ++this.data.times_read;
    ++World.world.game_stats.data.booksRead;
    ++World.world.map_stats.booksRead;
    this.readIt();
  }

  public override void Dispose()
  {
    base.Dispose();
    this._base_stats_read_action.clear();
  }
}

// Decompiled with JetBrains decompiler
// Type: BehFinishReading
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai.behaviours;
using System.Collections.Generic;

#nullable disable
public class BehFinishReading : BehCitizenActionCity
{
  protected override void setupErrorChecks()
  {
    base.setupErrorChecks();
    this.uses_books = true;
    this.uses_religions = true;
    this.uses_languages = true;
    this.uses_cultures = true;
  }

  public override BehResult execute(Actor pActor)
  {
    Book behBookTarget = pActor.beh_book_target;
    if (behBookTarget == null || !behBookTarget.isAlive())
      return BehResult.Stop;
    this.checkBookTrait(pActor, behBookTarget);
    this.checkBookValueBonuses(pActor, behBookTarget);
    this.checkBookAttributes(pActor, behBookTarget);
    this.checkSpecialBookRewards(pActor, behBookTarget);
    this.tryToConvertActorToMetaFromBook(pActor, behBookTarget);
    this.checkBookAssetAction(pActor, behBookTarget);
    this.tryToGetMetaTraitsFromBook(pActor, behBookTarget);
    behBookTarget.increaseReadTimes();
    return BehResult.Continue;
  }

  private void checkBookAttributes(Actor pActor, Book pBook)
  {
    foreach (BaseStatsContainer baseStatsContainer in pBook.getBaseStats().getList())
    {
      if (baseStatsContainer.asset.actor_data_attribute)
        pActor.data[baseStatsContainer.id] += baseStatsContainer.value;
    }
  }

  private void checkBookAssetAction(Actor pActor, Book pBook)
  {
    BookTypeAsset asset = pBook.getAsset();
    BookReadAction readAction = asset.read_action;
    if (readAction == null)
      return;
    readAction(pActor, asset);
  }

  private void checkSpecialBookRewards(Actor pActor, Book pBook)
  {
    foreach (LanguageTrait trait in (IEnumerable<LanguageTrait>) pBook.getLanguage().getTraits())
    {
      BookTraitAction readBookTraitAction = trait.read_book_trait_action;
      if (readBookTraitAction != null)
        readBookTraitAction(pActor, trait, pBook);
    }
  }

  private void checkBookValueBonuses(Actor pActor, Book pBook)
  {
    int happiness = pBook.getHappiness();
    int experience = pBook.getExperience();
    int mana = pBook.getMana();
    if (pActor.hasCulture())
    {
      if (pActor.culture.hasTrait("reading_lovers") && happiness < 0)
        happiness *= -1;
      if (pActor.culture.hasTrait("attentive_readers"))
        experience *= (int) ((double) experience * (double) CultureTraitLibrary.getValueFloat("attentive_readers"));
    }
    pActor.changeHappiness("just_read_book", happiness);
    pActor.addExperience(experience);
    pActor.addMana(mana);
  }

  private void checkBookTrait(Actor pActor, Book pBook)
  {
    if (!Randy.randomBool())
      return;
    ActorTrait bookTraitActor = pBook.getBookTraitActor();
    if (bookTraitActor == null)
      return;
    pActor.addTrait(bookTraitActor);
  }

  private void tryToConvertActorToMetaFromBook(Actor pActor, Book pBook)
  {
    this.tryToConvertActorToBookCulture(pActor, pBook);
    this.tryToConvertActorToBookLanguage(pActor, pBook);
    this.tryToConvertActorToBookReligion(pActor, pBook);
  }

  private void tryToGetMetaTraitsFromBook(Actor pActor, Book pBook)
  {
    if (!pActor.isKing() && !pActor.isCityLeader())
      return;
    this.tryToGetMetaTraitFromBookCulture(pActor, pBook);
    this.tryToGetMetaTraitFromBookLanguage(pActor, pBook);
    this.tryToGetMetaTraitFromBookReligion(pActor, pBook);
  }

  private void tryToGetMetaTraitFromBookCulture(Actor pActor, Book pBook)
  {
    if (!pActor.hasCulture())
      return;
    CultureTrait bookTraitCulture = pBook.getBookTraitCulture();
    if (bookTraitCulture == null || !Randy.randomBool())
      return;
    pActor.culture.addTrait(bookTraitCulture, false);
  }

  private void tryToGetMetaTraitFromBookLanguage(Actor pActor, Book pBook)
  {
    if (!pActor.hasLanguage())
      return;
    LanguageTrait bookTraitLanguage = pBook.getBookTraitLanguage();
    if (bookTraitLanguage == null || !Randy.randomBool())
      return;
    pActor.language.addTrait(bookTraitLanguage, false);
  }

  private void tryToGetMetaTraitFromBookReligion(Actor pActor, Book pBook)
  {
    if (!pActor.hasReligion())
      return;
    ReligionTrait bookTraitReligion = pBook.getBookTraitReligion();
    if (bookTraitReligion == null || !Randy.randomBool())
      return;
    pActor.religion.addTrait(bookTraitReligion, false);
  }

  private void tryToConvertActorToBookReligion(Actor pActor, Book pBook)
  {
    Religion religion = pBook.getReligion();
    if (religion == null || pActor.religion == religion)
      return;
    using (ListPool<Religion> listPool = new ListPool<Religion>(6))
    {
      if (pActor.hasReligion())
      {
        listPool.AddTimes<Religion>(3, pActor.religion);
        if (this.hasStylishWritingActor(pActor))
          listPool.AddTimes<Religion>(this.getStylishWritingValue(), pActor.religion);
      }
      listPool.AddTimes<Religion>(3, religion);
      if (this.hasStylishWritingBook(pBook))
        listPool.AddTimes<Religion>(this.getStylishWritingValue(), religion);
      Religion random = listPool.GetRandom<Religion>();
      if (random == pActor.religion)
        return;
      pActor.tryToConvertToReligion(random);
    }
  }

  private void tryToConvertActorToBookLanguage(Actor pActor, Book pBook)
  {
    Language language = pBook.getLanguage();
    if (language == null || pActor.language == language)
      return;
    using (ListPool<Language> listPool = new ListPool<Language>())
    {
      if (pActor.hasLanguage())
      {
        listPool.AddTimes<Language>(3, pActor.language);
        if (this.hasStylishWritingActor(pActor))
          listPool.AddTimes<Language>(this.getStylishWritingValue(), pActor.language);
      }
      listPool.AddTimes<Language>(3, language);
      if (this.hasStylishWritingBook(pBook))
        listPool.AddTimes<Language>(this.getStylishWritingValue(), language);
      Language random = listPool.GetRandom<Language>();
      if (random == pActor.language)
        return;
      pActor.tryToConvertToLanguage(random);
    }
  }

  private void tryToConvertActorToBookCulture(Actor pActor, Book pBook)
  {
    Culture culture1 = pBook.getCulture();
    if (culture1 == null)
      return;
    Culture culture2 = pActor.culture;
    if (culture2 == culture1)
      return;
    using (ListPool<Culture> listPool = new ListPool<Culture>())
    {
      if (pActor.hasCulture())
      {
        listPool.AddTimes<Culture>(3, culture2);
        if (this.hasStylishWritingActor(pActor))
          listPool.AddTimes<Culture>(this.getStylishWritingValue(), culture2);
      }
      listPool.AddTimes<Culture>(3, culture1);
      if (this.hasStylishWritingBook(pBook))
        listPool.AddTimes<Culture>(this.getStylishWritingValue(), culture1);
      Culture random = listPool.GetRandom<Culture>();
      if (random == culture2)
        return;
      pActor.tryToConvertToCulture(random);
    }
  }

  private bool hasStylishWritingActor(Actor pActor)
  {
    return pActor.hasLanguage() && pActor.language.hasTrait("stylish_writing");
  }

  private bool hasStylishWritingBook(Book pBook) => pBook.getLanguage().hasTrait("stylish_writing");

  private int getStylishWritingValue() => LanguageTraitLibrary.getValue("stylish_writing");
}

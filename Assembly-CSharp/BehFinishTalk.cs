// Decompiled with JetBrains decompiler
// Type: BehFinishTalk
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai;
using ai.behaviours;

#nullable disable
public class BehFinishTalk : BehaviourActionActor
{
  public BehFinishTalk() => this.socialize = true;

  public override BehResult execute(Actor pActor)
  {
    Actor a = pActor.beh_actor_target?.a;
    if (a == null || !this.stillCanTalk(a))
      return BehResult.Stop;
    this.finishTalk(pActor, a);
    return BehResult.Continue;
  }

  private bool stillCanTalk(Actor pTarget) => pTarget.isAlive() && !pTarget.isLying();

  private void finishTalk(Actor pActor, Actor pTarget)
  {
    pActor.resetSocialize();
    pTarget.resetSocialize();
    int num1 = Randy.randomChance(0.7f) ? 1 : 0;
    int pValue = num1 == 0 ? -15 : 10;
    pActor.changeHappiness("just_talked", pValue);
    pTarget.changeHappiness("just_talked", pValue);
    pActor.addStatusEffect("recovery_social");
    pTarget.addStatusEffect("recovery_social");
    if (num1 != 0)
      ActorTool.checkFallInLove(pActor, pTarget);
    if (num1 != 0)
      ActorTool.checkBecomingBestFriends(pActor, pTarget);
    this.checkMetaSpread(pActor, pTarget);
    if (pActor.hasCulture() && pActor.culture.hasTrait("youth_reverence") && this.throwDiceForGift(pActor, pTarget) && pActor.isAdult() && pTarget.getAge() < pActor.getAge())
      this.makeGift(pActor, pTarget);
    if (pActor.hasCulture() && pActor.culture.hasTrait("elder_reverence") && this.throwDiceForGift(pActor, pTarget) && pActor.isAdult() && pTarget.getAge() > pActor.getAge())
      this.makeGift(pActor, pTarget);
    this.checkPassLearningAttributes(pActor, pTarget);
    float num2 = Randy.randomFloat(1.1f, 3.3f);
    pActor.timer_action = num2;
    pTarget.timer_action = num2;
  }

  private void checkAttribue(Actor pActor, Actor pTarget, string pAttributeID)
  {
    if (!Randy.randomChance(0.3f))
      return;
    if ((double) pActor.stats[pAttributeID] > (double) pTarget.stats[pAttributeID])
    {
      pTarget.stats[pAttributeID]++;
    }
    else
    {
      if ((double) pActor.stats[pAttributeID] >= (double) pTarget.stats[pAttributeID])
        return;
      pActor.stats[pAttributeID]++;
    }
  }

  private void checkPassLearningAttributes(Actor pActor, Actor pTarget)
  {
    this.checkAttribue(pActor, pTarget, "intelligence");
    this.checkAttribue(pActor, pTarget, "warfare");
    this.checkAttribue(pActor, pTarget, "diplomacy");
    this.checkAttribue(pActor, pTarget, "stewardship");
  }

  private void checkMetaSpread(Actor pActor, Actor pTarget)
  {
    if (!pActor.hasSubspecies() || !pTarget.hasSubspecies())
      return;
    this.tryToSpreadCulture(pActor, pTarget);
    this.tryToSpreadLanguage(pActor, pTarget);
    this.tryToSpreadReligion(pActor, pTarget);
  }

  private void tryToSpreadCulture(Actor pActor, Actor pTarget)
  {
    if (!pActor.subspecies.has_advanced_memory || !pTarget.subspecies.has_advanced_memory)
      return;
    Culture pCulture = this.decideCulture(pActor, pTarget);
    if (pCulture == null)
      return;
    pActor.tryToConvertToCulture(pCulture);
    pTarget.tryToConvertToCulture(pCulture);
    if (pCulture.hasTrait("pep_talks") && Randy.randomChance(0.5f))
    {
      pActor.addStatusEffect("inspired");
      pTarget.addStatusEffect("inspired");
    }
    if (pCulture.hasTrait("expertise_exchange"))
    {
      pActor.addExperience(CultureTraitLibrary.getValue("expertise_exchange"));
      pTarget.addExperience(CultureTraitLibrary.getValue("expertise_exchange"));
    }
    if (!pCulture.hasTrait("gossip_lovers"))
      return;
    pActor.changeHappiness("just_talked_gossip");
    pTarget.changeHappiness("just_talked_gossip");
  }

  private void tryToSpreadLanguage(Actor pActor, Actor pTarget)
  {
    if (!pActor.subspecies.has_advanced_communication || !pTarget.subspecies.has_advanced_communication)
      return;
    Language pLanguage = this.decideLanguage(pActor, pTarget);
    if (pLanguage == null)
      return;
    pActor.tryToConvertToLanguage(pLanguage);
    pTarget.tryToConvertToLanguage(pLanguage);
  }

  private void tryToSpreadReligion(Actor pActor, Actor pTarget)
  {
    if (!pActor.subspecies.has_advanced_memory || !pTarget.subspecies.has_advanced_memory)
      return;
    Religion pReligion = this.decideReligion(pActor, pTarget);
    if (pReligion == null)
      return;
    pActor.tryToConvertToReligion(pReligion);
    pTarget.tryToConvertToReligion(pReligion);
  }

  private Religion decideReligion(Actor pActor1, Actor pActor2)
  {
    Religion religion1 = pActor1.religion;
    Religion religion2 = pActor2.religion;
    if (religion1 == null && religion2 == null)
      return (Religion) null;
    if (religion1 == null)
      return religion2;
    if (religion2 == null)
      return religion1;
    using (ListPool<Religion> list = new ListPool<Religion>())
    {
      list.Add(religion1);
      list.Add(religion2);
      if (pActor1.hasCity() && pActor1.religion == pActor1.city.getReligion())
        list.Add(pActor1.religion);
      if (pActor1.kingdom.hasReligion() && pActor1.religion == pActor1.kingdom.getReligion())
        list.Add(pActor1.religion);
      if (pActor2.hasCity() && pActor2.religion == pActor2.city.getReligion())
        list.Add(pActor2.religion);
      if (pActor2.kingdom.hasReligion() && pActor2.religion == pActor2.kingdom.getReligion())
        list.Add(pActor2.religion);
      return list.GetRandom<Religion>();
    }
  }

  private Language decideLanguage(Actor pActor1, Actor pActor2)
  {
    Language language1 = pActor1.language;
    Language language2 = pActor2.language;
    if (language1 == null && language2 == null)
      return (Language) null;
    if (language1 == null)
      return language2;
    if (language2 == null)
      return language1;
    using (ListPool<Language> listPool = new ListPool<Language>())
    {
      int pAmount1 = 3;
      int pAmount2 = 3;
      if (pActor1.hasLanguage() && pActor1.language.hasTrait("melodic"))
        pAmount1 += LanguageTraitLibrary.getValue("melodic");
      if (pActor2.hasLanguage() && pActor2.language.hasTrait("melodic"))
        pAmount2 += LanguageTraitLibrary.getValue("melodic");
      if (pActor1.hasCity() && pActor1.language == pActor1.city.getLanguage())
        ++pAmount1;
      if (pActor1.kingdom.hasLanguage() && pActor1.language == pActor1.kingdom.getLanguage())
        ++pAmount1;
      if (pActor2.hasCity() && pActor2.language == pActor2.city.getLanguage())
        ++pAmount2;
      if (pActor2.kingdom.hasLanguage() && pActor2.language == pActor2.kingdom.getLanguage())
        ++pAmount2;
      listPool.AddTimes<Language>(pAmount1, language1);
      listPool.AddTimes<Language>(pAmount2, language2);
      return listPool.GetRandom<Language>();
    }
  }

  private Culture decideCulture(Actor pActor1, Actor pActor2)
  {
    Culture culture1 = pActor1.culture;
    Culture culture2 = pActor2.culture;
    if (culture1 == null && culture2 == null)
      return (Culture) null;
    if (culture1 == null)
      return culture2;
    if (culture2 == null)
      return culture1;
    using (ListPool<Culture> listPool = new ListPool<Culture>())
    {
      int pAmount1 = 3;
      int pAmount2 = 3;
      if (pActor1.hasLanguage() && pActor1.language.hasTrait("melodic"))
        pAmount1 += LanguageTraitLibrary.getValue("melodic");
      if (pActor2.hasLanguage() && pActor2.language.hasTrait("melodic"))
        pAmount2 += LanguageTraitLibrary.getValue("melodic");
      if (pActor1.hasCity() && pActor1.culture == pActor1.city.getCulture())
        ++pAmount1;
      if (pActor1.kingdom.hasCulture() && pActor1.culture == pActor1.kingdom.getCulture())
        ++pAmount1;
      if (pActor2.hasCity() && pActor2.culture == pActor2.city.getCulture())
        ++pAmount2;
      if (pActor2.kingdom.hasCulture() && pActor2.culture == pActor2.kingdom.getCulture())
        ++pAmount2;
      listPool.AddTimes<Culture>(pAmount1, culture1);
      listPool.AddTimes<Culture>(pAmount2, culture2);
      return listPool.GetRandom<Culture>();
    }
  }

  private bool throwDiceForGift(Actor pActor, Actor pTarget)
  {
    int num = pActor.isRelatedTo(pTarget) ? 1 : (pActor.isImportantTo(pTarget) ? 1 : 0);
    float pVal = 0.2f;
    if (num != 0)
      pVal += 0.3f;
    return Randy.randomChance(pVal);
  }

  private void makeGift(Actor pActor, Actor pTarget)
  {
    bool acceptGift = pTarget.tryToAcceptGift(pActor);
    int moneyForGift = pActor.getMoneyForGift();
    if (moneyForGift > 0)
      pTarget.addMoney(moneyForGift);
    if (!(moneyForGift > 0 | acceptGift))
      return;
    pActor.changeHappiness("just_gave_gift");
    pTarget.changeHappiness("just_received_gift");
  }
}

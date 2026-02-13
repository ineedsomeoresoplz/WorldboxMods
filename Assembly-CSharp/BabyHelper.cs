// Decompiled with JetBrains decompiler
// Type: BabyHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class BabyHelper
{
  public static Actor debugTryToMakeUnit(Actor pActor)
  {
    WorldTile currentTile = pActor.current_tile;
    Actor pParent2 = (Actor) null;
    foreach (Actor actor in Finder.getUnitsFromChunk(currentTile, 1, 10f))
    {
      if (actor != pActor && actor.subspecies == pActor.subspecies)
      {
        pParent2 = actor;
        break;
      }
    }
    return pParent2 == null ? (Actor) null : BabyMaker.makeBaby(pActor, pParent2);
  }

  public static void countBirth(Actor pBaby)
  {
    ++World.world.game_stats.data.creaturesBorn;
    ++World.world.map_stats.creaturesBorn;
    if (pBaby.hasCity())
      pBaby.city.increaseBirths();
    if (pBaby.hasClan())
      pBaby.clan.increaseBirths();
    if (pBaby.hasFamily())
      pBaby.family.increaseBirths();
    if (pBaby.hasSubspecies())
      pBaby.subspecies.increaseBirths();
    if (!pBaby.isKingdomCiv())
      return;
    pBaby.kingdom.increaseBirths();
  }

  public static void applyParentsMeta(Actor pParent1, Actor pParent2, Actor pBaby)
  {
    Subspecies babySubspecies = BabyHelper.getBabySubspecies(pParent1, pParent2);
    pBaby.setSubspecies(babySubspecies);
    Family family = pParent1.family;
    Clan pObject = BabyHelper.checkGreatClan(pParent1, pParent2);
    if (pObject != null && !pObject.isFull())
      pBaby.setClan(pObject);
    if (babySubspecies.isSapient())
    {
      if (pParent1.hasCity())
        pBaby.setCity(pParent1.city);
      else if (pParent2 != null && pParent2.hasCity())
        pBaby.setCity(pParent2.city);
    }
    if (family != null)
    {
      pBaby.setFamily(family);
      pBaby.saveOriginFamily(family.data.id);
    }
    using (ListPool<Culture> list1 = new ListPool<Culture>(2))
    {
      using (ListPool<Religion> list2 = new ListPool<Religion>(2))
      {
        using (ListPool<Language> list3 = new ListPool<Language>(2))
        {
          using (ListPool<int> list4 = new ListPool<int>(2))
          {
            list4.Add(pParent1.data.phenotype_index);
            if (pParent1.hasCulture())
              list1.Add(pParent1.culture);
            if (pParent1.hasReligion())
              list2.Add(pParent1.religion);
            if (pParent1.hasLanguage())
              list3.Add(pParent1.language);
            if (pParent2 != null)
            {
              if (pParent2.hasCulture())
                list1.Add(pParent2.culture);
              if (pParent2.hasReligion())
                list2.Add(pParent2.religion);
              if (pParent2.hasLanguage())
                list3.Add(pParent2.language);
              if (pParent2.subspecies == pBaby.subspecies)
                list4.Add(pParent2.data.phenotype_index);
            }
            if (list1.Count > 0 && babySubspecies.has_advanced_memory)
              pBaby.setCulture(list1.GetRandom<Culture>());
            if (list2.Count > 0 && babySubspecies.has_advanced_memory)
              pBaby.setReligion(list2.GetRandom<Religion>());
            if (list3.Count > 0 && babySubspecies.has_advanced_communication)
              pBaby.joinLanguage(list3.GetRandom<Language>());
            if (pParent1 != null && pParent1.hasCultureTrait("ancestors_knowledge"))
            {
              string bestAtribute = BabyHelper.getBestAtribute(pParent1);
              if (bestAtribute != null)
                pBaby.data[bestAtribute] = (float) ((double) (int) pParent1.data[bestAtribute] * 0.5 + 1.0);
            }
            if (pParent2 != null && pParent2.hasCultureTrait("ancestors_knowledge"))
            {
              string bestAtribute = BabyHelper.getBestAtribute(pParent2);
              if (bestAtribute != null)
                pBaby.data[bestAtribute] = (float) ((double) (int) pParent2.data[bestAtribute] * 0.5 + 1.0);
            }
            pBaby.data.phenotype_index = list4.GetRandom<int>();
            pBaby.data.phenotype_shade = Actor.getRandomPhenotypeShade();
            if (!babySubspecies.hasTrait("parental_care"))
              return;
            pBaby.addStatusEffect("invincible", 90f);
          }
        }
      }
    }
  }

  private static string getBestAtribute(Actor pParent1)
  {
    string bestAtribute = (string) null;
    int num1 = 0;
    if ((double) pParent1.data["intelligence"] > (double) num1)
    {
      num1 = (int) pParent1.data["intelligence"];
      bestAtribute = "intelligence";
    }
    if ((double) pParent1.data["warfare"] > (double) num1)
    {
      num1 = (int) pParent1.data["warfare"];
      bestAtribute = "warfare";
    }
    if ((double) pParent1.data["diplomacy"] > (double) num1)
    {
      num1 = (int) pParent1.data["diplomacy"];
      bestAtribute = "diplomacy";
    }
    if ((double) pParent1.data["stewardship"] > (double) num1)
    {
      int num2 = (int) pParent1.data["stewardship"];
      bestAtribute = "stewardship";
    }
    return bestAtribute;
  }

  private static Clan checkGreatClan(Actor pParent1, Actor pParent2)
  {
    Clan clan = (Clan) null;
    if (pParent1.isKing())
      clan = pParent1.clan;
    else if (pParent2 != null && pParent2.isKing())
      clan = pParent2.clan;
    if (clan == null)
    {
      if (pParent1.isCityLeader() && pParent2 != null && pParent2.isCityLeader())
        clan = !Randy.randomBool() ? pParent2.clan : pParent1.clan;
      else if (pParent1 != null && pParent1.isCityLeader())
        clan = pParent1.clan;
      else if (pParent2 != null && pParent2.isCityLeader())
        clan = pParent2.clan;
    }
    return clan;
  }

  private static Subspecies getBabySubspecies(Actor pParent1, Actor pParent2)
  {
    Subspecies subspecies1 = pParent1.subspecies;
    Subspecies subspecies2 = pParent2?.subspecies ?? subspecies1;
    return subspecies1.isSapient() && subspecies1.isSapient() != subspecies2.isSapient() ? (subspecies1.isSapient() ? subspecies1 : subspecies2) : (subspecies1 != subspecies2 && subspecies1.getGeneration() != subspecies2.getGeneration() ? (subspecies1.getGeneration() > subspecies2.getGeneration() ? subspecies1 : subspecies2) : (Randy.randomBool() ? subspecies1 : subspecies2));
  }

  public static bool canMakeBabies(Actor pActor)
  {
    return pActor.isAdult() && pActor.canProduceBabies() && !pActor.hasReachedOffspringLimit() && pActor.haveNutritionForNewBaby();
  }

  public static bool isMetaLimitsReached(Actor pActor)
  {
    if (pActor.subspecies.hasReachedPopulationLimit())
      return true;
    if (!pActor.hasCity())
      return false;
    if (pActor.city.hasReachedWorldLawLimit())
      return true;
    Actor lover = pActor.lover;
    return ((!pActor.isImportantPerson() ? 0 : (!pActor.hasReachedOffspringLimit() ? 1 : 0)) | (lover == null || !lover.isImportantPerson() ? (false ? 1 : 0) : (!lover.hasReachedOffspringLimit() ? 1 : 0))) == 0 && (!pActor.subspecies.isReproductionSexual() || pActor.current_children_count != 0) && !pActor.city.hasFreeHouseSlots();
  }

  public static void countMakeChild(Actor pParent1, Actor pParent2)
  {
    if (!pParent1.isRekt())
      pParent1.increaseBirths();
    if (pParent2.isRekt())
      return;
    pParent2.increaseBirths();
  }

  public static void babyMakingStart(Actor pActor)
  {
    WorldAction actionsActorBirth = pActor.subspecies.all_actions_actor_birth;
    if (actionsActorBirth == null)
      return;
    int num = actionsActorBirth((BaseSimObject) pActor, pActor.current_tile) ? 1 : 0;
  }

  public static void traitsClone(Actor pActorTarget, Actor pParent1)
  {
    foreach (ActorTrait trait in (IEnumerable<ActorTrait>) pParent1.getTraits())
    {
      if (trait.rate_birth != 0 || trait.rate_inherit != 0)
        pActorTarget.addTrait(trait);
    }
  }

  public static void traitsInherit(Actor pActorTarget, Actor pParent1, Actor pParent2)
  {
    using (ListPool<ActorTrait> listPool = new ListPool<ActorTrait>(128 /*0x80*/))
    {
      int pCounter1 = 0;
      int pCounter2 = 0;
      BabyHelper.addTraitsFromParentToList(pParent1, listPool, out pCounter1);
      if (pParent2 != null)
        BabyHelper.addTraitsFromParentToList(pParent2, listPool, out pCounter2);
      if (listPool.Count == 0)
        return;
      int num = Mathf.Max(1, (int) ((double) (pCounter1 + pCounter2) * 0.25));
      for (int index = 0; index < num; ++index)
      {
        ActorTrait random = listPool.GetRandom<ActorTrait>();
        pActorTarget.addTrait(random.id);
      }
    }
  }

  private static void addTraitsFromParentToList(
    Actor pActor,
    ListPool<ActorTrait> pList,
    out int pCounter)
  {
    int num = 0;
    foreach (ActorTrait trait in (IEnumerable<ActorTrait>) pActor.getTraits())
    {
      if (trait.rate_inherit != 0 || trait.rate_birth != 0)
      {
        ++num;
        pList.AddTimes<ActorTrait>(trait.rate_birth, trait);
        pList.AddTimes<ActorTrait>(trait.rate_inherit, trait);
      }
    }
    pCounter = num;
  }
}

// Decompiled with JetBrains decompiler
// Type: BabyMaker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BabyMaker
{
  public static void startMiracleBirth(Actor pActor)
  {
    BabyHelper.babyMakingStart(pActor);
    if (pActor.hasSubspeciesTrait("reproduction_strategy_viviparity") && pActor.isSexFemale())
    {
      pActor.addStatusEffect("pregnant", pActor.getMaturationTimeSeconds());
    }
    else
    {
      pActor.birthEvent("miracle_bearer");
      BabyMaker.makeBabyFromMiracle(pActor, ActorSex.Male, true);
      BabyMaker.makeBabyFromMiracle(pActor, ActorSex.Female, true);
      if (Randy.randomBool())
        BabyMaker.makeBabyFromMiracle(pActor, pAddToFamily: true);
    }
    pActor.subspecies.counterReproduction();
  }

  public static void startSoulborneBirth(Actor pActor)
  {
    BabyHelper.babyMakingStart(pActor);
    if (pActor.subspecies.hasTrait("reproduction_strategy_viviparity") && pActor.isSexFemale())
    {
      pActor.addStatusEffect("pregnant", pActor.getMaturationTimeSeconds());
    }
    else
    {
      pActor.birthEvent();
      BabyMaker.makeBaby(pActor, (Actor) null, pJoinFamily: true);
    }
    pActor.subspecies.counterReproduction();
  }

  public static void spawnSporesFor(Actor pActor)
  {
    pActor.birthEvent();
    BabyHelper.babyMakingStart(pActor);
    int num = Randy.randomInt(3, 10);
    for (int index = 0; index < num; ++index)
    {
      Spores spores = (Spores) EffectsLibrary.spawn("fx_spores", pActor.current_tile);
      if (Object.op_Equality((Object) spores, (Object) null))
        return;
      spores.prepare();
      spores.setActorParent(pActor);
    }
    pActor.subspecies.counterReproduction();
  }

  public static void spawnBabyFromSpore(Actor pActor, Vector3 pPosition)
  {
    WorldTile tile = World.world.GetTile((int) pPosition.x, (int) pPosition.y);
    if (tile == null)
      return;
    BabyMaker.makeBaby(pActor, (Actor) null, pTile: tile, pJoinFamily: true);
  }

  public static void makeBabyFromMiracle(Actor pActor, ActorSex pSex = ActorSex.None, bool pAddToFamily = false)
  {
    BabyMaker.makeBaby(pActor, (Actor) null, pSex, pAddToFamily: pAddToFamily).addTrait("miracle_born");
  }

  public static Actor makeBabyViaFission(Actor pActor)
  {
    pActor.birthEvent();
    BabyHelper.babyMakingStart(pActor);
    Actor actor = BabyMaker.makeBaby(pActor, (Actor) null, pJoinFamily: true);
    int pValue1 = pActor.getHealth() / 2;
    int pValue2 = pActor.getHappiness() / 2;
    int pVal = pActor.getNutrition() / 2;
    pActor.setHealth(pValue1);
    pActor.setStamina(0);
    pActor.setHappiness(pValue2);
    pActor.setNutrition(pVal);
    actor.setHealth(pValue1);
    actor.setStamina(0);
    actor.setHappiness(pValue2);
    actor.setNutrition(pVal);
    pActor.subspecies.counterReproduction();
    return actor;
  }

  public static Actor makeBabyViaBudding(Actor pActor)
  {
    pActor.birthEvent();
    BabyHelper.babyMakingStart(pActor);
    return BabyMaker.makeBaby(pActor, (Actor) null, pJoinFamily: true);
  }

  public static Actor makeBabyViaVegetative(Actor pActor)
  {
    pActor.birthEvent();
    BabyHelper.babyMakingStart(pActor);
    Actor actor = BabyMaker.makeBaby(pActor, (Actor) null, pJoinFamily: true);
    actor.addStatusEffect("uprooting", actor.getMaturationTimeSeconds());
    return actor;
  }

  public static void makeBabyViaParthenogenesis(Actor pActor)
  {
    pActor.birthEvent();
    BabyHelper.babyMakingStart(pActor);
    BabyMaker.makeBaby(pActor, (Actor) null, pJoinFamily: true);
    pActor.subspecies.counterReproduction();
  }

  public static void makeBabiesViaSexual(Actor pMotherTarget, Actor pParentA, Actor pParentB)
  {
    pParentA.birthEvent();
    pParentB.birthEvent();
    BabyHelper.babyMakingStart(pParentA);
    BabyHelper.babyMakingStart(pParentB);
    BabyMaker.newImmediateBabySpawn(pParentA, pParentB);
    int stat = (int) pMotherTarget.stats["birth_rate"];
    float pVal = 0.5f;
    for (int index = 0; index < stat && Randy.randomChance(pVal); ++index)
    {
      BabyMaker.newImmediateBabySpawn(pParentA, pParentB);
      pVal *= 0.85f;
    }
  }

  public static void makeBabyFromPregnancy(Actor pActor)
  {
    pActor.hasLover();
    Actor lover = pActor.lover;
    pActor.birthEvent();
    BabyMaker.makeBaby(pActor, lover, pAddToFamily: true);
    float pVal = 0.5f;
    int stat = (int) pActor.stats["birth_rate"];
    for (int index = 0; index < stat && Randy.randomChance(pVal); ++index)
    {
      BabyMaker.makeBaby(pActor, lover, pAddToFamily: true);
      pVal *= 0.85f;
    }
  }

  private static void newImmediateBabySpawn(Actor pParent1, Actor pParent2)
  {
    BabyMaker.makeBaby(pParent1, pParent2, pAddToFamily: true).justBorn();
  }

  public static Actor makeBaby(
    Actor pParent1,
    Actor pParent2,
    ActorSex pForcedSexType = ActorSex.None,
    bool pCloneTraits = false,
    int pMutationRate = 0,
    WorldTile pTile = null,
    bool pAddToFamily = false,
    bool pJoinFamily = false)
  {
    City pCity = pParent1.city ?? pParent2?.city;
    if (pCity != null)
      --pCity.status.housing_free;
    ActorAsset asset = pParent1.asset;
    ActorData pData = new ActorData();
    pData.created_time = World.world.getCurWorldTime();
    pData.id = World.world.map_stats.getNextId("unit");
    pData.asset_id = asset.id;
    int generation = pParent1.data.generation;
    if (pParent2 != null && pParent2.data.generation > generation)
      generation = pParent2.data.generation;
    pData.generation = generation + 1;
    using (ListPool<WorldTile> list = new ListPool<WorldTile>(4))
    {
      foreach (WorldTile worldTile in pParent1.current_tile.neighboursAll)
      {
        if (worldTile != pParent1.current_tile && (pParent2 == null || worldTile != pParent2.current_tile) && worldTile.Type.ground)
          list.Add(worldTile);
      }
      WorldTile pTile1 = pTile == null ? (list.Count != 0 ? list.GetRandom<WorldTile>() : pParent1.current_tile) : pTile;
      Actor babyActorFromData = World.world.units.createBabyActorFromData(pData, pTile1, pCity);
      babyActorFromData.setParent1(pParent1);
      if (pParent2 != null)
        babyActorFromData.setParent2(pParent2);
      if (pAddToFamily && !pParent1.hasFamily())
        World.world.families.newFamily(pParent1, pParent1.current_tile, pParent2);
      else if (pJoinFamily)
      {
        Family pObject = pParent1.hasFamily() ? pParent1.family : World.world.families.newFamily(pParent1, pParent1.current_tile, pParent2);
        if (pObject != null)
          babyActorFromData.setFamily(pObject);
      }
      BabyHelper.applyParentsMeta(pParent1, pParent2, babyActorFromData);
      if (pCloneTraits || pParent1.hasSubspeciesTrait("genetic_mirror"))
      {
        BabyHelper.traitsClone(babyActorFromData, pParent1);
      }
      else
      {
        foreach (ActorTrait trait in (IEnumerable<ActorTrait>) babyActorFromData.subspecies.getActorBirthTraits().getTraits())
          babyActorFromData.addTrait(trait);
        BabyHelper.traitsInherit(babyActorFromData, pParent1, pParent2);
      }
      babyActorFromData.checkTraitMutationOnBirth();
      babyActorFromData.setNutrition(SimGlobals.m.nutrition_start_level_baby);
      if (pForcedSexType != ActorSex.None)
      {
        babyActorFromData.data.sex = pForcedSexType;
      }
      else
      {
        ActorSex actorSex = ActorSex.None;
        if (Randy.randomBool())
          actorSex = !pParent1.hasCity() ? (pParent1.subspecies.cached_females <= pParent1.subspecies.cached_males ? ActorSex.Female : ActorSex.Male) : (pParent1.city.status.females <= pParent1.city.status.males ? ActorSex.Female : ActorSex.Male);
        if (actorSex != ActorSex.None)
          babyActorFromData.data.sex = actorSex;
        else
          babyActorFromData.generateSex();
      }
      babyActorFromData.checkShouldBeEgg();
      babyActorFromData.makeStunned(10f);
      babyActorFromData.applyRandomForce();
      BabyHelper.countBirth(babyActorFromData);
      BabyHelper.countMakeChild(pParent1, pParent2);
      babyActorFromData.setStatsDirty();
      babyActorFromData.event_full_stats = true;
      return babyActorFromData;
    }
  }
}

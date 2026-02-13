// Decompiled with JetBrains decompiler
// Type: ActionLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using ai;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class ActionLibrary
{
  public static bool unluckyMeteorite(BaseSimObject pTarget, WorldTile pTile = null)
  {
    if (!WorldLawLibrary.world_law_disasters_nature.isEnabled() || World.world.cities.Count < 5 || pTarget.a.getAge() < 30 || !Randy.randomChance(5E-05f))
      return false;
    Meteorite.spawnMeteoriteDisaster(pTarget.current_tile, pTarget.a);
    return true;
  }

  public static bool unluckyFall(BaseSimObject pTarget, WorldTile pTile = null)
  {
    if (Randy.randomChance(0.8f))
      return false;
    pTarget.a.makeStunned();
    return true;
  }

  public static bool flamingWeapon(BaseSimObject pTarget, WorldTile pTile = null)
  {
    if (!MapBox.isRenderGameplay() || pTarget.isBuilding())
      return false;
    Actor a = pTarget.a;
    if (!a.a.is_visible)
      return false;
    Sprite renderedItemSprite = a.getRenderedItemSprite();
    if (Object.op_Equality((Object) renderedItemSprite, (Object) null))
      return false;
    AnimationFrameData animationFrameData = a.getAnimationFrameData();
    if (animationFrameData == null)
      return false;
    Vector3 point = new Vector3();
    point.x = a.cur_transform_position.x + animationFrameData.pos_item.x * a.current_scale.x;
    point.y = a.cur_transform_position.y + animationFrameData.pos_item.y * a.current_scale.y;
    point.z = -0.01f;
    Rect rect = renderedItemSprite.rect;
    float num1 = ((Rect) ref rect).height * a.current_scale.y;
    if (a.is_moving)
    {
      point.y += num1;
      point.x += Randy.randomFloat(-0.1f, 0.1f);
      point.y += Randy.randomFloat(-0.1f, 0.2f);
    }
    else
    {
      point.x += Randy.randomFloat(-0.05f, 0.05f);
      float num2 = Randy.randomFloat(0.0f, num1 * 1.5f);
      if ((double) num2 < (double) num1 * 0.5)
        point.x += Randy.randomFloat(-0.15f, 0.15f);
      point.y += num2;
    }
    if ((double) a.current_rotation.y != 0.0 || (double) a.current_rotation.z != 0.0)
      point = Toolbox.RotatePointAroundPivot(ref point, ref a.cur_transform_position, ref a.current_rotation);
    BaseEffect baseEffect = EffectsLibrary.spawn("fx_weapon_particle");
    if (!Object.op_Inequality((Object) baseEffect, (Object) null))
      return false;
    ((StatusParticle) baseEffect).spawnParticle(point, Toolbox.colors_fire.GetRandom<Color>());
    return true;
  }

  public static bool shiny(BaseSimObject pTarget, WorldTile pTile = null)
  {
    if (!pTile.has_tile_up || !MapBox.isRenderGameplay())
      return false;
    Vector3 posV3 = pTile.tile_up.posV3;
    posV3.x += Randy.randomFloat(-0.3f, 0.3f);
    posV3.y += Randy.randomFloat(-0.3f, 0.3f);
    EffectsLibrary.spawnAt("fx_building_sparkle", posV3, 0.1f);
    return true;
  }

  public static bool restoreHealthOnHit(
    BaseSimObject pSelf,
    BaseSimObject pTarget,
    WorldTile pTile = null)
  {
    if (pTarget == null || !pTarget.isActor() || !pSelf.isActor() || !pSelf.isAlive())
      return false;
    int maxHealthPercent = pTarget.getMaxHealthPercent(0.05f);
    pSelf.a.restoreHealth(maxHealthPercent);
    return true;
  }

  public static void throwTorchAtTile(BaseSimObject pSelf, WorldTile pTile)
  {
    Vector2Int pos = pTile.pos;
    Vector3 vector3 = Vector2.op_Implicit(pSelf.current_position);
    float pDist = Vector2.Distance(Vector2.op_Implicit(vector3), Vector2Int.op_Implicit(pos));
    Vector3 newPoint1 = Toolbox.getNewPoint(vector3.x, vector3.y, (float) ((Vector2Int) ref pos).x, (float) ((Vector2Int) ref pos).y, pDist);
    Vector3 newPoint2 = Toolbox.getNewPoint(vector3.x, vector3.y, (float) ((Vector2Int) ref pos).x, (float) ((Vector2Int) ref pos).y, pSelf.a.stats["size"]);
    newPoint2.y += 0.5f;
    World.world.projectiles.spawn(pSelf, (BaseSimObject) null, "torch", newPoint2, newPoint1);
  }

  public static bool canThrowBomb(BaseSimObject pTarget, WorldTile pTile)
  {
    double x1 = (double) pTarget.a.current_position.x;
    double y1 = (double) pTarget.a.current_position.y;
    Vector2Int pos = pTile.pos;
    double x2 = (double) ((Vector2Int) ref pos).x;
    pos = pTile.pos;
    double y2 = (double) ((Vector2Int) ref pos).y;
    float num = Toolbox.Dist((float) x1, (float) y1, (float) x2, (float) y2);
    return (double) num > 3.0 && (double) num < 26.0;
  }

  public static void throwBombAtTile(BaseSimObject pSelf, WorldTile pTile)
  {
    Vector2Int pos = pTile.pos;
    Vector3 vector3 = Vector2.op_Implicit(pSelf.current_position);
    float pDist = Vector2.Distance(Vector2.op_Implicit(vector3), Vector2Int.op_Implicit(pos));
    Vector3 newPoint1 = Toolbox.getNewPoint(vector3.x, vector3.y, (float) ((Vector2Int) ref pos).x, (float) ((Vector2Int) ref pos).y, pDist);
    Vector3 newPoint2 = Toolbox.getNewPoint(vector3.x, vector3.y, (float) ((Vector2Int) ref pos).x, (float) ((Vector2Int) ref pos).y, pSelf.a.stats["size"]);
    newPoint2.y += 0.5f;
    World.world.projectiles.spawn(pSelf, (BaseSimObject) null, "firebomb", newPoint2, newPoint1);
  }

  public static bool zombieInfectAttack(
    BaseSimObject pSelf,
    BaseSimObject pTarget,
    WorldTile pTile = null)
  {
    if (!pTarget.isAlive() || !pTarget.isActor())
      return false;
    if (Randy.randomChance(0.25f))
      pTarget.a.startShake(0.2f, 0.05f, pVertical: false);
    pTarget.a.spawnParticle(Toolbox.color_infected);
    if (pTarget.a.asset.can_turn_into_zombie && Randy.randomChance(0.5f))
      pTarget.a.addTrait("infected");
    return true;
  }

  public static bool zombieEffect(BaseSimObject pTarget, WorldTile pTile = null)
  {
    pTarget.a.spawnParticle(Toolbox.color_infected);
    if (Randy.randomChance(0.25f))
      pTarget.a.startShake(0.2f, 0.05f, pVertical: false);
    return true;
  }

  public static bool infectedEffect(BaseSimObject pTarget, WorldTile pTile = null)
  {
    int pDamage = pTarget.getHealth() / 10;
    if (pDamage < 10)
      pDamage = 10;
    pTarget.a.getHit((float) pDamage, pAttackType: AttackType.Infection, pSkipIfShake: false);
    pTarget.a.spawnParticle(Toolbox.color_infected);
    pTarget.a.startShake(0.4f, 0.2f, pVertical: false);
    return true;
  }

  public static bool mushSporesEffect(BaseSimObject pTarget, WorldTile pTile = null)
  {
    int num = 3;
    foreach (Actor actor in Finder.getUnitsFromChunk(pTile, 1, 3f, true))
    {
      if (actor != pTarget.a && !Randy.randomChance(0.7f) && actor.addTrait("mush_spores"))
      {
        actor.spawnParticle(Toolbox.color_mushSpores);
        --num;
        if (num == 0)
          break;
      }
    }
    return true;
  }

  public static bool tumorEffect(BaseSimObject pTarget, WorldTile pTile = null)
  {
    pTarget.a.startShake(0.4f, 0.2f, pVertical: false);
    if (Randy.randomChance(0.1f))
      pTarget.getHit((float) pTarget.getMaxHealthPercent(0.1f), false, AttackType.Tumor, pSkipIfShake: false);
    return true;
  }

  public static bool healingAuraEffect(BaseSimObject pSelf, WorldTile pTile = null)
  {
    if (!Randy.randomChance(0.2f))
      return false;
    foreach (Actor pTarget in Finder.getUnitsFromChunk(pTile, 1, 4f, true))
    {
      if (pTarget != pSelf.a && !pTarget.hasMaxHealth() && !pSelf.areFoes((BaseSimObject) pTarget))
      {
        pTarget.restoreHealth(10);
        pTarget.spawnParticle(Toolbox.color_heal);
        pTarget.removeTrait("plague");
      }
    }
    return true;
  }

  public static bool heliophobiaEffect(BaseSimObject pTarget, WorldTile pTile = null)
  {
    Actor a = pTarget.a;
    BiomeAsset biome = a.current_tile.getBiome();
    if (biome != null && (biome.cold_biome || biome.dark_biome) || !World.world_era.flag_light_damage)
      return false;
    int pDamage = (int) ((double) a.getMaxHealth() * 0.10000000149011612) + 1;
    a.getHit((float) pDamage);
    return true;
  }

  public static bool regenerationEffect(BaseSimObject pTarget, WorldTile pTile = null)
  {
    Actor a = pTarget.a;
    if (a.hasTrait("infected"))
      return true;
    if (!a.hasMaxHealth() && !a.isHungry() && Randy.randomChance(0.2f))
    {
      int maxHealthPercent = a.getMaxHealthPercent(0.02f);
      a.restoreHealth(maxHealthPercent);
      a.spawnParticle(Toolbox.color_heal);
    }
    ActionLibrary.checkRegenerationTraits(a);
    return true;
  }

  private static void checkRegenerationTraits(Actor pActorTarget)
  {
    if (pActorTarget.hasTrait("crippled") && Randy.randomChance(0.05f))
      pActorTarget.removeTrait("crippled");
    if (pActorTarget.hasTrait("skin_burns") && Randy.randomChance(0.05f))
      pActorTarget.removeTrait("skin_burns");
    if (!pActorTarget.hasTrait("eyepatch") || !Randy.randomChance(0.05f))
      return;
    pActorTarget.removeTrait("eyepatch");
  }

  public static bool regenerationEffectClan(BaseSimObject pTarget, WorldTile pTile = null)
  {
    Actor a = pTarget.a;
    if (a.hasTrait("infected"))
      return true;
    if (!a.hasMaxHealth() && !a.isHungry() && Randy.randomChance(0.2f))
    {
      int maxHealthPercent = a.getMaxHealthPercent(0.01f);
      a.restoreHealth(maxHealthPercent);
      a.spawnParticle(Toolbox.color_heal);
    }
    ActionLibrary.checkRegenerationTraits(a);
    return true;
  }

  public static bool suprisedByArchitector(BaseSimObject _, WorldTile pTile)
  {
    if (World.world.isPaused())
      return false;
    foreach (Actor actor in Finder.getUnitsFromChunk(pTile, 1, 8f))
      actor.tryToGetSurprised(pTile);
    return true;
  }

  public static bool coldAuraEffect(BaseSimObject pTarget, WorldTile pTile = null)
  {
    World.world.loopWithBrush(pTarget.current_tile, Brush.get(4), new PowerActionWithID(PowerLibrary.drawTemperatureMinus));
    return true;
  }

  public static bool megaHeartbeat(BaseSimObject pTarget, WorldTile pTile = null)
  {
    World.world.applyForceOnTile(pTile, 3, 0.3f, pByWho: pTarget);
    EffectsLibrary.spawnExplosionWave(pTile.posV3, 3f, 0.5f);
    return true;
  }

  public static bool thornsDefense(BaseSimObject pSelf, BaseSimObject pAttackedBy, WorldTile pTile = null)
  {
    if (!pSelf.isAlive() || !Randy.randomChance(0.5f))
      return false;
    if (pAttackedBy != null && pAttackedBy.isActor() && pAttackedBy.isAlive())
    {
      Actor a = pAttackedBy.a;
      if ((double) Toolbox.DistTile(pSelf.a.current_tile, a.a.current_tile) < 2.0)
      {
        float pDamage = a.stats["damage"] * 0.2f;
        a.getHit(pDamage, pAttackType: AttackType.Weapon, pAttacker: pSelf);
      }
    }
    return true;
  }

  public static bool bubbleDefense(BaseSimObject pSelf, BaseSimObject pAttackedBy, WorldTile pTile = null)
  {
    if (!pSelf.hasHealth() || !Randy.randomChance(0.1f))
      return false;
    pSelf.addStatusEffect("shield", 5f);
    return true;
  }

  public static bool plagueEffect(BaseSimObject pTarget, WorldTile pTile = null)
  {
    ActionLibrary.tickPlagueInfection(pTarget.a);
    pTarget.a.startShake(0.4f, 0.2f, pVertical: false);
    if (Randy.randomChance(0.1f))
    {
      int pDamage = pTarget.getMaxHealthPercent(0.15f) + 1;
      pTarget.a.getHit((float) pDamage, false, AttackType.Plague, (BaseSimObject) null, false, false, true);
    }
    return true;
  }

  public static bool energizedLightning(BaseSimObject pTarget, WorldTile pTile = null)
  {
    if (!Toolbox.inMapBorder(ref pTarget.current_position))
    {
      EffectsLibrary.spawnAt("fx_lightning_small", pTarget.current_position, 0.25f);
      return true;
    }
    MapBox.spawnLightningSmall(pTarget.current_tile, pActor: pTarget.a);
    return true;
  }

  public static bool contagiousEffect(BaseSimObject pTarget, WorldTile pTile = null)
  {
    if (!WorldLawLibrary.world_law_rat_plague.isEnabled())
      return false;
    if (Randy.randomChance(0.7f) && ActorTool.countContagiousNearby(pTarget.a) > 20 && Randy.randomChance(0.2f))
      ActionLibrary.tickPlagueInfection(pTarget.a);
    return true;
  }

  public static bool deathMark(BaseSimObject pTarget, WorldTile pTile = null)
  {
    if (Randy.randomChance(0.2f))
      pTarget.a.getHitFullHealth(AttackType.Divine);
    return true;
  }

  private static void tickPlagueInfection(Actor pActor)
  {
    pActor.spawnParticle(Toolbox.color_plague);
    if (!Randy.randomChance(0.05f))
      return;
    int num = 3;
    foreach (Actor actor in Finder.getUnitsFromChunk(pActor.current_tile, 0, 6f, true))
    {
      if (actor != pActor)
      {
        if (actor.addTrait("plague"))
          break;
        --num;
        if (num <= 0)
          break;
      }
    }
  }

  public static bool burningFeetEffectTileDraw(WorldTile pTile, string pPowerID)
  {
    if (pTile.isTemporaryFrozen() && Randy.randomBool())
      pTile.unfreeze();
    return true;
  }

  public static bool burningFeetEffect(BaseSimObject pSelf, WorldTile pTile = null)
  {
    WorldTile currentTile = pSelf.current_tile;
    if (!currentTile.Type.can_be_set_on_fire_by_burning_feet)
      return false;
    Actor a = pSelf.a;
    if (a.isInLiquid() || (a.has_attack_target ? 1 : (a.hasTag("moody") ? 1 : 0)) == 0)
      return false;
    World.world.loopWithBrush(currentTile, Brush.get(4), new PowerActionWithID(ActionLibrary.burningFeetEffectTileDraw));
    currentTile.startFire(true);
    for (int index = 0; index < currentTile.neighbours.Length; ++index)
    {
      WorldTile neighbour = currentTile.neighbours[index];
      neighbour.startFire();
      neighbour.setBurned();
    }
    return true;
  }

  public static bool flowerPrintsEffect(BaseSimObject pTarget, WorldTile pTile = null)
  {
    if (!Randy.randomChance(0.3f))
      return false;
    WorldTile currentTile = pTarget.a.current_tile;
    BiomeAsset biomeAsset = currentTile.Type.biome_asset;
    if (biomeAsset == null || !biomeAsset.grow_vegetation_auto)
      return false;
    if (biomeAsset.grow_type_selector_plants != null)
      BuildingActions.tryGrowVegetationRandom(currentTile, VegetationType.Plants, pCheckLimit: false);
    return true;
  }

  public static bool acidBloodEffect(BaseSimObject pTarget, WorldTile pTile = null)
  {
    for (int index = 0; index < 5; ++index)
    {
      if (Randy.randomBool())
        World.world.drop_manager.spawnParabolicDrop(pTarget.a.current_tile, "acid", pMinHeight: 0.1f, pMaxHeight: 5f, pMinRadius: 0.5f, pMaxRadius: 4f, pScale: 0.15f);
    }
    if (!pTarget.isActor() || pTarget.a.asset.actor_size < ActorSize.S17_Dragon)
      return true;
    for (int index1 = 0; index1 < 25; ++index1)
    {
      if (Randy.randomBool())
        World.world.drop_manager.spawnParabolicDrop(pTarget.a.current_tile, "acid", pMinHeight: 0.1f, pMaxHeight: 10f, pMinRadius: 0.5f, pMaxRadius: 10f, pScale: 0.15f);
      for (int index2 = 0; index2 < pTarget.a.current_tile.neighboursAll.Length; ++index2)
      {
        WorldTile pTile1 = pTarget.a.current_tile.neighboursAll[index2];
        if (Randy.randomBool())
          World.world.drop_manager.spawnParabolicDrop(pTile1, "acid", pMinHeight: 0.1f, pMaxHeight: 10f, pMinRadius: 0.5f, pMaxRadius: 7f, pScale: 0.15f);
      }
    }
    return true;
  }

  public static bool acidTouchEffect(BaseSimObject pTarget, WorldTile pTile = null)
  {
    if (!Randy.randomChance(0.3f))
      return false;
    MapAction.checkAcidTerraform(pTarget.a.current_tile);
    return true;
  }

  public static bool sunblessedEffect(BaseSimObject pTarget, WorldTile pTile = null)
  {
    if (!Randy.randomChance(0.5f) || World.world.era_manager.getCurrentAge().flag_night)
      return false;
    float pVal = Randy.randomFloat(0.05f, 0.1f);
    pTarget.a.restoreHealthPercent(pVal);
    return true;
  }

  public static bool castSpawnSkeleton(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
  {
    if (pTarget != null)
      pTile = pTarget.current_tile;
    int num = 0;
    foreach (Actor actor in Finder.findSpeciesAroundTileChunk(pTile, "skeleton"))
    {
      if (num++ > 6)
        return false;
    }
    WorldTile randomTile = pTile?.region?.getRandomTile();
    if (randomTile == null)
      return false;
    ActionLibrary.spawnSkeleton(pSelf, randomTile);
    return true;
  }

  public static bool spawnSkeleton(BaseSimObject pCaster, WorldTile pTile = null)
  {
    if (pTile == null)
      return false;
    BaseEffect baseEffect = EffectsLibrary.spawnAt("fx_create_skeleton", pTile.posV3, 0.1f);
    Actor tActorCaster = pCaster.a;
    Subspecies tTargetSubspecies = (Subspecies) null;
    TileZone currentZone = tActorCaster.current_zone;
    bool tNeedNewSkeletonForm = false;
    Subspecies tSubspeciesTargetForNewSkeleton = (Subspecies) null;
    City city1 = currentZone.city;
    if (city1 != null && !city1.kingdom.isNeutral())
    {
      Subspecies mainSubspecies = city1.getMainSubspecies();
      tTargetSubspecies = mainSubspecies?.getSkeletonForm();
      if (tTargetSubspecies == null)
      {
        tNeedNewSkeletonForm = true;
        tSubspeciesTargetForNewSkeleton = mainSubspecies;
      }
    }
    else if (tActorCaster.hasCity())
      tTargetSubspecies = tActorCaster.city.getSubspecies("skeleton");
    BaseCallback pCallback = (BaseCallback) (() =>
    {
      Actor newUnit = World.world.units.createNewUnit("skeleton", pTile, pSubspecies: tTargetSubspecies, pAdultAge: true);
      newUnit.makeWait(1f);
      if (tActorCaster.isRekt())
        return;
      if (newUnit.subspecies.isJustCreated() && tActorCaster.isKingdomCiv())
        newUnit.subspecies.addTrait("prefrontal_cortex");
      if (newUnit.subspecies.isJustCreated() && tNeedNewSkeletonForm && !tSubspeciesTargetForNewSkeleton.isRekt())
        tSubspeciesTargetForNewSkeleton.setSkeletonForm(newUnit.subspecies);
      if (!tActorCaster.isKingdomCiv() || !newUnit.subspecies.hasTrait("prefrontal_cortex"))
        return;
      City city2 = tActorCaster.city;
      Kingdom kingdom = tActorCaster.kingdom;
      if (!city2.isRekt() && city2.kingdom == kingdom)
        newUnit.joinCity(tActorCaster.city);
      else
        newUnit.joinKingdom(kingdom);
    });
    baseEffect.setCallback(19, pCallback);
    return true;
  }

  public static bool castFire(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
  {
    if (pTarget != null)
      pTile = pTarget.current_tile;
    if (pTile == null)
      return false;
    World.world.drop_manager.spawn(pTile, "fire", 15f);
    for (int index = 0; index < 3; ++index)
      World.world.drop_manager.spawn(pTile.neighboursAll.GetRandom<WorldTile>(), "fire", 15f);
    return true;
  }

  public static bool castSpellSilence(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
  {
    if (pTarget != null)
      pTile = pTarget.current_tile;
    if (pTile == null)
      return false;
    World.world.drop_manager.spawn(pTile, "spell_silence", 15f);
    return true;
  }

  public static bool castBloodRain(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
  {
    if (pTarget != null)
      pTile = pTarget.current_tile;
    if (pTile == null)
      return false;
    World.world.drop_manager.spawn(pTile, "blood_rain", 15f, pCasterId: pSelf.id);
    return true;
  }

  public static bool castSpawnGrassSeeds(
    BaseSimObject pSelf,
    BaseSimObject pTarget,
    WorldTile pTile = null)
  {
    if (pTile == null)
      pTile = pTarget.current_tile;
    if (pTile == null || WorldLawLibrary.world_law_gaias_covenant.isEnabled())
      return false;
    World.world.drop_manager.spawn(pTile, "seeds_grass", 15f);
    return true;
  }

  public static bool castSpawnFertilizer(
    BaseSimObject pSelf,
    BaseSimObject pTarget,
    WorldTile pTile = null)
  {
    if (pTile == null)
      pTile = pTarget.current_tile;
    if (pTile == null)
      return false;
    World.world.drop_manager.spawn(pTile, "fertilizer_trees", 15f);
    return true;
  }

  public static bool castCurses(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
  {
    if (pTarget != null)
    {
      if (pTarget.a.hasStatus("cursed"))
        return false;
      pTile = pTarget.current_tile;
    }
    if (pTile == null)
      return false;
    World.world.drop_manager.spawn(pTile, "curse", 15f);
    return true;
  }

  public static bool castLightning(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
  {
    if (pTarget != null)
      pTile = pTarget.current_tile;
    MapBox.spawnLightningMedium(pTile, 0.15f, pSelf.a);
    return true;
  }

  public static bool castTornado(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
  {
    if (pTarget != null)
      pTile = pTarget.current_tile;
    if (pTile == null)
      return false;
    (EffectsLibrary.spawnAtTile("fx_tornado", pTile, 0.0833333358f) as TornadoEffect).resizeTornado(0.166666672f);
    return true;
  }

  public static bool castCure(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
  {
    if (pTile == null)
      pTile = pTarget.current_tile;
    if (pTile == null)
      return false;
    World.world.drop_manager.spawn(pTile, "cure", 15f);
    return true;
  }

  public static bool castShieldOnHimself(
    BaseSimObject pSelf,
    BaseSimObject pTarget,
    WorldTile pTile = null)
  {
    return ActionLibrary.addShieldEffectOnTarget(pSelf, pTarget);
  }

  public static bool addShieldEffectOnTarget(
    BaseSimObject pSelf,
    BaseSimObject pTarget,
    WorldTile pTile = null)
  {
    if (pTarget.hasStatus("shield"))
      return false;
    pTarget.a.addStatusEffect("shield", 30f);
    return true;
  }

  public static bool addBurningEffectOnTarget(
    BaseSimObject pSelf,
    BaseSimObject pTarget,
    WorldTile pTile = null)
  {
    if (!pTarget.isAlive())
      return false;
    if (pTarget.isBuilding() && pTarget.b.isBurnable())
    {
      pTarget.addStatusEffect("burning");
      return true;
    }
    if (!pTarget.isActor())
      return false;
    pTarget.addStatusEffect("burning");
    return true;
  }

  public static bool addFrozenEffectOnTarget20(
    BaseSimObject pSelf,
    BaseSimObject pTarget,
    WorldTile pTile = null)
  {
    return pTarget.isAlive() && !pTarget.isBuilding() && Randy.randomChance(0.2f) && ActionLibrary.addFrozenEffectOnTarget(pSelf, pTarget, pTile);
  }

  public static bool addStunnedEffectOnTarget20(
    BaseSimObject pSelf,
    BaseSimObject pTarget,
    WorldTile pTile = null)
  {
    return !pTarget.isRekt() && !pTarget.isBuilding() && Randy.randomChance(0.2f) && ActionLibrary.addStunnedEffectOnTarget(pSelf, pTarget, pTile);
  }

  public static bool addStunnedEffectOnTarget(
    BaseSimObject pSelf,
    BaseSimObject pTarget,
    WorldTile pTile = null)
  {
    if (pTarget.isRekt() || pTarget.isBuilding())
      return false;
    pTarget.addStatusEffect("stunned");
    return true;
  }

  public static bool addFrozenEffectOnTarget(
    BaseSimObject pSelf,
    BaseSimObject pTarget,
    WorldTile pTile = null)
  {
    if (pTarget.isBuilding() || pTarget.current_tile.Type.lava || pTarget.current_tile.isOnFire())
      return false;
    pTarget.addStatusEffect("frozen");
    return true;
  }

  public static bool addSlowEffectOnTarget20(
    BaseSimObject pSelf,
    BaseSimObject pTarget,
    WorldTile pTile = null)
  {
    return pTarget.isAlive() && !pTarget.isBuilding() && Randy.randomChance(0.2f) && ActionLibrary.addSlowEffectOnTarget(pSelf, pTarget, pTile);
  }

  public static bool addSlowEffectOnTarget(
    BaseSimObject pSelf,
    BaseSimObject pTarget,
    WorldTile pTile = null)
  {
    if (pTarget.isBuilding())
      return false;
    pTarget.addStatusEffect("slowness");
    return true;
  }

  public static bool addPoisonedEffectOnTarget(
    BaseSimObject pSelf,
    BaseSimObject pTarget,
    WorldTile pTile = null)
  {
    if (!pTarget.isActor() || !pTarget.isAlive() || pTarget.a.hasTrait("poison_immune") || !pTarget.a.asset.has_skin || pTarget.a.asset.immune_to_injuries || !Randy.randomChance(0.3f))
      return false;
    pTarget.a.addStatusEffect("poisoned");
    return false;
  }

  public static void increaseDroppedBombsCounter(WorldTile pTile = null, string pDropID = null)
  {
    ++World.world.game_stats.data.bombsDropped;
    AchievementLibrary.many_bombs.check();
  }

  public static bool giveCursed(WorldTile pTile, Actor pActor)
  {
    if (pActor.hasSubspecies() && pActor.subspecies.hasTrait("adaptation_corruption"))
      return false;
    int num = pActor.addStatusEffect("cursed") ? 1 : 0;
    if (num == 0)
      return num != 0;
    pActor.removeTrait("blessed");
    return num != 0;
  }

  public static bool singularityTeleportation(WorldTile pTile, Actor pActor)
  {
    BiomeAsset biomeAsset = AssetManager.biome_library.get("biome_singularity");
    WorldTile pTile1 = (WorldTile) null;
    if (biomeAsset.getTileHigh().hashset.Count > 0 && Randy.randomBool())
      pTile1 = biomeAsset.getTileHigh().hashset.GetRandom<WorldTile>();
    else if (biomeAsset.getTileLow().hashset.Count > 0)
      pTile1 = biomeAsset.getTileLow().hashset.GetRandom<WorldTile>();
    if (pTile1 == null)
      return false;
    EffectsLibrary.spawnAt("fx_teleport_singularity", pTile1.posV3, pActor.stats["scale"] * 1.2f);
    EffectsLibrary.spawnAt("fx_teleport_singularity", pActor.current_position, pActor.stats["scale"] * 1.2f);
    pActor.cancelAllBeh();
    pActor.spawnOn(pTile1);
    pActor.makeStunned();
    return true;
  }

  public static bool timeParadox(WorldTile pTile, Actor pActor)
  {
    if (!pActor.isAlive())
      return false;
    ++pActor.data.age_overgrowth;
    return true;
  }

  public static bool giveEnchanted(WorldTile pTile, Actor pActor)
  {
    pActor.finishStatusEffect("cursed");
    return pActor.addStatusEffect("enchanted");
  }

  public static bool spawnGhost(BaseSimObject pTarget, WorldTile pTile = null)
  {
    if (!pTarget.isActor() || !pTarget.a.asset.has_soul)
      return false;
    Actor newUnit = World.world.units.createNewUnit("ghost", pTile);
    newUnit.removeTrait("blessed");
    ActorTool.copyUnitToOtherUnit(pTarget.a, newUnit);
    return true;
  }

  public static bool tryToGrowBiomeGrass(BaseSimObject pTarget, WorldTile pTile = null)
  {
    if (!pTile.Type.can_be_biome || pTile.Type.is_biome)
      return false;
    DropsLibrary.useSeedOn(pTile, TopTileLibrary.grass_low, TopTileLibrary.grass_high);
    return true;
  }

  public static bool tryToGrowTree(BaseSimObject pTarget, WorldTile pTile = null)
  {
    BuildingActions.tryGrowVegetationRandom(pTile, VegetationType.Trees, pCheckLimit: false);
    return true;
  }

  public static bool tryToCreatePlants(BaseSimObject pTarget, WorldTile pTile = null)
  {
    BiomeAsset biomeAsset = pTarget.current_tile.Type.biome_asset;
    if (biomeAsset == null)
      return false;
    if (biomeAsset.grow_type_selector_plants != null)
      BuildingActions.tryGrowVegetationRandom(pTarget.current_tile, VegetationType.Plants);
    return true;
  }

  public static bool startNuke(BaseSimObject pTarget, WorldTile pTile = null)
  {
    pTarget.a.findCurrentTile();
    EffectsLibrary.spawn("fx_nuke_flash", pTile, "atomic_bomb");
    return true;
  }

  public static bool clearCrabzilla(BaseSimObject pTarget, WorldTile pTile = null)
  {
    MusicBox.inst.stopDrawingSound("event:/SFX/UNIQUE/Crabzilla/CrabzillaLazer");
    MusicBox.inst.stopDrawingSound("event:/SFX/UNIQUE/Crabzilla/CrabzillaVoice");
    if (Config.joyControls)
      UltimateJoystick.ResetJoysticks();
    return true;
  }

  public static bool startCrabzillaNuke(BaseSimObject pTarget, WorldTile pTile = null)
  {
    pTarget.a.findCurrentTile();
    EffectsLibrary.spawn("fx_nuke_flash", pTile, "crabzilla_bomb");
    return true;
  }

  public static bool deathNuke(BaseSimObject pTarget, WorldTile pTile = null)
  {
    pTarget.a.findCurrentTile();
    DropsLibrary.action_atomic_bomb(pTarget.current_tile);
    return true;
  }

  public static bool deathBomb(BaseSimObject pTarget, WorldTile pTile = null)
  {
    pTarget.a.findCurrentTile();
    DropsLibrary.action_bomb(pTarget.current_tile);
    return true;
  }

  public static bool spawnAliens(BaseSimObject pTarget, WorldTile pTile = null)
  {
    Actor a = pTarget.a;
    a.findCurrentTile();
    if (!a.inMapBorder())
      return false;
    int num = 1;
    if (Randy.randomChance(0.5f))
      ++num;
    if (Randy.randomChance(0.1f))
      ++num;
    for (int index = 0; index < num; ++index)
      World.world.units.createNewUnit("alien", pTarget.a.current_tile, pSpawnHeight: pTarget.a.position_height, pAdultAge: true);
    return true;
  }

  public static bool fireDropsSpawn(BaseSimObject pTarget, WorldTile pTile = null)
  {
    for (int index = 0; index < 5; ++index)
    {
      if (Randy.randomBool())
        World.world.drop_manager.spawnParabolicDrop(pTarget.a.current_tile, "fire", pMinHeight: 0.1f, pMaxHeight: 5f, pMinRadius: 0.5f, pMaxRadius: 4f, pScale: 0.15f);
    }
    if (!pTarget.isActor() || pTarget.a.asset.actor_size < ActorSize.S17_Dragon)
      return true;
    for (int index1 = 0; index1 < 25; ++index1)
    {
      if (Randy.randomBool())
        World.world.drop_manager.spawnParabolicDrop(pTarget.a.current_tile, "fire", pMinHeight: 0.1f, pMaxHeight: 10f, pMinRadius: 0.5f, pMaxRadius: 10f, pScale: 0.15f);
      for (int index2 = 0; index2 < pTarget.a.current_tile.neighboursAll.Length; ++index2)
      {
        WorldTile pTile1 = pTarget.a.current_tile.neighboursAll[index2];
        if (Randy.randomBool())
          World.world.drop_manager.spawnParabolicDrop(pTile1, "fire", pMinHeight: 0.1f, pMaxHeight: 10f, pMinRadius: 0.5f, pMaxRadius: 7f, pScale: 0.15f);
      }
    }
    return true;
  }

  public static bool snowDropsSpawn(BaseSimObject pTarget, WorldTile pTile = null)
  {
    for (int index = 0; index < 20; ++index)
    {
      if (Randy.randomBool())
        World.world.drop_manager.spawnParabolicDrop(pTarget.a.current_tile, "snow", pMinHeight: 0.1f, pMaxHeight: 5f, pMinRadius: 0.5f, pMaxRadius: 4f, pScale: 0.15f);
    }
    return true;
  }

  public static bool teleportRandom(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
  {
    TileIsland randomIslandGround = World.world.islands_calculator.getRandomIslandGround();
    WorldTile worldTile;
    if (randomIslandGround == null)
    {
      worldTile = (WorldTile) null;
    }
    else
    {
      MapRegion random = randomIslandGround.regions.GetRandom();
      worldTile = random != null ? random.tiles.GetRandom<WorldTile>() : (WorldTile) null;
    }
    WorldTile pTile1 = worldTile;
    if (pTile1 == null || pTile1.Type.block || !pTile1.Type.ground)
      return false;
    ActionLibrary.teleportEffect(pTarget.a, pTile1);
    pTarget.a.cancelAllBeh();
    pTarget.a.spawnOn(pTile1);
    return true;
  }

  public static void teleportEffect(Actor pActor, WorldTile pTile)
  {
    string pID = pActor.asset.effect_teleport;
    if (string.IsNullOrEmpty(pID))
      pID = "fx_teleport_blue";
    EffectsLibrary.spawnAt(pID, pActor.current_position, pActor.stats["scale"]);
    BaseEffect baseEffect = EffectsLibrary.spawnAt(pID, pTile.posV3, pActor.stats["scale"]);
    if (!Object.op_Inequality((Object) baseEffect, (Object) null))
      return;
    baseEffect.sprite_animation.setFrameIndex(9);
  }

  public static bool metamorphInto(
    Actor pTarget,
    string pAsset,
    bool pRemoveAcquiredTraits = false,
    bool pUseCurrentSubspecies = false)
  {
    if (pTarget == null || !pTarget.inMapBorder() || pTarget.isAlreadyTransformed())
      return false;
    pTarget.finishStatusEffect("cursed");
    pTarget.removeTrait("infected");
    pTarget.removeTrait("mush_spores");
    pTarget.removeTrait("tumor_infection");
    if (pRemoveAcquiredTraits)
    {
      IReadOnlyCollection<ActorTrait> traits = pTarget.getTraits();
      using (ListPool<ActorTrait> pTraits = new ListPool<ActorTrait>(traits.Count))
      {
        foreach (ActorTrait actorTrait in (IEnumerable<ActorTrait>) traits)
        {
          if (actorTrait.group_id == "acquired")
            pTraits.Add(actorTrait);
        }
        pTarget.removeTraits((ICollection<ActorTrait>) pTraits);
      }
    }
    Subspecies pSubspecies = (Subspecies) null;
    if (pUseCurrentSubspecies)
      pSubspecies = pTarget.subspecies;
    Actor newUnit = World.world.units.createNewUnit(pAsset, pTarget.current_tile, pSubspecies: pSubspecies, pSpawnWithItems: false);
    ActorTool.copyUnitToOtherUnit(pTarget, newUnit, false);
    EffectsLibrary.spawn("fx_spawn", newUnit.current_tile);
    ActionLibrary.removeUnit(pTarget);
    pTarget.setTransformed();
    newUnit.addTrait("metamorphed");
    return true;
  }

  public static bool turnIntoMush(BaseSimObject pTarget, WorldTile pTile = null)
  {
    Actor a = pTarget.a;
    if (a == null || !a.hasTrait("mush_spores") || !a.inMapBorder() || !a.asset.can_turn_into_mush || a.isAlreadyTransformed())
      return false;
    a.finishStatusEffect("cursed");
    a.removeTrait("infected");
    a.removeTrait("mush_spores");
    a.removeTrait("tumor_infection");
    a.removeTrait("peaceful");
    Actor newUnit = World.world.units.createNewUnit(a.asset.mush_id, a.current_tile, pSpawnWithItems: false);
    ActorTool.copyUnitToOtherUnit(a, newUnit);
    if (MapBox.isRenderGameplay())
      EffectsLibrary.spawn("fx_spawn", newUnit.current_tile);
    ActionLibrary.removeUnit(pTarget.a);
    a.setTransformed();
    return true;
  }

  public static Actor turnIntoMetamorph(BaseSimObject pTarget, string pAssetID)
  {
    Actor a = pTarget.a;
    if (a == null)
      return (Actor) null;
    if (!a.inMapBorder())
      return (Actor) null;
    if (a.isAlreadyTransformed())
      return (Actor) null;
    a.finishStatusEffect("cursed");
    a.removeTrait("infected");
    a.removeTrait("mush_spores");
    a.removeTrait("tumor_infection");
    a.removeTrait("peaceful");
    Actor newUnit = World.world.units.createNewUnit(pAssetID, a.current_tile, pSpawnWithItems: false);
    ActorTool.copyUnitToOtherUnit(a, newUnit);
    EffectsLibrary.spawn("fx_spawn", newUnit.current_tile);
    ActionLibrary.removeUnit(pTarget.a);
    a.setTransformed();
    return newUnit;
  }

  public static Actor turnIntoIceOne(BaseSimObject pTarget, WorldTile pTile = null)
  {
    return ActionLibrary.turnIntoMetamorph(pTarget, "cold_one");
  }

  public static bool turnIntoDemon(BaseSimObject pTarget, WorldTile pTile = null)
  {
    Actor a = pTarget.a;
    if (a == null || !a.inMapBorder() || a.isAlreadyTransformed())
      return false;
    a.finishStatusEffect("cursed");
    a.removeTrait("infected");
    a.removeTrait("mush_spores");
    a.removeTrait("tumor_infection");
    a.removeTrait("peaceful");
    Actor newUnit = World.world.units.createNewUnit("demon", a.current_tile, pSpawnWithItems: false);
    ActorTool.copyUnitToOtherUnit(a, newUnit);
    EffectsLibrary.spawn("fx_spawn", newUnit.current_tile);
    ActionLibrary.removeUnit(pTarget.a);
    a.setTransformed();
    return true;
  }

  public static bool turnIntoTumorMonster(BaseSimObject pTarget, WorldTile pTile = null)
  {
    Actor a = pTarget.a;
    if (a == null || !a.hasTrait("tumor_infection") || !a.inMapBorder() || !a.asset.can_turn_into_tumor || a.isAlreadyTransformed())
      return false;
    a.finishStatusEffect("cursed");
    a.removeTrait("infected");
    a.removeTrait("mush_spores");
    a.removeTrait("tumor_infection");
    a.removeTrait("peaceful");
    Actor newUnit = World.world.units.createNewUnit(a.asset.tumor_id, a.current_tile, pSpawnWithItems: false);
    ActorTool.copyUnitToOtherUnit(a, newUnit);
    EffectsLibrary.spawn("fx_spawn", newUnit.current_tile);
    ActionLibrary.removeUnit(pTarget.a);
    a.setTransformed();
    return true;
  }

  public static bool turnIntoZombie(BaseSimObject pTarget, WorldTile pTile = null)
  {
    Actor a = pTarget.a;
    if (a == null || !a.hasTrait("infected") || !a.inMapBorder() || !a.asset.can_turn_into_zombie || a.isAlreadyTransformed())
      return false;
    a.finishStatusEffect("cursed");
    a.removeTrait("infected");
    a.removeTrait("mush_spores");
    a.removeTrait("tumor_infection");
    string zombieId = a.asset.getZombieID();
    if (a.asset.id == "dragon")
    {
      a.removeTrait("fire_blood");
      a.removeTrait("fire_proof");
    }
    Actor newUnit = World.world.units.createNewUnit(zombieId, a.current_tile, pSubspeciesMutateFrom: a.subspecies, pSpawnWithItems: false);
    ActorTool.copyUnitToOtherUnit(a, newUnit);
    newUnit.removeTrait("fast");
    newUnit.removeTrait("agile");
    newUnit.removeTrait("genius");
    newUnit.removeTrait("peaceful");
    if (!a.getName().StartsWith("Un"))
      newUnit.setName("Un" + Toolbox.LowerCaseFirst(a.getName()));
    EffectsLibrary.spawn("fx_spawn", newUnit.current_tile);
    ActionLibrary.removeUnit(pTarget.a);
    a.setTransformed();
    return true;
  }

  public static bool turnIntoSkeleton(BaseSimObject pTarget, WorldTile pTile = null)
  {
    Actor a = pTarget.a;
    if (string.IsNullOrEmpty(a.asset.skeleton_id) || a == null || !a.hasStatus("cursed") || !a.inMapBorder() || a.isAlreadyTransformed())
      return false;
    string skeletonId = a.asset.skeleton_id;
    a.finishStatusEffect("cursed");
    a.removeTrait("infected");
    a.removeTrait("mush_spores");
    a.removeTrait("tumor_infection");
    Subspecies pSubspecies = (Subspecies) null;
    if (a.hasSubspecies())
      pSubspecies = a.subspecies.getSkeletonForm();
    Actor newUnit = World.world.units.createNewUnit(skeletonId, a.current_tile, pSubspecies: pSubspecies, pSpawnWithItems: false);
    Subspecies subspecies = newUnit.subspecies;
    if (subspecies.isJustCreated() && pSubspecies != null)
      pSubspecies.setSkeletonForm(subspecies);
    ActorTool.copyUnitToOtherUnit(a, newUnit);
    if (!a.getName().StartsWith("Un"))
      newUnit.setName("Un" + Toolbox.LowerCaseFirst(a.getName()));
    EffectsLibrary.spawn("fx_spawn", newUnit.current_tile);
    ActionLibrary.removeUnit(pTarget.a);
    a.setTransformed();
    return true;
  }

  public static Actor getActorNearPos(Vector2 pPos)
  {
    Actor actorNearPos = (Actor) null;
    float num1 = float.MaxValue;
    Actor[] array = World.world.units.visible_units.array;
    int count = World.world.units.visible_units.count;
    for (int index = 0; index < count; ++index)
    {
      Actor actor = array[index];
      if (actor.isAlive() && actor.asset.can_be_inspected && !actor.isInsideSomething())
      {
        float num2 = Toolbox.DistVec2Float(actor.current_position, pPos);
        if ((double) num2 <= 3.0 && (double) num2 < (double) num1)
        {
          actorNearPos = actor;
          num1 = num2;
        }
      }
    }
    return actorNearPos;
  }

  public static Actor getActorFromTile(WorldTile pTile = null)
  {
    if (pTile == null)
      return (Actor) null;
    Actor actorFromTile = (Actor) null;
    float num1 = float.MaxValue;
    List<Actor> simpleList = World.world.units.getSimpleList();
    for (int index = 0; index < simpleList.Count; ++index)
    {
      Actor actor = simpleList[index];
      if (actor.isAlive())
      {
        float num2 = (float) Toolbox.SquaredDistTile(actor.current_tile, pTile);
        if ((double) num2 <= (double) num1 && (double) num2 <= 9.0 && actor.asset.can_be_inspected && !actor.isInsideSomething())
        {
          actorFromTile = actor;
          num1 = num2;
        }
      }
    }
    return actorFromTile;
  }

  public static void openUnitWindow(Actor pActor)
  {
    if (!pActor.isRekt())
    {
      SelectedUnit.clear();
      SelectedUnit.select(pActor);
    }
    else if (!SelectedUnit.isSet())
      return;
    ScrollWindow.showWindow("unit");
  }

  public static bool inspectUnit(WorldTile pTile = null, string pPower = null)
  {
    Actor pActor = pTile != null ? ActionLibrary.getActorFromTile(pTile) : World.world.getActorNearCursor();
    if (pActor == null)
      return false;
    ActionLibrary.openUnitWindow(pActor);
    return true;
  }

  public static bool inspectUnitSelectedMeta(WorldTile pTile = null, string pPower = null)
  {
    Actor actor = pTile != null ? ActionLibrary.getActorFromTile(pTile) : World.world.getActorNearCursor();
    if (actor == null)
      return false;
    MetaTypeAsset asset = Zones.getCurrentMapBorderMode().getAsset();
    if (asset == null)
      return false;
    if (asset.check_unit_has_meta(actor))
    {
      asset.set_unit_set_meta_for_meta_for_window(actor);
      ScrollWindow.showWindow(asset.window_name);
      return true;
    }
    ActionLibrary.openUnitWindow(actor);
    return true;
  }

  public static bool inspectCity(WorldTile pTile = null, string pPower = null)
  {
    if (pTile == null)
      return false;
    NanoObject nanoObjectFromTile = ActionLibrary.getNanoObjectFromTile(pTile, MetaTypeLibrary.city);
    if (nanoObjectFromTile == null)
      return false;
    MetaType.City.getAsset().selectAndInspect(nanoObjectFromTile);
    return true;
  }

  public static bool inspectKingdom(WorldTile pTile = null, string pPower = null)
  {
    if (pTile == null)
      return false;
    NanoObject nanoObjectFromTile = ActionLibrary.getNanoObjectFromTile(pTile, MetaTypeLibrary.kingdom);
    if (nanoObjectFromTile == null || ((Kingdom) nanoObjectFromTile).isNeutral())
      return false;
    MetaType.Kingdom.getAsset().selectAndInspect(nanoObjectFromTile);
    return true;
  }

  public static bool inspectAlliance(WorldTile pTile = null, string pPower = null)
  {
    if (pTile == null)
      return false;
    City city = pTile.zone.city;
    if (city.isRekt())
      return false;
    Kingdom kingdom = city.kingdom;
    if (kingdom.isRekt() || kingdom.isNeutral())
      return false;
    if (kingdom.hasAlliance())
      MetaType.Alliance.getAsset().selectAndInspect((NanoObject) kingdom.getAlliance());
    else
      ActionLibrary.inspectKingdom(pTile, pPower);
    return true;
  }

  public static bool inspectCulture(WorldTile pTile, string pPower = null)
  {
    if (pTile == null)
      return false;
    NanoObject nanoObjectFromTile = ActionLibrary.getNanoObjectFromTile(pTile, MetaTypeLibrary.culture);
    if (nanoObjectFromTile == null)
      return false;
    MetaType.Culture.getAsset().selectAndInspect(nanoObjectFromTile);
    return true;
  }

  public static bool inspectReligion(WorldTile pTile, string pPower = null)
  {
    if (pTile == null)
      return false;
    NanoObject nanoObjectFromTile = ActionLibrary.getNanoObjectFromTile(pTile, MetaTypeLibrary.religion);
    if (nanoObjectFromTile == null)
      return false;
    MetaType.Religion.getAsset().selectAndInspect(nanoObjectFromTile);
    return true;
  }

  public static bool inspectSubspecies(WorldTile pTile, string pPower = null)
  {
    if (pTile == null)
      return false;
    NanoObject nanoObjectFromTile = ActionLibrary.getNanoObjectFromTile(pTile, MetaTypeLibrary.subspecies);
    if (nanoObjectFromTile == null)
      return false;
    MetaType.Subspecies.getAsset().selectAndInspect(nanoObjectFromTile);
    return true;
  }

  public static bool inspectFamily(WorldTile pTile, string pPower = null)
  {
    if (pTile == null)
      return false;
    NanoObject nanoObjectFromTile = ActionLibrary.getNanoObjectFromTile(pTile, MetaTypeLibrary.family);
    if (nanoObjectFromTile == null)
      return false;
    MetaType.Family.getAsset().selectAndInspect(nanoObjectFromTile);
    return true;
  }

  public static bool inspectArmy(WorldTile pTile, string pPower = null)
  {
    if (pTile == null)
      return false;
    NanoObject nanoObjectFromTile = ActionLibrary.getNanoObjectFromTile(pTile, MetaTypeLibrary.army);
    if (nanoObjectFromTile == null)
      return false;
    MetaType.Army.getAsset().selectAndInspect(nanoObjectFromTile);
    return true;
  }

  public static bool inspectLanguage(WorldTile pTile, string pPower = null)
  {
    if (pTile == null)
      return false;
    NanoObject nanoObjectFromTile = ActionLibrary.getNanoObjectFromTile(pTile, MetaTypeLibrary.language);
    if (nanoObjectFromTile == null)
      return false;
    MetaType.Language.getAsset().selectAndInspect(nanoObjectFromTile);
    return true;
  }

  public static bool inspectClan(WorldTile pTile, string pPower = null)
  {
    if (pTile == null)
      return false;
    NanoObject nanoObjectFromTile = ActionLibrary.getNanoObjectFromTile(pTile, MetaTypeLibrary.clan);
    if (nanoObjectFromTile == null)
      return false;
    MetaType.Clan.getAsset().selectAndInspect(nanoObjectFromTile);
    return true;
  }

  public static bool burnTile(BaseSimObject pSelf, BaseSimObject pTarget = null, WorldTile pTile = null)
  {
    if (!World.world.flash_effects.contains(pTile) && Randy.randomChance(0.2f))
      World.world.particles_fire.spawn(pTile.posV3);
    pTile.startFire(true);
    return true;
  }

  public static bool tryToEvolveUnitViaMonolith(Actor pActor)
  {
    pActor.startShake();
    pActor.startColorEffect();
    if (!pActor.hasSubspecies() || pActor.hasSubspeciesTrait("pure"))
      return false;
    float pVal = 1f;
    if (pActor.asset.can_evolve_into_new_species)
      pVal = 1f;
    else if (pActor.hasSubspeciesTrait("uplifted") && pActor.subspecies.isSapient())
      pVal = 0.1f;
    if (!Randy.randomChance(pVal))
      return false;
    World.world.units.evolutionEvent(pActor, true, false);
    return true;
  }

  public static bool tryToEvolveUnitViaAscension(Actor pActor, out Actor pEvolvedActorForm)
  {
    pEvolvedActorForm = (Actor) null;
    pActor.startShake();
    pActor.startColorEffect();
    if (!pActor.hasSubspecies() || pActor.hasSubspeciesTrait("pure"))
      return false;
    Actor actor = World.world.units.evolutionEvent(pActor, true, true);
    pEvolvedActorForm = actor;
    return true;
  }

  public static void startBurningObjects(
    BaseSimObject pSelf,
    BaseSimObject pTarget = null,
    WorldTile pTile = null)
  {
    List<BaseSimObject> allObjectsInChunks = Finder.getAllObjectsInChunks(pTile);
    for (int index = 0; index < allObjectsInChunks.Count; ++index)
    {
      BaseSimObject pTarget1 = allObjectsInChunks[index];
      if (pTarget1.isAlive() && !pTarget1.current_tile.Type.ocean)
        ActionLibrary.addBurningEffectOnTarget(pSelf, pTarget1);
    }
  }

  public static void action_growTornadoes(WorldTile pTile = null, string pDropID = null)
  {
    TornadoEffect.growTornados(pTile);
  }

  public static void action_shrinkTornadoes(WorldTile pTile = null, string pDropID = null)
  {
    TornadoEffect.shrinkTornados(pTile);
  }

  public static bool dragonSlayer(BaseSimObject pTarget, WorldTile pTile = null)
  {
    if (pTarget == null || !pTarget.isActor())
      return false;
    BaseSimObject attackedBy = pTarget.a.attackedBy;
    if (attackedBy == null || !attackedBy.isActor() || !attackedBy.isAlive())
      return false;
    attackedBy.a.addTrait("dragonslayer");
    return true;
  }

  public static bool mageSlayerCheck(BaseSimObject pTarget, WorldTile pTile = null)
  {
    if (pTarget == null || !pTarget.isActor() || !pTarget.a.hasSpells())
      return false;
    BaseSimObject attackedBy = pTarget.a.attackedBy;
    if (attackedBy == null || !attackedBy.isActor() || !attackedBy.isAlive())
      return false;
    attackedBy.a.addTrait("mageslayer");
    return true;
  }

  public static bool checkPiranhaAchievement(BaseSimObject pTarget, WorldTile pTile = null)
  {
    AchievementLibrary.piranha_land.check((object) pTarget.a);
    return true;
  }

  public static bool clickRelations(WorldTile pTile, string pPowerID)
  {
    City city = pTile.zone.city;
    if (city.isRekt())
      return false;
    Kingdom kingdom = city.kingdom;
    if (kingdom.isRekt() || kingdom.isNeutral())
      return false;
    if (SelectedMetas.selected_kingdom != kingdom)
      SelectedMetas.selected_kingdom = kingdom;
    else
      ScrollWindow.showWindow("kingdom");
    return true;
  }

  public static bool clickWhisperOfWar(WorldTile pTile, string pPowerID)
  {
    // ISSUE: unable to decompile the method.
  }

  public static bool clickUnity(WorldTile pTile, string pPowerID)
  {
    City city = pTile.zone.city;
    if (city.isRekt())
      return false;
    Kingdom kingdom = city.kingdom;
    if (kingdom.isRekt() || kingdom.isNeutral())
      return false;
    if (Config.unity_A == null)
    {
      Config.unity_A = kingdom;
      ActionLibrary.showWhisperTip("unity_selected_first");
      return false;
    }
    if (Config.whisper_B == null && Config.unity_A == kingdom)
    {
      ActionLibrary.showWhisperTip("unity_cancelled");
      Config.unity_A = (Kingdom) null;
      Config.unity_B = (Kingdom) null;
      return false;
    }
    if (Config.unity_A.hasAlliance() && kingdom.hasAlliance() && Config.unity_A.getAlliance() == kingdom.getAlliance())
    {
      ActionLibrary.showWhisperTip("unity_cancelled");
      Config.unity_A = (Kingdom) null;
      Config.unity_B = (Kingdom) null;
      return false;
    }
    if (Config.unity_B == null)
      Config.unity_B = kingdom;
    if (Config.unity_B == Config.unity_A)
      return false;
    if (Config.unity_A.isEnemy(Config.unity_B))
    {
      ActionLibrary.showWhisperTip("unity_in_war");
      Config.unity_B = (Kingdom) null;
      return false;
    }
    if (Config.unity_A.hasAlliance())
    {
      if (Config.unity_A.getAlliance() == Config.unity_B.getAlliance())
      {
        ActionLibrary.showWhisperTip("unity_cancelled");
        Config.unity_B = (Kingdom) null;
        return false;
      }
      if (Config.unity_B.hasAlliance())
        Config.unity_A.getAlliance().leave(Config.unity_A);
    }
    if (World.world.alliances.forceAlliance(Config.unity_A, Config.unity_B))
      ActionLibrary.showWhisperTip("unity_new_alliance");
    else
      ActionLibrary.showWhisperTip("unity_joined_alliance");
    Config.unity_A.affectKingByPowers();
    Config.unity_A = (Kingdom) null;
    Config.unity_B = (Kingdom) null;
    World.world.zone_calculator.dirtyAndClear();
    return true;
  }

  private static void showWhisperTip(string pText)
  {
    string pText1 = LocalizedTextManager.getText(pText);
    if (Config.whisper_A != null)
      pText1 = pText1.Replace("$kingdom_A$", Config.whisper_A.name);
    if (Config.whisper_B != null)
      pText1 = pText1.Replace("$kingdom_B$", Config.whisper_B.name);
    WorldTip.showNow(pText1, false, "top", 6f);
  }

  public static bool selectWhisperOfWar(string pPowerID)
  {
    WorldTip.showNow("whisper_selected", pPosition: "top");
    Config.whisper_A = (Kingdom) null;
    Config.whisper_B = (Kingdom) null;
    return false;
  }

  public static bool selectUnity(string pPowerID)
  {
    WorldTip.showNow("unity_selected", pPosition: "top");
    Config.unity_A = (Kingdom) null;
    Config.unity_B = (Kingdom) null;
    return false;
  }

  public static bool selectRelations(string pPowerID)
  {
    SelectedMetas.selected_kingdom = World.world.kingdoms.getRandom();
    return false;
  }

  public static bool whirlwind(BaseSimObject pSelf, WorldTile pTile)
  {
    World.world.applyForceOnTile(pTile, pForceAmount: 3f, pForceOut: false, pByWho: pSelf);
    return true;
  }

  public static void removeUnit(Actor pActor) => pActor.removeByMetamorphosis();

  public static bool breakBones(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile)
  {
    if (!pTarget.isAlive())
      return false;
    if (pTarget.isActor())
      pTarget.a.addInjuryTrait("crippled");
    return true;
  }

  public static bool restoreMana(WorldTile pTile, Actor pSelf)
  {
    if (pSelf.isManaFull())
      return false;
    int pValue = (int) ((double) pSelf.getMaxMana() * 0.0099999997764825821);
    pSelf.addMana(pValue);
    return true;
  }

  public static bool restoreStamina(WorldTile pTile, Actor pSelf)
  {
    if (pSelf.isStaminaFull())
      return false;
    int pValue = (int) ((double) pSelf.getMaxStamina() * 0.0099999997764825821);
    pSelf.addStamina(pValue);
    return true;
  }

  public static bool restoreFullStats(NanoObject pTarget, BaseAugmentationAsset pTrait)
  {
    if (pTarget.isRekt())
      return false;
    ((BaseSimObject) pTarget).event_full_stats = true;
    return true;
  }

  public static bool forcedKingdomAdd(NanoObject pTarget, BaseAugmentationAsset pTrait)
  {
    if (!pTarget.isAlive())
      return false;
    ActorTrait actorTrait = (ActorTrait) pTrait;
    Actor actor = (Actor) pTarget;
    if (actor.asset.is_boat)
    {
      actor.getHitFullHealth(AttackType.Explosion);
      return false;
    }
    actor.applyForcedKingdomTrait();
    actor.setForcedKingdom(actorTrait.getForcedKingdom());
    return true;
  }

  public static bool forcedKingdomEffectRemove(NanoObject pTarget, BaseAugmentationAsset pTrait)
  {
    if (pTarget.isRekt())
      return false;
    ((Actor) pTarget).setDefaultKingdom();
    return true;
  }

  public static bool madnessEffectLoad(NanoObject pTarget, BaseAugmentationAsset pTrait)
  {
    if (pTarget.isRekt())
      return false;
    ((Actor) pTarget).setForcedKingdom(((ActorTrait) pTrait).getForcedKingdom());
    return true;
  }

  public static bool tryToMakeBuildingAlive(Building pBuilding)
  {
    if (!pBuilding.isAlive() || pBuilding.isRuin() || pBuilding.isUnderConstruction() || !pBuilding.asset.can_be_living_house)
      return false;
    Actor newUnit = World.world.units.createNewUnit("living_house", pBuilding.current_tile);
    newUnit.data.set("special_sprite_id", pBuilding.asset.id);
    newUnit.data.set("special_sprite_index", pBuilding.animData_index);
    newUnit.data.created_time = pBuilding.data.created_time;
    pBuilding.removeBuildingFinal();
    newUnit.startColorEffect();
    return true;
  }

  public static bool tryToMakeFloraAlive(Building pBuilding, bool pFullyGrownOnly = true)
  {
    if (!pBuilding.isAlive() || pBuilding.isRuin() || !pBuilding.asset.can_be_living_plant || pBuilding.chopped || pBuilding.isUnderConstruction() || pFullyGrownOnly && !pBuilding.isFullyGrown())
      return false;
    Actor newUnit = World.world.units.createNewUnit("living_plants", pBuilding.current_tile, pSpawnWithItems: false);
    newUnit.data.set("special_sprite_id", pBuilding.asset.id);
    newUnit.data.set("special_sprite_index", pBuilding.animData_index);
    newUnit.data.created_time = pBuilding.data.created_time;
    pBuilding.removeBuildingFinal();
    newUnit.startColorEffect();
    return true;
  }

  public static void growRandomVegetation(WorldTile pTile, BiomeAsset pBiomeAsset)
  {
    switch (Randy.randomInt(0, 3))
    {
      case 0:
        if (pBiomeAsset.grow_type_selector_trees == null)
          break;
        BuildingActions.tryGrowVegetationRandom(pTile, VegetationType.Trees);
        break;
      case 1:
        if (pBiomeAsset.grow_type_selector_plants == null)
          break;
        BuildingActions.tryGrowVegetationRandom(pTile, VegetationType.Plants);
        break;
      case 2:
        if (pBiomeAsset.grow_type_selector_bushes == null)
          break;
        BuildingActions.tryGrowVegetationRandom(pTile, VegetationType.Bushes);
        break;
    }
  }

  private static NanoObject getNanoObjectFromTile(WorldTile pTile, MetaTypeAsset pMetaTypeAsset)
  {
    if (pTile == null)
      return (NanoObject) null;
    NanoObject pObject = pMetaTypeAsset.tile_get_metaobject(pTile.zone, pMetaTypeAsset.getZoneOptionState()) as NanoObject;
    return pObject.isRekt() ? (NanoObject) null : pObject;
  }
}

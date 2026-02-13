// Decompiled with JetBrains decompiler
// Type: Kingdom
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using db;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Kingdom : MetaObjectWithTraits<KingdomData, KingdomTrait>
{
  public static KingdomCheckCache cache_enemy_check = new KingdomCheckCache();
  public KingdomAsset asset;
  public bool wild;
  public float timer_action;
  public Actor king;
  public City capital;
  public Culture culture;
  public Language language;
  public Religion religion;
  public readonly List<Building> buildings = new List<Building>();
  public readonly List<City> cities = new List<City>();
  public int power;
  public AiSystemKingdom ai;
  public Vector3 location;
  private float _cached_tax_local;
  private float _cached_tax_tribute;
  private bool _has_boats;

  protected override MetaType meta_type => MetaType.Kingdom;

  public override BaseSystemManager manager => (BaseSystemManager) World.world.kingdoms;

  protected override bool track_death_types => true;

  protected override void recalcBaseStats()
  {
    base.recalcBaseStats();
    this._cached_tax_local = SimGlobals.m.base_tax_rate_local;
    this._cached_tax_tribute = SimGlobals.m.base_tax_rate_tribute;
    foreach (KingdomTrait trait in (IEnumerable<KingdomTrait>) this.getTraits())
    {
      if (trait.is_local_tax_trait)
        this._cached_tax_local = trait.tax_rate;
      if (trait.is_tribute_tax_trait)
        this._cached_tax_tribute = trait.tax_rate;
    }
  }

  protected override AssetLibrary<KingdomTrait> trait_library
  {
    get => (AssetLibrary<KingdomTrait>) AssetManager.kingdoms_traits;
  }

  protected override List<string> default_traits => this.getActorAsset().default_kingdom_traits;

  protected override List<string> saved_traits => this.data.saved_traits;

  protected sealed override void setDefaultValues()
  {
    base.setDefaultValues();
    this.power = 1;
    this.timer_action = 5f;
  }

  protected override ColorLibrary getColorLibrary()
  {
    return (ColorLibrary) AssetManager.kingdom_colors_library;
  }

  public void clearListCities() => this.cities.Clear();

  public void clearBuildingList() => this.buildings.Clear();

  public override void increaseDeaths(AttackType pType)
  {
    if (!this.isAlive())
      return;
    base.increaseDeaths(pType);
    if (!this.hasAlliance())
      return;
    this.getAlliance().increaseDeaths(pType);
  }

  public override void increaseKills()
  {
    if (!this.isAlive())
      return;
    base.increaseKills();
    if (!this.hasAlliance())
      return;
    this.getAlliance().increaseKills();
  }

  public override void increaseBirths()
  {
    if (!this.isAlive())
      return;
    base.increaseBirths();
    if (this.hasAlliance())
      this.getAlliance().increaseBirths();
    this.addRenown(1);
  }

  public void increaseLeft()
  {
    if (!this.isAlive())
      return;
    ++this.data.left;
  }

  public void increaseJoined()
  {
    if (!this.isAlive())
      return;
    ++this.data.joined;
    this.addRenown(1);
  }

  public void increaseMoved()
  {
    if (!this.isAlive())
      return;
    ++this.data.moved;
    this.addRenown(2);
  }

  public void increaseMigrants()
  {
    if (!this.isAlive())
      return;
    ++this.data.migrated;
  }

  public long getTotalLeft() => this.data.left;

  public long getTotalJoined() => this.data.joined;

  public long getTotalMoved() => this.data.moved;

  public long getTotalMigrated() => this.data.migrated;

  public override bool isReadyForRemoval()
  {
    return this.buildings.Count <= 0 && this.getPopulationTotal() <= 0 && !this.hasCities() && !World.world.projectiles.hasActiveProjectiles(this) && base.isReadyForRemoval();
  }

  public bool hasBuildings() => this.buildings.Count > 0;

  public void addBuildings(List<Building> pListBuildings)
  {
    this.buildings.AddRange((IEnumerable<Building>) pListBuildings);
  }

  public void listCity(City pCity) => this.cities.Add(pCity);

  public void listBuilding(Building pBuilding) => this.buildings.Add(pBuilding);

  public Subspecies getMainSubspecies()
  {
    if (this.hasKing())
      return this.king.subspecies;
    return this.units.Count == 0 ? (Subspecies) null : this.units[0].subspecies;
  }

  public void createWildKingdom()
  {
    this.asset.default_kingdom_color.initColor();
    this.wild = true;
  }

  public void createAI()
  {
    if (!Globals.AI_TEST_ACTIVE)
      return;
    if (this.ai == null)
      this.ai = new AiSystemKingdom(this);
    this.ai.next_job_delegate = new GetNextJobID(this.getNextJob);
    this.ai.jobs_library = (AssetLibrary<KingdomJob>) AssetManager.job_kingdom;
    this.ai.task_library = (AssetLibrary<BehaviourTaskKingdom>) AssetManager.tasks_kingdom;
  }

  public bool isOpinionTowardsKingdomGood(Kingdom pKingdom)
  {
    return this == pKingdom || World.world.diplomacy.getOpinion(this, pKingdom).total >= 0;
  }

  public string getNextJob() => "kingdom";

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isCiv() => this.asset.civ;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isMobs() => this.asset.mobs;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isNeutral() => this.asset.neutral;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isNature() => this.asset.nature;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isNomads() => this.asset.nomads;

  public override void save()
  {
    base.save();
    if (this.hasCulture())
      this.data.id_culture = this.culture.id;
    if (this.hasReligion())
      this.data.id_religion = this.religion.id;
    if (this.hasLanguage())
      this.data.id_language = this.language.id;
    if (this.hasKing())
      this.data.kingID = this.king.data.id;
    else
      this.data.kingID = -1L;
    this.data.saved_traits = this.getTraitsAsStrings();
  }

  public IEnumerable<War> getWars(bool pRandom = false) => World.world.wars.getWars(this, pRandom);

  public bool isAttacker()
  {
    foreach (War war in this.getWars())
    {
      if (war.isAttacker(this))
        return true;
    }
    return false;
  }

  public bool isDefender()
  {
    foreach (War war in this.getWars())
    {
      if (war.isDefender(this))
        return true;
    }
    return false;
  }

  public bool isInWarWith(Kingdom pKingdom) => World.world.wars.isInWarWith(this, pKingdom);

  public bool isInWarOnSameSide(Kingdom pKingdom)
  {
    foreach (War war in this.getWars())
    {
      if (war.onTheSameSide(pKingdom, this))
        return true;
    }
    return false;
  }

  public bool isEnemy(Kingdom pKingdomTarget)
  {
    if (pKingdomTarget == null)
      return true;
    long hash = Kingdom.cache_enemy_check.getHash(this, pKingdomTarget);
    bool flag;
    if (Kingdom.cache_enemy_check.dict.TryGetValue(hash, out flag))
      return flag;
    if (this.isCiv() && pKingdomTarget.isCiv())
    {
      if (pKingdomTarget == this)
      {
        Kingdom.cache_enemy_check.dict[hash] = false;
        return false;
      }
      if (World.world.wars.isInWarWith(this, pKingdomTarget))
      {
        Kingdom.cache_enemy_check.dict[hash] = true;
        return true;
      }
      Kingdom.cache_enemy_check.dict[hash] = false;
      return false;
    }
    if (this.asset.isFoe(pKingdomTarget.asset))
    {
      Kingdom.cache_enemy_check.dict[hash] = true;
      return true;
    }
    Kingdom.cache_enemy_check.dict[hash] = false;
    return false;
  }

  public bool isGettingCaptured()
  {
    foreach (City city in this.getCities())
    {
      if (city.isGettingCaptured())
        return true;
    }
    return false;
  }

  [Obsolete("use .getColor() instead", false)]
  public ColorAsset kingdomColor => this.getColor();

  public override ColorAsset getColor()
  {
    return this.isCiv() ? base.getColor() : this.asset.default_kingdom_color;
  }

  internal void newCivKingdom(Actor pActor)
  {
    this.asset = AssetManager.kingdoms.get(pActor.asset.kingdom_id_civilization);
    this.data.original_actor_asset = pActor.asset.id;
    this.setName(pActor.generateName(MetaType.Kingdom, this.getID()));
    KingdomData data = this.data;
    Culture culture = this.culture;
    long num = culture != null ? culture.id : -1L;
    data.name_culture_id = num;
    this.generateNewMetaObject();
  }

  public override ActorAsset getActorAsset()
  {
    return this.hasKing() ? this.king.getActorAsset() : this.getFounderSpecies();
  }

  public ActorAsset getFounderSpecies()
  {
    return AssetManager.actor_library.get(this.data.original_actor_asset);
  }

  public string getSpecies()
  {
    if (string.IsNullOrEmpty(this.data.original_actor_asset))
      return (string) null;
    return this.getActorAsset()?.id;
  }

  public void trySetRoyalClan()
  {
    if (!this.hasKing() || !this.king.hasClan() || this.king.clan.id == this.data.royal_clan_id)
      return;
    Clan pOldClan = World.world.clans.get(this.data.royal_clan_id);
    if (pOldClan != null && pOldClan.isAlive())
      this.logNewRoyalClanChanged(pOldClan, this.king.clan);
    else if (this.king.clan.getRenown() >= 10)
      this.logNewRoyalClan(this.king.clan);
    this.data.royal_clan_id = this.king.clan.id;
  }

  public void checkEndWar() => this.data.timestamp_last_war = World.world.getCurWorldTime();

  public void madePeace(War pWar)
  {
    int pAmount = (int) ((double) pWar.getRenown() * 0.25);
    this.addRenown(pAmount);
    foreach (Actor unit in this.getUnits())
      unit.madePeace(pWar);
    if (!this.hasAlliance())
      return;
    this.getAlliance().addRenown(pAmount);
  }

  public void wonWar(War pWar)
  {
    this.addRenown(pWar.getRenown());
    foreach (Actor unit in this.getUnits())
      unit.warWon(pWar);
    if (!this.hasAlliance())
      return;
    this.getAlliance().addRenown(pWar.getRenown());
  }

  public void lostWar(War pWar)
  {
    int pAmount = (int) ((double) pWar.getRenown() * 0.10000000149011612);
    this.addRenown(pAmount);
    foreach (Actor unit in this.getUnits())
      unit.warLost(pWar);
    if (!this.hasAlliance())
      return;
    this.getAlliance().addRenown(pAmount);
  }

  internal void updateCiv(float pElapsed)
  {
    if ((double) this.data.timer_new_king > 0.0)
      this.data.timer_new_king -= pElapsed;
    if (this.ai == null)
      return;
    if ((double) this.timer_action > 0.0)
      this.timer_action -= pElapsed;
    else
      this.ai.update();
  }

  public void setCapital(City pCity)
  {
    this.capital = pCity;
    if (this.capital != null && this.capital.isAlive())
    {
      this.data.capitalID = this.data.last_capital_id = pCity.data.id;
      this.location = Vector2.op_Implicit(this.capital.city_center);
    }
    else
      this.data.capitalID = -1L;
  }

  public void setKing(Actor pActor, bool pFromLoad = false)
  {
    this.king = pActor;
    this.king.setProfession(UnitProfession.King);
    if (!pFromLoad)
    {
      ++this.data.total_kings;
      this.addRuler(pActor);
      this.data.timestamp_king_rule = World.world.getCurWorldTime();
      this.king.changeHappiness("become_king");
    }
    this.trySetRoyalClan();
  }

  internal void kingLeftEvent()
  {
    if (!this.hasKing())
      return;
    if (this.king.isAlive())
      this.king.changeHappiness("lost_crown");
    this.logKingLeft(this.king);
    this.removeKing();
  }

  internal void kingFledCity()
  {
    if (!this.hasKing())
      return;
    if (this.king.city.isCapitalCity())
      this.logKingFledCapital(this.king);
    else
      this.logKingFledCity(this.king);
    this.king.setCity((City) null);
  }

  internal void removeKing()
  {
    if (!this.king.isRekt())
      this.king.setProfession(UnitProfession.Unit);
    this.rulerLeft();
    this.king = (Actor) null;
    this.data.timer_new_king = Randy.randomFloat(5f, 20f);
  }

  public void updateRulers()
  {
    if (this.data.past_rulers == null || this.data.past_rulers.Count == 0)
      return;
    foreach (LeaderEntry pastRuler in this.data.past_rulers)
    {
      Actor pObject = World.world.units.get(pastRuler.id);
      if (!pObject.isRekt())
        pastRuler.name = pObject.name;
    }
  }

  public void addRuler(Actor pActor)
  {
    KingdomData data = this.data;
    if (data.past_rulers == null)
      data.past_rulers = new List<LeaderEntry>();
    this.rulerLeft();
    this.data.past_rulers.Add(new LeaderEntry()
    {
      id = pActor.getID(),
      name = pActor.name,
      color_id = this.data.color_id,
      timestamp_ago = World.world.getCurWorldTime()
    });
    if (this.data.past_rulers.Count <= 30)
      return;
    this.data.past_rulers.Shift<LeaderEntry>();
  }

  public void rulerLeft()
  {
    if (this.data.past_rulers == null || this.data.past_rulers.Count == 0)
      return;
    LeaderEntry leaderEntry = this.data.past_rulers.Last<LeaderEntry>();
    if (leaderEntry.timestamp_end >= leaderEntry.timestamp_ago)
      return;
    leaderEntry.timestamp_end = World.world.getCurWorldTime();
    this.updateRulers();
  }

  public void logKingDead(Actor pActor)
  {
    if (!pActor.attackedBy.isRekt() && pActor.attackedBy.isActor())
      WorldLog.logKingMurder(this, pActor, pActor.attackedBy.a);
    else
      WorldLog.logKingDead(this, pActor);
  }

  public void logKingFledCapital(Actor pActor) => WorldLog.logKingFledCapital(this, pActor);

  public void logKingFledCity(Actor pActor) => WorldLog.logKingFledCity(this, pActor);

  public void logKingLeft(Actor pActor) => WorldLog.logKingLeft(this, pActor);

  public void logNewRoyalClanChanged(Clan pOldClan, Clan pNewClan)
  {
    WorldLog.logRoyalClanChanged(this, pOldClan, pNewClan);
  }

  public void logNewRoyalClan(Clan pClan) => WorldLog.logRoyalClanNew(this, pClan);

  public void logRoyalClanLost(Clan pClan) => WorldLog.logRoyalClanNoMore(this, pClan);

  internal void checkClearCapital(City pCity)
  {
    if (!pCity.isCapitalCity())
      return;
    this.clearCapital();
  }

  public void clearCapital()
  {
    this.data.capitalID = -1L;
    this.capital = (City) null;
  }

  public bool hasNearbyKingdoms()
  {
    foreach (City city in this.getCities())
    {
      if (city.neighbours_kingdoms.Count > 0)
        return true;
    }
    return false;
  }

  public void capturedFrom(Kingdom pKingdom) => World.world.diplomacy.getRelation(this, pKingdom);

  public virtual string getMotto()
  {
    if (string.IsNullOrEmpty(this.data.motto))
      this.data.motto = NameGenerator.getName("kingdom_mottos");
    return this.data.motto;
  }

  public override void generateBanner()
  {
    BannerAsset bannerAsset = AssetManager.kingdom_banners_library.get(this.getActorAsset().banner_id);
    this.data.banner_icon_id = Randy.randomInt(0, bannerAsset.icons.Count);
    this.data.banner_background_id = Randy.randomInt(0, bannerAsset.backgrounds.Count);
  }

  public override void loadData(KingdomData pData)
  {
    base.loadData(pData);
    if (this.data.id_culture.hasValue())
      this.setCulture(World.world.cultures.get(this.data.id_culture));
    if (this.data.id_language.hasValue())
      this.setLanguage(World.world.languages.get(this.data.id_language));
    if (this.data.id_religion.hasValue())
      this.setReligion(World.world.religions.get(this.data.id_religion));
    ActorAsset actorAsset = this.getActorAsset();
    this.asset = AssetManager.kingdoms.get(actorAsset.kingdom_id_civilization);
  }

  internal void load2()
  {
    City pCity = World.world.cities.get(this.data.capitalID);
    if (pCity != null)
      this.setCapital(pCity);
    if (!this.data.kingID.hasValue())
      return;
    Actor pActor = World.world.units.get(this.data.kingID);
    if (pActor == null)
      return;
    this.setKing(pActor, true);
    pActor.setProfession(UnitProfession.King);
  }

  public override bool updateColor(ColorAsset pColor)
  {
    bool flag = base.updateColor(pColor);
    if (flag)
    {
      foreach (Building building in this.buildings)
        building.updateKingdomColors();
    }
    return flag;
  }

  public static float distanceBetweenKingdom(Kingdom pKingdom, Kingdom pTarget)
  {
    // ISSUE: unable to decompile the method.
  }

  public override IEnumerable<City> getCities()
  {
    Kingdom kingdom = this;
    if (World.world.kingdoms.hasDirtyCities())
    {
      foreach (City city in (CoreSystemManager<City, CityData>) World.world.cities)
      {
        if (!city.isRekt() && city.kingdom == kingdom)
          yield return city;
      }
    }
    else
    {
      foreach (City city in kingdom.cities)
      {
        if (!city.isRekt())
          yield return city;
      }
    }
  }

  public void clear()
  {
    this.buildings.Clear();
    this.cities.Clear();
    this.units.Clear();
    Kingdom.cache_enemy_check.clear();
    this.clearCapital();
  }

  public override void Dispose()
  {
    DBInserter.deleteData(this.getID(), "kingdom");
    this.clear();
    this.asset = (KingdomAsset) null;
    this.king = (Actor) null;
    this.capital = (City) null;
    this.culture = (Culture) null;
    this.language = (Language) null;
    this.religion = (Religion) null;
    this.ai?.reset();
    base.Dispose();
  }

  public bool hasEnemies() => World.world.wars.hasWars(this);

  public ListPool<Kingdom> getEnemiesKingdoms() => World.world.wars.getEnemiesOf(this);

  public void makeSurvivorsToNomads()
  {
    if (this.units.Count == 0)
      return;
    for (int index = 0; index < this.units.Count; ++index)
    {
      Actor unit = this.units[index];
      if (unit.isAlive())
      {
        if (unit.asset.is_boat)
        {
          unit.getHitFullHealth(AttackType.None);
        }
        else
        {
          unit.cancelAllBeh();
          unit.removeFromPreviousFaction();
          unit.joinKingdom(World.world.kingdoms_wild.get(unit.asset.kingdom_id_wild));
        }
      }
    }
    this.units.Clear();
  }

  public void clearKingData() => this.king = (Actor) null;

  public void updateAge()
  {
    if (!this.hasKing() || !this.king.hasClan())
      return;
    this.king.clan.addRenown(1);
  }

  public override int countCouples()
  {
    int num = 0;
    foreach (City city in this.getCities())
      num += city.countCouples();
    return num;
  }

  public override int countSingleMales()
  {
    int num = 0;
    foreach (City city in this.getCities())
      num += city.countSingleMales();
    return num;
  }

  public override int countSingleFemales()
  {
    int num = 0;
    foreach (City city in this.getCities())
      num += city.countSingleFemales();
    return num;
  }

  public int countZones()
  {
    int num = 0;
    foreach (City city in this.getCities())
      num += city.countZones();
    return num;
  }

  public int countBuildings()
  {
    int num = 0;
    foreach (City city in this.getCities())
      num += city.countBuildings();
    return num;
  }

  public int countCities()
  {
    if (!World.world.kingdoms.hasDirtyCities())
      return this.cities.Count;
    int num = 0;
    foreach (City city in this.getCities())
      ++num;
    return num;
  }

  public override int getPopulationPeople()
  {
    if (!this._has_boats)
      return this.units.Count;
    int populationPeople1 = 0;
    int num = 0;
    foreach (City city in this.getCities())
    {
      populationPeople1 += city.getPopulationPeople();
      num += city.countBoats();
    }
    if (populationPeople1 + num == this.units.Count)
      return populationPeople1;
    int populationPeople2 = 0;
    foreach (Actor unit in this.getUnits())
    {
      if (!unit.asset.is_boat)
        ++populationPeople2;
    }
    return populationPeople2;
  }

  public override int countUnits() => this.getPopulationPeople();

  public override IEnumerable<Actor> getUnits()
  {
    Kingdom kingdom = this;
    foreach (Actor unit in kingdom.units)
    {
      if (unit.isAlive() && !unit.asset.is_boat && unit.kingdom == kingdom)
        yield return unit;
    }
  }

  public override Actor getRandomUnit()
  {
    foreach (Actor randomUnit in this.units.LoopRandom<Actor>())
    {
      if (randomUnit.isAlive() && !randomUnit.asset.is_boat && randomUnit.kingdom == this)
        return randomUnit;
    }
    return (Actor) null;
  }

  public int getPopulationTotal() => this.units.Count;

  public int countBoats()
  {
    int num = 0;
    foreach (City city in this.getCities())
      num += city.countBoats();
    return num;
  }

  public int getPopulationTotalPossible()
  {
    int populationTotalPossible = 0;
    foreach (City city in this.getCities())
      populationTotalPossible += city.getPopulationMaximum();
    return populationTotalPossible;
  }

  public int countWeapons()
  {
    int num = 0;
    foreach (City city in this.getCities())
      num += city.countWeapons();
    return num;
  }

  public int countTotalFood()
  {
    int num = 0;
    foreach (City city in this.getCities())
      num += city.getTotalFood();
    return num;
  }

  public int countTotalWarriors()
  {
    int num = 0;
    foreach (City city in this.getCities())
      num += city.countWarriors();
    return num;
  }

  public int countWarriorsMax()
  {
    int num = 0;
    foreach (City city in this.getCities())
      num += city.getMaxWarriors();
    return num;
  }

  public int getMaxCities()
  {
    int maxCities = this.getActorAsset().civ_base_cities;
    if (this.hasKing())
      maxCities += (int) this.king.stats["cities"];
    if (maxCities < 1)
      maxCities = 1;
    return maxCities;
  }

  public bool diceAgressionSuccess()
  {
    if (!this.hasKing())
      return false;
    int num = this.countCities();
    return num < this.getMaxCities() || num >= this.getMaxCities() && Randy.randomChance(this.king.stats["personality_aggression"]);
  }

  public bool isSupreme() => DiplomacyManager.kingdom_supreme == this;

  public bool isSecondBest() => DiplomacyManager.kingdom_second == this;

  public bool hasAlliance() => this.getAlliance() != null;

  public Alliance getAlliance()
  {
    if (!this.data.allianceID.hasValue())
      return (Alliance) null;
    Alliance alliance = World.world.alliances.get(this.data.allianceID);
    if (alliance != null)
      return alliance;
    this.data.allianceID = -1L;
    return alliance;
  }

  public void allianceLeave(Alliance pAlliance)
  {
    this.data.allianceID = -1L;
    this.data.timestamp_alliance = World.world.getCurWorldTime();
  }

  public void allianceJoin(Alliance pAlliance)
  {
    this.data.allianceID = pAlliance.data.id;
    this.data.timestamp_alliance = World.world.getCurWorldTime();
  }

  public void calculateNeighbourCities()
  {
    foreach (City city in this.getCities())
      city.recalculateNeighbourCities();
  }

  public Culture getCulture() => this.culture;

  public void setCulture(Culture pCulture)
  {
    if (this.culture == pCulture)
      return;
    this.culture = pCulture;
    World.world.cultures.setDirtyKingdoms();
  }

  public bool hasCulture()
  {
    if (this.culture != null && !this.culture.isAlive())
      this.setCulture((Culture) null);
    return this.culture != null;
  }

  public void setLanguage(Language pLanguage)
  {
    this.language = pLanguage;
    World.world.languages.setDirtyKingdoms();
  }

  public Language getLanguage() => this.language;

  public bool hasLanguage()
  {
    if (this.language != null && !this.language.isAlive())
      this.setLanguage((Language) null);
    return this.language != null;
  }

  public void setReligion(Religion pReligion)
  {
    if (this.religion == pReligion)
      return;
    this.religion = pReligion;
    World.world.religions.setDirtyKingdoms();
  }

  public Religion getReligion() => this.religion;

  public bool hasReligion()
  {
    if (this.religion != null && !this.religion.isAlive())
      this.setReligion((Religion) null);
    return this.religion != null;
  }

  public bool isEnemyAroundZone(TileZone pZone)
  {
    foreach (TileZone neighbour in pZone.neighbours)
    {
      if (neighbour.city == null)
        return true;
      Kingdom kingdom = neighbour.city.kingdom;
      if (kingdom != this || kingdom != this && kingdom.isEnemy(this))
        return true;
    }
    return false;
  }

  public override bool hasCities()
  {
    using (IEnumerator<City> enumerator = this.getCities().GetEnumerator())
    {
      if (enumerator.MoveNext())
      {
        City current = enumerator.Current;
        return true;
      }
    }
    return false;
  }

  public bool hasCapital() => this.capital != null;

  public bool hasKing()
  {
    if (this.king == null)
      return false;
    if (this.king.isAlive())
      return true;
    this.removeKing();
    return false;
  }

  public void affectKingByPowers()
  {
    if (!this.hasKing())
      return;
    this.king.addStatusEffect("voices_in_my_head");
  }

  public int countUnhappyCities()
  {
    int num = 0;
    foreach (City city in this.getCities())
    {
      if (!city.isHappy())
        ++num;
    }
    return num;
  }

  public Sprite getSpeciesIcon() => this.getActorAsset().getSpriteIcon();

  public Sprite getElementIcon()
  {
    return AssetManager.kingdom_banners_library.getSpriteIcon(this.data.banner_icon_id, this.getActorAsset().banner_id);
  }

  public Sprite getElementBackground()
  {
    return AssetManager.kingdom_banners_library.getSpriteBackground(this.data.banner_background_id, this.getActorAsset().banner_id);
  }

  public void increaseHappinessFromNewCityCapture()
  {
    foreach (Actor unit in this.getUnits())
    {
      if (!unit.hasHappinessEntry("was_conquered", 400f))
        unit.changeHappiness("conquered_city");
    }
  }

  public void increaseHappinessFromDestroyingCity()
  {
    foreach (Actor unit in this.getUnits())
    {
      if (!unit.hasHappinessEntry("was_conquered", 400f))
        unit.changeHappiness("destroyed_city");
    }
  }

  public void decreaseHappinessFromLostCityCapture(City pCity)
  {
    foreach (Actor unit in this.units)
    {
      if (!unit.hasHappinessEntry("was_conquered", 400f))
      {
        if (pCity.isCapitalCity())
          unit.changeHappiness("lost_capital");
        else
          unit.changeHappiness("lost_city");
      }
    }
  }

  public void decreaseHappinessFromRazedCity(City pCity)
  {
    foreach (Actor unit in this.units)
    {
      if (!unit.hasHappinessEntry("was_conquered", 400f))
      {
        if (pCity.isCapitalCity())
          unit.changeHappiness("razed_capital");
        else
          unit.changeHappiness("razed_city");
      }
    }
  }

  public int getLootMin() => 5;

  public float getTaxRateTribute() => this._cached_tax_tribute;

  public float getTaxRateLocal() => this._cached_tax_local;

  public void copyMetasFromOtherKingdom(Kingdom pKingdom)
  {
    if (pKingdom.hasCulture())
      this.setCulture(pKingdom.culture);
    if (pKingdom.hasLanguage())
      this.setLanguage(pKingdom.language);
    if (!pKingdom.hasReligion())
      return;
    this.setReligion(pKingdom.religion);
  }

  public void setUnitMetas(Actor pActor)
  {
    if (pActor.hasCulture())
      this.setCulture(pActor.culture);
    if (pActor.hasLanguage())
      this.setLanguage(pActor.language);
    if (!pActor.hasReligion())
      return;
    this.setReligion(pActor.religion);
  }

  public void setCityMetas(City pCity)
  {
    if (pCity.hasCulture())
      this.setCulture(pCity.culture);
    if (pCity.hasLanguage())
      this.setLanguage(pCity.language);
    if (!pCity.hasReligion())
      return;
    this.setReligion(pCity.religion);
  }

  public Clan getKingClan() => this.hasKing() && this.king.hasClan() ? this.king.clan : (Clan) null;

  public override void listUnit(Actor pActor)
  {
    if (pActor.asset.is_boat)
      this._has_boats = true;
    base.listUnit(pActor);
  }

  internal override void clearListUnits()
  {
    this._has_boats = false;
    base.clearListUnits();
  }

  public override string ToString()
  {
    if (this.data == null)
      return "[Kingdom is null]";
    using (StringBuilderPool stringBuilderPool = new StringBuilderPool())
    {
      stringBuilderPool.Append($"[Kingdom:{this.id} ");
      if (!this.isAlive())
        stringBuilderPool.Append("[DEAD] ");
      stringBuilderPool.Append($"\"{this.name}\" ");
      stringBuilderPool.Append($"Cities:{this.cities.Count} ");
      if (World.world.kingdoms.hasDirtyCities())
        stringBuilderPool.Append($" [Dirty:{this.countCities()}] ");
      stringBuilderPool.Append($"Units:{this.units.Count} ");
      if (this.isDirtyUnits())
        stringBuilderPool.Append("[Dirty] ");
      if (this.hasKing())
        stringBuilderPool.Append($"King:{this.king.id} ");
      return stringBuilderPool.ToString().Trim() + "]";
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Clan
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using db;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Clan : MetaObjectWithTraits<ClanData, ClanTrait>
{
  public BaseStats base_stats_male = new BaseStats();
  public BaseStats base_stats_female = new BaseStats();

  protected override MetaType meta_type => MetaType.Clan;

  protected override bool track_death_types => true;

  public override BaseSystemManager manager => (BaseSystemManager) World.world.clans;

  public void newClan(Actor pFounder, bool pAddDefaultTraits)
  {
    this.data.original_actor_asset = pFounder.asset.id;
    this.generateNewMetaObject(pAddDefaultTraits);
    if (pFounder.kingdom.isCiv())
    {
      this.data.founder_kingdom_name = pFounder.kingdom.data.name;
      this.data.founder_kingdom_id = pFounder.kingdom.getID();
    }
    this.data.founder_actor_name = pFounder.getName();
    this.data.founder_actor_id = pFounder.getID();
    this.data.founder_city_name = pFounder.city?.name;
    ClanData data1 = this.data;
    City city = pFounder.city;
    long num1 = city != null ? city.getID() : -1L;
    data1.founder_city_id = num1;
    this.data.creator_subspecies_name = pFounder.subspecies.name;
    this.data.creator_subspecies_id = pFounder.subspecies.getID();
    this.data.creator_species_id = pFounder.asset.id;
    string name = pFounder.generateName(MetaType.Clan, this.getID());
    ClanData data2 = this.data;
    Culture culture = pFounder.culture;
    long num2 = culture != null ? culture.getID() : -1L;
    data2.name_culture_id = num2;
    this.setName(name);
  }

  protected override void recalcBaseStats()
  {
    base.recalcBaseStats();
    this.base_stats_female.clear();
    this.base_stats_male.clear();
    foreach (ClanTrait trait in (IEnumerable<ClanTrait>) this.getTraits())
    {
      this.base_stats_male.mergeStats(trait.base_stats_male);
      this.base_stats_female.mergeStats(trait.base_stats_female);
    }
  }

  public override void increaseBirths()
  {
    base.increaseBirths();
    this.addRenown(1);
  }

  protected override AssetLibrary<ClanTrait> trait_library
  {
    get => (AssetLibrary<ClanTrait>) AssetManager.clan_traits;
  }

  protected override List<string> default_traits => this.getActorAsset().default_clan_traits;

  protected override List<string> saved_traits => this.data.saved_traits;

  protected override string species_id => this.data.original_actor_asset;

  protected sealed override void setDefaultValues() => base.setDefaultValues();

  public override void listUnit(Actor pActor) => base.listUnit(pActor);

  protected override ColorLibrary getColorLibrary()
  {
    return (ColorLibrary) AssetManager.clan_colors_library;
  }

  public override void generateBanner()
  {
    this.data.banner_background_id = AssetManager.clan_banners_library.getNewIndexBackground();
    this.data.banner_icon_id = AssetManager.clan_banners_library.getNewIndexIcon();
  }

  public string getMotto()
  {
    if (string.IsNullOrEmpty(this.data.motto))
      this.data.motto = NameGenerator.getName("clan_mottos");
    return this.data.motto;
  }

  public override void save()
  {
    base.save();
    this.data.saved_traits = this.getTraitsAsStrings();
  }

  public void checkMembersForNewChief()
  {
    if (this.units.Count == 0 || !this.getChief().isRekt())
      return;
    this.setChief((Actor) null);
    Actor nextChief = this.getNextChief();
    if (nextChief == null)
      return;
    this.setChief(nextChief);
  }

  public void setChief(Actor pActor)
  {
    if (this.data == null)
      return;
    if (pActor.isRekt())
    {
      this.data.chief_id = -1L;
      this.chiefLeft();
    }
    else
    {
      this.data.chief_id = pActor.getID();
      this.addChief(pActor);
    }
  }

  public void updateChiefs()
  {
    if (this.data.past_chiefs == null || this.data.past_chiefs.Count == 0)
      return;
    foreach (LeaderEntry pastChief in this.data.past_chiefs)
    {
      Actor pObject = World.world.units.get(pastChief.id);
      if (!pObject.isRekt())
        pastChief.name = pObject.name;
    }
  }

  public void addChief(Actor pActor)
  {
    ClanData data = this.data;
    if (data.past_chiefs == null)
      data.past_chiefs = new List<LeaderEntry>();
    this.chiefLeft();
    this.data.past_chiefs.Add(new LeaderEntry()
    {
      id = pActor.getID(),
      name = pActor.name,
      color_id = this.data.color_id,
      timestamp_ago = World.world.getCurWorldTime()
    });
    if (this.data.past_chiefs.Count <= 30)
      return;
    this.data.past_chiefs.Shift<LeaderEntry>();
  }

  public void chiefLeft()
  {
    if (this.data.past_chiefs == null || this.data.past_chiefs.Count == 0)
      return;
    LeaderEntry leaderEntry = this.data.past_chiefs.Last<LeaderEntry>();
    if (leaderEntry.timestamp_end >= leaderEntry.timestamp_ago)
      return;
    leaderEntry.timestamp_end = World.world.getCurWorldTime();
    this.updateChiefs();
  }

  public void tryForgetChief(Actor pActor)
  {
    if (this.data.chief_id != pActor.getID())
      return;
    this.setChief((Actor) null);
  }

  public Culture getClanCulture()
  {
    Culture chiefCulture = this.getChiefCulture();
    if (chiefCulture == null && this.data.culture_id.hasValue())
      chiefCulture = World.world.cultures.get(this.data.culture_id);
    if (chiefCulture == null)
      this.data.culture_id = -1L;
    else
      this.data.culture_id = chiefCulture.getID();
    return chiefCulture;
  }

  private Culture getChiefCulture() => this.getChief()?.culture;

  public Actor getNextChief(Actor pIgnore = null)
  {
    // ISSUE: unable to decompile the method.
  }

  public int getMaxMembers() => (int) this.base_stats_meta["limit_clan_members"];

  public Actor getChief()
  {
    Actor chief = (Actor) null;
    if (this.data.chief_id.hasValue())
      chief = World.world.units.get(this.data.chief_id);
    return chief;
  }

  public bool hasChief() => this.getChief() != null;

  public bool isFull()
  {
    int maxMembers = this.getMaxMembers();
    return maxMembers != 0 && this.units.Count >= maxMembers;
  }

  public bool fitToRule(Actor pActor, Kingdom pKingdom)
  {
    return pActor.kingdom == pKingdom && pActor.isUnitFitToRule() && !pActor.isKing();
  }

  public Sprite getBackgroundSprite()
  {
    return AssetManager.clan_banners_library.getSpriteBackground(this.data.banner_background_id);
  }

  public Sprite getIconSprite()
  {
    return AssetManager.clan_banners_library.getSpriteIcon(this.data.banner_icon_id);
  }

  public override void Dispose()
  {
    DBInserter.deleteData(this.getID(), "clan");
    base.Dispose();
  }

  public string getTextMaxMembers()
  {
    int maxMembers = this.getMaxMembers();
    return maxMembers == 0 ? this.units.Count.ToString() : $"{this.units.Count}/{maxMembers}";
  }

  public override bool hasCities()
  {
    foreach (City city in (CoreSystemManager<City, CityData>) World.world.cities)
    {
      if (city.getRoyalClan() == this)
        return true;
    }
    return false;
  }

  public override IEnumerable<City> getCities()
  {
    Clan clan = this;
    foreach (City city in (CoreSystemManager<City, CityData>) World.world.cities)
    {
      if (city.getRoyalClan() == clan)
        yield return city;
    }
  }

  public override bool hasKingdoms()
  {
    foreach (Kingdom kingdom in (CoreSystemManager<Kingdom, KingdomData>) World.world.kingdoms)
    {
      if (kingdom.getKingClan() == this)
        return true;
    }
    return false;
  }

  public override IEnumerable<Kingdom> getKingdoms()
  {
    Clan clan = this;
    foreach (Kingdom kingdom in (CoreSystemManager<Kingdom, KingdomData>) World.world.kingdoms)
    {
      if (kingdom.getKingClan() == clan)
        yield return kingdom;
    }
  }
}

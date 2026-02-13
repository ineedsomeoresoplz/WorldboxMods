// Decompiled with JetBrains decompiler
// Type: Family
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using db;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Family : MetaObject<FamilyData>, ISapient
{
  private Actor _cached_alpha;
  private ActorAsset _cached_species;
  private Actor _founder_1;
  private Actor _founder_2;
  private bool _founders_dirty;
  private double _timestamp_hungry_check;
  private bool _cached_hungry_check_result;

  protected override MetaType meta_type => MetaType.Family;

  public override BaseSystemManager manager => (BaseSystemManager) World.world.families;

  public void newFamily(Actor pActor1, Actor pActor2, WorldTile pTile)
  {
    this.data.species_id = pActor1.asset.id;
    this.generateNewMetaObject();
    this.data.founder_actor_name_1 = pActor1.getName();
    this.data.founder_actor_name_2 = pActor2?.getName();
    if (pActor1.hasSubspecies())
    {
      this.data.subspecies_id = pActor1.subspecies.id;
      this.data.subspecies_name = pActor1.subspecies.name;
    }
    FamilyData data = this.data;
    City city = pActor1.city;
    long num = city != null ? city.getID() : -1L;
    data.founder_city_id = num;
    this.data.founder_city_name = pActor1.city?.name;
    if (pActor1.kingdom.isCiv())
    {
      this.data.founder_kingdom_id = pActor1.kingdom.getID();
      this.data.founder_kingdom_name = pActor1.kingdom.data.name;
    }
    this.data.main_founder_id_1 = pActor1.getID();
    if (pActor2 != null)
      this.data.main_founder_id_2 = pActor2.getID();
    this.generateName(pActor1);
  }

  public bool areMostUnitsHungry()
  {
    if ((double) World.world.getWorldTimeElapsedSince(this._timestamp_hungry_check) < 5.0)
      return this._cached_hungry_check_result;
    this._cached_hungry_check_result = this.countHungry() >= this.countUnits() / 2;
    this._timestamp_hungry_check = World.world.getCurWorldTime();
    return this._cached_hungry_check_result;
  }

  public bool isFull() => this.units.Count > this.getActorAsset().family_limit;

  public bool isAlpha(Actor pActor)
  {
    Actor cachedAlpha = this._cached_alpha;
    return pActor == cachedAlpha;
  }

  private void removeAlpha()
  {
    this.data.alpha_id = -1L;
    this._cached_alpha = (Actor) null;
  }

  public void checkAlpha()
  {
    if (this._cached_alpha != null)
    {
      if (!this._cached_alpha.isAlive())
        this.removeAlpha();
      else if (this._cached_alpha.family != this)
        this.removeAlpha();
    }
    if (this._cached_alpha != null)
      return;
    Actor alpha = this.findAlpha();
    if (alpha == null)
      return;
    this.setAlpha(alpha, true);
  }

  public Actor getAlpha() => this._cached_alpha;

  public Actor findAlpha()
  {
    Actor alpha = (Actor) null;
    double num = double.MaxValue;
    for (int index = 0; index < this.units.Count; ++index)
    {
      Actor unit = this.units[index];
      if (unit.isAlive() && unit.data.created_time <= num)
      {
        num = unit.data.created_time;
        alpha = unit;
      }
    }
    return alpha;
  }

  private void setAlpha(Actor pActor, bool pNew)
  {
    this.data.alpha_id = pActor.getID();
    if (pNew)
      pActor.changeHappiness("become_alpha");
    this._cached_alpha = pActor;
  }

  public void saveOriginFamily1(long pFamilyID) => this.data.original_family_1 = pFamilyID;

  public void saveOriginFamily2(long pFamilyID)
  {
    if (pFamilyID == this.data.original_family_1)
      return;
    this.data.original_family_2 = pFamilyID;
  }

  public IEnumerable<Family> getOriginFamilies()
  {
    Family family = this;
    if (family.data.original_family_1.hasValue())
    {
      Family pObject = World.world.families.get(family.data.original_family_1);
      if (!pObject.isRekt())
        yield return pObject;
    }
    if (family.data.original_family_2.hasValue())
    {
      Family pObject = World.world.families.get(family.data.original_family_2);
      if (!pObject.isRekt())
        yield return pObject;
    }
  }

  public override void generateBanner()
  {
    this.data.banner_background_id = AssetManager.family_banners_library.getNewIndexBackground();
    ActorAsset actorAsset = this.getActorAsset();
    int num;
    if (actorAsset.family_banner_frame_only_inclusion)
    {
      num = AssetManager.family_banners_library.main.frames.IndexOf(actorAsset.family_banner_frame_generation_inclusion);
    }
    else
    {
      using (ListPool<string> list = new ListPool<string>((ICollection<string>) AssetManager.family_banners_library.main.frames))
      {
        string generationExclusion1 = actorAsset.family_banner_frame_generation_exclusion;
        if (!string.IsNullOrEmpty(generationExclusion1))
          list.Remove(generationExclusion1);
        string generationExclusion2 = actorAsset.family_banner_frame_generation_exclusion;
        if (!string.IsNullOrEmpty(generationExclusion2))
          list.Remove(generationExclusion2);
        string random = list.GetRandom<string>();
        num = AssetManager.family_banners_library.main.frames.IndexOf(random);
      }
    }
    this.data.banner_frame_id = num;
  }

  public Sprite getSpriteBackground()
  {
    return AssetManager.family_banners_library.getSpriteBackground(this.data.banner_background_id);
  }

  public Sprite getSpriteFrame()
  {
    return AssetManager.family_banners_library.getSpriteFrame(this.data.banner_frame_id);
  }

  public override ActorAsset getActorAsset()
  {
    if (this._cached_species == null)
      this._cached_species = AssetManager.actor_library.get(this.data.species_id);
    return this._cached_species;
  }

  public bool isSapient()
  {
    Subspecies subspecies = World.world.subspecies.get(this.data.subspecies_id);
    return subspecies != null && subspecies.isSapient();
  }

  public bool isMainFounder(Actor pActor)
  {
    return pActor.data.id == this.data.main_founder_id_1 || pActor.data.id == this.data.main_founder_id_2;
  }

  public bool hasFounders()
  {
    this.checkFounders();
    return this._founder_1 != null || this._founder_2 != null;
  }

  public Actor getRandomFounder()
  {
    this.checkFounders();
    if (!this.hasFounders())
      return (Actor) null;
    return this._founder_1 != null && this._founder_2 == null || (this._founder_2 == null || this._founder_1 != null) && Randy.randomBool() ? this._founder_1 : this._founder_2;
  }

  public Actor getFounderFirst() => this._founder_1;

  public Actor getFounderSecond() => this._founder_2;

  private void checkFounders()
  {
    if (!this._founders_dirty)
      return;
    this.clearFounders();
    this._founders_dirty = false;
    foreach (Actor unit in this.units)
    {
      if (unit.data.id == this.data.main_founder_id_1)
        this._founder_1 = unit;
      if (unit.data.id == this.data.main_founder_id_2)
        this._founder_2 = unit;
    }
  }

  private void clearFounders()
  {
    this._founder_1 = (Actor) null;
    this._founder_2 = (Actor) null;
    this._founders_dirty = true;
  }

  protected override ColorLibrary getColorLibrary()
  {
    return (ColorLibrary) AssetManager.families_colors_library;
  }

  public bool isSameSpecies(string pAssetID) => this.data.species_id == pAssetID;

  private void generateName(Actor pActor)
  {
    this.setName(pActor.generateName(MetaType.Family, this.getID()));
    FamilyData data = this.data;
    Culture culture = pActor.culture;
    long num = culture != null ? culture.getID() : -1L;
    data.name_culture_id = num;
  }

  public override void loadData(FamilyData pData)
  {
    base.loadData(pData);
    if (!this.data.alpha_id.hasValue())
      return;
    Actor pActor = World.world.units.get(this.data.alpha_id);
    if (pActor == null)
      return;
    this.setAlpha(pActor, false);
  }

  public override void updateDirty()
  {
    base.updateDirty();
    this.clearFounders();
  }

  public override void Dispose()
  {
    DBInserter.deleteData(this.getID(), "family");
    this._cached_alpha = (Actor) null;
    this._cached_species = (ActorAsset) null;
    this._timestamp_hungry_check = 0.0;
    this._cached_hungry_check_result = false;
    this._founder_1 = (Actor) null;
    this._founder_2 = (Actor) null;
    this._founders_dirty = true;
    base.Dispose();
  }
}

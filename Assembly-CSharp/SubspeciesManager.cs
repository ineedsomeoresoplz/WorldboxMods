// Decompiled with JetBrains decompiler
// Type: SubspeciesManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SubspeciesManager : MetaSystemManager<Subspecies, SubspeciesData>
{
  public static readonly string[] NAME_ENDINGS = new string[9]
  {
    "us",
    "as",
    "os",
    "is",
    "es",
    "um",
    "ys",
    "bres",
    "bros"
  };
  private const float UNSTABLE_GENOME_INTERVAL = 300f;
  private float _timer_unstable_genome;

  public SubspeciesManager() => this.type_id = "subspecies";

  public Subspecies newSpecies(ActorAsset pAsset, WorldTile pTile, bool pMutation = false)
  {
    ++World.world.game_stats.data.subspeciesCreated;
    ++World.world.map_stats.subspeciesCreated;
    Subspecies pSubspecies = this.newObject();
    pSubspecies.newSpecies(pAsset, pTile, pMutation);
    this.addRandomTraitFromBiomeToSubspecies(pSubspecies, pTile);
    this.addTraitsFromBiomeToSubspecies(pSubspecies, pTile);
    return pSubspecies;
  }

  protected override void finishDirtyUnits()
  {
    base.finishDirtyUnits();
    this.cacheCounters();
  }

  private void cacheCounters()
  {
    foreach (Subspecies subspecies in this.list)
      subspecies.cacheCounters();
  }

  public void addRandomTraitFromBiomeToSubspecies(Subspecies pSubspecies, WorldTile pTile)
  {
    pSubspecies.addRandomTraitFromBiome<SubspeciesTrait>(pTile, pTile.Type.biome_asset?.spawn_trait_subspecies, (AssetLibrary<SubspeciesTrait>) AssetManager.subspecies_traits);
  }

  public void addTraitsFromBiomeToSubspecies(Subspecies pSubspecies, WorldTile pTile)
  {
    pSubspecies.addTraitFromBiome<SubspeciesTrait>(pTile, pTile.Type.biome_asset?.spawn_trait_subspecies_always, (AssetLibrary<SubspeciesTrait>) AssetManager.subspecies_traits);
  }

  public override void removeObject(Subspecies pObject)
  {
    ++World.world.game_stats.data.subspeciesExtinct;
    ++World.world.map_stats.subspeciesExtinct;
    base.removeObject(pObject);
  }

  public override void update(float pElapsed)
  {
    base.update(pElapsed);
    if (World.world.isPaused())
      return;
    this._timer_unstable_genome -= pElapsed;
    if ((double) this._timer_unstable_genome > 0.0)
      return;
    this._timer_unstable_genome = Randy.randomFloat(300f, 600f);
    this.checkSpecialTraits();
  }

  private void checkSpecialTraits()
  {
    foreach (Subspecies subspecies in this.list)
    {
      if (subspecies.hasTrait("unstable_genome"))
        subspecies.unstableGenomeEvent();
    }
  }

  protected override void updateDirtyUnits()
  {
    for (int index = 0; index < World.world.units.units_only_dying.Count; ++index)
    {
      Actor actor = World.world.units.units_only_dying[index];
      if (actor.subspecies != null)
        actor.subspecies.preserveAlive();
    }
    List<Actor> unitsOnlyAlive = World.world.units.units_only_alive;
    for (int index = 0; index < unitsOnlyAlive.Count; ++index)
    {
      Actor pActor = unitsOnlyAlive[index];
      Subspecies subspecies = pActor.subspecies;
      if (subspecies != null && subspecies.isDirtyUnits())
        subspecies.listUnit(pActor);
    }
  }

  public override void clear()
  {
    base.clear();
    this._timer_unstable_genome = Randy.randomFloat(300f, 600f);
  }

  public Subspecies getNearbySpecies(
    ActorAsset pAsset,
    WorldTile pTile,
    out Actor pSubspeciesActor,
    bool pLookForSapientSubspecies = false,
    bool pStopAtFirst = false)
  {
    Subspecies nearbySpecies = (Subspecies) null;
    pSubspeciesActor = (Actor) null;
    using (ListPool<Actor> listPool = new ListPool<Actor>((ICollection<Actor>) pAsset.units))
    {
      string.IsNullOrEmpty(pTile.Type.biome_id);
      float num1 = (float) pAsset.species_spawn_radius;
      for (int index = 0; index < listPool.Count; ++index)
      {
        Actor actor = listPool[index];
        if (actor.isAlive() && actor.hasSubspecies() && actor.subspecies.isSpecies(pAsset.id) && (!pLookForSapientSubspecies || actor.subspecies.isSapient()))
        {
          float num2 = Toolbox.DistTile(pTile, actor.current_tile);
          if ((double) num2 <= (double) pAsset.species_spawn_radius && (double) num2 < (double) num1)
          {
            num1 = num2;
            nearbySpecies = actor.subspecies;
            pSubspeciesActor = actor;
            if (pStopAtFirst && (double) num1 < 10.0)
              break;
          }
        }
      }
      return nearbySpecies;
    }
  }

  public Sprite getTopicSprite()
  {
    return this.list.GetRandom<Subspecies>()?.getActorAsset().getSpriteIcon();
  }
}

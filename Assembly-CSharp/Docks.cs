// Decompiled with JetBrains decompiler
// Type: Docks
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityPools;

#nullable disable
public class Docks : BaseBuildingComponent
{
  public ListPool<WorldTile> tiles_ocean;
  private Dictionary<string, int> _boat_types;

  internal override void create(Building pBuilding)
  {
    base.create(pBuilding);
    this.tiles_ocean = new ListPool<WorldTile>();
    this._boat_types = UnsafeCollectionPool<Dictionary<string, int>, KeyValuePair<string, int>>.Get();
  }

  public TileIsland getIsland()
  {
    if (!this.building.hasCity())
      return (TileIsland) null;
    return this.building.city.getTile()?.region.island;
  }

  public WorldTile getOceanTileInSameOcean(WorldTile pTile)
  {
    foreach (WorldTile oceanTileInSameOcean in this.tiles_ocean.LoopRandom<WorldTile>())
    {
      if (oceanTileInSameOcean.isSameIsland(pTile))
        return oceanTileInSameOcean;
    }
    return (WorldTile) null;
  }

  public bool hasOceanTiles()
  {
    this.recalculateOceanTiles();
    return this.tiles_ocean.Count > 0;
  }

  public void recalculateOceanTiles()
  {
    this.tiles_ocean.Clear();
    WorldTile tile1 = World.world.GetTile(this.building.current_tile.x - 4, this.building.current_tile.y);
    if (tile1 != null && tile1.isGoodForBoat())
      this.tiles_ocean.Add(tile1);
    WorldTile tile2 = World.world.GetTile(this.building.current_tile.x + 5, this.building.current_tile.y);
    if (tile2 != null && tile2.isGoodForBoat())
      this.tiles_ocean.Add(tile2);
    WorldTile tile3 = World.world.GetTile(this.building.current_tile.x, this.building.current_tile.y - 4);
    if (tile3 != null && tile3.isGoodForBoat())
      this.tiles_ocean.Add(tile3);
    WorldTile tile4 = World.world.GetTile(this.building.current_tile.x, this.building.current_tile.y + 7);
    if (tile4 != null && tile4.isGoodForBoat())
      this.tiles_ocean.Add(tile4);
    if (this.tiles_ocean.Count != 0)
      return;
    this.building.startDestroyBuilding();
  }

  public bool isDockGood()
  {
    if (this.tiles_ocean.Count == 0)
      return false;
    for (int index = 0; index < this.tiles_ocean.Count; ++index)
    {
      if (!this.tiles_ocean[index].Type.ocean)
        return false;
    }
    return true;
  }

  private bool ifStayingOnGround()
  {
    for (int index = 0; index < this.building.tiles.Count; ++index)
    {
      if (this.building.tiles[index].Type.ground)
        return true;
    }
    return false;
  }

  public override void update(float pElapsed)
  {
    base.update(pElapsed);
    if (this.ifStayingOnGround())
      this.building.getHit(1000f, true, AttackType.Other, (BaseSimObject) null, true, false, true);
    if (!this.building.hasCity() || this.building.city.buildings.Count != 1)
      return;
    this.building.getHit(1000f, true, AttackType.Other, (BaseSimObject) null, true, false, true);
  }

  public int countBoatTypes(string pType)
  {
    int num;
    this._boat_types.TryGetValue(pType, out num);
    return num;
  }

  public bool isFull(string pType) => this.countBoatTypes(pType) >= 1;

  public bool isOverfilled(string pType) => this.countBoatTypes(pType) > 1;

  public Actor buildBoatFromHere(City pCity)
  {
    ActorAsset boatAssetToBuild = this.building.asset.getRandomBoatAssetToBuild(pCity);
    if (boatAssetToBuild == null)
      return (Actor) null;
    if (this.countBoatTypes(boatAssetToBuild.boat_type) >= 1)
      return (Actor) null;
    if (!pCity.hasEnoughResourcesFor(boatAssetToBuild.cost))
      return (Actor) null;
    if (this.tiles_ocean.Count == 0)
    {
      this.recalculateOceanTiles();
      return (Actor) null;
    }
    WorldTile random = this.tiles_ocean.GetRandom<WorldTile>();
    if (!random.region.island.goodForDocks())
      return (Actor) null;
    Actor newUnit = World.world.units.createNewUnit(boatAssetToBuild.id, random);
    if (newUnit == null)
      return (Actor) null;
    this.addBoatToDock(newUnit);
    pCity.spendResourcesForBuildingAsset(boatAssetToBuild.cost);
    return newUnit;
  }

  public void clearBoatCounter() => this._boat_types.Clear();

  public void increaseBoatCounter(Actor pActor)
  {
    int num1 = this.countBoatTypes(pActor.asset.boat_type);
    int num2;
    this._boat_types[pActor.asset.boat_type] = num2 = num1 + 1;
  }

  public void addBoatToDock(Actor pBoat)
  {
    // Check for boat docking restrictions if world law is enabled
    if (WorldLawLibrary.world_law_army_no_cross_foreign.isEnabled() && pBoat.army != null && this.building.hasCity())
    {
      Kingdom boatKingdom = pBoat.kingdom;
      Kingdom dockKingdom = this.building.city.kingdom;
      if (dockKingdom != null && dockKingdom != boatKingdom)
      {
        Alliance boatAlliance = boatKingdom.getAlliance();
        bool isAllied = boatAlliance != null && boatAlliance.kingdoms_hashset.Contains(dockKingdom);
        bool isDirectEnemy = boatKingdom.isInWarWith(dockKingdom);
        if (!isAllied && !isDirectEnemy)
        {
          // Prevent docking in foreign territory
          return;
        }
      }
    }
    pBoat.setHomeBuilding(this.building);
    pBoat.joinCity(this.building.city);
    this.increaseBoatCounter(pBoat);
  }

  public override void Dispose()
  {
    base.Dispose();
    this.tiles_ocean.Dispose();
    this.tiles_ocean = (ListPool<WorldTile>) null;
    this._boat_types.Clear();
    UnsafeCollectionPool<Dictionary<string, int>, KeyValuePair<string, int>>.Release(this._boat_types);
    this._boat_types = (Dictionary<string, int>) null;
  }
}

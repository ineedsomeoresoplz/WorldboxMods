// Decompiled with JetBrains decompiler
// Type: BuildingSpreadBiome
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

#nullable disable
public class BuildingSpreadBiome : BaseBuildingComponent
{
  private const float SPREAD_INTERVAL_MIN = 8f;
  private const float SPREAD_INTERVAL_MAX = 16f;
  private const int SPREAD_RANGE = 2;
  private BiomeAsset _biome_asset;
  private float _spread_timer = 1f;

  internal override void create(Building pBuilding)
  {
    base.create(pBuilding);
    this._biome_asset = AssetManager.biome_library.get(pBuilding.asset.spread_biome_id);
  }

  public override void update(float pElapsed)
  {
    if (!WorldLawLibrary.world_law_terramorphing.isEnabled())
      return;
    base.update(pElapsed);
    if ((double) this._spread_timer > 0.0)
    {
      this._spread_timer -= pElapsed;
    }
    else
    {
      this._spread_timer = Randy.randomFloat(8f, 16f);
      this.spreadBiome();
    }
  }

  private void spreadBiome()
  {
    TileTypeBase tileHigh = (TileTypeBase) this._biome_asset.getTileHigh();
    WorldBehaviourActionBiomes.trySpreadBiomeAround(this.building.current_tile.Type.biome_asset == tileHigh.biome_asset ? Toolbox.getRandomTileWithinDistance(this.building.current_tile, 2) : this.building.current_tile, tileHigh, pSkipEraCheck: true);
  }

  public override void Dispose()
  {
    this._biome_asset = (BiomeAsset) null;
    base.Dispose();
  }
}

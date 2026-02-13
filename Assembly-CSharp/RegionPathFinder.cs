// Decompiled with JetBrains decompiler
// Type: RegionPathFinder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class RegionPathFinder
{
  private Dictionary<string, List<MapRegion>> cachedPaths = new Dictionary<string, List<MapRegion>>();
  private List<MapRegion> _temp_regions_cur_wave = new List<MapRegion>();
  private List<MapRegion> _temp_regions_next_wave = new List<MapRegion>();
  internal List<MapRegion> _temp_regions_checked = new List<MapRegion>();
  internal List<MapRegion> last_globalPath;
  private int currentWave;
  public WorldTile tileStart;
  public WorldTile tileTarget;
  public bool simplePath;

  public PathFinderResult getGlobalPath(WorldTile pFrom, WorldTile pTarget, bool pBoat = false)
  {
    this.last_globalPath = (List<MapRegion>) null;
    if (pFrom == pTarget || pFrom.region == pTarget.region && pFrom.region.tiles.Count == 256 /*0x0100*/)
      return PathFinderResult.SamePlace;
    if (pFrom.region == pTarget.region)
      return PathFinderResult.PathFound;
    if (pFrom.region.island != pTarget.region.island)
      return PathFinderResult.DifferentIslands;
    this.tileStart = pFrom;
    this.tileTarget = pTarget;
    string str = $"{pFrom.region.id.ToString()}_{pTarget.region.id.ToString()}";
    if (DebugConfig.isOn(DebugOption.UseCacheForRegionPath) && this.cachedPaths.TryGetValue(str, out this.last_globalPath))
      return PathFinderResult.PathFound;
    this.last_globalPath = new List<MapRegion>();
    this.startWave(this.tileTarget.region);
    if (DebugConfig.isOn(DebugOption.UseCacheForRegionPath))
      this.addToCache(str, this.last_globalPath);
    return PathFinderResult.PathFound;
  }

  public void addToCache(string pID, List<MapRegion> pPath)
  {
    if (this.cachedPaths.Count > 1000)
      this.cachedPaths.Clear();
    this.cachedPaths.Add(pID, pPath);
  }

  public void clearCache() => this.cachedPaths.Clear();

  public void clear()
  {
    this.last_globalPath = (List<MapRegion>) null;
    this._temp_regions_cur_wave.Clear();
    this._temp_regions_next_wave.Clear();
    this.clearRegions();
    this.clearCache();
    this.currentWave = 0;
    this.tileStart = (WorldTile) null;
    this.tileTarget = (WorldTile) null;
  }

  private void startWave(MapRegion pRegion)
  {
    this.simplePath = true;
    this.clearRegions();
    this.currentWave = 0;
    this._temp_regions_cur_wave.Clear();
    this._temp_regions_next_wave.Clear();
    this.addToNext(pRegion);
    this.newWave();
  }

  private void addToNext(MapRegion pRegion)
  {
    if (pRegion.tiles.Count != 256 /*0x0100*/)
      this.simplePath = false;
    pRegion.is_checked_path = true;
    this._temp_regions_next_wave.Add(pRegion);
    this._temp_regions_checked.Add(pRegion);
    pRegion.path_wave_id = this.currentWave;
  }

  private void newWave()
  {
    ++this.currentWave;
    this._temp_regions_cur_wave.Clear();
    this._temp_regions_cur_wave.AddRange((IEnumerable<MapRegion>) this._temp_regions_next_wave);
    this._temp_regions_next_wave.Clear();
    for (int index1 = 0; index1 < this._temp_regions_cur_wave.Count; ++index1)
    {
      MapRegion mapRegion = this._temp_regions_cur_wave[index1];
      for (int index2 = 0; index2 < mapRegion.neighbours.Count; ++index2)
      {
        MapRegion neighbour = mapRegion.neighbours[index2];
        if (!neighbour.is_checked_path)
        {
          this.addToNext(neighbour);
          if (neighbour == this.tileStart.region)
          {
            this.finalPath(this.tileStart.region);
            return;
          }
        }
      }
    }
    if (this._temp_regions_next_wave.Count <= 0)
      return;
    this.newWave();
  }

  private void finalPath(MapRegion pMainRegion)
  {
    this.last_globalPath.Add(pMainRegion);
    pMainRegion.region_path = true;
    if (pMainRegion == this.tileTarget.region)
      return;
    MapRegion pMainRegion1 = (MapRegion) null;
    for (int index = 0; index < pMainRegion.neighbours.Count; ++index)
    {
      MapRegion neighbour = pMainRegion.neighbours[index];
      if (neighbour.path_wave_id != -1)
      {
        if (pMainRegion1 == null)
          pMainRegion1 = neighbour;
        else if (neighbour.path_wave_id < pMainRegion1.path_wave_id)
          pMainRegion1 = neighbour;
        else if (neighbour.path_wave_id == pMainRegion1.path_wave_id)
        {
          MapChunk chunk1 = this.tileTarget.chunk;
          MapChunk chunk2 = pMainRegion1.chunk;
          MapChunk chunk3 = neighbour.chunk;
          int num = Toolbox.SquaredDist(chunk2.x, chunk2.y, chunk1.x, chunk1.y);
          if (Toolbox.SquaredDist(chunk3.x, chunk3.y, chunk1.x, chunk1.y) < num)
            pMainRegion1 = neighbour;
        }
      }
    }
    this.finalPath(pMainRegion1);
  }

  private void clearRegions()
  {
    for (int index = 0; index < this._temp_regions_checked.Count; ++index)
    {
      MapRegion mapRegion = this._temp_regions_checked[index];
      mapRegion.is_checked_path = false;
      mapRegion.path_wave_id = -1;
      mapRegion.region_path = false;
    }
    this._temp_regions_checked.Clear();
  }

  public string debug() => this.cachedPaths.Count.ToString() ?? "";
}

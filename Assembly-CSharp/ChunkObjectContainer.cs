// Decompiled with JetBrains decompiler
// Type: ChunkObjectContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable disable
public class ChunkObjectContainer : IDisposable
{
  public readonly List<long> kingdoms = new List<long>();
  public readonly List<Actor> units_all = new List<Actor>();
  public readonly List<Building> buildings_all = new List<Building>();
  private readonly HashSet<long> _hash_kingdoms = new HashSet<long>();
  private readonly Dictionary<long, List<Actor>> _dict_units = new Dictionary<long, List<Actor>>();
  private readonly Dictionary<long, List<Building>> _dict_buildings = new Dictionary<long, List<Building>>();
  private int _total_units;
  private int _total_buildings;

  public int total_units => this._total_units;

  public int total_buildings => this._total_buildings;

  public void reset(bool pClearBuildings)
  {
    if (this._total_units == 0 && this._total_buildings == 0 || this._total_units == 0 && !pClearBuildings)
      return;
    foreach (List<Actor> actorList in this._dict_units.Values)
      actorList.Clear();
    this.units_all.Clear();
    this._total_units = 0;
    this.kingdoms.Clear();
    this._hash_kingdoms.Clear();
    if (pClearBuildings)
    {
      this.buildings_all.Clear();
      this._total_buildings = 0;
      foreach (List<Building> buildingList in this._dict_buildings.Values)
        buildingList.Clear();
    }
    else
    {
      if (this._dict_buildings.Count <= 0)
        return;
      foreach (long key in this._dict_buildings.Keys)
        this.kingdoms.Add(key);
      this._hash_kingdoms.UnionWith((IEnumerable<long>) this.kingdoms);
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public List<Building> getBuildings(long pKingdom) => this._dict_buildings[pKingdom];

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public List<Actor> getUnits(long pKingdom) => this._dict_units[pKingdom];

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public bool isEmpty() => this.kingdoms.Count == 0;

  public void addActor(Actor pActor)
  {
    long id = pActor.kingdom.id;
    if (this._hash_kingdoms.Add(id))
    {
      List<Actor> actorList;
      if (!this._dict_units.TryGetValue(id, out actorList))
      {
        actorList = new List<Actor>();
        this._dict_units[id] = actorList;
        this._dict_buildings[id] = new List<Building>();
      }
      actorList.Add(pActor);
      this.kingdoms.Add(id);
      ++this._total_units;
    }
    else
    {
      this._dict_units[id].Add(pActor);
      ++this._total_units;
    }
    this.units_all.Add(pActor);
  }

  public void addBuilding(Building pBuilding)
  {
    long id = pBuilding.kingdom.id;
    if (this._hash_kingdoms.Add(id))
    {
      List<Building> buildingList;
      if (!this._dict_buildings.TryGetValue(id, out buildingList))
      {
        buildingList = new List<Building>();
        this._dict_buildings[id] = buildingList;
        this._dict_units[id] = new List<Actor>();
      }
      buildingList.Add(pBuilding);
      ++this._total_buildings;
      this.kingdoms.Add(id);
    }
    else
    {
      this._dict_buildings[id].Add(pBuilding);
      ++this._total_buildings;
    }
    this.buildings_all.Add(pBuilding);
  }

  public void Dispose()
  {
    this.reset(true);
    this._dict_units.Clear();
    this._dict_buildings.Clear();
    this.units_all.Clear();
    this.buildings_all.Clear();
    this._total_units = 0;
    this._total_buildings = 0;
  }

  public Dictionary<long, List<Building>>.ValueCollection getDebugBuildings()
  {
    return this._dict_buildings.Values;
  }

  public Dictionary<long, List<Actor>>.ValueCollection getDebugUnits() => this._dict_units.Values;
}

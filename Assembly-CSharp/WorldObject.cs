// Decompiled with JetBrains decompiler
// Type: WorldObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using db;
using db.tables;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class WorldObject : NanoObject, IMetaObject, ICoreObject
{
  protected static readonly HashSet<Family> _family_counter = new HashSet<Family>();

  private HistoryMetaDataAsset _history_meta_data_asset
  {
    get => AssetManager.history_meta_data_library.get("world");
  }

  protected override MetaType meta_type => MetaType.World;

  public override long getID() => 1;

  public override string name
  {
    get => World.world.map_stats.name;
    protected set => World.world.map_stats.name = value;
  }

  public List<Actor> units => World.world.units.getSimpleList();

  public int countUnits() => World.world.units.Count;

  public IEnumerable<Actor> getUnits() => (IEnumerable<Actor>) World.world.units;

  public bool hasUnits() => World.world.units.Count > 0;

  public Actor getRandomUnit() => World.world.units.GetRandom();

  public Actor getRandomActorForReaper() => (Actor) null;

  public IEnumerable<Family> getFamilies() => (IEnumerable<Family>) World.world.families;

  public int countFamilies() => World.world.families.Count;

  public bool hasFamilies() => World.world.families.Count > 0;

  public override ColorAsset getColor()
  {
    return AssetManager.kingdom_colors_library.list.GetRandom<ColorAsset>();
  }

  public MetaObjectData getMetaData() => throw new NotImplementedException();

  public int getRenown() => throw new NotImplementedException();

  public int getPopulationPeople() => throw new NotImplementedException();

  public long getTotalKills() => throw new NotImplementedException();

  public long getTotalDeaths() => throw new NotImplementedException();

  public bool isSelected() => throw new NotImplementedException();

  public Actor getOldestVisibleUnit() => throw new NotImplementedException();

  public Actor getOldestVisibleUnitForNameplatesCached() => throw new NotImplementedException();

  public void startCollectHistoryData()
  {
    foreach (HistoryDataCollector invocation in this._history_meta_data_asset.collector.GetInvocationList())
    {
      HistoryTable pObject = invocation((NanoObject) this);
      pObject.timestamp = (long) World.world.map_stats.history_current_year;
      DBInserter.insertData(pObject, "world");
    }
  }

  public void clearLastYearStats()
  {
  }

  public ActorAsset getActorAsset() => throw new NotImplementedException();

  public Sprite getSpriteIcon() => throw new NotImplementedException();

  public bool isCursorOver() => throw new NotImplementedException();

  public void setCursorOver() => throw new NotImplementedException();

  public int getAge() => throw new NotImplementedException();

  public bool isFavorite() => throw new NotImplementedException();

  public void switchFavorite() => throw new NotImplementedException();

  public bool hasCities() => World.world.cities.Count > 0;

  public IEnumerable<City> getCities() => (IEnumerable<City>) World.world.cities;

  public bool hasKingdoms() => World.world.kingdoms.Count > 0;

  public IEnumerable<Kingdom> getKingdoms() => (IEnumerable<Kingdom>) World.world.kingdoms;
}

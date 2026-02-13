// Decompiled with JetBrains decompiler
// Type: SavedMap
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Ionic.Zlib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Scripting;

#nullable disable
[Serializable]
public class SavedMap
{
  public int saveVersion;
  public int width;
  public int height;
  public HotkeyTabsData hotkey_tabs_data;
  public float camera_pos_x;
  public float camera_pos_y;
  public float camera_zoom;
  public MapStats mapStats;
  public WorldLaws worldLaws;
  public string tileString;
  public List<string> tileMap = new List<string>();
  public int[][] tileArray;
  public int[][] tileAmounts;
  public List<int> fire = new List<int>();
  public List<int> conwayEater = new List<int>();
  public List<int> conwayCreator = new List<int>();
  public List<int> frozen_tiles = new List<int>();
  public List<WorldTileData> tiles = new List<WorldTileData>();
  public List<CityData> cities = new List<CityData>();
  [Preserve]
  [Obsolete]
  public List<ActorDataObsolete> actors;
  public List<ActorData> actors_data = new List<ActorData>();
  public List<BuildingData> buildings = new List<BuildingData>();
  public List<KingdomData> kingdoms = new List<KingdomData>();
  public List<ClanData> clans = new List<ClanData>();
  public List<AllianceData> alliances = new List<AllianceData>();
  public List<WarData> wars = new List<WarData>();
  public List<PlotData> plots = new List<PlotData>();
  public List<DiplomacyRelationData> relations = new List<DiplomacyRelationData>();
  public List<CultureData> cultures = new List<CultureData>();
  public List<BookData> books = new List<BookData>();
  public List<SubspeciesData> subspecies = new List<SubspeciesData>();
  public List<LanguageData> languages = new List<LanguageData>();
  public List<ReligionData> religions = new List<ReligionData>();
  public List<FamilyData> families = new List<FamilyData>();
  public List<ArmyData> armies = new List<ArmyData>();
  public List<ItemData> items = new List<ItemData>();

  public SavedMap()
  {
    this.width = Config.ZONE_AMOUNT_X_DEFAULT;
    this.height = Config.ZONE_AMOUNT_Y_DEFAULT;
  }

  public void check()
  {
    if (this.worldLaws == null)
      this.worldLaws = new WorldLaws();
    if (this.mapStats == null)
      this.mapStats = new MapStats();
    if (this.hotkey_tabs_data == null)
      this.hotkey_tabs_data = new HotkeyTabsData();
    if (this.tileMap == null)
      this.tileMap = new List<string>();
    if (this.fire == null)
      this.fire = new List<int>();
    if (this.conwayEater == null)
      this.conwayEater = new List<int>();
    if (this.conwayCreator == null)
      this.conwayCreator = new List<int>();
    if (this.frozen_tiles == null)
      this.frozen_tiles = new List<int>();
    if (this.tiles == null)
      this.tiles = new List<WorldTileData>();
    if (this.cities == null)
      this.cities = new List<CityData>();
    if (this.actors_data == null)
      this.actors_data = new List<ActorData>();
    if (this.buildings == null)
      this.buildings = new List<BuildingData>();
    if (this.kingdoms == null)
      this.kingdoms = new List<KingdomData>();
    if (this.clans == null)
      this.clans = new List<ClanData>();
    if (this.alliances == null)
      this.alliances = new List<AllianceData>();
    if (this.wars == null)
      this.wars = new List<WarData>();
    if (this.plots == null)
      this.plots = new List<PlotData>();
    if (this.relations == null)
      this.relations = new List<DiplomacyRelationData>();
    if (this.cultures == null)
      this.cultures = new List<CultureData>();
    if (this.books == null)
      this.books = new List<BookData>();
    if (this.subspecies == null)
      this.subspecies = new List<SubspeciesData>();
    if (this.languages == null)
      this.languages = new List<LanguageData>();
    if (this.religions == null)
      this.religions = new List<ReligionData>();
    if (this.families == null)
      this.families = new List<FamilyData>();
    if (this.armies == null)
      this.armies = new List<ArmyData>();
    if (this.items == null)
      this.items = new List<ItemData>();
    this.worldLaws.check();
  }

  public void init()
  {
    this.worldLaws = new WorldLaws();
    this.worldLaws.init(false);
  }

  public int getTileMapID(string pTileString)
  {
    if (!this.tileMap.Contains(pTileString))
      this.tileMap.Add(pTileString);
    return this.tileMap.IndexOf(pTileString);
  }

  public void create()
  {
    this.init();
    this.width = Config.ZONE_AMOUNT_X;
    this.height = Config.ZONE_AMOUNT_Y;
    this.camera_pos_x = ((Component) World.world.camera).transform.position.x;
    this.camera_pos_y = ((Component) World.world.camera).transform.position.y;
    this.camera_zoom = MoveCamera.instance.getTargetZoom();
    this.saveVersion = Config.WORLD_SAVE_VERSION;
    this.hotkey_tabs_data = World.world.hotkey_tabs_data;
    this.mapStats = World.world.map_stats;
    this.worldLaws = World.world.world_laws;
    this.mapStats.population = (long) World.world.units.Count;
    this.items = World.world.items.save();
    this.books = World.world.books.save();
    this.subspecies = World.world.subspecies.save();
    this.families = World.world.families.save();
    this.armies = World.world.armies.save();
    this.languages = World.world.languages.save();
    this.religions = World.world.religions.save();
    this.cultures = World.world.cultures.save();
    this.kingdoms = World.world.kingdoms.save();
    this.clans = World.world.clans.save();
    this.alliances = World.world.alliances.save();
    this.wars = World.world.wars.save();
    this.plots = World.world.plots.save();
    this.relations = World.world.diplomacy.save((List<DiplomacyRelation>) null);
    this.cities = World.world.cities.save((List<City>) null);
    if (this.tileMap == null)
      this.check();
    this.tileMap.Clear();
    this.fire.Clear();
    this.conwayEater.Clear();
    this.conwayCreator.Clear();
    this.frozen_tiles.Clear();
    using (ListPool<int[]> list1 = new ListPool<int[]>())
    {
      using (ListPool<int[]> list2 = new ListPool<int[]>())
      {
        string pTileString = string.Empty;
        int num1 = 0;
        int index1 = 0;
        int length = this.width * 64 /*0x40*/;
        list1.Add(new int[length]);
        list2.Add(new int[length]);
        int aPos1 = 0;
        for (int index2 = 0; index2 < World.world.tiles_list.Length; ++index2)
        {
          WorldTile tiles = World.world.tiles_list[index2];
          string wholeTileIdForSave = this.getWholeTileIDForSave(tiles);
          Vector2Int pos;
          if (!(wholeTileIdForSave != pTileString))
          {
            int num2 = index1;
            pos = tiles.pos;
            int y = ((Vector2Int) ref pos).y;
            if (num2 == y)
              goto label_11;
          }
          if (num1 > 0)
          {
            list2[index1][aPos1] = num1;
            list1[index1][aPos1++] = this.getTileMapID(pTileString);
            num1 = 0;
          }
          pTileString = wholeTileIdForSave;
          int num3 = index1;
          pos = tiles.pos;
          int y1 = ((Vector2Int) ref pos).y;
          if (num3 != y1)
          {
            list1[index1] = Toolbox.resizeArray<int>(list1[index1], aPos1);
            list2[index1] = Toolbox.resizeArray<int>(list2[index1], aPos1);
            pos = tiles.pos;
            index1 = ((Vector2Int) ref pos).y;
            list1.Add(new int[length]);
            list2.Add(new int[length]);
            aPos1 = 0;
          }
label_11:
          ++num1;
          if (tiles.isOnFire())
            this.fire.Add(tiles.data.tile_id);
          if (tiles.data.conwayType == ConwayType.Eater)
            this.conwayEater.Add(tiles.data.tile_id);
          if (tiles.data.conwayType == ConwayType.Creator)
            this.conwayCreator.Add(tiles.data.tile_id);
          if (tiles.data.frozen)
            this.frozen_tiles.Add(tiles.data.tile_id);
        }
        if (num1 > 0)
        {
          list2[index1][aPos1] = num1;
          int[] numArray = list1[index1];
          int index3 = aPos1;
          int aPos2 = index3 + 1;
          int tileMapId = this.getTileMapID(pTileString);
          numArray[index3] = tileMapId;
          list1[index1] = Toolbox.resizeArray<int>(list1[index1], aPos2);
          list2[index1] = Toolbox.resizeArray<int>(list2[index1], aPos2);
        }
        this.tileArray = list1.ToArray<int[]>();
        this.tileAmounts = list2.ToArray<int[]>();
        foreach (Actor unit in (SimSystemManager<Actor, ActorData>) World.world.units)
        {
          if (unit.isAlive() && !unit.asset.skip_save)
          {
            unit.prepareForSave();
            this.actors_data.Add(unit.data);
          }
        }
        foreach (Building building in (SimSystemManager<Building, BuildingData>) World.world.buildings)
        {
          if (building.data.state != BuildingState.Removed)
          {
            building.prepareForSave();
            this.buildings.Add(building.data);
          }
        }
      }
    }
  }

  private string getWholeTileIDForSave(WorldTile pTile)
  {
    return pTile.top_type == null ? pTile.main_type.id : $"{pTile.main_type.id}:{pTile.top_type.id}";
  }

  public void toJson(string pFilePath)
  {
    if (this.worldLaws == null)
      this.create();
    try
    {
      using (FileStream fileStream = new FileStream(pFilePath, FileMode.Create, FileAccess.Write))
      {
        StreamWriter streamWriter1 = new StreamWriter((Stream) fileStream);
        streamWriter1.NewLine = "\n";
        using (StreamWriter streamWriter2 = streamWriter1)
        {
          using (JsonWriter jsonWriter = (JsonWriter) new JsonTextWriter((TextWriter) streamWriter2))
            JsonHelper.writer.Serialize(jsonWriter, (object) this);
        }
      }
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ex);
      throw;
    }
    Config.scheduleGC(nameof (toJson));
  }

  public string toJson(bool pBeautify = false)
  {
    if (this.worldLaws == null)
      this.create();
    string str = "";
    try
    {
      JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
      {
        DefaultValueHandling = (DefaultValueHandling) 3,
        Formatting = (Formatting) 0
      };
      if (pBeautify)
        serializerSettings.Formatting = (Formatting) 1;
      str = JsonConvert.SerializeObject((object) this, serializerSettings);
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ex);
    }
    return !string.IsNullOrEmpty(str) && str.Length >= 20 ? str : throw new Exception("Error while creating json ( empty string < 20 )");
  }

  public void toZip(string pFilePath)
  {
    using (FileStream fileStream = new FileStream(pFilePath, FileMode.Create, FileAccess.Write))
    {
      using (ZlibStream zlibStream = new ZlibStream((Stream) fileStream, (CompressionMode) 0, (CompressionLevel) 9))
      {
        using (StreamWriter streamWriter = new StreamWriter((Stream) zlibStream))
        {
          using (JsonWriter jsonWriter = (JsonWriter) new JsonTextWriter((TextWriter) streamWriter))
          {
            JsonHelper.writer.Serialize(jsonWriter, (object) this);
            Config.scheduleGC(nameof (toZip));
          }
        }
      }
    }
  }

  public byte[] toZip() => ZlibStream.CompressString(this.toJson());

  public MapMetaData getMeta()
  {
    MapMetaData meta = new MapMetaData();
    int num1 = 0;
    int num2 = 0;
    int num3 = 0;
    Dictionary<long, bool> dictionary = CollectionPool<Dictionary<long, bool>, KeyValuePair<long, bool>>.Get();
    if (this.subspecies != null)
    {
      foreach (SubspeciesData subspeciesData in this.subspecies)
      {
        if (subspeciesData.saved_traits.Contains("prefrontal_cortex"))
          dictionary.Add(subspeciesData.id, true);
      }
    }
    if (this.actors_data != null)
    {
      foreach (ActorData actorData in this.actors_data)
      {
        if (AssetManager.actor_library.has(actorData.asset_id))
        {
          if (actorData.favorite)
            ++num3;
          if (actorData.civ_kingdom_id != -1L)
            ++num1;
          else if (dictionary.ContainsKey(actorData.subspecies))
          {
            ++num1;
          }
          else
          {
            ActorAsset actorAsset = AssetManager.actor_library.get(actorData.asset_id);
            if (actorAsset != null)
            {
              if (actorAsset.civ)
                ++num1;
              else
                ++num2;
            }
          }
        }
      }
    }
    CollectionPool<Dictionary<long, bool>, KeyValuePair<long, bool>>.Release(dictionary);
    int num4 = 0;
    int num5 = 0;
    if (this.items != null)
    {
      foreach (ItemData itemData in this.items)
      {
        if (AssetManager.items.has(itemData.asset_id))
        {
          if (itemData.favorite)
            ++num5;
          ++num4;
        }
      }
    }
    meta.saveVersion = this.saveVersion;
    meta.width = this.width;
    meta.height = this.height;
    meta.mapStats = this.mapStats;
    meta.cities = this.cities.Count;
    MapMetaData mapMetaData = meta;
    List<ActorData> actorsData = this.actors_data;
    // ISSUE: explicit non-virtual call
    int count = actorsData != null ? __nonvirtual (actorsData.Count) : 0;
    mapMetaData.units = count;
    meta.population = num1;
    meta.mobs = num2;
    meta.deaths = World.world.map_stats.deaths;
    meta.favorites = num3;
    meta.favorite_items = num5;
    meta.equipment = num4;
    meta.books = this.books.Count;
    meta.wars = this.wars.Count;
    meta.alliances = this.alliances.Count;
    meta.families = this.families.Count;
    meta.clans = this.clans.Count;
    meta.cultures = this.cultures.Count;
    meta.religions = this.religions.Count;
    meta.languages = this.languages.Count;
    meta.subspecies = this.subspecies.Count;
    meta.cursed = WorldLawLibrary.world_law_cursed_world.isEnabled();
    int num6 = 0;
    int num7 = 0;
    int num8 = 0;
    foreach (BuildingData building in this.buildings)
    {
      if (AssetManager.buildings.has(building.asset_id))
      {
        if (building.cityID.hasValue())
          ++num6;
        if (AssetManager.buildings.get(building.asset_id).flora)
          ++num8;
        ++num7;
      }
    }
    meta.buildings = num6;
    meta.structures = num7;
    meta.kingdoms = this.kingdoms.Count;
    meta.vegetation = num8;
    return meta;
  }

  [OnDeserializing]
  private void OnDeserializingMethod(StreamingContext context) => LongJsonConverter.reset();
}

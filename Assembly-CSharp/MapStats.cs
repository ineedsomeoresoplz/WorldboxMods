// Decompiled with JetBrains decompiler
// Type: MapStats
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
[Serializable]
public class MapStats
{
  public string name = "WorldBox";
  public string description = "";
  public string player_name;
  public string player_mood;
  public SaveCustomData custom_data;
  [JsonProperty]
  public double world_time;
  public int history_current_year = -1;
  [JsonProperty]
  public string world_age_id;
  public int world_age_slot_index;
  public double world_age_started_at;
  public double same_world_age_started_at;
  public float current_world_ages_duration;
  public float current_age_progress;
  public bool is_world_ages_paused;
  [DefaultValue(1f)]
  public float world_ages_speed_multiplier = 1f;
  public string[] world_ages_slots;
  public long housesBuilt;
  public long housesDestroyed;
  public long population;
  public long creaturesBorn;
  public long creaturesCreated;
  public long subspeciesCreated;
  public long subspeciesExtinct;
  public long languagesCreated;
  public long languagesForgotten;
  public long booksWritten;
  public long booksRead;
  public long booksBurnt;
  public long culturesCreated;
  public long culturesForgotten;
  public long religionsCreated;
  public long religionsForgotten;
  public long kingdomsCreated;
  public long kingdomsDestroyed;
  public long citiesCreated;
  public long citiesConquered;
  public long citiesRebelled;
  public long citiesDestroyed;
  public long alliancesMade;
  public long alliancesDissolved;
  public long warsStarted;
  public long peacesMade;
  public long familiesCreated;
  public long armiesCreated;
  public long armiesDestroyed;
  public long familiesDestroyed;
  public long clansCreated;
  public long clansDestroyed;
  public long plotsStarted;
  public long plotsSucceeded;
  public long plotsForgotten;
  public double exploding_mushrooms_enabled_at;
  [DefaultValue(1)]
  public long id_unit = 1;
  [DefaultValue(1)]
  public long id_building = 1;
  [DefaultValue(1)]
  public long id_kingdom = 1;
  [DefaultValue(1)]
  public long id_city = 1;
  [DefaultValue(1)]
  public long id_culture = 1;
  [DefaultValue(1)]
  public long id_clan = 1;
  [DefaultValue(1)]
  public long id_alliance = 1;
  [DefaultValue(1)]
  public long id_war = 1;
  [DefaultValue(1)]
  public long id_projectile = 1;
  [DefaultValue(1)]
  public long id_status = 1;
  [DefaultValue(1)]
  public long id_plot = 1;
  [DefaultValue(1)]
  public long id_book = 1;
  [DefaultValue(1)]
  public long id_subspecies = 1;
  [DefaultValue(1)]
  public long id_family = 1;
  [DefaultValue(1)]
  public long id_army = 1;
  [DefaultValue(1)]
  public long id_language = 1;
  [DefaultValue(1)]
  public long id_religion = 1;
  [DefaultValue(1)]
  public long id_item = 1;
  [DefaultValue(1)]
  public long id_diplomacy = 1;
  [DefaultValue(1)]
  public long life_dna = 1;
  [NonSerialized]
  public long current_infected;
  [NonSerialized]
  public long current_mobs;
  [NonSerialized]
  public long current_houses;
  [NonSerialized]
  public long current_vegetation;
  [NonSerialized]
  public long current_infected_plague;
  private int _last_year = -1;
  private int _last_month = -1;
  private float _timer_stats = 0.1f;
  public static string[] possible_formats = new string[20]
  {
    "pr_",
    "st_",
    "w_",
    "a_",
    "c_",
    "u_",
    "b_",
    "k_",
    "c_",
    "cl_",
    "p_",
    "bo_",
    "sp_",
    "lang_",
    "rel_",
    "it_",
    "f_",
    "fa_",
    "army_",
    "d_"
  };

  [Preserve]
  [Obsolete("use .world_age_id instead", true)]
  public string era_id
  {
    set
    {
      if (string.IsNullOrEmpty(value) || !string.IsNullOrEmpty(this.world_age_id))
        return;
      this.world_age_id = value;
    }
  }

  public long deaths { get; set; }

  public long deaths_age { get; set; }

  public long deaths_hunger { get; set; }

  public long deaths_eaten { get; set; }

  public long deaths_plague { get; set; }

  public long deaths_poison { get; set; }

  public long deaths_infection { get; set; }

  public long deaths_tumor { get; set; }

  public long deaths_acid { get; set; }

  public long deaths_fire { get; set; }

  public long deaths_divine { get; set; }

  public long deaths_weapon { get; set; }

  public long deaths_gravity { get; set; }

  public long deaths_drowning { get; set; }

  public long deaths_water { get; set; }

  public long deaths_explosion { get; set; }

  public long metamorphosis { get; set; }

  public long evolutions { get; set; }

  public long deaths_other { get; set; }

  public long deaths_smile { get; set; }

  public MapStats() => this.checkDefault();

  private void checkDefault()
  {
    if (this.world_ages_slots == null)
      this.world_ages_slots = new string[8];
    if (string.IsNullOrEmpty(this.player_name))
      this.player_name = "The Creator";
    if (string.IsNullOrEmpty(this.player_mood))
      this.setDefaultMood();
    if (this.custom_data != null)
      return;
    this.custom_data = new SaveCustomData();
  }

  public void generateLifeDNA()
  {
    this.life_dna = long.Parse(DateTime.UtcNow.ToString("yyyyMMddHH", (IFormatProvider) CultureInfo.InvariantCulture));
  }

  internal void updateStatsForPanel(float pElapsed)
  {
    if ((double) this._timer_stats > 0.0)
    {
      this._timer_stats -= pElapsed;
    }
    else
    {
      this._timer_stats = 0.1f;
      this.recalcCounters();
    }
  }

  internal void updateWorldTime(float pElapsed)
  {
    this.world_time += (double) pElapsed;
    int currentYear = Date.getCurrentYear();
    int currentMonth = Date.getCurrentMonth();
    if (this._last_year != currentYear)
      World.world.updateObjectAge();
    if (this._last_year != currentYear)
      this._last_month = -1;
    this._last_year = currentYear;
    this._last_month = currentMonth;
  }

  public void load()
  {
    this.checkDefault();
    this._last_year = Date.getCurrentYear();
    this._last_month = Date.getCurrentMonth();
  }

  private void recalcCounters()
  {
    this.current_infected = 0L;
    this.current_mobs = 0L;
    this.current_houses = 0L;
    this.current_vegetation = 0L;
    this.current_infected_plague = 0L;
    List<Actor> simpleList1 = World.world.units.getSimpleList();
    int index1 = 0;
    for (int count = simpleList1.Count; index1 < count; ++index1)
    {
      Actor actor = simpleList1[index1];
      if (actor.hasTrait("plague"))
        ++this.current_infected_plague;
      if (actor.isSick())
        ++this.current_infected;
      if (actor.asset.count_as_unit && !actor.isSapient())
        ++this.current_mobs;
    }
    List<Building> simpleList2 = World.world.buildings.getSimpleList();
    int index2 = 0;
    for (int count = simpleList2.Count; index2 < count; ++index2)
    {
      Building building = simpleList2[index2];
      if (building.isCiv())
        ++this.current_houses;
      else if (building.asset.is_vegetation)
        ++this.current_vegetation;
    }
  }

  public long getNextId(string pType)
  {
    long nextId = 0;
    switch (pType)
    {
      case "alliance":
        nextId = this.id_alliance++;
        break;
      case "army":
        nextId = this.id_army++;
        break;
      case "book":
        nextId = this.id_book++;
        break;
      case "building":
        nextId = this.id_building++;
        break;
      case "city":
        nextId = this.id_city++;
        break;
      case "clan":
        nextId = this.id_clan++;
        break;
      case "culture":
        nextId = this.id_culture++;
        break;
      case "diplomacy":
        nextId = this.id_diplomacy++;
        break;
      case "family":
        nextId = this.id_family++;
        break;
      case "item":
        nextId = this.id_item++;
        break;
      case "kingdom":
        nextId = this.id_kingdom++;
        break;
      case "language":
        nextId = this.id_language++;
        break;
      case "plot":
        nextId = this.id_plot++;
        break;
      case "projectile":
        nextId = this.id_projectile++;
        break;
      case "religion":
        nextId = this.id_religion++;
        break;
      case "statuses":
        nextId = this.id_status++;
        break;
      case "subspecies":
        nextId = this.id_subspecies++;
        break;
      case "unit":
        nextId = this.id_unit++;
        break;
      case "war":
        nextId = this.id_war++;
        break;
      default:
        Debug.LogError((object) ("NO pType for id " + pType));
        break;
    }
    return nextId;
  }

  public static string formatId(string pType, long pID)
  {
    switch (pType)
    {
      case "alliance":
        return "a_" + pID.ToString();
      case "army":
        return "army_" + pID.ToString();
      case "book":
        return "bo_" + pID.ToString();
      case "building":
        return "b_" + pID.ToString();
      case "city":
        return "c_" + pID.ToString();
      case "clan":
        return "cl_" + pID.ToString();
      case "culture":
        return "c_" + pID.ToString();
      case "diplomacy":
        return "d_" + pID.ToString();
      case "family":
        return "fa_" + pID.ToString();
      case "item":
        return "it_" + pID.ToString();
      case "kingdom":
        return "k_" + pID.ToString();
      case "language":
        return "lang_" + pID.ToString();
      case "plot":
        return "p_" + pID.ToString();
      case "projectile":
        return "pr_" + pID.ToString();
      case "religion":
        return "rel_" + pID.ToString();
      case "statuses":
        return "st_" + pID.ToString();
      case "subspecies":
        return "sp_" + pID.ToString();
      case "unit":
        return "u_" + pID.ToString();
      case "war":
        return "w_" + pID.ToString();
      default:
        Debug.LogError((object) ("NO pType for id " + pType));
        return "???_" + pID.ToString();
    }
  }

  public void debug(DebugTool pTool)
  {
    pTool.setText("(d)worldTime:", (object) this.world_time);
    pTool.setText("(f)worldTime:", (object) this.getWorldTime());
    pTool.setText("cur month:", (object) Date.getCurrentMonth());
    pTool.setText("cur year:", (object) Date.getCurrentYear());
    pTool.setText("last_year:", (object) this._last_year);
    pTool.setText("last_month:", (object) this._last_month);
    pTool.setSeparator();
    pTool.setText("months since 0:", (object) Date.getMonthsSince(0.0));
    pTool.setText("years since 0:", (object) Date.getYearsSince(0.0));
    pTool.setText("months since now:", (object) Date.getMonthsSince(this.world_time));
    pTool.setText("years since now:", (object) Date.getYearsSince(this.world_time));
    pTool.setText("month time:", (object) Date.getMonthTime());
    pTool.setSeparator();
    pTool.setText("getDate 0:", (object) Date.getDate(0.0));
    pTool.setText("getYearDate 0:", (object) Date.getYearDate(0.0));
    pTool.setText("getYear 0:", (object) Date.getYear(0.0));
    pTool.setText("getYear0 0:", (object) Date.getYear0(0.0));
    pTool.setText("getDate now:", (object) Date.getDate(this.world_time));
    pTool.setText("getYearDate now:", (object) Date.getYearDate(this.world_time));
    pTool.setText("getYear now:", (object) Date.getYear(this.world_time));
    pTool.setText("getYear0 now:", (object) Date.getYear0(this.world_time));
    pTool.setSeparator();
    pTool.setText("max_float:", (object) float.MaxValue);
  }

  public float getWorldTime() => (float) this.world_time;

  [JsonIgnore]
  public int year => this._last_year;

  public void initNewWorld()
  {
    this.generateLifeDNA();
    this.generatePlayerName();
    this.setDefaultMood();
    this.name = NameGenerator.getName("world_name");
    AssetManager.gene_library.regenerateBasicDNACodesWithLifeSeed(this.life_dna);
  }

  private void generatePlayerName()
  {
    this.player_name = NameGenerator.getName("player_name", pIgnoreBlackList: true);
  }

  private void setDefaultMood() => this.player_mood = "serene";

  [Preserve]
  [JsonProperty("year")]
  [Obsolete("use .world_time instead", true)]
  public int year_obsolete
  {
    set
    {
      if (value == 0)
        return;
      this.world_time += (double) value * 60.0;
    }
  }

  [Preserve]
  [JsonProperty("month")]
  [Obsolete("use .world_time instead", true)]
  public int month_obsolete
  {
    set
    {
      if (value == 0)
        return;
      this.world_time += (double) value * 5.0;
    }
  }

  [Preserve]
  [JsonProperty("worldTime")]
  [Obsolete("use .world_time instead", true)]
  public double worldTime_obsolete
  {
    set
    {
      if (value == 0.0)
        return;
      this.world_time += value;
    }
  }

  public ArchitectMood getArchitectMood()
  {
    if (string.IsNullOrEmpty(this.player_mood))
      this.player_mood = "serene";
    return AssetManager.architect_mood_library.get(this.player_mood) ?? AssetManager.architect_mood_library.get("serene");
  }
}

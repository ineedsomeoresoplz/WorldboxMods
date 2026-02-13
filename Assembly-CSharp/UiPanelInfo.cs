// Decompiled with JetBrains decompiler
// Type: UiPanelInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UiPanelInfo : MonoBehaviour
{
  public GameObject population;
  public GameObject beasts;
  public GameObject deaths;
  public GameObject infected;
  public GameObject buildings;
  public GameObject vegetations;
  private float interval = 0.2f;
  private float timer;
  private Text population_name;
  private Text population_value;
  private Text beasts_name;
  private Text beasts_value;
  private Text deaths_name;
  private Text deaths_value;
  private Text infected_name;
  private Text infected_value;
  private Text buildings_name;
  private Text buildings_value;
  private Text vegetation_name;
  private Text vegetation_value;
  private bool lastRTL;

  private void OnEnable()
  {
    this.population_name = ((Component) this.population.transform.Find("Name")).GetComponent<Text>();
    this.population_value = ((Component) this.population.transform.Find("Value")).GetComponent<Text>();
    this.beasts_name = ((Component) this.beasts.transform.Find("Name")).GetComponent<Text>();
    this.beasts_value = ((Component) this.beasts.transform.Find("Value")).GetComponent<Text>();
    this.infected_name = ((Component) this.infected.transform.Find("Name")).GetComponent<Text>();
    this.infected_value = ((Component) this.infected.transform.Find("Value")).GetComponent<Text>();
    this.deaths_name = ((Component) this.deaths.transform.Find("Name")).GetComponent<Text>();
    this.deaths_value = ((Component) this.deaths.transform.Find("Value")).GetComponent<Text>();
    this.buildings_name = ((Component) this.buildings.transform.Find("Name")).GetComponent<Text>();
    this.buildings_value = ((Component) this.buildings.transform.Find("Value")).GetComponent<Text>();
    this.vegetation_name = ((Component) this.vegetations.transform.Find("Name")).GetComponent<Text>();
    this.vegetation_value = ((Component) this.vegetations.transform.Find("Value")).GetComponent<Text>();
  }

  private void Update()
  {
    if (!Config.game_loaded || Object.op_Equality((Object) World.world, (Object) null) || World.world.map_stats == null || Object.op_Equality((Object) World.world.game_stats, (Object) null))
      return;
    if ((double) this.timer > 0.0)
    {
      this.timer -= Time.deltaTime;
    }
    else
    {
      this.timer = this.interval;
      if (LocalizedTextManager.current_language.isRTL() != this.lastRTL)
      {
        this.lastRTL = LocalizedTextManager.current_language.isRTL();
        if (this.lastRTL)
        {
          this.population_value.alignment = (TextAnchor) 3;
          this.beasts_value.alignment = (TextAnchor) 3;
          this.infected_value.alignment = (TextAnchor) 3;
          this.deaths_value.alignment = (TextAnchor) 3;
          this.buildings_value.alignment = (TextAnchor) 3;
          this.vegetation_value.alignment = (TextAnchor) 3;
          this.population_name.alignment = (TextAnchor) 5;
          this.beasts_name.alignment = (TextAnchor) 5;
          this.infected_name.alignment = (TextAnchor) 5;
          this.deaths_name.alignment = (TextAnchor) 5;
          this.buildings_name.alignment = (TextAnchor) 5;
          this.vegetation_name.alignment = (TextAnchor) 5;
        }
        else
        {
          this.population_value.alignment = (TextAnchor) 5;
          this.beasts_value.alignment = (TextAnchor) 5;
          this.infected_value.alignment = (TextAnchor) 5;
          this.deaths_value.alignment = (TextAnchor) 5;
          this.buildings_value.alignment = (TextAnchor) 5;
          this.vegetation_value.alignment = (TextAnchor) 5;
          this.population_name.alignment = (TextAnchor) 3;
          this.beasts_name.alignment = (TextAnchor) 3;
          this.infected_name.alignment = (TextAnchor) 3;
          this.deaths_name.alignment = (TextAnchor) 3;
          this.buildings_name.alignment = (TextAnchor) 3;
          this.vegetation_name.alignment = (TextAnchor) 3;
        }
      }
      this.population_value.text = World.world.getCivWorldPopulation().ToString() ?? "";
      this.beasts_value.text = World.world.map_stats.current_mobs.ToString() ?? "";
      this.infected_value.text = World.world.map_stats.current_infected.ToString() ?? "";
      this.deaths_value.text = World.world.map_stats.deaths.ToString() ?? "";
      this.buildings_value.text = World.world.map_stats.current_houses.ToString() ?? "";
      this.vegetation_value.text = World.world.map_stats.current_vegetation.ToString() ?? "";
      ((Component) this.population_value).GetComponent<LocalizedText>().checkSpecialLanguages();
      ((Component) this.beasts_value).GetComponent<LocalizedText>().checkSpecialLanguages();
      ((Component) this.infected_value).GetComponent<LocalizedText>().checkSpecialLanguages();
      ((Component) this.deaths_value).GetComponent<LocalizedText>().checkSpecialLanguages();
      ((Component) this.buildings_value).GetComponent<LocalizedText>().checkSpecialLanguages();
      ((Component) this.vegetation_value).GetComponent<LocalizedText>().checkSpecialLanguages();
    }
  }
}

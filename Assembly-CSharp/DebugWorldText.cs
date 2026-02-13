// Decompiled with JetBrains decompiler
// Type: DebugWorldText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using FMOD.Studio;
using life.taxi;
using UnityEngine;

#nullable disable
public class DebugWorldText : MonoBehaviour
{
  public TextMesh text_mesh;
  public TextMesh text_mesh_bg_clone;
  private string _color_sounds_attached = "#FF1F44";
  private string _color_sounds = "#607BFF";
  private string _color_actors = "#FF8F44";
  private string _color_building = "#00FFFF";
  private string _color_city = "#A0FF93";
  private string _color_kingdom = "#FF4242";
  private string cur_string;
  private string cur_color;

  public void create()
  {
    ((Component) this.text_mesh_bg_clone).GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("Debug");
    ((Component) this.text_mesh_bg_clone).GetComponent<Renderer>().sortingOrder = 1;
    ((Component) this.text_mesh).GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("Debug");
    ((Component) this.text_mesh).GetComponent<Renderer>().sortingOrder = 2;
  }

  private void prepare(string pID, string pColor, float pSize = 0.25f)
  {
    this.text_mesh.color = Color.white;
    this.cur_string = pID;
    this.cur_color = $"<color={pColor}>";
    this.text_mesh_bg_clone.characterSize = pSize;
    this.text_mesh.characterSize = pSize;
  }

  private void add(string pTitle, object pText)
  {
    this.cur_string = $"{this.cur_string}{pTitle}: {this.cur_color}{pText?.ToString()}</color>\n";
  }

  public void setTextFmodSound(DebugMusicBoxData pData)
  {
    this.setTextFmodSound(pData, Color.white);
  }

  public void setTextFmodSound(DebugMusicBoxData pData, Color pColor)
  {
    float num = pData.timer / 3f;
    this.prepare("#fmod\n", this._color_sounds, 0.5f);
    this.cur_string = "mb:" + pData.path;
    Color color = pColor;
    color.a = num;
    this.fin();
    this.text_mesh.color = color;
    this.text_mesh_bg_clone.color = color;
  }

  public void setTextFmodSound(EventInstance pInstance)
  {
    EventDescription eventDescription;
    ((EventInstance) ref pInstance).getDescription(ref eventDescription);
    string pText;
    ((EventDescription) ref eventDescription).getPath(ref pText);
    this.prepare("#fmod\n", this._color_sounds_attached, 0.5f);
    this.add("name", (object) pText);
    this.fin();
  }

  public void setTextZone(TileZone pZone)
  {
    this.prepare("#zone\n", this._color_actors, 0.5f);
    foreach ((string, int) debugArg in pZone.debug_args)
      this.add(debugArg.Item1, (object) debugArg.Item2);
    this.fin();
  }

  public void setTextBoat(Actor pActor)
  {
    Boat simpleComponent = pActor.getSimpleComponent<Boat>();
    TaxiRequest taxiRequest = simpleComponent.taxi_request;
    if (simpleComponent.hasPassengers() || taxiRequest != null)
      this.prepare("#boat\n", this._color_kingdom, 0.8f);
    else
      this.prepare("#boat\n", this._color_actors, 0.4f);
    if (pActor.ai.job != null)
      this.add("job", (object) $"{pActor.ai.job.id}({pActor.ai.task_index.ToString()}/{pActor.ai.job.tasks.Count.ToString()})");
    if (pActor.hasTask())
    {
      string str = $" [{pActor.ai.action_index.ToString()}/{pActor.ai.task?.list.Count.ToString()}]";
      this.add("task", (object) $"{pActor.ai.task.id} {str}");
      string pText = pActor.ai.action?.GetType().ToString();
      if (pText != null)
        pText = pText.Replace("ai.behaviours.", "");
      this.add("action", (object) pText);
    }
    this.add("timer", (object) simpleComponent.actor.timer_action);
    this.fin();
  }

  private void debugForce(Actor pActor)
  {
    this.add("force xy", (object) $"{pActor.velocity.x.ToString()}-{pActor.velocity.y.ToString()}");
    this.add("force z", (object) pActor.velocity.z);
    this.add("zPosition", (object) pActor.position_height);
    this.add("force_speed", (object) pActor.velocity_speed);
    this.add("under_force", (object) pActor.under_forces);
    this.add("mass", (object) pActor.stats["mass"]);
  }

  public void setTextActor(Actor pActor)
  {
    this.prepare("#unit\n", this._color_actors, 0.2f);
    this.add("name", (object) pActor.data.name);
    this.add("timer_action", (object) pActor.timer_action);
    if (pActor.isCarryingResources())
    {
      this.add("inv.count", (object) pActor.inventory.countResources());
      this.add("inv.render", (object) pActor.inventory.getItemIDToRender());
    }
    this.add("stats", (object) pActor.asset.id);
    this.add("id", (object) pActor.data.id);
    this.add("alive", (object) pActor.isAlive());
    this.add("health", (object) $"{pActor.getHealth().ToString()}/{pActor.getMaxHealth().ToString()}");
    this.add("traits", (object) pActor.countTraits());
    if (pActor.hasAnyStatusEffect())
      this.add("statuses", (object) pActor.countStatusEffects());
    if (pActor.ai.job != null)
      this.add("job", (object) $"{pActor.ai.job.id}({pActor.ai.task_index.ToString()}/{pActor.ai.job.tasks.Count.ToString()})");
    if (pActor.hasTask())
    {
      this.add("task", (object) pActor.ai.task.id);
      string str = pActor.ai.action?.GetType().ToString();
      if (str != null)
        str = str.Replace("ai.behaviours.", "");
      this.add("action", (object) $"{str}{pActor.ai.action_index.ToString()}/{pActor.ai.task?.list.Count.ToString()}");
    }
    this.fin();
  }

  public void setTextArmy(Army pArmy)
  {
    this.prepare("#army\n", this._color_building, 0.3f);
    this.add("captain", (object) pArmy.getCaptain().getName());
    this.add("id", (object) pArmy.id);
    this.add("units", (object) pArmy.countUnits());
    this.add("alive", (object) pArmy.isAlive());
    if (pArmy.getCity().isAlive())
      this.add("city", (object) pArmy.getCity().name);
    else
      this.add("city", (object) "DESTROYED, SHOULD BE NULL");
    this.fin();
  }

  public void setTextBuilding(Building pObj)
  {
    this.prepare("#build\n", this._color_building, 0.3f);
    this.add("objectID", (object) pObj.data.id);
    this.add("state", (object) pObj.data.state);
    this.add("animationState", (object) pObj.animation_state);
    this.add("ownership", (object) pObj.state_ownership);
    this.add("kingdom", (object) pObj.kingdom.id);
    if (pObj.asset.hasHousingSlots())
      this.add("housing", (object) $"{pObj.countResidents().ToString()}/{pObj.asset.housing_slots.ToString()}");
    this.fin();
  }

  public void setTextCity(City pObj)
  {
    this.prepare("#city\n", this._color_city, 1.5f);
    bool flag = false;
    string pText1 = "";
    foreach (string key in pObj.buildings_dict_id.Keys)
    {
      if (!flag)
      {
        foreach (Building building in pObj.buildings_dict_id[key])
        {
          if (!building.isAlive())
          {
            flag = true;
            pText1 += "dead,";
          }
          if (building.asset.id != key)
          {
            flag = true;
            pText1 = $"{pText1}wrong stats {building.asset.id}";
          }
          if (flag)
            break;
        }
      }
      else
        break;
    }
    int pText2 = 0;
    foreach (Actor unit in pObj.units)
    {
      if (unit.isTask("put_out_fire"))
        ++pText2;
    }
    this.add("on_fire", (object) pObj.isCityUnderDangerFire());
    this.add("danger", (object) pObj.isInDanger());
    this.add("firemen", (object) pText2);
    string str1 = pObj.status.population.ToString();
    int num = pObj.getPopulationMaximum();
    string str2 = num.ToString();
    this.add("total", (object) $"{str1}/{str2}");
    this.add("units", (object) pObj.units.Count);
    this.add("buildings", (object) pObj.buildings.Count);
    this.add("orders_psbl", (object) pObj._debug_last_possible_build_orders);
    this.add("orders_no_res", (object) pObj._debug_last_possible_build_orders_no_resources);
    this.add("order_last", (object) pObj._debug_last_build_order_try);
    num = pObj.getHouseCurrent();
    string str3 = num.ToString();
    num = pObj.getHouseLimit();
    string str4 = num.ToString();
    this.add("house_zone_limit", (object) $"{str3}/{str4}");
    if (pObj.ai.job != null)
    {
      string[] strArray = new string[6]
      {
        pObj.ai.job.id,
        "(",
        pObj.ai.task_index.ToString(),
        "/",
        null,
        null
      };
      num = pObj.ai.job.tasks.Count;
      strArray[4] = num.ToString();
      strArray[5] = ")";
      this.add("job", (object) string.Concat(strArray));
    }
    if (pObj.ai.task != null)
      this.add("task", (object) pObj.ai.task.id);
    else
      this.add("task", (object) "-");
    if (flag)
      this.add("ERROR", (object) pText1);
    this.fin();
  }

  public void setTextCityTasks(City pCity)
  {
    this.prepare("#city_tasks\n", this._color_city, 0.5f);
    this.add("trees:", (object) pCity.tasks.trees);
    this.add("stone:", (object) pCity.tasks.minerals);
    this.add("minerals:", (object) pCity.tasks.minerals);
    this.add("bushes:", (object) pCity.tasks.bushes);
    this.add("plants:", (object) pCity.tasks.plants);
    this.add("hives:", (object) pCity.tasks.hives);
    this.add("farm_fields:", (object) pCity.tasks.farm_fields);
    this.add("wheats:", (object) pCity.tasks.wheats);
    this.add("ruins:", (object) pCity.tasks.ruins);
    this.add("poops:", (object) pCity.tasks.poops);
    this.add("roads:", (object) pCity.tasks.roads);
    this.add("fire:", (object) pCity.tasks.fire);
    this.add("", (object) "");
    int num1 = 0;
    int num2 = 0;
    foreach (CitizenJobAsset key in pCity.jobs.jobs.Keys)
    {
      int job = pCity.jobs.jobs[key];
      int num3 = 0;
      if (pCity.jobs.occupied.ContainsKey(key))
        num3 = pCity.jobs.occupied[key];
      num1 += job;
      num2 += num3;
      this.add(key.id + ":", (object) $"{num3.ToString()}/{job.ToString()}");
    }
    foreach (CitizenJobAsset key in pCity.jobs.occupied.Keys)
    {
      if (!pCity.jobs.jobs.ContainsKey(key))
      {
        int num4 = pCity.jobs.occupied[key];
        num2 += num4;
        this.add(key.id + ":", (object) $"{num4.ToString()}/{0.ToString()}");
      }
    }
    int num5 = 0;
    int num6 = 0;
    foreach (Actor unit in pCity.units)
    {
      if (unit.isAdult())
        ++num5;
      if (unit.hasTask() && unit.citizen_job != null)
        ++num6;
    }
    this.add("total:", (object) $"{num2.ToString()}/{num1.ToString()}");
    this.add("pop|adults|workers:", (object) $"{pCity.units.Count.ToString()} | {num5.ToString()} | {num6.ToString()}");
    this.fin();
  }

  public void setTextKingdom(Kingdom pObj)
  {
    this.prepare("#kingdom\n", this._color_kingdom, 2f);
    int num = pObj.getPopulationPeople();
    string str1 = num.ToString();
    num = pObj.getPopulationTotalPossible();
    string str2 = num.ToString();
    this.add("total", (object) $"{str1}/{str2}");
    this.add("units", (object) pObj.units.Count);
    this.add("buildings", (object) pObj.buildings.Count);
    this.add("timer_action", (object) pObj.timer_action);
    this.add("timer_new_king", (object) pObj.data.timer_new_king);
    if (pObj.ai.job != null)
      this.add("job", (object) $"{pObj.ai.job.id}({pObj.ai.task_index.ToString()}/{pObj.ai.job.tasks.Count.ToString()})");
    if (pObj.ai.task != null)
      this.add("task", (object) pObj.ai.task.id);
    else
      this.add("task", (object) "-");
    this.fin();
  }

  private void fin()
  {
    this.text_mesh.text = this.cur_string;
    this.text_mesh_bg_clone.text = this.cur_string;
  }
}

// Decompiled with JetBrains decompiler
// Type: Army
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using JetBrains.Annotations;
using System.Collections.Generic;

#nullable disable
public class Army : MetaObject<ArmyData>
{
  private Actor _captain;
  private WorldTile _prev_captain_position;
  private City _city;
  private Kingdom _kingdom;

  protected override MetaType meta_type => MetaType.Army;

  public override BaseSystemManager manager => (BaseSystemManager) World.world.armies;

  public override ActorAsset getActorAsset() => this.getKingdom().getActorAsset();

  public void createArmy(Actor pActor, City pCity)
  {
    this._city = pCity;
    this._kingdom = this._city.kingdom;
    this.setCaptain(pActor);
    this.generateNewMetaObject();
    this.generateName();
  }

  public void checkCity()
  {
    if (this._city.kingdom == this._kingdom)
      return;
    this._kingdom = this._city.kingdom;
    this.updateColor(this._kingdom?.getColor());
    this.generateName(this._kingdom);
  }

  public void onKingdomNameChange() => this.generateName();

  protected override void generateColor()
  {
    if (!this.isAlive())
      return;
    Kingdom kingdom = this.getKingdom();
    if (kingdom.isRekt())
      return;
    this.data.setColorID(kingdom.data.color_id);
  }

  public override void trackName(bool pPostChange = false)
  {
    if (string.IsNullOrEmpty(this.data.name) || pPostChange && (this.data.past_names == null || this.data.past_names.Count == 0))
      return;
    ArmyData data = this.data;
    if (data.past_names == null)
      data.past_names = new List<NameEntry>();
    if (this.data.past_names.Count == 0)
    {
      this.data.past_names.Add(new NameEntry(this.data.name, false, this.data.original_color_id, this.data.created_time));
    }
    else
    {
      if (this.data.past_names.Last<NameEntry>().name == this.data.name)
        return;
      this.data.past_names.Add(new NameEntry(this.data.name, this.data.custom_name, this.data.color_id));
    }
  }

  private void generateName(Kingdom pKingdom = null)
  {
    if (this.data.custom_name && !string.IsNullOrEmpty(this.data.name))
      return;
    Kingdom kingdom = pKingdom == null ? this.getKingdom() : pKingdom;
    if (kingdom == null)
    {
      this.setName("Forgotten Army");
    }
    else
    {
      string str = kingdom.name ?? "";
      string name = this.data.name;
      if ((name != null ? (name.StartsWith(str + " ") ? 1 : 0) : 0) != 0)
        return;
      using (ListPool<string> listPool = new ListPool<string>(World.world.armies.Count))
      {
        foreach (Army army in (CoreSystemManager<Army, ArmyData>) World.world.armies)
        {
          if (army != this && army.getKingdom() == kingdom)
            listPool.Add(army.name);
        }
        int pNumber = 1;
        string pName;
        while (true)
        {
          string roman = pNumber.ToRoman();
          pName = $"{str} {roman}";
          if (listPool.Contains(pName))
            ++pNumber;
          else
            break;
        }
        this.setName(pName);
      }
    }
  }

  public Actor getCaptain() => this._captain;

  public override void save()
  {
    base.save();
    ArmyData data1 = this.data;
    City city = this._city;
    long num1 = city != null ? city.id : -1L;
    data1.id_city = num1;
    ArmyData data2 = this.data;
    long? id = this._city?.kingdom?.id;
    long num2;
    if (!id.HasValue)
    {
      Kingdom kingdom = this._kingdom;
      num2 = kingdom != null ? kingdom.id : -1L;
    }
    else
      num2 = id.GetValueOrDefault();
    data2.id_kingdom = num2;
    this.data.id_captain = this.hasCaptain() ? this._captain.data.id : -1L;
  }

  public override void loadData(ArmyData pData)
  {
    base.loadData(pData);
    this._city = World.world.cities.get(pData.id_city);
    if (this._city != null)
      this._city.setArmy(this);
    this._kingdom = World.world.kingdoms.get(pData.id_kingdom);
    if (!string.IsNullOrEmpty(this.name))
      return;
    this.generateName();
  }

  public void loadDataCaptains()
  {
    Actor pActor = World.world.units.get(this.data.id_captain);
    if (pActor != null && pActor.army == this)
      this.setCaptain(pActor, true);
    this.updateColor(this.getColor());
  }

  public override void generateBanner()
  {
  }

  protected override ColorLibrary getColorLibrary()
  {
    return (ColorLibrary) AssetManager.kingdom_colors_library;
  }

  public override ColorAsset getColor() => this.getKingdom().getColor();

  public void clearCity()
  {
    this._city = (City) null;
    this.data.id_city = -1L;
  }

  public void disband()
  {
    for (int index = 0; index < this.units.Count; ++index)
      this.units[index].stopBeingWarrior();
    this.setCaptain((Actor) null);
  }

  public void updateCaptains()
  {
    if (this.data.past_captains == null || this.data.past_captains.Count == 0)
      return;
    foreach (LeaderEntry pastCaptain in this.data.past_captains)
    {
      Actor pObject = World.world.units.get(pastCaptain.id);
      if (!pObject.isRekt())
        pastCaptain.name = pObject.name;
    }
  }

  public void addCaptain(Actor pActor)
  {
    ArmyData data = this.data;
    if (data.past_captains == null)
      data.past_captains = new List<LeaderEntry>();
    this.captainLeft();
    List<LeaderEntry> pastCaptains = this.data.past_captains;
    LeaderEntry leaderEntry = new LeaderEntry();
    leaderEntry.id = pActor.getID();
    leaderEntry.name = pActor.name;
    Kingdom kingdom = this.getKingdom();
    leaderEntry.color_id = kingdom != null ? kingdom.data.color_id : this.data.color_id;
    leaderEntry.timestamp_ago = World.world.getCurWorldTime();
    pastCaptains.Add(leaderEntry);
    if (this.data.past_captains.Count <= 30)
      return;
    this.data.past_captains.Shift<LeaderEntry>();
  }

  public void captainLeft()
  {
    if (this.data.past_captains == null || this.data.past_captains.Count == 0)
      return;
    LeaderEntry leaderEntry = this.data.past_captains.Last<LeaderEntry>();
    if (leaderEntry.timestamp_end >= leaderEntry.timestamp_ago)
      return;
    leaderEntry.timestamp_end = World.world.getCurWorldTime();
    this.updateCaptains();
  }

  public void setCaptain(Actor pActor, bool pFromLoad = false)
  {
    this._captain = pActor;
    if (this.data == null)
      return;
    if (pActor.isRekt())
    {
      this.data.id_captain = -1L;
      if (pFromLoad)
        return;
      this.captainLeft();
    }
    else
    {
      this.data.id_captain = pActor.getID();
      if (pFromLoad)
        return;
      this.addCaptain(pActor);
    }
  }

  public void checkCaptainExistence()
  {
    Actor captain = this.getCaptain();
    if (!captain.isRekt() && captain.current_tile != null)
      this._prev_captain_position = captain.current_tile;
    if (captain.isRekt())
      this.setCaptain((Actor) null);
    this.findCaptain();
  }

  public void checkCaptainRemoval(Actor pActor)
  {
    if (this._captain != pActor)
      return;
    this.setCaptain((Actor) null);
  }

  public int countMelee()
  {
    int num = 0;
    for (int index = 0; index < this.units.Count; ++index)
    {
      Actor unit = this.units[index];
      if (unit.isAlive())
      {
        if (!unit.hasWeapon())
          ++num;
        else if (unit.getWeaponAsset().attack_type == WeaponType.Melee)
          ++num;
      }
    }
    return num;
  }

  public int countRange()
  {
    int num = 0;
    for (int index = 0; index < this.units.Count; ++index)
    {
      Actor unit = this.units[index];
      if (unit.isAlive() && unit.hasWeapon() && unit.getWeaponAsset().attack_type == WeaponType.Range)
        ++num;
    }
    return num;
  }

  public bool isGroupInCityAndHaveLeader()
  {
    if (!this.isAlive())
      return false;
    if (this.units.Count == 0)
      return true;
    if (!this.hasCaptain())
      return false;
    Actor captain = this.getCaptain();
    return !captain.isInsideSomething() && captain.current_zone.isSameCityHere(this._city);
  }

  private void findCaptain()
  {
    if (this.isLocked())
      return;
    if (this.hasCaptain())
    {
      if (this.getCaptain().isKingdomCiv())
        return;
      this.setCaptain((Actor) null);
    }
    if (this.units.Count == 0)
      return;
    Actor pActor = this._prev_captain_position != null ? this.getNearbyUnitForCaptain(this._prev_captain_position) : this.getRandomActorForCaptain();
    if (pActor == null)
      return;
    this.setCaptain(pActor);
  }

  private Actor getRandomActorForCaptain()
  {
    foreach (Actor pObject in this.units.LoopRandom<Actor>())
    {
      if (!pObject.isRekt() && pObject.army == this)
        return pObject;
    }
    return (Actor) null;
  }

  private Actor getNearbyUnitForCaptain(WorldTile pLastPosition)
  {
    Actor nearbyUnitForCaptain = (Actor) null;
    int num1 = int.MaxValue;
    List<Actor> units = this.units;
    for (int index = 0; index < units.Count; ++index)
    {
      Actor pObject = units[index];
      if (pObject.army == this && !pObject.isRekt())
      {
        int num2 = Toolbox.SquaredDistTile(pObject.current_tile, pLastPosition);
        if (num2 < num1)
        {
          nearbyUnitForCaptain = pObject;
          num1 = num2;
        }
      }
    }
    return nearbyUnitForCaptain;
  }

  public string getDebug()
  {
    int num = this.units.Count;
    string debug = num.ToString() ?? "";
    if (this._captain != null)
    {
      string[] strArray = new string[6]
      {
        debug,
        " ",
        this._captain.getName(),
        "(",
        null,
        null
      };
      num = this._captain.getAge();
      strArray[4] = num.ToString();
      strArray[5] = ")";
      debug = string.Concat(strArray);
    }
    return debug;
  }

  [CanBeNull]
  public Kingdom getKingdom()
  {
    Kingdom kingdom = (Kingdom) null;
    if (this.hasCaptain())
      kingdom = this.getCaptain().kingdom;
    if (kingdom == null)
      kingdom = this._city.isRekt() ? this._kingdom : this._city.kingdom;
    return kingdom;
  }

  public bool hasKingdom() => !this._kingdom.isRekt();

  public bool hasCaptain() => !this._captain.isRekt();

  public City getCity() => this._city;

  public bool hasCity() => !this._city.isRekt();

  public override bool isReadyForRemoval()
  {
    return this.units.Count <= 0 && !this.hasCaptain() && !this.hasCity() && base.isReadyForRemoval();
  }

  public override void Dispose()
  {
    base.Dispose();
    this.units.Clear();
    this._captain = (Actor) null;
    this._prev_captain_position = (WorldTile) null;
    this._city = (City) null;
    this._kingdom = (Kingdom) null;
  }

  public override string ToString()
  {
    if (this.data == null)
      return "[Army is null]";
    using (StringBuilderPool stringBuilderPool1 = new StringBuilderPool())
    {
      stringBuilderPool1.Append($"[Army:{this.id} ");
      if (!this.isAlive())
        stringBuilderPool1.Append("[DEAD] ");
      stringBuilderPool1.Append($"\"{this.name}\" ");
      Kingdom kingdom = this.getKingdom();
      stringBuilderPool1.Append($"Kingdom:{(kingdom != null ? kingdom.id : -1L)} ");
      if (this.hasCity())
        stringBuilderPool1.Append($"{this._city} ");
      if (kingdom != this._kingdom)
        stringBuilderPool1.Append($"_kingdom:{this._kingdom} ");
      if (this.hasCaptain())
      {
        StringBuilderPool stringBuilderPool2 = stringBuilderPool1;
        Actor captain = this._captain;
        string str = $"Captain:{(captain != null ? captain.id : -1L)} ";
        stringBuilderPool2.Append(str);
        if (this._captain?.kingdom != kingdom)
          stringBuilderPool1.Append($"CaptainKingdom:{this._captain?.kingdom?.id ?? -1L} ");
      }
      stringBuilderPool1.Append($"Units:{this.units.Count} ");
      if (this.manager.isUnitsDirty())
        stringBuilderPool1.Append("[Dirty] ");
      return stringBuilderPool1.ToString().Trim() + "]";
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: DebugTextGroupSystem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using FMOD;
using FMOD.Studio;
using UnityEngine;

#nullable disable
public class DebugTextGroupSystem : SpriteGroupSystem<GroupSpriteObject>
{
  private Vector2 _pos;

  public override void create()
  {
    base.create();
    ((Object) ((Component) this).transform).name = "Debug Text";
    this.prefab = ((GameObject) Resources.Load("Prefabs/PrefabDebugText")).GetComponent<GroupSpriteObject>();
  }

  protected override GroupSpriteObject createNew()
  {
    GroupSpriteObject groupSpriteObject = base.createNew();
    ((Component) groupSpriteObject).GetComponent<DebugWorldText>().create();
    return groupSpriteObject;
  }

  public override void update(float pElapsed)
  {
    this.prepare();
    this.checkSoundsAttached();
    this.checkSounds();
    this.checkSoundsPlaying();
    this.checkActors();
    this.checkBoats();
    this.checkBuildings();
    this.checkCitiesOverlay();
    this.checkCitiesTasksOverlay();
    this.checkKingdoms();
    this.checkArmies();
    this.checkZones();
    base.update(pElapsed);
  }

  private void checkSoundsPlaying()
  {
    if (!DebugConfig.isOn(DebugOption.OverlaySoundsActive) || MapBox.isRenderMiniMap())
      return;
    foreach (DebugMusicBoxData pData in MusicBox.inst.debug_box.list)
    {
      if (pData.isPlaying())
      {
        GroupSpriteObject next = this.getNext();
        this._pos.x = pData.x;
        this._pos.y = pData.y;
        ((Component) next).GetComponent<DebugWorldText>().setTextFmodSound(pData, Color.green);
        next.setPosOnly(ref this._pos);
      }
    }
  }

  private void checkSounds()
  {
    if (!DebugConfig.isOn(DebugOption.OverlaySounds) || MapBox.isRenderMiniMap())
      return;
    foreach (DebugMusicBoxData pData in MusicBox.inst.debug_box.list)
    {
      GroupSpriteObject next = this.getNext();
      this._pos.x = pData.x;
      this._pos.y = pData.y;
      ((Component) next).GetComponent<DebugWorldText>().setTextFmodSound(pData);
      next.setPosOnly(ref this._pos);
    }
  }

  private void checkSoundsAttached()
  {
    if (!DebugConfig.isOn(DebugOption.OverlaySoundsAttached) || MapBox.isRenderMiniMap())
      return;
    foreach (EventInstance pInstance in MusicBox.inst.idle.currentAttachedSounds.Values)
    {
      GroupSpriteObject next = this.getNext();
      ATTRIBUTES_3D attributes3D;
      ((EventInstance) ref pInstance).get3DAttributes(ref attributes3D);
      this._pos.x = attributes3D.position.x;
      this._pos.y = attributes3D.position.y;
      ((Component) next).GetComponent<DebugWorldText>().setTextFmodSound(pInstance);
      next.setPosOnly(ref this._pos);
    }
    foreach (QuantumSpriteAsset quantumSpriteAsset in AssetManager.quantum_sprites.list)
    {
      int num = quantumSpriteAsset.group_system.countActive();
      QuantumSprite[] all = quantumSpriteAsset.group_system.getAll();
      for (int index = 0; index < num; ++index)
      {
        QuantumSprite quantumSprite = all[index];
        if (((EventInstance) ref quantumSprite.fmod_instance).isValid())
        {
          ATTRIBUTES_3D attributes3D;
          ((EventInstance) ref quantumSprite.fmod_instance).get3DAttributes(ref attributes3D);
          this._pos.x = attributes3D.position.x;
          this._pos.y = attributes3D.position.y;
          GroupSpriteObject next = this.getNext();
          ((Component) next).GetComponent<DebugWorldText>().setTextFmodSound(quantumSprite.fmod_instance);
          next.setPosOnly(ref this._pos);
        }
      }
    }
    Actor[] array = World.world.units.visible_units.array;
    int count = World.world.units.visible_units.count;
    for (int index = 0; index < count; ++index)
    {
      Actor actor = array[index];
      if (actor.idle_loop_sound != null && ((EventInstance) ref actor.idle_loop_sound.fmod_instance).isValid())
      {
        ATTRIBUTES_3D attributes3D;
        ((EventInstance) ref actor.idle_loop_sound.fmod_instance).get3DAttributes(ref attributes3D);
        this._pos.x = attributes3D.position.x;
        this._pos.y = attributes3D.position.y;
        GroupSpriteObject next = this.getNext();
        ((Component) next).GetComponent<DebugWorldText>().setTextFmodSound(actor.idle_loop_sound.fmod_instance);
        next.setPosOnly(ref this._pos);
      }
    }
  }

  private void checkBoats()
  {
    if (!DebugConfig.isOn(DebugOption.OverlayBoatTransport))
      return;
    foreach (Actor unit in (SimSystemManager<Actor, ActorData>) World.world.units)
    {
      bool flag = false;
      if (unit.asset.is_boat)
        flag = true;
      if (flag)
      {
        GroupSpriteObject next = this.getNext();
        this._pos.x = unit.current_position.x;
        this._pos.y = unit.current_position.y;
        ((Component) next).GetComponent<DebugWorldText>().setTextBoat(unit);
        next.setPosOnly(ref this._pos);
      }
    }
  }

  private void checkActors()
  {
    if ((DebugConfig.isOn(DebugOption.OverlayActorCivs) || DebugConfig.isOn(DebugOption.OverlayCursorActor) || DebugConfig.isOn(DebugOption.OverlayActorGroupLeaderOnly) || DebugConfig.isOn(DebugOption.OverlayActorFavoritesOnly) ? 1 : (DebugConfig.isOn(DebugOption.OverlayActorMobs) ? 1 : 0)) == 0 || MapBox.isRenderMiniMap())
      return;
    Actor[] array = World.world.units.visible_units.array;
    int count = World.world.units.visible_units.count;
    for (int index = 0; index < count; ++index)
    {
      Actor pActor = array[index];
      bool flag = false;
      if (DebugConfig.isOn(DebugOption.OverlayCursorActor) && UnitSelectionEffect.last_actor == pActor)
        flag = true;
      if (DebugConfig.isOn(DebugOption.OverlayActorFavoritesOnly) && pActor.isFavorite())
        flag = true;
      if (DebugConfig.isOn(DebugOption.OverlayActorGroupLeaderOnly) && pActor.is_army_captain)
        flag = true;
      if (pActor.isSapient() && DebugConfig.isOn(DebugOption.OverlayActorCivs))
        flag = true;
      if (!pActor.isSapient() && DebugConfig.isOn(DebugOption.OverlayActorMobs))
        flag = true;
      if (flag)
      {
        GroupSpriteObject next = this.getNext();
        this._pos.x = pActor.current_position.x;
        this._pos.y = pActor.current_position.y;
        ((Component) next).GetComponent<DebugWorldText>().setTextActor(pActor);
        next.setPosOnly(ref this._pos);
      }
    }
  }

  private void checkBuildings()
  {
    if ((DebugConfig.isOn(DebugOption.OverlayTrees) || DebugConfig.isOn(DebugOption.OverlayPlants) || DebugConfig.isOn(DebugOption.OverlayCivBuildings) ? 1 : (DebugConfig.isOn(DebugOption.OverlayOtherBuildings) ? 1 : 0)) == 0 || MapBox.isRenderMiniMap())
      return;
    int num = World.world.buildings.countVisibleBuildings();
    Building[] visibleBuildings = World.world.buildings.getVisibleBuildings();
    for (int index = 0; index < num; ++index)
    {
      Building pObj = visibleBuildings[index];
      if (pObj.asset.city_building)
      {
        if (!DebugConfig.isOn(DebugOption.OverlayCivBuildings))
          continue;
      }
      else if (pObj.asset.building_type == BuildingType.Building_Tree)
      {
        if (!DebugConfig.isOn(DebugOption.OverlayTrees))
          continue;
      }
      else if (pObj.asset.building_type == BuildingType.Building_Plant)
      {
        if (!DebugConfig.isOn(DebugOption.OverlayPlants))
          continue;
      }
      else if (!DebugConfig.isOn(DebugOption.OverlayOtherBuildings))
        continue;
      GroupSpriteObject next = this.getNext();
      this._pos.x = pObj.current_position.x;
      this._pos.y = pObj.current_position.y;
      ((Component) next).GetComponent<DebugWorldText>().setTextBuilding(pObj);
      next.setPosOnly(ref this._pos);
    }
  }

  private void checkZones()
  {
    if (!DebugConfig.isOn(DebugOption.DebugZones))
      return;
    foreach (TileZone zone in World.world.zone_calculator.zones)
    {
      if (zone.debug_show)
      {
        GroupSpriteObject next = this.getNext();
        ref Vector2 local1 = ref this._pos;
        Vector2Int pos = zone.centerTile.pos;
        double x = (double) ((Vector2Int) ref pos).x;
        local1.x = (float) x;
        ref Vector2 local2 = ref this._pos;
        pos = zone.centerTile.pos;
        double y = (double) ((Vector2Int) ref pos).y;
        local2.y = (float) y;
        ((Component) next).GetComponent<DebugWorldText>().setTextZone(zone);
        next.setPosOnly(ref this._pos);
      }
    }
  }

  private void checkArmies()
  {
    if (!DebugConfig.isOn(DebugOption.OverlayArmies) || MapBox.isRenderMiniMap())
      return;
    foreach (Army army in (CoreSystemManager<Army, ArmyData>) World.world.armies)
    {
      if (army.hasCaptain())
      {
        Actor captain = army.getCaptain();
        GroupSpriteObject next = this.getNext();
        this._pos.x = captain.current_position.x;
        this._pos.y = captain.current_position.y;
        ((Component) next).GetComponent<DebugWorldText>().setTextArmy(army);
        next.setPosOnly(ref this._pos);
      }
    }
  }

  private void checkCitiesOverlay()
  {
    if (!DebugConfig.isOn(DebugOption.OverlayCity))
      return;
    foreach (City city in (CoreSystemManager<City, CityData>) World.world.cities)
    {
      GroupSpriteObject next = this.getNext();
      this._pos.x = city.city_center.x;
      this._pos.y = city.city_center.y;
      ((Component) next).GetComponent<DebugWorldText>().setTextCity(city);
      next.setPosOnly(ref this._pos);
    }
  }

  private void checkCitiesTasksOverlay()
  {
    if (!DebugConfig.isOn(DebugOption.OverlayCityTasks))
      return;
    foreach (City city in (CoreSystemManager<City, CityData>) World.world.cities)
    {
      GroupSpriteObject next = this.getNext();
      this._pos.x = city.city_center.x;
      this._pos.y = city.city_center.y;
      ((Component) next).GetComponent<DebugWorldText>().setTextCityTasks(city);
      next.setPosOnly(ref this._pos);
    }
  }

  private void checkKingdoms()
  {
    if (!DebugConfig.isOn(DebugOption.OverlayKingdom))
      return;
    foreach (Kingdom kingdom in (CoreSystemManager<Kingdom, KingdomData>) World.world.kingdoms)
    {
      if (kingdom.hasCapital())
      {
        GroupSpriteObject next = this.getNext();
        this._pos.x = kingdom.capital.city_center.x;
        this._pos.y = kingdom.capital.city_center.y;
        ((Component) next).GetComponent<DebugWorldText>().setTextKingdom(kingdom);
        next.setPosOnly(ref this._pos);
      }
    }
  }
}

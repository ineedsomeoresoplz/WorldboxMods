// Decompiled with JetBrains decompiler
// Type: DebugToolLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using life.taxi;
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityPools;

#nullable disable
public class DebugToolLibrary : AssetLibrary<DebugToolAsset>
{
  private UtilityBasedDecisionSystem _decision_system_debug;

  public override void init()
  {
    base.init();
    this.initBenchmarks();
    this.initMain();
    this.initGameplay();
    this.initMap();
    this.initAI();
    this.initCity();
    this.initSystems();
    this.initSubSystems();
    this.initFmod();
    this.initDiagnosticGameplay();
    this.initUI();
    this.initDebugConfigDefaults();
  }

  private void initDebugConfigDefaults()
  {
    foreach (string defaultDebugTool in DebugConfig.default_debug_tools)
    {
      DebugToolAsset debugToolAsset = this.get(defaultDebugTool);
      if (debugToolAsset != null)
        debugToolAsset.show_on_start = true;
    }
  }

  private void initDiagnosticGameplay()
  {
    DebugToolAsset pAsset1 = new DebugToolAsset();
    pAsset1.id = "hotkeys_nanoobjects";
    pAsset1.action_1 = (DebugToolAssetAction) (pTool =>
    {
      HotkeyTabsData hotkeyTabsData = World.world.hotkey_tabs_data;
      Dictionary<string, PlayerOptionData> dict = PlayerConfig.dict;
      pTool.setText("#map:", (object) "-------------");
      pTool.setText("hotkey_data_1:", (object) hotkeyTabsData.hotkey_data_1);
      pTool.setText("hotkey_data_2:", (object) hotkeyTabsData.hotkey_data_2);
      pTool.setText("hotkey_data_3:", (object) hotkeyTabsData.hotkey_data_3);
      pTool.setText("hotkey_data_4:", (object) hotkeyTabsData.hotkey_data_4);
      pTool.setText("hotkey_data_5:", (object) hotkeyTabsData.hotkey_data_5);
      pTool.setText("hotkey_data_6:", (object) hotkeyTabsData.hotkey_data_6);
      pTool.setText("hotkey_data_7:", (object) hotkeyTabsData.hotkey_data_7);
      pTool.setText("hotkey_data_8:", (object) hotkeyTabsData.hotkey_data_8);
      pTool.setText("hotkey_data_9:", (object) hotkeyTabsData.hotkey_data_9);
      pTool.setText("hotkey_data_0:", (object) hotkeyTabsData.hotkey_data_0);
      pTool.setText("#global_config:", (object) "-------------");
      pTool.setText("global_hotkey_1:", (object) dict["hotkey_1"].stringVal);
      pTool.setText("global_hotkey_2:", (object) dict["hotkey_2"].stringVal);
      pTool.setText("global_hotkey_3:", (object) dict["hotkey_3"].stringVal);
      pTool.setText("global_hotkey_4:", (object) dict["hotkey_4"].stringVal);
      pTool.setText("global_hotkey_5:", (object) dict["hotkey_5"].stringVal);
      pTool.setText("global_hotkey_6:", (object) dict["hotkey_6"].stringVal);
      pTool.setText("global_hotkey_7:", (object) dict["hotkey_7"].stringVal);
      pTool.setText("global_hotkey_8:", (object) dict["hotkey_8"].stringVal);
      pTool.setText("global_hotkey_9:", (object) dict["hotkey_9"].stringVal);
      pTool.setText("global_hotkey_0:", (object) dict["hotkey_0"].stringVal);
    });
    this.add(pAsset1);
    DebugToolAsset pAsset2 = new DebugToolAsset();
    pAsset2.id = "reproduction_diagnostic_cursor";
    pAsset2.action_1 = (DebugToolAssetAction) (pTool =>
    {
      WorldTile mouseTilePos = World.world.getMouseTilePos();
      if (mouseTilePos == null)
        return;
      if (Zones.showCityZones())
      {
        City city = mouseTilePos.zone.city;
        if (city == null)
          return;
        Subspecies mainSubspecies = city.getMainSubspecies();
        if (mainSubspecies == null)
          return;
        this.showReproductionDebugInfo(pTool, mainSubspecies);
      }
      else if (Zones.showKingdomZones())
      {
        City city = mouseTilePos.zone.city;
        if (city == null)
          return;
        Subspecies mainSubspecies = city.kingdom.getMainSubspecies();
        if (mainSubspecies == null)
          return;
        this.showReproductionDebugInfo(pTool, mainSubspecies);
      }
      else
      {
        if (!Zones.showSpeciesZones())
          return;
        ZoneMetaData zoneMetaData = ZoneMetaDataVisualizer.getZoneMetaData(mouseTilePos.zone);
        if (zoneMetaData.meta_object == null || !zoneMetaData.meta_object.isAlive())
          return;
        this.showReproductionDebugInfo(pTool, zoneMetaData.meta_object as Subspecies);
      }
    });
    this.add(pAsset2);
    DebugToolAsset pAsset3 = new DebugToolAsset();
    pAsset3.id = "reproduction_diagnostic_total";
    pAsset3.action_1 = (DebugToolAssetAction) (pTool =>
    {
      Dictionary<string, int> dictionary1 = UnsafeCollectionPool<Dictionary<string, int>, KeyValuePair<string, int>>.Get();
      Dictionary<string, int> dictionary2 = UnsafeCollectionPool<Dictionary<string, int>, KeyValuePair<string, int>>.Get();
      foreach (Subspecies subspecies in (CoreSystemManager<Subspecies, SubspeciesData>) World.world.subspecies)
      {
        foreach (RateCounter listCounter in subspecies.list_counters)
        {
          dictionary1[listCounter.id] = CollectionExtensions.GetValueOrDefault<string, int>((IReadOnlyDictionary<string, int>) dictionary1, listCounter.id) + listCounter.getTotal();
          dictionary2[listCounter.id] = CollectionExtensions.GetValueOrDefault<string, int>((IReadOnlyDictionary<string, int>) dictionary2, listCounter.id) + listCounter.getEventsPerMinute();
        }
      }
      foreach (KeyValuePair<string, int> keyValuePair in dictionary1)
        pTool.setText(keyValuePair.Key + ":", (object) $"{dictionary2[keyValuePair.Key]} | tot: {keyValuePair.Value}");
      UnsafeCollectionPool<Dictionary<string, int>, KeyValuePair<string, int>>.Release(dictionary1);
      UnsafeCollectionPool<Dictionary<string, int>, KeyValuePair<string, int>>.Release(dictionary2);
    });
    this.add(pAsset3);
  }

  private void showReproductionDebugInfo(DebugTool pTool, Subspecies pSubspecies)
  {
    pSubspecies.debugReproductionEvents(pTool);
  }

  private void initCity()
  {
    DebugToolAsset pAsset1 = new DebugToolAsset();
    pAsset1.id = "Cities";
    pAsset1.action_1 = (DebugToolAssetAction) (pTool =>
    {
      List<City> cityList = new List<City>((IEnumerable<City>) World.world.cities);
      cityList.Sort(new Comparison<City>(pTool.citySorter));
      foreach (City city in cityList)
      {
        if (pTool.textCount > 0)
          pTool.setSeparator();
        pTool.setText("#name:", (object) city.name);
        pTool.setText("pep:", (object) city.getPopulationPeople());
        pTool.setText("units:", (object) city.getUnitsTotal());
        pTool.setText("boats:", (object) city.countBoats());
        pTool.setText("zones:", (object) city.zones.Count);
        pTool.setText("buildings:", (object) city.buildings.Count);
        pTool.setText("city_center:", (object) city.city_center);
        if (pTool.textCount > 30)
        {
          pTool.setSeparator();
          pTool.setText("more...", (object) "...");
          break;
        }
      }
    });
    this.add(pAsset1);
    DebugToolAsset pAsset2 = new DebugToolAsset();
    pAsset2.id = "City Loyalty";
    pAsset2.action_1 = (DebugToolAssetAction) (pTool =>
    {
      List<City> cityList = new List<City>((IEnumerable<City>) World.world.cities);
      cityList.Sort(new Comparison<City>(pTool.citySorter));
      int pT2_1 = 0;
      int pT2_2 = 0;
      foreach (City city in cityList)
      {
        if (city.getCachedLoyalty() >= 0)
          ++pT2_1;
        else
          ++pT2_2;
      }
      pTool.setText("cities with loyalty above 0:", (object) pT2_1);
      pTool.setText("cities with loyalty below 0:", (object) pT2_2);
    });
    this.add(pAsset2);
    DebugToolAsset pAsset3 = new DebugToolAsset();
    pAsset3.id = "City Capture";
    pAsset3.action_1 = (DebugToolAssetAction) (pTool =>
    {
      WorldTile mouseTilePos = World.world.getMouseTilePos();
      if (mouseTilePos == null)
        return;
      City city = mouseTilePos.zone.city;
      if (city == null)
        return;
      if (city.being_captured_by != null && city.being_captured_by.isAlive())
        pTool.setText("capturing by:", (object) city.being_captured_by.name);
      pTool.setText("ticks:", (object) city.getCaptureTicks());
      city.debugCaptureUnits(pTool);
    });
    this.add(pAsset3);
    DebugToolAsset pAsset4 = new DebugToolAsset();
    pAsset4.id = "City Tasks";
    pAsset4.action_1 = (DebugToolAssetAction) (pTool =>
    {
      WorldTile mouseTilePos = World.world.getMouseTilePos();
      if (mouseTilePos == null)
        return;
      City city = mouseTilePos.zone.city;
      if (city == null)
        return;
      pTool.setText("trees:", (object) city.tasks.trees);
      pTool.setText("stone:", (object) city.tasks.minerals);
      pTool.setText("minerals:", (object) city.tasks.minerals);
      pTool.setText("bushes:", (object) city.tasks.bushes);
      pTool.setText("plants:", (object) city.tasks.plants);
      pTool.setText("hives:", (object) city.tasks.hives);
      pTool.setText("farm_fields:", (object) city.tasks.farm_fields);
      pTool.setText("wheats:", (object) city.tasks.wheats);
      pTool.setText("ruins:", (object) city.tasks.ruins);
      pTool.setText("poops:", (object) city.tasks.poops);
      pTool.setText("roads:", (object) city.tasks.roads);
      pTool.setText("fire:", (object) city.tasks.fire);
    });
    this.add(pAsset4);
    DebugToolAsset pAsset5 = new DebugToolAsset();
    pAsset5.id = "city_jobs";
    pAsset5.action_1 = (DebugToolAssetAction) (pTool =>
    {
      WorldTile mouseTilePos = World.world.getMouseTilePos();
      if (mouseTilePos == null)
        return;
      City city = mouseTilePos.zone.city;
      if (city == null)
        return;
      int num1 = 0;
      int num2 = 0;
      foreach (CitizenJobAsset key in city.jobs.jobs.Keys)
      {
        int job = city.jobs.jobs[key];
        int num3 = 0;
        if (city.jobs.occupied.ContainsKey(key))
          num3 = city.jobs.occupied[key];
        num1 += job;
        num2 += num3;
        pTool.setText(key.id + ":", (object) $"{num3.ToString()}/{job.ToString()}");
      }
      foreach (CitizenJobAsset key in city.jobs.occupied.Keys)
      {
        if (!city.jobs.jobs.ContainsKey(key))
        {
          int num4 = city.jobs.occupied[key];
          num2 += num4;
          pTool.setText(key.id + ":", (object) $"{num4.ToString()}/{0.ToString()}");
        }
      }
      pTool.setSeparator();
      pTool.setText("total JOBS:", (object) $"{num2.ToString()}/{num1.ToString()}");
      pTool.setSeparator();
      pTool.setText("pop:", (object) $"{city.getPopulationPeople().ToString()} / {city.getPopulationMaximum().ToString()}");
      pTool.setText("adults/children:", (object) $"{city.countAdults().ToString()}/{city.countChildren().ToString()}");
      pTool.setText("food:", (object) city.countFood());
      DebugTool debugTool = pTool;
      int num5 = city.countHungry();
      string str1 = num5.ToString();
      num5 = city.countStarving();
      string str2 = num5.ToString();
      string pT2 = $"{str1}/{str2}";
      debugTool.setText("hungry/starving:", (object) pT2);
    });
    this.add(pAsset5);
    DebugToolAsset pAsset6 = new DebugToolAsset();
    pAsset6.id = "City Info";
    pAsset6.action_1 = (DebugToolAssetAction) (pTool =>
    {
      WorldTile mouseTilePos = World.world.getMouseTilePos();
      if (mouseTilePos == null)
        return;
      City city = mouseTilePos.zone.city;
      if (city == null)
        return;
      pTool.setText("#name:", (object) city.name);
      pTool.setText("city units all:", (object) city.getUnitsTotal());
      pTool.setText("city people:", (object) city.getPopulationPeople());
      DebugTool debugTool = pTool;
      int num = city.getPopulationPeople();
      string str3 = num.ToString();
      num = city.getPopulationMaximum();
      string str4 = num.ToString();
      string pT2 = $"{str3}/{str4}";
      debugTool.setText("units:", (object) pT2);
      if (city.getPopulationMaximum() != city.status.housing_total)
        pTool.setText("unit housing:", (object) $"{city.getPopulationPeople().ToString()}/{city.status.housing_total.ToString()}");
      pTool.setText("in houses:", (object) city.countInHouses());
      pTool.setSeparator();
      if (city.hasLeader())
        pTool.setText("leader:", (object) city.leader.getName());
      if (city.hasKingdom())
        pTool.setText("kingdom:", (object) city.kingdom.name);
      if (city.hasKingdom())
        pTool.setText("#name:", (object) city.kingdom.id);
      pTool.setSeparator();
      pTool.setText("zones:", (object) city.zones.Count);
      pTool.setText("buildings:", (object) city.buildings.Count);
      pTool.setText("homes free:", (object) city.status.housing_free);
      pTool.setText("homes occupied:", (object) city.status.housing_occupied);
      pTool.setSeparator();
      pTool.setSeparator();
      pTool.setText("roads to build:", (object) city.road_tiles_to_build.Count);
      pTool.setSeparator();
      pTool.setSeparator();
    });
    this.add(pAsset6);
    DebugToolAsset pAsset7 = new DebugToolAsset();
    pAsset7.id = "city_storage";
    pAsset7.action_1 = (DebugToolAssetAction) (pTool =>
    {
      WorldTile mouseTilePos = World.world.getMouseTilePos();
      if (mouseTilePos == null)
        return;
      City city = mouseTilePos.zone.city;
      if (city == null || !city.hasStorages())
        return;
      for (int index = 0; index < city.storages.Count; ++index)
      {
        foreach (string key in city.storages[index].resources.getKeys())
          pTool.setText($"stock_{index.ToString()}:{key}:", (object) city.getResourcesAmount(key));
      }
    });
    this.add(pAsset7);
    DebugToolAsset pAsset8 = new DebugToolAsset();
    pAsset8.id = "City Buildings";
    pAsset8.action_1 = (DebugToolAssetAction) (pTool =>
    {
      WorldTile mouseTilePos = World.world.getMouseTilePos();
      if (mouseTilePos == null)
        return;
      City city = mouseTilePos.zone.city;
      if (city == null)
        return;
      pTool.setSeparator();
      int pT2_3 = 0;
      int pT2_4 = 0;
      pTool.setText("#type", (object) "");
      foreach (string key in city.buildings_dict_type.Keys)
      {
        pTool.setText(key + ":", (object) city.buildings_dict_type[key].Count);
        pT2_3 += city.buildings_dict_type[key].Count;
      }
      pTool.setSeparator();
      pTool.setText("#name", (object) "");
      foreach (string key in city.buildings_dict_id.Keys)
      {
        pTool.setText(key + ":", (object) city.buildings_dict_id[key].Count);
        pT2_4 += city.buildings_dict_id[key].Count;
      }
      pTool.setSeparator();
      pTool.setText("total:", (object) city.buildings.Count);
      pTool.setText("total by type:", (object) pT2_3);
      pTool.setText("total by name:", (object) pT2_4);
    });
    this.add(pAsset8);
    DebugToolAsset pAsset9 = new DebugToolAsset();
    pAsset9.id = "City Professions";
    pAsset9.action_1 = (DebugToolAssetAction) (pTool =>
    {
      WorldTile mouseTilePos = World.world.getMouseTilePos();
      if (mouseTilePos == null)
        return;
      City city = mouseTilePos.zone.city;
      if (city == null)
        return;
      pTool.setSeparator();
      pTool.setText("total:", (object) city.units.Count);
      pTool.setText("king:", (object) city.countProfession(UnitProfession.King));
      pTool.setText("leader:", (object) city.countProfession(UnitProfession.Leader));
      pTool.setText("units:", (object) city.countProfession(UnitProfession.Unit));
      pTool.setText("babies:", (object) city.countChildren());
      pTool.setText("warriors:", (object) city.countProfession(UnitProfession.Warrior));
      pTool.setText("null:", (object) city.countProfession(UnitProfession.Nothing));
    });
    this.add(pAsset9);
  }

  private void initSystems()
  {
    DebugToolAsset pAsset1 = new DebugToolAsset();
    pAsset1.id = "Effects";
    pAsset1.action_1 = (DebugToolAssetAction) (pTool =>
    {
      ExplosionChecker.debug(pTool);
      foreach (BaseEffectController effectController in World.world.stack_effects.list)
        effectController.debug(pTool);
    });
    this.add(pAsset1);
    DebugToolAsset pAsset2 = new DebugToolAsset();
    pAsset2.id = "Auto Tester";
    pAsset2.action_1 = (DebugToolAssetAction) (pTool =>
    {
      if (!Object.op_Inequality((Object) World.world.auto_tester, (Object) null))
        return;
      pTool.setText("active:", (object) World.world.auto_tester.active);
      pTool.setText("d_string:", (object) World.world.auto_tester.debugString);
      World.world.auto_tester.ai.debug(pTool);
    });
    this.add(pAsset2);
    DebugToolAsset pAsset3 = new DebugToolAsset();
    pAsset3.id = "Controllable Units";
    pAsset3.action_1 = (DebugToolAssetAction) (pTool =>
    {
      pTool.setText("isOverUI:", (object) World.world.isOverUI());
      pTool.setText("isGameplayControlsLocked:", (object) World.world.isGameplayControlsLocked());
      pTool.setText("controlsLocked:", (object) MapBox.controlsLocked());
      pTool.setSeparator();
      pTool.setText("IsselectedUnit:", (object) SelectedUnit.isSet());
      pTool.setText("Total Selected:", (object) SelectedUnit.getAllSelected().Count);
      pTool.setSeparator();
      pTool.setText("ControllableUnit:", (object) ControllableUnit.isControllingUnit());
      pTool.setText("Total Controlled:", (object) ControllableUnit.getCotrolledUnits().Count);
      pTool.setSeparator();
      pTool.setText("Square Selection:", (object) World.world.player_control.square_selection_started);
      pTool.setText("Square Selection Pos:", (object) World.world.player_control.square_selection_position_current);
    });
    this.add(pAsset3);
    DebugToolAsset pAsset4 = new DebugToolAsset();
    pAsset4.id = "Selected Unit";
    pAsset4.action_1 = (DebugToolAssetAction) (pTool =>
    {
      Actor unit = SelectedUnit.unit;
      if (unit == null)
        return;
      Actor actorNearCursor = World.world.getActorNearCursor();
      if (actorNearCursor == null)
        return;
      float objectTarget = unit.distanceToObjectTarget((BaseSimObject) actorNearCursor);
      pTool.setText("dist to target:", (object) objectTarget);
    });
    this.add(pAsset4);
    DebugToolAsset pAsset5 = new DebugToolAsset();
    pAsset5.id = "Window";
    pAsset5.action_1 = (DebugToolAssetAction) (pTool =>
    {
      ScrollWindow.debug(pTool);
      pTool.setSeparator();
      WindowHistory.debug(pTool);
    });
    this.add(pAsset5);
    DebugToolAsset pAsset6 = new DebugToolAsset();
    pAsset6.id = "Selected Meta";
    pAsset6.action_1 = (DebugToolAssetAction) (pTool => AssetManager.meta_type_library.debug(pTool));
    this.add(pAsset6);
    DebugToolAsset pAsset7 = new DebugToolAsset();
    pAsset7.id = "Camera";
    pAsset7.action_1 = (DebugToolAssetAction) (pTool =>
    {
      World.world.move_camera.debug(pTool);
      pTool.setSeparator();
      pTool.setText("zoom", (object) World.world.camera.orthographicSize);
      pTool.setText("aspect", (object) World.world.camera.aspect);
      pTool.setText("zoom_bound_mod", (object) World.world.quality_changer.getZoomRateBoundLow());
      pTool.setSeparator();
      DebugTool debugTool1 = pTool;
      int num1 = World.world.zone_camera.countVisibleZones();
      string str1 = num1.ToString();
      num1 = World.world.zone_calculator.zones.Count;
      string str2 = num1.ToString();
      string pT2_1 = $"{str1}/{str2}";
      debugTool1.setText("visible zones", (object) pT2_1);
      pTool.setText("Input.touchCount", (object) Input.touchCount);
      pTool.setText("origin_touch_dist", (object) World.world.player_control.getDistanceBetweenOriginAndCurrentTouch());
      DebugTool debugTool2 = pTool;
      float num2 = World.world.player_control.getCurrentDragDistance();
      string str3 = num2.ToString("F3");
      num2 = 0.007f;
      string str4 = num2.ToString();
      string pT2_2 = $"{str3} / {str4}";
      debugTool2.setText("getDebugDragThreshold", (object) pT2_2);
      pTool.setText("getDebugDragThreshold %", (object) ((World.world.player_control.getCurrentDragDistance() * 100f).ToString("F3") + "%"));
      pTool.setText("isTouchMoreThanDragThreshold %", (object) World.world.player_control.isTouchMoreThanDragThreshold());
      pTool.setText("already_used_camera_drag", (object) World.world.player_control.already_used_camera_drag);
      pTool.setText("inspect_timer_click", (object) World.world.player_control.inspect_timer_click);
      pTool.setText("touch_timer", (object) World.world.player_control.touch_ticks_skip);
      if (Input.touchCount > 0)
      {
        for (int index = 0; index < Input.touchCount; ++index)
        {
          DebugTool debugTool3 = pTool;
          string pT1_1 = $"Touch.fingerId[{index.ToString()}]";
          Touch touch = Input.GetTouch(index);
          // ISSUE: variable of a boxed type
          __Boxed<int> fingerId = (ValueType) ((Touch) ref touch).fingerId;
          debugTool3.setText(pT1_1, (object) fingerId);
          DebugTool debugTool4 = pTool;
          string pT1_2 = $"Touch.rawPosition[{index.ToString()}]";
          touch = Input.GetTouch(index);
          Vector2 rawPosition = (object) ((Touch) ref touch).rawPosition;
          debugTool4.setText(pT1_2, (object) rawPosition);
          DebugTool debugTool5 = pTool;
          string pT1_3 = $"Touch.pos[{index.ToString()}]";
          touch = Input.GetTouch(index);
          Vector2 position = (object) ((Touch) ref touch).position;
          debugTool5.setText(pT1_3, (object) position);
          DebugTool debugTool6 = pTool;
          string pT1_4 = $"Touch.dpos[{index.ToString()}]";
          touch = Input.GetTouch(index);
          Vector2 deltaPosition = (object) ((Touch) ref touch).deltaPosition;
          debugTool6.setText(pT1_4, (object) deltaPosition);
          DebugTool debugTool7 = pTool;
          string pT1_5 = $"Touch.delta[{index.ToString()}]";
          touch = Input.GetTouch(index);
          // ISSUE: variable of a boxed type
          __Boxed<float> deltaTime = (ValueType) ((Touch) ref touch).deltaTime;
          debugTool7.setText(pT1_5, (object) deltaTime);
          DebugTool debugTool8 = pTool;
          string pT1_6 = $"Touch.radius[{index.ToString()}]";
          touch = Input.GetTouch(index);
          // ISSUE: variable of a boxed type
          __Boxed<float> radius = (ValueType) ((Touch) ref touch).radius;
          debugTool8.setText(pT1_6, (object) radius);
          DebugTool debugTool9 = pTool;
          string pT1_7 = $"Touch.pressure[{index.ToString()}]";
          touch = Input.GetTouch(index);
          // ISSUE: variable of a boxed type
          __Boxed<float> pressure = (ValueType) ((Touch) ref touch).pressure;
          debugTool9.setText(pT1_7, (object) pressure);
        }
      }
      pTool.setText("Axis Vertical", (object) Input.GetAxis("Vertical"));
      pTool.setText("Axis Horizontal", (object) Input.GetAxis("Horizontal"));
      pTool.setText("Input.touchSupported", (object) Input.touchSupported);
      pTool.setText("Input.touchPressureSupported", (object) Input.touchPressureSupported);
      pTool.setText("Input.multiTouchEnabled", (object) Input.multiTouchEnabled);
      pTool.setText("Input.stylusTouchSupported", (object) Input.stylusTouchSupported);
      pTool.setText("Input.simulateMouseWithTouches", (object) Input.simulateMouseWithTouches);
      pTool.setText("Input.mousePresent", (object) Input.mousePresent);
      pTool.setText("Input.mousePosition", (object) Input.mousePosition);
      pTool.setText("Input.mouseScrollDelta", (object) Input.mouseScrollDelta);
      pTool.setText("Button 0", (object) Input.GetMouseButton(0));
      pTool.setText("Button 1", (object) Input.GetMouseButton(1));
      pTool.setText("Button 2", (object) Input.GetMouseButton(2));
      pTool.setText("Axis ScrollWheel", (object) Input.mouseScrollDelta.y);
      pTool.setText("Axis Mouse X", (object) Input.GetAxis("Mouse X"));
      pTool.setText("Axis Mouse Y", (object) Input.GetAxis("Mouse Y"));
      pTool.setText("Raw Mouse X", (object) Input.GetAxisRaw("Mouse X"));
      pTool.setText("Raw Mouse Y", (object) Input.GetAxisRaw("Mouse Y"));
      pTool.setText("Velocity", (object) World.world.move_camera.getVelocity());
    });
    this.add(pAsset7);
  }

  private void initMap()
  {
    DebugToolAsset pAsset1 = new DebugToolAsset();
    pAsset1.id = "tile_info";
    pAsset1.action_1 = (DebugToolAssetAction) (pTool =>
    {
      WorldTile mouseTilePos = World.world.getMouseTilePos();
      if (mouseTilePos == null)
        return;
      pTool.setText("x", (object) mouseTilePos.x);
      pTool.setText("y", (object) mouseTilePos.y);
      pTool.setText("id", (object) mouseTilePos.data.tile_id);
      pTool.setText("height", (object) mouseTilePos.data.height);
      pTool.setText("type", (object) mouseTilePos.Type.id);
      pTool.setText("layer", (object) mouseTilePos.Type.layer_type);
      pTool.setText("main tile", mouseTilePos.main_type != null ? (object) mouseTilePos.main_type.id : (object) "-");
      pTool.setText("cap tile", mouseTilePos.top_type != null ? (object) mouseTilePos.top_type.id : (object) "-");
      pTool.setText("burned", (object) mouseTilePos.burned_stages);
      pTool.setText("targetedBy", (object) mouseTilePos.isTargeted());
      pTool.setText("units", (object) mouseTilePos.countUnits());
      pTool.setText("good_for_boat", (object) mouseTilePos.isGoodForBoat());
      pTool.setText("heat", (object) mouseTilePos.heat);
      pTool.setSeparator();
      pTool.setText("--zone:", (object) "");
      TileZone zone = mouseTilePos.zone;
      if (zone.hasAnyBuildingsInSet(BuildingList.Civs))
        pTool.setText("buildings:", (object) zone.getHashset(BuildingList.Civs).Count);
      if (zone.hasAnyBuildingsInSet(BuildingList.Ruins))
        pTool.setText("ruins:", (object) zone.getHashset(BuildingList.Ruins).Count);
      if (zone.hasAnyBuildingsInSet(BuildingList.Trees))
        pTool.setText("trees:", (object) zone.getHashset(BuildingList.Trees).Count);
      if (zone.hasAnyBuildingsInSet(BuildingList.Minerals))
        pTool.setText("stone:", (object) zone.getHashset(BuildingList.Minerals).Count);
      if (zone.hasAnyBuildingsInSet(BuildingList.Food))
        pTool.setText("fruits:", (object) zone.getHashset(BuildingList.Food).Count);
      if (zone.hasAnyBuildingsInSet(BuildingList.Hives))
        pTool.setText("hives:", (object) zone.getHashset(BuildingList.Hives).Count);
      if (zone.hasAnyBuildingsInSet(BuildingList.Poops))
        pTool.setText("poops:", (object) zone.getHashset(BuildingList.Poops).Count);
      if (zone.isZoneOnFire())
        pTool.setText("fire:", (object) WorldBehaviourActionFire.countFires(zone));
      if (zone.tiles_with_liquid > 0)
        pTool.setText("water tiles:", (object) zone.tiles_with_liquid);
      if (zone.tiles_with_ground > 0)
        pTool.setText("ground tiles:", (object) zone.tiles_with_ground);
      if (zone.city != null)
        pTool.setText("city:", (object) zone.city.name);
      if (zone.city != null && zone.city.kingdom != null)
        pTool.setText("kingdom:", (object) zone.city.kingdom.name);
      if (!mouseTilePos.hasBuilding())
        return;
      pTool.setSeparator();
      pTool.setText("--building:", (object) "");
      pTool.setText("resources:", (object) mouseTilePos.building.hasResourcesToCollect());
      pTool.setText("alive:", (object) mouseTilePos.building.isAlive());
      pTool.setText("is_usable:", (object) mouseTilePos.building.isUsable());
      pTool.setText("city:", mouseTilePos.building.city != null ? (object) mouseTilePos.building.city.name : (object) "-");
      pTool.setText("kingdom:", mouseTilePos.building.city?.kingdom != null ? (object) mouseTilePos.building.city.kingdom.name : (object) "-");
    });
    this.add(pAsset1);
    DebugToolAsset pAsset2 = new DebugToolAsset();
    pAsset2.id = "Connections";
    pAsset2.action_1 = (DebugToolAssetAction) (pTool => RegionLinkHashes.debug(pTool));
    this.add(pAsset2);
    DebugToolAsset pAsset3 = new DebugToolAsset();
    pAsset3.id = "Region";
    pAsset3.action_1 = (DebugToolAssetAction) (pTool =>
    {
      WorldTile mouseTilePos1 = World.world.getMouseTilePos();
      if (mouseTilePos1 == null || mouseTilePos1.region == null)
        return;
      WorldTile mouseTilePos2 = World.world.getMouseTilePos();
      if (mouseTilePos2 == null)
        return;
      MapRegion region = mouseTilePos2.region;
      if (region == null)
        return;
      bool pT2_1 = false;
      string pT2_2 = "";
      foreach (MapRegion neighbour in region.neighbours)
      {
        if (neighbour.tiles.Count == 0)
        {
          pT2_1 = true;
          pT2_2 = neighbour.id.ToString();
          break;
        }
      }
      pTool.setText("- region id:", (object) region.id);
      pTool.setText("-chunk id:", (object) region.chunk.id);
      pTool.setText("-chunk xy:", (object) $"{region.chunk.x.ToString()} {region.chunk.y.ToString()}");
      pTool.setText("- tEmptyRegionNeighbour:", (object) pT2_1);
      if (pT2_1)
        pTool.setText("- tEmptyRegionNeighbourID:", (object) pT2_2);
      pTool.setText("- getEdgeTiles :", (object) region.getEdgeTiles().Count);
      pTool.setText("- used in path :", (object) $"{region.used_by_path_lock.ToString()} {region.region_path_id.ToString()}");
      pTool.setText("- region wave:", (object) region.path_wave_id);
      pTool.setText("- centerRegion:", (object) region.center_region);
      pTool.setText("- region tiles:", (object) region.tiles.Count);
      pTool.setText("- region neigbours:", (object) region.neighbours.Count);
      pTool.setText("- created:", (object) region.created);
      pTool.setText("- island:", (object) (region.island == null));
      pTool.setText("- getEdgeRegions:", (object) region.getEdgeRegions().Count);
      pTool.setText("- island connections:", (object) region.island.getConnectedIslands().Count);
      pTool.setText("- debug_connections_left:", (object) region.debug_blink_edges_left?.Count);
      pTool.setText("- debug_connections_right:", (object) region.debug_blink_edges_right?.Count);
      pTool.setText("- debug_connections_up:", (object) region.debug_blink_edges_up?.Count);
      pTool.setText("- debug_connections_down:", (object) region.debug_blink_edges_down?.Count);
      mouseTilePos2.region.debugLinks(pTool);
    });
    this.add(pAsset3);
    DebugToolAsset pAsset4 = new DebugToolAsset();
    pAsset4.id = "Zone Info";
    pAsset4.action_1 = (DebugToolAssetAction) (pTool =>
    {
      World.world.zone_calculator.debug(pTool);
      WorldTile mouseTilePos = World.world.getMouseTilePos();
      if (mouseTilePos == null)
        return;
      MapChunk chunk = mouseTilePos.chunk;
      pTool.setText("visible:", (object) mouseTilePos.zone.visible);
      pTool.setText("buildings:", (object) mouseTilePos.zone.getHashset(BuildingList.Civs)?.Count);
      pTool.setText("types:", (object) mouseTilePos.zone.countNotNullTypes());
      pTool.setSeparator();
      pTool.setText("id:", (object) mouseTilePos.zone.id);
      pTool.setText("pos:", (object) $"x: {mouseTilePos.zone.x.ToString()}, y: {mouseTilePos.zone.y.ToString()}");
      pTool.setText("city:", (object) mouseTilePos.zone.hasCity());
      pTool.setText("bushes:", (object) mouseTilePos.zone.getHashset(BuildingList.Food)?.Count);
      pTool.setText("hives:", (object) mouseTilePos.zone.getHashset(BuildingList.Hives)?.Count);
      pTool.setText("trees:", (object) mouseTilePos.zone.getHashset(BuildingList.Trees)?.Count);
      pTool.setText("poops:", (object) mouseTilePos.zone.getHashset(BuildingList.Poops)?.Count);
      pTool.setText("deposits:", (object) mouseTilePos.zone.getHashset(BuildingList.Minerals)?.Count);
      pTool.setText("flore:", (object) mouseTilePos.zone.getHashset(BuildingList.Flora)?.Count);
      pTool.setText("buildings:", (object) mouseTilePos.zone.getHashset(BuildingList.Civs)?.Count);
      pTool.setText("buildings all:", (object) mouseTilePos.zone.buildings_all.Count);
      pTool.setText("abandoned:", (object) mouseTilePos.zone.getHashset(BuildingList.Abandoned)?.Count);
      pTool.setText("ruins:", (object) mouseTilePos.zone.getHashset(BuildingList.Ruins)?.Count);
      pTool.setText("tilesWithGround:", (object) mouseTilePos.zone.tiles_with_ground);
      pTool.setText("count deep ocean:", (object) mouseTilePos.zone.getTilesOfType((TileTypeBase) TileLibrary.deep_ocean)?.Count);
      pTool.setText("count soil:", (object) mouseTilePos.zone.getTilesOfType((TileTypeBase) TileLibrary.soil_low)?.Count);
      pTool.setText("count fuse:", (object) mouseTilePos.zone.getTilesOfType((TileTypeBase) TopTileLibrary.fuse)?.Count);
    });
    this.add(pAsset4);
    DebugToolAsset pAsset5 = new DebugToolAsset();
    pAsset5.id = "map_chunks";
    pAsset5.action_1 = (DebugToolAssetAction) (pTool =>
    {
      int length = World.world.map_chunk_manager.chunks.Length;
      if (length < 1)
        return;
      int pT2_3 = int.MaxValue;
      int pT2_4 = 0;
      int pT2_5 = 0;
      int pT2_6 = int.MaxValue;
      int pT2_7 = 0;
      int pT2_8 = 0;
      foreach (MapChunk chunk in World.world.map_chunk_manager.chunks)
      {
        int count1 = chunk.objects.kingdoms.Count;
        if (count1 < pT2_3)
          pT2_3 = count1;
        if (count1 > pT2_4)
          pT2_4 = count1;
        pT2_5 += count1;
        foreach (List<Actor> debugUnit in chunk.objects.getDebugUnits())
        {
          int count2 = debugUnit.Count;
          if (count2 < pT2_6)
            pT2_6 = count2;
          if (count2 > pT2_7)
            pT2_7 = count2;
          pT2_8 += count2;
        }
        foreach (List<Building> debugBuilding in chunk.objects.getDebugBuildings())
        {
          int count3 = debugBuilding.Count;
          if (count3 < pT2_6)
            pT2_6 = count3;
          if (count3 > pT2_7)
            pT2_7 = count3;
          pT2_8 += count3;
        }
      }
      pTool.setText("batches:", (object) DebugConfig.isOn(DebugOption.ChunkBatches));
      pTool.setText("debug_batch_size:", (object) ParallelHelper.DEBUG_BATCH_SIZE);
      pTool.setText("chunks:", (object) length);
      pTool.setText("objects:", (object) pT2_5);
      pTool.setText("objects min:", (object) pT2_3);
      pTool.setText("objects max:", (object) pT2_4);
      pTool.setText("objects avg:", (object) (pT2_5 / length));
      pTool.setSeparator();
      pTool.setText("kingdom objects:", (object) pT2_8);
      pTool.setText("kingdom objects min:", (object) pT2_6);
      pTool.setText("kingdom objects max:", (object) pT2_7);
      pTool.setText("kingdom objects avg:", (object) (pT2_8 / length));
    });
    this.add(pAsset5);
    DebugToolAsset pAsset6 = new DebugToolAsset();
    pAsset6.id = "Map Chunk";
    pAsset6.action_1 = (DebugToolAssetAction) (pTool =>
    {
      WorldTile mouseTilePos = World.world.getMouseTilePos();
      if (mouseTilePos == null)
        return;
      MapChunk chunk = mouseTilePos.chunk;
      pTool.setText("chunk_id:", (object) chunk.id);
      pTool.setText("chunk_x/y:", (object) $"{chunk.x.ToString()}/{chunk.y.ToString()}");
      pTool.setSeparator();
      pTool.setText("kingdoms:", (object) chunk.objects.kingdoms.Count);
      pTool.setSeparator();
      pTool.setSeparator();
      pTool.setText("total_units:", (object) chunk.objects.total_units);
      pTool.setText("total_buildings:", (object) chunk.objects.total_buildings);
      pTool.setSeparator();
      pTool.setText("total:", (object) (chunk.objects.total_units + chunk.objects.total_buildings));
    });
    this.add(pAsset6);
    DebugToolAsset pAsset7 = new DebugToolAsset();
    pAsset7.id = "Island Info";
    pAsset7.action_1 = (DebugToolAssetAction) (pTool =>
    {
      WorldTile mouseTilePos = World.world.getMouseTilePos();
      if (mouseTilePos == null || mouseTilePos.region == null)
        return;
      TileIsland island = mouseTilePos.region.island;
      if (island == null)
        return;
      pTool.setText("islands:", (object) World.world.islands_calculator.islands.Count);
      pTool.setText("regions:", (object) island.regions.Count);
      pTool.setSeparator();
      pTool.setText("id:", (object) island.id);
      pTool.setText("hash:", (object) island.debug_hash_code);
      pTool.setText("tiles:", (object) island.getTileCount());
      pTool.setText("unit limit:", (object) (island.regions.Count * 4));
      pTool.setText("created:", (object) island.created);
      pTool.setText("type:", (object) island.type);
      pTool.setText("docks:", (object) island.docks?.Count);
    });
    this.add(pAsset7);
    DebugToolAsset pAsset8 = new DebugToolAsset();
    pAsset8.id = "Tilemap Renderer";
    pAsset8.action_1 = (DebugToolAssetAction) (pTool => World.world.tilemap.debug(pTool));
    this.add(pAsset8);
  }

  private void initSubSystems()
  {
    DebugToolAsset pAsset1 = new DebugToolAsset();
    pAsset1.id = "boat";
    pAsset1.action_1 = (DebugToolAssetAction) (pTool =>
    {
      WorldTile mouseTilePos = World.world.getMouseTilePos();
      if (mouseTilePos == null)
        return;
      Actor actor = (Actor) null;
      int num1 = int.MaxValue;
      foreach (Actor unit in (SimSystemManager<Actor, ActorData>) World.world.units)
      {
        int num2 = Toolbox.SquaredDistTile(unit.current_tile, mouseTilePos);
        if (unit.asset.is_boat && num2 < num1)
        {
          actor = unit;
          num1 = num2;
        }
      }
      if (actor == null)
        return;
      Boat simpleComponent = actor.getSimpleComponent<Boat>();
      pTool.setSeparator();
      pTool.setText("units:", (object) simpleComponent.countPassengers());
      pTool.setText("passengerWaitCounter:", (object) simpleComponent.passengerWaitCounter);
    });
    this.add(pAsset1);
    DebugToolAsset pAsset2 = new DebugToolAsset();
    pAsset2.id = "taxi";
    pAsset2.action_1 = (DebugToolAssetAction) (pTool =>
    {
      pTool.setText("requests:", (object) TaxiManager.list.Count);
      pTool.setSeparator();
      TaxiManager.list.Sort((Comparison<TaxiRequest>) ((a, b) => b.countActors().CompareTo(a.countActors())));
      TaxiManager.list.ForEach((Action<TaxiRequest>) (tRequest =>
      {
        int num = 0;
        if (tRequest.hasAssignedBoat())
          num = tRequest.getBoat().countPassengers();
        pTool.setText("state", (object) $"{tRequest.state.ToString()} {num.ToString()}/{tRequest.countActors().ToString()} | {tRequest.hasAssignedBoat().ToString()}");
      }));
    });
    this.add(pAsset2);
  }

  private void initGameplay()
  {
    DebugToolAsset pAsset1 = new DebugToolAsset();
    pAsset1.id = "World Laws";
    pAsset1.action_1 = (DebugToolAssetAction) (pTool =>
    {
      foreach (WorldLawAsset worldLawAsset in AssetManager.world_laws_library.list)
      {
        DebugTool debugTool = pTool;
        string id = worldLawAsset.id;
        bool flag = worldLawAsset.isEnabled();
        string str1 = flag.ToString();
        flag = worldLawAsset.isEnabledRaw();
        string str2 = flag.ToString();
        string pT2 = $"{str1} : {str2}";
        debugTool.setText(id, (object) pT2);
      }
      pTool.setSeparator();
    });
    this.add(pAsset1);
    DebugToolAsset pAsset2 = new DebugToolAsset();
    pAsset2.id = "Building Manager";
    pAsset2.action_1 = (DebugToolAssetAction) (pTool =>
    {
      pTool.setText("buildings:", (object) World.world.buildings.Count);
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      foreach (Building building in (SimSystemManager<Building, BuildingData>) World.world.buildings)
      {
        if (building.is_visible)
          ++num1;
        if (building.scale_helper.active)
          ++num3;
      }
      pTool.setText("visible:", (object) $"{num1.ToString()}/{World.world.buildings.Count.ToString()}");
      pTool.setText("tweens:", (object) $"{num2.ToString()}/{World.world.buildings.Count.ToString()}");
      pTool.setText("tween_active:", (object) $"{num3.ToString()}/{World.world.buildings.Count.ToString()}");
      pTool.setSeparator();
    });
    this.add(pAsset2);
    DebugToolAsset pAsset3 = new DebugToolAsset();
    pAsset3.id = "Cultures";
    pAsset3.action_1 = (DebugToolAssetAction) (pTool =>
    {
      pTool.setText("cultures:", (object) World.world.cultures.Count);
      foreach (Culture culture in (CoreSystemManager<Culture, CultureData>) World.world.cultures)
        culture.debug(pTool);
    });
    this.add(pAsset3);
    DebugToolAsset pAsset4 = new DebugToolAsset();
    pAsset4.id = "Tile Types";
    pAsset4.action_1 = (DebugToolAssetAction) (pTool =>
    {
      pTool.setText("tumor_low", (object) TopTileLibrary.tumor_low.hashset.Count);
      pTool.setText("tumor_high", (object) TopTileLibrary.tumor_high.hashset.Count);
      pTool.setText("biomass_low", (object) TopTileLibrary.biomass_low.hashset.Count);
      pTool.setText("biomass_high", (object) TopTileLibrary.biomass_high.hashset.Count);
      pTool.setText("pumpkin_low", (object) TopTileLibrary.pumpkin_low.hashset.Count);
      pTool.setText("pumpkin_high", (object) TopTileLibrary.pumpkin_high.hashset.Count);
      pTool.setText("cybertile_low", (object) TopTileLibrary.cybertile_low.hashset.Count);
      pTool.setText("cybertile_high", (object) TopTileLibrary.cybertile_high.hashset.Count);
      pTool.setText("deep_ocean", (object) TileLibrary.deep_ocean.hashset.Count);
      pTool.setText("pit_deep_ocean", (object) TileLibrary.pit_deep_ocean.hashset.Count);
    });
    this.add(pAsset4);
    DebugToolAsset pAsset5 = new DebugToolAsset();
    pAsset5.id = "Jobs Buildings";
    pAsset5.action_1 = (DebugToolAssetAction) (pTool => World.world.buildings.debugJobManager(pTool));
    this.add(pAsset5);
    DebugToolAsset pAsset6 = new DebugToolAsset();
    pAsset6.id = "Jobs Actors";
    pAsset6.action_1 = (DebugToolAssetAction) (pTool => World.world.units.debugJobManager(pTool));
    this.add(pAsset6);
    DebugToolAsset pAsset7 = new DebugToolAsset();
    pAsset7.id = "Building Info";
    pAsset7.action_1 = (DebugToolAssetAction) (pTool =>
    {
      WorldTile mouseTilePos = World.world.getMouseTilePos();
      if (mouseTilePos == null)
        return;
      Building building = mouseTilePos.building;
      if (building == null)
        return;
      if (building.asset.docks)
      {
        pTool.setText("boats_fishing:", (object) building.component_docks.countBoatTypes("boat_type_fishing"));
        pTool.setText("boats_transport:", (object) building.component_docks.countBoatTypes("boat_type_transport"));
        pTool.setText("boats_trading:", (object) building.component_docks.countBoatTypes("boat_type_trading"));
      }
      pTool.setText("id:", (object) building.data.id);
      pTool.setText("hash:", (object) building.GetHashCode());
      pTool.setText("animData_index:", (object) building.animData_index);
      pTool.setText("residents:", (object) $"{building.countResidents().ToString()}/{building.asset.housing_slots.ToString()}");
      pTool.setText("kingdom:", (object) building.kingdom.id);
      pTool.setText("kingdom civ:", (object) building.isKingdomCiv());
      pTool.setText("animationState:", (object) building.animation_state);
      pTool.setText("ownership:", (object) building.state_ownership);
      pTool.setText("state:", (object) building.data.state);
      pTool.setText("template:", (object) building.data.asset_id);
      pTool.setText("health:", (object) building.getHealth());
      pTool.setText("health cur:", (object) building.getMaxHealth());
      if (building.hasKingdom())
        pTool.setText("kingdom:", (object) building.kingdom.name);
      pTool.setSeparator();
      pTool.setText("tiles:", (object) building.tiles.Count);
      pTool.setText("zones:", (object) building.zones.Count);
      pTool.setSeparator();
      pTool.setText("alive:", (object) building.isAlive());
      pTool.setText("usable:", (object) building.isUsable());
      pTool.setText("under construction:", (object) building.isUnderConstruction());
      pTool.setText("progress:", (object) building.getConstructionProgress());
      if (building.city != null)
        pTool.setText("city:", (object) building.city.name);
      pTool.setSeparator();
      pTool.setText("tween_active:", (object) building.scale_helper.active);
      pTool.setSeparator();
      pTool.setText("state:", (object) building.animation_state);
      pTool.setText("has_resources:", (object) building.hasResourcesToCollect());
      pTool.setText("is_visible:", (object) building.is_visible);
      pTool.setText("scale_start:", (object) building.scale_helper.scale_start);
      pTool.setText("currentScale.y:", (object) building.current_scale.y);
      pTool.setSeparator();
      pTool.setText("flip.x:", (object) building.flip_x);
    });
    this.add(pAsset7);
    DebugToolAsset pAsset8 = new DebugToolAsset();
    pAsset8.id = "Debug Buildings Render";
    pAsset8.action_1 = (DebugToolAssetAction) (pTool =>
    {
      WorldTile mouseTilePos = World.world.getMouseTilePos();
      if (mouseTilePos == null)
        return;
      Building building1 = mouseTilePos.building;
      if (building1 == null)
        return;
      pTool.setText("flip.x:", (object) building1.flip_x);
      if (!building1.is_visible || World.world.quality_changer.isLowRes())
        return;
      int num4 = World.world.buildings.countVisibleBuildings();
      int index1 = 0;
      int num5 = 0;
      int num6 = 0;
      Building[] visibleBuildings = World.world.buildings.getVisibleBuildings();
      HashSet<Building> buildingSet = UnsafeCollectionPool<HashSet<Building>, Building>.Get();
      int length;
      for (int index2 = 0; index2 < num4; ++index2)
      {
        Building building2 = visibleBuildings[index2];
        if (building2 != null)
        {
          if (building2 == building1)
          {
            index1 = index2;
            DebugTool debugTool = pTool;
            string str3 = index2.ToString();
            length = visibleBuildings.Length;
            string str4 = length.ToString();
            string pT2 = $"{str3}/{str4}";
            debugTool.setText("visible id:", (object) pT2);
          }
          buildingSet.Add(building2);
          if (building2.isAlive())
            ++num5;
          else
            ++num6;
        }
      }
      DebugTool debugTool1 = pTool;
      string str5 = num5.ToString();
      length = visibleBuildings.Length;
      string str6 = length.ToString();
      string pT2_1 = $"{str5}/{str6}";
      debugTool1.setText("alive:", (object) pT2_1);
      DebugTool debugTool2 = pTool;
      string str7 = num6.ToString();
      length = visibleBuildings.Length;
      string str8 = length.ToString();
      string pT2_2 = $"{str7}/{str8}";
      debugTool2.setText("dead:", (object) pT2_2);
      pTool.setText("_visible_buildings_count:", (object) num4);
      pTool.setText("tUniqueBuildings:", (object) buildingSet.Count);
      UnsafeCollectionPool<HashSet<Building>, Building>.Release(buildingSet);
      BuildingRenderData renderData = World.world.buildings.render_data;
      pTool.setText("render_data_flip:", (object) renderData.flip_x_states[index1].ToString());
      QuantumSpriteAsset quantumSpriteAsset = AssetManager.quantum_sprites.get("draw_buildings");
      QuantumSpriteCacheData cacheData = quantumSpriteAsset.group_system.getCacheData(num4);
      if (cacheData != null)
      {
        if (cacheData.flip_x_states.Length <= index1)
          return;
        pTool.setText("render_data_flip:", (object) cacheData.flip_x_states[index1].ToString());
      }
      QuantumSprite[] fastActiveList = quantumSpriteAsset.group_system.getFastActiveList(num4);
      if (fastActiveList.Length <= index1)
        return;
      pTool.setText("q flip x:", (object) fastActiveList[index1].sprite_renderer.flipX.ToString());
    });
    this.add(pAsset8);
    DebugToolAsset pAsset9 = new DebugToolAsset();
    pAsset9.id = "Actor Statistics";
    pAsset9.action_1 = (DebugToolAssetAction) (pTool =>
    {
      Actor actorNearCursor = World.world.getActorNearCursor();
      if (actorNearCursor == null)
        return;
      pTool.setText("getSecondsLife:", (object) StatTool.getStringSecondsLife(actorNearCursor));
      pTool.setText("getAmountBreeding:", (object) StatTool.getStringAmountBreeding(actorNearCursor));
      pTool.setText("getAmountFood:", (object) StatTool.getAmountFood(actorNearCursor));
      pTool.setText("getDPS:", (object) StatTool.getDPS(actorNearCursor));
    });
    this.add(pAsset9);
    DebugToolAsset pAsset10 = new DebugToolAsset();
    pAsset10.id = "Biome Adaptation";
    pAsset10.action_1 = (DebugToolAssetAction) (pTool =>
    {
      Actor unit = SelectedUnit.unit;
      if (unit == null || !unit.hasSubspecies())
        return;
      WorldTile mouseTilePos = World.world.getMouseTilePos();
      if (mouseTilePos == null)
        return;
      mouseTilePos.zone.checkCanSettleInThisBiomes(unit.subspecies);
      pTool.setText("adapted:", (object) TileZone.debug_adapted);
      pTool.setText("not_adapted:", (object) TileZone.debug_not_adapted);
      pTool.setText("soil:", (object) TileZone.debug_soil);
      pTool.setText("can_settle:", (object) TileZone.debug_can_settle);
    });
    this.add(pAsset10);
    DebugToolAsset pAsset11 = new DebugToolAsset();
    pAsset11.id = "Kingdoms Wild";
    pAsset11.action_1 = (DebugToolAssetAction) (pTool =>
    {
      pTool.setText("#wild_kingdoms:", (object) World.world.kingdoms_wild.Count);
      foreach (Kingdom kingdom in (CoreSystemManager<Kingdom, KingdomData>) World.world.kingdoms_wild)
      {
        if (kingdom.hasUnits() || kingdom.hasBuildings())
        {
          DebugTool debugTool = pTool;
          string name = kingdom.name;
          int count = kingdom.units.Count;
          string str9 = count.ToString();
          count = kingdom.buildings.Count;
          string str10 = count.ToString();
          string pT2 = $"{str9} {str10}";
          debugTool.setText(name, (object) pT2);
        }
      }
    });
    this.add(pAsset11);
    DebugToolAsset pAsset12 = new DebugToolAsset();
    pAsset12.id = "Buildings Check";
    pAsset12.action_1 = (DebugToolAssetAction) (pTool =>
    {
      int pT2_3 = 0;
      int pT2_4 = 0;
      foreach (Building building in (SimSystemManager<Building, BuildingData>) World.world.buildings)
      {
        if (building.getHealth() <= building.getMaxHealth())
          ++pT2_3;
        else
          ++pT2_4;
      }
      pTool.setText("within max health:", (object) pT2_3);
      pTool.setText("higher than max health:", (object) pT2_4);
      pTool.setSeparator();
      foreach (Kingdom kingdom in (CoreSystemManager<Kingdom, KingdomData>) World.world.kingdoms)
      {
        bool flag = kingdom.buildings.Count == kingdom.countBuildings();
        pTool.setText(kingdom.name, (object) $"{flag.ToString()} | {kingdom.buildings.Count.ToString()} {kingdom.countBuildings().ToString()}");
      }
    });
    this.add(pAsset12);
    DebugToolAsset pAsset13 = new DebugToolAsset();
    pAsset13.id = "Kingdoms Civ";
    pAsset13.action_1 = (DebugToolAssetAction) (pTool =>
    {
      pTool.setText("#kingdoms:", (object) World.world.kingdoms.Count);
      pTool.setText("- units total:", (object) World.world.units.Count);
      int pT2_5 = 0;
      foreach (BaseSimObject unit in (SimSystemManager<Actor, ActorData>) World.world.units)
      {
        if (unit.kingdom == null)
          ++pT2_5;
      }
      pTool.setText("- units no kingdom:", (object) pT2_5);
      List<Kingdom> kingdomList = new List<Kingdom>((IEnumerable<Kingdom>) World.world.kingdoms);
      kingdomList.Sort(new Comparison<Kingdom>(pTool.kingdomSorter));
      foreach (Kingdom kingdom in kingdomList)
      {
        if (pTool.textCount > 0)
          pTool.setSeparator();
        pTool.setText("#id", (object) kingdom.id);
        pTool.setText("#name", (object) kingdom.name);
        pTool.setText("age", (object) kingdom.getAge());
        pTool.setText("units", (object) kingdom.units.Count);
        DebugTool debugTool = pTool;
        int num = kingdom.countTotalWarriors();
        string str11 = num.ToString();
        num = kingdom.countWarriorsMax();
        string str12 = num.ToString();
        string pT2_6 = $"{str11}/{str12}";
        debugTool.setText("army", (object) pT2_6);
        pTool.setText("buildings", (object) kingdom.buildings.Count);
      }
    });
    this.add(pAsset13);
    DebugToolAsset pAsset14 = new DebugToolAsset();
    pAsset14.id = "Behaviours";
    pAsset14.action_1 = (DebugToolAssetAction) (pTool =>
    {
      World.world.drop_manager.debug(pTool);
      pTool.setText("dirty last:", (object) World.world.dirty_tiles_last);
      pTool.setText("dirty tiles:", (object) World.world.tiles_dirty.Count);
      pTool.setSeparator();
      pTool.setText("tiles:", (object) World.world.tiles_list.Length);
      pTool.setSeparator();
      pTool.setText("water:", (object) WorldBehaviourOcean.tiles.Count);
      pTool.setText("burned_tiles:", (object) WorldBehaviourActionBurnedTiles.countBurnedTiles());
      pTool.setSeparator();
      pTool.setText("grey goo:", (object) World.world.grey_goo_layer.hashset?.Count);
      pTool.setText("conway", (object) World.world.conway_layer.hashsetTiles?.Count);
      pTool.setText("flash effect:", (object) World.world.flash_effects.pixels_to_update.Count);
      pTool.setSeparator();
      pTool.setText("explosion layer:", (object) World.world.explosion_layer.hashsetTiles?.Count);
      pTool.setText("bombDict:", (object) World.world.explosion_layer.hashset_bombs.Count);
      pTool.setText("nextWave:", (object) World.world.explosion_layer.nextWave.Count);
      pTool.setText("delayedBombs:", (object) World.world.explosion_layer.nextWave.Count);
      pTool.setText("timedBombs:", (object) World.world.explosion_layer.timedBombs.Count);
    });
    this.add(pAsset14);
    DebugToolAsset pAsset15 = new DebugToolAsset();
    pAsset15.id = "Unit Info";
    pAsset15.action_1 = (DebugToolAssetAction) (pTool =>
    {
      Actor actorNearCursor = World.world.getActorNearCursor();
      if (actorNearCursor == null)
        return;
      if (actorNearCursor.hasAnyStatusEffect())
        pTool.setText("status effects", (object) actorNearCursor.countStatusEffects());
      pTool.setText("profession:", (object) actorNearCursor.getProfession());
      if (actorNearCursor.ai.job != null)
        pTool.setText("current_job:", (object) actorNearCursor.ai.job.id);
      else
        pTool.setText("job:", (object) "-");
      pTool.setText("id:", (object) actorNearCursor.data.id);
      if (actorNearCursor.hasTask())
        pTool.setText("task:", (object) actorNearCursor.ai.task.id);
      else
        pTool.setText("task:", (object) "-");
      pTool.setSeparator();
      pTool.setText("name:", (object) actorNearCursor.getName());
      pTool.setText("is_moving:", (object) actorNearCursor.is_moving);
      pTool.setText("next_step:", (object) actorNearCursor.next_step_position.x);
      pTool.setSeparator();
      pTool.setText("stayingInBuilding:", (object) (actorNearCursor.inside_building != null));
      pTool.setText("bag.hasResources:", (object) actorNearCursor.isCarryingResources());
      pTool.setText("ignore:", (object) actorNearCursor.countTargetsToIgnore());
      pTool.setText("path global:", (object) actorNearCursor.current_path_global?.Count);
      pTool.setText("path local:", (object) actorNearCursor.current_path.Count);
      pTool.setText("path local index:", (object) actorNearCursor.current_path_index);
      pTool.setText("path split status:", (object) actorNearCursor.split_path.ToString());
      DebugTool debugTool3 = pTool;
      int num7 = actorNearCursor.getHealth();
      string str13 = num7.ToString();
      num7 = actorNearCursor.getMaxHealth();
      string str14 = num7.ToString();
      string pT2_7 = $"{str13}/{str14}";
      debugTool3.setText("health:", (object) pT2_7);
      pTool.setText("damage:", (object) $"{actorNearCursor.asset.base_stats["damage"].ToString()}/{actorNearCursor.stats["damage"].ToString()}");
      pTool.setText("city:", actorNearCursor.city == null ? (object) "-" : (object) actorNearCursor.city.name);
      pTool.setText("kingdom:", actorNearCursor.kingdom == null ? (object) "-" : (object) actorNearCursor.kingdom.name);
      pTool.setSeparator();
      DebugTool debugTool4 = pTool;
      int num8 = actorNearCursor.getNutrition();
      string str15 = num8.ToString();
      num8 = actorNearCursor.getMaxNutrition();
      string str16 = num8.ToString();
      string pT2_8 = $"{str15}/{str16}";
      debugTool4.setText("nutrition:", (object) pT2_8);
      pTool.setSeparator();
      if (actorNearCursor.animation_container != null)
        pTool.setText("actorAnimationData:", (object) actorNearCursor.animation_container.id);
      pTool.setText("stats name:", (object) actorNearCursor.asset.id);
      pTool.setSeparator();
      pTool.setText("timer_action:", (object) actorNearCursor.timer_action);
      pTool.setText("_timeout_targets:", (object) actorNearCursor._timeout_targets);
      pTool.setText("unitAttackTarget:", actorNearCursor.has_attack_target ? (object) (actorNearCursor.isEnemyTargetAlive().ToString() ?? "") : (object) "-");
      pTool.setSeparator();
      pTool.setText("attackTimer:", (object) actorNearCursor.attack_timer);
      pTool.setSeparator();
      pTool.setSeparator();
      pTool.setText("moveJumpOffset:", (object) actorNearCursor.move_jump_offset.y);
      pTool.setText("alive:", (object) actorNearCursor.isAlive());
      pTool.setText("zPosition:", (object) actorNearCursor.position_height);
      pTool.setSeparator();
      pTool.setText("phenotype_index:", (object) actorNearCursor.data.phenotype_index);
      pTool.setText("shade_id:", (object) actorNearCursor.data.phenotype_shade);
    });
    this.add(pAsset15);
    DebugToolAsset pAsset16 = new DebugToolAsset();
    pAsset16.id = "Actor Stats";
    pAsset16.action_1 = (DebugToolAssetAction) (pTool =>
    {
      WorldTile mouseTilePos = World.world.getMouseTilePos();
      if (mouseTilePos == null)
        return;
      int num9 = int.MaxValue;
      Actor actor = (Actor) null;
      foreach (Actor unit in (SimSystemManager<Actor, ActorData>) World.world.units)
      {
        int num10 = Toolbox.SquaredDistTile(unit.current_tile, mouseTilePos);
        if (num10 < num9)
        {
          actor = unit;
          num9 = num10;
        }
      }
      if (actor == null)
        return;
      pTool.setText("name:", (object) actor.getName());
      pTool.setSeparator();
      List<BaseStatsContainer> list = actor.stats.getList();
      foreach (BaseStatsContainer baseStatsContainer in list)
        pTool.setText(baseStatsContainer.id, (object) actor.stats[baseStatsContainer.id]);
      if (list.Count > 0)
        pTool.setSeparator();
      Dictionary<string, string> dictionary = actor.data.debug();
      foreach (string key in dictionary.Keys)
        pTool.setText(key, (object) dictionary[key]);
      if (dictionary.Count > 0)
        pTool.setSeparator();
      pTool.setText("currentTile:", actor.current_tile == null ? (object) "-" : (object) (actor.current_tile?.ToString() ?? ""));
      if (actor.current_tile == null)
        return;
      pTool.setText("x / y", (object) $"{actor.current_tile.x.ToString()} {actor.current_tile.y.ToString()}");
      pTool.setText("id", (object) actor.current_tile.data.tile_id);
      pTool.setText("height", (object) actor.current_tile.data.height);
      pTool.setText("type", (object) actor.current_tile.Type.id);
      pTool.setText("layer", (object) actor.current_tile.Type.layer_type);
      pTool.setText("main type", actor.current_tile.main_type != null ? (object) actor.current_tile.main_type.id : (object) "-");
      pTool.setText("top type", actor.current_tile.top_type != null ? (object) actor.current_tile.top_type.id : (object) "-");
      pTool.setText("targetedBy", (object) actor.current_tile.isTargeted());
      pTool.setText("units", (object) actor.current_tile.countUnits());
      pTool.setSeparator();
    });
    this.add(pAsset16);
    DebugToolAsset pAsset17 = new DebugToolAsset();
    pAsset17.id = "Unit Temperature";
    pAsset17.action_1 = (DebugToolAssetAction) (pTool => WorldBehaviourUnitTemperatures.debug(pTool));
    this.add(pAsset17);
    DebugToolAsset pAsset18 = new DebugToolAsset();
    pAsset18.id = "Zoom";
    pAsset18.action_1 = (DebugToolAssetAction) (pTool => World.world.quality_changer.debug(pTool));
    this.add(pAsset18);
    DebugToolAsset pAsset19 = new DebugToolAsset();
    pAsset19.id = "Mouse Cursor";
    pAsset19.action_1 = (DebugToolAssetAction) (pTool => MouseCursor.debug(pTool));
    this.add(pAsset19);
    DebugToolAsset pAsset20 = new DebugToolAsset();
    pAsset20.id = "Selected Power";
    pAsset20.action_1 = (DebugToolAssetAction) (pTool =>
    {
      if (!World.world.isAnyPowerSelected())
      {
        pTool.setText("no power selected", (object) "");
      }
      else
      {
        pTool.setText("selectedPower:", (object) World.world.getSelectedPowerID());
        GodPower selectedPowerAsset = World.world.getSelectedPowerAsset();
        pTool.setText("type:", (object) selectedPowerAsset.type);
        pTool.setSeparator();
        pTool.setText("show_tool_sizes:", (object) selectedPowerAsset.show_tool_sizes);
        pTool.setText("unselect_when_window:", (object) selectedPowerAsset.unselect_when_window);
        pTool.setText("ignore_cursor_icon:", (object) selectedPowerAsset.ignore_cursor_icon);
        pTool.setText("hold_action:", (object) selectedPowerAsset.hold_action);
        pTool.setText("click_interval:", (object) selectedPowerAsset.click_interval);
        pTool.setText("particle_interval:", (object) selectedPowerAsset.particle_interval);
        pTool.setText("falling_chance:", (object) selectedPowerAsset.falling_chance);
        pTool.setSeparator();
        pTool.setText("click_brush_action:", (object) selectedPowerAsset.click_brush_action);
        pTool.setText("click_action:", (object) selectedPowerAsset.click_action);
        pTool.setText("click_special_action:", (object) selectedPowerAsset.click_special_action);
        pTool.setText("click_power_brush_action:", (object) selectedPowerAsset.click_power_brush_action);
        pTool.setText("click_power_action:", (object) selectedPowerAsset.click_power_action);
        pTool.setText("select_button_action:", (object) selectedPowerAsset.select_button_action);
        pTool.setText("toggle_action:", (object) selectedPowerAsset.toggle_action);
        pTool.setSeparator();
        pTool.setText("actor_asset_id:", (object) selectedPowerAsset.actor_asset_id);
        pTool.setText("actor_asset_ids:", (object) selectedPowerAsset.actor_asset_ids);
        pTool.setText("toggle_name:", (object) selectedPowerAsset.toggle_name);
        pTool.setText("map_modes_switch:", (object) selectedPowerAsset.map_modes_switch);
        pTool.setText("show_spawn_effect:", (object) selectedPowerAsset.show_spawn_effect);
        pTool.setText("activate_on_hotkey_select:", (object) selectedPowerAsset.activate_on_hotkey_select);
      }
    });
    this.add(pAsset20);
    DebugToolAsset pAsset21 = new DebugToolAsset();
    pAsset21.id = "Hotkeys";
    pAsset21.action_1 = (DebugToolAssetAction) (pTool => AssetManager.hotkey_library.debug(pTool));
    this.add(pAsset21);
    DebugToolAsset pAsset22 = new DebugToolAsset();
    pAsset22.id = "Armies";
    pAsset22.action_1 = (DebugToolAssetAction) (pTool =>
    {
      pTool.setText("groups:", (object) World.world.armies.Count);
      foreach (Army army in (CoreSystemManager<Army, ArmyData>) World.world.armies)
        pTool.setText(": " + army.id.ToString(), (object) army.getDebug());
      pTool.setSeparator();
    });
    this.add(pAsset22);
    DebugToolAsset pAsset23 = new DebugToolAsset();
    pAsset23.id = "Magnet Debug";
    pAsset23.action_1 = (DebugToolAssetAction) (pTool =>
    {
      pTool.setText("hasUnits():", (object) World.world.magnet.hasUnits());
      pTool.setText("countUnits():", (object) World.world.magnet.countUnits());
      pTool.setText("magnetUnits.Count:", (object) World.world.magnet.magnet_units.Count);
      int pT2 = 0;
      foreach (Actor unit in (SimSystemManager<Actor, ActorData>) World.world.units)
      {
        if (unit.isAlive() && unit.is_in_magnet)
          ++pT2;
      }
      pTool.setText("tUnitsWithMagnetStatus:", (object) pT2);
    });
    this.add(pAsset23);
    DebugToolAsset pAsset24 = new DebugToolAsset();
    pAsset24.id = "Mindmap Debug";
    pAsset24.action_1 = new DebugToolAssetAction(NeuronsOverview.debugTool);
    this.add(pAsset24);
  }

  private void initMain()
  {
    // ISSUE: unable to decompile the method.
  }

  private void initAI()
  {
    DebugToolAsset pAsset1 = new DebugToolAsset();
    pAsset1.id = "Actor AI";
    pAsset1.action_1 = (DebugToolAssetAction) (pTool =>
    {
      WorldTile mouseTilePos = World.world.getMouseTilePos();
      if (mouseTilePos == null)
        return;
      int num1 = int.MaxValue;
      Actor actor = (Actor) null;
      foreach (Actor unit in (SimSystemManager<Actor, ActorData>) World.world.units)
      {
        if (!unit.isInsideSomething())
        {
          int num2 = Toolbox.SquaredDistTile(unit.current_tile, mouseTilePos);
          if (num2 < num1)
          {
            actor = unit;
            num1 = num2;
          }
        }
      }
      if (actor == null)
        return;
      pTool.setText("timer_action:", (object) actor.timer_action);
      pTool.setText("stat id:", (object) actor.asset.id);
      actor.ai.debug(pTool);
      DebugTool debugTool = pTool;
      WorldTile behTileTarget1 = actor.beh_tile_target;
      Vector2Int pos;
      int? nullable1;
      if (behTileTarget1 == null)
      {
        nullable1 = new int?();
      }
      else
      {
        pos = behTileTarget1.pos;
        nullable1 = new int?(((Vector2Int) ref pos)[0]);
      }
      string str1 = nullable1.ToString();
      WorldTile behTileTarget2 = actor.beh_tile_target;
      int? nullable2;
      if (behTileTarget2 == null)
      {
        nullable2 = new int?();
      }
      else
      {
        pos = behTileTarget2.pos;
        nullable2 = new int?(((Vector2Int) ref pos)[1]);
      }
      string str2 = nullable2.ToString();
      string pT2 = $"{str1}:{str2}";
      debugTool.setText("beh_tile_target", (object) pT2);
    });
    this.add(pAsset1);
    DebugToolAsset pAsset2 = new DebugToolAsset();
    pAsset2.id = "Boat AI";
    pAsset2.action_1 = (DebugToolAssetAction) (pTool =>
    {
      WorldTile mouseTilePos = World.world.getMouseTilePos();
      if (mouseTilePos == null)
        return;
      int num3 = int.MaxValue;
      Actor actor = (Actor) null;
      foreach (Actor unit in (SimSystemManager<Actor, ActorData>) World.world.units)
      {
        if (!unit.isInsideSomething() && unit.asset.is_boat)
        {
          int num4 = Toolbox.SquaredDistTile(unit.current_tile, mouseTilePos);
          if (num4 < num3)
          {
            actor = unit;
            num3 = num4;
          }
        }
      }
      if (actor == null)
        return;
      pTool.setText("action_timer:", (object) actor.timer_action);
      pTool.setText("stat id:", (object) actor.asset.id);
      TaxiRequest taxiRequest = actor.getSimpleComponent<Boat>().taxi_request;
      if (taxiRequest != null)
      {
        pTool.setText("taxi state:", (object) taxiRequest.state);
        pTool.setText("taxi actors:", (object) taxiRequest.countActors());
        WorldTile tileTarget = taxiRequest.getTileTarget();
        DebugTool debugTool1 = pTool;
        int num5;
        Vector2Int pos1;
        string pT2_1;
        if (tileTarget == null)
        {
          pT2_1 = "-";
        }
        else
        {
          Vector2Int pos2 = tileTarget.pos;
          num5 = ((Vector2Int) ref pos2)[0];
          string str3 = num5.ToString();
          pos1 = tileTarget.pos;
          num5 = ((Vector2Int) ref pos1)[1];
          string str4 = num5.ToString();
          pT2_1 = $"{str3}:{str4}";
        }
        debugTool1.setText("taxi target:", (object) pT2_1);
        WorldTile tileStart = taxiRequest.getTileStart();
        DebugTool debugTool2 = pTool;
        string pT2_2;
        if (tileStart == null)
        {
          pT2_2 = "-";
        }
        else
        {
          pos1 = tileStart.pos;
          num5 = ((Vector2Int) ref pos1)[0];
          string str5 = num5.ToString();
          pos1 = tileStart.pos;
          num5 = ((Vector2Int) ref pos1)[1];
          string str6 = num5.ToString();
          pT2_2 = $"{str5}:{str6}";
        }
        debugTool2.setText("taxi start:", (object) pT2_2);
      }
      actor.ai.debug(pTool);
    });
    this.add(pAsset2);
    DebugToolAsset pAsset3 = new DebugToolAsset();
    pAsset3.id = "City AI";
    pAsset3.action_1 = (DebugToolAssetAction) (pTool =>
    {
      WorldTile mouseTilePos = World.world.getMouseTilePos();
      if (mouseTilePos == null)
        return;
      City city = mouseTilePos.zone.city;
      if (city == null)
        return;
      pTool.setText("warrior_timer:", (object) city.getTimerForNewWarrior());
      pTool.setSeparator();
      if (city.ai != null)
        city.ai.debug(pTool);
      pTool.setSeparator();
      pTool.setText("action_timer:", (object) city.timer_action);
    });
    this.add(pAsset3);
    DebugToolAsset pAsset4 = new DebugToolAsset();
    pAsset4.id = "Kingdom AI";
    pAsset4.action_1 = (DebugToolAssetAction) (pTool =>
    {
      WorldTile mouseTilePos = World.world.getMouseTilePos();
      if (mouseTilePos == null)
        return;
      City city = mouseTilePos.zone.city;
      if (city == null)
        return;
      Kingdom kingdom = city.kingdom;
      if (kingdom.hasKing())
      {
        pTool.setText("personality:", (object) kingdom.king.s_personality.id);
        pTool.setText("agression:", (object) kingdom.king.stats["personality_aggression"]);
        pTool.setText("administration:", (object) kingdom.king.stats["personality_administration"]);
        pTool.setText("diplomatic:", (object) kingdom.king.stats["personality_diplomatic"]);
        pTool.setSeparator();
      }
      pTool.setText("timer_action:", (object) kingdom.timer_action);
      pTool.setText("timer_new_king:", (object) kingdom.data.timer_new_king);
      pTool.setSeparator();
      pTool.setText("action_timer:", (object) kingdom.timer_action);
      if (kingdom.ai == null)
        return;
      kingdom.ai.debug(pTool);
    });
    this.add(pAsset4);
  }

  private void initFmod()
  {
    DebugToolAsset pAsset1 = new DebugToolAsset();
    pAsset1.id = "FMOD";
    pAsset1.action_1 = (DebugToolAssetAction) (pTool => MusicBox.debug_fmod(pTool));
    this.add(pAsset1);
    DebugToolAsset pAsset2 = new DebugToolAsset();
    pAsset2.id = "FMOD World Params";
    pAsset2.action_1 = (DebugToolAssetAction) (pTool => MusicBox.inst.debug_world_params(pTool));
    this.add(pAsset2);
    DebugToolAsset pAsset3 = new DebugToolAsset();
    pAsset3.id = "FMOD Unit Params";
    pAsset3.action_1 = (DebugToolAssetAction) (pTool => MusicBox.inst.debug_unit_params(pTool));
    this.add(pAsset3);
    DebugToolAsset pAsset4 = new DebugToolAsset();
    pAsset4.id = "FMOD Params";
    pAsset4.action_1 = (DebugToolAssetAction) (pTool => MusicBox.inst.debug_params(pTool));
    this.add(pAsset4);
    DebugToolAsset pAsset5 = new DebugToolAsset();
    pAsset5.id = "Cursor Speed";
    pAsset5.action_1 = (DebugToolAssetAction) (pTool => MapBox.cursor_speed.debug(pTool));
    this.add(pAsset5);
  }

  private void initUI()
  {
    DebugToolAsset pAsset = new DebugToolAsset();
    pAsset.id = "screen_orientation";
    pAsset.action_1 = (DebugToolAssetAction) (pTool =>
    {
      pTool.setText("width:", (object) Screen.width);
      pTool.setText("height:", (object) Screen.height);
      pTool.setText("last width:", (object) CanvasMain.instance.getLastWidth());
      pTool.setText("last height:", (object) CanvasMain.instance.getLastHeight());
      pTool.setText("orientation:", (object) Screen.orientation);
      pTool.setText("saved orientation:", PlayerConfig.optionBoolEnabled("portrait") ? (object) (ScreenOrientation) 1 : (object) (ScreenOrientation) 3);
      pTool.setText("rotation to portrait:", (object) Screen.autorotateToPortrait);
      pTool.setText("rotation to landscape left:", (object) Screen.autorotateToLandscapeLeft);
      pTool.setText("rotation to landscape right:", (object) Screen.autorotateToLandscapeRight);
      pTool.setText("rotation to portrait reversed:", (object) Screen.autorotateToPortraitUpsideDown);
    });
    this.add(pAsset);
  }

  public override void post_init()
  {
    base.post_init();
    this.list.Sort((Comparison<DebugToolAsset>) ((a, b) => a.priority.CompareTo(b.priority)));
    this.list.Sort((Comparison<DebugToolAsset>) ((a, b) => string.Compare(a.id, b.id, StringComparison.InvariantCultureIgnoreCase)));
    TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;
    foreach (DebugToolAsset debugToolAsset in this.list)
      debugToolAsset.name = !(debugToolAsset.id.ToLower() == debugToolAsset.id) ? debugToolAsset.id : textInfo.ToTitleCase(debugToolAsset.id.Replace('_', ' '));
  }

  public override DebugToolAsset get(string pId)
  {
    foreach (DebugToolAsset debugToolAsset in this.list)
    {
      if (debugToolAsset.name == pId)
        return debugToolAsset;
    }
    return base.get(pId);
  }

  private void initBenchmarks()
  {
    DebugToolAsset pAsset1 = new DebugToolAsset();
    pAsset1.id = "Benchmark All";
    pAsset1.show_benchmark_buttons = true;
    pAsset1.type = DebugToolType.Benchmarks;
    pAsset1.priority = 1;
    pAsset1.benchmark_group_id = "game_total";
    pAsset1.benchmark_total = "game_total";
    pAsset1.benchmark_total_group = "main";
    pAsset1.action_start = new DebugToolAssetAction(this.setBenchmarksDefaultValue);
    pAsset1.action_1 = new DebugToolAssetAction(this.showGroupBenchmarkTop);
    pAsset1.action_2 = new DebugToolAssetAction(this.showGroupBenchmarkBottom);
    this.add(pAsset1);
    DebugToolAsset pAsset2 = new DebugToolAsset();
    pAsset2.id = "Benchmark Test Decisions";
    pAsset2.show_benchmark_buttons = true;
    pAsset2.type = DebugToolType.Benchmarks;
    pAsset2.priority = 50;
    pAsset2.benchmark_group_id = "decisions_test";
    pAsset2.benchmark_total = "decisions_test";
    pAsset2.benchmark_total_group = "decisions_test_total";
    pAsset2.split_benchmark = true;
    pAsset2.action_1 = new DebugToolAssetAction(this.showGroupBenchmarkTop);
    pAsset2.action_2 = new DebugToolAssetAction(this.showGroupBenchmarkBottom);
    pAsset2.action_start = (DebugToolAssetAction) (pTool =>
    {
      this.setBenchmarksDefaultValue(pTool);
      pTool.show_counter = true;
      pTool.show_averages = false;
      pTool.hide_zeroes = false;
      pTool.show_max = true;
      pTool.sort_by_names = false;
      pTool.sort_by_values = true;
      pTool.state = DebugToolState.Values;
    });
    pAsset2.action_update = (DebugToolUpdateDelegate) (pTool =>
    {
      if (World.world.units.Count == 0)
        return;
      Actor simple = World.world.units.getSimpleList()[0];
      Bench.bench("decisions_test", "decisions_test_total");
      Bench.bench("decisions", "decisions_test");
      for (int index = 0; index < 5000; ++index)
        DecisionHelper.runSimulation(simple);
      Bench.benchEnd("decisions", "decisions_test");
      Bench.benchEnd("decisions_test", "decisions_test_total");
    });
    this.add(pAsset2);
    DebugToolAsset pAsset3 = new DebugToolAsset();
    pAsset3.id = "Benchmark Zone Camera";
    pAsset3.show_benchmark_buttons = true;
    pAsset3.type = DebugToolType.Benchmarks;
    pAsset3.benchmark_group_id = "zone_camera";
    pAsset3.benchmark_total = "zone_camera";
    pAsset3.benchmark_total_group = "zone_camera_total";
    pAsset3.action_start = new DebugToolAssetAction(this.setBenchmarksDefaultValue);
    pAsset3.action_1 = new DebugToolAssetAction(this.showGroupBenchmarkTop);
    pAsset3.action_2 = new DebugToolAssetAction(this.showGroupBenchmarkBottom);
    this.add(pAsset3);
    DebugToolAsset pAsset4 = new DebugToolAsset();
    pAsset4.id = "benchmark_chunks";
    pAsset4.show_benchmark_buttons = true;
    pAsset4.type = DebugToolType.Benchmarks;
    pAsset4.benchmark_group_id = "chunks";
    pAsset4.benchmark_total = "chunks";
    pAsset4.benchmark_total_group = "chunks_total";
    pAsset4.split_benchmark = true;
    pAsset4.action_1 = (DebugToolAssetAction) (pTool =>
    {
      double totalFrameBudget = this.getTotalFrameBudget();
      double benchResultAsDouble = Bench.getBenchResultAsDouble(pTool.asset.benchmark_total, pTool.asset.benchmark_total_group, pTool.isValueAverage());
      pTool.setText("group total:", (object) this.trim(benchResultAsDouble, true), 100f, true);
      double num1 = benchResultAsDouble / (double) Time.deltaTime * 100.0;
      pTool.setText("total frame time spent:", (object) this.trimPercent(num1), (float) num1, true);
      double num2 = benchResultAsDouble * 1000.0 / totalFrameBudget * 100.0;
      pTool.setText("total budget time spent:", (object) this.trimPercent(num2), (float) num2, true);
      pTool.setSeparator();
      pTool.setText("########### last_dirty:", (object) null);
      pTool.setText("chunks:", (object) Bench.getBenchValue("m_dirtyChunks", "chunks"));
      pTool.setText("new regions:", (object) Bench.getBenchValue("m_newRegions", "chunks"));
      pTool.setText("new links:", (object) Bench.getBenchValue("m_newLinks", "chunks"));
      pTool.setText("new islands:", (object) Bench.getBenchValue("m_newIslands", "chunks"));
      pTool.setText("last dirty islands:", (object) Bench.getBenchValue("m_dirtyIslands", "chunks"));
      pTool.setText("last dirty corners:", (object) Bench.getBenchValue("m_dirtyCorners", "chunks"));
      pTool.setText("dirty islands neighb:", (object) Bench.getBenchValue("m_dirtyIslandNeighb", "chunks"));
      pTool.setSeparator();
      pTool.setText("########### last_bench:", (object) null);
    });
    pAsset4.action_2 = new DebugToolAssetAction(this.showGroupBenchmarkBottom);
    pAsset4.action_start = (DebugToolAssetAction) (pTool =>
    {
      pTool.show_averages = false;
      pTool.show_counter = true;
      pTool.show_max = false;
      pTool.hide_zeroes = false;
      pTool.state = DebugToolState.Percent;
    });
    this.add(pAsset4);
    DebugToolAsset pAsset5 = new DebugToolAsset();
    pAsset5.id = "Benchmark Quantum Sprites";
    pAsset5.show_benchmark_buttons = true;
    pAsset5.type = DebugToolType.Benchmarks;
    pAsset5.benchmark_group_id = "quantum_sprites";
    pAsset5.benchmark_total = "quantum_sprites";
    pAsset5.benchmark_total_group = "game_total";
    pAsset5.split_benchmark = true;
    pAsset5.action_start = new DebugToolAssetAction(this.setBenchmarksDefaultValue);
    pAsset5.action_1 = new DebugToolAssetAction(this.showGroupBenchmarkTop);
    pAsset5.action_2 = new DebugToolAssetAction(this.showGroupBenchmarkBottom);
    this.add(pAsset5);
    DebugToolAsset pAsset6 = new DebugToolAsset();
    pAsset6.id = "Benchmark Cache Manager";
    pAsset6.show_benchmark_buttons = true;
    pAsset6.type = DebugToolType.Benchmarks;
    pAsset6.benchmark_group_id = "world_cache_manager";
    pAsset6.benchmark_total = "world_cache_manager";
    pAsset6.benchmark_total_group = "game_total";
    pAsset6.split_benchmark = true;
    pAsset6.action_1 = new DebugToolAssetAction(this.showGroupBenchmarkTop);
    pAsset6.action_2 = new DebugToolAssetAction(this.showGroupBenchmarkBottom);
    pAsset6.action_start = (DebugToolAssetAction) (pTool =>
    {
      this.setBenchmarksDefaultValue(pTool);
      pTool.show_counter = false;
      pTool.hide_zeroes = false;
      pTool.show_max = false;
      pTool.state = DebugToolState.Values;
    });
    this.add(pAsset6);
    DebugToolAsset pAsset7 = new DebugToolAsset();
    pAsset7.id = "Benchmark Sim Zones";
    pAsset7.show_benchmark_buttons = true;
    pAsset7.type = DebugToolType.Benchmarks;
    pAsset7.benchmark_group_id = "sim_zones";
    pAsset7.benchmark_total = "sim_zones";
    pAsset7.benchmark_total_group = "game_total";
    pAsset7.split_benchmark = true;
    pAsset7.action_1 = new DebugToolAssetAction(this.showGroupBenchmarkTop);
    pAsset7.action_2 = new DebugToolAssetAction(this.showGroupBenchmarkBottom);
    pAsset7.action_start = (DebugToolAssetAction) (pTool =>
    {
      this.setBenchmarksDefaultValue(pTool);
      pTool.show_counter = false;
      pTool.hide_zeroes = false;
      pTool.show_max = false;
      pTool.state = DebugToolState.Values;
    });
    this.add(pAsset7);
    DebugToolAsset pAsset8 = new DebugToolAsset();
    pAsset8.id = "Benchmark MusicBox";
    pAsset8.show_benchmark_buttons = true;
    pAsset8.type = DebugToolType.Benchmarks;
    pAsset8.benchmark_group_id = "music_box";
    pAsset8.benchmark_total = "music_box";
    pAsset8.benchmark_total_group = "music_box_total";
    pAsset8.split_benchmark = true;
    pAsset8.action_1 = new DebugToolAssetAction(this.showGroupBenchmarkTop);
    pAsset8.action_2 = new DebugToolAssetAction(this.showGroupBenchmarkBottom);
    pAsset8.action_start = (DebugToolAssetAction) (pTool =>
    {
      this.setBenchmarksDefaultValue(pTool);
      pTool.show_counter = false;
      pTool.hide_zeroes = false;
      pTool.show_max = false;
      pTool.state = DebugToolState.Values;
    });
    this.add(pAsset8);
    DebugToolAsset pAsset9 = new DebugToolAsset();
    pAsset9.id = "Benchmark Nameplates";
    pAsset9.show_benchmark_buttons = true;
    pAsset9.type = DebugToolType.Benchmarks;
    pAsset9.benchmark_group_id = "nameplates";
    pAsset9.benchmark_total = "nameplates";
    pAsset9.benchmark_total_group = "nameplates_total";
    pAsset9.split_benchmark = true;
    pAsset9.action_1 = new DebugToolAssetAction(this.showGroupBenchmarkTop);
    pAsset9.action_2 = new DebugToolAssetAction(this.showGroupBenchmarkBottom);
    pAsset9.action_start = (DebugToolAssetAction) (pTool =>
    {
      this.setBenchmarksDefaultValue(pTool);
      pTool.show_counter = false;
      pTool.hide_zeroes = false;
      pTool.show_max = false;
      pTool.state = DebugToolState.FrameBudget;
    });
    this.add(pAsset9);
    DebugToolAsset pAsset10 = new DebugToolAsset();
    pAsset10.id = "Benchmark Borderers Renderer";
    pAsset10.show_benchmark_buttons = true;
    pAsset10.type = DebugToolType.Benchmarks;
    pAsset10.benchmark_group_id = "borders_renderer";
    pAsset10.benchmark_total = "borders_renderer";
    pAsset10.benchmark_total_group = "borders_renderer_total";
    pAsset10.split_benchmark = true;
    pAsset10.action_1 = new DebugToolAssetAction(this.showGroupBenchmarkTop);
    pAsset10.action_2 = new DebugToolAssetAction(this.showGroupBenchmarkBottom);
    pAsset10.action_start = (DebugToolAssetAction) (pTool =>
    {
      this.setBenchmarksDefaultValue(pTool);
      pTool.show_counter = false;
      pTool.hide_zeroes = false;
      pTool.show_max = false;
      pTool.state = DebugToolState.FrameBudget;
    });
    this.add(pAsset10);
    DebugToolAsset pAsset11 = new DebugToolAsset();
    pAsset11.id = "Benchmark Fluid Zones Data";
    pAsset11.show_benchmark_buttons = true;
    pAsset11.type = DebugToolType.Benchmarks;
    pAsset11.benchmark_group_id = "fluid_zones_data";
    pAsset11.benchmark_total = "fluid_zones_data";
    pAsset11.benchmark_total_group = "fluid_zones_data_total";
    pAsset11.split_benchmark = true;
    pAsset11.action_1 = new DebugToolAssetAction(this.showGroupBenchmarkTop);
    pAsset11.action_2 = new DebugToolAssetAction(this.showGroupBenchmarkBottom);
    pAsset11.action_start = (DebugToolAssetAction) (pTool =>
    {
      this.setBenchmarksDefaultValue(pTool);
      pTool.show_counter = false;
      pTool.hide_zeroes = false;
      pTool.show_max = false;
      pTool.state = DebugToolState.FrameBudget;
    });
    this.add(pAsset11);
    DebugToolAsset pAsset12 = new DebugToolAsset();
    pAsset12.id = "Benchmark World Beh";
    pAsset12.show_benchmark_buttons = true;
    pAsset12.type = DebugToolType.Benchmarks;
    pAsset12.benchmark_group_id = "world_beh";
    pAsset12.benchmark_total = "world_beh";
    pAsset12.benchmark_total_group = "game_total";
    pAsset12.action_start = new DebugToolAssetAction(this.setBenchmarksDefaultValue);
    pAsset12.action_1 = new DebugToolAssetAction(this.showGroupBenchmarkTop);
    pAsset12.action_2 = new DebugToolAssetAction(this.showGroupBenchmarkBottom);
    this.add(pAsset12);
    DebugToolAsset pAsset13 = new DebugToolAsset();
    pAsset13.id = "Benchmark Buildings";
    pAsset13.show_benchmark_buttons = true;
    pAsset13.type = DebugToolType.Benchmarks;
    pAsset13.benchmark_group_id = "buildings";
    pAsset13.benchmark_total = "buildings";
    pAsset13.benchmark_total_group = "game_total";
    pAsset13.split_benchmark = true;
    pAsset13.action_start = new DebugToolAssetAction(this.setBenchmarksDefaultValue);
    pAsset13.action_1 = new DebugToolAssetAction(this.showGroupBenchmarkTop);
    pAsset13.action_2 = new DebugToolAssetAction(this.showGroupBenchmarkBottom);
    this.add(pAsset13);
    this.t.action_1 += (DebugToolAssetAction) (pTool =>
    {
      JobManagerBuildings jobManager = World.world.buildings.getJobManager();
      pTool.setText("batches total/free:", (object) jobManager.debugBatchCount());
      pTool.setText("active jobs:", (object) jobManager.debugJobCount());
      pTool.setSeparator();
    });
    DebugToolAsset pAsset14 = new DebugToolAsset();
    pAsset14.id = "Benchmark Actors";
    pAsset14.show_benchmark_buttons = true;
    pAsset14.type = DebugToolType.Benchmarks;
    pAsset14.benchmark_group_id = "actors";
    pAsset14.benchmark_total = "actors";
    pAsset14.benchmark_total_group = "game_total";
    pAsset14.split_benchmark = true;
    pAsset14.action_start = new DebugToolAssetAction(this.setBenchmarksDefaultValue);
    pAsset14.action_1 = new DebugToolAssetAction(this.showGroupBenchmarkTop);
    pAsset14.action_2 = new DebugToolAssetAction(this.showGroupBenchmarkBottom);
    this.add(pAsset14);
    this.t.action_1 += (DebugToolAssetAction) (pTool =>
    {
      JobManagerActors jobManager = World.world.units.getJobManager();
      pTool.setText("batches total/free:", (object) jobManager.debugBatchCount());
      pTool.setText("active jobs:", (object) jobManager.debugJobCount());
      pTool.setSeparator();
    });
    DebugToolAsset pAsset15 = new DebugToolAsset();
    pAsset15.id = "Benchmark AI Actions";
    pAsset15.show_benchmark_buttons = true;
    pAsset15.type = DebugToolType.Benchmarks;
    pAsset15.benchmark_group_id = "ai_actions";
    pAsset15.benchmark_total = "ai_actions";
    pAsset15.benchmark_total_group = "ai_actions_total";
    pAsset15.split_benchmark = true;
    pAsset15.action_start = (DebugToolAssetAction) (pTool =>
    {
      this.setBenchmarksDefaultValue(pTool);
      pTool.show_max = false;
      pTool.show_averages = true;
      pTool.hide_zeroes = true;
      pTool.show_max = false;
      pTool.sort_by_names = false;
      pTool.sort_by_values = true;
      pTool.state = DebugToolState.Values;
    });
    pAsset15.action_1 = new DebugToolAssetAction(this.showGroupBenchmarkTop);
    pAsset15.action_2 = new DebugToolAssetAction(this.showGroupBenchmarkBottom);
    this.add(pAsset15);
    this.t.show_on_start = DebugConfig.isOn(DebugOption.BenchAiEnabled);
    DebugToolAsset pAsset16 = new DebugToolAsset();
    pAsset16.id = "Benchmark AI Tasks";
    pAsset16.show_benchmark_buttons = true;
    pAsset16.type = DebugToolType.Benchmarks;
    pAsset16.benchmark_group_id = "ai_tasks";
    pAsset16.benchmark_total = "ai_tasks";
    pAsset16.benchmark_total_group = "ai_tasks_total";
    pAsset16.split_benchmark = true;
    pAsset16.action_start = (DebugToolAssetAction) (pTool =>
    {
      this.setBenchmarksDefaultValue(pTool);
      pTool.show_max = false;
      pTool.show_averages = true;
      pTool.hide_zeroes = true;
      pTool.show_max = false;
      pTool.sort_by_names = false;
      pTool.sort_by_values = true;
      pTool.state = DebugToolState.Values;
    });
    pAsset16.action_1 = new DebugToolAssetAction(this.showGroupBenchmarkTop);
    pAsset16.action_2 = new DebugToolAssetAction(this.showGroupBenchmarkBottom);
    this.add(pAsset16);
    this.t.show_on_start = DebugConfig.isOn(DebugOption.BenchAiEnabled);
    DebugToolAsset pAsset17 = new DebugToolAsset();
    pAsset17.id = "$benchmark_loops$";
    pAsset17.show_benchmark_buttons = true;
    pAsset17.type = DebugToolType.Benchmarks;
    pAsset17.benchmark_group_id = "loops_test_100";
    pAsset17.benchmark_total = "loops_test_100";
    pAsset17.benchmark_total_group = "loops_test_total_100";
    pAsset17.show_last_count = true;
    pAsset17.action_1 = new DebugToolAssetAction(this.showGroupBenchmarkTop);
    pAsset17.action_2 = new DebugToolAssetAction(this.showGroupBenchmarkBottom);
    pAsset17.action_start = (DebugToolAssetAction) (pTool =>
    {
      this.setBenchmarksDefaultValue(pTool);
      pTool.show_counter = true;
      pTool.show_averages = true;
      pTool.hide_zeroes = false;
      pTool.show_max = true;
      pTool.sort_by_names = false;
      pTool.sort_by_values = true;
      pTool.state = DebugToolState.Values;
    });
    pAsset17.action_update = (DebugToolUpdateDelegate) (pTool => BenchmarkLoops.update(pTool.asset));
    this.add(pAsset17);
    this.clone("Benchmark Loops 10", "$benchmark_loops$");
    this.t.benchmark_group_id = "loops_test_10";
    this.t.benchmark_total = "loops_test_10";
    this.t.benchmark_total_group = "loops_test_total_10";
    BenchmarkLoops benchmarkLoops1;
    this.t.action_start += (DebugToolAssetAction) (pTool => benchmarkLoops1 = new BenchmarkLoops(pTool.asset, 10));
    this.clone("Benchmark Loops 100", "$benchmark_loops$");
    this.t.benchmark_group_id = "loops_test_100";
    this.t.benchmark_total = "loops_test_100";
    this.t.benchmark_total_group = "loops_test_total_100";
    BenchmarkLoops benchmarkLoops2;
    this.t.action_start += (DebugToolAssetAction) (pTool => benchmarkLoops2 = new BenchmarkLoops(pTool.asset, 100));
    this.clone("Benchmark Loops 1000", "$benchmark_loops$");
    this.t.benchmark_group_id = "loops_test_1000";
    this.t.benchmark_total = "loops_test_1000";
    this.t.benchmark_total_group = "loops_test_total_1000";
    BenchmarkLoops benchmarkLoops3;
    this.t.action_start += (DebugToolAssetAction) (pTool => benchmarkLoops3 = new BenchmarkLoops(pTool.asset, 1000));
    this.clone("Benchmark Loops 10000", "$benchmark_loops$");
    this.t.benchmark_group_id = "loops_test_10000";
    this.t.benchmark_total = "loops_test_10000";
    this.t.benchmark_total_group = "loops_test_total_10000";
    BenchmarkLoops benchmarkLoops4;
    this.t.action_start += (DebugToolAssetAction) (pTool => benchmarkLoops4 = new BenchmarkLoops(pTool.asset, 10000));
    DebugToolAsset pAsset18 = new DebugToolAsset();
    pAsset18.id = "Benchmark Distance";
    pAsset18.show_benchmark_buttons = true;
    pAsset18.type = DebugToolType.Benchmarks;
    pAsset18.benchmark_group_id = "dist_test";
    pAsset18.benchmark_total = "dist_test";
    pAsset18.benchmark_total_group = "dist_test_total";
    pAsset18.show_last_count = true;
    pAsset18.action_1 = new DebugToolAssetAction(this.showGroupBenchmarkTop);
    pAsset18.action_2 = new DebugToolAssetAction(this.showGroupBenchmarkBottom);
    pAsset18.action_start = (DebugToolAssetAction) (pTool =>
    {
      this.setBenchmarksDefaultValue(pTool);
      pTool.show_counter = true;
      pTool.show_averages = false;
      pTool.hide_zeroes = false;
      pTool.show_max = true;
      pTool.sort_by_names = false;
      pTool.sort_by_values = true;
      pTool.state = DebugToolState.Percent;
      BenchmarkDist benchmarkDist = new BenchmarkDist();
    });
    pAsset18.action_update = (DebugToolUpdateDelegate) (_ => BenchmarkDist.update());
    this.add(pAsset18);
    this.clone("$benchmark_shuffle_loops$", "$benchmark_loops$");
    this.t.action_update = (DebugToolUpdateDelegate) (pTool => BenchmarkShuffle.update(pTool.asset));
    this.clone("Benchmark Shuffle Loops 10", "$benchmark_shuffle_loops$");
    this.t.benchmark_group_id = "shuffle_test_10";
    this.t.benchmark_total = "shuffle_test_10";
    this.t.benchmark_total_group = "shuffle_test_total_10";
    BenchmarkShuffle benchmarkShuffle1;
    this.t.action_start += (DebugToolAssetAction) (pTool => benchmarkShuffle1 = new BenchmarkShuffle(pTool.asset, 10, 50));
    this.clone("Benchmark Shuffle Loops 100", "$benchmark_shuffle_loops$");
    this.t.benchmark_group_id = "shuffle_test_100";
    this.t.benchmark_total = "shuffle_test_100";
    this.t.benchmark_total_group = "shuffle_test_total_100";
    BenchmarkShuffle benchmarkShuffle2;
    this.t.action_start += (DebugToolAssetAction) (pTool => benchmarkShuffle2 = new BenchmarkShuffle(pTool.asset, 100, 500));
    this.clone("Benchmark Shuffle Loops 1000", "$benchmark_shuffle_loops$");
    this.t.benchmark_group_id = "shuffle_test_1000";
    this.t.benchmark_total = "shuffle_test_1000";
    this.t.benchmark_total_group = "shuffle_test_total_1000";
    BenchmarkShuffle benchmarkShuffle3;
    this.t.action_start += (DebugToolAssetAction) (pTool => benchmarkShuffle3 = new BenchmarkShuffle(pTool.asset, 1000, 5000));
    this.clone("Benchmark Shuffle Loops 10000", "$benchmark_shuffle_loops$");
    this.t.benchmark_group_id = "shuffle_test_10000";
    this.t.benchmark_total = "shuffle_test_10000";
    this.t.benchmark_total_group = "shuffle_test_total_10000";
    BenchmarkShuffle benchmarkShuffle4;
    this.t.action_start += (DebugToolAssetAction) (pTool => benchmarkShuffle4 = new BenchmarkShuffle(pTool.asset, 10000, 25000));
    DebugToolAsset pAsset19 = new DebugToolAsset();
    pAsset19.id = "Benchmark Field Acess";
    pAsset19.show_benchmark_buttons = true;
    pAsset19.type = DebugToolType.Benchmarks;
    pAsset19.benchmark_group_id = "field_acess_test";
    pAsset19.benchmark_total = "field_acess_test";
    pAsset19.benchmark_total_group = "field_acess_total";
    pAsset19.split_benchmark = true;
    pAsset19.action_1 = new DebugToolAssetAction(this.showGroupBenchmarkTop);
    pAsset19.action_2 = new DebugToolAssetAction(this.showGroupBenchmarkBottom);
    pAsset19.action_start = (DebugToolAssetAction) (pTool =>
    {
      this.setBenchmarksDefaultValue(pTool);
      pTool.show_counter = true;
      pTool.show_averages = false;
      pTool.hide_zeroes = false;
      pTool.show_max = true;
      pTool.sort_by_names = false;
      pTool.sort_by_values = true;
      pTool.state = DebugToolState.Values;
    });
    pAsset19.action_update = (DebugToolUpdateDelegate) (_ => BenchmarkFieldAccess.start());
    this.add(pAsset19);
    DebugToolAsset pAsset20 = new DebugToolAsset();
    pAsset20.id = "Benchmark Sprites";
    pAsset20.show_benchmark_buttons = true;
    pAsset20.type = DebugToolType.Benchmarks;
    pAsset20.benchmark_group_id = "sprites_test";
    pAsset20.benchmark_total = "sprites_test";
    pAsset20.benchmark_total_group = "sprites_test_total";
    pAsset20.split_benchmark = true;
    pAsset20.action_1 = new DebugToolAssetAction(this.showGroupBenchmarkTop);
    pAsset20.action_2 = new DebugToolAssetAction(this.showGroupBenchmarkBottom);
    pAsset20.action_start = (DebugToolAssetAction) (pTool =>
    {
      this.setBenchmarksDefaultValue(pTool);
      pTool.show_counter = true;
      pTool.show_averages = false;
      pTool.hide_zeroes = false;
      pTool.show_max = true;
      pTool.sort_by_names = false;
      pTool.sort_by_values = true;
      pTool.state = DebugToolState.Values;
    });
    pAsset20.action_update = (DebugToolUpdateDelegate) (_ => BenchmarkSprites.start());
    this.add(pAsset20);
    DebugToolAsset pAsset21 = new DebugToolAsset();
    pAsset21.id = "Benchmark Struct Loops";
    pAsset21.show_benchmark_buttons = true;
    pAsset21.type = DebugToolType.Benchmarks;
    pAsset21.benchmark_group_id = "loops_struct_test";
    pAsset21.benchmark_total = "loops_struct_test";
    pAsset21.benchmark_total_group = "loops_struct_test_total";
    pAsset21.split_benchmark = true;
    pAsset21.action_1 = new DebugToolAssetAction(this.showGroupBenchmarkTop);
    pAsset21.action_2 = new DebugToolAssetAction(this.showGroupBenchmarkBottom);
    pAsset21.action_start = (DebugToolAssetAction) (pTool =>
    {
      this.setBenchmarksDefaultValue(pTool);
      pTool.show_counter = true;
      pTool.show_averages = false;
      pTool.hide_zeroes = false;
      pTool.show_max = true;
      pTool.sort_by_names = false;
      pTool.sort_by_values = true;
      pTool.state = DebugToolState.Values;
    });
    pAsset21.action_update = (DebugToolUpdateDelegate) (pTool => BenchmarkStructLoops.start());
    this.add(pAsset21);
    DebugToolAsset pAsset22 = new DebugToolAsset();
    pAsset22.id = "Benchmark ECS";
    pAsset22.show_benchmark_buttons = true;
    pAsset22.type = DebugToolType.Benchmarks;
    pAsset22.benchmark_group_id = "ecs_test";
    pAsset22.benchmark_total = "ecs_test";
    pAsset22.benchmark_total_group = "ecs_test_total";
    pAsset22.split_benchmark = true;
    pAsset22.action_1 = new DebugToolAssetAction(this.showGroupBenchmarkTop);
    pAsset22.action_2 = new DebugToolAssetAction(this.showGroupBenchmarkBottom);
    pAsset22.action_start = (DebugToolAssetAction) (pTool =>
    {
      this.setBenchmarksDefaultValue(pTool);
      pTool.show_counter = true;
      pTool.show_averages = false;
      pTool.hide_zeroes = false;
      pTool.show_max = true;
      pTool.sort_by_names = true;
      pTool.state = DebugToolState.Percent;
    });
    this.add(pAsset22);
    DebugToolAsset pAsset23 = new DebugToolAsset();
    pAsset23.id = "Benchmark Blacklist";
    pAsset23.show_benchmark_buttons = true;
    pAsset23.type = DebugToolType.Benchmarks;
    pAsset23.benchmark_group_id = "blacklist_test";
    pAsset23.benchmark_total = "blacklist_test";
    pAsset23.benchmark_total_group = "blacklist_test_total";
    pAsset23.split_benchmark = true;
    pAsset23.action_1 = new DebugToolAssetAction(this.showGroupBenchmarkTop);
    pAsset23.action_2 = new DebugToolAssetAction(this.showGroupBenchmarkBottom);
    pAsset23.action_start = (DebugToolAssetAction) (pTool =>
    {
      this.setBenchmarksDefaultValue(pTool);
      pTool.show_counter = true;
      pTool.show_averages = true;
      pTool.hide_zeroes = false;
      pTool.show_max = true;
      pTool.sort_by_names = false;
      pTool.sort_by_values = true;
      pTool.state = DebugToolState.TimeSpent;
    });
    pAsset23.action_update = (DebugToolUpdateDelegate) (pTool => BenchmarkBlacklist.start());
    this.add(pAsset23);
    DebugToolAsset pAsset24 = new DebugToolAsset();
    pAsset24.id = "Benchmark Trait Effects";
    pAsset24.show_benchmark_buttons = true;
    pAsset24.type = DebugToolType.Benchmarks;
    pAsset24.benchmark_group_id = "effects_traits";
    pAsset24.benchmark_total = "effects_traits";
    pAsset24.benchmark_total_group = "game_total";
    pAsset24.split_benchmark = true;
    pAsset24.action_start = new DebugToolAssetAction(this.setBenchmarksDefaultValue);
    pAsset24.action_1 = new DebugToolAssetAction(this.showGroupBenchmarkTop);
    pAsset24.action_2 = new DebugToolAssetAction(this.showGroupBenchmarkBottom);
    this.add(pAsset24);
    DebugToolAsset pAsset25 = new DebugToolAsset();
    pAsset25.id = "Benchmark Item Effects";
    pAsset25.show_benchmark_buttons = true;
    pAsset25.type = DebugToolType.Benchmarks;
    pAsset25.benchmark_group_id = "effects_items";
    pAsset25.benchmark_total = "effects_items";
    pAsset25.benchmark_total_group = "game_total";
    pAsset25.split_benchmark = true;
    pAsset25.action_start = new DebugToolAssetAction(this.setBenchmarksDefaultValue);
    pAsset25.action_1 = new DebugToolAssetAction(this.showGroupBenchmarkTop);
    pAsset25.action_2 = new DebugToolAssetAction(this.showGroupBenchmarkBottom);
    this.add(pAsset25);
    DebugToolAsset pAsset26 = new DebugToolAsset();
    pAsset26.id = "Benchmark";
    pAsset26.type = DebugToolType.Benchmarks;
    pAsset26.priority = 2;
    pAsset26.action_1 = (DebugToolAssetAction) (pTool =>
    {
      pTool.setText("CityBehCheckSettleTarget_tick:", (object) Bench.getBenchResult("CityBehCheckSettleTarget", pAverage: false));
      pTool.setSeparator();
      pTool.setText("test_follow:", (object) Bench.getBenchResult("test_follow"));
    });
    this.add(pAsset26);
  }

  private void setBenchmarksDefaultValue(DebugTool pTool)
  {
    pTool.sort_order_reversed = false;
    pTool.sort_by_names = false;
    pTool.sort_by_values = false;
    pTool.show_averages = true;
    pTool.hide_zeroes = true;
    pTool.show_counter = true;
    pTool.show_max = true;
    pTool.state = DebugToolState.FrameBudget;
    pTool.paused = false;
    pTool.percentage_slowest = false;
    if (!Config.editor_mastef)
      return;
    DebugConfig.debugToolMastefDefaults(pTool);
  }

  private void showGroupBenchmarkTop(DebugTool pTool)
  {
    float deltaTime = Time.deltaTime;
    double totalFrameBudget = this.getTotalFrameBudget();
    double benchResultAsDouble1 = Bench.getBenchResultAsDouble("game_total", pAverage: pTool.isValueAverage());
    pTool.setText("game total:", (object) this.trim(benchResultAsDouble1, true));
    pTool.setText("fps:", (object) FPS.getFPS());
    pTool.setSeparator();
    double benchResultAsDouble2 = Bench.getBenchResultAsDouble(pTool.asset.benchmark_total, pTool.asset.benchmark_total_group, pTool.isValueAverage());
    if (pTool.asset.benchmark_total != "game_total")
    {
      pTool.setText("group total:", (object) this.trim(benchResultAsDouble2, true), 100f, true);
      double num = benchResultAsDouble2 / benchResultAsDouble1 * 100.0;
      pTool.setText("usage from total:", (object) this.trimPercent(num), (float) num, true);
    }
    else
    {
      pTool.setSeparator();
      pTool.setSeparator();
    }
    double num1 = benchResultAsDouble2 / (double) deltaTime * 100.0;
    pTool.setText("total frame time spent:", (object) this.trimPercent(num1), (float) num1, true);
    double num2 = benchResultAsDouble2 * 1000.0 / totalFrameBudget * 100.0;
    pTool.setText("total budget time spent:", (object) this.trimPercent(num2), (float) num2, true);
    pTool.setSeparator();
  }

  private void showGroupBenchmarkBottom(DebugTool pTool)
  {
    double totalFrameBudget = this.getTotalFrameBudget();
    float deltaTime = Time.deltaTime;
    List<ToolBenchmarkData> toolBenchmarkDataList = new List<ToolBenchmarkData>((IEnumerable<ToolBenchmarkData>) Bench.getGroup(pTool.asset.benchmark_group_id).dict_data.Values);
    if (!pTool.percentage_slowest)
    {
      double benchResultAsDouble = Bench.getBenchResultAsDouble(pTool.asset.benchmark_total, pTool.asset.benchmark_total_group, pTool.isValueAverage());
      foreach (ToolBenchmarkData toolBenchmarkData in toolBenchmarkDataList)
      {
        double num1 = toolBenchmarkData.latest_result;
        if (pTool.isValueAverage())
          num1 = toolBenchmarkData.getAverage();
        double num2 = num1 / benchResultAsDouble * 100.0;
        toolBenchmarkData.calculated_percentage = num2;
      }
    }
    else
    {
      double num3 = 0.0;
      foreach (ToolBenchmarkData toolBenchmarkData in toolBenchmarkDataList)
      {
        double num4 = pTool.isValueAverage() ? toolBenchmarkData.getAverage() : toolBenchmarkData.latest_result;
        if (num4 > num3)
          num3 = num4;
      }
      foreach (ToolBenchmarkData toolBenchmarkData in toolBenchmarkDataList)
      {
        double num5 = (pTool.isValueAverage() ? toolBenchmarkData.getAverage() : toolBenchmarkData.latest_result) / num3 * 100.0;
        if (((float) num5).Equals(100f))
          ++num5;
        toolBenchmarkData.calculated_percentage = num5;
      }
    }
    if (pTool.sort_by_names)
      toolBenchmarkDataList.Sort((Comparison<ToolBenchmarkData>) ((a, b) => b.id.CompareTo(a.id)));
    else if (pTool.isState(DebugToolState.Percent))
      toolBenchmarkDataList.Sort((Comparison<ToolBenchmarkData>) ((a, b) => a.calculated_percentage.CompareTo(b.calculated_percentage)));
    else if (pTool.isValueAverage())
      toolBenchmarkDataList.Sort((Comparison<ToolBenchmarkData>) ((a, b) => a.getAverage().CompareTo(b.getAverage())));
    else
      toolBenchmarkDataList.Sort((Comparison<ToolBenchmarkData>) ((a, b) => a.latest_result.CompareTo(b.latest_result)));
    if (!pTool.sort_order_reversed)
      toolBenchmarkDataList.Reverse();
    foreach (ToolBenchmarkData toolBenchmarkData in toolBenchmarkDataList)
    {
      double pValue1 = toolBenchmarkData.latest_result;
      if (pTool.isValueAverage())
        pValue1 = toolBenchmarkData.getAverage();
      long pCounter = 0;
      bool pShowCounter = false;
      bool showMax = pTool.show_max;
      string pMaxValue = string.Empty;
      if (pTool.asset.split_benchmark && pTool.show_counter)
      {
        pCounter = toolBenchmarkData.getAverageCount();
        pShowCounter = true;
      }
      else if (pTool.asset.show_last_count && pTool.show_counter)
      {
        pCounter = toolBenchmarkData.getLastCount();
        pShowCounter = true;
      }
      string pT1 = string.Empty;
      string pT2 = string.Empty;
      double pBarValue = 0.0;
      switch (pTool.state)
      {
        case DebugToolState.Values:
          if (!pTool.hide_zeroes || pValue1 >= 1E-06)
          {
            pT1 = toolBenchmarkData.id + ":";
            pT2 = this.trim(pValue1);
            pBarValue = toolBenchmarkData.calculated_percentage;
            toolBenchmarkData.saveLastMaxValue(pValue1);
            pMaxValue = this.trim(toolBenchmarkData.last_max_value);
            break;
          }
          continue;
        case DebugToolState.Percent:
          if (!pTool.hide_zeroes || toolBenchmarkData.calculated_percentage >= 0.1)
          {
            pT1 = toolBenchmarkData.id + ":";
            pT2 = this.trimPercent(toolBenchmarkData.calculated_percentage);
            pBarValue = toolBenchmarkData.calculated_percentage;
            toolBenchmarkData.saveLastMaxValue(toolBenchmarkData.calculated_percentage);
            pMaxValue = this.trimPercent(toolBenchmarkData.last_max_value);
            break;
          }
          continue;
        case DebugToolState.TimeSpent:
          double pValue2 = pValue1 / (double) deltaTime * 100.0;
          if (!pTool.hide_zeroes || pValue2 >= 0.1)
          {
            pT1 = toolBenchmarkData.id + ":";
            pT2 = this.trimPercent(pValue2);
            pBarValue = pValue2;
            toolBenchmarkData.saveLastMaxValue(pValue2);
            pMaxValue = this.trimPercent(toolBenchmarkData.last_max_value);
            break;
          }
          continue;
        case DebugToolState.FrameBudget:
          double pValue3 = pValue1 * 1000.0 / totalFrameBudget * 100.0;
          if (!pTool.hide_zeroes || pValue3 >= 0.1)
          {
            pT1 = toolBenchmarkData.id + ":";
            pT2 = this.trimPercent(pValue3);
            pBarValue = pValue3;
            toolBenchmarkData.saveLastMaxValue(pValue3);
            pMaxValue = this.trimPercent(toolBenchmarkData.last_max_value);
            break;
          }
          continue;
      }
      pTool.setText(pT1, (object) pT2, (float) pBarValue, true, pCounter, pShowCounter, showMax, pMaxValue);
    }
  }

  private string trim(double pValue, bool pAddMS = false)
  {
    pValue *= 1000.0;
    string str = pValue.ToString("F5");
    if (pAddMS)
      str += " ms";
    return str;
  }

  private string trimPercent(double pValue, bool pAddPercent = true)
  {
    string str = pValue.ToString("F1");
    if (pAddPercent)
      str += "%";
    return str;
  }

  private double getTotalFrameBudget()
  {
    double num = 60.0;
    if (Config.fps_lock_30)
      num = 30.0;
    return 1000.0 / num * 0.64999997615814209;
  }
}

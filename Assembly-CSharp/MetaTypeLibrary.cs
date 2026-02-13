// Decompiled with JetBrains decompiler
// Type: MetaTypeLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MetaTypeLibrary : AssetLibrary<MetaTypeAsset>
{
  [NonSerialized]
  public static MetaTypeAsset alliance;
  [NonSerialized]
  public static MetaTypeAsset city;
  [NonSerialized]
  public static MetaTypeAsset clan;
  [NonSerialized]
  public static MetaTypeAsset culture;
  [NonSerialized]
  public static MetaTypeAsset family;
  [NonSerialized]
  public static MetaTypeAsset army;
  [NonSerialized]
  public static MetaTypeAsset kingdom;
  [NonSerialized]
  public static MetaTypeAsset language;
  [NonSerialized]
  public static MetaTypeAsset plot;
  [NonSerialized]
  public static MetaTypeAsset religion;
  [NonSerialized]
  public static MetaTypeAsset subspecies;
  [NonSerialized]
  public static MetaTypeAsset unit;
  [NonSerialized]
  public static MetaTypeAsset war;
  [NonSerialized]
  public static MetaTypeAsset item;

  public override void init()
  {
    base.init();
    MetaTypeAsset pAsset1 = new MetaTypeAsset();
    pAsset1.id = "world";
    pAsset1.window_name = "world_info";
    pAsset1.get_list = (MetaTypeListAction) (() => (IEnumerable<NanoObject>) new NanoObject[0]);
    pAsset1.has_any = (MetaTypeListHasAction) (() => false);
    pAsset1.get_selected = (MetaSelectedGetter) (() => (NanoObject) World.world.world_object);
    pAsset1.set_selected = (MetaSelectedSetter) (pElement => World.world.world_object = pElement as WorldObject);
    pAsset1.get = (MetaGetter) (_ => (NanoObject) World.world.world_object);
    this.add(pAsset1);
    MetaTypeAsset pAsset2 = new MetaTypeAsset();
    pAsset2.id = "item";
    pAsset2.window_name = "item";
    pAsset2.window_action_clear = (MetaTypeAction) (() => SelectedMetas.selected_item = (Item) null);
    pAsset2.window_history_action_update = (MetaTypeHistoryAction) ((ref WindowHistoryData pHistoryData) =>
    {
      pHistoryData.item = SelectedMetas.selected_item;
      if (!WindowHistory.hasHistory() || ((Component) WindowHistory.list.Last<WindowHistoryData>().window).GetComponent<ItemWindow>() == null)
        return;
      ScrollWindow.setPreviousWindowSprite(pHistoryData.item.getSprite());
    });
    pAsset2.window_history_action_restore = (MetaTypeHistoryAction) ((ref WindowHistoryData pHistoryData) => SelectedMetas.selected_item = pHistoryData.item);
    pAsset2.get_list = (MetaTypeListAction) (() => (IEnumerable<NanoObject>) World.world.items);
    pAsset2.custom_sorted_list = (MetaTypeListPoolAction) (() =>
    {
      ListPool<NanoObject> listPool = new ListPool<NanoObject>(64 /*0x40*/);
      foreach (Item obj in (CoreSystemManager<Item, ItemData>) World.world.items)
      {
        if (obj.isFavorite())
          listPool.Add((NanoObject) obj);
      }
      return listPool;
    });
    pAsset2.has_any = (MetaTypeListHasAction) (() => World.world.items.hasAny());
    pAsset2.get_selected = (MetaSelectedGetter) (() => (NanoObject) SelectedMetas.selected_item);
    pAsset2.set_selected = (MetaSelectedSetter) (pElement => SelectedMetas.selected_item = pElement as Item);
    pAsset2.get = (MetaGetter) (pId => (NanoObject) World.world.items.get(pId));
    pAsset2.stat_hover = (MetaStatAction) ((pMetaId, pField) =>
    {
      Item pObject = World.world.items.get(pMetaId);
      if (pObject.isRekt())
        return;
      Tooltip.show((object) pField, "equipment", new TooltipData()
      {
        item = pObject
      });
    });
    pAsset2.stat_click = (MetaStatAction) ((pMetaId, _) =>
    {
      Item pObject = World.world.items.get(pMetaId);
      if (pObject.isRekt())
        return;
      SelectedMetas.selected_item = pObject;
      ScrollWindow.showWindow("item");
    });
    MetaTypeLibrary.item = this.add(pAsset2);
    MetaTypeAsset pAsset3 = new MetaTypeAsset();
    pAsset3.id = "unit";
    pAsset3.ranks = MetaTypeLibrary.generateExponentialRanks(100.0, 1.5);
    pAsset3.window_name = "unit";
    pAsset3.power_tab_id = "selected_unit";
    pAsset3.icon_single_path = "ui/icons/iconSpecies";
    pAsset3.window_action_clear = (MetaTypeAction) (() =>
    {
      if (!SelectedUnit.isSet() || !(SelectedObjects.getSelectedNanoObject() is Actor))
        return;
      PowerTabController.showTabSelectedUnit();
    });
    pAsset3.window_history_action_update = (MetaTypeHistoryAction) ((ref WindowHistoryData pHistoryData) =>
    {
      if (!SelectedUnit.isSet())
        return;
      pHistoryData.unit = SelectedUnit.unit;
      if (!WindowHistory.hasHistory())
        return;
      WindowHistoryData windowHistoryData = WindowHistory.list.Last<WindowHistoryData>();
      if (((Component) windowHistoryData.window).GetComponent<UnitWindow>() == null || windowHistoryData.unit.isRekt())
        return;
      ScrollWindow.setPreviousWindowSprite(windowHistoryData.unit.asset.getSpriteIcon());
    });
    pAsset3.window_history_action_restore = (MetaTypeHistoryAction) ((ref WindowHistoryData pHistoryData) =>
    {
      if (!pHistoryData.unit.isRekt())
      {
        SelectedUnit.clear();
        SelectedUnit.select(pHistoryData.unit);
      }
      else
        SelectedUnit.clear();
    });
    pAsset3.get_list = (MetaTypeListAction) (() => (IEnumerable<NanoObject>) World.world.units);
    pAsset3.custom_sorted_list = (MetaTypeListPoolAction) (() =>
    {
      ListPool<NanoObject> listPool = new ListPool<NanoObject>(64 /*0x40*/);
      foreach (Actor unit in (SimSystemManager<Actor, ActorData>) World.world.units)
      {
        if (!unit.isRekt() && unit.isFavorite())
          listPool.Add((NanoObject) unit);
      }
      return listPool;
    });
    pAsset3.has_any = (MetaTypeListHasAction) (() => World.world.units.Count > 0);
    pAsset3.get_selected = (MetaSelectedGetter) (() => (NanoObject) SelectedUnit.unit);
    pAsset3.set_selected = (MetaSelectedSetter) (pElement => SelectedUnit.select(pElement as Actor));
    pAsset3.get = (MetaGetter) (pId => (NanoObject) World.world.units.get(pId));
    pAsset3.stat_hover = (MetaStatAction) ((pMetaId, pField) =>
    {
      Actor pObject = World.world.units.get(pMetaId);
      if (pObject.isRekt())
        return;
      string pType = "actor";
      if (pObject.isKing())
        pType = "actor_king";
      if (pObject.isCityLeader())
        pType = "actor_leader";
      Tooltip.show((object) pField, pType, new TooltipData()
      {
        actor = pObject
      });
    });
    pAsset3.stat_click = (MetaStatAction) ((pMetaId, _) =>
    {
      Actor actor = World.world.units.get(pMetaId);
      if (actor.isRekt())
        return;
      ActionLibrary.openUnitWindow(actor);
    });
    MetaTypeLibrary.unit = this.add(pAsset3);
    MetaTypeAsset pAsset4 = new MetaTypeAsset();
    pAsset4.id = "war";
    pAsset4.window_name = "war";
    pAsset4.icon_list = "iconWarList";
    pAsset4.window_action_clear = (MetaTypeAction) (() => SelectedMetas.selected_war = (War) null);
    pAsset4.window_history_action_update = (MetaTypeHistoryAction) ((ref WindowHistoryData pHistoryData) => pHistoryData.war = SelectedMetas.selected_war);
    pAsset4.window_history_action_restore = (MetaTypeHistoryAction) ((ref WindowHistoryData pHistoryData) => SelectedMetas.selected_war = pHistoryData.war);
    pAsset4.reports = new string[7]
    {
      "war_high_casualties",
      "war_long",
      "war_fresh",
      "war_defenders_getting_captured",
      "war_attackers_getting_captured",
      "war_quiet",
      "war_full_on_battle"
    };
    pAsset4.get_list = (MetaTypeListAction) (() => (IEnumerable<NanoObject>) World.world.wars);
    pAsset4.has_any = (MetaTypeListHasAction) (() => World.world.wars.hasAny());
    pAsset4.get_selected = (MetaSelectedGetter) (() => (NanoObject) SelectedMetas.selected_war);
    pAsset4.set_selected = (MetaSelectedSetter) (pElement => SelectedMetas.selected_war = pElement as War);
    pAsset4.get = (MetaGetter) (pId => (NanoObject) World.world.wars.get(pId));
    pAsset4.stat_hover = (MetaStatAction) ((pMetaId, pField) =>
    {
      War pObject = World.world.wars.get(pMetaId);
      if (pObject.isRekt())
        return;
      Tooltip.show((object) pField, "war", new TooltipData()
      {
        war = pObject
      });
    });
    pAsset4.stat_click = (MetaStatAction) ((pMetaId, _) =>
    {
      War pObject = World.world.wars.get(pMetaId);
      if (pObject.isRekt())
        return;
      SelectedMetas.selected_war = pObject;
      ScrollWindow.showWindow("war");
    });
    MetaTypeLibrary.war = this.add(pAsset4);
    MetaTypeAsset pAsset5 = new MetaTypeAsset();
    pAsset5.id = "plot";
    pAsset5.window_name = "plot";
    pAsset5.icon_list = "iconPlotList";
    pAsset5.window_action_clear = (MetaTypeAction) (() => SelectedMetas.selected_plot = (Plot) null);
    pAsset5.window_history_action_update = (MetaTypeHistoryAction) ((ref WindowHistoryData pHistoryData) => pHistoryData.plot = SelectedMetas.selected_plot);
    pAsset5.window_history_action_restore = (MetaTypeHistoryAction) ((ref WindowHistoryData pHistoryData) => SelectedMetas.selected_plot = pHistoryData.plot);
    pAsset5.get_list = (MetaTypeListAction) (() => (IEnumerable<NanoObject>) World.world.plots);
    pAsset5.has_any = (MetaTypeListHasAction) (() => World.world.plots.hasAny());
    pAsset5.get_selected = (MetaSelectedGetter) (() => (NanoObject) SelectedMetas.selected_plot);
    pAsset5.set_selected = (MetaSelectedSetter) (pElement => SelectedMetas.selected_plot = pElement as Plot);
    pAsset5.get = (MetaGetter) (pId => (NanoObject) World.world.plots.get(pId));
    pAsset5.stat_hover = (MetaStatAction) ((pMetaId, pField) =>
    {
      Plot pObject = World.world.plots.get(pMetaId);
      if (pObject.isRekt())
        return;
      Tooltip.show((object) pField, "plot", new TooltipData()
      {
        plot = pObject
      });
    });
    pAsset5.stat_click = (MetaStatAction) ((pMetaId, _) =>
    {
      Plot pObject = World.world.plots.get(pMetaId);
      if (pObject.isRekt())
        return;
      SelectedMetas.selected_plot = pObject;
      ScrollWindow.showWindow("plot");
    });
    pAsset5.decision_ids = new string[1]{ "check_plot" };
    MetaTypeLibrary.plot = this.add(pAsset5);
    MetaTypeAsset pAsset6 = new MetaTypeAsset();
    pAsset6.id = "religion";
    pAsset6.ranks = MetaTypeLibrary.generateExponentialRanks(100.0, 1.5);
    pAsset6.window_name = "religion";
    pAsset6.power_tab_id = "selected_religion";
    pAsset6.force_zone_when_selected = true;
    pAsset6.set_icon_for_cancel_button = true;
    pAsset6.icon_list = "iconReligionList";
    pAsset6.icon_single_path = "ui/icons/iconReligion";
    pAsset6.window_action_clear = (MetaTypeAction) (() => SelectedMetas.selected_religion = (Religion) null);
    pAsset6.window_history_action_update = (MetaTypeHistoryAction) ((ref WindowHistoryData pHistoryData) => pHistoryData.religion = SelectedMetas.selected_religion);
    pAsset6.window_history_action_restore = (MetaTypeHistoryAction) ((ref WindowHistoryData pHistoryData) => SelectedMetas.selected_religion = pHistoryData.religion);
    pAsset6.reports = new string[4]
    {
      "happy",
      "unhappy",
      "many_children",
      "many_homeless"
    };
    pAsset6.get_list = (MetaTypeListAction) (() => (IEnumerable<NanoObject>) World.world.religions);
    pAsset6.has_any = (MetaTypeListHasAction) (() => World.world.religions.hasAny());
    pAsset6.get_selected = (MetaSelectedGetter) (() => (NanoObject) SelectedMetas.selected_religion);
    pAsset6.set_selected = (MetaSelectedSetter) (pElement => SelectedMetas.selected_religion = pElement as Religion);
    pAsset6.get = (MetaGetter) (pId => (NanoObject) World.world.religions.get(pId));
    pAsset6.map_mode = MetaType.Religion;
    pAsset6.option_id = "map_religion_layer";
    pAsset6.power_option_zone_id = "religion_layer";
    pAsset6.has_dynamic_zones = true;
    pAsset6.click_action_zone = new MetaZoneClickAction(ActionLibrary.inspectReligion);
    pAsset6.selected_tab_action_meta = new MetaTypeActionAsset(this.defaultClickActionZone);
    pAsset6.check_unit_has_meta = (MetaCheckUnitWindowAction) (pActor => pActor.hasReligion());
    pAsset6.set_unit_set_meta_for_meta_for_window = (MetaUnitSetMetaForWindow) (pActor => SelectedMetas.selected_religion = pActor.religion);
    pAsset6.draw_zones = (MetaZoneDrawAction) (pMetaTypeAsset =>
    {
      if (MetaTypeLibrary.religion.getZoneOptionState() == 2)
        this.drawDefaultFluid(pMetaTypeAsset);
      else
        this.drawDefaultMeta(pMetaTypeAsset);
    });
    pAsset6.check_cursor_highlight = (MetaZoneHighlightAction) ((pMetaTypeAsset, pTile, pQAsset) =>
    {
      Color color = pQAsset.color;
      switch (pMetaTypeAsset.getZoneOptionState())
      {
        case 0:
          City city1 = pTile.zone.city;
          if (city1.isRekt() || city1.kingdom.getReligion().isRekt())
            break;
          using (IEnumerator<City> enumerator = city1.kingdom.getCities().GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              City current = enumerator.Current;
              QuantumSpriteLibrary.colorZones(pQAsset, current.zones, color);
            }
            break;
          }
        case 1:
          City city2 = pTile.zone.city;
          if (city2.isRekt())
            break;
          Religion religion = city2.getReligion();
          if (religion.isRekt())
            break;
          using (IEnumerator<City> enumerator = World.world.cities.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              City current = enumerator.Current;
              if (current.getReligion() == religion)
                QuantumSpriteLibrary.colorZones(pQAsset, current.zones, color);
            }
            break;
          }
        default:
          this.highlightDefault(pTile, pQAsset, color);
          break;
      }
    });
    pAsset6.tile_get_metaobject = (MetaZoneGetMeta) ((pZone, pZoneOption) => pZone.getReligionOnZone(pZoneOption));
    pAsset6.tile_get_metaobject_0 = (MetaZoneGetMetaSimple) (pZone =>
    {
      City city = pZone.city;
      return city == null ? (IMetaObject) null : (IMetaObject) city.kingdom.religion;
    });
    pAsset6.tile_get_metaobject_1 = (MetaZoneGetMetaSimple) (pZone =>
    {
      City city = pZone.city;
      return city == null ? (IMetaObject) null : (IMetaObject) city.religion;
    });
    pAsset6.tile_get_metaobject_2 = (MetaZoneGetMetaSimple) (pZone => ZoneMetaDataVisualizer.getZoneMetaData(pZone).meta_object);
    pAsset6.check_tile_has_meta = new MetaZoneTooltipAction(this.checkTileHasMetaDefault);
    pAsset6.check_cursor_tooltip = new MetaZoneTooltipAction(this.checkCursorTooltipDefault);
    pAsset6.cursor_tooltip_action = (MetaTooltipShowAction) (pMeta =>
    {
      Religion religion = pMeta as Religion;
      if (religion.isRekt())
        return;
      string str = "religion";
      Tooltip.hideTooltip((object) religion, true, str);
      Tooltip.show((object) religion, str, new TooltipData()
      {
        religion = religion,
        tooltip_scale = 0.7f,
        is_sim_tooltip = true
      });
    });
    pAsset6.dynamic_zones = (MetaZoneDynamicAction) (() =>
    {
      List<Actor> simpleList = World.world.units.getSimpleList();
      double curWorldTime = World.world.getCurWorldTime();
      int index = 0;
      for (int count = simpleList.Count; index < count; ++index)
      {
        Actor actor = simpleList[index];
        if (actor.asset.show_on_meta_layer)
        {
          TileZone zone = actor.current_tile.zone;
          if (actor.hasReligion())
            ZoneMetaDataVisualizer.countMetaZone(zone, (IMetaObject) actor.religion, curWorldTime);
        }
      }
    });
    pAsset6.stat_hover = (MetaStatAction) ((pMetaId, pField) =>
    {
      Religion pObject = World.world.religions.get(pMetaId);
      if (pObject.isRekt())
        return;
      Tooltip.show((object) pField, "religion", new TooltipData()
      {
        religion = pObject
      });
    });
    pAsset6.stat_click = (MetaStatAction) ((pMetaId, _) =>
    {
      Religion pObject = World.world.religions.get(pMetaId);
      if (pObject.isRekt())
        return;
      SelectedMetas.selected_religion = pObject;
      ScrollWindow.showWindow("religion");
    });
    MetaTypeLibrary.religion = this.add(pAsset6);
    MetaTypeAsset pAsset7 = new MetaTypeAsset();
    pAsset7.id = "culture";
    pAsset7.ranks = MetaTypeLibrary.generateExponentialRanks(100.0, 1.5);
    pAsset7.window_name = "culture";
    pAsset7.power_tab_id = "selected_culture";
    pAsset7.force_zone_when_selected = true;
    pAsset7.set_icon_for_cancel_button = true;
    pAsset7.icon_list = "iconCultureList";
    pAsset7.icon_single_path = "ui/icons/iconCulture";
    pAsset7.window_action_clear = (MetaTypeAction) (() => SelectedMetas.selected_culture = (Culture) null);
    pAsset7.window_history_action_update = (MetaTypeHistoryAction) ((ref WindowHistoryData pHistoryData) => pHistoryData.culture = SelectedMetas.selected_culture);
    pAsset7.window_history_action_restore = (MetaTypeHistoryAction) ((ref WindowHistoryData pHistoryData) => SelectedMetas.selected_culture = pHistoryData.culture);
    pAsset7.reports = new string[4]
    {
      "happy",
      "unhappy",
      "many_children",
      "many_homeless"
    };
    pAsset7.get_list = (MetaTypeListAction) (() => (IEnumerable<NanoObject>) World.world.cultures);
    pAsset7.has_any = (MetaTypeListHasAction) (() => World.world.cultures.hasAny());
    pAsset7.get_selected = (MetaSelectedGetter) (() => (NanoObject) SelectedMetas.selected_culture);
    pAsset7.set_selected = (MetaSelectedSetter) (pElement => SelectedMetas.selected_culture = pElement as Culture);
    pAsset7.get = (MetaGetter) (pId => (NanoObject) World.world.cultures.get(pId));
    pAsset7.map_mode = MetaType.Culture;
    pAsset7.option_id = "map_culture_layer";
    pAsset7.power_option_zone_id = "culture_layer";
    pAsset7.has_dynamic_zones = true;
    pAsset7.click_action_zone = new MetaZoneClickAction(ActionLibrary.inspectCulture);
    pAsset7.selected_tab_action_meta = new MetaTypeActionAsset(this.defaultClickActionZone);
    pAsset7.check_unit_has_meta = (MetaCheckUnitWindowAction) (pActor => pActor.hasCulture());
    pAsset7.set_unit_set_meta_for_meta_for_window = (MetaUnitSetMetaForWindow) (pActor => SelectedMetas.selected_culture = pActor.culture);
    pAsset7.draw_zones = (MetaZoneDrawAction) (pMetaTypeAsset =>
    {
      if (pMetaTypeAsset.isMetaZoneOptionSelectedFluid())
        this.drawDefaultFluid(pMetaTypeAsset);
      else
        this.drawDefaultMeta(pMetaTypeAsset);
    });
    pAsset7.check_cursor_highlight = (MetaZoneHighlightAction) ((pMetaTypeAsset, pTile, pQAsset) =>
    {
      Color color = pQAsset.color;
      switch (pMetaTypeAsset.getZoneOptionState())
      {
        case 0:
          City city3 = pTile.zone.city;
          if (city3.isRekt() || city3.kingdom.getCulture().isRekt())
            break;
          using (IEnumerator<City> enumerator = city3.kingdom.getCities().GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              City current = enumerator.Current;
              QuantumSpriteLibrary.colorZones(pQAsset, current.zones, color);
            }
            break;
          }
        case 1:
          City city4 = pTile.zone.city;
          if (city4.isRekt())
            break;
          Culture culture = city4.getCulture();
          if (culture.isRekt())
            break;
          using (IEnumerator<City> enumerator = World.world.cities.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              City current = enumerator.Current;
              if (current.getCulture() == culture)
                QuantumSpriteLibrary.colorZones(pQAsset, current.zones, color);
            }
            break;
          }
        default:
          this.highlightDefault(pTile, pQAsset, color);
          break;
      }
    });
    pAsset7.tile_get_metaobject = (MetaZoneGetMeta) ((pZone, pZoneOption) => pZone.getCultureOnZone(pZoneOption));
    pAsset7.tile_get_metaobject_0 = (MetaZoneGetMetaSimple) (pZone =>
    {
      City city = pZone.city;
      return city == null ? (IMetaObject) null : (IMetaObject) city.kingdom.culture;
    });
    pAsset7.tile_get_metaobject_1 = (MetaZoneGetMetaSimple) (pZone =>
    {
      City city = pZone.city;
      return city == null ? (IMetaObject) null : (IMetaObject) city.culture;
    });
    pAsset7.tile_get_metaobject_2 = (MetaZoneGetMetaSimple) (pZone => ZoneMetaDataVisualizer.getZoneMetaData(pZone).meta_object);
    pAsset7.check_tile_has_meta = new MetaZoneTooltipAction(this.checkTileHasMetaDefault);
    pAsset7.check_cursor_tooltip = new MetaZoneTooltipAction(this.checkCursorTooltipDefault);
    pAsset7.cursor_tooltip_action = (MetaTooltipShowAction) (pMeta =>
    {
      Culture culture = pMeta as Culture;
      if (culture.isRekt())
        return;
      string str = "culture";
      Tooltip.hideTooltip((object) culture, true, str);
      Tooltip.show((object) culture, str, new TooltipData()
      {
        culture = culture,
        tooltip_scale = 0.7f,
        is_sim_tooltip = true
      });
    });
    pAsset7.dynamic_zones = (MetaZoneDynamicAction) (() =>
    {
      List<Actor> simpleList = World.world.units.getSimpleList();
      double curWorldTime = World.world.getCurWorldTime();
      int index = 0;
      for (int count = simpleList.Count; index < count; ++index)
      {
        Actor actor = simpleList[index];
        if (actor.asset.show_on_meta_layer)
        {
          TileZone zone = actor.current_tile.zone;
          if (actor.hasCulture())
            ZoneMetaDataVisualizer.countMetaZone(zone, (IMetaObject) actor.culture, curWorldTime);
        }
      }
    });
    pAsset7.stat_hover = (MetaStatAction) ((pMetaId, pField) =>
    {
      Culture pObject = World.world.cultures.get(pMetaId);
      if (pObject.isRekt())
        return;
      Tooltip.show((object) pField, "culture", new TooltipData()
      {
        culture = pObject
      });
    });
    pAsset7.stat_click = (MetaStatAction) ((pMetaId, _) =>
    {
      Culture pObject = World.world.cultures.get(pMetaId);
      if (pObject.isRekt())
        return;
      SelectedMetas.selected_culture = pObject;
      ScrollWindow.showWindow("culture");
    });
    MetaTypeLibrary.culture = this.add(pAsset7);
    MetaTypeAsset pAsset8 = new MetaTypeAsset();
    pAsset8.id = "family";
    pAsset8.window_name = "family";
    pAsset8.power_tab_id = "selected_family";
    pAsset8.force_zone_when_selected = true;
    pAsset8.set_icon_for_cancel_button = true;
    pAsset8.unit_amount_alpha = true;
    pAsset8.icon_list = "iconFamilyList";
    pAsset8.icon_single_path = "ui/icons/iconFamily";
    pAsset8.window_action_clear = (MetaTypeAction) (() => SelectedMetas.selected_family = (Family) null);
    pAsset8.window_history_action_update = (MetaTypeHistoryAction) ((ref WindowHistoryData pHistoryData) => pHistoryData.family = SelectedMetas.selected_family);
    pAsset8.window_history_action_restore = (MetaTypeHistoryAction) ((ref WindowHistoryData pHistoryData) => SelectedMetas.selected_family = pHistoryData.family);
    pAsset8.reports = new string[4]
    {
      "happy",
      "unhappy",
      "many_children",
      "many_homeless"
    };
    pAsset8.get_list = (MetaTypeListAction) (() => (IEnumerable<NanoObject>) World.world.families);
    pAsset8.has_any = (MetaTypeListHasAction) (() => World.world.families.hasAny());
    pAsset8.get_selected = (MetaSelectedGetter) (() => (NanoObject) SelectedMetas.selected_family);
    pAsset8.set_selected = (MetaSelectedSetter) (pElement => SelectedMetas.selected_family = pElement as Family);
    pAsset8.get = (MetaGetter) (pId => (NanoObject) World.world.families.get(pId));
    pAsset8.map_mode = MetaType.Family;
    pAsset8.option_id = "map_family_layer";
    pAsset8.power_option_zone_id = "family_layer";
    pAsset8.has_dynamic_zones = true;
    pAsset8.decision_ids = new string[5]
    {
      "family_check_existence",
      "family_alpha_move",
      "family_group_follow",
      "family_group_leave",
      "child_follow_parent"
    };
    pAsset8.click_action_zone = new MetaZoneClickAction(ActionLibrary.inspectFamily);
    pAsset8.selected_tab_action_meta = new MetaTypeActionAsset(this.defaultClickActionZone);
    pAsset8.check_unit_has_meta = (MetaCheckUnitWindowAction) (pActor => pActor.hasFamily());
    pAsset8.set_unit_set_meta_for_meta_for_window = (MetaUnitSetMetaForWindow) (pActor => SelectedMetas.selected_family = pActor.family);
    pAsset8.draw_zones = (MetaZoneDrawAction) (pMetaTypeAsset =>
    {
      if (pMetaTypeAsset.isMetaZoneOptionSelectedFluid())
        this.drawDefaultFluid(pMetaTypeAsset);
      else
        this.drawDefaultMeta(pMetaTypeAsset);
    });
    pAsset8.check_cursor_highlight = (MetaZoneHighlightAction) ((pMetaTypeAsset, pTile, pQAsset) =>
    {
      Color color = pQAsset.color;
      switch (pMetaTypeAsset.getZoneOptionState())
      {
        case 0:
          City city5 = pTile.zone.city;
          if (city5.isRekt() || !city5.kingdom.hasKing() || !city5.kingdom.king.hasFamily() || city5.kingdom.king.family.isRekt())
            break;
          using (IEnumerator<City> enumerator = city5.kingdom.getCities().GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              City current = enumerator.Current;
              QuantumSpriteLibrary.colorZones(pQAsset, current.zones, color);
            }
            break;
          }
        case 1:
          City city6 = pTile.zone.city;
          if (city6.isRekt() || !city6.hasLeader() || !city6.leader.hasFamily())
            break;
          Family family = city6.leader.family;
          if (family.isRekt())
            break;
          using (IEnumerator<City> enumerator = World.world.cities.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              City current = enumerator.Current;
              if (!current.hasLeader() || !current.leader.hasFamily())
                break;
              if (current.leader.family == family)
                QuantumSpriteLibrary.colorZones(pQAsset, current.zones, color);
            }
            break;
          }
        default:
          this.highlightDefault(pTile, pQAsset, color);
          break;
      }
    });
    pAsset8.tile_get_metaobject = (MetaZoneGetMeta) ((pZone, pZoneOption) => pZone.getFamilyOnZone(pZoneOption));
    pAsset8.tile_get_metaobject_0 = (MetaZoneGetMetaSimple) (pZone =>
    {
      City city = pZone.city;
      if (city == null)
        return (IMetaObject) null;
      Actor king = city.kingdom.king;
      return king == null ? (IMetaObject) null : (IMetaObject) king.family;
    });
    pAsset8.tile_get_metaobject_1 = (MetaZoneGetMetaSimple) (pZone =>
    {
      City city = pZone.city;
      if (city == null)
        return (IMetaObject) null;
      Actor leader = city.leader;
      return leader == null ? (IMetaObject) null : (IMetaObject) leader.family;
    });
    pAsset8.tile_get_metaobject_2 = (MetaZoneGetMetaSimple) (pZone => ZoneMetaDataVisualizer.getZoneMetaData(pZone).meta_object);
    pAsset8.check_tile_has_meta = new MetaZoneTooltipAction(this.checkTileHasMetaDefault);
    pAsset8.check_cursor_tooltip = new MetaZoneTooltipAction(this.checkCursorTooltipDefault);
    pAsset8.cursor_tooltip_action = (MetaTooltipShowAction) (pMeta =>
    {
      Family family = pMeta as Family;
      if (family.isRekt())
        return;
      string str = "family";
      Tooltip.hideTooltip((object) family, true, str);
      Tooltip.show((object) family, str, new TooltipData()
      {
        family = family,
        tooltip_scale = 0.7f,
        is_sim_tooltip = true
      });
    });
    pAsset8.dynamic_zones = (MetaZoneDynamicAction) (() =>
    {
      List<Actor> simpleList = World.world.units.getSimpleList();
      double curWorldTime = World.world.getCurWorldTime();
      int index = 0;
      for (int count = simpleList.Count; index < count; ++index)
      {
        Actor actor = simpleList[index];
        if (actor.asset.show_on_meta_layer)
        {
          TileZone zone = actor.current_tile.zone;
          if (actor.hasFamily() && actor.family.units.Count >= 2)
            ZoneMetaDataVisualizer.countMetaZone(zone, (IMetaObject) actor.family, curWorldTime);
        }
      }
    });
    pAsset8.stat_hover = (MetaStatAction) ((pMetaId, pField) =>
    {
      Family pObject = World.world.families.get(pMetaId);
      if (pObject.isRekt())
        return;
      Tooltip.show((object) pField, "family", new TooltipData()
      {
        family = pObject
      });
    });
    pAsset8.stat_click = (MetaStatAction) ((pMetaId, _) =>
    {
      Family pObject = World.world.families.get(pMetaId);
      if (pObject.isRekt())
        return;
      SelectedMetas.selected_family = pObject;
      ScrollWindow.showWindow("family");
    });
    MetaTypeLibrary.family = this.add(pAsset8);
    MetaTypeAsset pAsset9 = new MetaTypeAsset();
    pAsset9.id = "army";
    pAsset9.window_name = "army";
    pAsset9.power_tab_id = "selected_army";
    pAsset9.force_zone_when_selected = true;
    pAsset9.set_icon_for_cancel_button = true;
    pAsset9.icon_list = "iconArmyList";
    pAsset9.icon_single_path = "ui/icons/iconArmy";
    pAsset9.window_action_clear = (MetaTypeAction) (() => SelectedMetas.selected_army = (Army) null);
    pAsset9.window_history_action_update = (MetaTypeHistoryAction) ((ref WindowHistoryData pHistoryData) => pHistoryData.army = SelectedMetas.selected_army);
    pAsset9.window_history_action_restore = (MetaTypeHistoryAction) ((ref WindowHistoryData pHistoryData) => SelectedMetas.selected_army = pHistoryData.army);
    pAsset9.get_list = (MetaTypeListAction) (() => (IEnumerable<NanoObject>) World.world.armies);
    pAsset9.has_any = (MetaTypeListHasAction) (() => World.world.armies.hasAny());
    pAsset9.get_selected = (MetaSelectedGetter) (() => (NanoObject) SelectedMetas.selected_army);
    pAsset9.set_selected = (MetaSelectedSetter) (pElement => SelectedMetas.selected_army = pElement as Army);
    pAsset9.get = (MetaGetter) (pId => (NanoObject) World.world.armies.get(pId));
    pAsset9.map_mode = MetaType.Army;
    pAsset9.option_id = "map_army_layer";
    pAsset9.power_option_zone_id = "army_layer";
    pAsset9.has_dynamic_zones = true;
    pAsset9.dynamic_zone_option = 0;
    pAsset9.click_action_zone = new MetaZoneClickAction(ActionLibrary.inspectArmy);
    pAsset9.selected_tab_action_meta = new MetaTypeActionAsset(this.defaultClickActionZone);
    pAsset9.check_unit_has_meta = (MetaCheckUnitWindowAction) (pActor => pActor.hasArmy());
    pAsset9.set_unit_set_meta_for_meta_for_window = (MetaUnitSetMetaForWindow) (pActor => SelectedMetas.selected_army = pActor.army);
    pAsset9.reports = new string[2]{ "happy", "unhappy" };
    pAsset9.draw_zones = (MetaZoneDrawAction) (pMetaTypeAsset =>
    {
      if (pMetaTypeAsset.isMetaZoneOptionSelectedFluid())
        this.drawDefaultFluid(pMetaTypeAsset);
      else
        this.drawDefaultMeta(pMetaTypeAsset);
    });
    pAsset9.check_cursor_highlight = (MetaZoneHighlightAction) ((pMetaTypeAsset, pTile, pQAsset) =>
    {
      Color color = pQAsset.color;
      pMetaTypeAsset.getZoneOptionState();
      this.highlightDefault(pTile, pQAsset, color);
    });
    pAsset9.tile_get_metaobject = (MetaZoneGetMeta) ((pZone, pZoneOption) => pZone.getArmyOnZone(pZoneOption));
    pAsset9.tile_get_metaobject_0 = (MetaZoneGetMetaSimple) (_ => (IMetaObject) null);
    pAsset9.tile_get_metaobject_1 = (MetaZoneGetMetaSimple) (_ => (IMetaObject) null);
    pAsset9.tile_get_metaobject_2 = (MetaZoneGetMetaSimple) (pZone => ZoneMetaDataVisualizer.getZoneMetaData(pZone).meta_object);
    pAsset9.check_tile_has_meta = new MetaZoneTooltipAction(this.checkTileHasMetaDefault);
    pAsset9.check_cursor_tooltip = new MetaZoneTooltipAction(this.checkCursorTooltipDefault);
    pAsset9.cursor_tooltip_action = (MetaTooltipShowAction) (pMeta =>
    {
      Army army = pMeta as Army;
      if (army.isRekt())
        return;
      string str = "army";
      Tooltip.hideTooltip((object) army, true, str);
      Tooltip.show((object) army, str, new TooltipData()
      {
        army = army,
        tooltip_scale = 0.7f,
        is_sim_tooltip = true
      });
    });
    pAsset9.dynamic_zones = (MetaZoneDynamicAction) (() =>
    {
      List<Actor> simpleList = World.world.units.getSimpleList();
      double curWorldTime = World.world.getCurWorldTime();
      int index = 0;
      for (int count = simpleList.Count; index < count; ++index)
      {
        Actor actor = simpleList[index];
        if (actor.asset.show_on_meta_layer)
        {
          TileZone zone = actor.current_tile.zone;
          if (actor.hasArmy())
            ZoneMetaDataVisualizer.countMetaZone(zone, (IMetaObject) actor.army, curWorldTime);
        }
      }
    });
    pAsset9.stat_hover = (MetaStatAction) ((pMetaId, pField) =>
    {
      Army pObject = World.world.armies.get(pMetaId);
      if (pObject.isRekt())
        return;
      Tooltip.show((object) pField, "army", new TooltipData()
      {
        army = pObject
      });
    });
    pAsset9.stat_click = (MetaStatAction) ((pMetaId, _) =>
    {
      Army pObject = World.world.armies.get(pMetaId);
      if (pObject.isRekt())
        return;
      SelectedMetas.selected_army = pObject;
      ScrollWindow.showWindow("army");
    });
    MetaTypeLibrary.army = this.add(pAsset9);
    MetaTypeAsset pAsset10 = new MetaTypeAsset();
    pAsset10.id = "language";
    pAsset10.ranks = MetaTypeLibrary.generateExponentialRanks(100.0, 1.5);
    pAsset10.window_name = "language";
    pAsset10.power_tab_id = "selected_language";
    pAsset10.force_zone_when_selected = true;
    pAsset10.set_icon_for_cancel_button = true;
    pAsset10.icon_list = "iconLanguageList";
    pAsset10.icon_single_path = "ui/icons/iconLanguage";
    pAsset10.window_action_clear = (MetaTypeAction) (() => SelectedMetas.selected_language = (Language) null);
    pAsset10.window_history_action_update = (MetaTypeHistoryAction) ((ref WindowHistoryData pHistoryData) => pHistoryData.language = SelectedMetas.selected_language);
    pAsset10.window_history_action_restore = (MetaTypeHistoryAction) ((ref WindowHistoryData pHistoryData) => SelectedMetas.selected_language = pHistoryData.language);
    pAsset10.reports = new string[4]
    {
      "happy",
      "unhappy",
      "many_children",
      "many_homeless"
    };
    pAsset10.get_list = (MetaTypeListAction) (() => (IEnumerable<NanoObject>) World.world.languages);
    pAsset10.has_any = (MetaTypeListHasAction) (() => World.world.languages.hasAny());
    pAsset10.get_selected = (MetaSelectedGetter) (() => (NanoObject) SelectedMetas.selected_language);
    pAsset10.set_selected = (MetaSelectedSetter) (pElement => SelectedMetas.selected_language = pElement as Language);
    pAsset10.get = (MetaGetter) (pId => (NanoObject) World.world.languages.get(pId));
    pAsset10.map_mode = MetaType.Language;
    pAsset10.option_id = "map_language_layer";
    pAsset10.power_option_zone_id = "language_layer";
    pAsset10.has_dynamic_zones = true;
    pAsset10.click_action_zone = new MetaZoneClickAction(ActionLibrary.inspectLanguage);
    pAsset10.selected_tab_action_meta = new MetaTypeActionAsset(this.defaultClickActionZone);
    pAsset10.check_unit_has_meta = (MetaCheckUnitWindowAction) (pActor => pActor.hasLanguage());
    pAsset10.set_unit_set_meta_for_meta_for_window = (MetaUnitSetMetaForWindow) (pActor => SelectedMetas.selected_language = pActor.language);
    pAsset10.draw_zones = (MetaZoneDrawAction) (pMetaTypeAsset =>
    {
      if (pMetaTypeAsset.isMetaZoneOptionSelectedFluid())
        this.drawDefaultFluid(pMetaTypeAsset);
      else
        this.drawDefaultMeta(pMetaTypeAsset);
    });
    pAsset10.check_cursor_highlight = (MetaZoneHighlightAction) ((pMetaTypeAsset, pTile, pQAsset) =>
    {
      Color color = pQAsset.color;
      switch (pMetaTypeAsset.getZoneOptionState())
      {
        case 0:
          City city7 = pTile.zone.city;
          if (city7.isRekt() || city7.kingdom.getLanguage().isRekt())
            break;
          using (IEnumerator<City> enumerator = city7.kingdom.getCities().GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              City current = enumerator.Current;
              QuantumSpriteLibrary.colorZones(pQAsset, current.zones, color);
            }
            break;
          }
        case 1:
          City city8 = pTile.zone.city;
          if (city8.isRekt())
            break;
          Language language = city8.getLanguage();
          if (language.isRekt())
            break;
          using (IEnumerator<City> enumerator = World.world.cities.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              City current = enumerator.Current;
              if (current.getLanguage() == language)
                QuantumSpriteLibrary.colorZones(pQAsset, current.zones, color);
            }
            break;
          }
        default:
          this.highlightDefault(pTile, pQAsset, color);
          break;
      }
    });
    pAsset10.tile_get_metaobject = (MetaZoneGetMeta) ((pZone, pZoneOption) => pZone.getLanguageOnZone(pZoneOption));
    pAsset10.tile_get_metaobject_0 = (MetaZoneGetMetaSimple) (pZone =>
    {
      City city = pZone.city;
      return city == null ? (IMetaObject) null : (IMetaObject) city.kingdom.getLanguage();
    });
    pAsset10.tile_get_metaobject_1 = (MetaZoneGetMetaSimple) (pZone =>
    {
      City city = pZone.city;
      return city == null ? (IMetaObject) null : (IMetaObject) city.getLanguage();
    });
    pAsset10.tile_get_metaobject_2 = (MetaZoneGetMetaSimple) (pZone => ZoneMetaDataVisualizer.getZoneMetaData(pZone).meta_object);
    pAsset10.check_tile_has_meta = new MetaZoneTooltipAction(this.checkTileHasMetaDefault);
    pAsset10.check_cursor_tooltip = new MetaZoneTooltipAction(this.checkCursorTooltipDefault);
    pAsset10.cursor_tooltip_action = (MetaTooltipShowAction) (pMeta =>
    {
      Language language = pMeta as Language;
      if (language.isRekt())
        return;
      string str = "language";
      Tooltip.hideTooltip((object) language, true, str);
      Tooltip.show((object) language, str, new TooltipData()
      {
        language = language,
        tooltip_scale = 0.7f,
        is_sim_tooltip = true
      });
    });
    pAsset10.dynamic_zones = (MetaZoneDynamicAction) (() =>
    {
      List<Actor> simpleList = World.world.units.getSimpleList();
      double curWorldTime = World.world.getCurWorldTime();
      int index = 0;
      for (int count = simpleList.Count; index < count; ++index)
      {
        Actor actor = simpleList[index];
        if (actor.asset.show_on_meta_layer)
        {
          TileZone zone = actor.current_tile.zone;
          if (actor.hasLanguage())
            ZoneMetaDataVisualizer.countMetaZone(zone, (IMetaObject) actor.language, curWorldTime);
        }
      }
    });
    pAsset10.stat_hover = (MetaStatAction) ((pMetaId, pField) =>
    {
      Language pObject = World.world.languages.get(pMetaId);
      if (pObject.isRekt())
        return;
      Tooltip.show((object) pField, "language", new TooltipData()
      {
        language = pObject
      });
    });
    pAsset10.stat_click = (MetaStatAction) ((pMetaId, _) =>
    {
      Language pObject = World.world.languages.get(pMetaId);
      if (pObject.isRekt())
        return;
      SelectedMetas.selected_language = pObject;
      ScrollWindow.showWindow("language");
    });
    MetaTypeLibrary.language = this.add(pAsset10);
    MetaTypeAsset pAsset11 = new MetaTypeAsset();
    pAsset11.id = "subspecies";
    pAsset11.ranks = MetaTypeLibrary.generateExponentialRanks(100.0, 1.5);
    pAsset11.window_name = "subspecies";
    pAsset11.power_tab_id = "selected_subspecies";
    pAsset11.force_zone_when_selected = true;
    pAsset11.set_icon_for_cancel_button = true;
    pAsset11.unit_amount_alpha = true;
    pAsset11.icon_list = "iconSubspeciesList";
    pAsset11.icon_single_path = "ui/icons/iconSpecies";
    pAsset11.window_action_clear = (MetaTypeAction) (() => SelectedMetas.selected_subspecies = (Subspecies) null);
    pAsset11.window_history_action_update = (MetaTypeHistoryAction) ((ref WindowHistoryData pHistoryData) => pHistoryData.subspecies = SelectedMetas.selected_subspecies);
    pAsset11.window_history_action_restore = (MetaTypeHistoryAction) ((ref WindowHistoryData pHistoryData) => SelectedMetas.selected_subspecies = pHistoryData.subspecies);
    pAsset11.reports = new string[4]
    {
      "happy",
      "unhappy",
      "many_children",
      "many_homeless"
    };
    pAsset11.get_list = (MetaTypeListAction) (() => (IEnumerable<NanoObject>) World.world.subspecies);
    pAsset11.has_any = (MetaTypeListHasAction) (() => World.world.subspecies.hasAny());
    pAsset11.get_selected = (MetaSelectedGetter) (() => (NanoObject) SelectedMetas.selected_subspecies);
    pAsset11.set_selected = (MetaSelectedSetter) (pElement => SelectedMetas.selected_subspecies = pElement as Subspecies);
    pAsset11.get = (MetaGetter) (pId => (NanoObject) World.world.subspecies.get(pId));
    pAsset11.map_mode = MetaType.Subspecies;
    pAsset11.option_id = "map_subspecies_layer";
    pAsset11.power_option_zone_id = "subspecies_layer";
    pAsset11.has_dynamic_zones = true;
    pAsset11.click_action_zone = new MetaZoneClickAction(ActionLibrary.inspectSubspecies);
    pAsset11.selected_tab_action_meta = new MetaTypeActionAsset(this.defaultClickActionZone);
    pAsset11.check_unit_has_meta = (MetaCheckUnitWindowAction) (pActor => pActor.hasSubspecies());
    pAsset11.set_unit_set_meta_for_meta_for_window = (MetaUnitSetMetaForWindow) (pActor => SelectedMetas.selected_subspecies = pActor.subspecies);
    pAsset11.draw_zones = (MetaZoneDrawAction) (pMetaTypeAsset =>
    {
      if (pMetaTypeAsset.isMetaZoneOptionSelectedFluid())
        this.drawDefaultFluid(pMetaTypeAsset);
      else
        this.drawDefaultMeta(pMetaTypeAsset);
    });
    pAsset11.check_cursor_highlight = (MetaZoneHighlightAction) ((pMetaTypeAsset, pTile, pQAsset) =>
    {
      Color color = pQAsset.color;
      switch (pMetaTypeAsset.getZoneOptionState())
      {
        case 0:
          City city9 = pTile.zone.city;
          if (city9.isRekt())
            break;
          Subspecies mainSubspecies1 = city9.kingdom.getMainSubspecies();
          if (mainSubspecies1.isRekt())
            break;
          using (IEnumerator<City> enumerator = World.world.cities.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              City current = enumerator.Current;
              if (current.getMainSubspecies() == mainSubspecies1)
                QuantumSpriteLibrary.colorZones(pQAsset, current.zones, color);
            }
            break;
          }
        case 1:
          City city10 = pTile.zone.city;
          if (city10.isRekt())
            break;
          Subspecies mainSubspecies2 = city10.getMainSubspecies();
          if (mainSubspecies2.isRekt())
            break;
          using (IEnumerator<City> enumerator = World.world.cities.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              City current = enumerator.Current;
              if (current.getMainSubspecies() == mainSubspecies2)
                QuantumSpriteLibrary.colorZones(pQAsset, current.zones, color);
            }
            break;
          }
        default:
          this.highlightDefault(pTile, pQAsset, color);
          break;
      }
    });
    pAsset11.tile_get_metaobject = (MetaZoneGetMeta) ((pZone, pZoneOption) => pZone.getSubspeciesOnZone(pZoneOption));
    pAsset11.tile_get_metaobject_0 = (MetaZoneGetMetaSimple) (pZone =>
    {
      City city = pZone.city;
      return city == null ? (IMetaObject) null : (IMetaObject) city.kingdom.getMainSubspecies();
    });
    pAsset11.tile_get_metaobject_1 = (MetaZoneGetMetaSimple) (pZone =>
    {
      City city = pZone.city;
      return city == null ? (IMetaObject) null : (IMetaObject) city.getMainSubspecies();
    });
    pAsset11.tile_get_metaobject_2 = (MetaZoneGetMetaSimple) (pZone => ZoneMetaDataVisualizer.getZoneMetaData(pZone).meta_object);
    pAsset11.check_tile_has_meta = new MetaZoneTooltipAction(this.checkTileHasMetaDefault);
    pAsset11.check_cursor_tooltip = new MetaZoneTooltipAction(this.checkCursorTooltipDefault);
    pAsset11.cursor_tooltip_action = (MetaTooltipShowAction) (pMeta =>
    {
      Subspecies subspecies = pMeta as Subspecies;
      if (subspecies.isRekt())
        return;
      string str = "subspecies";
      Tooltip.hideTooltip((object) subspecies, true, str);
      Tooltip.show((object) subspecies, str, new TooltipData()
      {
        subspecies = subspecies,
        tooltip_scale = 0.7f,
        is_sim_tooltip = true
      });
    });
    pAsset11.dynamic_zones = (MetaZoneDynamicAction) (() =>
    {
      List<Actor> simpleList = World.world.units.getSimpleList();
      double curWorldTime = World.world.getCurWorldTime();
      int index = 0;
      for (int count = simpleList.Count; index < count; ++index)
      {
        Actor actor = simpleList[index];
        if (actor.asset.show_on_meta_layer)
        {
          TileZone zone = actor.current_tile.zone;
          if (actor.hasSubspecies())
            ZoneMetaDataVisualizer.countMetaZone(zone, (IMetaObject) actor.subspecies, curWorldTime);
        }
      }
    });
    pAsset11.stat_hover = (MetaStatAction) ((pMetaId, pField) =>
    {
      Subspecies pObject = World.world.subspecies.get(pMetaId);
      if (pObject.isRekt())
        return;
      Tooltip.show((object) pField, "subspecies", new TooltipData()
      {
        subspecies = pObject
      });
    });
    pAsset11.stat_click = (MetaStatAction) ((pMetaId, _) =>
    {
      Subspecies pObject = World.world.subspecies.get(pMetaId);
      if (pObject.isRekt())
        return;
      SelectedMetas.selected_subspecies = pObject;
      ScrollWindow.showWindow("subspecies");
    });
    MetaTypeLibrary.subspecies = this.add(pAsset11);
    MetaTypeAsset pAsset12 = new MetaTypeAsset();
    pAsset12.id = "city";
    pAsset12.ranks = MetaTypeLibrary.generateExponentialRanks(100.0, 1.5);
    pAsset12.window_name = "city";
    pAsset12.power_tab_id = "selected_city";
    pAsset12.force_zone_when_selected = true;
    pAsset12.set_icon_for_cancel_button = true;
    pAsset12.icon_list = "iconCityList";
    pAsset12.icon_single_path = "ui/icons/iconCity";
    pAsset12.window_action_clear = (MetaTypeAction) (() => SelectedMetas.selected_city = (City) null);
    pAsset12.window_history_action_update = (MetaTypeHistoryAction) ((ref WindowHistoryData pHistoryData) => pHistoryData.city = SelectedMetas.selected_city);
    pAsset12.window_history_action_restore = (MetaTypeHistoryAction) ((ref WindowHistoryData pHistoryData) => SelectedMetas.selected_city = pHistoryData.city);
    pAsset12.has_dynamic_zones = true;
    pAsset12.dynamic_zone_option = 1;
    pAsset12.reports = new string[11]
    {
      "happy",
      "unhappy",
      "food_none",
      "food_plenty",
      "food_running_out",
      "wood_none",
      "stone_none",
      "gold_none",
      "metal_none",
      "many_children",
      "many_homeless"
    };
    pAsset12.get_list = (MetaTypeListAction) (() => (IEnumerable<NanoObject>) World.world.cities);
    pAsset12.has_any = (MetaTypeListHasAction) (() => World.world.cities.hasAny());
    pAsset12.get_selected = (MetaSelectedGetter) (() => (NanoObject) SelectedMetas.selected_city);
    pAsset12.set_selected = (MetaSelectedSetter) (pElement => SelectedMetas.selected_city = pElement as City);
    pAsset12.get = (MetaGetter) (pId => (NanoObject) World.world.cities.get(pId));
    pAsset12.map_mode = MetaType.City;
    pAsset12.option_id = "map_city_layer";
    pAsset12.power_option_zone_id = "city_layer";
    pAsset12.decision_ids = new string[9]
    {
      "give_tax",
      "store_resources",
      "make_items",
      "find_house",
      "try_to_take_city_item",
      "repair_equipment",
      "city_idle_walking",
      "replenish_energy",
      "put_out_fire"
    };
    pAsset12.click_action_zone = new MetaZoneClickAction(ActionLibrary.inspectCity);
    pAsset12.selected_tab_action_meta = new MetaTypeActionAsset(this.defaultClickActionZone);
    pAsset12.check_unit_has_meta = (MetaCheckUnitWindowAction) (pActor => pActor.hasCity());
    pAsset12.set_unit_set_meta_for_meta_for_window = (MetaUnitSetMetaForWindow) (pActor => SelectedMetas.selected_city = pActor.city);
    pAsset12.draw_zones = (MetaZoneDrawAction) (pMetaTypeAsset =>
    {
      if (pMetaTypeAsset.isMetaZoneOptionSelectedFluid())
      {
        this.drawDefaultFluid(pMetaTypeAsset);
      }
      else
      {
        this.drawDefaultMeta(pMetaTypeAsset);
        this.drawForCities(pMetaTypeAsset, WildKingdomsManager.neutral.getCities(), this.getZoneDelegate(pMetaTypeAsset));
      }
    });
    pAsset12.dynamic_zones = (MetaZoneDynamicAction) (() =>
    {
      List<Actor> simpleList = World.world.units.getSimpleList();
      double curWorldTime = World.world.getCurWorldTime();
      int index = 0;
      for (int count = simpleList.Count; index < count; ++index)
      {
        Actor actor = simpleList[index];
        if (actor.asset.show_on_meta_layer)
        {
          TileZone zone = actor.current_tile.zone;
          if (actor.hasCity() && actor.isKingdomCiv())
            ZoneMetaDataVisualizer.countMetaZone(zone, (IMetaObject) actor.city, curWorldTime);
        }
      }
    });
    pAsset12.check_cursor_highlight = (MetaZoneHighlightAction) ((pMetaTypeAsset, pTile, pQAsset) =>
    {
      bool flag = PlayerConfig.optionBoolEnabled("highlight_kingdom_enemies");
      if (pMetaTypeAsset.getZoneOptionState() == 0)
      {
        if (pTile.zone.city.isRekt())
          return;
        QuantumSpriteLibrary.colorZones(pQAsset, pTile.zone.city.zones, pQAsset.color);
        if (!flag)
          return;
        QuantumSpriteLibrary.colorEnemies(pQAsset, pTile.zone.city.kingdom);
      }
      else
        this.highlightDefault(pTile, pQAsset, pQAsset.color);
    });
    pAsset12.tile_get_metaobject = (MetaZoneGetMeta) ((pZone, pZoneOption) => pZone.getCityOnZone(pZoneOption));
    pAsset12.tile_get_metaobject_0 = (MetaZoneGetMetaSimple) (pZone => (IMetaObject) pZone.city);
    pAsset12.tile_get_metaobject_1 = (MetaZoneGetMetaSimple) (pZone => ZoneMetaDataVisualizer.getZoneMetaData(pZone).meta_object);
    pAsset12.tile_get_metaobject_2 = (MetaZoneGetMetaSimple) (pZone => ZoneMetaDataVisualizer.getZoneMetaData(pZone).meta_object);
    pAsset12.check_tile_has_meta = (MetaZoneTooltipAction) ((pZone, pAsset, pZoneOption) => pAsset.tile_get_metaobject(pZone, pZoneOption) != null);
    pAsset12.check_cursor_tooltip = new MetaZoneTooltipAction(this.checkCursorTooltipDefault);
    pAsset12.cursor_tooltip_action = (MetaTooltipShowAction) (pMeta =>
    {
      City city = pMeta as City;
      if (city.isRekt())
        return;
      string str = "city";
      Tooltip.hideTooltip((object) city, true, str);
      Tooltip.show((object) city, str, new TooltipData()
      {
        city = city,
        tooltip_scale = 0.7f,
        is_sim_tooltip = true
      });
    });
    pAsset12.stat_hover = (MetaStatAction) ((pMetaId, pField) =>
    {
      City pObject = World.world.cities.get(pMetaId);
      if (pObject.isRekt())
        return;
      Tooltip.show((object) pField, "city", new TooltipData()
      {
        city = pObject
      });
    });
    pAsset12.stat_click = (MetaStatAction) ((pMetaId, _) =>
    {
      City pObject = World.world.cities.get(pMetaId);
      if (pObject.isRekt())
        return;
      SelectedMetas.selected_city = pObject;
      ScrollWindow.showWindow("city");
    });
    MetaTypeLibrary.city = this.add(pAsset12);
    MetaTypeAsset pAsset13 = new MetaTypeAsset();
    pAsset13.id = "kingdom";
    pAsset13.ranks = MetaTypeLibrary.generateExponentialRanks(100.0, 1.5);
    pAsset13.window_name = "kingdom";
    pAsset13.power_tab_id = "selected_kingdom";
    pAsset13.force_zone_when_selected = true;
    pAsset13.set_icon_for_cancel_button = true;
    pAsset13.icon_list = "iconKingdomList";
    pAsset13.icon_single_path = "ui/icons/iconKingdom";
    pAsset13.window_action_clear = (MetaTypeAction) (() => SelectedMetas.selected_kingdom = (Kingdom) null);
    pAsset13.window_history_action_update = (MetaTypeHistoryAction) ((ref WindowHistoryData pHistoryData) => pHistoryData.kingdom = SelectedMetas.selected_kingdom);
    pAsset13.window_history_action_restore = (MetaTypeHistoryAction) ((ref WindowHistoryData pHistoryData) => SelectedMetas.selected_kingdom = pHistoryData.kingdom);
    pAsset13.has_dynamic_zones = true;
    pAsset13.dynamic_zone_option = 1;
    pAsset13.reports = new string[4]
    {
      "happy",
      "unhappy",
      "many_children",
      "many_homeless"
    };
    pAsset13.get_list = (MetaTypeListAction) (() => (IEnumerable<NanoObject>) World.world.kingdoms);
    pAsset13.has_any = (MetaTypeListHasAction) (() => World.world.kingdoms.hasAny());
    pAsset13.get_selected = (MetaSelectedGetter) (() => (NanoObject) SelectedMetas.selected_kingdom);
    pAsset13.set_selected = (MetaSelectedSetter) (pElement => SelectedMetas.selected_kingdom = pElement as Kingdom);
    pAsset13.get = (MetaGetter) (pId => (NanoObject) World.world.kingdoms.get(pId));
    pAsset13.map_mode = MetaType.Kingdom;
    pAsset13.option_id = "map_kingdom_layer";
    pAsset13.power_option_zone_id = "kingdom_layer";
    pAsset13.click_action_zone = new MetaZoneClickAction(ActionLibrary.inspectKingdom);
    pAsset13.selected_tab_action_meta = new MetaTypeActionAsset(this.defaultClickActionZone);
    pAsset13.check_unit_has_meta = (MetaCheckUnitWindowAction) (pActor => pActor.isKingdomCiv());
    pAsset13.set_unit_set_meta_for_meta_for_window = (MetaUnitSetMetaForWindow) (pActor => SelectedMetas.selected_kingdom = pActor.kingdom);
    pAsset13.draw_zones = (MetaZoneDrawAction) (pMetaTypeAsset =>
    {
      if (pMetaTypeAsset.isMetaZoneOptionSelectedFluid())
      {
        this.drawDefaultFluid(pMetaTypeAsset);
      }
      else
      {
        this.drawDefaultMeta(pMetaTypeAsset);
        this.drawForCities(pMetaTypeAsset, WildKingdomsManager.neutral.getCities(), this.getZoneDelegate(pMetaTypeAsset));
      }
    });
    pAsset13.dynamic_zones = (MetaZoneDynamicAction) (() =>
    {
      List<Actor> simpleList = World.world.units.getSimpleList();
      double curWorldTime = World.world.getCurWorldTime();
      int index = 0;
      for (int count = simpleList.Count; index < count; ++index)
      {
        Actor actor = simpleList[index];
        if (actor.asset.show_on_meta_layer)
        {
          TileZone zone = actor.current_tile.zone;
          if (actor.hasKingdom() && actor.isKingdomCiv())
            ZoneMetaDataVisualizer.countMetaZone(zone, (IMetaObject) actor.kingdom, curWorldTime);
        }
      }
    });
    pAsset13.check_cursor_highlight = (MetaZoneHighlightAction) ((pMetaTypeAsset, pTile, pQAsset) =>
    {
      bool flag = PlayerConfig.optionBoolEnabled("highlight_kingdom_enemies");
      Color color = pQAsset.color;
      if (pMetaTypeAsset.getZoneOptionState() == 0)
      {
        City city11 = pTile.zone.city;
        if (city11.isRekt())
          return;
        foreach (City city12 in city11.kingdom.getCities())
          QuantumSpriteLibrary.colorZones(pQAsset, city12.zones, color);
        if (!flag)
          return;
        QuantumSpriteLibrary.colorEnemies(pQAsset, city11.kingdom);
      }
      else
        this.highlightDefault(pTile, pQAsset, color);
    });
    pAsset13.tile_get_metaobject = (MetaZoneGetMeta) ((pZone, pZoneOption) =>
    {
      IMetaObject kingdomOnZone = pZone.getKingdomOnZone(pZoneOption);
      if (kingdomOnZone == null)
        return (IMetaObject) null;
      return ((Kingdom) kingdomOnZone).isNeutral() ? (IMetaObject) null : kingdomOnZone;
    });
    pAsset13.tile_get_metaobject_0 = (MetaZoneGetMetaSimple) (pZone =>
    {
      City city = pZone.city;
      return city == null ? (IMetaObject) null : (IMetaObject) city.kingdom;
    });
    pAsset13.tile_get_metaobject_1 = (MetaZoneGetMetaSimple) (pZone => ZoneMetaDataVisualizer.getZoneMetaData(pZone).meta_object);
    pAsset13.tile_get_metaobject_2 = (MetaZoneGetMetaSimple) (pZone => ZoneMetaDataVisualizer.getZoneMetaData(pZone).meta_object);
    pAsset13.check_tile_has_meta = (MetaZoneTooltipAction) ((pZone, pAsset, pZoneOption) =>
    {
      IMetaObject metaObject = pAsset.tile_get_metaobject(pZone, pZoneOption);
      return metaObject != null && !((Kingdom) metaObject).isNeutral();
    });
    pAsset13.check_cursor_tooltip = new MetaZoneTooltipAction(this.checkCursorTooltipDefault);
    pAsset13.cursor_tooltip_action = (MetaTooltipShowAction) (pMeta =>
    {
      Kingdom kingdom = pMeta as Kingdom;
      if (kingdom.isRekt())
        return;
      string str = "kingdom";
      Tooltip.hideTooltip((object) kingdom, true, str);
      Tooltip.show((object) kingdom, str, new TooltipData()
      {
        kingdom = kingdom,
        tooltip_scale = 0.7f,
        is_sim_tooltip = true
      });
    });
    pAsset13.stat_hover = (MetaStatAction) ((pMetaId, pField) =>
    {
      Kingdom pObject = World.world.kingdoms.get(pMetaId);
      if (pObject.isRekt() || pObject.isNeutral())
        return;
      Tooltip.show((object) pField, "kingdom", new TooltipData()
      {
        kingdom = pObject
      });
    });
    pAsset13.stat_click = (MetaStatAction) ((pMetaId, _) =>
    {
      Kingdom pObject = World.world.kingdoms.get(pMetaId);
      if (pObject.isRekt() || pObject.isNeutral())
        return;
      SelectedMetas.selected_kingdom = pObject;
      ScrollWindow.showWindow("kingdom");
    });
    MetaTypeLibrary.kingdom = this.add(pAsset13);
    MetaTypeAsset pAsset14 = new MetaTypeAsset();
    pAsset14.id = "clan";
    pAsset14.ranks = MetaTypeLibrary.generateExponentialRanks(100.0, 1.5);
    pAsset14.window_name = "clan";
    pAsset14.power_tab_id = "selected_clan";
    pAsset14.force_zone_when_selected = true;
    pAsset14.set_icon_for_cancel_button = true;
    pAsset14.icon_list = "iconClanList";
    pAsset14.icon_single_path = "ui/icons/iconClan";
    pAsset14.window_action_clear = (MetaTypeAction) (() => SelectedMetas.selected_clan = (Clan) null);
    pAsset14.window_history_action_update = (MetaTypeHistoryAction) ((ref WindowHistoryData pHistoryData) => pHistoryData.clan = SelectedMetas.selected_clan);
    pAsset14.window_history_action_restore = (MetaTypeHistoryAction) ((ref WindowHistoryData pHistoryData) => SelectedMetas.selected_clan = pHistoryData.clan);
    pAsset14.reports = new string[4]
    {
      "happy",
      "unhappy",
      "many_children",
      "many_homeless"
    };
    pAsset14.get_list = (MetaTypeListAction) (() => (IEnumerable<NanoObject>) World.world.clans);
    pAsset14.has_any = (MetaTypeListHasAction) (() => World.world.clans.hasAny());
    pAsset14.get_selected = (MetaSelectedGetter) (() => (NanoObject) SelectedMetas.selected_clan);
    pAsset14.set_selected = (MetaSelectedSetter) (pElement => SelectedMetas.selected_clan = pElement as Clan);
    pAsset14.get = (MetaGetter) (pId => (NanoObject) World.world.clans.get(pId));
    pAsset14.map_mode = MetaType.Clan;
    pAsset14.option_id = "map_clan_layer";
    pAsset14.power_option_zone_id = "clan_layer";
    pAsset14.has_dynamic_zones = true;
    pAsset14.dynamic_zone_option = 2;
    pAsset14.click_action_zone = new MetaZoneClickAction(ActionLibrary.inspectClan);
    pAsset14.selected_tab_action_meta = new MetaTypeActionAsset(this.defaultClickActionZone);
    pAsset14.check_unit_has_meta = (MetaCheckUnitWindowAction) (pActor => pActor.hasClan());
    pAsset14.set_unit_set_meta_for_meta_for_window = (MetaUnitSetMetaForWindow) (pActor => SelectedMetas.selected_clan = pActor.clan);
    pAsset14.decision_ids = new string[1]{ "try_new_plot" };
    pAsset14.draw_zones = (MetaZoneDrawAction) (pMetaTypeAsset =>
    {
      if (pMetaTypeAsset.isMetaZoneOptionSelectedFluid())
        this.drawDefaultFluid(pMetaTypeAsset);
      else
        this.drawDefaultMeta(pMetaTypeAsset);
    });
    pAsset14.check_cursor_highlight = (MetaZoneHighlightAction) ((pMetaTypeAsset, pTile, pQAsset) =>
    {
      Color color = pQAsset.color;
      switch (pMetaTypeAsset.getZoneOptionState())
      {
        case 0:
          City city13 = pTile.zone.city;
          if (city13.isRekt() || city13.kingdom.getKingClan().isRekt())
            break;
          using (IEnumerator<City> enumerator = city13.kingdom.getCities().GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              City current = enumerator.Current;
              QuantumSpriteLibrary.colorZones(pQAsset, current.zones, color);
            }
            break;
          }
        case 1:
          City city14 = pTile.zone.city;
          if (city14.isRekt())
            break;
          Clan royalClan = city14.getRoyalClan();
          if (royalClan.isRekt())
            break;
          using (IEnumerator<City> enumerator = World.world.cities.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              City current = enumerator.Current;
              if (current.getRoyalClan() == royalClan)
                QuantumSpriteLibrary.colorZones(pQAsset, current.zones, color);
            }
            break;
          }
        default:
          this.highlightDefault(pTile, pQAsset, color);
          break;
      }
    });
    pAsset14.tile_get_metaobject = (MetaZoneGetMeta) ((pZone, pZoneOption) => pZone.getClanOnZone(pZoneOption));
    pAsset14.tile_get_metaobject_0 = (MetaZoneGetMetaSimple) (pZone =>
    {
      City city = pZone.city;
      return city == null ? (IMetaObject) null : (IMetaObject) city.kingdom.getKingClan();
    });
    pAsset14.tile_get_metaobject_1 = (MetaZoneGetMetaSimple) (pZone =>
    {
      City city = pZone.city;
      return city == null ? (IMetaObject) null : (IMetaObject) city.getRoyalClan();
    });
    pAsset14.tile_get_metaobject_2 = (MetaZoneGetMetaSimple) (pZone => ZoneMetaDataVisualizer.getZoneMetaData(pZone).meta_object);
    pAsset14.check_tile_has_meta = new MetaZoneTooltipAction(this.checkTileHasMetaDefault);
    pAsset14.check_cursor_tooltip = new MetaZoneTooltipAction(this.checkCursorTooltipDefault);
    pAsset14.cursor_tooltip_action = (MetaTooltipShowAction) (pMeta =>
    {
      Clan clan = pMeta as Clan;
      if (clan.isRekt())
        return;
      string str = "clan";
      Tooltip.hideTooltip((object) clan, true, str);
      Tooltip.show((object) clan, str, new TooltipData()
      {
        clan = clan,
        tooltip_scale = 0.7f,
        is_sim_tooltip = true
      });
    });
    pAsset14.dynamic_zones = (MetaZoneDynamicAction) (() =>
    {
      List<Actor> simpleList = World.world.units.getSimpleList();
      double curWorldTime = World.world.getCurWorldTime();
      int index = 0;
      for (int count = simpleList.Count; index < count; ++index)
      {
        Actor actor = simpleList[index];
        if (actor.asset.show_on_meta_layer)
        {
          TileZone zone = actor.current_tile.zone;
          if (actor.hasClan())
            ZoneMetaDataVisualizer.countMetaZone(zone, (IMetaObject) actor.clan, curWorldTime);
        }
      }
    });
    pAsset14.stat_hover = (MetaStatAction) ((pMetaId, pField) =>
    {
      Clan pObject = World.world.clans.get(pMetaId);
      if (pObject.isRekt())
        return;
      Tooltip.show((object) pField, "clan", new TooltipData()
      {
        clan = pObject
      });
    });
    pAsset14.stat_click = (MetaStatAction) ((pMetaId, _) =>
    {
      Clan pObject = World.world.clans.get(pMetaId);
      if (pObject.isRekt())
        return;
      SelectedMetas.selected_clan = pObject;
      ScrollWindow.showWindow("clan");
    });
    MetaTypeLibrary.clan = this.add(pAsset14);
    MetaTypeAsset pAsset15 = new MetaTypeAsset();
    pAsset15.id = "alliance";
    pAsset15.window_name = "alliance";
    pAsset15.power_tab_id = "selected_alliance";
    pAsset15.force_zone_when_selected = true;
    pAsset15.set_icon_for_cancel_button = true;
    pAsset15.icon_list = "iconAllianceList";
    pAsset15.icon_single_path = "ui/icons/iconAlliance";
    pAsset15.window_action_clear = (MetaTypeAction) (() => SelectedMetas.selected_alliance = (Alliance) null);
    pAsset15.window_history_action_update = (MetaTypeHistoryAction) ((ref WindowHistoryData pHistoryData) => pHistoryData.alliance = SelectedMetas.selected_alliance);
    pAsset15.window_history_action_restore = (MetaTypeHistoryAction) ((ref WindowHistoryData pHistoryData) => SelectedMetas.selected_alliance = pHistoryData.alliance);
    pAsset15.reports = new string[4]
    {
      "happy",
      "unhappy",
      "many_children",
      "many_homeless"
    };
    pAsset15.get_list = (MetaTypeListAction) (() => (IEnumerable<NanoObject>) World.world.alliances);
    pAsset15.has_any = (MetaTypeListHasAction) (() => World.world.alliances.hasAny());
    pAsset15.get_selected = (MetaSelectedGetter) (() => (NanoObject) SelectedMetas.selected_alliance);
    pAsset15.set_selected = (MetaSelectedSetter) (pElement => SelectedMetas.selected_alliance = pElement as Alliance);
    pAsset15.get = (MetaGetter) (pId => (NanoObject) World.world.alliances.get(pId));
    pAsset15.map_mode = MetaType.Alliance;
    pAsset15.option_id = "map_alliance_layer";
    pAsset15.power_option_zone_id = "alliance_layer";
    pAsset15.click_action_zone = new MetaZoneClickAction(ActionLibrary.inspectAlliance);
    pAsset15.selected_tab_action_meta = new MetaTypeActionAsset(this.defaultClickActionZone);
    pAsset15.check_unit_has_meta = (MetaCheckUnitWindowAction) (pActor => pActor.kingdom.hasAlliance());
    pAsset15.set_unit_set_meta_for_meta_for_window = (MetaUnitSetMetaForWindow) (pActor => SelectedMetas.selected_alliance = pActor.kingdom.getAlliance());
    pAsset15.draw_zones = (MetaZoneDrawAction) (pMetaTypeAsset =>
    {
      int zoneOptionState = pMetaTypeAsset.getZoneOptionState();
      foreach (Alliance alliance in (CoreSystemManager<Alliance, AllianceData>) World.world.alliances)
      {
        foreach (MetaObject<KingdomData> metaObject in alliance.kingdoms_hashset)
        {
          foreach (City city in metaObject.getCities())
          {
            foreach (TileZone zone in city.zones)
            {
              this.zone_manager.drawBegin();
              this.zone_manager.drawZoneAlliance(zone, zoneOptionState);
              this.zone_manager.drawEnd(zone);
            }
          }
        }
      }
      foreach (Kingdom kingdom in (CoreSystemManager<Kingdom, KingdomData>) World.world.kingdoms)
      {
        if (!kingdom.hasAlliance())
        {
          foreach (City city in kingdom.getCities())
          {
            foreach (TileZone zone in city.zones)
            {
              this.zone_manager.drawBegin();
              this.zone_manager.drawZoneCity(zone);
              this.zone_manager.drawEnd(zone);
            }
          }
        }
      }
    });
    pAsset15.check_cursor_highlight = (MetaZoneHighlightAction) ((pMetaTypeAsset, pTile, pQAsset) =>
    {
      bool flag = PlayerConfig.optionBoolEnabled("highlight_kingdom_enemies");
      Color color = pQAsset.color;
      City city15 = pTile.zone.city;
      if (city15.isRekt())
        return;
      Kingdom kingdom = city15.kingdom;
      if (kingdom.hasAlliance())
      {
        foreach (MetaObject<KingdomData> metaObject in kingdom.getAlliance().kingdoms_hashset)
        {
          foreach (City city16 in metaObject.getCities())
            QuantumSpriteLibrary.colorZones(pQAsset, city16.zones, color);
        }
      }
      else
      {
        foreach (City city17 in city15.kingdom.getCities())
          QuantumSpriteLibrary.colorZones(pQAsset, city17.zones, color);
      }
      if (!flag)
        return;
      QuantumSpriteLibrary.colorEnemies(pQAsset, kingdom);
    });
    pAsset15.check_tile_has_meta = (MetaZoneTooltipAction) ((pZone, pAsset, pZoneOption) =>
    {
      City city = pZone.city;
      return !city.isRekt() && !city.kingdom.getAlliance().isRekt();
    });
    pAsset15.check_cursor_tooltip = (MetaZoneTooltipAction) ((pZone, pAsset, pZoneOption) =>
    {
      City city = pZone.city;
      if (city.isRekt())
        return false;
      Alliance alliance = city.kingdom.getAlliance();
      if (alliance.isRekt())
        return MetaTypeLibrary.kingdom.check_cursor_tooltip(pZone, MetaTypeLibrary.kingdom, pZoneOption);
      alliance.meta_type_asset.cursor_tooltip_action((NanoObject) alliance);
      return true;
    });
    pAsset15.cursor_tooltip_action = (MetaTooltipShowAction) (pMeta =>
    {
      Alliance alliance = pMeta as Alliance;
      if (alliance.isRekt())
        return;
      string str = "alliance";
      Tooltip.hideTooltip((object) alliance, true, str);
      Tooltip.show((object) alliance, str, new TooltipData()
      {
        alliance = alliance,
        tooltip_scale = 0.7f,
        is_sim_tooltip = true
      });
    });
    pAsset15.stat_hover = (MetaStatAction) ((pMetaId, pField) =>
    {
      Alliance pObject = World.world.alliances.get(pMetaId);
      if (pObject.isRekt())
        return;
      Tooltip.show((object) pField, "alliance", new TooltipData()
      {
        alliance = pObject
      });
    });
    pAsset15.stat_click = (MetaStatAction) ((pMetaId, _) =>
    {
      Alliance pObject = World.world.alliances.get(pMetaId);
      if (pObject.isRekt())
        return;
      SelectedMetas.selected_alliance = pObject;
      ScrollWindow.showWindow("alliance");
    });
    MetaTypeLibrary.alliance = this.add(pAsset15);
  }

  private MetaZoneGetMetaSimple getZoneDelegate(MetaTypeAsset pMetaTypeAsset)
  {
    switch (pMetaTypeAsset.getZoneOptionState())
    {
      case 0:
        return pMetaTypeAsset.tile_get_metaobject_0;
      case 1:
        return pMetaTypeAsset.tile_get_metaobject_1;
      case 2:
        return pMetaTypeAsset.tile_get_metaobject_2;
      default:
        return pMetaTypeAsset.tile_get_metaobject_2;
    }
  }

  private void drawDefaultFluid(MetaTypeAsset pMetaTypeAsset)
  {
    foreach (ZoneMetaData pData in ZoneMetaDataVisualizer.zone_data_dict.Values)
    {
      if (pData.meta_object != null && pData.meta_object.isAlive())
      {
        this.zone_manager.drawBegin();
        this.zone_manager.drawGenericFluid(pData, pMetaTypeAsset);
        this.zone_manager.drawEnd(pData.zone);
      }
    }
  }

  private void drawDefaultMeta(MetaTypeAsset pMetaTypeAsset)
  {
    MetaZoneGetMetaSimple zoneDelegate = this.getZoneDelegate(pMetaTypeAsset);
    foreach (Kingdom kingdom in (CoreSystemManager<Kingdom, KingdomData>) World.world.kingdoms)
      this.drawForCities(pMetaTypeAsset, kingdom.getCities(), zoneDelegate);
  }

  private void drawForCities(
    MetaTypeAsset pMetaTypeAsset,
    IEnumerable<City> pListCities,
    MetaZoneGetMetaSimple pZoneGetDelegate)
  {
    foreach (City pListCity in pListCities)
      this.drawZonesForMeta(pMetaTypeAsset, pListCity.zones, pZoneGetDelegate);
  }

  private void drawZonesForMeta(
    MetaTypeAsset pMetaTypeAsset,
    List<TileZone> pZones,
    MetaZoneGetMetaSimple pZoneGetDelegate)
  {
    foreach (TileZone pZone in pZones)
    {
      this.zone_manager.drawBegin();
      this.zone_manager.drawZoneMeta(pZone, pMetaTypeAsset, pZoneGetDelegate);
      this.zone_manager.drawEnd(pZone);
    }
  }

  private void defaultClickActionZone(MetaTypeAsset pMetaTypeAsset)
  {
    PowerTabController.showTabSelectedMeta(pMetaTypeAsset);
  }

  private bool checkCursorTooltipDefault(TileZone pTile, MetaTypeAsset pAsset, int pZoneOption)
  {
    IMetaObject pType = pAsset.tile_get_metaobject(pTile, pZoneOption);
    if (pType == null)
      return false;
    pAsset.cursor_tooltip_action(pType as NanoObject);
    return true;
  }

  private bool checkTileHasMetaDefault(TileZone pZone, MetaTypeAsset pAsset, int pZoneOption)
  {
    return pAsset.tile_get_metaobject(pZone, pZoneOption) != null;
  }

  private void highlightDefault(WorldTile pTile, QuantumSpriteAsset pQAsset, Color pColorAnimated)
  {
    ZoneMetaData zoneMetaData = ZoneMetaDataVisualizer.getZoneMetaData(pTile.zone);
    if (zoneMetaData.meta_object == null || !zoneMetaData.meta_object.isAlive())
      return;
    using (ListPool<TileZone> zonesWithMeta = ZoneMetaDataVisualizer.getZonesWithMeta(zoneMetaData.meta_object))
      QuantumSpriteLibrary.colorZones(pQAsset, zonesWithMeta, pColorAnimated);
  }

  public override void linkAssets()
  {
    base.linkAssets();
    foreach (MetaTypeAsset metaTypeAsset in this.list)
    {
      if (metaTypeAsset.decision_ids != null)
      {
        metaTypeAsset.decisions_assets = new DecisionAsset[metaTypeAsset.decision_ids.Length];
        for (int index = 0; index < metaTypeAsset.decision_ids.Length; ++index)
        {
          string decisionId = metaTypeAsset.decision_ids[index];
          DecisionAsset decisionAsset = AssetManager.decisions_library.get(decisionId);
          metaTypeAsset.decisions_assets[index] = decisionAsset;
        }
      }
      if (!string.IsNullOrEmpty(metaTypeAsset.option_id))
        metaTypeAsset.option_asset = AssetManager.options_library.get(metaTypeAsset.option_id);
    }
  }

  public override void editorDiagnostic() => base.editorDiagnostic();

  public static int[] generateExponentialRanks(double pBasePoints, double pGrowthFactor)
  {
    int[] exponentialRanks = new int[10];
    double num = pBasePoints;
    for (int index = 1; index <= 10; ++index)
    {
      exponentialRanks[index - 1] = MetaTypeLibrary.roundToNiceNumber(num);
      num += pBasePoints * Math.Pow(pGrowthFactor, (double) (index - 1));
    }
    return exponentialRanks;
  }

  private static int roundToNiceNumber(double value)
  {
    return value < 1000.0 ? (int) (Math.Round(value / 100.0) * 100.0) : (int) (Math.Round(value / 500.0) * 500.0);
  }

  public MetaTypeAsset getAsset(MetaType pType) => this.get(pType.AsString());

  public MetaTypeAsset getFromPower(string pPower)
  {
    return this.getFromPower(AssetManager.powers.get(pPower));
  }

  public MetaTypeAsset getFromPower(GodPower pPower)
  {
    foreach (MetaTypeAsset fromPower in this.list)
    {
      if (fromPower.power_option_zone_id == pPower.id)
        return fromPower;
    }
    return (MetaTypeAsset) null;
  }

  private ZoneCalculator zone_manager => World.world.zone_calculator;

  public void debug(DebugTool pTool)
  {
    foreach (MetaTypeAsset metaTypeAsset in AssetManager.meta_type_library.list)
    {
      NanoObject pObject = metaTypeAsset.get_selected();
      if (!pObject.isRekt())
        pTool.setText(metaTypeAsset.id + ":", (object) pObject.getTypeID());
      else
        pTool.setText(metaTypeAsset.id + ":", (object) "-");
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: CursorTooltipHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class CursorTooltipHelper
{
  private static float _timeout = 0.0f;
  private static float _timeout_interval = 0.2f;
  public static bool is_over_meta;

  public static void update()
  {
    if (!InputHelpers.mouseSupported)
      return;
    if (World.world.isBusyWithUI())
      CursorTooltipHelper.cancel();
    else if (CursorTooltipHelper.isInputHappening())
    {
      CursorTooltipHelper.cancel();
    }
    else
    {
      bool flag = CursorTooltipHelper.updateGameplayTooltip();
      if (!flag)
        flag = CursorTooltipHelper.updateMapTooltip();
      if (flag)
        return;
      CursorTooltipHelper.cancel();
    }
  }

  private static bool updateGameplayTooltip()
  {
    if (!PlayerConfig.optionBoolEnabled("tooltip_units") || !MapBox.isRenderGameplay())
      return false;
    Actor lastActor = UnitSelectionEffect.last_actor;
    if (lastActor == null || !lastActor.isAlive())
      return false;
    if ((double) CursorTooltipHelper._timeout > 0.0)
    {
      CursorTooltipHelper._timeout -= World.world.delta_time;
      return true;
    }
    string str = "actor";
    if (!HotkeyLibrary.many_mod.isHolding() || !CursorTooltipHelper.showTooltipForSelectedMeta(lastActor))
    {
      if (lastActor.isKing())
        str = "actor_king";
      else if (lastActor.isCityLeader())
        str = "actor_leader";
      Tooltip.hideTooltip((object) lastActor, true, str);
      Tooltip.show((object) lastActor, str, new TooltipData()
      {
        actor = lastActor,
        tooltip_scale = 0.7f,
        is_sim_tooltip = true,
        sound_allowed = false
      });
    }
    return true;
  }

  private static bool showTooltipForSelectedMeta(Actor pActor)
  {
    MetaType currentMapBorderMode = Zones.getCurrentMapBorderMode();
    TooltipData pData = new TooltipData()
    {
      tooltip_scale = 0.7f,
      is_sim_tooltip = true
    };
    string str;
    object obj;
    switch (currentMapBorderMode)
    {
      case MetaType.Subspecies:
        if (!pActor.hasSubspecies())
          return false;
        str = "subspecies";
        pData.subspecies = pActor.subspecies;
        obj = (object) pActor.subspecies;
        break;
      case MetaType.Family:
        if (!pActor.hasFamily())
          return false;
        str = "family";
        pData.family = pActor.family;
        obj = (object) pActor.family;
        break;
      case MetaType.Language:
        if (!pActor.hasLanguage())
          return false;
        str = "language";
        pData.language = pActor.language;
        obj = (object) pActor.language;
        break;
      case MetaType.Culture:
        if (!pActor.hasCulture())
          return false;
        str = "culture";
        pData.culture = pActor.culture;
        obj = (object) pActor.culture;
        break;
      case MetaType.Religion:
        if (!pActor.hasReligion())
          return false;
        str = "religion";
        pData.religion = pActor.religion;
        obj = (object) pActor.religion;
        break;
      case MetaType.Clan:
        if (!pActor.hasClan())
          return false;
        str = "clan";
        pData.clan = pActor.clan;
        obj = (object) pActor.clan;
        break;
      case MetaType.City:
        if (!pActor.hasCity())
          return false;
        str = "city";
        pData.city = pActor.city;
        obj = (object) pActor.city;
        break;
      case MetaType.Kingdom:
        if (!pActor.isKingdomCiv())
          return false;
        str = "kingdom";
        pData.kingdom = pActor.kingdom;
        obj = (object) pActor.kingdom;
        break;
      case MetaType.Alliance:
        if (!pActor.kingdom.hasAlliance())
          return false;
        str = "alliance";
        pData.alliance = pActor.kingdom.getAlliance();
        obj = (object) pActor.kingdom.getAlliance();
        break;
      default:
        return false;
    }
    Tooltip.hideTooltip(obj, true, str);
    Tooltip.show(obj, str, pData);
    return true;
  }

  private static bool updateMapTooltip()
  {
    if (!PlayerConfig.optionBoolEnabled("tooltip_zones") || !MapBox.isRenderMiniMap() || !Zones.showMapBorders())
      return false;
    if ((double) CursorTooltipHelper._timeout > 0.0)
    {
      CursorTooltipHelper._timeout -= World.world.delta_time;
      return true;
    }
    bool flag = false;
    WorldTile tilePosCachedFrame = World.world.getMouseTilePosCachedFrame();
    MetaTypeAsset cachedMapMetaAsset = World.world.getCachedMapMetaAsset();
    if (tilePosCachedFrame != null && cachedMapMetaAsset != null)
      flag = cachedMapMetaAsset.check_cursor_tooltip(tilePosCachedFrame.zone, cachedMapMetaAsset, cachedMapMetaAsset.getZoneOptionState());
    return flag;
  }

  private static void cancel()
  {
    Tooltip.hideTooltip((object) null, true, string.Empty);
    CursorTooltipHelper.resetTimout();
  }

  private static bool isInputHappening()
  {
    if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2) || (double) Input.mouseScrollDelta.y != 0.0)
      return true;
    return !HotkeyLibrary.many_mod.isHolding() && Input.anyKey;
  }

  private static void resetTimout()
  {
    CursorTooltipHelper._timeout = CursorTooltipHelper._timeout_interval;
  }
}

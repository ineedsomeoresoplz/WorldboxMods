// Decompiled with JetBrains decompiler
// Type: WorldLog
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class WorldLog
{
  public static WorldLog instance;
  public const int WORLDLOG_LIMIT = 2000;

  public WorldLog() => WorldLog.instance = this;

  public static void logNewKing(Kingdom pKingdom)
  {
    new WorldLogMessage(WorldLogLibrary.king_new, pKingdom.name, pKingdom.king.getName())
    {
      kingdom = pKingdom,
      unit = pKingdom.king,
      location = pKingdom.king.current_position,
      color_special1 = pKingdom.getColor().getColorText(),
      color_special2 = pKingdom.getColor().getColorText()
    }.add();
  }

  public static void logRoyalClanNew(Kingdom pKingdom, Clan pClan)
  {
    new WorldLogMessage(WorldLogLibrary.kingdom_royal_clan_new, pKingdom.name, pClan.name, pKingdom.king.name)
    {
      kingdom = pKingdom,
      unit = pKingdom.king,
      location = pKingdom.king.current_position,
      color_special1 = pKingdom.getColor().getColorText(),
      color_special2 = pClan.getColor().getColorText(),
      color_special3 = pKingdom.getColor().getColorText()
    }.add();
  }

  public static void logRoyalClanChanged(Kingdom pKingdom, Clan pOldClan, Clan pNewClan)
  {
    new WorldLogMessage(WorldLogLibrary.kingdom_royal_clan_changed, pKingdom.name, pOldClan.name, pNewClan.name)
    {
      kingdom = pKingdom,
      unit = pKingdom.king,
      location = pKingdom.king.current_position,
      color_special1 = pKingdom.getColor().getColorText(),
      color_special2 = pOldClan.getColor().getColorText(),
      color_special3 = pNewClan.getColor().getColorText()
    }.add();
  }

  public static void logRoyalClanNoMore(Kingdom pKingdom, Clan pClan)
  {
    WorldLogMessage pMessage = new WorldLogMessage(WorldLogLibrary.kingdom_royal_clan_dead, pKingdom.name, pClan.name);
    pMessage.kingdom = pKingdom;
    if (pKingdom.hasCapital())
      pMessage.location = pKingdom.capital.last_city_center;
    pMessage.color_special1 = pKingdom.getColor().getColorText();
    pMessage.color_special2 = pClan.getColor().getColorText();
    pMessage.add();
  }

  public static void logKingFledCity(Kingdom pKingdom, Actor pActor)
  {
    new WorldLogMessage(WorldLogLibrary.king_fled_city, pKingdom.name, pActor.getName(), pActor.city.name)
    {
      kingdom = pKingdom,
      unit = pActor,
      location = pActor.current_position,
      color_special1 = pKingdom.getColor().getColorText(),
      color_special2 = pKingdom.getColor().getColorText(),
      color_special3 = pKingdom.getColor().getColorText()
    }.add();
  }

  public static void logKingFledCapital(Kingdom pKingdom, Actor pActor)
  {
    new WorldLogMessage(WorldLogLibrary.king_fled_capital, pKingdom.name, pActor.getName(), pKingdom.capital.name)
    {
      kingdom = pKingdom,
      unit = pActor,
      location = pActor.current_position,
      color_special1 = pKingdom.getColor().getColorText(),
      color_special2 = pKingdom.getColor().getColorText(),
      color_special3 = pKingdom.getColor().getColorText()
    }.add();
  }

  public static void logKingLeft(Kingdom pKingdom, Actor pActor)
  {
    new WorldLogMessage(WorldLogLibrary.king_left, pKingdom.name, pActor.getName())
    {
      kingdom = pKingdom,
      unit = pActor,
      location = pActor.current_position,
      color_special1 = pKingdom.getColor().getColorText(),
      color_special2 = pKingdom.getColor().getColorText()
    }.add();
  }

  public static void logKingDead(Kingdom pKingdom, Actor pActor)
  {
    new WorldLogMessage(WorldLogLibrary.king_dead, pKingdom.name, pActor.getName())
    {
      kingdom = pKingdom,
      location = pActor.current_position,
      color_special1 = pKingdom.getColor().getColorText(),
      color_special2 = pKingdom.getColor().getColorText()
    }.add();
  }

  public static void logKingMurder(Kingdom pKingdom, Actor pActor, Actor pAttacker)
  {
    WorldLogMessage pMessage = new WorldLogMessage(WorldLogLibrary.king_killed, pKingdom.name, pActor.getName(), pAttacker?.getName());
    pMessage.kingdom = pKingdom;
    pMessage.color_special1 = pKingdom.getColor().getColorText();
    pMessage.color_special2 = pKingdom.getColor().getColorText();
    if (pAttacker?.kingdom?.getColor() != null)
      pMessage.color_special3 = pAttacker.kingdom.getColor().getColorText();
    if (pAttacker != null && pAttacker.asset.can_be_inspected)
      pMessage.unit = pAttacker;
    pMessage.location = pActor.current_position;
    pMessage.add();
  }

  public static void logFavDead(Actor pActor)
  {
    WorldLogMessage pMessage = new WorldLogMessage(WorldLogLibrary.favorite_dead, pActor.getName());
    pMessage.location = pActor.current_position;
    if (pActor?.kingdom?.getColor() != null)
    {
      pMessage.kingdom = pActor.kingdom;
      pMessage.color_special1 = pActor.kingdom.getColor().getColorText();
    }
    pMessage.add();
  }

  public static void logFavMurder(Actor pActor, Actor pAttacker)
  {
    WorldLogMessage pMessage = new WorldLogMessage(WorldLogLibrary.favorite_killed, pActor.getName(), pAttacker?.getName());
    if (pActor?.kingdom?.getColor() != null)
    {
      pMessage.kingdom = pActor.kingdom;
      pMessage.color_special1 = pActor.kingdom.getColor().getColorText();
    }
    if (pAttacker?.kingdom?.getColor() != null)
      pMessage.color_special2 = pAttacker.kingdom.getColor().getColorText();
    if (pAttacker != null && pAttacker.asset.can_be_inspected)
      pMessage.unit = pAttacker;
    pMessage.location = pActor.current_position;
    pMessage.add();
  }

  public static void logNewCity(City pCity)
  {
    new WorldLogMessage(WorldLogLibrary.city_new, pCity.name)
    {
      kingdom = pCity.kingdom,
      location = pCity.last_city_center,
      color_special1 = pCity.kingdom.getColor().getColorText()
    }.add();
  }

  public static void logCityRevolt(City pCity)
  {
    new WorldLogMessage(WorldLogLibrary.log_city_revolted, pCity.name, pCity.kingdom.name)
    {
      kingdom = pCity.kingdom,
      location = pCity.last_city_center
    }.add();
  }

  public static void logWarEnded(War pWar)
  {
    new WorldLogMessage(WorldLogLibrary.diplomacy_war_ended, pWar.data.name)
    {
      color_special1 = pWar.getColor().getColorText()
    }.add();
  }

  public static void logNewWar(Kingdom pKingdom1, Kingdom pKingdom2)
  {
    new WorldLogMessage(WorldLogLibrary.diplomacy_war_started, pKingdom1.name, pKingdom2.name)
    {
      location = Vector2.op_Implicit(pKingdom1.location),
      color_special1 = pKingdom1.getColor().getColorText(),
      color_special2 = pKingdom2.getColor().getColorText()
    }.add();
  }

  public static void logNewTotalWar(Kingdom pKingdom)
  {
    new WorldLogMessage(WorldLogLibrary.total_war_started, pKingdom.name)
    {
      location = Vector2.op_Implicit(pKingdom.location),
      color_special1 = pKingdom.getColor().getColorText()
    }.add();
  }

  public static void logAllianceCreated(Alliance pAlliance)
  {
    new WorldLogMessage(WorldLogLibrary.alliance_new, pAlliance.name)
    {
      color_special1 = pAlliance.getColor().getColorText()
    }.add();
  }

  public static void logAllianceDisolved(Alliance pAlliance)
  {
    new WorldLogMessage(WorldLogLibrary.alliance_dissolved, pAlliance.name)
    {
      color_special1 = pAlliance.getColor().getColorText()
    }.add();
  }

  public static void logNewKingdom(Kingdom pKingdom)
  {
    new WorldLogMessage(WorldLogLibrary.kingdom_new, pKingdom.name)
    {
      kingdom = pKingdom,
      location = Vector2.op_Implicit(pKingdom.location),
      color_special1 = pKingdom.getColor().getColorText()
    }.add();
  }

  public static void logKingdomDestroyed(Kingdom pKingdom)
  {
    new WorldLogMessage(WorldLogLibrary.kingdom_destroyed, pKingdom.name)
    {
      kingdom = pKingdom,
      location = Vector2.op_Implicit(pKingdom.location),
      color_special1 = pKingdom.getColor().getColorText()
    }.add();
  }

  public static void logCityDestroyed(City pCity)
  {
    new WorldLogMessage(WorldLogLibrary.city_destroyed, pCity.name)
    {
      kingdom = pCity.kingdom,
      color_special1 = pCity.kingdom.getColor().getColorText(),
      location = pCity.last_city_center
    }.add();
  }

  public static void logDisaster(
    DisasterAsset pAsset,
    WorldTile pTile,
    string pName = null,
    City pCity = null,
    Actor pUnit = null)
  {
    if (string.IsNullOrEmpty(pAsset.world_log))
      return;
    WorldLogMessage pMessage = new WorldLogMessage(AssetManager.world_log_library.get(pAsset.world_log));
    pMessage.location = Vector2.op_Implicit(pTile.posV3);
    pMessage.special1 = pName;
    if (pCity != null)
      pMessage.special2 = pCity.name;
    if (pUnit != null && pUnit.asset.can_be_inspected)
      pMessage.unit = pUnit;
    pMessage.add();
  }

  public static void locationJump(Vector3 pVector)
  {
    HistoryHud.disableRaycasts();
    World.world.locatePosition(pVector, new Action(HistoryHud.enableRaycasts), new Action(HistoryHud.enableRaycasts));
  }

  public static void locationFollow(Actor pActor)
  {
    if (pActor == null || !pActor.isAlive())
      return;
    HistoryHud.disableRaycasts();
    World.world.locateAndFollow(pActor, new Action(HistoryHud.enableRaycasts), new Action(HistoryHud.enableRaycasts));
  }

  public static void logShatteredCrown(Kingdom pKingdom)
  {
    new WorldLogMessage(WorldLogLibrary.kingdom_shattered, pKingdom.name)
    {
      kingdom = pKingdom,
      location = Vector2.op_Implicit(pKingdom.location),
      color_special1 = pKingdom.getColor().getColorText()
    }.add();
  }

  public static void logFracturedKingdom(Kingdom pKingdom)
  {
    new WorldLogMessage(WorldLogLibrary.kingdom_fractured, pKingdom.name)
    {
      kingdom = pKingdom,
      location = Vector2.op_Implicit(pKingdom.location),
      color_special1 = pKingdom.getColor().getColorText()
    }.add();
  }
}

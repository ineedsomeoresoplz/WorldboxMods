// Decompiled with JetBrains decompiler
// Type: WorldLogMessageExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using db;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public static class WorldLogMessageExtensions
{
  public static void clear(this WorldLogMessage pMessage) => pMessage.unit = (Actor) null;

  public static void add(this WorldLogMessage pMessage)
  {
    HistoryHud.instance.newHistory(pMessage);
    DBInserter.insertLog(pMessage);
  }

  public static string getFormatedText(this WorldLogMessage pMessage, Text pTextField)
  {
    WorldLogAsset asset = pMessage.getAsset();
    string localeId;
    if (asset.random_ids > 0)
    {
      int pIndex = pMessage.timestamp % asset.random_ids + 1;
      localeId = asset.getLocaleID(pIndex);
    }
    else
      localeId = asset.getLocaleID();
    string text = LocalizedTextManager.getText(localeId);
    if (asset.text_replacer != null)
      asset.text_replacer(pMessage, ref text);
    ((Graphic) pTextField).color = asset.color;
    return text;
  }

  public static bool followLocation(this WorldLogMessage pMessage)
  {
    if (!pMessage.hasFollowLocation())
      return false;
    WorldLog.locationFollow(pMessage.unit);
    return true;
  }

  public static void jumpToLocation(this WorldLogMessage pMessage)
  {
    if (pMessage.followLocation())
      return;
    Vector3 location = pMessage.getLocation();
    if (!Vector3.op_Inequality(location, Vector3.zero) || !Toolbox.inMapBorder(ref location))
      return;
    WorldLog.locationJump(location);
  }

  public static bool hasLocation(this WorldLogMessage pMessage)
  {
    return Vector3.op_Inequality(pMessage.getLocation(), Vector3.zero);
  }

  public static bool hasFollowLocation(this WorldLogMessage pMessage)
  {
    return pMessage.unit != null && pMessage.unit.isAlive();
  }

  public static Vector3 getLocation(this WorldLogMessage pMessage)
  {
    if (pMessage.unit != null && pMessage.unit.isAlive())
      return Vector2.op_Implicit(pMessage.unit.current_position);
    int? x = pMessage.x;
    int num1 = -1;
    if (!(x.GetValueOrDefault() == num1 & x.HasValue))
    {
      int? y = pMessage.y;
      int num2 = -1;
      if (!(y.GetValueOrDefault() == num2 & y.HasValue))
      {
        Vector2 location = pMessage.location;
        if (Toolbox.inMapBorder(ref location))
          return Vector2.op_Implicit(pMessage.location);
      }
    }
    return Vector3.zero;
  }

  public static WorldLogAsset getAsset(this WorldLogMessage pMessage)
  {
    return AssetManager.world_log_library.get(pMessage.asset_id);
  }

  public static string getSpecial(this WorldLogMessage pMessage, int pSpecialId)
  {
    switch (pSpecialId)
    {
      case 1:
        return Toolbox.coloredText(pMessage.special1, pMessage.color_special_1);
      case 2:
        return Toolbox.coloredText(pMessage.special2, pMessage.color_special_2);
      case 3:
        return Toolbox.coloredText(pMessage.special3, pMessage.color_special_3);
      default:
        return "";
    }
  }
}

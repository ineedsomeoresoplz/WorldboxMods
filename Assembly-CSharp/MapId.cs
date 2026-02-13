// Decompiled with JetBrains decompiler
// Type: MapId
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class MapId : MonoBehaviour
{
  public Button continueButton;
  public InputField mapIdText;
  public Text statusText;
  public static string mapId;
  public static Map map;
  public Sprite buttonOn;
  public Sprite buttonOff;

  public static string formattedMapId
  {
    get
    {
      if (string.IsNullOrEmpty(MapId.mapId) || MapId.mapId.Length != 12)
        return MapId.mapId;
      return $"WB-{MapId.mapId.Substring(0, 4)}-{MapId.mapId.Substring(4, 4)}-{MapId.mapId.Substring(8, 4)}";
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: WorldElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class WorldElement : MonoBehaviour
{
  internal Map map;
  internal ScrollWindow windowWorldList;
  internal WorldListWindow worldListWindow;
  public Text mapName;
  public Text mapId;
  public Text mapDescription;
  public GameObject iconsGroup;
  public Image raceHumans;
  public Image raceOrcs;
  public Image raceElves;
  public Image raceDwarves;
  public Text population;
  public Text cities;
  public Text houses;
  public Text zones;
  public GameObject tag1;
  public Image tag1icon;
  public GameObject tag2;
  public Image tag2icon;
  public GameObject year;
  public GameObject uploader;
  public Text tag1Text;
  public Text tag2Text;
  public Text yearText;
  public Text uploaderText;
  public Text downloads;
  public Text plays;
  public Text favs;
  public MapPreviewImage mapPreviewImage;
  public bool clickable = true;
  public bool listPreview;
}

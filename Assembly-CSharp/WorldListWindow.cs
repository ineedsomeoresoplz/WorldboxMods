// Decompiled with JetBrains decompiler
// Type: WorldListWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class WorldListWindow : MonoBehaviour
{
  private static WorldListWindow instance;
  public WorldElement worldElementPrefab;
  public GameObject notFound;
  public ScrollWindow windowWorldList;
  private List<WorldElement> elements = new List<WorldElement>();
  public Transform transformContent;
  public Transform listContent;
  public Transform tagContent;
  public GameObject loadingSpinner;
  public GameObject textStatusBG;
  public Text textStatusMessage;
  public LocalizedText windowTitle;
  public static List<MapTagType> tagsActive = new List<MapTagType>();
  public static string authorId;
  public GameObject sectionTextBG;
  public GameObject profileImage;
  public GameObject filterButton;
  public Text sectionText;
  public Image filterTag1;
  public Image filterTag2;
}

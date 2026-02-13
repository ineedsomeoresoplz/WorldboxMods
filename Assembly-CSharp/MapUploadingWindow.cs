// Decompiled with JetBrains decompiler
// Type: MapUploadingWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class MapUploadingWindow : MonoBehaviour
{
  public Button doneButton;
  public Image loadingImage;
  public Image doneImage;
  public GameObject mapIDGroup;
  public Text mapIDText;
  public Text statusMessage;
  public Text percents;
  public Image bar;
  public Image mask;
  public static bool uploading;
}

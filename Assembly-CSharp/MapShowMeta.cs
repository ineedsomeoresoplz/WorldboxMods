// Decompiled with JetBrains decompiler
// Type: MapShowMeta
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class MapShowMeta : MonoBehaviour
{
  public WorldElement worldElementPrefab;
  public WorldElement element;
  public Transform transformContent;
  public GameObject loadingSpinner;
  public GameObject errorImage;
  public GameObject textStatusBG;
  public Text textStatusMessage;
  public GameObject playButton;
  public GameObject favButton;
  public Text playButtonText;
  public GameObject deleteButton;
  public GameObject reportButton;
  public GameObject bottomButtons;
  public Image iconFavorite;
  private bool startSpinning;
  private float angle;

  private void Update()
  {
    if (this.startSpinning)
    {
      this.angle -= Time.deltaTime * 180f;
      ((Component) this.iconFavorite).transform.localEulerAngles = new Vector3(0.0f, 0.0f, this.angle);
    }
    else
    {
      if ((double) this.angle == 0.0)
        return;
      this.angle -= Time.deltaTime * 720f;
      if ((double) this.angle < -360.0)
        this.angle = 0.0f;
      ((Component) this.iconFavorite).transform.localEulerAngles = new Vector3(0.0f, 0.0f, this.angle);
    }
  }

  public void pressFavorite()
  {
  }

  public void copyToClipboard()
  {
  }
}

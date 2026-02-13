// Decompiled with JetBrains decompiler
// Type: KarpenkosPromo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class KarpenkosPromo : MonoBehaviour
{
  public List<Sprite> sprites = new List<Sprite>();
  public Image image1;
  public Image image2;
  private float intervalChange = 1f;
  private float intervalMainImage = 1.5f;
  private int maxElements;
  private int curImageIndex;
  private float timerChange;
  private Image imageTransition;
  private Image imageCurrent;

  private void Awake() => this.maxElements = this.sprites.Count;

  private void OnEnable()
  {
    this.curImageIndex = 0;
    this.timerChange = this.intervalMainImage / 2f;
    this.setImage(this.image1, this.curImageIndex);
    ++this.curImageIndex;
    this.setImage(this.image2, this.curImageIndex);
    this.imageCurrent = this.image1;
    this.imageTransition = this.image2;
    ((Component) this.imageCurrent).GetComponent<CanvasGroup>().alpha = 1f;
    ((Component) this.imageTransition).GetComponent<CanvasGroup>().alpha = 0.0f;
  }

  private void setImage(Image pImage, int pIndex) => pImage.sprite = this.sprites[pIndex];

  private void Update()
  {
    if ((double) this.timerChange > 0.0)
    {
      this.timerChange -= Time.deltaTime;
    }
    else
    {
      if ((double) ((Component) this.imageTransition).GetComponent<CanvasGroup>().alpha >= 1.0)
        return;
      ((Component) this.imageTransition).GetComponent<CanvasGroup>().alpha += Time.deltaTime * 2f;
      if ((double) ((Component) this.imageTransition).GetComponent<CanvasGroup>().alpha < 1.0)
        return;
      ((Component) this.imageTransition).GetComponent<CanvasGroup>().alpha = 0.0f;
      this.imageCurrent.sprite = this.imageTransition.sprite;
      this.timerChange = this.intervalChange;
      if (this.curImageIndex == 0)
        this.timerChange = this.intervalMainImage;
      ++this.curImageIndex;
      if (this.curImageIndex >= this.maxElements)
        this.curImageIndex = 0;
      this.setImage(this.imageTransition, this.curImageIndex);
    }
  }
}

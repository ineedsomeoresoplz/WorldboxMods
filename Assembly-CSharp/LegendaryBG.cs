// Decompiled with JetBrains decompiler
// Type: LegendaryBG
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class LegendaryBG : MonoBehaviour
{
  public Sprite[] spriteArray;
  private Image img;
  private int max_frames = 9;
  private int currentFrame;
  private float timer = 0.07f;

  private void Awake()
  {
    this.img = ((Component) this).GetComponent<Image>();
    this.max_frames = this.spriteArray.Length;
  }

  private void OnEnable()
  {
    this.timer = 0.2f;
    this.currentFrame = this.max_frames - 1;
  }

  private void Update()
  {
    if ((double) this.timer > 0.0)
    {
      this.timer -= Time.deltaTime;
    }
    else
    {
      this.timer = 0.07f;
      ++this.currentFrame;
      if (this.currentFrame >= this.max_frames)
        this.currentFrame = 0;
      else if (this.currentFrame == this.max_frames - 1)
        this.timer = 2.4f;
      this.img.sprite = this.spriteArray[this.currentFrame];
    }
  }
}

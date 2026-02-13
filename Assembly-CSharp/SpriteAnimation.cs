// Decompiled with JetBrains decompiler
// Type: SpriteAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class SpriteAnimation : MonoBehaviour
{
  public bool ignorePause = true;
  public bool isOn = true;
  public float timeBetweenFrames = 0.1f;
  public float nextFrameTime;
  public bool useNormalDeltaTime;
  public AnimPlayType playType;
  public int currentFrameIndex;
  public bool looped = true;
  public bool returnToPool;
  public Sprite[] frames;
  public bool dirty;
  internal SpriteRenderer spriteRenderer;
  internal Image image;
  internal bool useOnSpriteRenderer = true;
  internal PhenotypeAsset phenotype;
  private bool stopFrameTrigger;
  internal Sprite currentSpriteGraphic;

  public void Awake()
  {
    this.spriteRenderer = ((Component) this).GetComponent<SpriteRenderer>();
    if (Object.op_Equality((Object) this.spriteRenderer, (Object) null))
    {
      this.image = ((Component) this).GetComponent<Image>();
      this.useOnSpriteRenderer = false;
    }
    else
      this.currentSpriteGraphic = this.spriteRenderer.sprite;
  }

  public virtual void create()
  {
    this.spriteRenderer = ((Component) this).GetComponent<SpriteRenderer>();
    this.nextFrameTime = this.timeBetweenFrames;
  }

  public void stopAnimations() => this.isOn = false;

  internal void setFrames(Sprite[] pFrames)
  {
    if (this.frames == pFrames)
      return;
    this.frames = pFrames;
    if (this.currentFrameIndex >= this.frames.Length)
      this.currentFrameIndex = 0;
    this.updateFrame();
  }

  public bool isLastFrame() => this.currentFrameIndex >= this.frames.Length - 1;

  public bool isFirstFrame() => this.currentFrameIndex == 0;

  internal virtual void update(float pElapsed)
  {
    if (this.useNormalDeltaTime)
      pElapsed = Time.deltaTime;
    if (this.dirty)
    {
      this.dirty = false;
      this.forceUpdateFrame();
    }
    else if (!this.isOn)
    {
      if (!this.stopFrameTrigger)
        return;
      this.stopFrameTrigger = false;
      this.updateFrame();
    }
    else
    {
      if (World.world.isPaused() && !this.ignorePause)
        return;
      if ((double) this.nextFrameTime > 0.0)
      {
        this.nextFrameTime -= pElapsed;
      }
      else
      {
        this.nextFrameTime = this.timeBetweenFrames;
        if (this.playType == AnimPlayType.Forward)
        {
          if (this.currentFrameIndex + 1 >= this.frames.Length)
          {
            if (this.returnToPool)
            {
              ((Component) this).GetComponent<BaseEffect>().kill();
              return;
            }
            if (!this.looped)
              return;
            this.currentFrameIndex = 0;
          }
          else
            ++this.currentFrameIndex;
        }
        else if (this.currentFrameIndex - 1 < 0)
        {
          if (this.returnToPool)
          {
            ((Component) this).GetComponent<BaseEffect>().kill();
            return;
          }
          if (!this.looped)
            return;
          this.currentFrameIndex = this.frames.Length - 1;
        }
        else
          --this.currentFrameIndex;
        this.updateFrame();
      }
    }
  }

  public void stopAt(int pFrameId = 0, bool pNow = false)
  {
    this.isOn = false;
    this.currentFrameIndex = pFrameId;
    if (pNow)
      this.updateFrame();
    else
      this.stopFrameTrigger = true;
  }

  public void forceUpdateFrame()
  {
    if (this.frames.Length == 0)
      return;
    this.currentSpriteGraphic = this.frames[this.currentFrameIndex];
    this.applyCurrentSpriteGraphics(this.currentSpriteGraphic);
  }

  public void setRandomFrame()
  {
    this.currentFrameIndex = Randy.randomInt(0, this.frames.Length);
    this.updateFrame();
  }

  public void setFrameIndex(int pFrame)
  {
    this.currentFrameIndex = pFrame;
    this.updateFrame();
  }

  public void updateFrame()
  {
    if (this.frames.Length == 0 || this.currentFrameIndex >= this.frames.Length || Object.op_Equality((Object) this.currentSpriteGraphic, (Object) this.frames[this.currentFrameIndex]))
      return;
    this.currentSpriteGraphic = this.frames[this.currentFrameIndex];
    this.applyCurrentSpriteGraphics(this.currentSpriteGraphic);
  }

  internal void applyCurrentSpriteGraphics(Sprite pSprite)
  {
    if (this.useOnSpriteRenderer)
    {
      Sprite sprite = pSprite;
      if (this.phenotype != null)
        sprite = DynamicSprites.getIconWithColors(pSprite, this.phenotype, (ColorAsset) null);
      this.spriteRenderer.sprite = sprite;
    }
    else
      this.image.sprite = pSprite;
  }

  public Sprite getCurrentGraphics() => this.currentSpriteGraphic;

  public void resetAnim(int pFrameIndex = 0)
  {
    this.nextFrameTime = this.timeBetweenFrames;
    this.currentFrameIndex = pFrameIndex;
    this.updateFrame();
  }
}

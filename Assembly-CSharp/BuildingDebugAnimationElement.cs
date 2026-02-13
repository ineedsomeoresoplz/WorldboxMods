// Decompiled with JetBrains decompiler
// Type: BuildingDebugAnimationElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class BuildingDebugAnimationElement : BaseDebugAnimationElement<BuildingAsset>
{
  public BuildingDebugAnimationVariation variation_prefab;
  public Sprite no_animation_sprite;
  public Transform variations_transform;
  private List<BuildingDebugAnimationVariation> _variations;
  private bool _has_baby;

  public override void update()
  {
    if (!this.is_playing)
      return;
    foreach (BuildingDebugAnimationVariation variation in this._variations)
      variation.update(Time.deltaTime);
    this.frame_number_text.text = this._variations[0].sprite_animation.currentFrameIndex.ToString();
  }

  public override void setData(BuildingAsset pAsset)
  {
    base.setData(pAsset);
    this._variations = new List<BuildingDebugAnimationVariation>();
    for (int index = 0; index < pAsset.building_sprites.animation_data.Count; ++index)
    {
      BuildingDebugAnimationVariation animationVariation = Object.Instantiate<BuildingDebugAnimationVariation>(this.variation_prefab, this.variations_transform);
      this.setAnimationSettings(animationVariation.sprite_animation, animationVariation.image);
      this.setAnimationSettings(animationVariation.shadow_animation, animationVariation.shadow);
      this._variations.Add(animationVariation);
    }
  }

  private void setAnimationSettings(SpriteAnimation pAnimation, Image pImage)
  {
    pAnimation.create();
    pAnimation.useOnSpriteRenderer = false;
    pAnimation.image = pImage;
    pAnimation.timeBetweenFrames = 1f / this.asset.animation_speed;
  }

  public void setFrames(List<DebugAnimatedVariation> pVariations, bool pShouldHaveSprites)
  {
    if (pVariations.Count != this._variations.Count)
      throw new ArgumentOutOfRangeException();
    bool flag = false;
    for (int index1 = 0; index1 < pVariations.Count; ++index1)
    {
      BuildingDebugAnimationVariation variation = this._variations[index1];
      if (!pShouldHaveSprites)
      {
        ((Graphic) variation.image).color = Color.clear;
        ((Graphic) variation.shadow).color = Color.clear;
      }
      else
      {
        DebugAnimatedVariation pVariation = pVariations[index1];
        Sprite[] frames = pVariation.frames;
        if (frames == null || frames.Length == 0)
        {
          variation.image.sprite = this.no_animation_sprite;
          ((Graphic) variation.shadow).color = Color.clear;
          ((Behaviour) variation).enabled = false;
          Debug.LogError((object) ("Missing sprites for Building asset " + this.asset.id));
        }
        else if (!pVariation.animated)
        {
          Sprite pSprite = frames[0];
          variation.image.sprite = pSprite;
          if (this.asset.shadow)
          {
            DynamicSpriteCreator.createBuildingShadow(this.asset, pSprite, false);
            variation.shadow.sprite = DynamicSprites.getShadowBuilding(this.asset, pSprite);
          }
          else
            ((Graphic) variation.shadow).color = Color.clear;
          ((Behaviour) variation).enabled = false;
        }
        else
        {
          variation.sprite_animation.setFrames(frames);
          Sprite[] pFrames = new Sprite[frames.Length];
          for (int index2 = 0; index2 < frames.Length; ++index2)
          {
            Sprite pSprite = frames[index2];
            if (this.asset.shadow)
            {
              DynamicSpriteCreator.createBuildingShadow(this.asset, pSprite, false);
              pFrames[index2] = DynamicSprites.getShadowBuilding(this.asset, pSprite);
            }
            else
              ((Graphic) variation.shadow).color = Color.clear;
          }
          variation.shadow_animation.setFrames(pFrames);
          flag = true;
        }
      }
    }
    if (!flag)
      return;
    this.startAnimations();
  }

  protected override void clear()
  {
    foreach (Component component in this.variations_transform)
      Object.Destroy((Object) component.gameObject);
  }

  public override void stopAnimations()
  {
    base.stopAnimations();
    foreach (BuildingDebugAnimationVariation variation in this._variations)
      variation.toggleAnimation(false);
    this.frame_number_text.text = this._variations[0].sprite_animation.currentFrameIndex.ToString();
  }

  public override void startAnimations()
  {
    base.startAnimations();
    foreach (BuildingDebugAnimationVariation variation in this._variations)
      variation.toggleAnimation(true);
  }

  protected override void clickNextFrame()
  {
    if (this.is_playing)
      return;
    SpriteAnimation spriteAnimation = this._variations[0].sprite_animation;
    int length = spriteAnimation.frames.Length;
    int pIndex = spriteAnimation.currentFrameIndex++;
    if (pIndex > length - 1)
      pIndex = 0;
    this.frame_number_text.text = pIndex.ToString();
    foreach (BuildingDebugAnimationVariation variation in this._variations)
      variation.setFrame(pIndex);
  }
}

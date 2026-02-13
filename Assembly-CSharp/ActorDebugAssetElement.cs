// Decompiled with JetBrains decompiler
// Type: ActorDebugAssetElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ActorDebugAssetElement : BaseDebugAssetElement<ActorAsset>
{
  public Image icon_left;
  public Image icon_right;
  public ActorDebugAnimationElement animation_idle;
  public ActorDebugAnimationElement animation_walk;
  public ActorDebugAnimationElement animation_swim;
  public Image egg;
  public Sprite no_animation_baby;
  private AnimationContainerUnit _animation_container_adult;
  private AnimationContainerUnit _animation_container_baby;
  private int _phenotype_index;
  private int _phenotype_shade_id;

  public override void setData(ActorAsset pAsset)
  {
    this.asset = pAsset;
    Sprite spriteIcon = this.asset.getSpriteIcon();
    this.icon_left.sprite = spriteIcon;
    this.icon_right.sprite = spriteIcon;
    this.title.text = this.asset.id;
    ((Component) this.egg).gameObject.SetActive(true);
    this.initAnimations();
    this.initStats();
  }

  protected override void initAnimations()
  {
    if (this.asset.hasDefaultEggForm())
    {
      ((Component) this.egg).gameObject.SetActive(true);
      this.egg.sprite = SpriteTextureLoader.getSprite(AssetManager.subspecies_traits.get(this.asset.getDefaultEggID()).sprite_path);
    }
    else
      ((Component) this.egg).gameObject.SetActive(false);
    this.animation_idle.setData(this.asset);
    this.animation_walk.setData(this.asset);
    this.animation_swim.setData(this.asset);
    if (this.asset.special)
    {
      Sprite sprite = this.icon_left.sprite;
      this.animation_idle.adult.image.sprite = sprite;
      this.animation_walk.adult.image.sprite = sprite;
      this.animation_swim.adult.image.sprite = sprite;
      ((Component) this.egg).gameObject.SetActive(false);
      this.stopAnimations();
    }
    else
    {
      if (this.asset.use_phenotypes)
      {
        this._phenotype_index = AssetManager.phenotype_library.get(this.asset.debug_phenotype_colors).phenotype_index;
        this._phenotype_shade_id = Actor.getRandomPhenotypeShade();
      }
      else
      {
        this._phenotype_index = 0;
        this._phenotype_shade_id = 0;
      }
      if (this.asset.is_boat)
      {
        AnimationDataBoat pBoatAnimation = ActorAnimationLoader.loadAnimationBoat(this.asset.id);
        this.setAnimation(DynamicActorSpriteCreatorUI.getBoatAnimation(pBoatAnimation), this.animation_idle, this.asset.animation_idle_speed, true, true, true);
        this.setAnimation(pBoatAnimation.normal, this.animation_walk, this.asset.animation_walk_speed, true, true, true);
        this.setAnimation(pBoatAnimation.broken, this.animation_swim, this.asset.animation_swim_speed, true, true, true);
      }
      else
      {
        string[] animationIdle = this.asset.animation_idle;
        bool pShouldHaveAnimation1 = animationIdle != null && animationIdle.Length != 0;
        string[] animationWalk = this.asset.animation_walk;
        bool pShouldHaveAnimation2 = animationWalk != null && animationWalk.Length != 0;
        string[] animationSwim = this.asset.animation_swim;
        bool pShouldHaveAnimation3 = animationSwim != null && animationSwim.Length != 0;
        this._animation_container_adult = DynamicActorSpriteCreatorUI.getContainerForUI(this.asset, true, this.asset.texture_asset);
        this.setAnimation(this._animation_container_adult.idle, this.animation_idle, this.asset.animation_idle_speed, true, this._animation_container_adult.has_idle, pShouldHaveAnimation1);
        this.setAnimation(this._animation_container_adult.walking, this.animation_walk, this.asset.animation_walk_speed, true, this._animation_container_adult.has_walking, pShouldHaveAnimation2);
        List<string> subspeciesTraits = this.asset.default_subspecies_traits;
        // ISSUE: explicit non-virtual call
        if ((subspeciesTraits != null ? (!__nonvirtual (subspeciesTraits.Contains("hovering")) ? 1 : 0) : 0) != 0 && !this.asset.flying)
        {
          this.setAnimation(this._animation_container_adult.swimming, this.animation_swim, this.asset.animation_swim_speed, true, this._animation_container_adult.has_swimming, pShouldHaveAnimation3);
        }
        else
        {
          ((Graphic) this.animation_swim.adult.image).color = Color.clear;
          ((Behaviour) this.animation_swim.adult).enabled = false;
        }
        if (!this.asset.has_baby_form)
          return;
        this._animation_container_baby = DynamicActorSpriteCreatorUI.getContainerForUI(this.asset, false, this.asset.texture_asset);
        this.setAnimation(this._animation_container_baby.idle, this.animation_idle, this.asset.animation_idle_speed, false, this._animation_container_baby.has_idle, pShouldHaveAnimation1);
        this.setAnimation(this._animation_container_baby.walking, this.animation_walk, this.asset.animation_walk_speed, false, this._animation_container_baby.has_walking, pShouldHaveAnimation2);
        if (!this.asset.default_subspecies_traits.Contains("hovering") && !this.asset.flying)
        {
          this.setAnimation(this._animation_container_baby.swimming, this.animation_swim, this.asset.animation_swim_speed, false, this._animation_container_baby.has_swimming, pShouldHaveAnimation3);
        }
        else
        {
          ((Graphic) this.animation_swim.baby.image).color = Color.clear;
          ((Behaviour) this.animation_swim.baby).enabled = false;
        }
      }
    }
  }

  public override void update()
  {
    if (!((Component) this).gameObject.activeSelf)
      return;
    this.animation_idle.update();
    this.animation_walk.update();
    this.animation_swim.update();
  }

  public override void stopAnimations()
  {
    this.animation_idle.stopAnimations();
    this.animation_walk.stopAnimations();
    this.animation_swim.stopAnimations();
  }

  public override void startAnimations()
  {
    this.animation_idle.startAnimations();
    this.animation_walk.startAnimations();
    this.animation_swim.startAnimations();
  }

  private void setAnimation(
    ActorAnimation pAnimation,
    ActorDebugAnimationElement pElement,
    float pAnimationSpeed,
    bool pIsAdult,
    bool pHasAnimation,
    bool pShouldHaveAnimation)
  {
    SpriteAnimation spriteAnimation = pIsAdult ? pElement.adult : pElement.baby;
    if (!pShouldHaveAnimation)
    {
      ((Graphic) spriteAnimation.image).color = Color.clear;
      spriteAnimation.image.sprite = (Sprite) null;
      ((Behaviour) spriteAnimation).enabled = false;
    }
    else if (!pHasAnimation)
    {
      ((Graphic) spriteAnimation.image).color = Color.white;
      spriteAnimation.image.sprite = pIsAdult ? this.no_animation : this.no_animation_baby;
      ((Behaviour) spriteAnimation).enabled = false;
    }
    else
    {
      AnimationContainerUnit pContainer = pIsAdult ? this._animation_container_adult : this._animation_container_baby;
      Sprite[] pFrames = new Sprite[pAnimation.frames.Length];
      ColorAsset debugColorAsset = AssetManager.kingdoms.get(this.asset.kingdom_id_wild).debug_color_asset;
      for (int index = 0; index < pAnimation.frames.Length; ++index)
        pFrames[index] = DynamicActorSpriteCreatorUI.getUnitSpriteForUI(this.asset, pAnimation.frames[index], pContainer, pIsAdult, AssetsDebugManager.actors_sex, this._phenotype_index, this._phenotype_shade_id, debugColorAsset, 0L, 0);
      ((Behaviour) spriteAnimation).enabled = true;
      spriteAnimation.setFrames(pFrames);
      spriteAnimation.timeBetweenFrames = 1f / pAnimationSpeed;
      pElement.startAnimations();
    }
  }

  protected override void initStats()
  {
    base.initStats();
    BaseStats statsForOverview = this.asset.getStatsForOverview();
    this.showStat("health", (object) statsForOverview["health"]);
    this.showStat("damage", (object) statsForOverview["damage"]);
    this.showStat("speed", (object) statsForOverview["speed"]);
    this.showStat("lifespan", (object) statsForOverview["lifespan"]);
  }

  protected override void showAssetWindow()
  {
    base.showAssetWindow();
    ScrollWindow.showWindow("actor_asset");
  }
}

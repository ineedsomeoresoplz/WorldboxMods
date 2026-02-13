// Decompiled with JetBrains decompiler
// Type: UnitAvatarLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UnitAvatarLoader : MonoBehaviour
{
  private const float DEFAULT_ANIMATION_SPEED = 8f;
  private const float ANIMATION_SPEED_ITEM = 5f;
  private const float FLOATING_UNITS_ANCHOR = 0.25f;
  private const float DEFAULT_AVATAR_SCALE = 2.5f;
  private const float DIED_AVATAR_SCALE = 1f;
  private static int _syntetic_index;
  public float avatarSize = 1f;
  [SerializeField]
  private Transform _frame;
  [SerializeField]
  private RectTransform _actor_and_item_container;
  [SerializeField]
  private Image _actor_image;
  [SerializeField]
  private Image _item_image;
  [SerializeField]
  private Sprite _died_sprite;
  private Actor _actor;
  private ActorAvatarData _data;
  private AnimationContainerUnit _animation_container;
  private readonly List<Sprite> _unit_sprites = new List<Sprite>();
  private readonly List<Sprite> _item_sprites = new List<Sprite>();
  private readonly List<Vector3> _item_positions = new List<Vector3>();
  private readonly List<bool> _item_show_frames = new List<bool>();
  private readonly List<AvatarEffect> _effects = new List<AvatarEffect>();
  [SerializeField]
  private AvatarEffect _effect_prefab;
  [SerializeField]
  private Transform _effects_parent_attached_below;
  [SerializeField]
  private Transform _effects_parent_attached_above;
  [SerializeField]
  private Transform _effects_parent_below;
  [SerializeField]
  private Transform _effects_parent_above;
  private ObjectPoolGenericMono<AvatarEffect> _effects_pool_attached_below;
  private ObjectPoolGenericMono<AvatarEffect> _effects_pool_attached_above;
  private ObjectPoolGenericMono<AvatarEffect> _effects_pool_below;
  private ObjectPoolGenericMono<AvatarEffect> _effects_pool_above;
  private bool _show_item;
  private bool _animated;
  private float _animation_speed = 8f;
  private int _last_frame_index;
  private int _last_frame_index_item;
  private bool _same_actor_reloaded;
  private bool _same_skin_mutation_reloaded;
  private bool _is_swimming;
  private bool _died;

  public void load(Actor pActor)
  {
    this._actor = pActor;
    this._same_actor_reloaded = this._actor == pActor;
    if (this._data == null)
      this._data = new ActorAvatarData();
    if (!pActor.isAlive())
    {
      this.load(true);
    }
    else
    {
      this._same_skin_mutation_reloaded = this._data?.mutation_skin_asset == pActor.subspecies?.mutation_skin_asset;
      this._data.setData(pActor);
      if (!this._data.asset.has_override_sprite)
        this._animation_container = DynamicActorSpriteCreatorUI.getContainerForUI(this._data.asset, this._data.is_adult, this._data.getTextureAsset(), this._data.mutation_skin_asset, this._data.is_egg, this._data.egg_asset, this._actor.getUnitTexturePath());
      this.load();
    }
  }

  public void load(ActorAvatarData pData, bool pSameActor = false)
  {
    this._died = false;
    this._same_actor_reloaded = pSameActor;
    this._same_skin_mutation_reloaded = this._data?.mutation_skin_asset == pData.mutation_skin_asset;
    this._data = pData;
    if (!this._data.asset.has_override_sprite)
      this._animation_container = DynamicActorSpriteCreatorUI.getContainerForUI(this._data.asset, this._data.is_adult, this._data.getTextureAsset(), this._data.mutation_skin_asset, this._data.is_egg, this._data.egg_asset);
    this.load();
  }

  private void load(bool pDied = false)
  {
    if (this._effects_pool_attached_below == null)
      this._effects_pool_attached_below = new ObjectPoolGenericMono<AvatarEffect>(this._effect_prefab, this._effects_parent_attached_below);
    if (this._effects_pool_attached_above == null)
      this._effects_pool_attached_above = new ObjectPoolGenericMono<AvatarEffect>(this._effect_prefab, this._effects_parent_attached_above);
    if (this._effects_pool_below == null)
      this._effects_pool_below = new ObjectPoolGenericMono<AvatarEffect>(this._effect_prefab, this._effects_parent_below);
    if (this._effects_pool_above == null)
      this._effects_pool_above = new ObjectPoolGenericMono<AvatarEffect>(this._effect_prefab, this._effects_parent_above);
    this.clear();
    this._died = pDied;
    if (this._died)
    {
      ((Component) this).transform.localScale = new Vector3(1f * this.avatarSize, 1f * this.avatarSize, 0.0f);
      this.showDied();
    }
    else
    {
      ((Component) this).transform.localScale = new Vector3(this._data.asset.inspect_avatar_scale * this.avatarSize, this._data.asset.inspect_avatar_scale * this.avatarSize, 0.0f);
      if (Object.op_Inequality((Object) this._frame, (Object) null))
        this._frame.localScale = new Vector3((float) (2.5 / ((double) this._data.asset.inspect_avatar_scale * (double) this.avatarSize)), (float) (2.5 / ((double) this._data.asset.inspect_avatar_scale * (double) this.avatarSize)), 0.0f);
      this.loadItemSprites();
      AnimationContainerUnit animationContainer = this._animation_container;
      bool flag = animationContainer != null && animationContainer.has_walking;
      this._animated = (!this._data.asset.has_override_sprite || this._data.asset.has_override_avatar_frames || this._data.asset.is_boat) && (flag || this._data.asset.is_boat || this._data.asset.has_override_avatar_frames) && !this._data.is_egg && !this._data.is_lying && !this._data.is_stop_idle_animation;
      if (!this._animated)
        this.showStatic();
      else
        this.showAnimation();
      this.checkRotationAndPivot();
      this.showStatusEffects();
    }
  }

  private void loadItemSprites()
  {
    IHandRenderer itemRenderer = this._data.item_renderer;
    this._show_item = this._data.asset.use_items && itemRenderer != null;
    if (!this._show_item)
      return;
    if (!itemRenderer.is_animated)
    {
      Sprite itemMainSpriteFrame = ItemRendering.getItemMainSpriteFrame(itemRenderer);
      if (itemMainSpriteFrame == null)
        return;
      this._item_image.sprite = this.getColoredItemSprite(itemMainSpriteFrame, itemRenderer);
    }
    else
    {
      foreach (Sprite sprite in itemRenderer.getSprites())
        this._item_sprites.Add(this.getColoredItemSprite(sprite, itemRenderer));
      this._item_image.sprite = this._item_sprites[this.getActualSpriteIndexItem()];
    }
  }

  private void clear()
  {
    this._died = false;
    this._unit_sprites.Clear();
    this._item_sprites.Clear();
    this._item_positions.Clear();
    this._item_show_frames.Clear();
    this._effects.Clear();
    this._effects_pool_above.clear();
    this._effects_pool_below.clear();
    this._effects_pool_attached_above.clear();
    this._effects_pool_attached_below.clear();
  }

  private void Update()
  {
    if (this._died)
      return;
    this.updateEffects();
    this.updateItem();
    if (!this._animated)
      return;
    int actualSpriteIndex = this.getActualSpriteIndex();
    if (this._last_frame_index == actualSpriteIndex)
      return;
    this._last_frame_index = actualSpriteIndex;
    this._actor_image.sprite = this._unit_sprites[this._last_frame_index];
    this.syncItemWithUnit();
  }

  private void updateEffects()
  {
    foreach (AvatarEffect effect in this._effects)
      effect.update(Time.deltaTime);
  }

  private void updateItem()
  {
    if (!this._show_item || !this._data.item_renderer.is_animated)
      return;
    int actualSpriteIndexItem = this.getActualSpriteIndexItem();
    if (this._last_frame_index_item == actualSpriteIndexItem)
      return;
    this._last_frame_index_item = actualSpriteIndexItem;
    this._item_image.sprite = this._item_sprites[this._last_frame_index_item];
  }

  private void checkRotationAndPivot()
  {
    this.checkRotation();
    this.checkPivot();
  }

  private float getRotation()
  {
    return !this._data.is_lying || this._data.is_touching_liquid && this._data.is_unconscious ? 0.0f : 90f;
  }

  private void checkRotation()
  {
    ((Transform) this._actor_and_item_container).rotation = Quaternion.Euler(0.0f, 0.0f, this.getRotation());
  }

  private void checkPivot()
  {
    Vector2 vector2;
    if (this._data.is_lying && (!this._data.is_touching_liquid || !this._data.is_unconscious))
    {
      // ISSUE: explicit constructor call
      ((Vector2) ref vector2).\u002Ector(0.75f, 0.25f);
    }
    else
    {
      // ISSUE: explicit constructor call
      ((Vector2) ref vector2).\u002Ector(0.5f, 0.5f);
    }
    this._actor_and_item_container.pivot = vector2;
  }

  private void syncItemWithUnit()
  {
    if (!this._show_item)
      return;
    bool itemShowFrame = this._item_show_frames[this._last_frame_index];
    ((Behaviour) this._item_image).enabled = itemShowFrame;
    if (!itemShowFrame)
      return;
    this.setImageParams(this._item_image, this._item_positions[this._last_frame_index]);
  }

  private void showDied()
  {
    this._show_item = false;
    this._animated = false;
    this._is_swimming = false;
    this._actor_image.sprite = this._died_sprite;
    this.setImageParams(this._actor_image, Vector2.op_Implicit(Vector2.zero));
    ((Behaviour) this._item_image).enabled = false;
  }

  private void showStatic()
  {
    Vector3 avatarPosition1 = this.getAvatarPosition();
    Sprite pSprite;
    Sprite sprite;
    if (this._data.asset.has_override_sprite)
    {
      pSprite = (Sprite) null;
      sprite = this._data.asset.get_override_sprite(this._actor);
    }
    else
    {
      if (this._data.is_touching_liquid && this._animation_container.has_swimming && !this._data.is_inside_boat)
      {
        this._is_swimming = true;
        pSprite = this._animation_container.swimming.frames[0];
      }
      else
      {
        this._is_swimming = false;
        pSprite = this._animation_container.walking.frames[0];
      }
      sprite = this._data.getColoredSprite(pSprite, this._animation_container);
    }
    this._actor_image.sprite = sprite;
    this.setImageParams(this._actor_image, avatarPosition1);
    if (this._show_item)
    {
      AnimationFrameData animationFrameData = this._animation_container.dict_frame_data[((Object) pSprite).name];
      if (!animationFrameData.show_item)
      {
        ((Behaviour) this._item_image).enabled = false;
        return;
      }
      ((Behaviour) this._item_image).enabled = true;
      Vector3 avatarPosition2 = this.getAvatarPosition();
      avatarPosition2.x += animationFrameData.pos_item.x;
      avatarPosition2.y += animationFrameData.pos_item.y;
      this.setImageParams(this._item_image, avatarPosition2);
    }
    else
      ((Behaviour) this._item_image).enabled = false;
    ((Object) ((Component) this).gameObject).name = "UnitAvatar_" + (this._actor != null ? this._actor.data.id.ToString() : $"syntetic_{this._data.asset.id}_{++UnitAvatarLoader._syntetic_index}");
  }

  private void showAnimation()
  {
    ((Behaviour) this._item_image).enabled = this._show_item;
    ActorAsset asset = this._data.asset;
    Vector2 vector2_1;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2_1).\u002Ector(0.5f, 0.0f);
    if (this._data.is_hovering && !this._data.is_lying && !this._data.is_immovable)
      vector2_1.y = 0.25f;
    ((Graphic) this._actor_image).rectTransform.anchorMax = vector2_1;
    ((Graphic) this._actor_image).rectTransform.anchorMin = vector2_1;
    ((Graphic) this._item_image).rectTransform.anchorMax = vector2_1;
    ((Graphic) this._item_image).rectTransform.anchorMin = vector2_1;
    Vector2 vector2_2;
    if (asset.has_override_avatar_frames)
    {
      vector2_2 = Vector2.op_Implicit(this.getAvatarPosition());
      this._unit_sprites.AddRange((IEnumerable<Sprite>) asset.get_override_avatar_frames(this._actor));
      this._animation_speed = 8f;
    }
    else
    {
      vector2_2 = Vector2.zero;
      ActorAnimation actorAnimation;
      if (asset.is_boat)
      {
        actorAnimation = DynamicActorSpriteCreatorUI.getBoatAnimation(ActorAnimationLoader.loadAnimationBoat(asset.id));
        this._animation_speed = 8f;
      }
      else if (this._data.is_touching_liquid && this._animation_container.has_swimming && !this._data.is_inside_boat)
      {
        this._is_swimming = true;
        actorAnimation = this._animation_container.swimming;
        this._animation_speed = asset.animation_swim_speed;
      }
      else
      {
        this._is_swimming = false;
        actorAnimation = this._animation_container.walking;
        this._animation_speed = asset.animation_walk_speed;
      }
      foreach (Sprite frame in actorAnimation.frames)
      {
        this._unit_sprites.Add(this._data.getColoredSprite(frame, this._animation_container));
        if (this._show_item)
        {
          AnimationFrameData animationFrameData = this._animation_container.dict_frame_data[((Object) frame).name];
          float num1 = 0.0f;
          float num2 = 0.0f;
          if (animationFrameData != null)
          {
            if (!animationFrameData.show_item)
            {
              this._item_show_frames.Add(false);
              this._item_positions.Add(Vector3.zero);
              continue;
            }
            num1 = animationFrameData.pos_item.x;
            num2 = animationFrameData.pos_item.y;
          }
          float num3 = asset.inspect_avatar_offset_x + num1;
          float num4 = asset.inspect_avatar_offset_y + num2;
          Vector3 vector3;
          // ISSUE: explicit constructor call
          ((Vector3) ref vector3).\u002Ector(num3, num4, -0.01f);
          this._item_positions.Add(vector3);
          this._item_show_frames.Add(true);
        }
      }
    }
    if (!this._same_actor_reloaded || !this._same_skin_mutation_reloaded || this._last_frame_index >= this._unit_sprites.Count)
      this._last_frame_index = 0;
    this._actor_image.sprite = this._unit_sprites[this._last_frame_index];
    this.setImageParams(this._actor_image, Vector2.op_Implicit(vector2_2));
    if (!this._show_item)
      return;
    this.setImageParams(this._item_image, this._item_positions[this._last_frame_index]);
  }

  private void showStatusEffects()
  {
    if (this._data.statuses == null)
      return;
    foreach (string statuse in this._data.statuses)
    {
      if (this._data.statuses_gameplay == null || !this._data.statuses_gameplay[statuse].is_finished)
      {
        StatusAsset pAsset = AssetManager.status.get(statuse);
        if (pAsset.need_visual_render && pAsset.render_check(this._data.asset) && (pAsset.has_override_sprite || pAsset.texture != null))
        {
          AvatarEffect next = this.getEffectsPool(pAsset).getNext();
          this._effects.Add(next);
          Image image = next.image;
          RectTransform rectTransform = ((Graphic) image).rectTransform;
          Vector2 vector2;
          // ISSUE: explicit constructor call
          ((Vector2) ref vector2).\u002Ector(0.5f, 0.0f);
          if (this._data.is_hovering && !this._data.is_lying && !this._data.is_immovable)
            vector2.y = 0.25f;
          rectTransform.anchorMax = vector2;
          rectTransform.anchorMin = vector2;
          next.load(pAsset, this._actor, this);
          Rect rect = image.sprite.rect;
          this.setImageParams(image, new Vector3()
          {
            x = pAsset.offset_x_ui * (((Rect) ref rect).width * pAsset.scale),
            y = pAsset.offset_y_ui * (((Rect) ref rect).height * pAsset.scale)
          }, pAsset.scale);
          next.setInitialPosition(Vector2.op_Implicit(((Component) image).transform.localPosition));
        }
      }
    }
  }

  private void setImageParams(Image pImage, Vector3 pPosition, float pScale = 1f)
  {
    RectTransform rectTransform = ((Graphic) pImage).rectTransform;
    Rect rect1 = pImage.sprite.rect;
    double num1 = (double) ((Rect) ref rect1).width * (double) pScale;
    rect1 = pImage.sprite.rect;
    double num2 = (double) ((Rect) ref rect1).height * (double) pScale;
    Vector2 vector2 = new Vector2((float) num1, (float) num2);
    rectTransform.sizeDelta = vector2;
    double x = (double) pImage.sprite.pivot.x;
    Rect rect2 = pImage.sprite.rect;
    double width = (double) ((Rect) ref rect2).width;
    float num3 = (float) (x / width);
    double y = (double) pImage.sprite.pivot.y;
    Rect rect3 = pImage.sprite.rect;
    double height = (double) ((Rect) ref rect3).height;
    float num4 = (float) (y / height);
    ((Graphic) pImage).rectTransform.pivot = new Vector2(num3, num4);
    ((Graphic) pImage).rectTransform.anchoredPosition = Vector2.op_Implicit(pPosition);
  }

  private Sprite getColoredItemSprite(Sprite pSprite, IHandRenderer pIHandRenderer)
  {
    ColorAsset colorAsset = this._data.kingdom_color;
    if (!pIHandRenderer.is_colored)
      colorAsset = (ColorAsset) null;
    if (pIHandRenderer.is_colored && colorAsset == null)
      throw new InvalidOperationException("ItemRenderer is colored but no color asset found");
    return DynamicSprites.getCachedAtlasItemSprite(DynamicSprites.getItemSpriteID(pSprite, colorAsset), pSprite, colorAsset);
  }

  public int getActualSpriteIndex()
  {
    int actualSpriteIndex = 0;
    if (this._animated)
      actualSpriteIndex = AnimationHelper.getSpriteIndex((Time.time + (float) this.getAnimationHashCode()) * this._animation_speed, 0, this._unit_sprites.Count);
    return actualSpriteIndex;
  }

  private int getActualSpriteIndexItem()
  {
    return AnimationHelper.getSpriteIndex((float) (((double) Time.time + (double) this.getAnimationHashCode()) * 5.0), 0, this._item_sprites.Count);
  }

  private int getAnimationHashCode() => this._data.actor_hash;

  private Vector3 getAvatarPosition()
  {
    return new Vector3(this._data.asset.inspect_avatar_offset_x, this._data.asset.inspect_avatar_offset_y);
  }

  private ObjectPoolGenericMono<AvatarEffect> getEffectsPool(StatusAsset pAsset)
  {
    return pAsset.use_parent_rotation ? ((double) pAsset.position_z >= 0.0 ? this._effects_pool_attached_above : this._effects_pool_attached_below) : ((double) pAsset.position_z >= 0.0 ? this._effects_pool_above : this._effects_pool_below);
  }

  public bool actorStateChanged()
  {
    return !this._died && ((this._is_swimming || !this._actor.isTouchingLiquid() ? (!this._is_swimming ? 0 : (!this._actor.isTouchingLiquid() ? 1 : 0)) : 1) | (this._data.item_renderer != this._actor.getHandRendererAsset() ? 1 : 0)) != 0;
  }

  public ActorAvatarData getData() => this._data;

  public AnimationContainerUnit getAnimationContainer() => this._animation_container;
}

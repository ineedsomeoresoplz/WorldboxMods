// Decompiled with JetBrains decompiler
// Type: Cloud
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Cloud : BaseEffect
{
  public CloudAsset asset;
  private float speed = 1f;
  public SpriteShadow spriteShadow;
  private float _timer_action_1;
  private float _timer_action_2;
  internal float alive_time;
  private float _fade_multiplier = 0.2f;
  internal float effect_texture_width;
  internal float effect_texture_height;
  private float _lifespan;

  internal override void create() => base.create();

  internal override void prepare()
  {
    this.sprite_renderer.sprite = Randy.getRandom<Sprite>(this.asset.cached_sprites);
    this.sprite_renderer.flipX = Randy.randomBool();
    this.speed = Randy.randomFloat(this.asset.speed_min, this.asset.speed_max);
    Rect rect1 = this.sprite_renderer.sprite.rect;
    this.effect_texture_width = ((Rect) ref rect1).width * 0.08f;
    Rect rect2 = this.sprite_renderer.sprite.rect;
    this.effect_texture_height = ((Rect) ref rect2).height * 0.04f;
    this._timer_action_1 = this.asset.interval_action_1;
    this._lifespan = 0.0f;
    this.alive_time = 0.0f;
    base.prepare();
    this.setAlpha(0.0f);
  }

  public void setLifespan(float pLifespan) => this._lifespan = pLifespan;

  internal void setType(CloudAsset pAsset)
  {
    this.asset = pAsset;
    this.sprite_renderer.color = this.asset.color;
  }

  internal void setType(string pType)
  {
    CloudAsset pAsset = AssetManager.clouds.get(pType);
    if (pAsset == null)
      return;
    this.setType(pAsset);
  }

  public void spawn(WorldTile pTile, string pType)
  {
    if (pTile == null)
    {
      this.setType(pType);
      this.prepare();
    }
    else
      this.prepare(pTile.posV3, pType);
  }

  internal void prepare(Vector3 pVec, string pType)
  {
    this.setType(pType);
    this.prepare();
    pVec.y -= this.spriteShadow.offset.y;
    ((Component) this).transform.localPosition = pVec;
  }

  internal override void prepare(WorldTile pTile, float pScale = 0.5f)
  {
    this.prepare();
    ((Component) this).transform.localPosition = new Vector3(pTile.posV3.x, pTile.posV3.y);
  }

  public override void update(float pElapsed)
  {
    this.alive_time += pElapsed;
    this._fade_multiplier = !Config.time_scale_asset.sonic ? 0.2f : 0.05f;
    if (this.asset.draw_light_area)
    {
      Vector2 vector2 = Vector2.op_Implicit(((Component) this).transform.localPosition);
      vector2.x += this.asset.draw_light_area_offset_x;
      vector2.y += this.asset.draw_light_area_offset_y;
      World.world.stack_effects.light_blobs.Add(new LightBlobData()
      {
        position = vector2,
        radius = this.asset.draw_light_size
      });
    }
    if (!World.world.isPaused())
    {
      ((Component) this).transform.Translate(this.speed * pElapsed, 0.0f, 0.0f);
      if (this.asset.cloud_action_1 != null)
      {
        if ((double) this._timer_action_1 > 0.0)
        {
          this._timer_action_1 -= pElapsed;
        }
        else
        {
          this._timer_action_1 = this.asset.interval_action_1;
          this.asset.cloud_action_1(this);
        }
      }
      if (this.asset.cloud_action_2 != null)
      {
        if ((double) this._timer_action_2 > 0.0)
        {
          this._timer_action_2 -= pElapsed;
        }
        else
        {
          this._timer_action_2 = this.asset.interval_action_2;
          this.asset.cloud_action_2(this);
        }
      }
    }
    if ((double) ((Component) this).transform.localPosition.x > (double) MapBox.width || (double) this._lifespan > 0.0 && (double) this.alive_time > (double) this._lifespan)
      this.startToDie();
    float maxAlpha = this.asset.max_alpha;
    if (Object.op_Inequality((Object) World.world.camera, (Object) null) && (double) World.world.camera.orthographicSize > 0.0)
    {
      maxAlpha *= World.world.camera.orthographicSize / 100f;
      if ((double) maxAlpha > (double) this.asset.max_alpha)
        maxAlpha = this.asset.max_alpha;
    }
    switch (this.state)
    {
      case 1:
        if ((double) this.alpha < (double) maxAlpha)
        {
          this.alpha += pElapsed * this._fade_multiplier;
          if ((double) this.alpha >= (double) maxAlpha)
            this.alpha = maxAlpha;
        }
        else if ((double) this.alpha > (double) maxAlpha)
        {
          this.alpha -= pElapsed * this._fade_multiplier;
          if ((double) this.alpha <= (double) maxAlpha)
            this.alpha = maxAlpha;
        }
        else
          this.alpha = maxAlpha;
        this.setAlpha(this.alpha);
        break;
      case 2:
        if ((double) this.alpha > 0.0)
        {
          this.alpha -= pElapsed * this._fade_multiplier;
          this.setAlpha(this.alpha);
          break;
        }
        this.alpha = 0.0f;
        this.controller.killObject((BaseEffect) this);
        break;
    }
  }
}

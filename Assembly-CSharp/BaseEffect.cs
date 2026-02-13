// Decompiled with JetBrains decompiler
// Type: BaseEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using FMOD.Studio;
using UnityEngine;

#nullable disable
public class BaseEffect : BaseAnimatedObject
{
  private const int MAP_MARGIN_TOP = 25;
  private const int MAP_OFFSET_BOTTOM_MIN = -50;
  private const int MAP_OFFSET_BOTTOM_MAX = 30;
  internal bool active;
  internal int effectIndex;
  public const int STATE_START = 1;
  public const int STATE_ON_DEATH = 2;
  public const int STATE_KILLED = 3;
  protected float scale;
  protected float alpha;
  public WorldTile tile;
  internal BaseEffectController controller;
  internal int state;
  private double _timestamp_spawned;
  internal SpriteRenderer sprite_renderer;
  internal BaseCallback callback;
  internal int callbackOnFrame = -1;
  internal EventInstance fmod_instance;
  internal Actor attachedToActor;

  public double timestamp_spawned => this._timestamp_spawned;

  public override void Awake()
  {
    this.sprite_renderer = ((Component) this).GetComponent<SpriteRenderer>();
    base.Awake();
  }

  public void activate()
  {
    this.active = true;
    ((Component) this).gameObject.SetActive(true);
    this.state = 1;
    this._timestamp_spawned = (double) Time.time;
    this.clear();
  }

  internal void attachTo(Actor pActor)
  {
    this.attachedToActor = pActor;
    this.updateAttached();
  }

  internal void makeParentController()
  {
    ((Component) this).transform.SetParent(((Component) this.controller).transform, true);
  }

  internal virtual void prepare(WorldTile pTile, float pScale = 0.5f)
  {
    this.state = 1;
    ((Component) this).transform.localEulerAngles = Vector3.zero;
    Vector2Int pos = pTile.pos;
    double num = (double) ((Vector2Int) ref pos).x + 0.5;
    pos = pTile.pos;
    double y = (double) ((Vector2Int) ref pos).y;
    this.current_position = Vector2.op_Implicit(new Vector3((float) num, (float) y));
    ((Component) this).transform.localPosition = Vector2.op_Implicit(this.current_position);
    this.setScale(pScale);
    this.setAlpha(1f);
    this.resetAnim();
  }

  public void setScale(float pScale)
  {
    this.scale = pScale;
    if ((double) this.scale < 0.0)
      this.scale = 0.0f;
    ((Component) this).transform.localScale = new Vector3(pScale, pScale);
  }

  internal virtual void prepare(Vector2 pVector, float pScale = 1f)
  {
    this.state = 1;
    ((Component) this).transform.rotation = Quaternion.identity;
    ((Component) this).transform.localPosition = Vector2.op_Implicit(pVector);
    this.setScale(pScale);
    this.setAlpha(1f);
    this.resetAnim();
  }

  protected void setAlpha(float pVal)
  {
    this.alpha = pVal;
    Color color = this.sprite_renderer.color;
    color.a = this.alpha;
    this.sprite_renderer.color = color;
  }

  internal virtual void prepare()
  {
    ((Component) this).transform.position = new Vector3((float) Randy.randomInt(-50, 30), (float) Randy.randomInt(0, MapBox.height + 25));
    this.state = 1;
    this.setAlpha(0.0f);
  }

  internal virtual void spawnOnTile(WorldTile pTile)
  {
    this.tile = pTile;
    ((Component) this).transform.localPosition = new Vector3(pTile.posV3.x, pTile.posV3.y);
  }

  internal void startToDie() => this.state = 2;

  public virtual void kill()
  {
    this.state = 3;
    this.controller.killObject(this);
  }

  public void deactivate()
  {
    if (((EventInstance) ref this.fmod_instance).isValid())
    {
      ((EventInstance) ref this.fmod_instance).stop((STOP_MODE) 0);
      ((EventInstance) ref this.fmod_instance).release();
    }
    this.active = false;
    ((Component) this).transform.SetParent(((Component) this).transform);
    ((Component) this).gameObject.SetActive(false);
    this.clear();
  }

  public void clear()
  {
    this.tile = (WorldTile) null;
    this.attachedToActor = (Actor) null;
    this.callback = (BaseCallback) null;
    this.callbackOnFrame = -1;
  }

  public override void update(float pElapsed)
  {
    if (this.controller.asset.draw_light_area)
    {
      Vector2 vector2 = Vector2.op_Implicit(((Component) this).transform.position);
      vector2.x += this.controller.asset.draw_light_area_offset_x;
      vector2.y += this.controller.asset.draw_light_area_offset_y;
      World.world.stack_effects.light_blobs.Add(new LightBlobData()
      {
        position = vector2,
        radius = this.controller.asset.draw_light_size
      });
    }
    if (World.world.isPaused() && DebugConfig.isOn(DebugOption.PauseEffects))
      return;
    if (this.attachedToActor != null)
      this.updateAttached();
    base.update(pElapsed);
    if (this.callbackOnFrame == -1 || this.sprite_animation.currentFrameIndex != this.callbackOnFrame)
      return;
    this.callback();
    this.clear();
  }

  private void updateAttached()
  {
    if (!this.attachedToActor.isAlive())
    {
      this.kill();
    }
    else
    {
      this.sprite_renderer.flipX = this.attachedToActor.a.flip;
      ((Component) this).transform.localScale = this.attachedToActor.current_scale;
      ((Component) this).transform.localPosition = this.attachedToActor.cur_transform_position;
      ((Component) this).transform.eulerAngles = this.attachedToActor.current_rotation;
    }
  }

  public void setCallback(int pFrame, BaseCallback pCallback)
  {
    this.callbackOnFrame = pFrame;
    this.callback = pCallback;
  }

  public bool isKilled() => this.state == 3;
}

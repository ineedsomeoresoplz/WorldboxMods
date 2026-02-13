// Decompiled with JetBrains decompiler
// Type: Status
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Runtime.CompilerServices;

#nullable disable
public class Status : CoreSystemObject<StatusData>
{
  private float _action_timer;
  private StatusAsset _asset;
  private bool _finished;
  private BaseSimObject _sim_object;
  private int _anim_frame;
  private float _anim_time_between_frames;
  private float _anim_timer;
  public bool flip_x;
  private double _end_time;
  public float duration;
  private bool _has_action;
  private bool _is_animated;

  public override BaseSystemManager manager => (BaseSystemManager) World.world.statuses;

  public int get_sprites_count => this._asset.get_sprites_count(this._sim_object, this._asset);

  public void setDuration(float pDuration)
  {
    this._end_time = World.world.getCurWorldTime() + (double) pDuration;
    this.duration = pDuration;
    this._finished = false;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public double getRemainingTime() => this._end_time - World.world.getCurWorldTime();

  protected override void setDefaultValues()
  {
    this._finished = false;
    this._action_timer = 0.0f;
    this._anim_frame = 0;
    this._anim_time_between_frames = 0.0f;
    this._anim_timer = 0.0f;
    this.flip_x = false;
    this._end_time = 0.0;
    this.duration = 0.0f;
    this._sim_object = (BaseSimObject) null;
    this._has_action = false;
    this._is_animated = false;
  }

  public void start(BaseSimObject pSimObject, StatusAsset pAsset)
  {
    this._sim_object = pSimObject;
    this._asset = pAsset;
    this._action_timer = this._asset.action_interval;
    this.setDuration(this._asset.duration);
    if (this._asset.random_frame)
      this._anim_frame = Randy.randomInt(0, this.get_sprites_count);
    if (this._asset.random_flip)
      this.flip_x = Randy.randomBool();
    if (this._asset.sprite_list != null)
    {
      this._anim_time_between_frames = this._asset.animation_speed + Randy.randomFloat(0.0f, this._asset.animation_speed_random);
      this._anim_timer = this._anim_time_between_frames;
    }
    this._has_action = this._asset.action != null;
    this._is_animated = this._asset.animated && this._asset.texture != null;
  }

  internal void updateAnimationFrame(float pElapsed)
  {
    if (!this._is_animated)
      return;
    this._anim_timer -= pElapsed;
    if ((double) this._anim_timer > 0.0)
      return;
    int getSpritesCount = this.get_sprites_count;
    this._anim_timer = this._anim_time_between_frames;
    ++this._anim_frame;
    if (this._anim_frame >= getSpritesCount && this._asset.loop)
    {
      this._anim_frame = 0;
    }
    else
    {
      if (this._anim_frame < getSpritesCount)
        return;
      this._anim_frame = getSpritesCount - 1;
    }
  }

  private void updateActionTimer(float pElapsed)
  {
    float actionInterval = this._asset.action_interval;
    if ((double) this._action_timer > 0.0)
    {
      this._action_timer -= pElapsed;
    }
    else
    {
      this._action_timer = actionInterval;
      if (!this._sim_object.isAlive())
        return;
      WorldAction action = this._asset.action;
      if (action == null)
        return;
      int num = action(this._sim_object, this._sim_object.current_tile) ? 1 : 0;
    }
  }

  public void update(float pElapsed, float pWorldTime)
  {
    if (this._has_action)
      this.updateActionTimer(pElapsed);
    if ((double) pWorldTime < this._end_time)
      return;
    this.finish();
    if (!this._sim_object.isAlive())
      return;
    WorldAction actionFinish = this._asset.action_finish;
    if (actionFinish == null)
      return;
    int num = actionFinish(this._sim_object, this._sim_object.current_tile) ? 1 : 0;
  }

  public void finish() => this._finished = true;

  public BaseSimObject sim_object => this._sim_object;

  public bool is_finished => this._finished;

  public StatusAsset asset => this._asset;

  public int anim_frame => this._anim_frame;
}

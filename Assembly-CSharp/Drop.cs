// Decompiled with JetBrains decompiler
// Type: Drop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Drop : BaseMapObject
{
  private readonly bool DEBUG_COLOR;
  public int drop_index;
  internal bool active;
  private SpriteRenderer _sprite_renderer;
  private SpriteAnimation _sprite_animation;
  private float _currentHeightZ;
  private bool _landed;
  private DropAsset _asset;
  internal bool soundOn;
  private bool _parabolic;
  private float _falling_speed;
  private float _scale = 1f;
  private bool _force_surprise;
  private long _caster_id = -1;
  private Vector2 _targetPosition;
  private Vector2 _startPosition;
  private float _targetHeight;
  private float _timeToTarget;
  private float _timeInAir;
  private Color _gizmoColor = Color.op_Implicit(Vector4.zero);
  private Color _gizmoColor2 = Color.op_Implicit(Vector4.zero);
  private float _rotation_speed;

  private void Awake()
  {
    this._sprite_renderer = ((Component) this).gameObject.GetComponent<SpriteRenderer>();
    this._sprite_animation = ((Component) this).gameObject.GetComponent<SpriteAnimation>();
  }

  public void setForceSurprise() => this._force_surprise = true;

  internal void prepare()
  {
    if (!this.created)
      this.create();
    ((Component) this).gameObject.SetActive(true);
    this.m_transform.localScale = Vector3.one;
    this.active = true;
    this._force_surprise = false;
    this._timeInAir = 0.0f;
    this._timeToTarget = 0.0f;
    this._landed = false;
    this._parabolic = false;
    this.soundOn = false;
    this._currentHeightZ = 0.0f;
    this._caster_id = -1L;
    ((Component) this).transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));
    if (!this.DEBUG_COLOR)
      return;
    this._sprite_renderer.color = Randy.getRandomColor();
  }

  internal void launchStraight(WorldTile pTile, DropAsset pAsset, float zDropHeight = -1f)
  {
    this._asset = pAsset;
    if (this._asset.animation_rotation)
    {
      this._rotation_speed = Randy.randomFloat(this._asset.animation_rotation_speed_min, this._asset.animation_rotation_speed_max);
      if (Randy.randomBool())
        this._rotation_speed *= -1f;
    }
    if (!string.IsNullOrEmpty(this._asset.sound_launch))
      MusicBox.playSound(this._asset.sound_launch, pTile);
    if (this._asset.action_launch != null)
      this._asset.action_launch();
    this._falling_speed = this._asset.falling_speed + Randy.randomFloat(0.0f, this._asset.falling_speed_random);
    if (this._asset.cached_sprites == null || this._asset.cached_sprites.Length == 0)
      this._asset.cached_sprites = SpriteTextureLoader.getSpriteList(this._asset.path_texture);
    ((Renderer) this._sprite_renderer).sharedMaterial = LibraryMaterials.instance.dict[this._asset.material];
    this._sprite_animation.setFrames(this._asset.cached_sprites);
    if (this._asset.random_flip)
      this._sprite_renderer.flipX = Randy.randomBool();
    if (this._asset.animated)
    {
      this._sprite_animation.isOn = true;
      this._sprite_animation.timeBetweenFrames = this._asset.animation_speed + Randy.randomFloat(0.0f, this._asset.animation_speed_random);
    }
    else
      this._sprite_animation.isOn = false;
    if (this._asset.random_frame)
      this._sprite_animation.setRandomFrame();
    this._sprite_animation.forceUpdateFrame();
    this.current_tile = pTile;
    this._currentHeightZ = (double) zDropHeight == -1.0 ? (float) (int) Randy.randomFloat(pAsset.falling_height.x, pAsset.falling_height.y) : zDropHeight;
    this.current_position = new Vector2(pTile.posV3.x, pTile.posV3.y);
    this._startPosition = this.current_position;
    this.updatePosition();
  }

  public void launchParabolic(
    float pStartHeight,
    float pMinHeight,
    float pMaxHeight,
    float pMinRadius,
    float pMaxRadius)
  {
    this._targetPosition = Vector2.op_Addition(this._startPosition, Randy.randomPointOnCircle(pMinRadius, pMaxRadius));
    this._targetHeight = Randy.randomFloat(pMinHeight, pMaxHeight);
    this._startPosition.y += pStartHeight;
    this._currentHeightZ = this._startPosition.y;
    this._timeInAir = 0.0f;
    if ((double) this._scale < 1.0)
      this._falling_speed /= this._scale * 2f;
    this._timeToTarget = (float) (((double) Toolbox.DistVec2Float(this._startPosition, this._targetPosition) + (double) this._targetHeight * 3.0) * 0.25) / this._falling_speed;
    if ((double) this._timeToTarget < 1.0)
      this._timeToTarget += 0.5f;
    this._parabolic = true;
    this.updatePosition();
  }

  private void updateStraightFall(float pElapsed)
  {
    float num = 15f * pElapsed;
    float pChangeX = (double) this._scale >= 1.0 ? num * this._falling_speed : num * (this._falling_speed / (this._scale * 2f));
    if ((double) this._currentHeightZ < 0.0)
      pChangeX = 0.0f;
    this._currentHeightZ -= pChangeX * this._scale;
    this.applyRandomXMove(pChangeX);
    if ((double) this._currentHeightZ <= 0.0)
    {
      this._currentHeightZ = 0.0f;
      this.updatePosition();
      this.current_tile = World.world.GetTile((int) this.current_position.x, (int) this.current_position.y);
      this.land();
    }
    else
      this.updatePosition();
  }

  private void applyRandomXMove(float pChangeX)
  {
    if (!this._asset.falling_random_x_move || (double) pChangeX <= 0.0 || !Randy.randomBool())
      return;
    if (Randy.randomBool())
      this.current_position.x -= 1f * this._scale;
    else
      this.current_position.x += 1f * this._scale;
  }

  private void land()
  {
    if (this.current_tile != null)
    {
      if (this._asset.action_landed != null)
        this._asset.action_landed(this.current_tile, this._asset.id);
      if (this._asset.action_landed_drop != null)
        this._asset.action_landed_drop(this, this.current_tile, this._asset.id);
      if (this.current_tile.zone.visible && this._asset.sound_drop != string.Empty)
        MusicBox.playSound(this._asset.sound_drop, this.current_tile);
      if (this._force_surprise || this._asset.surprises_units)
        ActionLibrary.suprisedByArchitector((BaseSimObject) null, this.current_tile);
    }
    World.world.drop_manager.landDrop(this);
    this._landed = true;
  }

  public override void update(float pElapsed)
  {
    if (this._landed)
      return;
    this._sprite_animation.update(pElapsed);
    if (this._parabolic)
      this.updateParabolicFall(pElapsed);
    else
      this.updateStraightFall(pElapsed);
    if (this._landed || !this._asset.animation_rotation)
      return;
    this.updateRotation(pElapsed);
  }

  private void updateRotation(float pElapsed)
  {
    Quaternion rotation = ((Component) this).transform.rotation;
    ((Component) this).transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, ((Quaternion) ref rotation).eulerAngles.z + this._rotation_speed * pElapsed));
  }

  private void updateParabolicFall(float pElapsed)
  {
    if ((double) this._timeInAir > (double) this._timeToTarget)
      return;
    this._timeInAir += pElapsed;
    if ((double) this._timeInAir > (double) this._timeToTarget)
      this._timeInAir = this._timeToTarget;
    float pTime = this._timeInAir / this._timeToTarget;
    Vector2 vector2_1 = Toolbox.ParabolaDrag(this._startPosition, this._targetPosition, this._targetHeight, pTime);
    Vector2 vector2_2 = Vector2.Lerp(this._startPosition, this._targetPosition, pTime);
    this._currentHeightZ = vector2_1.y - vector2_2.y;
    ((Vector2) ref this.current_position).Set(vector2_1.x, vector2_1.y - this._currentHeightZ);
    if (Vector2.op_Equality(this.current_position, this._targetPosition))
    {
      this.current_tile = World.world.GetTile((int) this._targetPosition.x, (int) this._targetPosition.y);
      this.land();
    }
    else if ((double) this._timeInAir >= (double) this._timeToTarget)
    {
      this.current_tile = World.world.GetTile((int) this._targetPosition.x, (int) this._targetPosition.y);
      this.land();
    }
    else
      this.updatePosition();
  }

  private void updatePosition()
  {
    Vector3 vector3;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3).\u002Ector(this.current_position.x, this.current_position.y + this._currentHeightZ, this._currentHeightZ);
    this.m_transform.position = vector3;
  }

  public void setScale(Vector3 pVec)
  {
    this.m_transform.localScale = pVec;
    this._scale = pVec.x;
  }

  public void setCasterId(long pCasterId) => this._caster_id = pCasterId;

  public long getCasterId() => this._caster_id;

  public void makeInactive()
  {
    this.reset();
    this.active = false;
    ((Component) this).gameObject.SetActive(false);
  }

  public void reset()
  {
    this._asset = (DropAsset) null;
    this.current_tile = (WorldTile) null;
  }

  private void OnDrawGizmos()
  {
    if (!this._parabolic || this._landed || (double) this._timeToTarget == 0.0 || (double) this._timeInAir > (double) this._timeToTarget)
      return;
    if (((Color) ref this._gizmoColor).Equals(Color.op_Implicit(Vector4.zero)))
      this._gizmoColor = Randy.ColorHSV();
    if (((Color) ref this._gizmoColor2).Equals(Color.op_Implicit(Vector4.zero)))
    {
      this._gizmoColor2 = Randy.ColorHSV();
      this._gizmoColor2.a = 0.5f;
    }
    Gizmos.color = this._gizmoColor;
    Vector2 vector2_1 = this._startPosition;
    Vector2 vector2_2 = this._startPosition;
    int num = 60;
    for (int index = 1; index <= num; ++index)
    {
      float pTime = (float) index / (float) num * this._timeToTarget;
      Vector2 vector2_3 = Toolbox.ParabolaDrag(this._startPosition, this._targetPosition, this._targetHeight, pTime);
      Vector2 vector2_4 = Toolbox.Parabola(this._startPosition, this._targetPosition, this._targetHeight, pTime);
      Gizmos.color = this._gizmoColor;
      Gizmos.DrawLine(Vector2.op_Implicit(vector2_1), Vector2.op_Implicit(vector2_3));
      Gizmos.color = this._gizmoColor2;
      Gizmos.DrawLine(Vector2.op_Implicit(vector2_1), Vector2.op_Implicit(vector2_4));
      Gizmos.DrawLine(Vector2.op_Implicit(vector2_3), Vector2.op_Implicit(vector2_4));
      Gizmos.DrawLine(Vector2.op_Implicit(vector2_2), Vector2.op_Implicit(vector2_4));
      vector2_1 = vector2_3;
      vector2_2 = vector2_4;
    }
  }
}

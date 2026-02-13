// Decompiled with JetBrains decompiler
// Type: Magnet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Magnet
{
  private const float ANIMATED_SHRINK_SPEED = 0.3f;
  private const float PICKED_UP_SPEED_MULTIPLIER = 0.1f;
  private int _magnet_state;
  private WorldTile _magnet_last_pos;
  private bool _has_units;
  internal List<Actor> magnet_units = new List<Actor>();
  private HashSet<Actor> _magnet_units = new HashSet<Actor>();
  private float _picked_up_multiplier = 1f;
  private float _angle;
  public float moving_angle;
  private MagnetThrow _magnet_throw = new MagnetThrow();
  private float _target_angle;
  private float _current_angle;
  private float _rotation_velocity;

  internal void magnetAction(bool pFromUpdate, WorldTile pTile = null)
  {
    if (ScrollWindow.isWindowActive())
    {
      this.dropPickedUnits();
    }
    else
    {
      if (pFromUpdate && this._magnet_state != 1 && this._magnet_state != 3)
        return;
      if (pTile != null)
        this._magnet_last_pos = pTile;
      this._magnet_throw.trackMouseMovement(this._magnet_state);
      this.updatePickedUnits();
      if (pTile != null)
        World.world.flash_effects.flashPixel(pTile, 10);
      switch (this._magnet_state)
      {
        case 0:
          if (!Input.GetMouseButton(0))
            break;
          this._magnet_state = 1;
          this._magnet_throw.initializeMouseTracking();
          break;
        case 1:
          if (!pFromUpdate)
            this.pickupUnits(pTile);
          if (!Input.GetMouseButtonUp(0))
            break;
          this._magnet_state = 2;
          this.dropPickedUnits();
          break;
        case 2:
          if (pFromUpdate || !Input.GetMouseButton(0))
            break;
          this.dropPickedUnits();
          this._magnet_state = 0;
          break;
      }
    }
  }

  public void dropPickedUnits()
  {
    if (this.magnet_units.Count == 0)
      return;
    Vector2 throwForce = this._magnet_throw.calculateThrowForce();
    for (int index = 0; index < this.magnet_units.Count; ++index)
    {
      Actor magnetUnit = this.magnet_units[index];
      if (magnetUnit != null && magnetUnit.isAlive())
      {
        magnetUnit.current_position.y -= magnetUnit.position_height;
        magnetUnit.is_in_magnet = false;
        magnetUnit.dirty_current_tile = true;
        magnetUnit.findCurrentTile();
        magnetUnit.spawnOn(magnetUnit.current_tile, magnetUnit.getActorAsset().default_height);
        magnetUnit.makeStunned(1f);
        magnetUnit.addStatusEffect("magnetized");
        magnetUnit.target_angle.z = 0.0f;
        if ((double) ((Vector2) ref throwForce).magnitude > 0.10000000149011612)
        {
          Vector2 vector2 = throwForce;
          vector2.x += Random.Range(-0.3f, 0.3f);
          vector2.y += Random.Range(-0.3f, 0.3f);
          magnetUnit.addForce(vector2.x, vector2.y, ((Vector2) ref vector2).magnitude * 0.3f, true, true);
        }
        else
          magnetUnit.addForce(0.0f, 0.0f, 0.1f, true);
        magnetUnit.addActionWaitAfterLand(0.5f);
      }
    }
    this.magnet_units.Clear();
    this._magnet_units.Clear();
    this._has_units = false;
    this._magnet_throw.clear();
  }

  private void updatePickedUnits()
  {
    if (this._magnet_last_pos == null || this.magnet_units.Count == 0)
      return;
    this.updateMovingForce();
    if ((double) this._picked_up_multiplier > 0.10000000149011612)
    {
      this._picked_up_multiplier -= World.world.delta_time * 0.3f;
      if ((double) this._picked_up_multiplier < 0.10000000149011612)
        this._picked_up_multiplier = 0.1f;
    }
    float count = (float) this.magnet_units.Count;
    float num1 = 6f;
    if ((double) count > 100.0)
      num1 = 4f;
    else if ((double) count > 50.0)
      num1 = 4.5f;
    else if ((double) count > 5.0)
      num1 = 5f;
    float num2 = 6.28318548f / num1;
    int num3 = Config.current_brush_data.width + 1;
    float num4 = true ? (float) num3 / 2f : Mathf.Lerp(0.0f, (float) num3, this._picked_up_multiplier) / 2f;
    float num5 = 1f / count * num4;
    this._angle += num2 * Time.deltaTime;
    Vector2 mousePos = World.world.getMousePos();
    for (int index = 0; (double) index < (double) count; ++index)
    {
      Actor magnetUnit = this.magnet_units[index];
      if (magnetUnit != null && magnetUnit.isAlive())
      {
        magnetUnit.findCurrentTile();
        Vector3 vector3 = Vector2.op_Implicit(mousePos);
        vector3.x += Mathf.Cos(this._angle + (float) index) * (num5 * (float) index);
        vector3.y += Mathf.Sin(this._angle + (float) index) * (num5 * (float) index);
        magnetUnit.current_position = new Vector2(vector3.x, vector3.y - magnetUnit.position_height);
        BaseActionActor callbacksMagnetUpdate = magnetUnit.callbacks_magnet_update;
        if (callbacksMagnetUpdate != null)
          callbacksMagnetUpdate(magnetUnit);
      }
    }
  }

  private void updateMovingForce()
  {
    Vector2 vector2 = Vector2.op_Multiply(new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")), 3f);
    if ((double) ((Vector2) ref vector2).magnitude > 0.10000000149011612)
    {
      this._target_angle = Mathf.Atan2(vector2.y, vector2.x) * 57.29578f;
      this._target_angle -= 90f;
    }
    else
      this._target_angle = 0.0f;
    this._current_angle = Mathf.SmoothDampAngle(this._current_angle, this._target_angle, ref this._rotation_velocity, 0.2f);
    this.moving_angle = this._current_angle;
  }

  private void pickupUnits(WorldTile pTile)
  {
    BrushPixelData[] pos = Config.current_brush_data.pos;
    for (int index = 0; index < pos.Length; ++index)
    {
      WorldTile tile = World.world.GetTile(pos[index].x + pTile.x, pos[index].y + pTile.y);
      if (tile != null && tile.hasUnits())
        tile.doUnits((Action<Actor>) (tActor =>
        {
          if (!tActor.asset.can_be_moved_by_powers || tActor.isInsideSomething() || !this._magnet_units.Add(tActor))
            return;
          tActor.cancelAllBeh();
          this.magnet_units.Add(tActor);
          tActor.is_in_magnet = true;
          this._picked_up_multiplier = 2f;
        }));
    }
    this._has_units = this._magnet_units.Count > 0;
  }

  public int countUnits() => this._magnet_units.Count;

  public bool hasUnits() => this._has_units;
}

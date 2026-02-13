// Decompiled with JetBrains decompiler
// Type: MagnetThrow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MagnetThrow
{
  private Vector2 _mouse_velocity = Vector2.zero;
  private Vector2 _last_mouse_position;
  private readonly List<Vector2> _velocity_samples = new List<Vector2>();
  private const int MAX_VELOCITY_SAMPLES = 5;
  private const float THROW_FORCE_MULTIPLIER = 5f;
  public const float MIN_THROW_FORCE = 0.1f;
  private const float MAX_THROW_FORCE = 10f;
  private Vector2 _throw_momentum = Vector2.zero;
  private const float MOMENTUM_DECAY = 0.85f;
  private const float MOMENTUM_BUILD_RATE = 0.7f;

  public void initializeMouseTracking()
  {
    this._last_mouse_position = World.world.getMousePos();
    this._velocity_samples.Clear();
    this._throw_momentum = Vector2.zero;
  }

  public void trackMouseMovement(int pMagnetState)
  {
    if (pMagnetState != 1)
      return;
    Vector2 mousePos = World.world.getMousePos();
    Vector2 vector2 = Vector2.op_Subtraction(mousePos, this._last_mouse_position);
    this._mouse_velocity = Vector2.op_Multiply(vector2, 60f);
    this._velocity_samples.Add(this._mouse_velocity);
    if (this._velocity_samples.Count > 5)
      this._velocity_samples.RemoveAt(0);
    this._throw_momentum = Vector2.Lerp(this._throw_momentum, Vector2.op_Multiply(vector2, 0.7f), Time.deltaTime * 10f);
    this._last_mouse_position = mousePos;
  }

  public Vector2 calculateThrowForce()
  {
    Vector2 vector2 = Vector2.zero;
    if (this._velocity_samples.Count > 0)
    {
      foreach (Vector2 velocitySample in this._velocity_samples)
        vector2 = Vector2.op_Addition(vector2, velocitySample);
      vector2 = Vector2.op_Division(vector2, (float) this._velocity_samples.Count);
    }
    Vector2 throwForce = Vector2.op_Multiply(Vector2.op_Multiply(vector2, 5f), Time.deltaTime);
    float magnitude = ((Vector2) ref throwForce).magnitude;
    if ((double) magnitude > 10.0)
      throwForce = Vector2.op_Multiply(((Vector2) ref throwForce).normalized, 10f);
    else if ((double) magnitude < 0.10000000149011612 && (double) magnitude > 0.10000000149011612)
      throwForce = Vector2.op_Multiply(((Vector2) ref throwForce).normalized, 0.1f);
    return throwForce;
  }

  public void clear()
  {
    this._velocity_samples.Clear();
    this._throw_momentum = Vector2.zero;
  }
}

// Decompiled with JetBrains decompiler
// Type: BuildingTweenScaleHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
public class BuildingTweenScaleHelper
{
  internal bool active;
  internal float scale_start;
  internal float scale_target = 1f;
  internal double scale_time;
  internal float scale_duration = 1f;
  internal float scale_last_priority;
  internal bool scale_use_x;
  internal Action scale_final_action;
  internal EasingFunction scale_ease;
  public float angle_target;
  public float angle_duration;
  public float angle_time;
  internal Action angle_final_action;

  public void doRotateTween(float pTargetAngle, float pDuration, Action pAction)
  {
    this.angle_target = pTargetAngle;
    this.angle_duration = pDuration;
    this.angle_final_action = pAction;
    this.angle_time = 0.0f;
  }

  public void reset()
  {
    this.active = false;
    this.scale_start = 0.0f;
    this.scale_target = 1f;
    this.scale_time = 0.0;
    this.scale_duration = 1f;
    this.scale_last_priority = 0.0f;
    this.scale_use_x = false;
    this.scale_final_action = (Action) null;
    this.scale_ease = (EasingFunction) null;
    this.angle_target = 0.0f;
    this.angle_duration = 0.0f;
    this.angle_time = 0.0f;
    this.angle_final_action = (Action) null;
  }
}

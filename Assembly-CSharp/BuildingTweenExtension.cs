// Decompiled with JetBrains decompiler
// Type: BuildingTweenExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public static class BuildingTweenExtension
{
  internal static void checkTweens(this Building pBuilding)
  {
    switch (pBuilding.animation_state)
    {
      case BuildingAnimationState.OnRuin:
        // ISSUE: method pointer
        pBuilding.setScaleTween(1f, 0.1f, 0.0f, new Action(pBuilding.completeMakingRuin), new EasingFunction((object) null, __methodptr(easeInCubic)));
        break;
      case BuildingAnimationState.OnRemove:
        // ISSUE: method pointer
        EasingFunction pEase = new EasingFunction((object) null, __methodptr(easeInBack));
        if (pBuilding.chopped)
        {
          // ISSUE: method pointer
          pEase = new EasingFunction((object) null, __methodptr(easeInCubic));
          pBuilding.scale_helper.scale_use_x = true;
        }
        pBuilding.setScaleTween(1f, 0.5f, 0.0f, new Action(pBuilding.removeBuildingFinal), pEase, 1);
        if (!pBuilding.asset.city_building)
          break;
        pBuilding.startShake(0.5f);
        break;
    }
  }

  internal static void setScaleTween(
    this Building pBuilding,
    float pFrom = 0.0f,
    float pDuration = 0.2f,
    float pTarget = 1f,
    Action pActionOnComplete = null,
    EasingFunction pEase = null,
    int pPriority = 0)
  {
    BuildingTweenScaleHelper scaleHelper = pBuilding.scale_helper;
    if (scaleHelper.active && scaleHelper.scale_final_action != null && (double) scaleHelper.scale_last_priority >= (double) pPriority)
      return;
    if (pEase == null)
    {
      // ISSUE: method pointer
      pEase = new EasingFunction((object) null, __methodptr(easeOutBack));
    }
    scaleHelper.active = true;
    scaleHelper.scale_start = pFrom;
    scaleHelper.scale_target = pTarget;
    scaleHelper.scale_time = World.world.getCurSessionTime() + (double) pDuration;
    scaleHelper.scale_duration = pDuration;
    scaleHelper.scale_final_action = pActionOnComplete;
    scaleHelper.scale_ease = pEase;
    if (scaleHelper.scale_use_x)
      pBuilding.current_scale.x = pBuilding.asset.scale_base.x * pFrom;
    else
      pBuilding.current_scale.y = pBuilding.asset.scale_base.y * pFrom;
    pBuilding.batch.c_scale.Add(pBuilding);
  }

  public static void checkFinalAction(this Building pBuilding)
  {
    Action scaleFinalAction = pBuilding.scale_helper.scale_final_action;
    if (scaleFinalAction != null)
      scaleFinalAction();
    pBuilding.scale_helper.scale_final_action = (Action) null;
    Action angleFinalAction = pBuilding.scale_helper.angle_final_action;
    if (angleFinalAction != null)
      angleFinalAction();
    pBuilding.scale_helper.angle_final_action = (Action) null;
  }

  internal static void finishScaleTween(this Building pBuilding)
  {
    pBuilding.setAnimationState(BuildingAnimationState.Normal);
    BuildingTweenScaleHelper scaleHelper = pBuilding.scale_helper;
    scaleHelper.scale_time = World.world.getCurSessionTime() + (double) scaleHelper.scale_duration;
  }

  internal static void updateAngle(this Building pBuilding, float pElapsed)
  {
    if ((double) pBuilding.current_rotation.z == (double) pBuilding.scale_helper.angle_target)
      return;
    BuildingTweenScaleHelper scaleHelper = pBuilding.scale_helper;
    scaleHelper.angle_time += pElapsed;
    if ((double) scaleHelper.angle_time >= 1.0)
    {
      scaleHelper.angle_time = 1f;
      pBuilding.batch.c_angle.Remove(pBuilding);
      pBuilding.batch.actions_to_run.Add(new Action(((BuildingTweenExtension) pBuilding).checkFinalAction));
    }
    float num = iTween.easeInExpo(0.0f, 1f, scaleHelper.angle_time);
    ((Vector3) ref pBuilding.current_rotation).Set(0.0f, 0.0f, num * pBuilding.scale_helper.angle_target);
  }

  internal static void updateScale(this Building pBuilding)
  {
    if (!pBuilding.scale_helper.active)
      return;
    BuildingTweenScaleHelper scaleHelper = pBuilding.scale_helper;
    double num1 = scaleHelper.scale_time - World.world.getCurSessionTime();
    float num2;
    if (num1 <= 0.0)
    {
      scaleHelper.scale_time = World.world.getCurSessionTime() + (double) scaleHelper.scale_duration;
      scaleHelper.active = false;
      pBuilding.batch.actions_to_run.Add(new Action(((BuildingTweenExtension) pBuilding).checkFinalAction));
      pBuilding.batch.c_scale.Remove(pBuilding);
      num2 = scaleHelper.scale_target;
    }
    else
    {
      float num3 = (scaleHelper.scale_duration - (float) num1) / scaleHelper.scale_duration;
      num2 = scaleHelper.scale_ease.Invoke(scaleHelper.scale_start, scaleHelper.scale_target, num3);
    }
    if (scaleHelper.scale_use_x)
      pBuilding.current_scale.x = pBuilding.asset.scale_base.x * num2;
    else
      pBuilding.current_scale.y = pBuilding.asset.scale_base.y * num2;
  }
}

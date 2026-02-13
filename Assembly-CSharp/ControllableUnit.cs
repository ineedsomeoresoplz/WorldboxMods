// Decompiled with JetBrains decompiler
// Type: ControllableUnit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public static class ControllableUnit
{
  private const float TOUCH_ATTACK_START_DELAY = 0.05f;
  private static Actor _unit_main = (Actor) null;
  private static HashSet<Actor> _units = new HashSet<Actor>();
  private static Vector2 _movement_vector;
  private static Vector2 _click_vector;
  private static bool _action_pressed_jump = false;
  private static bool _action_pressed_dash = false;
  private static bool _action_pressed_backstep = false;
  private static bool _action_pressed_steal = false;
  private static bool _action_pressed_swear = false;
  private static bool _action_pressed_talk = false;
  private static bool _attack_pressed_button_left = false;
  private static bool _attack_pressed_button_right = false;
  private static bool _attack_just_pressed_button_left = false;
  private static bool _attack_just_pressed_button_right = false;
  private static float _touch_attack_started_at;
  private static bool _touch_attack_just_started;
  private static string[] _possessed_icons = new string[6]
  {
    "ui/Icons/iconBre",
    "ui/Icons/iconCrying",
    "ui/Icons/iconAngry",
    "ui/Icons/actor_traits/iconStupid",
    "ui/Icons/actor_traits/iconStrongMinded",
    "ui/Icons/iconDead"
  };

  public static bool isControllingUnit(Actor pUnit)
  {
    return ControllableUnit.isControllingUnit() && ControllableUnit._units.Contains(pUnit);
  }

  public static HashSet<Actor> getCotrolledUnits() => ControllableUnit._units;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool isControllingUnit() => ControllableUnit._units.Any<Actor>();

  public static int count() => ControllableUnit._units.Count;

  public static bool isControllingCrabzilla()
  {
    return ControllableUnit.isControllingUnit() && ControllableUnit._unit_main.asset.id == "crabzilla";
  }

  public static bool isControllingNormalUnits()
  {
    return ControllableUnit.isControllingUnit() && ControllableUnit._unit_main.asset.show_controllable_tip;
  }

  public static Actor getControllableUnit() => ControllableUnit._unit_main;

  public static bool isAttackPressedLeft() => ControllableUnit._attack_pressed_button_left;

  public static bool isAttackPressedRight() => ControllableUnit._attack_pressed_button_right;

  public static bool isAttackJustPressedLeft() => ControllableUnit._attack_just_pressed_button_left;

  public static bool isAttackJustPressedRight()
  {
    return ControllableUnit._attack_just_pressed_button_right;
  }

  public static void setControllableCreatures(ListPool<Actor> pListActors)
  {
    // ISSUE: unable to decompile the method.
  }

  public static void setControllableCreatureAndSelected(Actor pActor)
  {
    using (ListPool<Actor> pListActors = new ListPool<Actor>())
    {
      foreach (Actor actor in SelectedUnit.getAllSelected())
      {
        if (actor.canBePossessed())
          pListActors.Add(actor);
      }
      ControllableUnit.setControllableCreatures(pListActors);
      ControllableUnit.setControllableCreature(pActor);
      SelectedUnit.clear();
    }
  }

  public static void setControllableCreatureCrabzilla(Actor pActor)
  {
    ControllableUnit.setControllableCreature(pActor);
  }

  public static void setControllableCreature(Actor pActor)
  {
    if (!pActor.canBePossessed())
      return;
    SelectedUnit.clear();
    ControllableUnit._unit_main = pActor;
    ControllableUnit._units.Add(pActor);
    ControllableUnit.addStatus(pActor);
    if (ControllableUnit.isControllingUnit())
    {
      Config.setWorldSpeed("x1");
      Config.paused = false;
    }
    if (Config.joyControls)
    {
      if (ControllableUnit.isControllingUnit())
      {
        World.world.joys.SetActive(true);
        if (ControllableUnit.isControllingCrabzilla())
        {
          UltimateJoystick.EnableJoystick("JoyRight");
          ((Component) TouchPossessionController.instance).gameObject.SetActive(false);
        }
        else
        {
          UltimateJoystick.DisableJoystick("JoyRight");
          if (!InputHelpers.mouseSupported)
            ((Component) TouchPossessionController.instance).gameObject.SetActive(true);
        }
      }
      else
        World.world.joys.SetActive(false);
      UltimateJoystick.ResetJoysticks();
    }
    else if (Object.op_Inequality((Object) World.world.joys, (Object) null))
    {
      Object.Destroy((Object) World.world.joys, 0.5f);
      World.world.joys = (GameObject) null;
    }
    ControllableUnit._movement_vector = Vector2.zero;
    ControllableUnit.resetClickVector();
    ControllableUnit._attack_pressed_button_left = false;
    ControllableUnit._attack_pressed_button_right = false;
    ControllableUnit._attack_just_pressed_button_left = false;
    ControllableUnit._attack_just_pressed_button_right = false;
    if (!ControllableUnit.isControllingNormalUnits() || !InputHelpers.mouseSupported)
      return;
    PossessionUI.toggle(true);
  }

  private static void resetClickVector() => ControllableUnit._click_vector = Vector2.zero;

  public static Vector2 getMovementVector() => ControllableUnit._movement_vector;

  public static Vector2 getClickVector() => ControllableUnit._click_vector;

  public static bool isActionPressedJump() => ControllableUnit._action_pressed_jump;

  public static bool isActionPressedTalk() => ControllableUnit._action_pressed_talk;

  public static bool isActionPressedDash() => ControllableUnit._action_pressed_dash;

  public static bool isActionPressedBackstep() => ControllableUnit._action_pressed_backstep;

  public static bool isActionPressedSteal() => ControllableUnit._action_pressed_steal;

  public static bool isActionPressedSwear() => ControllableUnit._action_pressed_swear;

  public static void remove(Actor pActor)
  {
    ControllableUnit._units.Remove(pActor);
    if (ControllableUnit._unit_main != pActor)
      return;
    ControllableUnit._unit_main = (Actor) null;
    ControllableUnit.trySelectNewMain();
  }

  private static void trySelectNewMain()
  {
    if (ControllableUnit._units.Count == 0)
      ControllableUnit.clear();
    else
      ControllableUnit._unit_main = ControllableUnit._units.GetRandom<Actor>();
  }

  public static void clear(bool pCallKill = true)
  {
    // ISSUE: unable to decompile the method.
  }

  public static void updateControllableUnit()
  {
    if (!ControllableUnit.isControllingUnit())
      return;
    if (InputHelpers.GetAnyMouseButtonUp())
    {
      foreach (Actor cotrolledUnit in ControllableUnit.getCotrolledUnits())
        cotrolledUnit.resetAttackTimeout();
    }
    ControllableUnit.updateCamera();
    ControllableUnit.updateMovementVector();
    ControllableUnit.updateClick();
    ControllableUnit.updateMouseAttackPosition();
    ControllableUnit.checkActions();
    ControllableUnit.checkPossessionStatus();
  }

  private static bool isAnyActionsPressed()
  {
    return ControllableUnit._action_pressed_jump || ControllableUnit._action_pressed_dash || ControllableUnit._action_pressed_steal || ControllableUnit._action_pressed_swear || ControllableUnit._action_pressed_talk || ControllableUnit._action_pressed_backstep;
  }

  private static void checkActions()
  {
    ControllableUnit._action_pressed_jump = HotkeyLibrary.action_jump.isJustPressed() || TouchPossessionController.isActionPressedJump();
    ControllableUnit._action_pressed_dash = HotkeyLibrary.action_dash.isJustPressed() || TouchPossessionController.isActionPressedDash();
    ControllableUnit._action_pressed_backstep = HotkeyLibrary.action_backstep.isJustPressed() || TouchPossessionController.isActionPressedBackStep();
    bool pressedButtonLeft = ControllableUnit._attack_pressed_button_left;
    ControllableUnit._action_pressed_steal = HotkeyLibrary.action_steal.isJustPressed() || pressedButtonLeft && TouchPossessionController.isSelectedActionSteal();
    ControllableUnit._action_pressed_swear = HotkeyLibrary.action_swear.isJustPressed() || pressedButtonLeft && TouchPossessionController.isSelectedActionSwear();
    ControllableUnit._action_pressed_talk = HotkeyLibrary.action_talk.isJustPressed() || pressedButtonLeft && TouchPossessionController.isSelectedActionTalk();
    ControllableUnit._attack_just_pressed_button_right = ControllableUnit._attack_just_pressed_button_right || pressedButtonLeft && TouchPossessionController.isSelectedActionKick();
  }

  private static void updateMouseAttackPosition()
  {
    ControllableUnit.resetClickVector();
    if (!InputHelpers.mouseSupported && Input.touchSupported)
      ControllableUnit._click_vector = ControllableUnit.getTouchAttackPosition();
    else
      ControllableUnit._click_vector = World.world.getMousePos();
  }

  private static bool getAttackTouch(out Touch pTouch)
  {
    pTouch = new Touch();
    if (World.world.player_control.already_used_zoom)
      return false;
    UltimateJoystick ultimateJoystick = UltimateJoystick.GetUltimateJoystick("JoyLeft");
    bool joystickState = ultimateJoystick.GetJoystickState();
    int touchId = ultimateJoystick.getTouchId();
    bool attackTouch = false;
    foreach (Touch touch in Input.touches)
    {
      if (!World.world.isTouchOverUI(touch) && (!joystickState || ((Touch) ref touch).fingerId != touchId))
      {
        pTouch = touch;
        attackTouch = true;
        break;
      }
    }
    return attackTouch;
  }

  private static Vector2 getTouchAttackPosition()
  {
    Vector2 touchAttackPosition = Vector2.zero;
    Touch pTouch;
    if (ControllableUnit.getAttackTouch(out pTouch))
      touchAttackPosition = Vector2.op_Implicit(World.world.camera.ScreenToWorldPoint(Vector2.op_Implicit(((Touch) ref pTouch).position)));
    return touchAttackPosition;
  }

  private static void checkPossessionStatus()
  {
    if (!Vector2.op_Inequality(ControllableUnit._movement_vector, Vector2.zero) && !ControllableUnit._attack_pressed_button_right && !ControllableUnit._attack_pressed_button_left && !ControllableUnit.isAnyActionsPressed())
      return;
    foreach (Actor cotrolledUnit in ControllableUnit.getCotrolledUnits())
    {
      ControllableUnit.addStatus(cotrolledUnit);
      cotrolledUnit.stopSleeping();
      ControllableUnit.fixNextStep(cotrolledUnit);
    }
  }

  private static void fixNextStep(Actor pActor)
  {
    pActor.next_step_position = pActor.next_step_position_possession;
  }

  private static void addStatus(Actor pActor)
  {
    bool flag = false;
    if (pActor.hasStatus("possessed"))
      flag = true;
    pActor.addStatusEffect("possessed", 10f, false);
    pActor.cancelAllBeh();
    if (flag || !pActor.hasTag("strong_mind"))
      return;
    pActor.spawnSlashYell(World.world.getMousePos());
    pActor.addStatusEffect("swearing", 2f, false);
    pActor.punchTargetAnimation(Vector2.op_Implicit(World.world.getMousePos()), false, pAngle: -40f);
    string random = ControllableUnit._possessed_icons.GetRandom<string>();
    pActor.forceSocializeTopic(random);
  }

  private static void updateClick()
  {
    if (Config.joyControls)
    {
      if (((Component) UltimateJoystick.GetUltimateJoystick("JoyRight")).gameObject.activeSelf)
      {
        if (ControllableUnit._attack_pressed_button_left && !UltimateJoystick.GetJoystickState("JoyRight"))
        {
          ControllableUnit._attack_pressed_button_left = false;
        }
        else
        {
          if (!UltimateJoystick.GetTapCount("JoyRight"))
            return;
          ControllableUnit._attack_pressed_button_left = !ControllableUnit._attack_pressed_button_left;
        }
      }
      else
      {
        bool joystickState = UltimateJoystick.GetJoystickState("JoyLeft");
        if (joystickState && Input.touchCount <= 1)
        {
          ControllableUnit._attack_pressed_button_left = false;
          ControllableUnit._attack_pressed_button_right = false;
          ControllableUnit._attack_just_pressed_button_left = false;
          ControllableUnit._attack_just_pressed_button_right = false;
        }
        else
        {
          Touch pTouch;
          if (ControllableUnit.getAttackTouch(out pTouch))
          {
            if (((Touch) ref pTouch).phase == 0)
            {
              ControllableUnit._touch_attack_started_at = Time.time;
              ControllableUnit._touch_attack_just_started = true;
            }
            else
            {
              int num = joystickState ? 1 : 0;
              ControllableUnit._attack_pressed_button_left = ((Touch) ref pTouch).phase == 2 || ((Touch) ref pTouch).phase == 1;
              ControllableUnit._attack_pressed_button_right = false;
              ControllableUnit._attack_just_pressed_button_left = ControllableUnit._touch_attack_just_started;
              ControllableUnit._attack_just_pressed_button_right = false;
              ControllableUnit._touch_attack_just_started = false;
            }
          }
          else
          {
            ControllableUnit._attack_pressed_button_left = false;
            ControllableUnit._attack_pressed_button_right = false;
            ControllableUnit._attack_just_pressed_button_left = false;
            ControllableUnit._attack_just_pressed_button_right = false;
          }
        }
      }
    }
    else
    {
      ControllableUnit._attack_pressed_button_left = Input.GetMouseButton(0);
      ControllableUnit._attack_pressed_button_right = Input.GetMouseButton(1);
      ControllableUnit._attack_just_pressed_button_left = Input.GetMouseButtonDown(0);
      ControllableUnit._attack_just_pressed_button_right = Input.GetMouseButtonDown(1);
    }
  }

  public static bool isMovementActionActive()
  {
    if (Config.joyControls)
    {
      UltimateJoystick ultimateJoystick = UltimateJoystick.GetUltimateJoystick("JoyLeft");
      return !Object.op_Equality((Object) ultimateJoystick, (Object) null) && ultimateJoystick.GetJoystickState();
    }
    return HotkeyLibrary.up.isHolding() || HotkeyLibrary.down.isHolding() || HotkeyLibrary.left.isHolding() || HotkeyLibrary.right.isHolding();
  }

  private static void updateMovementVector()
  {
    if (Config.joyControls)
      ControllableUnit.updateMovementVectorJoystick();
    else
      ControllableUnit.updateMovementVectorKeyboard();
  }

  private static void updateMovementVectorJoystick()
  {
    if (!ControllableUnit.isMovementActionActive())
      return;
    float axisVerticalLeft = ControllableUnit.getJoyAxisVerticalLeft();
    float axisHorizontalLeft = ControllableUnit.getJoyAxisHorizontalLeft();
    ControllableUnit._movement_vector.x = axisHorizontalLeft;
    ControllableUnit._movement_vector.y = axisVerticalLeft;
  }

  private static void updateMovementVectorKeyboard()
  {
    ControllableUnit._movement_vector = Vector2.zero;
    if (HotkeyLibrary.up.isHolding())
      ControllableUnit._movement_vector.y = 1f;
    else if (HotkeyLibrary.down.isHolding())
      ControllableUnit._movement_vector.y = -1f;
    if (HotkeyLibrary.right.isHolding())
    {
      ControllableUnit._movement_vector.x = 1f;
    }
    else
    {
      if (!HotkeyLibrary.left.isHolding())
        return;
      ControllableUnit._movement_vector.x = -1f;
    }
  }

  public static void updateCamera()
  {
    Vector2 currentPosition = ControllableUnit._unit_main.current_position;
    Vector3 position = ((Component) World.world.camera).transform.position;
    position.x = currentPosition.x;
    position.y = currentPosition.y;
    float num = 1f / World.world.camera.orthographicSize;
    ((Component) World.world.camera).transform.position = Vector3.Lerp(((Component) World.world.camera).transform.position, position, num);
  }

  public static float getJoyAxisVerticalRight() => UltimateJoystick.GetVerticalAxis("JoyRight");

  public static float getJoyAxisHorizontalRight() => UltimateJoystick.GetHorizontalAxis("JoyRight");

  private static float getJoyAxisVerticalLeft() => UltimateJoystick.GetVerticalAxis("JoyLeft");

  private static float getJoyAxisHorizontalLeft() => UltimateJoystick.GetHorizontalAxis("JoyLeft");
}

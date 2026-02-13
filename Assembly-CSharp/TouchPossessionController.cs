// Decompiled with JetBrains decompiler
// Type: TouchPossessionController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class TouchPossessionController : MonoBehaviour
{
  public static TouchPossessionController instance;
  [SerializeField]
  private GameObject _button_dash;
  [SerializeField]
  private GameObject _button_jump;
  [SerializeField]
  private GameObject _button_backstep;
  [SerializeField]
  private GameObject _button_attack;
  [SerializeField]
  private GameObject _button_kick;
  [SerializeField]
  private GameObject _button_talk;
  [SerializeField]
  private GameObject _button_swear;
  [SerializeField]
  private GameObject _button_steal;
  public List<PossessionModeButton> possession_mode_buttons = new List<PossessionModeButton>();
  [SerializeField]
  private RectTransform _rect;
  [SerializeField]
  private UltimateJoystick _right_joystick;
  private static bool _action_pressed_jump;
  private static bool _action_pressed_dash;
  private static bool _action_pressed_backstep;
  public static PossessionActionMode _current_mode;

  private void Awake() => TouchPossessionController.instance = this;

  private void OnEnable()
  {
    this.onResizeResolution((float) Screen.width, (float) Screen.height);
    this.checkButtonGraphics();
    this.setMode(PossessionActionMode.Attack);
  }

  private void Update() => this.checkActiveButtons();

  private void checkActiveButtons()
  {
    if (!ControllableUnit.isControllingUnit() || ControllableUnit.isControllingCrabzilla())
      return;
    Actor controllableUnit = ControllableUnit.getControllableUnit();
    ActorAsset asset = controllableUnit.asset;
    this._button_dash.gameObject.SetActive(asset.control_can_dash);
    this._button_jump.gameObject.SetActive(asset.control_can_jump);
    this._button_backstep.gameObject.SetActive(asset.control_can_backstep);
    this._button_attack.gameObject.SetActive(!asset.skip_fight_logic);
    this._button_kick.gameObject.SetActive(asset.control_can_kick);
    this._button_talk.gameObject.SetActive(asset.control_can_talk && !controllableUnit.hasTrait("mute"));
    this._button_swear.gameObject.SetActive(asset.control_can_swear && !controllableUnit.hasTrait("mute"));
    this._button_steal.gameObject.SetActive(asset.control_can_steal);
  }

  private void onResizeResolution(float pWidth, float pHeight)
  {
    this._right_joystick.UpdateSizeAndPlacement(this._rect);
  }

  public static bool isActionPressedJump() => TouchPossessionController._action_pressed_jump;

  public static bool isActionPressedDash() => TouchPossessionController._action_pressed_dash;

  public static bool isActionPressedBackStep()
  {
    return TouchPossessionController._action_pressed_backstep;
  }

  public static bool isSelectedActionAttack()
  {
    return TouchPossessionController.isMode(PossessionActionMode.Attack);
  }

  public static bool isSelectedActionTalk()
  {
    return TouchPossessionController.isMode(PossessionActionMode.Talk);
  }

  public static bool isSelectedActionSwear()
  {
    return TouchPossessionController.isMode(PossessionActionMode.Swear);
  }

  public static bool isSelectedActionSteal()
  {
    return TouchPossessionController.isMode(PossessionActionMode.Steal);
  }

  public static bool isSelectedActionKick()
  {
    return TouchPossessionController.isMode(PossessionActionMode.Kick);
  }

  public static void pressJump() => TouchPossessionController._action_pressed_jump = true;

  public static void pressDash() => TouchPossessionController._action_pressed_dash = true;

  public static void pressBackStep() => TouchPossessionController._action_pressed_backstep = true;

  public void selectModeAttack()
  {
    WorldTip.showNow("possession_action_mode_attack", pPosition: "top");
    this.setMode(PossessionActionMode.Attack);
  }

  public void selectModeTalk()
  {
    WorldTip.showNow("possession_action_mode_talk", pPosition: "top");
    this.setMode(PossessionActionMode.Talk);
  }

  public void selectModeSwear()
  {
    WorldTip.showNow("possession_action_mode_swear", pPosition: "top");
    this.setMode(PossessionActionMode.Swear);
  }

  public void selectModeSteal()
  {
    WorldTip.showNow("possession_action_mode_steal", pPosition: "top");
    this.setMode(PossessionActionMode.Steal);
  }

  public void selectModeKick()
  {
    WorldTip.showNow("possession_action_mode_kick", pPosition: "top");
    this.setMode(PossessionActionMode.Kick);
  }

  private static bool isMode(PossessionActionMode pMode)
  {
    return TouchPossessionController._current_mode == pMode;
  }

  private void setMode(PossessionActionMode pMode)
  {
    TouchPossessionController._current_mode = pMode;
    this.checkButtonGraphics();
  }

  private void checkButtonGraphics()
  {
    foreach (PossessionModeButton possessionModeButton in this.possession_mode_buttons)
      possessionModeButton.updateGraphics(TouchPossessionController._current_mode);
  }

  private void LateUpdate() => this.clearActions();

  private void clearActions()
  {
    TouchPossessionController._action_pressed_jump = false;
    TouchPossessionController._action_pressed_dash = false;
    TouchPossessionController._action_pressed_backstep = false;
  }
}

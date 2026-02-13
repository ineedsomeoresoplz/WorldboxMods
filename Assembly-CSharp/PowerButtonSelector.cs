// Decompiled with JetBrains decompiler
// Type: PowerButtonSelector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class PowerButtonSelector : MonoBehaviour
{
  private const float TOOLBAR_TEMP_SHOW_DURATION = 1f;
  private const float TOOLBAR_TEMP_SHOW_ALPHA = 0.5f;
  public GameObject buttonSelectionSprite;
  public GameObject buttonUnlocked;
  public GameObject buttonUnlockedFlash;
  public GameObject buttonUnlockedFlashNew;
  [SerializeField]
  private CanvasGroup _toolbar_canvas_group;
  public UiMover cancelUnitSelectedMover;
  public UiMover cancelButtMover;
  public UiMover sizeButtMover;
  public UiMover clockButtMover;
  public UiMover bottomElementsMover;
  public UiMover spectateUnitMover;
  public UiMover pauseButtonMover;
  public UiMover unhideButtonMover;
  public PowerButton clockButton;
  public PowerButton sizeButton;
  public CancelButton cancelButton;
  public PowerButton pauseButton;
  public PowerButton pauseButton2;
  public PowerButton cityInfo;
  public PowerButton cityZones;
  public PowerButton boatMarks;
  public PowerButton kingsAndLeaders;
  public PowerButton historyLog;
  public PowerButton followUnit;
  public GameObject joy_control_cancel_button;
  [SerializeField]
  private Image _joy_control_cancel_button_icon;
  private Coroutine _toolbar_hide_routine;
  private bool _is_toolbar_temp_showed;
  internal PowerButton selectedButton;
  public GameObject buttons;
  internal static PowerButtonSelector instance;

  private void Awake() => PowerButtonSelector.instance = this;

  private void Start()
  {
    this.clockButtMover.setVisible(false, true);
    this.sizeButtMover.setVisible(false, true);
    this.cancelButtMover.setVisible(false, true);
    this.cancelUnitSelectedMover.setVisible(false, true);
    this.toggleBottomElements(false, true);
    this.spectateUnitMover.setVisible(false, true);
    this.resetToolbarTempShow();
  }

  internal void checkToggleIcons()
  {
    Color color;
    // ISSUE: explicit constructor call
    ((Color) ref color).\u002Ector(0.7f, 0.7f, 0.7f, 1f);
    foreach (PowerButton toggleButton in PowerButton.toggle_buttons)
    {
      toggleButton.checkToggleIcon();
      if (toggleButton.godPower.option_asset.isActive())
        ((Graphic) toggleButton.icon).color = Color.white;
      else
        ((Graphic) toggleButton.icon).color = color;
    }
  }

  public virtual void setSelectedPower(PowerButton pButton, GodPower pPower, bool pAnim = false)
  {
    if (Object.op_Equality((Object) this.selectedButton, (Object) null))
      return;
    GodPower godPower = this.selectedButton.godPower;
    this.selectedButton.setSelectedPower(pButton);
    this.selectedButton.newClickAnimation();
  }

  public void setPower(PowerButton pButton)
  {
    this.selectedButton = pButton;
    if (Object.op_Inequality((Object) this.selectedButton, (Object) null) && this.selectedButton.godPower != null)
    {
      Config.debug_last_selected_power_button = this.selectedButton.godPower.id;
      if (this.selectedButton.godPower.type == PowerActionType.PowerSpawnActor)
      {
        ActorAsset actorAsset = this.selectedButton.godPower.getActorAsset();
        if (actorAsset.has_sound_spawn)
          MusicBox.playSoundUI(actorAsset.sound_spawn);
      }
    }
    if (Object.op_Inequality((Object) this.selectedButton, (Object) null))
    {
      this.cancelButton.setIconFrom(this.selectedButton);
      LogText.log("Power Selected", pButton.godPower.id);
    }
    if (Object.op_Equality((Object) pButton, (Object) null))
      PowerTracker.setPower((GodPower) null);
    else
      PowerTracker.setPower(pButton.godPower);
  }

  public void unselectTabs()
  {
    if (!PowersTab.isTabSelected())
      return;
    SelectedObjects.unselectNanoObject();
    PowersTab.unselect();
    SelectedTabsHistory.clear();
  }

  public void unselectAll()
  {
    if (Object.op_Inequality((Object) this.selectedButton, (Object) null))
    {
      this.selectedButton.unselectActivePower();
      this.setPower((PowerButton) null);
      this.clearHighlightedButton();
      WorldTip.instance.startHide();
    }
    if (ControllableUnit.isControllingUnit())
      ControllableUnit.clear();
    if (!MoveCamera.hasFocusUnit())
      return;
    MoveCamera.clearFocusUnitOnly();
  }

  public bool isPowerSelected()
  {
    return Object.op_Inequality((Object) this.selectedButton, (Object) null);
  }

  public bool isPowerSelected(PowerButton pButton)
  {
    return Object.op_Equality((Object) this.selectedButton, (Object) pButton);
  }

  public void clickPowerButton(PowerButton pButton)
  {
    if (!pButton.canSelect())
    {
      if (InputHelpers.mouseSupported)
        return;
      this.showToolbarText(pButton);
    }
    else if (Object.op_Equality((Object) this.selectedButton, (Object) pButton))
    {
      this.unselectAll();
    }
    else
    {
      if (Object.op_Inequality((Object) this.selectedButton, (Object) null))
        this.selectedButton.unselectActivePower();
      if (pButton.godPower.select_button_action != null && pButton.godPower.select_button_action(pButton.godPower.id))
        return;
      this.setPower(pButton);
      if (Object.op_Inequality((Object) this.selectedButton, (Object) null))
      {
        this.highlightButton(this.selectedButton);
        if (this.selectedButton.godPower != null)
          Config.logSelectedPower(this.selectedButton.godPower);
      }
      if (InputHelpers.mouseSupported)
        this.showToolbarText(pButton);
      Analytics.LogEvent("select_power", "powerID", pButton.godPower.id);
    }
  }

  public void showToolbarText(PowerButton pButton)
  {
    WorldTip.instance.showToolbarText(pButton.godPower);
  }

  internal void highlightButton(PowerButton pButton)
  {
    if (Object.op_Equality((Object) pButton, (Object) null))
      return;
    this.buttonSelectionSprite.SetActive(true);
    RectTransform transform = (RectTransform) this.buttonSelectionSprite.transform;
    ((Transform) transform).position = ((Component) pButton).transform.position;
    ((Transform) transform).SetParent(((Component) pButton).transform.parent);
    ((Transform) transform).localScale = Vector3.one;
    transform.sizeDelta = pButton.rect_transform.sizeDelta;
  }

  internal void clearHighlightedButton() => this.buttonSelectionSprite.SetActive(false);

  private void Update()
  {
    this.updateSelectedPowerButtons();
    this.updateHideUiButton();
    this.updateSelectedUnitCancelButton();
  }

  private bool isSpecialTabActive() => SelectedUnit.isSet() || SelectedObjects.isNanoObjectSet();

  private void updateSelectedUnitCancelButton()
  {
    if (!World.world.isAnyPowerSelected() && this.isSpecialTabActive() && !ScrollWindow.isWindowActive())
      this.cancelUnitSelectedMover.setVisible(true);
    else
      this.cancelUnitSelectedMover.setVisible(false);
  }

  private void updateHideUiButton()
  {
    if (Config.ui_main_hidden)
      this.unhideButtonMover.setVisible(true);
    else
      this.unhideButtonMover.setVisible(false);
  }

  private void updateSelectedPowerButtons()
  {
    PowerButton selectedButton = this.selectedButton;
    GodPower godPower = Object.op_Inequality((Object) selectedButton, (Object) null) ? selectedButton.godPower : (GodPower) null;
    if (Object.op_Equality((Object) selectedButton, (Object) null))
      this.cancelButtMover.setVisible(false);
    else if (ScrollWindow.isWindowActive())
      this.cancelButtMover.setVisible(false);
    else
      this.cancelButtMover.setVisible(true);
    bool pVisible = MoveCamera.inSpectatorMode();
    if (((Object.op_Equality((Object) selectedButton, (Object) null) ? 1 : (ScrollWindow.isWindowActive() ? 1 : 0)) | (pVisible ? 1 : 0)) != 0)
    {
      this.sizeButtMover.setVisible(false);
      this.sizeButton.hideSizes();
      if (pVisible)
      {
        this.clockButtMover.setVisible(true);
      }
      else
      {
        this.clockButtMover.setVisible(false);
        this.clockButton.hideSizes();
      }
    }
    else
    {
      selectedButton.animate(Time.deltaTime);
      if (godPower.show_tool_sizes)
      {
        this.sizeButtMover.setVisible(true);
      }
      else
      {
        this.sizeButtMover.setVisible(false);
        this.sizeButton.hideSizes();
      }
      if (this.selectedButton.godPower.id == "clock")
      {
        this.clockButtMover.setVisible(true);
      }
      else
      {
        this.clockButtMover.setVisible(false);
        this.clockButton.hideSizes();
      }
    }
    if (CanvasMain.isBottomBarShowing())
    {
      this.toggleBottomElements(true);
      if (this._is_toolbar_temp_showed)
        this.resetToolbarTempShow();
    }
    else if (!this._is_toolbar_temp_showed)
      this.toggleBottomElements(false);
    this.pauseButtonMover.setVisible(pVisible);
    this.spectateUnitMover.setVisible(pVisible);
    this.updateTopButtons();
    int num = pVisible ? 1 : 0;
  }

  private void updateTopButtons()
  {
    if (this.bottomElementsMover.visible)
    {
      this.cancelButton.goDown = false;
      this.cancelButton.goUp = false;
      ((Component) this.cancelButton).gameObject.SetActive(true);
      PremiumElementsChecker.checkElements();
      bool flag = DebugConfig.isOn(DebugOption.DebugButton);
      DebugConfig.instance.debugButton.SetActive(flag);
      ((Component) this.clockButton).GetComponent<CancelButton>().goDown = false;
      this.joy_control_cancel_button.SetActive(false);
    }
    else
    {
      if (ControllableUnit.isControllingUnit())
      {
        ((Component) this.cancelButton).gameObject.SetActive(false);
        PremiumElementsChecker.toggleActive(false);
        DebugConfig.instance.debugButton.SetActive(false);
        this.joy_control_cancel_button.SetActive(true);
        this._joy_control_cancel_button_icon.sprite = ControllableUnit.getControllableUnit().getActorAsset().getSpriteIcon();
      }
      bool flag = MoveCamera.inSpectatorMode();
      this.cancelButton.goDown = flag || ControllableUnit.isControllingUnit() && !Config.joyControls;
      ((Component) this.clockButton).GetComponent<CancelButton>().goDown = flag;
    }
  }

  private void toggleBottomElements(bool pState, bool pNow = false)
  {
    TweenCallback pCompleteCallback = (TweenCallback) null;
    if (!pState)
      pCompleteCallback = (TweenCallback) (() =>
      {
        this.buttons.SetActive(false);
        this.resetToolbarCanvasGroup();
      });
    else
      this.buttons.SetActive(true);
    this.bottomElementsMover.setVisible(pState, pNow, pCompleteCallback);
  }

  public void showBarTemporary()
  {
    if (!Config.game_loaded)
      return;
    if (this._toolbar_hide_routine != null)
      this.StopCoroutine(this._toolbar_hide_routine);
    this._is_toolbar_temp_showed = true;
    this._toolbar_canvas_group.blocksRaycasts = false;
    this._toolbar_canvas_group.alpha = 0.5f;
    this.toggleBottomElements(true);
    this._toolbar_hide_routine = this.StartCoroutine(this.toolbarHideRoutine());
  }

  private IEnumerator toolbarHideRoutine()
  {
    yield return (object) new WaitForSeconds(1f);
    this.toggleBottomElements(false);
    this._is_toolbar_temp_showed = false;
  }

  private void resetToolbarCanvasGroup()
  {
    this._toolbar_canvas_group.blocksRaycasts = true;
    this._toolbar_canvas_group.alpha = 1f;
  }

  private void resetToolbarTempShow()
  {
    this.resetToolbarCanvasGroup();
    this._is_toolbar_temp_showed = false;
    if (this._toolbar_hide_routine == null)
      return;
    this.StopCoroutine(this._toolbar_hide_routine);
  }
}

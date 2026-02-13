// Decompiled with JetBrains decompiler
// Type: CanvasMain
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class CanvasMain : MonoBehaviour
{
  public static CanvasMain instance;
  public static float tooltip_show_timeout;
  public Canvas canvas_ui;
  public Canvas canvas_windows;
  public Canvas canvas_map_names;
  public Canvas canvas_tooltip;
  public Image blocker;
  private ScreenOrientation screenOrientation;
  private CanvasScaler scaler_main_ui;
  private CanvasScaler scaler_windows_ui;
  private CanvasScaler scaler_tooltip;
  private CanvasScaler scaler_map_names;
  public Transform transformWindows;
  private float last_width = -1f;
  private float last_height = -1f;
  private const int REFERENCE_SIZE_X = 285;
  private const int REFERENCE_SIZE_Y = 420;
  private ResizeAction _on_resize;
  private ResizeUIAction _on_resize_ui;

  private void Awake()
  {
    CanvasMain.instance = this;
    this.scaler_main_ui = ((Component) this.canvas_ui).GetComponent<CanvasScaler>();
    this.scaler_windows_ui = ((Component) this.canvas_windows).GetComponent<CanvasScaler>();
    this.scaler_tooltip = ((Component) this.canvas_tooltip).GetComponent<CanvasScaler>();
    this.scaler_map_names = ((Component) this.canvas_map_names).GetComponent<CanvasScaler>();
  }

  public bool setMainUiEnabled(bool pEnabled)
  {
    if (((Behaviour) this.canvas_ui).enabled == pEnabled)
      return false;
    ((Behaviour) this.canvas_ui).enabled = pEnabled;
    return true;
  }

  public float getLastWidth() => this.last_width;

  public float getLastHeight() => this.last_height;

  public void addCallbackResize(ResizeAction pAction) => this._on_resize += pAction;

  public void removeCallbackResize(ResizeAction pAction) => this._on_resize -= pAction;

  public void addCallbackResizeUI(ResizeUIAction pAction) => this._on_resize_ui += pAction;

  public void removeCallbackResizeUI(ResizeUIAction pAction) => this._on_resize_ui -= pAction;

  private void checkResize(float pWidth, float pHeight)
  {
    this.last_width = pWidth;
    this.last_height = pHeight;
    this.screenOrientation = Screen.orientation;
    this.resizeMainUI();
    this.resizeWindowsUI();
    this.resizeTooltipUI();
    ResizeAction onResize = this._on_resize;
    if (onResize == null)
      return;
    onResize(pWidth, pHeight);
  }

  public void resizeWindowsUI()
  {
    this.changeCanvasSize(this.scaler_windows_ui, "ui_size_windows", 285f, 420f);
    float pUISize = (float) PlayerConfig.getIntValue("ui_size_windows") / 100f;
    ResizeUIAction onResizeUi = this._on_resize_ui;
    if (onResizeUi == null)
      return;
    onResizeUi(pUISize);
  }

  public void resizeTooltipUI()
  {
    this.changeCanvasSize(this.scaler_tooltip, "ui_size_tooltips", 285f, 420f);
  }

  public void resizeMapNames()
  {
    this.changeCanvasSize(this.scaler_map_names, "ui_size_map_names", 285f, 420f);
  }

  public void resizeMainUI()
  {
    this.changeCanvasSize(this.scaler_main_ui, "ui_size_main", 285f, Screen.height <= Screen.width ? 500f : 360f);
  }

  private void changeCanvasSize(
    CanvasScaler pScaler,
    string pSizeOption,
    float pReferenceWidth,
    float pReferenceHeight)
  {
    pScaler.uiScaleMode = (CanvasScaler.ScaleMode) 1;
    float num = 2f - (float) PlayerConfig.getIntValue(pSizeOption) / 100f;
    pScaler.referenceResolution = new Vector2(pReferenceWidth, pReferenceHeight * num);
  }

  private void Start() => this.screenOrientation = Screen.orientation;

  private void Update()
  {
    if ((double) CanvasMain.tooltip_show_timeout > 0.0)
      CanvasMain.tooltip_show_timeout -= Time.deltaTime;
    if ((double) Screen.width != (double) this.last_width || (double) Screen.height != (double) this.last_height)
      this.checkResize((float) Screen.width, (float) Screen.height);
    if (this.screenOrientation != Screen.orientation)
    {
      this.screenOrientation = Screen.orientation;
      if (ScrollWindow.isWindowActive())
        ScrollWindow.hideAllEvent();
    }
    if (Config.lockGameControls || Object.op_Inequality((Object) World.world?.stack_effects, (Object) null) && World.world.stack_effects.isLocked())
      ((Component) this.blocker).gameObject.SetActive(true);
    else
      ((Component) this.blocker).gameObject.SetActive(false);
  }

  public static void addTooltipShowTimeout(float pTime)
  {
    CanvasMain.tooltip_show_timeout = pTime;
    Tooltip.hideTooltipNow();
  }

  public static bool isBottomBarShowing()
  {
    return !ScrollWindow.isWindowActive() && !ControllableUnit.isControllingUnit() && !MoveCamera.inSpectatorMode() && !Config.ui_main_hidden && !SmoothLoader.isLoading() && !SaveManager.isLoadingSaveAnimationActive();
  }

  public static bool isNameplatesAllowed()
  {
    return !SmoothLoader.isLoading() && !SaveManager.isLoadingSaveAnimationActive();
  }
}

// Decompiled with JetBrains decompiler
// Type: WindowToolbar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class WindowToolbar : MonoBehaviour, IShakable
{
  private const int REFERENCE_HEIGHT = 1080;
  private const int HIDE_DISTANCE_MIN = 225;
  private const int HIDE_DISTANCE_MAX = 440;
  private const float FOCUS_SCROLL_TOP_ERROR = 0.5f;
  private const float FOCUS_SCROLL_BOTTOM_ERROR = 0.1f;
  private const float FOCUS_SCROLL_BUTTON_ERROR = 0.5f;
  private const float FOCUS_SCROLL_DURATION = 0.3f;
  private static WindowToolbar _instance;
  private UiMover _ui_mover;
  private Transform _content;
  private RectTransform _parent_rect;
  private CanvasGroup _canvas_group;
  [SerializeField]
  private ScrollRectExtended _scroll_rect;
  [SerializeField]
  private RectTransform _content_rect;
  [SerializeField]
  private RectTransform _windows_parent;
  [SerializeField]
  private Transform _selector_base_parent;
  [SerializeField]
  private Transform _selector;
  private float _ui_size_min;
  private float _ui_size_max;
  private Dictionary<string, Transform> _window_buttons = new Dictionary<string, Transform>();
  public bool _last_state;

  public float shake_duration { get; } = 0.5f;

  public float shake_strength { get; } = 8f;

  public Tweener shake_tween { get; set; }

  private void Awake()
  {
    WindowToolbar._instance = this;
    this._ui_mover = ((Component) this).GetComponent<UiMover>();
    this._content = ((Component) this).transform.FindRecursive("Content");
    this._canvas_group = ((Component) this).GetComponent<CanvasGroup>();
    this._parent_rect = ((Component) ((Component) this).transform.parent).GetComponent<RectTransform>();
    ScrollWindow.addCallbackShow((ScrollWindowNameAction) (_ => this.toggleShow(true)));
    ScrollWindow.addCallbackClose((ScrollWindowAction) (() => this.toggleShow(false)));
    OptionAsset optionAsset = AssetManager.options_library.get("ui_size_windows");
    this._ui_size_min = (float) optionAsset.min_value / 100f;
    this._ui_size_max = (float) optionAsset.max_value / 100f;
    this._ui_mover.initPos.y = -6f;
    this._ui_mover.hidePos.y = -6f;
    Vector3 localPosition = ((Component) this).transform.localPosition;
    localPosition.y = -6f;
    ((Component) this).transform.localPosition = localPosition;
    foreach (PowerButton componentsInChild in ((Component) this).GetComponentsInChildren<PowerButton>())
    {
      if (componentsInChild.type == PowerButtonType.Window)
        this._window_buttons.Add(componentsInChild.open_window_id, ((Component) componentsInChild).transform);
    }
    ScrollWindow.addCallbackShow(new ScrollWindowNameAction(this.checkSelectOnWindowShow));
    ScrollWindow.addCallbackHide(new ScrollWindowNameAction(this.checkDeselectOnWindowHide));
  }

  private void Start()
  {
    CanvasMain.instance.addCallbackResize(new ResizeAction(this.onResize));
    CanvasMain.instance.addCallbackResizeUI(new ResizeUIAction(this.checkShow));
  }

  private void OnDisable() => this.killShakeTween();

  private void onResize(float pWidth, float pHeight)
  {
    this.checkShow((float) PlayerConfig.getOptionInt("ui_size_windows") / 100f);
  }

  private void checkShow(float pUISize)
  {
    if (this.isHideDistance(pUISize))
    {
      this.toggleShow(false);
    }
    else
    {
      if (!ScrollWindow.isWindowActive())
        return;
      this.toggleShow(true);
    }
  }

  public static void toggleActive(bool pState)
  {
    WindowToolbar._instance.toggleActiveInstance(pState);
  }

  private void toggleActiveInstance(bool pState)
  {
    if (pState)
    {
      ((Component) this).gameObject.SetActive(true);
      if (ScrollWindow.isWindowActive())
        this.toggleShow(true);
      else
        this._ui_mover.setVisible(false, true);
    }
    else
      this._ui_mover.setVisible(false, pCompleteCallback: (TweenCallback) (() => ((Component) this).gameObject.SetActive(false)));
  }

  private void toggleShow(bool pState)
  {
    if (!((Component) this).gameObject.activeSelf)
      return;
    if (pState)
    {
      if (this.isHideDistance((float) PlayerConfig.getIntValue("ui_size_windows") / 100f))
        pState = false;
      else if (!AssetManager.window_library.get(ScrollWindow.getCurrentWindow().screen_id).window_toolbar_enabled)
        pState = false;
    }
    this._last_state = pState;
    this._canvas_group.blocksRaycasts = pState;
    if (pState)
      ((Component) this._content).gameObject.SetActive(true);
    TweenCallback pCompleteCallback = (TweenCallback) null;
    if (!pState)
      pCompleteCallback = (TweenCallback) (() => ((Component) this._content).gameObject.SetActive(this._last_state));
    this._ui_mover.setVisible(pState, pCompleteCallback: pCompleteCallback);
  }

  public static void shake() => WindowToolbar._instance.shake();

  private bool isPortrait() => false;

  private bool isHideDistance(float pUISize)
  {
    if (!ScrollWindow.isWindowActive())
      return true;
    float num1 = Mathf.InverseLerp(this._ui_size_min, this._ui_size_max, pUISize);
    float num2 = (float) Screen.height / 1080f;
    float num3 = Mathf.Lerp(225f, 440f, num1) * num2;
    Rect rect1 = this._windows_parent.WorldRect();
    double xMin1 = (double) ((Rect) ref rect1).xMin;
    Rect rect2 = this._parent_rect.WorldRect();
    double xMin2 = (double) ((Rect) ref rect2).xMin;
    return xMin1 - xMin2 < (double) num3;
  }

  private void checkSelectOnWindowShow(string pWindowId)
  {
    Transform pButtonTransform;
    if (!this._window_buttons.TryGetValue(pWindowId, out pButtonTransform))
    {
      pWindowId = AssetManager.window_library.get(pWindowId).related_parent_window;
      if (string.IsNullOrEmpty(pWindowId) || !this._window_buttons.TryGetValue(pWindowId, out pButtonTransform))
        return;
    }
    this._selector.SetParent(pButtonTransform);
    this._selector.localPosition = Vector3.zero;
    this._selector.localScale = Vector3.one;
    this.scrollToButton(pButtonTransform);
  }

  private void scrollToButton(Transform pButtonTransform)
  {
    Rect worldRect1 = this._scroll_rect.rectTransform.GetWorldRect();
    float yMax = ((Rect) ref worldRect1).yMax;
    Rect worldRect2 = this._content_rect.GetWorldRect();
    float height = ((Rect) ref worldRect2).height;
    Rect worldRect3 = ((RectTransform) pButtonTransform).GetWorldRect();
    float num1 = pButtonTransform.position.y - ((Rect) ref worldRect2).yMax + yMax;
    Vector2 vector2;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2).\u002Ector(0.0f, ((Rect) ref worldRect3).height * 0.5f);
    if ((!((Rect) ref worldRect1).Contains(Vector2.op_Subtraction(((Rect) ref worldRect3).position, vector2)) ? 0 : (((Rect) ref worldRect1).Contains(Vector2.op_Addition(((Rect) ref worldRect3).max, vector2)) ? 1 : 0)) != 0)
      return;
    float num2 = (num1 - ((Rect) ref worldRect3).height / 0.5f) / height;
    float num3 = 1f - Mathf.InverseLerp(this._ui_size_min, this._ui_size_max, (float) PlayerConfig.getOptionInt("ui_size_windows") / 100f);
    if ((double) num2 > 1.0 - (0.5 - (double) num3))
      num2 = 1f;
    else if ((double) num2 < 0.10000000149011612 + (double) num3)
      num2 = 0.0f;
    // ISSUE: method pointer
    // ISSUE: method pointer
    DOTween.To(new DOGetter<float>((object) this, __methodptr(\u003CscrollToButton\u003Eb__43_0)), new DOSetter<float>((object) this, __methodptr(\u003CscrollToButton\u003Eb__43_1)), num2, 0.3f);
  }

  private void checkDeselectOnWindowHide(string pWindowId)
  {
    this._selector.SetParent(this._selector_base_parent);
  }

  Transform IShakable.get_transform() => ((Component) this).transform;
}

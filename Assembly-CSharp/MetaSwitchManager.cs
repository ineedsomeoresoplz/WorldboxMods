// Decompiled with JetBrains decompiler
// Type: MetaSwitchManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class MetaSwitchManager : MonoBehaviour
{
  private const float POSITION_SHOW = 0.0f;
  private const float POSITION_HIDE = -44f;
  private const float WINDOW_MAX_SIZE_PERCENT = 100f;
  private const float WINDOW_SIZE_PORTRAIT_START = 100f;
  private const float WINDOW_SIZE_PORTRAIT_END = 115f;
  private const float WINDOW_SIZE_PORTRAIT_RATIO_MIN = 1.275f;
  private const float WINDOW_SIZE_PORTRAIT_RATIO_MAX = 1.45f;
  private const float ANIMATION_DURATION = 0.35f;
  [SerializeField]
  private MetaSwitchButton _button_left;
  [SerializeField]
  private MetaSwitchButton _button_right;
  [SerializeField]
  private Text _window_number_current;
  [SerializeField]
  private Text _window_number_total;
  [SerializeField]
  private GameObject _container;
  private StatsWindow _stats_window;
  private MetaTypeAsset _meta_type_asset;
  private ListPool<NanoObject> _list;
  private static MetaSwitchManager _instance;
  private bool _is_switching_enabled;
  private bool _was_just_opened;
  private bool _is_enabled;
  private Tweener _tweener;

  private void Awake()
  {
    MetaSwitchManager._instance = this;
    ScrollWindow.addCallbackOpen((ScrollWindowNameAction) (_ =>
    {
      this._was_just_opened = true;
      this.enable(true);
    }));
    ScrollWindow.addCallbackShow((ScrollWindowNameAction) (_ =>
    {
      if (this._was_just_opened)
        this._was_just_opened = false;
      else
        this.enable(false);
    }));
    ScrollWindow.addCallbackClose((ScrollWindowAction) (() => this.disable()));
    this._button_left.init(MetaSwitchManager.Direction.Left, new SwitchWindowsAction(MetaSwitchManager.switchWindowsWithCheck));
    this._button_right.init(MetaSwitchManager.Direction.Right, new SwitchWindowsAction(MetaSwitchManager.switchWindowsWithCheck));
  }

  private void Start()
  {
    CanvasMain.instance.addCallbackResize((ResizeAction) ((_1, _2) =>
    {
      if (!this._is_enabled)
        this.enable(false, false);
      else
        this.refresh(false, false);
    }));
  }

  private void enable(bool pOpen, bool pCompleteOnDisable = true)
  {
    this._is_enabled = true;
    StatsWindow component = ((Component) ScrollWindow.getCurrentWindow())?.GetComponent<StatsWindow>();
    if (Object.op_Equality((Object) component, (Object) this._stats_window) && Object.op_Inequality((Object) this._stats_window, (Object) null))
      this.updateShowingData();
    else if (Object.op_Equality((Object) component, (Object) null))
    {
      this.disable(!pOpen);
    }
    else
    {
      this._stats_window = component;
      this._meta_type_asset = AssetManager.meta_type_library.getAsset(this._stats_window.meta_type);
      this.refresh(pCompleteOnDisable: pCompleteOnDisable);
    }
  }

  private void disable(bool pAnimated = true, bool pCompleteTween = true)
  {
    this._is_enabled = false;
    if (pAnimated)
      this.toggleControlsPosition(false, pCompleteTween);
    else
      this.toggleControls(false);
    this._stats_window = (StatsWindow) null;
    this._list?.Dispose();
    this._list = (ListPool<NanoObject>) null;
  }

  public static void checkAndRefresh() => MetaSwitchManager._instance.checkRefresh();

  public static void refresh() => MetaSwitchManager._instance.refresh(true, true);

  public static void refreshWithoutComplete() => MetaSwitchManager._instance.refresh(false);

  private void checkRefresh()
  {
    if (!this._is_enabled)
      return;
    this.refresh(false);
  }

  internal void refresh(bool pCompleteTween = true, bool pCompleteOnDisable = true)
  {
    int optionInt = PlayerConfig.getOptionInt("ui_size_windows");
    if ((double) optionInt > 100.0)
    {
      float num = (float) Screen.width / (float) Screen.height * Mathf.Lerp(1.275f, 1.45f, 1f - Mathf.InverseLerp(100f, 115f, (float) optionInt));
      if ((double) optionInt * (double) num > 100.0)
      {
        this.disable(pCompleteTween: pCompleteOnDisable);
        return;
      }
    }
    this._list?.Dispose();
    this._list = this._meta_type_asset.getSortedList();
    bool pState = this._list.Count >= 2;
    this._is_switching_enabled = pState;
    this.toggleControlsPosition(pState, pCompleteTween);
    if (!pState)
      return;
    this.updateShowingData();
  }

  private static void switchWindowsWithCheck(MetaSwitchManager.Direction pDirection)
  {
    if (!ScrollWindow.isWindowActive() || ScrollWindow.isAnimationActive())
      return;
    MetaSwitchManager.switchWindows(pDirection);
  }

  public static void switchWindows(MetaSwitchManager.Direction pDirection)
  {
    MetaSwitchManager._instance.switchWindow(pDirection);
  }

  private int getCurrentMetaIndex()
  {
    NanoObject nanoObject = this._meta_type_asset.get_selected();
    int currentMetaIndex = this._list.IndexOf(nanoObject);
    if (currentMetaIndex == -1)
    {
      this._list.Add(nanoObject);
      currentMetaIndex = this._list.IndexOf(nanoObject);
    }
    return currentMetaIndex;
  }

  private void switchWindow(MetaSwitchManager.Direction pDirection)
  {
    if (!this._is_switching_enabled || Object.op_Equality((Object) this._stats_window, (Object) null) || this._list.Count < 2)
      return;
    this._meta_type_asset.set_selected(this.getElement(pDirection));
    WindowHistory.popHistory();
    ScrollWindow.showWindow(this._meta_type_asset.window_name);
    this.updateShowingData();
  }

  private void updateShowingData()
  {
    this.updateWindowNumber();
    this.showBannersAndNames();
  }

  private void updateWindowNumber()
  {
    if (this._list == null)
    {
      this._window_number_current.text = "";
      this._window_number_total.text = "";
    }
    else
    {
      int num = this.getCurrentMetaIndex() + 1;
      int count = this._list.Count;
      this._window_number_current.text = $"{num}";
      this._window_number_total.text = $"{count}";
    }
  }

  private void showBannersAndNames()
  {
    this.clear();
    this.showBanner(this.getIndex(MetaSwitchManager.Direction.Left), this._button_left);
    this.showBanner(this.getIndex(MetaSwitchManager.Direction.Right), this._button_right);
  }

  private IBanner showBanner(int pIndex, MetaSwitchButton pButton)
  {
    NanoObject pObject = this._list[pIndex];
    IBanner next = pButton.getPool().getNext(pObject);
    next.load(pObject);
    Button button;
    if (next.gameObject.TryGetComponent<Button>(ref button))
      ((Behaviour) button).enabled = false;
    pButton.setBanner(next);
    Transform transform = next.transform;
    Transform parent = transform.parent;
    parent.localPosition = Vector3.zero;
    parent.localScale = Vector3.one;
    transform.localPosition = Vector3.zero;
    ColorAsset color = pObject.getColor();
    if (color != null)
    {
      string colorText = color.color_text;
      pButton.meta_name.text = pObject.name.ColorHex(colorText);
    }
    return next;
  }

  private void toggleControlsPosition(bool pState, bool pCompleteTween = true)
  {
    TweenExtensions.Kill((Tween) this._tweener, pCompleteTween);
    float num = pState ? 0.0f : -44f;
    if (pState)
      this.toggleControls(true);
    if (Mathf.Approximately(((Component) this).transform.localPosition.y, num))
      return;
    this._tweener = (Tweener) TweenSettingsExtensions.OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOLocalMoveY(((Component) this).transform, num, 0.35f, false), (Ease) 10), (TweenCallback) (() =>
    {
      if (!pState)
        this.toggleControls(false);
      this.checkRefresh();
    }));
  }

  private void toggleControls(bool pState) => this._container.SetActive(pState);

  private void clear()
  {
    this._button_left.clear();
    this._button_right.clear();
  }

  private NanoObject getElement(MetaSwitchManager.Direction pDirection)
  {
    return this._list[this.getIndex(pDirection)];
  }

  private int getIndex(MetaSwitchManager.Direction pDirection)
  {
    int currentMetaIndex = this.getCurrentMetaIndex();
    return Toolbox.loopIndex(pDirection == MetaSwitchManager.Direction.Left ? currentMetaIndex - 1 : currentMetaIndex + 1, this._list.Count);
  }

  public static bool isAnimationActive()
  {
    return TweenExtensions.IsActive((Tween) MetaSwitchManager._instance._tweener);
  }

  public static bool isSwitcherEnabled() => MetaSwitchManager._instance._is_enabled;

  public static MetaSwitchButton getLeftbutton() => MetaSwitchManager._instance._button_left;

  public static MetaSwitchButton getRightButton() => MetaSwitchManager._instance._button_right;

  public enum Direction
  {
    Left,
    Right,
  }
}

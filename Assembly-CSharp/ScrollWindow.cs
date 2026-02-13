// Decompiled with JetBrains decompiler
// Type: ScrollWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class ScrollWindow : MonoBehaviour, IShakable
{
  public const int WINDOW_POSITION_Y = -6;
  private const float SCROLLBAR_POSITION_SHOW = 0.0f;
  private const float SCROLLBAR_POSITION_HIDE = -17.2f;
  private const float SCROLLBAR_ANIMATION_DURATION = 0.35f;
  private const float SCROLLBAR_ANIMATION_DURATION_COLOR = 0.35f;
  private const float SCROLLBAR_ANIMATION_DELAY = 0.25f;
  private const float SCROLLBAR_ANIMATION_DELAY_COLOR = 0.125f;
  private const string SCROLLBAR_ACTIVE_COLOR = "#E75340";
  private const string SCROLLBAR_INACTIVE_COLOR = "#545454";
  private static ScrollWindowNameAction _open_callback;
  private static ScrollWindowNameAction _show_started_callback;
  private static ScrollWindowNameAction _show_callback;
  private static ScrollWindowNameAction _show_finished_callback;
  private static ScrollWindowNameAction _hide_callback;
  private static ScrollWindowAction _close_callback;
  private static ScrollWindow _current_window = (ScrollWindow) null;
  private static Dictionary<string, ScrollWindow> _all_windows = new Dictionary<string, ScrollWindow>();
  private static bool _is_window_active = false;
  private static bool _is_any_window_active = false;
  public string screen_id = "screen";
  public bool unselectPower = true;
  private Canvas _canvas;
  private CanvasGroup _canvas_group;
  private RectTransform _bg_rect;
  public Text titleText;
  [SerializeField]
  private GameObject _back_button_container;
  [SerializeField]
  private Button _back_button;
  public Image previous_window_icon;
  private static string _queued_window = "";
  public ScrollRect scrollRect;
  public Image scrollingGradient;
  public RectTransform transform_content;
  public RectTransform transform_viewport;
  public RectTransform transform_scrollRect;
  public GameObject[] destroyOnAwake;
  public bool force_gradient;
  public bool historyActionEnabled = true;
  private static bool _should_clear;
  public List<Sprite> close_sprites;
  public Image close_background;
  private int _current_background_sprite_index;
  [SerializeField]
  private TipButton _close_button_tip;
  public WindowMetaTabButtonsContainer tabs;
  public static bool skip_worldtip_hide;
  private static List<Tweener> _animations_list = new List<Tweener>();
  private Tweener _animation_tween;
  private WindowAsset _asset;
  private bool _scrollbar_cached_state;
  private Tweener _scrollbar_tweener;
  private Tweener _scrollbar_tweener_color;
  private Coroutine _scrollbar_routine;
  private bool _initialized;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool isWindowActive() => ScrollWindow._is_window_active;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool isAnimationActive() => ScrollWindow._animations_list.Count > 0;

  public float shake_duration { get; } = 0.5f;

  public float shake_strength { get; } = 8f;

  public Tweener shake_tween { get; set; }

  private void Awake() => this.init();

  public void init()
  {
    if (this._initialized)
      return;
    this._asset = AssetManager.window_library.get(this.screen_id);
    this._canvas_group = ((Component) this).gameObject.GetComponent<CanvasGroup>();
    this._bg_rect = ((Component) ((Component) this).gameObject.transform.GetChild(0)).GetComponent<RectTransform>();
    if (this.destroyOnAwake != null)
    {
      foreach (Object @object in this.destroyOnAwake)
        Object.Destroy(@object);
    }
    this.initComponents();
  }

  private void OnEnable() => WorldTip.hideNow();

  private void Start()
  {
    if (Object.op_Equality((Object) this._canvas, (Object) null))
      this.create(true);
    this.toggleScrollbar(false);
  }

  private void Update()
  {
    this.updateScrollbar();
    this.updateRightClickBack();
  }

  private void updateRightClickBack()
  {
    if (!InputHelpers.GetMouseButtonDown(1) || !this.canGoBackWithRightClick())
      return;
    WindowHistory.clickBack();
  }

  private bool canGoBackWithRightClick()
  {
    return this.historyActionEnabled && !Config.isDraggingItem() && InputHelpers.mouseSupported && !World.world.isOverUiButton();
  }

  private void updateScrollbar()
  {
    Scrollbar verticalScrollbar = this.scrollRect.verticalScrollbar;
    bool flag = !Mathf.Approximately(verticalScrollbar.size, 1f);
    if (this._scrollbar_cached_state == flag)
      return;
    this._scrollbar_cached_state = flag;
    this.toggleScrollbar(true);
    this.checkGradient();
    TweenExtensions.Kill((Tween) this._scrollbar_tweener_color, false);
    Color color = !this._scrollbar_cached_state ? Toolbox.makeColor("#545454") : Toolbox.makeColor("#E75340");
    this._scrollbar_tweener_color = (Tweener) DOTweenModuleUI.DOColor(((Selectable) verticalScrollbar).image, color, 0.35f);
    if (!this._scrollbar_cached_state)
      TweenSettingsExtensions.SetDelay<Tweener>(this._scrollbar_tweener_color, 0.125f);
    if (this._scrollbar_routine != null)
      this.StopCoroutine(this._scrollbar_routine);
    this._scrollbar_routine = this.StartCoroutine(this.toggleScrollbarRoutine());
  }

  private IEnumerator toggleScrollbarRoutine()
  {
    yield return (object) new WaitForSecondsRealtime(0.25f);
    if ((double) this.scrollRect.verticalScrollbar.size >= 1.0)
      this.toggleScrollbar(false);
  }

  private void toggleScrollbar(bool pState)
  {
    Scrollbar verticalScrollbar = this.scrollRect.verticalScrollbar;
    TweenExtensions.Kill((Tween) this._scrollbar_tweener, false);
    float num = pState ? 0.0f : -17.2f;
    this._scrollbar_tweener = (Tweener) TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOLocalMoveX(((Component) verticalScrollbar).transform, num, 0.35f, false), (Ease) 10);
  }

  public void resetScroll()
  {
    Vector3 localPosition = ((Transform) this.transform_content).localPosition;
    localPosition.y = 0.0f;
    ((Transform) this.transform_content).localPosition = localPosition;
  }

  internal void create(bool pHide = false)
  {
    this._canvas = CanvasMain.instance.canvas_windows;
    if (this.historyActionEnabled)
    {
      // ISSUE: method pointer
      ((UnityEvent) this._back_button.onClick).AddListener(new UnityAction((object) null, __methodptr(clickBack)));
    }
    else
      this._back_button_container.gameObject.SetActive(false);
    if (pHide)
    {
      this.hide(pPlaySound: false);
      this.finishTween();
    }
    this.checkGradient();
  }

  private void checkGradient()
  {
    if (this.force_gradient || this._scrollbar_cached_state)
      ((Component) this.scrollingGradient).gameObject.SetActive(true);
    else
      ((Component) this.scrollingGradient).gameObject.SetActive(false);
  }

  public void clickBack() => WindowHistory.clickBack();

  private static void setCurrentWindow(ScrollWindow pWindow)
  {
    if (Object.op_Equality((Object) pWindow, (Object) null))
      return;
    ScrollWindow._current_window = pWindow;
    ScrollWindow._is_window_active = true;
    if (!ScrollWindow._is_any_window_active)
    {
      ScrollWindowNameAction openCallback = ScrollWindow._open_callback;
      if (openCallback != null)
        openCallback(pWindow.screen_id);
    }
    ScrollWindow._is_any_window_active = true;
    ScrollWindowNameAction showCallback = ScrollWindow._show_callback;
    if (showCallback == null)
      return;
    showCallback(pWindow.screen_id);
  }

  private static void clearCurrentWindow(ScrollWindow pWindow)
  {
    if (Object.op_Equality((Object) ScrollWindow._current_window, (Object) null))
      return;
    ScrollWindowNameAction hideCallback = ScrollWindow._hide_callback;
    if (hideCallback != null)
      hideCallback(ScrollWindow._current_window.screen_id);
    ScrollWindow._current_window = (ScrollWindow) null;
    ScrollWindow._is_window_active = false;
    Config.debug_window_stats.setCurrent((string) null);
  }

  public static void queueWindow(string pWindowID) => ScrollWindow._queued_window = pWindowID;

  public static void clearQueue()
  {
    if (!(ScrollWindow._queued_window != ""))
      return;
    string queuedWindow = ScrollWindow._queued_window;
    ScrollWindow._queued_window = "";
    ScrollWindow.hideAllEvent(false);
    ScrollWindow.showWindow(queuedWindow);
  }

  public static void showWindow(string pWindowID)
  {
    ScrollWindow.showWindow(pWindowID, false, false);
  }

  public static void showWindow(string pWindowID, bool pSkipAnimation = false, bool pBlockSame = false)
  {
    World.world.selected_buttons.clearHighlightedButton();
    if (ScrollWindow.isAnimationActive())
      return;
    bool pJustCreated = ScrollWindow.checkWindowExist(pWindowID);
    ScrollWindow allWindow = ScrollWindow._all_windows[pWindowID];
    if (pBlockSame && pWindowID == ScrollWindow._current_window.screen_id)
    {
      allWindow.shake();
      WindowToolbar.shake();
      ScrollWindow.randomDropHoveringIcon(3, 6);
    }
    else
      allWindow.clickShow(pSkipAnimation, pJustCreated);
  }

  public static void randomDropHoveringIcon(int pMin, int pMax)
  {
    int num = Randy.randomInt(pMin, pMax);
    for (int index = 0; index < num; ++index)
      HoveringBgIconManager.randomDrop();
  }

  public static string checkWindowID(string pWindowID)
  {
    return pWindowID.StartsWith("worldnet", StringComparison.Ordinal) ? "not_found" : pWindowID;
  }

  public static bool windowLoaded(string pWindowID)
  {
    string key = ScrollWindow.checkWindowID(pWindowID);
    return ScrollWindow._all_windows.ContainsKey(key);
  }

  public static bool checkWindowExist(string pWindowID)
  {
    string str1 = ScrollWindow.checkWindowID(pWindowID);
    bool flag = false;
    if (!ScrollWindow._all_windows.ContainsKey(pWindowID))
    {
      flag = true;
      string str2 = "windows/" + str1;
      ScrollWindow tScrollWindow;
      if (!WindowPreloader.TryGetPreloadedWindow(pWindowID, out tScrollWindow))
      {
        ScrollWindow pPrefab = (ScrollWindow) Resources.Load(str2, typeof (ScrollWindow));
        if (Object.op_Equality((Object) pPrefab, (Object) null))
        {
          Debug.LogError((object) $"Window with id {str1} not found!");
          pPrefab = (ScrollWindow) Resources.Load("windows/not_found", typeof (ScrollWindow));
        }
        ListPool<GameObject> pTabsObjects = ScrollWindow.disableTabsInPrefab(pPrefab);
        tScrollWindow = Object.Instantiate<ScrollWindow>(pPrefab, CanvasMain.instance.transformWindows);
        ScrollWindow.enableTabsInPrefab(pTabsObjects);
      }
      if (!ScrollWindow._all_windows.ContainsKey(pWindowID))
        ScrollWindow._all_windows.Add(pWindowID, tScrollWindow);
      tScrollWindow.screen_id = pWindowID;
      ((Object) tScrollWindow).name = pWindowID;
      tScrollWindow.create();
    }
    return flag;
  }

  public static ListPool<GameObject> disableTabsInPrefab(ScrollWindow pPrefab)
  {
    ListPool<GameObject> listPool = new ListPool<GameObject>();
    if (Object.op_Inequality((Object) pPrefab.tabs, (Object) null))
    {
      foreach (WindowMetaTab windowMetaTab in ((Component) pPrefab.tabs).transform.FindAllRecursive<WindowMetaTab>())
      {
        foreach (Transform tabElement in windowMetaTab.tab_elements)
        {
          if (!Object.op_Equality((Object) tabElement, (Object) null) && ((Component) tabElement).gameObject.activeSelf)
          {
            ((Component) tabElement).gameObject.SetActive(false);
            listPool.Add(((Component) tabElement).gameObject);
          }
        }
      }
    }
    return listPool;
  }

  public static void enableTabsInPrefab(ListPool<GameObject> pTabsObjects)
  {
    // ISSUE: unable to decompile the method.
  }

  public static ScrollWindow get(string pWindowID)
  {
    ScrollWindow.checkWindowExist(pWindowID);
    return ScrollWindow._all_windows[pWindowID];
  }

  public void clickShow(bool pSkipAnimation = false, bool pJustCreated = false)
  {
    if (ScrollWindow.isAnimationActive())
      return;
    LogText.log("Window Opened", this.screen_id);
    if (Object.op_Equality((Object) ScrollWindow._current_window, (Object) this))
    {
      this.showSameWindow();
    }
    else
    {
      ScrollWindow.moveAllToLeftAndRemove();
      this.show(pSkipAnimation: pSkipAnimation, pJustCreated: pJustCreated);
    }
  }

  public void clickShowLeft()
  {
    if (ScrollWindow.isAnimationActive())
      return;
    LogText.log("Window Opened", this.screen_id);
    if (Object.op_Equality((Object) ScrollWindow._current_window, (Object) this))
    {
      this.showSameWindow();
    }
    else
    {
      ScrollWindow.moveAllToRightAndRemove();
      this.show("left", "left");
    }
  }

  public void forceShow() => this.show(pSkipAnimation: true);

  public void show(
    string pDistPosition = "right",
    string pStartPosition = "right",
    bool pSkipAnimation = false,
    bool pJustCreated = false)
  {
    this.setActive(true, pDistPosition, pStartPosition, pSkipAnimation, pJustCreated);
    CanvasMain.addTooltipShowTimeout(0.01f);
    if (this.screen_id == "PremiumPurchaseError")
      Analytics.LogEvent("purchase_premium_error");
    Analytics.trackWindow(this.screen_id);
    this.historyAction();
    PowerTracker.trackWindow(this.screen_id, this);
    MusicBox.playSoundUI("event:/SFX/UI/WindowWhoosh");
  }

  private void historyAction()
  {
    if (!this.historyActionEnabled)
      return;
    if (WindowHistory.hasHistory())
    {
      this._back_button_container.SetActive(true);
      this.previous_window_icon.sprite = WindowHistory.list.Last<WindowHistoryData>().window._asset.getSprite();
      this._close_button_tip.text_description_2 = "";
    }
    else
    {
      this._back_button_container.SetActive(false);
      this._close_button_tip.text_description_2 = "hotkey_cancel";
    }
    WindowHistory.addIntoHistory(this);
  }

  public static void moveAllToLeftAndRemove(bool pWithAnimation = true)
  {
    if (Object.op_Inequality((Object) ScrollWindow._current_window, (Object) null))
    {
      if (pWithAnimation)
        ScrollWindow._current_window.moveToLeft(true);
      else
        ScrollWindow._current_window.activeToFalse();
      ScrollWindowNameAction hideCallback = ScrollWindow._hide_callback;
      if (hideCallback != null)
        hideCallback(ScrollWindow._current_window.screen_id);
      ScrollWindow._current_window = (ScrollWindow) null;
    }
    ScrollWindow._is_window_active = false;
    Config.debug_window_stats.setCurrent((string) null);
  }

  public static bool isCurrentWindow(string pWindowID)
  {
    return !Object.op_Equality((Object) ScrollWindow._current_window, (Object) null) && ScrollWindow._current_window.screen_id == pWindowID;
  }

  public static ScrollWindow getCurrentWindow()
  {
    return !ScrollWindow.isWindowActive() ? (ScrollWindow) null : ScrollWindow._current_window;
  }

  public static void setPreviousWindowSprite(Sprite pSprite)
  {
    if (!ScrollWindow.isWindowActive())
      return;
    ScrollWindow.getCurrentWindow().previous_window_icon.sprite = pSprite;
  }

  public static void moveAllToRightAndRemove(bool pWithAnimation = true)
  {
    if (Object.op_Inequality((Object) ScrollWindow._current_window, (Object) null))
    {
      if (pWithAnimation)
        ScrollWindow._current_window.moveToRight(true);
      else
        ScrollWindow._current_window.activeToFalse();
      ScrollWindowNameAction hideCallback = ScrollWindow._hide_callback;
      if (hideCallback != null)
        hideCallback(ScrollWindow._current_window.screen_id);
      ScrollWindow._current_window = (ScrollWindow) null;
    }
    ScrollWindow._is_window_active = false;
    Config.debug_window_stats.setCurrent((string) null);
  }

  public void moveToLeft(bool pRemove = false)
  {
    ScrollWindow.setCanvasGroupEnabled(this._canvas_group, false);
    if (!pRemove)
      return;
    this.moveTween(((Component) this).transform.localPosition.x + this.getHidePosLeft(), new TweenCallback(this.activeToFalse));
  }

  public void moveToRight(bool pRemove = false)
  {
    ScrollWindow.setCanvasGroupEnabled(this._canvas_group, false);
    if (!pRemove)
      return;
    this.moveTween(((Component) this).transform.localPosition.x - this.getHidePosLeft(), new TweenCallback(this.activeToFalse));
  }

  public static void setCanvasGroupEnabled(CanvasGroup pGroup, bool pValue)
  {
    pGroup.interactable = pValue;
    pGroup.blocksRaycasts = pValue;
  }

  public void showSameWindow()
  {
    this.sameWindowTween(pCompleteCallback: new TweenCallback(this.finishTween));
    this.historyAction();
    ScrollWindow.clearCurrentWindow(this);
    ScrollWindowNameAction showStartedCallback = ScrollWindow._show_started_callback;
    if (showStartedCallback != null)
      showStartedCallback(this.screen_id);
    ((Component) this).gameObject.SetActive(false);
    ((Component) this).gameObject.SetActive(true);
    ScrollWindow.setCurrentWindow(this);
    Tooltip.hideTooltipNow();
    this.resetScroll();
  }

  public void setActive(
    bool pActive,
    string pDistPosition = "right",
    string pStartPosition = "right",
    bool pSkipAnimation = false,
    bool pJustCreated = false)
  {
    if (ScrollWindow.skip_worldtip_hide)
      ScrollWindow.skip_worldtip_hide = false;
    else
      WorldTip.hideNow();
    Tooltip.hideTooltipNow();
    if (this.unselectPower && Object.op_Inequality((Object) World.world.selected_buttons.selectedButton, (Object) null) && World.world.selected_buttons.selectedButton.godPower.unselect_when_window)
      World.world.selected_buttons.unselectAll();
    if (pActive)
    {
      ScrollWindow.setCanvasGroupEnabled(this._canvas_group, true);
      ScrollWindowNameAction showStartedCallback = ScrollWindow._show_started_callback;
      if (showStartedCallback != null)
        showStartedCallback(this.screen_id);
      ((Component) this).gameObject.SetActive(true);
      ScrollWindow.setCurrentWindow(this);
      this.resetScroll();
      switch (pStartPosition)
      {
        case "right":
          ((Component) this).transform.localPosition = new Vector3(this.getHidePosRight(), -6f, ((Component) this).transform.localPosition.z);
          break;
        case "left":
          ((Component) this).transform.localPosition = new Vector3(this.getHidePosLeft(), -6f, ((Component) this).transform.localPosition.z);
          break;
      }
      if (pSkipAnimation)
      {
        this.finishTween();
        ((Component) this).transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
      }
      else
        this.moveTween(pCompleteCallback: new TweenCallback(this.finishTween), pJustCreated: pJustCreated);
    }
    else
    {
      ScrollWindow.clearCurrentWindow(this);
      if (pDistPosition == "left")
        this.moveTween(this.getHidePosLeft(), new TweenCallback(this.activeToFalse), pJustCreated);
      else
        this.moveTween(this.getHidePosRight(), new TweenCallback(this.activeToFalse), pJustCreated);
    }
  }

  protected void moveTween(float pToX = 0.0f, TweenCallback pCompleteCallback = null, bool pJustCreated = false)
  {
    float num1 = 0.35f;
    Ease ease = (Ease) 27;
    if (pCompleteCallback == null)
      pCompleteCallback = new TweenCallback(this.finishTween);
    if (pCompleteCallback == new TweenCallback(this.activeToFalse))
    {
      num1 = 0.1f;
      ease = (Ease) 1;
    }
    Vector3 vector3;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3).\u002Ector(pToX, -6f, ((Component) this).transform.localPosition.z);
    TweenExtensions.Kill((Tween) this._animation_tween, true);
    float num2 = 0.02f;
    if (pJustCreated)
      num2 = 0.1f;
    this._animation_tween = (Tweener) TweenSettingsExtensions.OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOLocalMove(((Component) this).transform, vector3, num1, false), num2), ease), (TweenCallback) (() =>
    {
      pCompleteCallback();
      ScrollWindow._animations_list.Remove(this._animation_tween);
    }));
    ScrollWindow._animations_list.Add(this._animation_tween);
  }

  protected void sameWindowTween(float pToX = 0.0f, TweenCallback pCompleteCallback = null)
  {
    float num = 0.09f;
    if (pCompleteCallback == null)
      pCompleteCallback = new TweenCallback(this.finishTween);
    ((Component) this).transform.localPosition = new Vector3(0.0f, -4f, 0.0f);
    Vector3 vector3;
    // ISSUE: explicit constructor call
    ((Vector3) ref vector3).\u002Ector(pToX, -6f, ((Component) this).gameObject.transform.localPosition.z);
    TweenExtensions.Kill((Tween) this._animation_tween, true);
    this._animation_tween = (Tweener) TweenSettingsExtensions.OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOLocalMove(((Component) this).transform, vector3, num, false), (Ease) 4), (TweenCallback) (() =>
    {
      pCompleteCallback();
      ScrollWindow._animations_list.Remove(this._animation_tween);
    }));
    ScrollWindow._animations_list.Add(this._animation_tween);
  }

  public static void hideAllEvent(bool pWithAnimation = true)
  {
    if (ScrollWindow.isWindowActive())
    {
      ScrollWindow._should_clear = true;
      ScrollWindowAction closeCallback = ScrollWindow._close_callback;
      if (closeCallback != null)
        closeCallback();
      ScrollWindow.moveAllToLeftAndRemove(pWithAnimation);
    }
    Tooltip.hideTooltipNow();
    World.world.player_control.controls_lock_timer = 0.1f;
    PowerTracker.trackWatching();
  }

  public void clickCloseButton(string pDirection = "right")
  {
    this.clickHide(pDirection);
    ++this._current_background_sprite_index;
    if (this._current_background_sprite_index >= this.close_sprites.Count)
      this._current_background_sprite_index = this.close_sprites.Count - 1;
    this.close_background.sprite = this.close_sprites[this._current_background_sprite_index];
  }

  public void clickHide(string pDirection = "right")
  {
    if (!ScrollWindow.canClickHide())
      return;
    this.hide(pDirection);
    ((Component) World.world.selected_buttons).gameObject.SetActive(true);
    ScrollWindow._should_clear = true;
    ScrollWindowAction closeCallback = ScrollWindow._close_callback;
    if (closeCallback != null)
      closeCallback();
    World.world.player_control.controls_lock_timer = 0.3f;
    PowerTracker.trackWatching();
  }

  internal static void checkElements()
  {
    if (!ScrollWindow.isWindowActive())
      return;
    ScrollWindow currentWindow = ScrollWindow.getCurrentWindow();
    bool flag1 = currentWindow.shouldClose();
    bool flag2 = false;
    if (!flag1)
      flag2 = currentWindow.shouldRefresh();
    if (flag1 | flag2)
    {
      TabbedWindow tabbedWindow;
      if (((Component) currentWindow).TryGetComponent<TabbedWindow>(ref tabbedWindow))
        tabbedWindow.StopAllCoroutines();
      foreach (MonoBehaviour componentsInChild in ((Component) currentWindow).GetComponentsInChildren<WindowMetaElementBase>(false))
        componentsInChild.StopAllCoroutines();
    }
    if (flag1)
    {
      ScrollWindow.hideAllEvent(false);
    }
    else
    {
      if (!flag2)
        return;
      WindowHistory.popHistory();
      currentWindow.showSameWindow();
    }
  }

  public bool shouldClose()
  {
    TabbedWindow tabbedWindow;
    return ((Component) this).TryGetComponent<TabbedWindow>(ref tabbedWindow) && tabbedWindow.checkCancelWindow();
  }

  public bool shouldRefresh()
  {
    foreach (IShouldRefreshWindow componentsInChild in ((Component) this).GetComponentsInChildren<IShouldRefreshWindow>(false))
    {
      if (componentsInChild.checkRefreshWindow())
        return true;
    }
    return false;
  }

  private void OnDisable()
  {
    if (ScrollWindow._should_clear)
    {
      ScrollWindow._should_clear = false;
      WindowHistory.clear();
      ScrollWindow.clear();
      ScrollWindow._is_any_window_active = false;
      Config.debug_window_stats.setCurrent((string) null);
    }
    this.killShakeTween();
  }

  internal static void clear()
  {
    foreach (MetaTypeAsset metaTypeAsset in AssetManager.meta_type_library.list)
    {
      MetaTypeAction windowActionClear = metaTypeAsset.window_action_clear;
      if (windowActionClear != null)
        windowActionClear();
    }
    NanoObject selectedNanoObject = SelectedObjects.getSelectedNanoObject();
    if (selectedNanoObject.isRekt())
      return;
    AssetManager.meta_type_library.getAsset(selectedNanoObject.getMetaType()).selectAndInspect(selectedNanoObject, pClearAction: true);
  }

  public static bool canClickHide() => !WorkshopUploadingWorldWindow.uploading;

  public void hide(string pDirection = "right", bool pPlaySound = true)
  {
    LogText.log("Window Hide", this.screen_id);
    this.setActive(false, pDirection);
    CanvasMain.addTooltipShowTimeout(0.01f);
    ScrollWindow.setCanvasGroupEnabled(this._canvas_group, false);
    if (pPlaySound)
      MusicBox.playSoundUI("event:/SFX/UI/WindowClose");
    Analytics.hideWindow();
  }

  public static void finishAnimations()
  {
    // ISSUE: unable to decompile the method.
  }

  public void finishTween()
  {
    ScrollWindowNameAction finishedCallback = ScrollWindow._show_finished_callback;
    if (finishedCallback == null)
      return;
    finishedCallback(this.screen_id);
  }

  public void activeToFalse() => ((Component) this).gameObject.SetActive(false);

  public float getHidePosRight()
  {
    return (float) ((double) ((Component) this._canvas).GetComponent<RectTransform>().sizeDelta.x / 2.0 + (double) this._bg_rect.sizeDelta.x / 2.0 + (double) this._bg_rect.sizeDelta.x * 0.20000000298023224);
  }

  public float getHidePosLeft() => this.getHidePosRight() * -1f;

  public float getHidePosLeftHalf()
  {
    float num = (float) (((double) ((Component) this._canvas).GetComponent<RectTransform>().sizeDelta.x - (double) this._bg_rect.sizeDelta.x) / 2.0);
    return this.getHidePosLeft() + num / 2f;
  }

  public float getDistBetweenWindows() => this.getHidePosLeftHalf();

  public void openConsole() => World.world.console.Show();

  public static void addCallbackOpen(ScrollWindowNameAction pAction)
  {
    ScrollWindow._open_callback += pAction;
  }

  public static void removeCallbackOpen(ScrollWindowNameAction pAction)
  {
    ScrollWindow._open_callback -= pAction;
  }

  public static void addCallbackShowStarted(ScrollWindowNameAction pAction)
  {
    ScrollWindow._show_started_callback += pAction;
  }

  public static void removeCallbackShowStarted(ScrollWindowNameAction pAction)
  {
    ScrollWindow._show_started_callback -= pAction;
  }

  public static void addCallbackShow(ScrollWindowNameAction pAction)
  {
    ScrollWindow._show_callback += pAction;
  }

  public static void removeCallbackShow(ScrollWindowNameAction pAction)
  {
    ScrollWindow._show_callback -= pAction;
  }

  public static void addCallbackShowFinished(ScrollWindowNameAction pAction)
  {
    ScrollWindow._show_finished_callback += pAction;
  }

  public static void removeCallbackShowFinished(ScrollWindowNameAction pAction)
  {
    ScrollWindow._show_finished_callback -= pAction;
  }

  public static void addCallbackHide(ScrollWindowNameAction pAction)
  {
    ScrollWindow._hide_callback += pAction;
  }

  public static void removeCallbackHide(ScrollWindowNameAction pAction)
  {
    ScrollWindow._hide_callback -= pAction;
  }

  public static void addCallbackClose(ScrollWindowAction pAction)
  {
    ScrollWindow._close_callback += pAction;
  }

  public static void removeCallbackClose(ScrollWindowAction pAction)
  {
    ScrollWindow._close_callback -= pAction;
  }

  private void initComponents()
  {
  }

  public static void debug(DebugTool pTool)
  {
    if (ScrollWindow.isWindowActive())
    {
      pTool.setText("currentWindow:", (object) ScrollWindow.getCurrentWindow().screen_id);
      pTool.setText("historyActionEnabled:", (object) ScrollWindow.getCurrentWindow().historyActionEnabled);
    }
    pTool.setText("_is_window_active:", (object) ScrollWindow._is_window_active);
    pTool.setText("_is_any_window_active:", (object) ScrollWindow._is_any_window_active);
    pTool.setText("isAnimationActive:", (object) ScrollWindow.isAnimationActive());
    pTool.setText("queuedWindow:", (object) ScrollWindow._queued_window);
  }

  Transform IShakable.get_transform() => ((Component) this).transform;
}

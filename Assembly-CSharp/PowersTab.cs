// Decompiled with JetBrains decompiler
// Type: PowersTab
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class PowersTab : MonoBehaviour
{
  private const float END_SPACE = 5f;
  private static PowersTab _current_tab;
  private static Button _current_tab_button;
  private static PowersTab _main_tab;
  private GameObject parentObj;
  public Sprite image_normal;
  public Sprite image_selected;
  public Button powerButton;
  public static float scale_time = 0.2f;
  public static float buttonScaleTime = 0.1f;
  private List<PowerButton> _power_buttons = new List<PowerButton>();
  private PowerButton _last_selected_button;
  private PowerTabAsset _asset;
  private SelectedNanoBase _selected_nano;
  private int _children;
  public const int BACK_BUTTON_Y = -2;
  public const int TOP_BUTTON_Y = 16 /*0x10*/;
  private const int Y_TOP_ITEM = 32 /*0x20*/;
  private const int Y_BOTTOM_ITEM = -4;
  private const float Y_LINE_ITEM = 37.2f;
  private const int SIDE_SPACING = 2;
  private const int BUTTON_WIDTH = 32 /*0x20*/;

  private void Start()
  {
    this.parentObj = ((Component) ((Component) this).transform.parent.parent).gameObject;
    this._selected_nano = ((Component) this).GetComponent<SelectedNanoBase>();
    if (Object.op_Equality((Object) PowerTabController.instance.tab_main, (Object) this))
    {
      this.setActive();
      PowersTab._main_tab = PowerTabController.instance.tab_main;
    }
    else
      this.hideTab();
    foreach (PowerButton componentsInChild in ((Component) this).GetComponentsInChildren<PowerButton>())
    {
      if (!Object.op_Equality((Object) componentsInChild, (Object) null) && !Object.op_Equality((Object) componentsInChild.rect_transform, (Object) null))
        this._power_buttons.Add(componentsInChild);
    }
    this.findNeighbours();
    this._asset = AssetManager.power_tab_library.get(((Object) ((Component) this).gameObject).name);
    if (this._asset != null)
      return;
    Debug.LogError((object) ("No Power_Tab_library found for " + ((Object) ((Component) this).gameObject).name));
  }

  public void findNeighbours(bool pCheckForActive = false)
  {
    foreach (PowerButton powerButton in this._power_buttons)
    {
      powerButton.up = (PowerButton) null;
      powerButton.down = (PowerButton) null;
      powerButton.left = (PowerButton) null;
      powerButton.right = (PowerButton) null;
    }
    foreach (PowerButton powerButton in this._power_buttons)
      powerButton.findNeighbours(this._power_buttons, pCheckForActive);
  }

  public void update()
  {
    if (Object.op_Inequality((Object) this._selected_nano, (Object) null) && !ScrollWindow.isWindowActive() && !ScrollWindow.isAnimationActive())
      this._selected_nano.update();
    int num = ((Component) this).transform.CountChildren((Func<Transform, bool>) (pChild => ((Component) pChild).gameObject.activeSelf && !((Object) pChild).name.StartsWith("ButtonSelection")));
    if (this._children == num)
      return;
    this._children = num;
    this.sortButtons();
    this.setNewWidth();
    if (this._asset == null)
      return;
    this._asset.last_scroll_position = 0.0f;
  }

  public PowerTabAsset getAsset() => this._asset;

  private void selectButton(PowerButton pButton)
  {
    World.world.selected_buttons.unselectAll();
    if (Object.op_Equality((Object) pButton, (Object) null))
    {
      World.world.selected_buttons.clearHighlightedButton();
    }
    else
    {
      this._last_selected_button = pButton;
      if (pButton.godPower != null && pButton.godPower.activate_on_hotkey_select)
        World.world.selected_buttons.clickPowerButton(pButton);
      else
        World.world.selected_buttons.highlightButton(pButton);
      float num1 = (float) Screen.width / CanvasMain.instance.canvas_ui.scaleFactor;
      if ((double) ((Transform) pButton.rect_transform).position.x > 0.0 && (double) ((Transform) pButton.rect_transform).position.x + 32.0 < (double) Screen.width)
        return;
      float num2 = (float) (-(double) ((Component) pButton).transform.localPosition.x - 96.0) + num1;
      if ((double) num2 > 0.0)
        num2 = 0.0f;
      ShortcutExtensions.DOLocalMoveX(((Component) ((Component) pButton).transform.parent.parent.parent).transform, num2, 0.25f, false);
    }
  }

  internal int currentPowerIndex()
  {
    PowerButton selectedButton = World.world.selected_buttons.selectedButton;
    if (Object.op_Inequality((Object) selectedButton, (Object) null) && Object.op_Inequality((Object) selectedButton, (Object) this._last_selected_button))
    {
      if (Object.op_Equality((Object) this._last_selected_button, (Object) null))
        this._last_selected_button = selectedButton;
      else if (this._power_buttons.IndexOf(selectedButton) >= 0)
        this._last_selected_button = selectedButton;
    }
    int num = this._power_buttons.IndexOf(this._last_selected_button);
    if (num < 0)
      num = 0;
    return num;
  }

  internal PowerButton getActiveButton() => this._power_buttons[this.currentPowerIndex()];

  internal void leftButton()
  {
    PowerButton pButton = this.getActiveButton();
    while (Object.op_Inequality((Object) pButton.left, (Object) null))
    {
      pButton = pButton.left;
      if (pButton.canSelect() && ((Behaviour) pButton).isActiveAndEnabled)
        break;
    }
    this.selectButton(pButton);
  }

  internal void rightButton()
  {
    PowerButton pButton = this.getActiveButton();
    while (Object.op_Inequality((Object) pButton.right, (Object) null))
    {
      pButton = pButton.right;
      if (pButton.canSelect() && ((Behaviour) pButton).isActiveAndEnabled)
        break;
    }
    this.selectButton(pButton);
  }

  internal void upButton()
  {
    PowerButton activeButton = this.getActiveButton();
    if (Object.op_Inequality((Object) activeButton.up, (Object) null) && ((Behaviour) activeButton.up).isActiveAndEnabled && activeButton.up.canSelect())
    {
      this.selectButton(activeButton.up);
    }
    else
    {
      if (!Object.op_Inequality((Object) activeButton.down, (Object) null) || !((Behaviour) activeButton.down).isActiveAndEnabled || !activeButton.down.canSelect())
        return;
      this.selectButton(activeButton.down);
    }
  }

  internal void downButton()
  {
    PowerButton activeButton = this.getActiveButton();
    if (Object.op_Inequality((Object) activeButton.down, (Object) null) && ((Behaviour) activeButton.down).isActiveAndEnabled && activeButton.down.canSelect())
    {
      this.selectButton(activeButton.down);
    }
    else
    {
      if (!Object.op_Inequality((Object) activeButton.up, (Object) null) || !((Behaviour) activeButton.up).isActiveAndEnabled || !activeButton.up.canSelect())
        return;
      this.selectButton(activeButton.up);
    }
  }

  public static void showTabFromButton(Button pButtonTab, bool pHideTooltips = false)
  {
    ((UnityEvent) pButtonTab.onClick).Invoke();
    if (!pHideTooltips)
      return;
    Tooltip.hideTooltipNow();
  }

  public static PowersTab getActiveTab()
  {
    return !PowersTab.isTabSelected() ? PowersTab._main_tab : PowersTab._current_tab;
  }

  public static bool isTabSelected()
  {
    return Object.op_Inequality((Object) PowersTab._current_tab, (Object) null);
  }

  public static void unselect()
  {
    PowersTab._current_tab?.hideTab();
    PowersTab._main_tab.setActive();
    Tooltip.hideTooltip();
  }

  public bool isCurrentPowerTabSelected()
  {
    return Object.op_Equality((Object) PowersTab._current_tab, (Object) this);
  }

  public void tryToShowTab()
  {
    if (this.isCurrentPowerTabSelected())
      return;
    this.showTab((Button) null);
  }

  public void showTab(Button pTabButton)
  {
    bool flag = false;
    if (Object.op_Inequality((Object) PowersTab._current_tab, (Object) null))
    {
      if (this.isCurrentPowerTabSelected())
        flag = true;
      PowersTab._current_tab.hideTab();
      PowersTab._current_tab = (PowersTab) null;
    }
    if (flag)
    {
      PowersTab._main_tab.setActive();
    }
    else
    {
      PowerTabController.instance.tab_main.hideTab();
      PowersTab._current_tab = this;
      PowersTab._current_tab_button = pTabButton;
      this.setActive(pTabButton);
      if (Object.op_Inequality((Object) pTabButton, (Object) null))
      {
        string textOnClick = ((Component) pTabButton).gameObject.GetComponent<TipButton>().textOnClick;
        WorldTip.instance.showToolbarText(LocalizedTextManager.getText(textOnClick));
      }
      MusicBox.playSoundUI("event:/SFX/UI/ThumbnailsSlide");
    }
  }

  private void setActive(Button pTabButton = null)
  {
    if (Object.op_Inequality((Object) pTabButton, (Object) null))
      ((Selectable) pTabButton).image.sprite = this.image_selected;
    ((Component) this).gameObject.SetActive(true);
    ((Component) this).gameObject.transform.localPosition = new Vector3(0.0f, -16f);
    this.setNewWidth();
    if (this._asset != null)
    {
      PowerTabController.loadScrollPosition(this._asset.last_scroll_position);
      if (this._asset.tab_type_main)
        SelectedTabsHistory.clear();
    }
    Object.op_Equality((Object) ((Component) this).GetComponent<TabCenterer>(), (Object) null);
    ((Component) this).gameObject.transform.localScale = new Vector3(0.2f, 0.9f, 0.9f);
    TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOScale(((Component) this).gameObject.transform, 1f, PowersTab.scale_time), (Ease) 9);
    if (this._asset == null || !this._asset.tab_type_main)
      return;
    foreach (PowerTabAsset pPowerTabAsset in AssetManager.power_tab_library.list)
    {
      PowerTabAction onMainTabSelect = pPowerTabAsset.on_main_tab_select;
      if (onMainTabSelect != null)
        onMainTabSelect(pPowerTabAsset);
    }
  }

  private bool setNewWidth()
  {
    int childCount = ((Component) this).transform.childCount;
    RectTransform component1 = this.parentObj.GetComponent<RectTransform>();
    float x1 = component1.sizeDelta.x;
    float num1 = 0.0f;
    for (int index = 0; index < childCount; ++index)
    {
      GameObject gameObject = ((Component) ((Component) this).transform.GetChild(index)).gameObject;
      if (gameObject.activeSelf)
      {
        RectTransform component2 = gameObject.GetComponent<RectTransform>();
        double num2 = (double) ((Transform) component2).localPosition.x + (double) component2.sizeDelta.x;
        Rect rect = component2.rect;
        double x2 = (double) ((Rect) ref rect).x;
        float num3 = (float) (num2 + x2);
        if ((double) num3 > (double) num1)
          num1 = num3;
      }
    }
    component1.sizeDelta = new Vector2(num1 + 5f, component1.sizeDelta.y);
    PowerTabController.instance.resetToStartScrollPosition();
    return (double) x1 != (double) num1;
  }

  public bool recalc() => this.setNewWidth();

  public void hideTab()
  {
    this.saveScrollPosition();
    this.completeHide();
    PowersTab._current_tab = (PowersTab) null;
    if (!Object.op_Inequality((Object) PowersTab._current_tab_button, (Object) null))
      return;
    ((Selectable) PowersTab._current_tab_button).image.sprite = this.image_normal;
    PowersTab._current_tab_button = (Button) null;
  }

  private void saveScrollPosition()
  {
    if (this._asset == null)
      return;
    this._asset.last_scroll_position = PowerTabController.currentScrollPosition();
  }

  private void completeHide() => ((Component) this).gameObject.SetActive(false);

  private void OnDisable()
  {
    if (!Config.isDraggingItem())
      return;
    Config.getDraggingObject().KillDrag();
  }

  private void prepareButtonPosition(RectTransform pRect)
  {
    if (Object.op_Equality((Object) pRect, (Object) null))
      return;
    pRect.SetAnchor(AnchorPresets.TopLeft);
    pRect.SetPivot(PivotPresets.TopLeft);
  }

  private void restoreButtonPosition(RectTransform pRect)
  {
    if (Object.op_Equality((Object) pRect, (Object) null))
      return;
    pRect.SetPivot(PivotPresets.MiddleCenter, true);
  }

  public void sortButtons()
  {
    Transform transform = ((Component) this).gameObject.transform;
    float num1 = 9.6f;
    int num2 = 0;
    int num3 = 0;
    float num4 = 20f;
    for (int index = 0; index < transform.childCount; ++index)
    {
      GameObject gameObject = ((Component) transform.GetChild(index)).gameObject;
      ShortcutExtensions.DOKill((Component) gameObject.transform, true);
      if (gameObject.activeSelf)
      {
        RectTransform pRect;
        gameObject.TryGetComponent<RectTransform>(ref pRect);
        if (((Object) gameObject).name.StartsWith("_space_half"))
        {
          if (num3 > 0)
          {
            num4 += 36f;
            num3 = 0;
          }
          num4 += num1;
        }
        else if (((Object) gameObject).name.StartsWith("_line") || ((Object) gameObject).name.StartsWith("element_"))
        {
          if (num3 > 0)
          {
            num4 += 36f;
            num3 = 0;
          }
          float num5 = 32f;
          if (((Object) gameObject).name.StartsWith("_line"))
            num5 = 37.2f;
          this.prepareButtonPosition(pRect);
          gameObject.transform.localPosition = new Vector3(num4, num5, 0.0f);
          double num6 = (double) num4;
          Rect rect = ((RectTransform) gameObject.transform).rect;
          double width = (double) ((Rect) ref rect).width;
          num4 = (float) (num6 + width) + 4f;
          this.restoreButtonPosition(pRect);
        }
        else
        {
          PowerButton powerButton;
          gameObject.TryGetComponent<PowerButton>(ref powerButton);
          if (((Object) gameObject).name.Contains("_space") || !Object.op_Equality((Object) powerButton, (Object) null) && ((Behaviour) powerButton).isActiveAndEnabled)
          {
            bool flag = false;
            int num7;
            if (num3 % 2 == 0)
            {
              ++num2;
              num7 = 0;
              flag = num3 > 0;
            }
            else
              num7 = 1;
            ++num3;
            if (((Object) gameObject).name.StartsWith("_space"))
            {
              if (flag)
                num4 += 36f;
            }
            else
            {
              if (flag)
                num4 += 36f;
              if (!Object.op_Equality((Object) powerButton, (Object) null) && ((Behaviour) powerButton).isActiveAndEnabled)
              {
                float num8 = num7 != 0 ? -4f : 32f;
                if (((Object) gameObject).name.Contains("tab_back_button"))
                  num8 = 32f;
                this.prepareButtonPosition(pRect);
                ((Component) powerButton).transform.localPosition = new Vector3(num4, num8, 0.0f);
                this.restoreButtonPosition(pRect);
              }
            }
          }
        }
      }
    }
  }
}

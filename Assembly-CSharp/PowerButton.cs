// Decompiled with JetBrains decompiler
// Type: PowerButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class PowerButton : 
  MonoBehaviour,
  IPointerClickHandler,
  IEventSystemHandler,
  IBeginDragHandler,
  IDragHandler,
  IEndDragHandler,
  IInitializePotentialDragHandler,
  IScrollHandler
{
  public bool drag_power_bar;
  public string open_window_id = string.Empty;
  public bool block_same_window;
  public Image icon;
  private Image _image;
  private Button _button;
  public PowerButtonType type;
  public GameObject sizeButtons;
  public PowerButton mainSizeButton;
  internal GodPower godPower;
  private Image _icon_lock;
  public GameObject buttonUnlocked;
  public GameObject buttonUnlockedFlash;
  public static List<PowerButton> power_buttons = new List<PowerButton>();
  public static List<PowerButton> toggle_buttons = new List<PowerButton>();
  public static Dictionary<ActorAsset, PowerButton> actor_spawn_buttons = new Dictionary<ActorAsset, PowerButton>();
  internal RectTransform rect_transform;
  internal PowerButton left;
  internal PowerButton right;
  internal PowerButton down;
  internal PowerButton up;
  private Vector3 _default_scale;
  private Vector3 _clicked_scale;
  [HideInInspector]
  public bool is_selectable = true;
  private bool _initialized;

  private PowerButtonSelector _selected_buttons => PowerButtonSelector.instance;

  public static PowerButton get(string pID)
  {
    for (int index = 0; index < PowerButton.power_buttons.Count; ++index)
    {
      PowerButton powerButton = PowerButton.power_buttons[index];
      if (((Object) ((Component) powerButton).gameObject).name == pID)
        return powerButton;
    }
    return (PowerButton) null;
  }

  private void init()
  {
    this.rect_transform = ((Component) this).GetComponent<RectTransform>();
    this._image = ((Component) this).GetComponent<Image>();
    this._button = ((Component) this).GetComponent<Button>();
    if (this.type == PowerButtonType.Special || this.type == PowerButtonType.Active)
    {
      PowerButton.power_buttons.Add(this);
      this.godPower = AssetManager.powers.get(((Object) ((Component) this).gameObject).name);
    }
    else if (this.type == PowerButtonType.Shop)
      this.godPower = AssetManager.powers.get(((Object) ((Component) this).gameObject).name);
    if (this.godPower == null)
      return;
    if (this.godPower.disabled_on_mobile && Config.isMobile)
    {
      ((Component) this).gameObject.SetActive(false);
    }
    else
    {
      if (this.type == PowerButtonType.Active)
        GodPower.addPower(this.godPower, this);
      if (this.godPower.toggle_action != null)
        PowerButton.toggle_buttons.Add(this);
      if (!this.isActorSpawn())
        return;
      ActorAsset actorAsset = this.godPower.getActorAsset();
      if (!actorAsset.isAvailable())
        ((Graphic) this.icon).color = Toolbox.color_black;
      PowerButton.actor_spawn_buttons.TryAdd(actorAsset, this);
    }
  }

  private void OnEnable()
  {
    if (this._initialized)
      return;
    this._initialized = true;
    this.init();
    this._default_scale = ((Component) this).transform.localScale;
    this._clicked_scale = Vector3.op_Multiply(this._default_scale, 0.9f);
    if (this.type == PowerButtonType.Active && this.godPower != null)
    {
      if (((Object) ((Component) this).gameObject).name.Contains("Button"))
      {
        Color color;
        // ISSUE: explicit constructor call
        ((Color) ref color).\u002Ector(0.5f, 0.5f, 0.5f, 1f);
        ((Graphic) this._image).color = color;
        ((Graphic) this.icon).color = color;
      }
      else
        this.godPower.id = ((Object) ((Component) this).gameObject.transform).name;
    }
    if (((Component) this).HasComponent<TipButton>())
      return;
    // ISSUE: method pointer
    this._button.OnHover(new UnityAction((object) this, __methodptr(showTooltip)));
    // ISSUE: method pointer
    this._button.OnHoverOut(new UnityAction((object) null, __methodptr(hideTooltip)));
  }

  public void OnPointerClick(PointerEventData pEventData)
  {
    if (this.draggingBarEnabled() && ScrollRectExtended.isAnyDragged())
      return;
    if (!InputHelpers.mouseSupported && !Tooltip.isShowingFor((object) this) && (this.type == PowerButtonType.Active || this.type == PowerButtonType.Special) && Object.op_Inequality((Object) PowerButtonSelector.instance.selectedButton, (Object) this))
      this.showTooltip();
    this.clickButton();
  }

  internal void clickButton()
  {
    this.newClickAnimation();
    this.playSound();
    if ((this.type == PowerButtonType.Active || this.type == PowerButtonType.Library || this.type == PowerButtonType.Special) && (this.godPower == null || this.godPower.track_activity))
      PowerTracker.trackPower(this.getText());
    if (this.type == PowerButtonType.Active)
      this.clickActivePower();
    if (this.type == PowerButtonType.BrushSizeMain)
      this.clickSizeMainTool();
    if (this.type == PowerButtonType.TimeScale)
      this.clickTimeScaleTool();
    if (this.type == PowerButtonType.Shop)
      this.clickShop();
    if (this.type == PowerButtonType.Special)
      this.clickSpecial();
    if (this.type != PowerButtonType.Window)
      return;
    this.clickOpenWindow();
  }

  private void showTooltip()
  {
    if (InputHelpers.mouseSupported && !Config.tooltips_active)
      return;
    if (this.godPower != null)
    {
      TooltipData pData = new TooltipData();
      if ((Config.isComputer || Config.isEditor) && this.godPower.multiple_spawn_tip && this.type != PowerButtonType.Shop)
        pData.tip_description_2 = "hotkey_many_mod";
      pData.tip_name = this.godPower.getLocaleID();
      pData.tip_description = this.godPower.getDescriptionID();
      switch (this.godPower.type)
      {
        case PowerActionType.PowerSpawnActor:
          pData.power = this.godPower;
          Tooltip.show((object) this, "unit_spawn", pData);
          break;
        case PowerActionType.PowerSpawnSeeds:
          pData.power = this.godPower;
          Tooltip.show((object) this, "biome_seed", pData);
          break;
        default:
          Tooltip.show((object) this, "normal", pData);
          break;
      }
    }
    else
    {
      string text = this.getText();
      string description = this.getDescription();
      if (text == "")
        return;
      if (description != "")
        Tooltip.show((object) this, "normal", new TooltipData()
        {
          tip_name = text,
          tip_description = description
        });
      else
        Tooltip.show((object) this, "tip", new TooltipData()
        {
          tip_name = text
        });
    }
    ((Component) this).transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
    ShortcutExtensions.DOKill((Component) ((Component) this).transform, false);
    TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOScale(((Component) this).transform, 1f, 0.1f), (Ease) 26);
  }

  private string getText()
  {
    if (this.godPower != null)
      return this.godPower.getLocaleID();
    string pKey = ((Object) this._button).name.Underscore();
    if (LocalizedTextManager.stringExists("button_" + pKey))
      return "button_" + pKey;
    return LocalizedTextManager.stringExists(pKey) ? pKey : "";
  }

  private string getDescription()
  {
    if (this.godPower != null)
      return this.godPower.getDescriptionID();
    string str = ((Object) this._button).name.Underscore();
    if (LocalizedTextManager.stringExists($"button_{str}_description"))
      return $"button_{str}_description";
    return LocalizedTextManager.stringExists(str + "_description") ? str + "_description" : "";
  }

  private void clickOpenWindow()
  {
    if (this.open_window_id == "steam" && (Config.isComputer || Config.isEditor))
      this.open_window_id = "steam_workshop_main";
    if (ScrollWindow.isAnimationActive() && !ScrollWindow.isCurrentWindow(this.open_window_id))
      ScrollWindow.finishAnimations();
    PowerButtonSelector.instance.clearHighlightedButton();
    this.showWindow(this.open_window_id, this.block_same_window);
  }

  internal void clickSpecial()
  {
    Analytics.LogEvent("select_power", "powerID", this.godPower.id);
    if (this.godPower.id == "pause")
      ((Component) this).GetComponent<PauseButton>().press();
    if (this.godPower.toggle_action == null)
      return;
    this.godPower.toggle_action(this.godPower.id);
    PowerButtonSelector.instance.unselectAll();
    PowerButtonSelector.instance.checkToggleIcons();
  }

  public void checkToggleIcon()
  {
    GodPower godPower = this.godPower;
    if (godPower == null || string.IsNullOrEmpty(godPower.toggle_name))
      return;
    OptionAsset optionAsset = this.godPower.option_asset;
    bool flag = optionAsset.isActive();
    bool pEnabled1 = flag;
    bool pEnabled2 = flag;
    bool pEnabled3 = flag;
    ToggleIcon toggleIcon1 = (ToggleIcon) null;
    ToggleIcon toggleIcon2 = (ToggleIcon) null;
    ToggleIcon toggleIcon3 = (ToggleIcon) null;
    ToggleIcon component = ((Component) ((Component) this).transform.Find("ToggleIcon"))?.GetComponent<ToggleIcon>();
    if (godPower.multi_toggle)
    {
      int currentIntValue = optionAsset.current_int_value;
      toggleIcon1 = ((Component) ((Component) this).transform.Find("toggle_0"))?.GetComponent<ToggleIcon>();
      toggleIcon2 = ((Component) ((Component) this).transform.Find("toggle_1"))?.GetComponent<ToggleIcon>();
      toggleIcon3 = ((Component) ((Component) this).transform.Find("toggle_2"))?.GetComponent<ToggleIcon>();
      switch (optionAsset.max_value)
      {
        case 1:
          pEnabled1 = currentIntValue == 0;
          pEnabled3 = currentIntValue == 1;
          break;
        case 2:
          pEnabled1 = currentIntValue == 0;
          pEnabled2 = currentIntValue == 1;
          pEnabled3 = currentIntValue == 2;
          break;
        default:
          pEnabled1 = flag;
          pEnabled2 = flag;
          pEnabled3 = flag;
          break;
      }
    }
    component?.updateIcon(flag);
    if (!godPower.multi_toggle)
      return;
    toggleIcon1?.updateIconMultiToggle(flag, pEnabled3);
    toggleIcon2?.updateIconMultiToggle(flag, pEnabled1);
    toggleIcon3?.updateIconMultiToggle(flag, pEnabled2);
  }

  private void playSound() => SoundBox.click();

  public void checkLockIcon()
  {
    if (this.type == PowerButtonType.Shop)
      return;
    if (this.godPower != null && this.godPower.requires_premium)
    {
      if (Object.op_Equality((Object) this._icon_lock, (Object) null))
      {
        this._icon_lock = Object.Instantiate<Image>(PrefabLibrary.instance.iconLock, ((Component) this).transform);
        ((Behaviour) this._icon_lock).enabled = true;
      }
      if (Object.op_Equality((Object) this.buttonUnlocked, (Object) null))
      {
        this.buttonUnlocked = Object.Instantiate<GameObject>(PowerButtonSelector.instance.buttonUnlockedFlashNew, ((Component) this).transform);
        this.buttonUnlocked.transform.position = ((Component) this).transform.position;
        this.buttonUnlocked.SetActive(false);
        this.buttonUnlocked.transform.SetSiblingIndex(0);
        this.buttonUnlocked.GetComponent<RectTransform>().pivot = ((Component) this).GetComponent<RectTransform>().pivot;
      }
      if (Object.op_Equality((Object) this.buttonUnlockedFlash, (Object) null))
      {
        this.buttonUnlockedFlash = Object.Instantiate<GameObject>(PowerButtonSelector.instance.buttonUnlockedFlash, ((Component) this).transform);
        this.buttonUnlockedFlash.transform.position = ((Component) this).transform.position;
        this.buttonUnlockedFlash.SetActive(false);
        this.buttonUnlockedFlash.transform.SetSiblingIndex(0);
        this.buttonUnlockedFlash.GetComponent<RectTransform>().pivot = ((Component) this).GetComponent<RectTransform>().pivot;
      }
    }
    if (Object.op_Equality((Object) this._icon_lock, (Object) null))
      return;
    if (this.godPower == null)
      ((Behaviour) this._icon_lock).enabled = false;
    else if (Config.hasPremium)
      ((Behaviour) this._icon_lock).enabled = false;
    else
      ((Behaviour) this._icon_lock).enabled = true;
  }

  public void showOthers() => Debug.Log((object) "other");

  public bool isSelected() => this._selected_buttons.isPowerSelected(this);

  public void cancelSelection() => this._selected_buttons.unselectAll();

  public void unselectActivePower()
  {
    if (((Component) this).gameObject.activeInHierarchy)
      this.StartCoroutine(this.angleToZero());
    else
      ((Component) this.icon).transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
  }

  public void hideSizes() => this.sizeButtons.SetActive(false);

  private void clickSizeMainTool() => this.sizeButtons.SetActive(!this.sizeButtons.activeSelf);

  public void clickTimeScaleTool()
  {
    this.sizeButtons.SetActive(false);
    Config.setWorldSpeed(((Object) ((Component) this).transform).name);
    this.mainSizeButton.newClickAnimation();
    World.world.player_control.inspect_timer_click = 1f;
  }

  private void clickActivePower() => this._selected_buttons.clickPowerButton(this);

  public bool canSelect()
  {
    return this.is_selectable && (!this.isActorSpawn() || this.godPower.getActorAsset().isAvailable());
  }

  private void clickShop()
  {
    WorldTip.showNow($"{LocalizedTextManager.getText(this.godPower.getLocaleID())}\n{LocalizedTextManager.getText(this.godPower.getDescriptionID())}", false, "top");
  }

  public void setSelectedPower(PowerButton pLibraryButton, bool pAnim = false)
  {
    this.godPower = pLibraryButton.godPower;
  }

  public void newClickAnimation()
  {
    ((Component) this).gameObject.transform.localScale = this._clicked_scale;
    TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOScale(((Component) this).gameObject.transform, this._default_scale, 0.1f), (Ease) 28);
  }

  protected IEnumerator angleToZero()
  {
    while ((double) ((Component) this.icon).transform.localEulerAngles.z != 0.0)
    {
      Vector3 localEulerAngles = ((Component) this.icon).transform.localEulerAngles;
      localEulerAngles.z -= 100f * Time.deltaTime;
      if ((double) localEulerAngles.z < 0.0)
        localEulerAngles.z = 0.0f;
      ((Component) this.icon).transform.localEulerAngles = localEulerAngles;
      yield return (object) null;
    }
  }

  public void animate(float pElapsed)
  {
    if ((double) ((Component) this.icon).transform.localEulerAngles.z >= 20.0)
      return;
    Vector3 localEulerAngles = ((Component) this.icon).transform.localEulerAngles;
    localEulerAngles.z += 100f * pElapsed;
    if ((double) localEulerAngles.z > 20.0)
      localEulerAngles.z = 20f;
    ((Component) this.icon).transform.localEulerAngles = localEulerAngles;
  }

  public void destroyLockIcon()
  {
    if (Object.op_Inequality((Object) this.buttonUnlocked, (Object) null))
      Object.Destroy((Object) this.buttonUnlocked);
    if (Object.op_Inequality((Object) this.buttonUnlockedFlash, (Object) null))
      Object.Destroy((Object) this.buttonUnlockedFlash);
    if (Object.op_Inequality((Object) this._icon_lock, (Object) null))
    {
      Object.Destroy((Object) ((Component) this._icon_lock).gameObject);
    }
    else
    {
      Transform transform1 = ((Component) this).transform.Find("IconLock");
      if (Object.op_Inequality((Object) transform1, (Object) null))
      {
        Object.Destroy((Object) ((Component) transform1).gameObject);
      }
      else
      {
        Transform transform2 = ((Component) this).transform.Find("IconLock(Clone)");
        if (!Object.op_Inequality((Object) transform2, (Object) null))
          return;
        Object.Destroy((Object) ((Component) transform2).gameObject);
      }
    }
  }

  public void showWindow(string pID, bool pBlockSame)
  {
    if (ScrollWindow.isAnimationActive())
      return;
    ScrollWindow.showWindow(pID, pBlockSame: pBlockSame);
  }

  public void showWindow(string pID) => ScrollWindow.showWindow(pID, false, false);

  public void selectPowerTab(TweenCallback pOnComplete = null)
  {
    if (!((Component) ((Component) this).transform.parent).gameObject.activeInHierarchy)
    {
      Button tabForTabGroup = PowerTabController.instance.getTabForTabGroup(((Object) ((Component) this).transform.parent).name);
      if (Object.op_Inequality((Object) tabForTabGroup, (Object) null))
        PowersTab.showTabFromButton(tabForTabGroup);
    }
    RectTransform parent = ((Component) this).transform.parent.parent.parent as RectTransform;
    Rect rect = parent.rect;
    float width = ((Rect) ref rect).width;
    float num1 = (float) Screen.width / CanvasMain.instance.canvas_ui.scaleFactor;
    float num2 = (float) (-(double) ((Component) this).transform.localPosition.x + 32.0 + (double) num1 / 2.0);
    float num3 = 0.0f;
    if ((double) width > (double) num1)
      num3 = (float) (-1.0 * ((double) width - (double) num1));
    float num4 = Mathf.Clamp(num2, num3, 0.0f);
    ScrollRectExtended tScrollRect = ((Component) parent).GetComponentInParent<ScrollRectExtended>();
    tScrollRect.movementType = ScrollRectExtended.MovementType.Clamped;
    if (pOnComplete == null)
      TweenSettingsExtensions.OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOLocalMoveX((Transform) parent, num4, 1.5f, false), (Ease) 27), 0.3f), (TweenCallback) (() =>
      {
        tScrollRect.StopMovement();
        tScrollRect.movementType = ScrollRectExtended.MovementType.Elastic;
      }));
    else
      TweenSettingsExtensions.OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOLocalMoveX((Transform) parent, num4, 0.125f, false), (Ease) 27), (TweenCallback) (() =>
      {
        tScrollRect.StopMovement();
        tScrollRect.movementType = ScrollRectExtended.MovementType.Elastic;
        pOnComplete();
      }));
  }

  internal void findNeighbours(List<PowerButton> pButtons, bool pCheckForActive = false)
  {
    Vector2 anchoredPosition1 = this.rect_transform.anchoredPosition;
    if ((double) anchoredPosition1.y == -2.0)
      anchoredPosition1.y = 16f;
    foreach (PowerButton pButton in pButtons)
    {
      if (!Object.op_Equality((Object) pButton, (Object) this) && (!pCheckForActive || ((Component) pButton).gameObject.activeSelf))
      {
        Vector2 anchoredPosition2 = pButton.rect_transform.anchoredPosition;
        if ((double) anchoredPosition2.y == -2.0)
          anchoredPosition2.y = 16f;
        if ((double) anchoredPosition2.y == (double) anchoredPosition1.y)
        {
          if ((double) anchoredPosition2.x < (double) anchoredPosition1.x)
          {
            if (Object.op_Equality((Object) this.left, (Object) null))
              this.left = pButton;
            else if ((double) this.left.rect_transform.anchoredPosition.x < (double) anchoredPosition2.x)
              this.left = pButton;
          }
          if ((double) anchoredPosition2.x > (double) anchoredPosition1.x)
          {
            if (Object.op_Equality((Object) this.right, (Object) null))
              this.right = pButton;
            else if ((double) this.right.rect_transform.anchoredPosition.x > (double) anchoredPosition2.x)
              this.right = pButton;
          }
        }
        if ((double) anchoredPosition2.x == (double) anchoredPosition1.x)
        {
          if ((double) anchoredPosition2.y < (double) anchoredPosition1.y)
          {
            if (Object.op_Equality((Object) this.down, (Object) null))
              this.down = pButton;
            else if ((double) this.down.rect_transform.anchoredPosition.y < (double) anchoredPosition2.y)
              this.down = pButton;
          }
          if ((double) anchoredPosition2.y > (double) anchoredPosition1.y)
          {
            if (Object.op_Equality((Object) this.up, (Object) null))
              this.up = pButton;
            else if ((double) this.up.rect_transform.anchoredPosition.y > (double) anchoredPosition2.y)
              this.up = pButton;
          }
        }
      }
    }
    if (Object.op_Equality((Object) this.left, (Object) null))
    {
      foreach (PowerButton pButton in pButtons)
      {
        if (!Object.op_Equality((Object) pButton, (Object) this) && (!pCheckForActive || ((Component) pButton).gameObject.activeSelf))
        {
          Vector2 anchoredPosition3 = pButton.rect_transform.anchoredPosition;
          if ((double) anchoredPosition3.y == -2.0)
            anchoredPosition3.y = 16f;
          if ((double) anchoredPosition3.y == (double) anchoredPosition1.y)
          {
            if (Object.op_Equality((Object) this.left, (Object) null))
              this.left = pButton;
            else if ((double) this.left.rect_transform.anchoredPosition.x < (double) anchoredPosition3.x)
              this.left = pButton;
          }
        }
      }
    }
    if (!Object.op_Equality((Object) this.right, (Object) null))
      return;
    foreach (PowerButton pButton in pButtons)
    {
      if (!Object.op_Equality((Object) pButton, (Object) this) && (!pCheckForActive || ((Component) pButton).gameObject.activeSelf))
      {
        Vector2 anchoredPosition4 = pButton.rect_transform.anchoredPosition;
        if ((double) anchoredPosition4.y == -2.0)
          anchoredPosition4.y = 16f;
        if ((double) anchoredPosition4.y == (double) anchoredPosition1.y)
        {
          if (Object.op_Equality((Object) this.right, (Object) null))
            this.right = pButton;
          else if ((double) this.right.rect_transform.anchoredPosition.x > (double) anchoredPosition4.x)
            this.right = pButton;
        }
      }
    }
  }

  private bool isActorSpawn()
  {
    return this.godPower != null && this.godPower.type == PowerActionType.PowerSpawnActor;
  }

  public static void checkActorSpawnButtons()
  {
    Color color;
    // ISSUE: explicit constructor call
    ((Color) ref color).\u002Ector(0.75f, 0.75f, 0.75f, 0.9f);
    foreach (KeyValuePair<ActorAsset, PowerButton> actorSpawnButton in PowerButton.actor_spawn_buttons)
    {
      ActorAsset key = actorSpawnButton.Key;
      PowerButton powerButton = actorSpawnButton.Value;
      if (key.isAvailable())
      {
        if (key.countPopulation() > 0)
        {
          powerButton._image.sprite = ToolbarButtons.getSpriteButtonUnitExists();
          ((Graphic) powerButton.icon).color = Toolbox.color_white;
        }
        else
        {
          powerButton._image.sprite = ToolbarButtons.getSpriteButtonNormal();
          ((Graphic) powerButton.icon).color = color;
        }
      }
      else
        ((Graphic) powerButton.icon).color = Toolbox.color_black;
    }
  }

  private bool draggingBarEnabled() => this.drag_power_bar && InputHelpers.mouseSupported;

  public void OnBeginDrag(PointerEventData pEventData)
  {
    if (!this.draggingBarEnabled())
      return;
    ScrollRectExtended.SendMessageToAll(nameof (OnBeginDrag), pEventData);
  }

  public void OnDrag(PointerEventData pEventData)
  {
    if (!this.draggingBarEnabled())
      return;
    ScrollRectExtended.SendMessageToAll(nameof (OnDrag), pEventData);
  }

  public void OnEndDrag(PointerEventData pEventData)
  {
    if (!this.draggingBarEnabled())
      return;
    ScrollRectExtended.SendMessageToAll(nameof (OnEndDrag), pEventData);
  }

  public void OnInitializePotentialDrag(PointerEventData pEventData)
  {
    if (!this.draggingBarEnabled())
      return;
    ScrollRectExtended.SendMessageToAll(nameof (OnInitializePotentialDrag), pEventData);
  }

  public void OnScroll(PointerEventData pEventData)
  {
    if (!this.draggingBarEnabled())
      return;
    ScrollRectExtended.SendMessageToAll(nameof (OnScroll), pEventData);
  }

  public void OnDestroy()
  {
    ShortcutExtensions.DOKill((Component) ((Component) this).transform, false);
    PowerButton.power_buttons.Remove(this);
    PowerButton.toggle_buttons.Remove(this);
    if (!PowerButton.actor_spawn_buttons.ContainsValue(this))
      return;
    ActorAsset key = PowerButton.actor_spawn_buttons.FirstOrDefault<KeyValuePair<ActorAsset, PowerButton>>((Func<KeyValuePair<ActorAsset, PowerButton>, bool>) (x => Object.op_Equality((Object) x.Value, (Object) this))).Key;
    PowerButton.actor_spawn_buttons.Remove(key);
  }
}

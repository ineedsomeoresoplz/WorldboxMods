// Decompiled with JetBrains decompiler
// Type: OptionButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class OptionButton : MonoBehaviour
{
  private const float DISABLED_ALPHA = 0.8f;
  public static bool player_config_dirty;
  public Image icon;
  public Text counter;
  public Text text;
  public PowerButton button_switch;
  public SliderExtended slider;
  public GameObject sliderArea;
  public GameObject optionArea;
  public Action eventLeft;
  public Action eventRight;
  public Color color_enabled;
  public Color color_disabled;
  [SerializeField]
  private Sprite _button_sprite_interactable;
  [SerializeField]
  private Sprite _button_sprite_not_interactable;
  public SettingsWindow settings_window;

  private void Start()
  {
    this.settings_window = ((Component) ((Component) this).transform.parent).GetComponentInParent<SettingsWindow>();
    this.settings_window.buttons.Add(this);
  }

  private void OnDestroy()
  {
    if (!Object.op_Inequality((Object) this.settings_window, (Object) null))
      return;
    this.settings_window.buttons.Remove(this);
  }

  private void OnEnable()
  {
    if (this.option_asset == null)
    {
      Debug.LogError((object) ("Missing Option - " + ((Object) ((Component) this).transform).name));
    }
    else
    {
      string localeId = this.option_asset.getLocaleID();
      ((Component) this.text).GetComponent<LocalizedText>().setKeyAndUpdate(localeId);
      (this.option_asset.type != OptionType.Bool ? ((Component) this.slider).GetComponent<TipButton>() : ((Component) this.button_switch).GetComponent<TipButton>()).setHoverAction((TooltipAction) (() =>
      {
        if (!InputHelpers.mouseSupported)
          return;
        this.showTooltip();
      }));
      if (!string.IsNullOrEmpty(this.option_asset.getDescriptionID2()))
      {
        ((Component) this.slider).GetComponent<TipButton>().text_description_2 = this.option_asset.getDescriptionID2();
        ((Component) this.button_switch).GetComponent<TipButton>().text_description_2 = this.option_asset.getDescriptionID2();
      }
      if (this.option_asset.type == OptionType.Bool)
      {
        ((Component) this.counter).gameObject.SetActive(false);
        ((Component) this.button_switch).gameObject.SetActive(true);
        this.sliderArea.SetActive(false);
      }
      else if (this.option_asset.type == OptionType.Int)
      {
        ((Component) this.counter).gameObject.SetActive(true);
        ((Component) this.button_switch).gameObject.SetActive(false);
        this.sliderArea.SetActive(true);
        this.updateSlider();
        // ISSUE: method pointer
        ((UnityEvent<float>) this.slider.onValueChanged).AddListener(new UnityAction<float>((object) this, __methodptr(sliderChanged)));
        this.slider.addCallbackPointerDown(new SliderPointerDownEvent(this.checkShowSliderTooltip));
      }
      this.updateElements();
    }
  }

  public void showTooltip() => Tooltip.show((object) this, "tip", this.getTooltipData());

  public void checkShowSliderTooltip()
  {
    if (InputHelpers.mouseSupported)
      return;
    if (Input.touchCount >= 2)
      Tooltip.hideTooltip();
    else
      this.showTooltip();
  }

  private TooltipData getTooltipData()
  {
    TooltipData tooltipData = new TooltipData()
    {
      tip_name = this.option_asset.getLocaleID(),
      tip_description = this.option_asset.getDescriptionID()
    };
    string descriptionId2 = this.option_asset.getDescriptionID2();
    if (!string.IsNullOrEmpty(descriptionId2))
      tooltipData.tip_description_2 = descriptionId2;
    return tooltipData;
  }

  public void switchBoolOption()
  {
    PlayerConfig.setOptionBool(this.option_asset.id, !PlayerConfig.optionBoolEnabled(this.option_asset.id));
    OptionButton.player_config_dirty = true;
  }

  public void clickSwitch()
  {
    if (this.option_asset == null)
      Debug.LogError((object) ("Missing Option - " + ((Object) ((Component) this).transform).name));
    else if (!InputHelpers.mouseSupported && !Tooltip.isShowingFor((object) this))
    {
      this.showTooltip();
    }
    else
    {
      this.switchBoolOption();
      ActionOptionAsset action = this.option_asset.action;
      if (action != null)
        action(this.option_asset);
      if (this.option_asset.update_all_elements_after_click)
        this.settings_window.updateAllElements();
      else
        this.updateElements();
    }
  }

  private int clampAssetValue(int pValue)
  {
    return Mathf.Clamp(pValue, this.option_asset.min_value, this.option_asset.max_value);
  }

  private void sliderChanged(float pValue)
  {
    PlayerConfig.setOptionInt(this.option_asset.id, this.clampAssetValue((int) pValue));
    OptionButton.player_config_dirty = true;
    ActionOptionAsset action = this.option_asset.action;
    if (action != null)
      action(this.option_asset);
    this.updateElements();
    this.checkShowSliderTooltip();
  }

  public void updateElements(bool pCallCallbacks = false)
  {
    this.updateCounter();
    this.updateSwitchButton();
    this.updateColors();
    this.updateSlider(pCallCallbacks);
  }

  private void updateSlider(bool pCallCallbacks = false)
  {
    if (this.option_asset.type != OptionType.Int)
      return;
    this.slider.minValue = (float) this.option_asset.min_value;
    this.slider.maxValue = (float) this.option_asset.max_value;
    if (!pCallCallbacks)
      this.slider.SetValueWithoutNotify((float) PlayerConfig.getIntValue(this.option_asset.id));
    else
      this.slider.value = (float) PlayerConfig.getIntValue(this.option_asset.id);
    this.slider.wholeNumbers = false;
  }

  private void updateColors()
  {
    if (this.option_asset.type != OptionType.Bool)
      return;
    if (!this.option_asset.interactable)
    {
      ((Graphic) this.text).color = this.color_disabled;
      ((Graphic) this.icon).color = this.color_disabled;
    }
    else if (PlayerConfig.optionBoolEnabled(this.option_asset.id))
    {
      ((Graphic) this.text).color = this.color_enabled;
      ((Graphic) this.icon).color = this.color_enabled;
    }
    else
    {
      ((Graphic) this.text).color = this.color_disabled;
      ((Graphic) this.icon).color = this.color_disabled;
    }
  }

  private void updateSwitchButton()
  {
    if (this.option_asset.type != OptionType.Bool)
      return;
    Image component1 = ((Component) this.button_switch).GetComponent<Image>();
    CanvasGroup component2 = ((Component) this.button_switch).GetComponent<CanvasGroup>();
    if (!this.option_asset.interactable)
    {
      component2.alpha = 0.8f;
      component2.interactable = false;
      component1.sprite = this._button_sprite_not_interactable;
    }
    else
    {
      component2.interactable = true;
      component1.sprite = this._button_sprite_interactable;
      if (PlayerConfig.optionBoolEnabled(this.option_asset.id))
      {
        component2.alpha = 1f;
        ((Component) ((Component) this.button_switch).transform.Find("Text")).GetComponent<LocalizedText>().setKeyAndUpdate("short_on");
        this.button_switch.icon.sprite = SpriteTextureLoader.getSprite("ui/icons/IconOn");
      }
      else
      {
        component2.alpha = 0.8f;
        ((Component) ((Component) this.button_switch).transform.Find("Text")).GetComponent<LocalizedText>().setKeyAndUpdate("short_off");
        this.button_switch.icon.sprite = SpriteTextureLoader.getSprite("ui/icons/IconOff");
      }
    }
  }

  public void updateCounter()
  {
    if (this.option_asset.type != OptionType.Int)
      return;
    int intValue = PlayerConfig.getIntValue(this.option_asset.id);
    string str = intValue.ToString();
    if (this.option_asset.counter_format != null)
      str = this.option_asset.counter_format(this.option_asset);
    else if (this.option_asset.counter_percent)
      str += "%";
    this.counter.text = str;
    if (intValue == 0)
    {
      ((Graphic) this.text).color = this.color_disabled;
      ((Graphic) this.counter).color = this.color_disabled;
      ((Graphic) this.icon).color = this.color_disabled;
    }
    else
    {
      ((Graphic) this.text).color = this.color_enabled;
      ((Graphic) this.counter).color = this.color_enabled;
      ((Graphic) this.icon).color = this.color_enabled;
    }
  }

  private void sliderDragEnded()
  {
    if (InputHelpers.mouseSupported)
      return;
    Tooltip.hideTooltip();
  }

  public OptionAsset option_asset
  {
    get => AssetManager.options_library.get(((Object) ((Component) this).transform).name);
  }
}

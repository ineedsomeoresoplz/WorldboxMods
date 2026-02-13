// Decompiled with JetBrains decompiler
// Type: StatsIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class StatsIcon : MonoBehaviour
{
  private const float TWEEN_DURATION = 0.45f;
  private const float SCALE = 1.2f;
  public Text text;
  private float _value = -1f;
  private float? _max_value;
  private char _separator = '/';
  private string _ending = "";
  private string _color = "";
  private bool _is_float;
  private Tweener _cur_tween;
  private Tweener _text_scale_anim;
  private bool _is_counter_enabled;
  internal bool enable_animation = true;
  private TipButton _tip_button;
  private Vector2 _default_text_scale;

  private void Awake()
  {
    if (Object.op_Equality((Object) this.text, (Object) null))
      return;
    this._default_text_scale = Vector2.op_Implicit(((Component) this.text).transform.localScale);
    if (((Component) this).TryGetComponent<TipButton>(ref this._tip_button) && this._tip_button.type == "tip")
      this._tip_button.setHoverAction(new TooltipAction(this.tooltipAction));
    // ISSUE: method pointer
    ((UnityEvent) ((Component) this).gameObject.AddOrGetComponent<Button>().onClick).AddListener(new UnityAction((object) this, __methodptr(restartCounter)));
  }

  private void tooltipAction()
  {
    if (this._tip_button.textOnClick == "" && this._tip_button.textOnClickDescription == "")
      return;
    CustomDataContainer<string> customDataContainer = new CustomDataContainer<string>();
    customDataContainer["value"] = this._value.ToText();
    if (this._max_value.HasValue)
      customDataContainer["max_value"] = this._max_value.Value.ToText();
    Tooltip.show((object) ((Component) this).gameObject, "stats_icon", new TooltipData()
    {
      tip_name = this._tip_button.textOnClick,
      tip_description = this._tip_button.textOnClickDescription,
      tip_description_2 = this._tip_button.text_description_2,
      custom_data_string = customDataContainer
    });
  }

  public Image getIcon()
  {
    return ((Component) ((Component) this).transform.Find("Icon")).GetComponent<Image>();
  }

  private void restartCounter()
  {
    if (!this._is_counter_enabled || !this.enable_animation)
      return;
    this.setValue(this._value, this._max_value, this._color, this._is_float, this._ending, this._separator, true);
  }

  public void setValue(float pValue)
  {
    this.setValue(pValue, new float?(), "", false, "", '/', false);
  }

  public bool areValuesTooClose(float pNewValue) => Mathf.Approximately(this.getValue(), pNewValue);

  public void setValue(
    float pValue,
    float? pMax = null,
    string pColor = "",
    bool pFloat = false,
    string pEnding = "",
    char pSeparator = '/',
    bool pFromZero = false)
  {
    this._is_counter_enabled = true;
    float fromValue = pFromZero ? 0.0f : this._value;
    this._value = pValue;
    this._max_value = pMax;
    this._color = pColor;
    this._ending = pEnding;
    this._is_float = pFloat;
    this._separator = pSeparator;
    Color color = ((Graphic) this.text).color;
    color.a = (double) pValue != 0.0 ? 1f : 0.5f;
    ((Graphic) this.text).color = color;
    if (!this.enable_animation)
    {
      this.text.text = this.getFinalText();
    }
    else
    {
      this.checkDestroyTween();
      string ending = this.getEnding();
      if (pFloat)
        this._cur_tween = (Tweener) this.text.DOUpCounter(fromValue, this._value, 0.45f, ending, pColor);
      else
        this._cur_tween = (Tweener) this.text.DOUpCounter((int) fromValue, (int) this._value, 0.45f, ending, pColor);
    }
  }

  private string getEnding()
  {
    string ending = "";
    if (this._max_value.HasValue)
    {
      string str = ending + this._separator.ToString();
      if (this._is_float)
      {
        float? maxValue = this._max_value;
        float num1 = 1f;
        float? nullable = maxValue.HasValue ? new float?(maxValue.GetValueOrDefault() % num1) : new float?();
        float num2 = 0.0f;
        if (!((double) nullable.GetValueOrDefault() == (double) num2 & nullable.HasValue))
        {
          ending = str + this._max_value.Value.ToText();
          goto label_5;
        }
      }
      ending = str + Toolbox.formatNumber((long) this._max_value.Value, 4);
    }
label_5:
    if (!string.IsNullOrEmpty(this._ending))
      ending += this._ending;
    return ending;
  }

  private string getFinalText()
  {
    string pText = (!this._is_float || (double) this._value % 1.0 == 0.0 ? Toolbox.formatNumber((long) this._value, 4) : this._value.ToText()) + this.getEnding();
    return this._color != "" ? Toolbox.coloredText(pText, this._color) : pText;
  }

  public float getValue() => this._value;

  private void OnEnable() => this.restartCounter();

  private void OnDisable() => this.checkDestroyTween();

  public void checkDestroyTween()
  {
    TweenExtensions.Kill((Tween) this._cur_tween, true);
    this._cur_tween = (Tweener) null;
    Tweener textScaleAnim = this._text_scale_anim;
    if (textScaleAnim != null)
      TweenExtensions.Kill((Tween) textScaleAnim, true);
    this._text_scale_anim = (Tweener) null;
  }

  public void textScaleAnimation()
  {
    ((Component) this.text).transform.localScale = Vector2.op_Implicit(Vector2.op_Multiply(this._default_text_scale, 1.2f));
    Tweener textScaleAnim = this._text_scale_anim;
    if (textScaleAnim != null)
      TweenExtensions.Kill((Tween) textScaleAnim, true);
    this._text_scale_anim = (Tweener) TweenSettingsExtensions.SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(ShortcutExtensions.DOScale(((Component) this.text).transform, Vector2.op_Implicit(this._default_text_scale), 0.1f), (Ease) 26);
  }
}

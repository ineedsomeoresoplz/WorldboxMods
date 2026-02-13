// Decompiled with JetBrains decompiler
// Type: KeyValueField
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class KeyValueField : 
  MonoBehaviour,
  IPointerEnterHandler,
  IEventSystemHandler,
  IPointerExitHandler,
  ISelectHandler,
  IDeselectHandler
{
  public Color odd_color = Toolbox.makeColor("#000000", 0.0f);
  public Color even_color = Toolbox.makeColor("#30322B");
  public Color highlight_color = Toolbox.makeColor("#111111");
  public Image background;
  public Image icon;
  public Image icon_secondary;
  public Text name_text;
  public Text value;
  public bool auto_odd_even_coloring;
  public UnityAction on_hover_value;
  public UnityAction on_hover_value_out;
  public UnityAction on_click_value;
  private Color _not_highlight_color;
  private LocalizedText _name_text;
  private LocalizedText _value;
  private bool _check_language = true;

  private void Awake()
  {
    Button component = ((Component) this.value).GetComponent<Button>();
    if (Input.mousePresent)
    {
      // ISSUE: method pointer
      component.OnHover(new UnityAction((object) this, __methodptr(\u003CAwake\u003Eb__16_0)));
      // ISSUE: method pointer
      component.OnHoverOut(new UnityAction((object) this, __methodptr(\u003CAwake\u003Eb__16_1)));
    }
    // ISSUE: method pointer
    ((UnityEvent) component.onClick).AddListener(new UnityAction((object) this, __methodptr(\u003CAwake\u003Eb__16_2)));
    this._name_text = ((Component) this.name_text).GetComponent<LocalizedText>();
    this._value = ((Component) this.value).GetComponent<LocalizedText>();
    this._check_language = Object.op_Inequality((Object) this._name_text, (Object) null) || Object.op_Inequality((Object) this._value, (Object) null);
  }

  private void OnEnable()
  {
    this.checkLanguage();
    if (!this.auto_odd_even_coloring)
      return;
    this.checkOddEvenColor(((Component) this).transform.GetActiveSiblingIndex());
  }

  private void OnDisable()
  {
    this.on_hover_value = (UnityAction) null;
    this.on_hover_value_out = (UnityAction) null;
    this.on_click_value = (UnityAction) null;
  }

  private void checkLanguage()
  {
    if (!this._check_language)
      return;
    this._name_text?.checkSpecialLanguages();
    this._value?.checkSpecialLanguages();
  }

  public void checkOddEvenColor(int pIndex)
  {
    if (pIndex % 2 != 0)
      this.setEvenColor();
    else
      this.setOddColor();
  }

  public void OnPointerEnter(PointerEventData pData)
  {
    if (!InputHelpers.mouseSupported)
      return;
    this._not_highlight_color = ((Graphic) this.background).color;
    this.setHighlightColor();
  }

  public void OnPointerExit(PointerEventData pData)
  {
    if (!InputHelpers.mouseSupported)
      return;
    this.setBackgroundColor(this._not_highlight_color);
  }

  public void OnSelect(BaseEventData pEventData)
  {
    if (InputHelpers.mouseSupported)
      return;
    this._not_highlight_color = ((Graphic) this.background).color;
    this.setHighlightColor();
  }

  public void OnDeselect(BaseEventData pEventData)
  {
    if (InputHelpers.mouseSupported)
      return;
    this.setNotHighlightColor();
  }

  public void setEvenColor() => this.setBackgroundColor(this.even_color);

  public void setOddColor() => this.setBackgroundColor(this.odd_color);

  public void setHighlightColor() => this.setBackgroundColor(this.highlight_color);

  public void setNotHighlightColor() => this.setBackgroundColor(this._not_highlight_color);

  private void setBackgroundColor(Color pColor) => ((Graphic) this.background).color = pColor;

  public void setMetaForTooltip(
    MetaType pMetaType,
    long pMetaId,
    string pTooltipId = null,
    TooltipDataGetter pData = null)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    KeyValueField.\u003C\u003Ec__DisplayClass30_0 cDisplayClass300 = new KeyValueField.\u003C\u003Ec__DisplayClass30_0();
    // ISSUE: reference to a compiler-generated field
    cDisplayClass300.pMetaId = pMetaId;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass300.\u003C\u003E4__this = this;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass300.pTooltipId = pTooltipId;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass300.pData = pData;
    this.on_hover_value = (UnityAction) null;
    // ISSUE: method pointer
    this.on_hover_value_out = new UnityAction((object) null, __methodptr(hideTooltip));
    this.on_click_value = (UnityAction) null;
    if (!pMetaType.isNone())
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      KeyValueField.\u003C\u003Ec__DisplayClass30_1 cDisplayClass301 = new KeyValueField.\u003C\u003Ec__DisplayClass30_1();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass301.CS\u0024\u003C\u003E8__locals1 = cDisplayClass300;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass301.tAsset = AssetManager.meta_type_library.getAsset(pMetaType);
      // ISSUE: method pointer
      this.on_hover_value = new UnityAction((object) cDisplayClass301, __methodptr(\u003CsetMetaForTooltip\u003Eb__1));
      // ISSUE: method pointer
      this.on_click_value = new UnityAction((object) cDisplayClass301, __methodptr(\u003CsetMetaForTooltip\u003Eb__2));
    }
    else
    {
      // ISSUE: reference to a compiler-generated field
      if (string.IsNullOrEmpty(cDisplayClass300.pTooltipId))
        return;
      // ISSUE: method pointer
      this.on_hover_value = new UnityAction((object) cDisplayClass300, __methodptr(\u003CsetMetaForTooltip\u003Eb__0));
    }
  }
}

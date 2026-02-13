// Decompiled with JetBrains decompiler
// Type: LocalizedText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UPersian.Utils;

#nullable disable
public class LocalizedText : UIBehaviour
{
  public const string DEFAULT_KEY = "??????";
  protected const char LINE_ENDING = '\n';
  public bool convertToUppercase;
  public bool autoField = true;
  public bool specialTags;
  public string key = "??????";
  private FontStyle? _font_style_before;
  private bool? _shadow_before = new bool?(false);
  private bool _has_shadow;
  internal UnityEngine.UI.Text text;
  private TextAnchor? _text_alignment_before;

  protected virtual void Awake()
  {
    base.Awake();
    this.text = ((Component) this).GetComponent<UnityEngine.UI.Text>();
  }

  protected virtual void Start()
  {
    base.Start();
    if (!this.autoField)
      return;
    LocalizedTextManager.addTextField(this);
    this.updateText();
  }

  public void setKeyAndUpdate(string pKey)
  {
    this.key = pKey;
    this.updateText();
  }

  protected virtual void OnRectTransformDimensionsChange()
  {
    GameLanguageAsset currentLanguage = LocalizedTextManager.current_language;
    if ((currentLanguage != null ? (!currentLanguage.isRTL() ? 1 : 0) : 0) != 0 || string.IsNullOrEmpty(this.key) || this.key == "??????" || Object.op_Equality((Object) this.text, (Object) null))
      return;
    this.updateText();
    base.OnRectTransformDimensionsChange();
  }

  internal virtual void updateText(bool pCheckText = true)
  {
    if (Object.op_Equality((Object) this.text, (Object) null) || LocalizedTextManager.instance == null || !LocalizedTextManager.instance.initiated)
      return;
    if (Object.op_Inequality((Object) LocalizedTextManager.current_font, (Object) null))
      this.text.font = LocalizedTextManager.current_font;
    string str1 = LocalizedTextManager.getText(this.key, this.text);
    if (this.convertToUppercase)
      str1 = str1.ToUpper();
    if (this.specialTags && str1.Contains("$"))
    {
      int num;
      if (str1.Contains("$total_prem_powers$"))
      {
        string str2 = str1;
        num = GodPower.premium_powers.Count;
        string newValue = num.ToString() ?? "";
        str1 = str2.Replace("$total_prem_powers$", newValue);
      }
      if (str1.Contains("$minutes$"))
      {
        string str3 = str1;
        num = 30;
        string newValue = num.ToString() ?? "";
        str1 = str3.Replace("$minutes$", newValue);
      }
      if (str1.Contains("$minutes_clock$"))
      {
        string str4 = str1;
        num = 720;
        string newValue = num.ToString() ?? "";
        str1 = str4.Replace("$minutes_clock$", newValue);
      }
      if (str1.Contains("$hours_clock$"))
      {
        string str5 = str1;
        num = 12;
        string newValue = num.ToString() ?? "";
        str1 = str5.Replace("$hours_clock$", newValue);
      }
      if (str1.Contains("$power$") && Config.power_to_unlock != null)
        str1 = str1.Replace("$power$", Config.power_to_unlock.getLocaleID().Localize() ?? "");
      if (str1.Contains("$hours$"))
      {
        string str6 = str1;
        num = 3;
        string newValue = num.ToString() ?? "";
        str1 = str6.Replace("$hours$", newValue);
      }
      if (str1.Contains("$number$"))
      {
        string str7 = str1;
        num = 3;
        string newValue = num.ToString() ?? "";
        str1 = str7.Replace("$number$", newValue);
      }
      if (str1.Contains("$discord_count$"))
        str1 = str1.Replace("$discord_count$", 560000.ToText() ?? "");
      if (str1.Contains("$wbcode$"))
        str1 = str1.Replace("$wbcode$", "<color=cyan>WB-5555-1166-5555</color>");
      if (str1.Contains("$lifeissimhours$"))
        str1 = str1.Replace("$lifeissimhours$", 24f.ToText());
      if (str1.Contains("$current_era_year"))
        str1 = str1.Replace("$current_era_year$", Date.getCurrentYear().ToText());
      if (str1.Contains("$era_moons_left"))
      {
        int moonsLeft = World.world.era_manager.calculateMoonsLeft();
        str1 = str1.Replace("$era_moons_left$", moonsLeft.ToText());
      }
    }
    this.text.text = str1;
    this.checkTextFont();
    if (!pCheckText)
      return;
    this.checkSpecialLanguages();
  }

  internal void checkTextFont(GameLanguageAsset pLanguage = null)
  {
    if (Object.op_Equality((Object) this.text, (Object) null))
      return;
    if (pLanguage == null)
      pLanguage = LocalizedTextManager.current_language;
    Font font = pLanguage.font();
    if (Object.op_Equality((Object) font, (Object) null))
      return;
    this.text.font = font;
  }

  internal void checkSpecialLanguages(GameLanguageAsset pLanguage = null)
  {
    if (Object.op_Equality((Object) this.text, (Object) null))
      return;
    if (pLanguage == null)
      pLanguage = LocalizedTextManager.current_language;
    this.checkTextFont(pLanguage);
    if (!this._text_alignment_before.HasValue)
      this._text_alignment_before = new TextAnchor?(this.text.alignment);
    if (!this._font_style_before.HasValue)
      this._font_style_before = new FontStyle?(this.text.fontStyle);
    if (!this._shadow_before.HasValue)
      this._shadow_before = new bool?(this._has_shadow = ((Component) this.text).HasComponent<Shadow>());
    if (pLanguage.hasForcedStyle())
    {
      this.text.fontStyle = pLanguage.force_style.style;
      if (this.text.fontSize < 9 && pLanguage.force_style.shadow && !this._has_shadow)
      {
        ((Component) this.text).gameObject.AddComponent<Shadow>().effectColor = new Color(0.0f, 0.0f, 0.0f, 160f);
        this._has_shadow = true;
      }
    }
    else
    {
      this.text.fontStyle = this._font_style_before.Value;
      if (this._has_shadow)
      {
        bool? shadowBefore = this._shadow_before;
        bool flag = false;
        if (shadowBefore.GetValueOrDefault() == flag & shadowBefore.HasValue)
        {
          Shadow shadow;
          if (((Component) this.text).TryGetComponent<Shadow>(ref shadow))
            Object.Destroy((Object) shadow);
          this._has_shadow = false;
        }
      }
    }
    if (pLanguage.isRTL())
    {
      this.text.text = LocalizedText.getRTLText(this.text, this.text.text);
      this.text.alignment = this.getRTLAlignment(this._text_alignment_before.Value);
    }
    else
      this.text.alignment = this._text_alignment_before.Value;
    if (!pLanguage.isHindi() || Regex.IsMatch(this.text.text, "[a-zA-Z]"))
      return;
    this.text.SetHindiText(this.text.text);
  }

  internal static string getRTLText(UnityEngine.UI.Text pText, string pString)
  {
    string str1 = pString;
    TextGenerator cachedTextGenerator = pText.cachedTextGenerator;
    string str2 = str1;
    UnityEngine.UI.Text text = pText;
    Rect rect = ((Graphic) pText).rectTransform.rect;
    Vector2 size = ((Rect) ref rect).size;
    TextGenerationSettings generationSettings = text.GetGenerationSettings(size);
    cachedTextGenerator.Populate(str2, generationSettings);
    if (!(pText.cachedTextGenerator.lines is List<UILineInfo> lines))
      return (string) null;
    string rtlText = "";
    if (lines.Count == 0)
      rtlText = str1;
    for (int index = 0; index < lines.Count; ++index)
    {
      if (index < lines.Count - 1)
      {
        int startCharIdx = lines[index].startCharIdx;
        int length = lines[index + 1].startCharIdx - lines[index].startCharIdx;
        rtlText += str1.Substring(startCharIdx, length);
        if (rtlText.Length > 0 && rtlText[rtlText.Length - 1] != '\n' && rtlText[rtlText.Length - 1] != '\r')
          rtlText += "\n";
      }
      else
        rtlText += str1.Substring(lines[index].startCharIdx);
    }
    UPersianUtils.RtlFix(ref rtlText);
    return rtlText;
  }

  internal TextAnchor getRTLAlignment(TextAnchor pTextAlignment)
  {
    if (pTextAlignment == null)
      return (TextAnchor) 2;
    if (pTextAlignment == 2)
      return (TextAnchor) 0;
    if (pTextAlignment == 3)
      return (TextAnchor) 5;
    if (pTextAlignment == 5)
      return (TextAnchor) 3;
    if (pTextAlignment == 6)
      return (TextAnchor) 8;
    return pTextAlignment == 8 ? (TextAnchor) 6 : pTextAlignment;
  }
}

// Decompiled with JetBrains decompiler
// Type: LocalizedTextManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Newtonsoft.Json;
using Proyecto26;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEngine;

#nullable disable
public class LocalizedTextManager
{
  private const string MISSING_LOCALE = "LOC_ER";
  private static List<string> _all_languages;
  private static List<string> _changed_languages;
  public static LocalizedTextManager instance;
  private static readonly Dictionary<string, string> _boat_strings = new Dictionary<string, string>();
  public static GameLanguageAsset current_language;
  private bool _lang_dirty;
  private Dictionary<string, string> _localized_text;
  private Dictionary<string, string> _localized_text_files;
  private Font _default_font;
  private Font _hindi_font;
  private Font _japanese_font;
  private Font _korean_font;
  private Font _arabic_font;
  private Font _persian_font;
  private Font _simplified_chinese_font;
  private Font _thai_font;
  public Font default_font;
  internal bool initiated;
  internal string language = "not_set";
  internal List<LocalizedText> texts;

  public static Font current_font
  {
    get
    {
      GameLanguageAsset currentLanguage = LocalizedTextManager.current_language;
      Font font1;
      if (currentLanguage == null)
      {
        font1 = (Font) null;
      }
      else
      {
        FontGetter font2 = currentLanguage.font;
        font1 = font2 != null ? font2() : (Font) null;
      }
      return font1 ?? LocalizedTextManager.instance.default_font;
    }
  }

  public Font hindi_font
  {
    get
    {
      if (this._hindi_font == null)
        this._hindi_font = (Font) Resources.Load("Fonts/Poppins-Regular", typeof (Font));
      return this._hindi_font;
    }
  }

  public Font japanese_font
  {
    get
    {
      if (this._japanese_font == null)
        this._japanese_font = (Font) Resources.Load("Fonts/MPLUSRounded1c-Medium", typeof (Font));
      return this._japanese_font;
    }
  }

  public Font korean_font
  {
    get
    {
      if (this._korean_font == null)
        this._korean_font = (Font) Resources.Load("Fonts/NanumGothicCoding-Bold", typeof (Font));
      return this._korean_font;
    }
  }

  public Font persian_font
  {
    get
    {
      if (this._persian_font == null)
        this._persian_font = (Font) Resources.Load("Fonts/Vazirmatn-Bold", typeof (Font));
      return this._persian_font;
    }
  }

  public Font arabic_font
  {
    get
    {
      if (this._arabic_font == null)
        this._arabic_font = (Font) Resources.Load("Fonts/Tajawal-Bold", typeof (Font));
      return this._arabic_font;
    }
  }

  public Font simplified_chinese_font
  {
    get
    {
      if (this._simplified_chinese_font == null)
        this._simplified_chinese_font = (Font) Resources.Load("Fonts/NotoSansCJKsc-Bold", typeof (Font));
      return this._simplified_chinese_font;
    }
  }

  public Font thai_font
  {
    get
    {
      if (this._thai_font == null)
        this._thai_font = (Font) Resources.Load("Fonts/krubbold", typeof (Font));
      return this._thai_font;
    }
  }

  public static void init(string pLanguage = null)
  {
    if (LocalizedTextManager.instance != null)
      return;
    LocalizedTextManager.instance = new LocalizedTextManager();
    LocalizedTextManager.instance.create();
    if (pLanguage == null)
      pLanguage = PlayerConfig.dict["language"].stringVal;
    LocalizedTextManager.instance.setLanguage(pLanguage);
  }

  private void create()
  {
    this.default_font = (Font) Resources.Load("Fonts/Roboto-Bold", typeof (Font));
    LocalizedTextManager.instance = this;
    this.texts = new List<LocalizedText>();
  }

  public bool contains(string pString)
  {
    return LocalizedTextManager.instance._localized_text.ContainsKey(pString);
  }

  public static IEnumerable<string> getKeys()
  {
    return (IEnumerable<string>) LocalizedTextManager.instance._localized_text.Keys;
  }

  public static void addTextField(LocalizedText pText)
  {
    LocalizedTextManager.instance.texts.Add(pText);
    LocalizedTextManager.instance._lang_dirty = true;
  }

  public static void removeTextField(LocalizedText pText)
  {
    if (LocalizedTextManager.instance == null)
      return;
    LocalizedTextManager.instance.texts.Remove(pText);
  }

  public static void updateTexts()
  {
    Debug.Log((object) ("LocalizedTextManager: total texts loaded: " + LocalizedTextManager.instance.texts.Count.ToString()));
    foreach (LocalizedText text in LocalizedTextManager.instance.texts)
      text.updateText();
  }

  public static bool stringExists(string pKey)
  {
    return !string.IsNullOrEmpty(pKey) && LocalizedTextManager.instance._localized_text.ContainsKey(pKey);
  }

  public static string getText(string pKey, UnityEngine.UI.Text text = null, bool pForceEnglish = false)
  {
    if (LocalizedTextManager.instance.language == "boat")
      return LocalizedTextManager.transformToBoat(pKey);
    if (LocalizedTextManager.instance.language == "keys")
      return LocalizedTextManager.transformToKeys(pKey);
    string text1;
    if (LocalizedTextManager.instance._localized_text.ContainsKey(pKey))
    {
      text1 = LocalizedTextManager.instance._localized_text[pKey];
    }
    else
    {
      text1 = pKey;
      if (pKey.Contains("_placeholder"))
        text1 = "";
      else if (AssetManager.missing_locale_keys.Add(pKey))
      {
        Debug.LogError((object) ("LocalizedTextManager: missing text: " + pKey), (Object) text);
        AssetManager.generateMissingLocalesFile();
      }
    }
    if (pKey.StartsWith("world_law_", StringComparison.Ordinal))
      text1 = text1.Replace("\n\n", "\n");
    return text1;
  }

  public static string transformToKeys(string pTextKey)
  {
    string str = string.Empty;
    if (LocalizedTextManager.instance._localized_text_files.ContainsKey(pTextKey))
      str = LocalizedTextManager.instance._localized_text_files[pTextKey];
    return $"{str}: {pTextKey}";
  }

  public static string transformToBoat(string pTextKey)
  {
    if (LocalizedTextManager._boat_strings.ContainsKey(pTextKey))
      return LocalizedTextManager._boat_strings[pTextKey];
    int num = pTextKey.Split(' ', StringSplitOptions.None).Length + 1;
    string str = "";
    for (int index = 0; index < num; ++index)
    {
      if (str.Length > 0)
        str += " ";
      str = !Randy.randomBool() ? (!Randy.randomBool() ? (!Randy.randomBool() ? str + "ye" : str + "boat") : str + "Ahoy") : (!Randy.randomBool() ? (!Randy.randomBool() ? str + "Argh" : str + "Aye") : str + "Boat");
    }
    LocalizedTextManager._boat_strings[pTextKey] = str;
    return LocalizedTextManager._boat_strings[pTextKey];
  }

  public void loadLocalizedText(string pLocaleID)
  {
    this.initiated = true;
    LocalizedTextManager.instance._localized_text = new Dictionary<string, string>();
    LocalizedTextManager.instance._localized_text_files = new Dictionary<string, string>();
    string str = "locales/" + pLocaleID;
    TextAsset[] textAssetArray;
    try
    {
      textAssetArray = Resources.LoadAll<TextAsset>(str);
    }
    catch (Exception ex)
    {
      textAssetArray = Resources.LoadAll<TextAsset>("locales/en");
    }
    if (textAssetArray == null || textAssetArray.Length == 0)
      textAssetArray = Resources.LoadAll<TextAsset>("locales/en");
    foreach (TextAsset textAsset in textAssetArray)
    {
      string text = textAsset.text;
      string name = ((Object) textAsset).name;
      Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(text);
      foreach (string key in dictionary.Keys)
        LocalizedTextManager.add(key, dictionary[key], pFileName: name, pCheckForCharacters: false);
    }
  }

  public static void add(
    string pKey,
    string pTranslation,
    bool pReplace = false,
    string pFileName = "",
    bool pCheckForCharacters = true)
  {
    pKey = pKey.Underscore();
    if (!pReplace && LocalizedTextManager.instance._localized_text.ContainsKey(pKey))
    {
      if (LocalizedTextManager.instance._localized_text[pKey] == pTranslation)
      {
        Debug.LogWarning((object) $"Skipped - already exists - {pKey} - exists : {pTranslation}");
      }
      else
      {
        Debug.LogError((object) $"Already exists - {pKey} - exists : {LocalizedTextManager.instance._localized_text[pKey]}");
        Debug.LogError((object) $"Already exists - {pKey} - skipped: {pTranslation}");
      }
    }
    else
    {
      LocalizedTextManager.instance._localized_text[pKey] = pTranslation;
      LocalizedTextManager.instance._localized_text_files[pKey] = pFileName;
    }
  }

  public void setLanguage(string pLanguage)
  {
    if (this.language == pLanguage && !this._lang_dirty)
      return;
    this._lang_dirty = false;
    Debug.Log((object) ("LOAD LANGUAGE " + pLanguage));
    string str = "locales/" + pLanguage + "/creatures";
    if (pLanguage != "boat" && pLanguage != "keys" && Resources.Load(str) == null)
      pLanguage = PlayerConfig.detectLanguage();
    bool flag = false;
    if (this.language != "not_set")
      flag = true;
    this.language = pLanguage;
    LocalizedTextManager.instance.loadLocalizedText(pLanguage);
    try
    {
      RestClient.DefaultRequestHeaders["wb-language"] = this.language ?? "na";
    }
    catch (Exception ex)
    {
    }
    LocalizedTextManager.current_language = AssetManager.game_language_library.get(this.language);
    DebugLocales.init();
    LocalizedTextManager.updateTexts();
    if (PlayerConfig.dict["language"].stringVal != pLanguage)
      flag = true;
    PlayerConfig.dict["language"].stringVal = pLanguage;
    if (!flag)
      return;
    PlayerConfig.saveData();
  }

  public static string langToCulture(string pLanguage = null)
  {
    if (pLanguage == null)
      pLanguage = LocalizedTextManager.instance.language;
    string str = pLanguage;
    if (str == "boat" || str == "keys")
      return "";
    if (str == "lb")
      return "lb-LU";
    if (str == "ka")
      return "ka-GE";
    if (str == "gr")
      return "el-GR";
    if (str == "hr")
      return "hr-HR";
    if (str == "by")
      return "be-BY";
    if (str == "ch")
      return "zh-Hant";
    if (str == "cz")
      return "zh-Hans";
    if (str == "fn")
      return "fi-FI";
    if (str == "ph")
      return "fil-PH";
    if (str == "gr")
      return "fi";
    if (str == "br")
      return "pt";
    if (str == "ko")
      return "ko-KR";
    if (str == "th")
      return "th-TH";
    if (str == "ua")
      return "uk";
    if (str == "no")
      return "nb-NO";
    if (str == "lt")
      return "lt";
    return str == "vn" ? "vi" : str;
  }

  public static CultureInfo getCulture(string pLanguage = null)
  {
    string culture = LocalizedTextManager.langToCulture(pLanguage);
    if (!(culture != ""))
      return CultureInfo.CurrentCulture;
    try
    {
      return new CultureInfo(culture);
    }
    catch (CultureNotFoundException ex)
    {
      return CultureInfo.CurrentCulture;
    }
  }

  public static CultureInfo getCurrentCulture() => CultureInfo.CurrentCulture;

  public static bool cultureSupported()
  {
    string language = LocalizedTextManager.instance.language;
    return !(language == "boat") && !(language == "hi") && !(language == "by") && !(language == "ka") && !(language == "lb");
  }

  public static List<string> getAllLanguages()
  {
    if (LocalizedTextManager._all_languages == null)
    {
      LocalizedTextManager._all_languages = new List<string>();
      foreach (TextAsset textAsset in Resources.LoadAll<TextAsset>("locales"))
        LocalizedTextManager._all_languages.Add(((Object) textAsset).name);
    }
    return LocalizedTextManager._all_languages;
  }

  public static List<string> getAllLanguagesWithChanges()
  {
    if (LocalizedTextManager._changed_languages == null)
    {
      LocalizedTextManager._changed_languages = new List<string>();
      int num = 10;
      foreach (TextAsset textAsset in Resources.LoadAll<TextAsset>("locales"))
      {
        string name = ((Object) textAsset).name;
        string screenshotFolder = TesterBehScreenshotFolder.getScreenshotFolder(name);
        if (!(name == "boat") && !(name == "keys"))
        {
          if (File.Exists($"{screenshotFolder}/{name}.json"))
          {
            string str = File.ReadAllText($"{screenshotFolder}/{name}.json", Encoding.UTF8);
            string text = textAsset.text;
            Debug.Log((object) $"{text.Length.ToString()} vs {str.Length.ToString()}");
            if (str == text)
            {
              Debug.Log((object) $"Language {name} has no changes");
              continue;
            }
            Debug.Log((object) $"Language {name} has changes");
            File.Delete($"{screenshotFolder}/{name}.json");
          }
          LocalizedTextManager._changed_languages.Add(name);
          if (LocalizedTextManager._changed_languages.Count >= num)
            break;
        }
      }
    }
    return LocalizedTextManager._changed_languages;
  }
}

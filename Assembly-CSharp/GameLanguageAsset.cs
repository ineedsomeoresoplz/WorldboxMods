// Decompiled with JetBrains decompiler
// Type: GameLanguageAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class GameLanguageAsset : Asset
{
  public string name;
  public bool main;
  public bool export = true;
  public bool is_rtl;
  public bool is_hanzi;
  public bool is_hindi;
  public bool debug_only;
  public string path_icon;
  public bool show_translators = true;
  public FontGetter font = (FontGetter) (() => LocalizedTextManager.instance.default_font);
  public ForcedFontStyle force_style;
  private Dictionary<string, Dictionary<string, string>> _translations;
  private static Dictionary<string, GameLanguageData> _language_data;

  public IEnumerable<string> getGroups() => (IEnumerable<string>) this.translations.Keys;

  [JsonIgnore]
  public Dictionary<string, Dictionary<string, string>> translations
  {
    get
    {
      if (this._translations == null)
      {
        this._translations = new Dictionary<string, Dictionary<string, string>>();
        foreach (TextAsset textAsset in Resources.LoadAll<TextAsset>("locales/" + this.id))
          this._translations[((Object) textAsset).name] = JsonConvert.DeserializeObject<Dictionary<string, string>>(textAsset.text);
      }
      return this._translations;
    }
  }

  public GameLanguageData getLanguageData()
  {
    if (GameLanguageAsset._language_data == null)
    {
      TextAsset textAsset = Resources.Load<TextAsset>("texts/tooltip_translators");
      if (Object.op_Equality((Object) textAsset, (Object) null))
      {
        Debug.LogError((object) ("No tooltip translators found for language: " + this.id));
        return (GameLanguageData) null;
      }
      GameLanguageAsset._language_data = JsonConvert.DeserializeObject<Dictionary<string, GameLanguageData>>(textAsset.text);
    }
    GameLanguageData languageData;
    GameLanguageAsset._language_data.TryGetValue(this.id, out languageData);
    return languageData;
  }

  public bool isRTL() => this.is_rtl;

  public bool isHanzi() => this.is_hanzi;

  public bool isHindi() => this.is_hindi;

  public bool hasForcedStyle() => this.force_style != null;
}

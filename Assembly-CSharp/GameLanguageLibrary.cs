// Decompiled with JetBrains decompiler
// Type: GameLanguageLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class GameLanguageLibrary : AssetLibrary<GameLanguageAsset>
{
  public override void init()
  {
    base.init();
    this.add("en", "English");
    this.t.main = true;
    this.add("de", "Deutsch");
    this.add("pl", "Polski");
    this.add("cz", "中文(简体)");
    this.t.is_hanzi = true;
    this.t.force_style = new ForcedFontStyle((FontStyle) 0, true);
    this.t.font = (FontGetter) (() => LocalizedTextManager.instance.simplified_chinese_font);
    this.add("ch", "中文(繁體)");
    this.t.is_hanzi = true;
    this.t.force_style = new ForcedFontStyle((FontStyle) 0, true);
    this.add("ko", "한국어");
    this.t.font = (FontGetter) (() => LocalizedTextManager.instance.korean_font);
    this.add("ja", "日本語");
    this.t.is_hanzi = true;
    this.t.force_style = new ForcedFontStyle((FontStyle) 0, true);
    this.t.font = (FontGetter) (() => LocalizedTextManager.instance.japanese_font);
    this.add("th", "ภาษาไทย");
    this.t.font = (FontGetter) (() => LocalizedTextManager.instance.thai_font);
    this.add("vn", "Tiếng Việt");
    this.add("ru", "Русский");
    this.t.show_translators = false;
    this.add("ua", "Українська");
    this.add("by", "Беларуская");
    this.add("es", "Español");
    this.add("br", "Português do Brasil");
    this.add("pt", "Português de Portugal");
    this.add("fr", "Français");
    this.add("it", "Italiano");
    this.add("ro", "Română");
    this.add("hr", "Hrvatski");
    this.add("tr", "Türkçe");
    this.add("gr", "Ελληνικά");
    this.add("ka", "ქართული");
    this.t.font = (FontGetter) (() => LocalizedTextManager.instance.arabic_font);
    this.add("sk", "Slovenčina");
    this.add("cs", "Čeština");
    this.add("hu", "Magyar");
    this.add("nl", "Nederlands");
    this.add("id", "Bahasa Indonesia");
    this.add("sv", "Svenska");
    this.add("no", "Norsk");
    this.add("da", "Dansk");
    this.add("fa", "فارسی");
    this.t.is_rtl = true;
    this.t.font = (FontGetter) (() => LocalizedTextManager.instance.persian_font);
    this.add("ar", "العربية");
    this.t.is_rtl = true;
    this.t.font = (FontGetter) (() => LocalizedTextManager.instance.arabic_font);
    this.add("he", "עִברִית");
    this.t.is_rtl = true;
    this.add("fn", "Suomi");
    this.add("ph", "Tagalog");
    this.add("hi", "हिन्दी");
    this.t.is_hindi = true;
    this.t.font = (FontGetter) (() => LocalizedTextManager.instance.hindi_font);
    this.t.force_style = new ForcedFontStyle((FontStyle) 1);
    this.add("lb", "Lëtzebuergesch");
    this.add("lt", "Lithuanian");
    GameLanguageAsset pAsset1 = new GameLanguageAsset();
    pAsset1.id = "boat";
    pAsset1.name = "Boat";
    pAsset1.export = false;
    pAsset1.path_icon = "ui/Icons/iconSeaborn";
    this.add(pAsset1);
    GameLanguageAsset pAsset2 = new GameLanguageAsset();
    pAsset2.id = "keys";
    pAsset2.name = "KEYS";
    pAsset2.export = false;
    pAsset2.debug_only = true;
    pAsset2.path_icon = "ui/Icons/iconDebug";
    this.add(pAsset2);
  }

  public GameLanguageAsset add(string pID, string pName)
  {
    GameLanguageAsset pAsset = new GameLanguageAsset();
    pAsset.id = pID;
    pAsset.name = pName;
    return this.add(pAsset);
  }
}

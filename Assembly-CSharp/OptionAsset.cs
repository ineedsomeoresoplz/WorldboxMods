// Decompiled with JetBrains decompiler
// Type: OptionAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
[Serializable]
public class OptionAsset : 
  Asset,
  IDescription2Asset,
  IDescriptionAsset,
  ILocalizedAsset,
  IMultiLocalesAsset
{
  public OptionType type;
  [DefaultValue(true)]
  public bool has_locales = true;
  public string translation_key;
  public string translation_key_description;
  public string translation_key_description_2;
  [DefaultValue(true)]
  public bool reset_to_default_on_launch;
  [DefaultValue(true)]
  public bool default_bool = true;
  public bool computer_only;
  public bool override_bool_mobile;
  [DefaultValue(true)]
  public bool default_bool_mobile = true;
  public string default_string = string.Empty;
  public int default_int;
  public ActionOptionAsset action;
  public ActionFormatCounterOptionAsset counter_format;
  public int min_value;
  public int max_value;
  public bool multi_toggle;
  public bool counter_percent;
  public string[] locale_options_ids;
  public bool update_all_elements_after_click;
  [DefaultValue(true)]
  public bool interactable = true;

  public string getLocaleID()
  {
    if (!this.has_locales)
      return (string) null;
    return !string.IsNullOrEmpty(this.translation_key) ? this.translation_key : this.id;
  }

  public string getDescriptionID()
  {
    return !this.has_locales ? (string) null : this.translation_key_description;
  }

  public string getDescriptionID2()
  {
    return !this.has_locales ? (string) null : this.translation_key_description_2;
  }

  public string getOptionLocaleID(int pIndex) => this.locale_options_ids[pIndex];

  public string getOptionLocaleID() => this.getOptionLocaleID(this.current_int_value);

  public string getTranslatedOption() => this.getOptionLocaleID().Localize();

  public IEnumerable<string> getLocaleIDs()
  {
    if (this.has_locales)
    {
      yield return this.getLocaleID();
      yield return this.getDescriptionID();
      yield return this.getDescriptionID2();
      if (this.locale_options_ids != null)
      {
        string[] strArray = this.locale_options_ids;
        for (int index = 0; index < strArray.Length; ++index)
          yield return strArray[index];
        strArray = (string[]) null;
      }
    }
  }

  public bool isActive()
  {
    return this.type != OptionType.Bool ? PlayerConfig.getOptionInt(this.id) > 0 : PlayerConfig.optionBoolEnabled(this.id);
  }

  [JsonIgnore]
  public PlayerOptionData data => PlayerConfig.dict[this.id];

  [JsonIgnore]
  public int current_int_value => PlayerConfig.dict[this.id].intVal;

  [JsonIgnore]
  public bool current_bool_value => PlayerConfig.optionBoolEnabled(this.id);
}

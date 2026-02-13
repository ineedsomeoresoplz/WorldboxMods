// Decompiled with JetBrains decompiler
// Type: PlayerConfigData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using UnityEngine.Scripting;

#nullable disable
[Preserve]
[Serializable]
public class PlayerConfigData
{
  [DefaultValue(5)]
  public int nextReward = 5;
  [DefaultValue("")]
  public string powerReward = "";
  [DefaultValue("")]
  public string lastReward = "";
  [DefaultValue(-1.0)]
  public double nextAdTimestamp = -1.0;
  public List<RewardedPower> rewardedPowers = new List<RewardedPower>();
  public List<PlayerOptionData> list = new List<PlayerOptionData>();
  [Preserve]
  [Obsolete("use GameProgressData.achievements instead")]
  public List<string> achievements = new List<string>();
  [Preserve]
  [Obsolete("use GameProgressData.unlocked_traits instead")]
  public List<string> unlocked_traits = new List<string>();
  public List<string> trait_editor_gamma = new List<string>();
  [DefaultValue(RainState.Add)]
  public RainState trait_editor_gamma_state;
  public List<string> trait_editor_omega = new List<string>();
  [DefaultValue(RainState.Add)]
  public RainState trait_editor_omega_state;
  public List<string> trait_editor_delta = new List<string>();
  [DefaultValue(RainState.Add)]
  public RainState trait_editor_delta_state;
  public List<string> equipment_editor = new List<string>();
  [DefaultValue(RainState.Add)]
  public RainState equipment_editor_state;
  [DefaultValue(-1)]
  public int favorite_world = -1;
  internal string worldnet = "";
  public bool premium;
  public bool valCheck2025;
  public bool magicCheck2025;
  public bool fireworksCheck2025;
  public int saveVersion = 1;
  public int lastRateID;
  public bool tutorialFinished;
  [DefaultValue(true)]
  public bool pPossible0507 = true;
  public bool premiumDisabled;
  public bool clearDebugOnStart;
  public bool testAds;

  public void initData()
  {
    PlayerConfig.dict.Clear();
    foreach (OptionAsset optionAsset in AssetManager.options_library.list)
    {
      if (optionAsset.id[0] != '_')
      {
        PlayerOptionData pData = new PlayerOptionData(optionAsset.id);
        if (optionAsset.type == OptionType.Bool)
          pData.boolVal = optionAsset.default_bool;
        else if (optionAsset.type == OptionType.String)
          pData.stringVal = optionAsset.default_string;
        else if (optionAsset.type == OptionType.Int)
          pData.intVal = optionAsset.default_int;
        if (Config.isMobile && optionAsset.override_bool_mobile)
          pData.boolVal = optionAsset.default_bool_mobile;
        this.add(pData);
      }
    }
  }

  public PlayerOptionData get(string pKey)
  {
    foreach (PlayerOptionData playerOptionData in this.list)
    {
      if (string.Equals(pKey, playerOptionData.name))
        return playerOptionData;
    }
    return (PlayerOptionData) null;
  }

  public PlayerOptionData add(PlayerOptionData pData)
  {
    foreach (PlayerOptionData playerOptionData in this.list)
    {
      if (string.Equals(pData.name, playerOptionData.name))
      {
        PlayerConfig.dict.Add(playerOptionData.name, playerOptionData);
        return playerOptionData;
      }
    }
    this.list.Add(pData);
    PlayerConfig.dict.Add(pData.name, pData);
    return pData;
  }

  public string toJson()
  {
    using (StringBuilderPool stringBuilderPool = new StringBuilderPool(8192 /*0x2000*/))
    {
      using (StringWriter stringWriter = new StringWriter(stringBuilderPool.string_builder, (IFormatProvider) CultureInfo.InvariantCulture))
      {
        using (JsonTextWriter jsonTextWriter = new JsonTextWriter((TextWriter) stringWriter))
          JsonHelper.writer.Serialize((JsonWriter) jsonTextWriter, (object) this, typeof (PlayerConfigData));
        return stringWriter.ToString();
      }
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: SettingsWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class SettingsWindow : TabbedWindow
{
  internal List<OptionButton> buttons = new List<OptionButton>();

  public void resetToDefault()
  {
    foreach (OptionButton button in this.buttons)
    {
      OptionAsset optionAsset = button.option_asset;
      if (optionAsset.type == OptionType.Bool)
        PlayerConfig.setOptionBool(optionAsset.id, optionAsset.default_bool);
      else if (optionAsset.type == OptionType.String)
        PlayerConfig.setOptionString(optionAsset.id, optionAsset.default_string);
      else if (optionAsset.type == OptionType.Int)
        PlayerConfig.setOptionInt(optionAsset.id, optionAsset.default_int);
      if (Config.isMobile && optionAsset.override_bool_mobile)
        PlayerConfig.setOptionBool(optionAsset.id, optionAsset.default_bool_mobile);
    }
    this.updateAllElements(true);
  }

  public void updateAllElements(bool pCallCallbacks = false)
  {
    foreach (OptionButton button in this.buttons)
      button.updateElements(pCallCallbacks);
  }

  private void OnDisable()
  {
    if (!OptionButton.player_config_dirty)
      return;
    OptionButton.player_config_dirty = false;
    PlayerConfig.saveData();
  }
}

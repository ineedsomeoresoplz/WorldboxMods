// Decompiled with JetBrains decompiler
// Type: HotkeyAsset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

#nullable disable
[Serializable]
public class HotkeyAsset : Asset
{
  public KeyCode default_key_mod_1;
  public KeyCode default_key_mod_2;
  public KeyCode default_key_mod_3;
  public KeyCode default_key_1;
  public KeyCode default_key_2;
  public KeyCode default_key_3;
  public KeyCode overridden_key_1;
  public KeyCode overridden_key_2;
  public KeyCode overridden_key_3;
  public KeyCode overridden_key_mod_1;
  public KeyCode overridden_key_mod_2;
  public KeyCode overridden_key_mod_3;
  public bool use_mouse_wheel;
  public HotkeyAction just_pressed_action;
  public HotkeyAction holding_action;
  [DefaultValue(0.1f)]
  public float holding_cooldown = 0.1f;
  [DefaultValue(0.33f)]
  public float holding_cooldown_first_action = 0.33f;
  public bool ignore_same_key_diagnostic;
  public bool disable_for_controlled_unit;
  public bool ignore_mod_keys;
  public bool check_only_controllable_unit;
  public bool check_only_not_controllable_unit;
  public bool check_controls_locked;
  public bool check_window_active;
  public bool check_window_not_active;
  public bool check_render_gameplay;
  public bool check_render_minimap;
  public bool check_debug_active;
  public bool check_no_multi_unit_selection;
  public bool check_no_selection;
  public bool check_multi_unit_selection;
  public bool allow_unit_control;

  public bool isJustPressed()
  {
    if (!Input.anyKeyDown || this.disable_for_controlled_unit && ControllableUnit.isControllingUnit())
      return false;
    if (this.hasModKey())
    {
      if (!this.isHoldingModKey())
        return false;
      if (!this.hasKey() && this.isJustPressedModKey())
        return true;
    }
    else if (!this.ignore_mod_keys && HotkeyAsset.isHoldingAnyModKey())
      return false;
    return this.hasKey() && this.isJustPressedKey();
  }

  public bool isHolding()
  {
    if (this.use_mouse_wheel)
    {
      if ((double) Input.mouseScrollDelta.y == 0.0)
        return false;
    }
    else if (!Input.anyKey)
      return false;
    if (this.disable_for_controlled_unit && ControllableUnit.isControllingUnit())
      return false;
    if (this.hasModKey())
    {
      if (!this.isHoldingModKey())
        return false;
      if (!this.hasKey() && this.isHoldingModKey())
        return true;
    }
    else if (!this.ignore_mod_keys && HotkeyAsset.isHoldingAnyModKey())
      return false;
    return this.hasKey() && this.isHoldingKey() || this.use_mouse_wheel;
  }

  private bool isHoldingModKey()
  {
    return Input.anyKey && (!this.disable_for_controlled_unit || !ControllableUnit.isControllingUnit()) && (this.default_key_mod_1 != null && Input.GetKey(this.default_key_mod_1) || this.default_key_mod_2 != null && Input.GetKey(this.default_key_mod_2) || this.default_key_mod_3 != null && Input.GetKey(this.default_key_mod_3));
  }

  public static bool isHoldingAnyModKey() => AssetManager.hotkey_library.isHoldingAnyModKey();

  private bool hasKey() => this.default_key_1 > 0;

  private bool hasModKey() => this.default_key_mod_1 > 0;

  public string getLocalizedKeys()
  {
    string localizedKeys = "";
    string localizedKey1 = HotkeysLocalized.getLocalizedKey(this.default_key_1);
    string localizedKey2 = HotkeysLocalized.getLocalizedKey(this.default_key_2);
    string localizedKey3 = HotkeysLocalized.getLocalizedKey(this.default_key_3);
    string localizedKey4 = HotkeysLocalized.getLocalizedKey(this.default_key_mod_1);
    string localizedKey5 = HotkeysLocalized.getLocalizedKey(this.default_key_mod_2);
    string localizedKey6 = HotkeysLocalized.getLocalizedKey(this.default_key_mod_3);
    List<string> collection1 = new List<string>();
    if (!string.IsNullOrEmpty(localizedKey1))
      collection1.Add(localizedKey1);
    if (!string.IsNullOrEmpty(localizedKey2))
      collection1.Add(localizedKey2);
    if (!string.IsNullOrEmpty(localizedKey3))
      collection1.Add(localizedKey3);
    List<string> collection2 = new List<string>();
    if (!string.IsNullOrEmpty(localizedKey4))
      collection2.Add(localizedKey4);
    if (!string.IsNullOrEmpty(localizedKey5))
      collection2.Add(localizedKey5);
    if (!string.IsNullOrEmpty(localizedKey6))
      collection2.Add(localizedKey6);
    List<string> values1 = new List<string>((IEnumerable<string>) new HashSet<string>((IEnumerable<string>) collection1));
    List<string> values2 = new List<string>((IEnumerable<string>) new HashSet<string>((IEnumerable<string>) collection2));
    if (this.hasKey() && this.hasModKey())
    {
      int num = Mathf.Max(values1.Count, values2.Count);
      string str1 = "";
      string str2 = "";
      for (int index = 0; index < num; ++index)
      {
        if (index > 0)
          localizedKeys += " / ";
        if (index < values2.Count)
          str1 = values2[index];
        if (index < values1.Count)
          str2 = values1[index];
        localizedKeys = $"{localizedKeys}{str1} + {str2}";
      }
    }
    else if (this.hasModKey())
      localizedKeys += string.Join(", ", (IEnumerable<string>) values2);
    else if (this.hasKey())
      localizedKeys += string.Join(", ", (IEnumerable<string>) values1);
    return localizedKeys;
  }

  private bool isHoldingKey()
  {
    return Input.anyKey && (this.default_key_1 != null && Input.GetKey(this.default_key_1) || this.default_key_2 != null && Input.GetKey(this.default_key_2) || this.default_key_3 != null && Input.GetKey(this.default_key_3));
  }

  private bool isJustPressedKey()
  {
    return Input.anyKeyDown && (this.default_key_1 != null && Input.GetKeyDown(this.default_key_1) || this.default_key_2 != null && Input.GetKeyDown(this.default_key_2) || this.default_key_3 != null && Input.GetKeyDown(this.default_key_3));
  }

  private bool isJustPressedModKey()
  {
    return Input.anyKeyDown && (this.default_key_mod_1 != null && Input.GetKeyDown(this.default_key_mod_1) || this.default_key_mod_2 != null && Input.GetKeyDown(this.default_key_mod_2) || this.default_key_mod_3 != null && Input.GetKeyDown(this.default_key_mod_3));
  }

  public bool checkIsPossible()
  {
    return (!this.check_render_gameplay || MapBox.isRenderGameplay()) && (!this.check_render_minimap || MapBox.isRenderMiniMap()) && (!this.check_window_active || ScrollWindow.isWindowActive() && !ScrollWindow.isAnimationActive()) && (!this.check_window_not_active || !ScrollWindow.isWindowActive()) && (!this.check_no_selection || !SelectedUnit.isSet()) && (!this.check_no_multi_unit_selection || !SelectedUnit.multipleSelected()) && (!this.check_multi_unit_selection || SelectedUnit.multipleSelected()) && (!this.check_only_not_controllable_unit || !ControllableUnit.isControllingUnit()) && (!this.check_only_controllable_unit || ControllableUnit.isControllingUnit());
  }
}

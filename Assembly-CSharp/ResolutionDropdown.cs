// Decompiled with JetBrains decompiler
// Type: ResolutionDropdown
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
[Obsolete]
public class ResolutionDropdown : MonoBehaviour
{
  private Button button;
  private Dropdown dropdown;
  public OptionBool fullscreenOption;
  private static List<string> options = new List<string>();

  private void Start()
  {
    this.dropdown = ((Component) this).GetComponent<Dropdown>();
    this.PopulateDropdown(this.dropdown);
    // ISSUE: method pointer
    ((UnityEvent<int>) this.dropdown.onValueChanged).AddListener(new UnityAction<int>((object) this, __methodptr(\u003CStart\u003Eb__4_0)));
  }

  private void OnEnable()
  {
    if (!Config.game_loaded)
      return;
    this.dropdown = ((Component) this).GetComponent<Dropdown>();
    this.PopulateDropdown(this.dropdown);
  }

  private void DropdownValueChanged(Dropdown change)
  {
    Resolution[] resolutions = Screen.resolutions;
    if (ResolutionDropdown.options[change.value] == LocalizedTextManager.getText("windowed_mode"))
    {
      PlayerConfig.setFullScreen(false);
    }
    else
    {
      foreach (Resolution resolution in resolutions)
      {
        if (resolution.ToString() == ResolutionDropdown.options[change.value])
        {
          if (!Screen.fullScreen)
            PlayerConfig.setFullScreen(true, false);
          Screen.SetResolution(((Resolution) ref resolution).width, ((Resolution) ref resolution).height, true, ((Resolution) ref resolution).refreshRate);
          break;
        }
      }
    }
    this.fullscreenOption.checkGameOption();
  }

  private void PopulateDropdown(Dropdown dropdown)
  {
    ResolutionDropdown.options.Clear();
    foreach (Resolution resolution in Screen.resolutions)
      ResolutionDropdown.options.Add(resolution.ToString());
    ResolutionDropdown.options.Add(LocalizedTextManager.getText("windowed_mode"));
    dropdown.ClearOptions();
    ResolutionDropdown.options.Reverse();
    int num = ResolutionDropdown.options.IndexOf(Screen.currentResolution.ToString());
    if (!Screen.fullScreen)
      num = ResolutionDropdown.options.IndexOf(LocalizedTextManager.getText("windowed_mode"));
    dropdown.AddOptions(ResolutionDropdown.options);
    if (num > -1)
    {
      dropdown.value = num;
    }
    else
    {
      ResolutionDropdown.options.Insert(0, Screen.currentResolution.ToString());
      dropdown.AddOptions(ResolutionDropdown.options);
      dropdown.value = ResolutionDropdown.options.IndexOf(Screen.currentResolution.ToString());
    }
    dropdown.RefreshShownValue();
  }
}

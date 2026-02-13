// Decompiled with JetBrains decompiler
// Type: UiSizeDropdown
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class UiSizeDropdown : MonoBehaviour
{
  private Button button;
  private Dropdown dropdown;
  private List<string> options = new List<string>();

  private void Start()
  {
    this.createDropdownOptions();
    this.renderDropdownValue(this.dropdown);
    // ISSUE: method pointer
    ((UnityEvent<int>) this.dropdown.onValueChanged).AddListener(new UnityAction<int>((object) this, __methodptr(\u003CStart\u003Eb__3_0)));
  }

  private void createDropdownOptions()
  {
    this.dropdown = ((Component) this).GetComponent<Dropdown>();
    this.dropdown.ClearOptions();
    this.options.Clear();
    this.dropdown.AddOptions(this.options);
  }

  private void OnEnable()
  {
    if (!Config.game_loaded)
      return;
    this.dropdown = ((Component) this).GetComponent<Dropdown>();
    this.renderDropdownValue(this.dropdown);
  }

  private void DropdownValueChanged(Dropdown change)
  {
  }

  private void renderDropdownValue(Dropdown dropdown)
  {
    string stringVal = PlayerConfig.dict["ui_size"].stringVal;
    dropdown.value = this.options.IndexOf(stringVal);
    dropdown.RefreshShownValue();
  }
}

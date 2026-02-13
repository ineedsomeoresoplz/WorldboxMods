// Decompiled with JetBrains decompiler
// Type: UiDebugButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class UiDebugButton : MonoBehaviour
{
  public Sprite button_on;
  public Sprite button_off;
  public Text text;
  public Image iconOn;
  public Button button;
  private DebugOption _debug_option;

  public void Awake()
  {
    string name = ((Object) ((Component) this).gameObject.transform).name;
    try
    {
      this._debug_option = (DebugOption) Enum.Parse(typeof (DebugOption), name);
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ("THERE'S NO DEBUG OPTION CALLED " + name));
      throw;
    }
    // ISSUE: method pointer
    ((UnityEvent) this.button.onClick).AddListener(new UnityAction((object) this, __methodptr(click)));
  }

  public void Start()
  {
    this.text.text = ((Object) ((Component) ((Component) this).transform).gameObject).name;
    this.checkButtonGraphics();
  }

  private void OnEnable() => this.checkButtonGraphics();

  private void OnValidate()
  {
    string name = ((Object) ((Component) this).gameObject.transform).name;
    string str = "";
    int num = 0;
    foreach (char c in name)
    {
      if (num == 0)
      {
        str += c.ToString();
      }
      else
      {
        if (char.IsUpper(c))
          str += " ";
        str += c.ToString();
      }
      ++num;
    }
    this.text.text = str;
  }

  public void click()
  {
    DebugConfig.switchOption(this._debug_option);
    this.checkButtonGraphics();
  }

  private void checkButtonGraphics()
  {
    if (DebugConfig.isOn(this._debug_option))
      ((Component) this.button).GetComponent<Image>().sprite = this.button_on;
    else
      ((Component) this.button).GetComponent<Image>().sprite = this.button_off;
  }
}

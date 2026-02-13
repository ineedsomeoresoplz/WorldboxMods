// Decompiled with JetBrains decompiler
// Type: UiAutoTesterButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class UiAutoTesterButton : MonoBehaviour
{
  public Sprite button_on;
  public Sprite button_off;
  public Text text;
  public Button button;
  private string _tester_name;

  public void Awake()
  {
    // ISSUE: method pointer
    ((UnityEvent) this.button.onClick).AddListener(new UnityAction((object) this, __methodptr(click)));
    this._tester_name = ((Object) ((Component) this).gameObject.transform).name;
  }

  public void Start()
  {
    this._tester_name = ((Object) ((Component) this).gameObject.transform).name;
    this.checkButtonGraphics();
  }

  private void OnEnable() => this.checkButtonGraphics();

  private void OnValidate()
  {
    string name = ((Object) ((Component) this).gameObject.transform).name;
    string str = "";
    int num = 0;
    bool flag = true;
    foreach (char c in name)
    {
      if (flag)
      {
        c = char.ToUpper(c);
        flag = false;
      }
      if (num == 0)
      {
        str += c.ToString();
      }
      else
      {
        if (c == '_')
        {
          c = ' ';
          flag = true;
        }
        str += c.ToString();
      }
      ++num;
    }
    this.text.text = str;
  }

  public void click()
  {
    AssetManager.loadAutoTester();
    if (World.world.auto_tester.active_tester == this._tester_name)
    {
      World.world.auto_tester.toggleAutoTester();
    }
    else
    {
      World.world.auto_tester.create(this._tester_name);
      ((Component) World.world.auto_tester).gameObject.SetActive(true);
    }
    this.checkButtonGraphics();
    ScrollWindow.hideAllEvent();
  }

  private void checkButtonGraphics()
  {
    if (World.world.auto_tester.active && World.world.auto_tester.active_tester == this._tester_name)
      ((Component) this.button).GetComponent<Image>().sprite = this.button_on;
    else
      ((Component) this.button).GetComponent<Image>().sprite = this.button_off;
  }
}

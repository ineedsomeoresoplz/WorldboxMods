// Decompiled with JetBrains decompiler
// Type: UiInputCounter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class UiInputCounter : MonoBehaviour
{
  public InputField nameText;

  private void Start()
  {
    // ISSUE: method pointer
    ((UnityEvent<string>) this.nameText.onValueChanged).AddListener(new UnityAction<string>((object) this, __methodptr(\u003CStart\u003Eb__1_0)));
  }

  private void OnEnable()
  {
    if (!Config.game_loaded)
      return;
    this.textChanged();
  }

  public void textChanged()
  {
    Text component = ((Component) this).GetComponent<Text>();
    int num = this.nameText.text.Length;
    string str1 = num.ToString();
    num = this.nameText.characterLimit;
    string str2 = num.ToString();
    string str3 = $"{str1} / {str2}";
    component.text = str3;
  }
}

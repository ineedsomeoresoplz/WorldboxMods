// Decompiled with JetBrains decompiler
// Type: ButtonSfx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
[RequireComponent(typeof (Button))]
public class ButtonSfx : MonoBehaviour
{
  private Button _button;

  private void Start()
  {
    this._button = ((Component) this).GetComponent<Button>();
    // ISSUE: method pointer
    ((UnityEvent) this._button.onClick).AddListener(new UnityAction((object) this, __methodptr(playSound)));
  }

  private void playSound()
  {
    SoundBox.click();
    ((Behaviour) this._button).enabled = false;
    ((Behaviour) this._button).enabled = true;
  }
}

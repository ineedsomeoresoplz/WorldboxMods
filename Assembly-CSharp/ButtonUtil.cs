// Decompiled with JetBrains decompiler
// Type: ButtonUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
[RequireComponent(typeof (Button))]
public class ButtonUtil : MonoBehaviour
{
  private Button _button;

  public void ResetState()
  {
    if (Object.op_Equality((Object) this._button, (Object) null))
    {
      this._button = ((Component) this).GetComponent<Button>();
      // ISSUE: method pointer
      ((UnityEvent) this._button.onClick).AddListener(new UnityAction((object) this, __methodptr(playSound)));
    }
    ((Behaviour) this._button).enabled = false;
    ((Behaviour) this._button).enabled = true;
  }

  private void playSound() => SoundBox.click();
}

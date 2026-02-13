// Decompiled with JetBrains decompiler
// Type: UnfoldButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class UnfoldButton : MonoBehaviour
{
  [SerializeField]
  private Button _button;
  [SerializeField]
  private Text _text;
  private UnfoldAction _action;
  public int offset;

  private void Awake()
  {
    // ISSUE: method pointer
    ((UnityEvent) this._button.onClick).AddListener(new UnityAction((object) this, __methodptr(\u003CAwake\u003Eb__4_0)));
  }

  public void setData(int pCount, int pOffset)
  {
    this.offset = pOffset;
    this.setText(pCount.ToString());
  }

  public void setCallback(UnfoldAction pCallback) => this._action = pCallback;

  public void setText(string pText) => this._text.text = pText;

  public void clear() => this.offset = 0;

  public Button getButton() => this._button;
}

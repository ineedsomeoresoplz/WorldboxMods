// Decompiled with JetBrains decompiler
// Type: UiDebugButtonBatchSize
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class UiDebugButtonBatchSize : MonoBehaviour
{
  [SerializeField]
  private Text _text;
  [SerializeField]
  private Button _button;

  private void Awake()
  {
    // ISSUE: method pointer
    ((UnityEvent) this._button.onClick).AddListener(new UnityAction((object) this, __methodptr(click)));
  }

  public void click()
  {
    ParallelHelper.moveDebugBatchSize();
    this.updateText();
  }

  private void OnEnable() => this.updateText();

  private void updateText() => this._text.text = ParallelHelper.DEBUG_BATCH_SIZE.ToString();
}

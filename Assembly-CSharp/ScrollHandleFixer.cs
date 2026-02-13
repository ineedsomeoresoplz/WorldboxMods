// Decompiled with JetBrains decompiler
// Type: ScrollHandleFixer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class ScrollHandleFixer : MonoBehaviour, ILayoutSelfController, ILayoutController
{
  private const float MIN_SIZE = 0.05f;
  [SerializeField]
  private Scrollbar _bar;
  private bool _bar_updating;

  private void Awake()
  {
    // ISSUE: method pointer
    ((UnityEvent<float>) this._bar.onValueChanged).AddListener(new UnityAction<float>((object) this, __methodptr(\u003CAwake\u003Eb__3_0)));
  }

  private void Update() => this.checkBarSize();

  private void LateUpdate() => this.checkBarSize();

  public void SetLayoutHorizontal() => this.checkBarSize();

  public void SetLayoutVertical() => this.checkBarSize();

  private void checkBarSize()
  {
    if ((double) this._bar.size > 0.05000000074505806)
      return;
    this._bar.size = 0.05f;
  }
}

// Decompiled with JetBrains decompiler
// Type: XtwitterText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class XtwitterText : MonoBehaviour
{
  private Text _text;
  private string[] _strings = new string[6]
  {
    "Twitter",
    "Xwitter",
    "??",
    "X?",
    "X??",
    "X???"
  };
  private int _index;
  private float _timer = 2f;
  private const int INTERVAL = 2;
  private Tweener _current_tween;

  private void Awake() => this._text = ((Component) this).GetComponent<Text>();

  private void Update()
  {
    this._timer -= Time.deltaTime;
    if ((double) this._timer > 0.0)
      return;
    this._timer = 2f;
    this._text.text = this._strings[this._index];
    this._index = (this._index + 1) % this._strings.Length;
    ShortcutExtensions.DOPunchScale(((Component) this).transform, new Vector3(0.2f, 0.2f, 0.2f), 0.3f, 10, 1f);
  }
}

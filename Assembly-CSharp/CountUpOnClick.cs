// Decompiled with JetBrains decompiler
// Type: CountUpOnClick
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using System;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class CountUpOnClick : MonoBehaviour
{
  private const float TWEEN_DURATION = 0.45f;
  [SerializeField]
  private Text _text;
  private Tweener _cur_tween;
  private int _value;
  private string _end = "";
  private bool _value_updated;

  private void Start()
  {
    Button button;
    if (!((Component) this).TryGetComponent<Button>(ref button) || Object.op_Equality((Object) this._text, (Object) null))
      ((Behaviour) this).enabled = false;
    else if (!this._value_updated && !this.checkString())
    {
      ((Behaviour) this).enabled = false;
    }
    else
    {
      // ISSUE: method pointer
      ((UnityEvent) button.onClick).AddListener(new UnityAction((object) this, __methodptr(countAnimation)));
    }
  }

  public void setValue(int pValue, string pEnd = "")
  {
    ((Behaviour) this).enabled = true;
    this._value = pValue;
    this._end = pEnd;
    this._value_updated = true;
    this._text.text = this._value.ToText(4) + pEnd;
  }

  private bool checkString()
  {
    string text = this._text.text;
    if (!this.checkIfStringIsLegit(text))
      return false;
    if (int.TryParse(text, NumberStyles.Any, (IFormatProvider) CultureInfo.CurrentCulture, out this._value))
      return true;
    ((Behaviour) this).enabled = false;
    return false;
  }

  private bool checkIfStringIsLegit(string pString)
  {
    return !string.IsNullOrEmpty(pString) && pString.All<char>(new Func<char, bool>(char.IsDigit));
  }

  private void countAnimation()
  {
    if (this._value_updated)
      this._value_updated = false;
    this.checkDestroyTween();
    this._cur_tween = (Tweener) this._text.DOUpCounter(0, this._value, 0.45f, this._end);
  }

  public Text getText() => this._text;

  private void OnDisable() => this.checkDestroyTween();

  private void checkDestroyTween()
  {
    TweenExtensions.Kill((Tween) this._cur_tween, true);
    this._cur_tween = (Tweener) null;
  }
}

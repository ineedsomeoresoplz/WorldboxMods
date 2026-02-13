// Decompiled with JetBrains decompiler
// Type: TweenExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Globalization;
using UnityEngine.UI;

#nullable disable
public static class TweenExtensions
{
  public static TweenerCore<int, int, NoOptions> DOUpCounter(this Text target, int endValue)
  {
    int result;
    if (!int.TryParse(target.text, NumberStyles.Any, (IFormatProvider) CultureInfo.CurrentCulture, out result))
      result = 0;
    return target.DOUpCounter(result, endValue, 0.45f);
  }

  public static TweenerCore<int, int, NoOptions> DOUpCounter(
    this Text target,
    int endValue,
    float duration,
    string pEnding = "",
    string pColor = "")
  {
    int result;
    if (!int.TryParse(target.text, NumberStyles.Any, (IFormatProvider) CultureInfo.CurrentCulture, out result))
      result = 0;
    return target.DOUpCounter(result, endValue, duration, pEnding, pColor);
  }

  public static TweenerCore<int, int, NoOptions> DOUpCounter(
    this Text target,
    int fromValue,
    int endValue,
    float duration,
    string pEnding = "",
    string pColor = "")
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    TweenExtensions.\u003C\u003Ec__DisplayClass2_0 cDisplayClass20 = new TweenExtensions.\u003C\u003Ec__DisplayClass2_0();
    // ISSUE: reference to a compiler-generated field
    cDisplayClass20.pColor = pColor;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass20.target = target;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass20.pEnding = pEnding;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass20.v = fromValue;
    // ISSUE: method pointer
    // ISSUE: method pointer
    TweenerCore<int, int, NoOptions> tweenerCore = DOTween.To(new DOGetter<int>((object) cDisplayClass20, __methodptr(\u003CDOUpCounter\u003Eb__0)), new DOSetter<int>((object) cDisplayClass20, __methodptr(\u003CDOUpCounter\u003Eb__1)), endValue, duration);
    TweenSettingsExtensions.SetEase<TweenerCore<int, int, NoOptions>>(tweenerCore, (Ease) 12);
    // ISSUE: reference to a compiler-generated field
    TweenSettingsExtensions.SetTarget<TweenerCore<int, int, NoOptions>>(tweenerCore, (object) cDisplayClass20.target);
    return tweenerCore;
  }

  public static TweenerCore<float, float, FloatOptions> DOUpCounter(
    this Text target,
    float fromValue,
    float endValue,
    float duration,
    string pEnding = "",
    string pColor = "")
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    TweenExtensions.\u003C\u003Ec__DisplayClass3_0 cDisplayClass30 = new TweenExtensions.\u003C\u003Ec__DisplayClass3_0();
    // ISSUE: reference to a compiler-generated field
    cDisplayClass30.pColor = pColor;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass30.target = target;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass30.pEnding = pEnding;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass30.v = fromValue;
    // ISSUE: method pointer
    // ISSUE: method pointer
    TweenerCore<float, float, FloatOptions> tweenerCore = DOTween.To(new DOGetter<float>((object) cDisplayClass30, __methodptr(\u003CDOUpCounter\u003Eb__0)), new DOSetter<float>((object) cDisplayClass30, __methodptr(\u003CDOUpCounter\u003Eb__1)), endValue, duration);
    TweenSettingsExtensions.SetEase<TweenerCore<float, float, FloatOptions>>(tweenerCore, (Ease) 14);
    // ISSUE: reference to a compiler-generated field
    TweenSettingsExtensions.SetTarget<TweenerCore<float, float, FloatOptions>>(tweenerCore, (object) cDisplayClass30.target);
    return tweenerCore;
  }

  public static TweenerCore<long, long, NoOptions> DORandomCounter(
    this Text target,
    long fromValue,
    long endValue,
    float duration)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    TweenExtensions.\u003C\u003Ec__DisplayClass4_0 cDisplayClass40 = new TweenExtensions.\u003C\u003Ec__DisplayClass4_0()
    {
      fromValue = fromValue,
      endValue = endValue,
      target = target
    };
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    cDisplayClass40.current = cDisplayClass40.fromValue;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    cDisplayClass40.endLength = cDisplayClass40.endValue.ToString().Length;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    cDisplayClass40.endVal = cDisplayClass40.endValue.ToString();
    // ISSUE: method pointer
    // ISSUE: method pointer
    // ISSUE: reference to a compiler-generated field
    TweenerCore<long, long, NoOptions> tweenerCore = DOTween.To(new DOGetter<long>((object) cDisplayClass40, __methodptr(\u003CDORandomCounter\u003Eb__0)), new DOSetter<long>((object) cDisplayClass40, __methodptr(\u003CDORandomCounter\u003Eb__1)), cDisplayClass40.endValue, duration);
    TweenSettingsExtensions.SetEase<TweenerCore<long, long, NoOptions>>(tweenerCore, (Ease) 12);
    // ISSUE: reference to a compiler-generated field
    TweenSettingsExtensions.SetTarget<TweenerCore<long, long, NoOptions>>(tweenerCore, (object) cDisplayClass40.target);
    return tweenerCore;
  }

  public static TweenerCore<int, int, NoOptions> DORandomCounter(
    this Text target,
    int fromValue,
    int endValue,
    float duration)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    TweenExtensions.\u003C\u003Ec__DisplayClass5_0 cDisplayClass50 = new TweenExtensions.\u003C\u003Ec__DisplayClass5_0()
    {
      fromValue = fromValue,
      endValue = endValue,
      target = target
    };
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    cDisplayClass50.current = cDisplayClass50.fromValue;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    cDisplayClass50.endLength = cDisplayClass50.endValue.ToString().Length;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    cDisplayClass50.endVal = cDisplayClass50.endValue.ToString();
    // ISSUE: method pointer
    // ISSUE: method pointer
    // ISSUE: reference to a compiler-generated field
    TweenerCore<int, int, NoOptions> tweenerCore = DOTween.To(new DOGetter<int>((object) cDisplayClass50, __methodptr(\u003CDORandomCounter\u003Eb__0)), new DOSetter<int>((object) cDisplayClass50, __methodptr(\u003CDORandomCounter\u003Eb__1)), cDisplayClass50.endValue, duration);
    TweenSettingsExtensions.SetEase<TweenerCore<int, int, NoOptions>>(tweenerCore, (Ease) 12);
    // ISSUE: reference to a compiler-generated field
    TweenSettingsExtensions.SetTarget<TweenerCore<int, int, NoOptions>>(tweenerCore, (object) cDisplayClass50.target);
    return tweenerCore;
  }

  public static TweenerCore<float, float, FloatOptions> DOMinHeight(
    this LayoutElement target,
    float endValue,
    float duration,
    bool snapping = false)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    TweenExtensions.\u003C\u003Ec__DisplayClass6_0 cDisplayClass60 = new TweenExtensions.\u003C\u003Ec__DisplayClass6_0();
    // ISSUE: reference to a compiler-generated field
    cDisplayClass60.target = target;
    // ISSUE: method pointer
    // ISSUE: method pointer
    TweenerCore<float, float, FloatOptions> tweenerCore = DOTween.To(new DOGetter<float>((object) cDisplayClass60, __methodptr(\u003CDOMinHeight\u003Eb__0)), new DOSetter<float>((object) cDisplayClass60, __methodptr(\u003CDOMinHeight\u003Eb__1)), endValue, duration);
    // ISSUE: reference to a compiler-generated field
    TweenSettingsExtensions.SetTarget<Tweener>(TweenSettingsExtensions.SetOptions(tweenerCore, snapping), (object) cDisplayClass60.target);
    return tweenerCore;
  }

  public static TweenerCore<float, float, FloatOptions> DOPreferredHeight(
    this LayoutElement target,
    float endValue,
    float duration,
    bool snapping = false)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    TweenExtensions.\u003C\u003Ec__DisplayClass7_0 cDisplayClass70 = new TweenExtensions.\u003C\u003Ec__DisplayClass7_0();
    // ISSUE: reference to a compiler-generated field
    cDisplayClass70.target = target;
    // ISSUE: method pointer
    // ISSUE: method pointer
    TweenerCore<float, float, FloatOptions> tweenerCore = DOTween.To(new DOGetter<float>((object) cDisplayClass70, __methodptr(\u003CDOPreferredHeight\u003Eb__0)), new DOSetter<float>((object) cDisplayClass70, __methodptr(\u003CDOPreferredHeight\u003Eb__1)), endValue, duration);
    // ISSUE: reference to a compiler-generated field
    TweenSettingsExtensions.SetTarget<Tweener>(TweenSettingsExtensions.SetOptions(tweenerCore, snapping), (object) cDisplayClass70.target);
    return tweenerCore;
  }
}

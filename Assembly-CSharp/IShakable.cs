// Decompiled with JetBrains decompiler
// Type: IShakable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using DG.Tweening;
using UnityEngine;

#nullable disable
public interface IShakable
{
  float shake_duration { get; }

  float shake_strength { get; }

  Tweener shake_tween { get; set; }

  Transform transform { get; }

  void shake()
  {
    this.killShakeTween();
    this.shake_tween = ShortcutExtensions.DOShakePosition(this.transform, this.shake_duration, this.shake_strength, 10, 90f, false, true, (ShakeRandomnessMode) 0);
  }

  void killShakeTween() => TweenExtensions.Kill((Tween) this.shake_tween, true);
}

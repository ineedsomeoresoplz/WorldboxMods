// Decompiled with JetBrains decompiler
// Type: FadeOutDelayed
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FadeOutDelayed : MonoBehaviour
{
  [SerializeField]
  private CanvasGroup _group;
  [SerializeField]
  private float _duration;
  [SerializeField]
  private float _delay;
  [SerializeField]
  [Range(0.0f, 1f)]
  private float _max_alpha = 1f;
  [SerializeField]
  [Range(0.0f, 1f)]
  private float _min_alpha;
  private float _time_left;
  private float _delay_time_left;

  private void OnEnable() => this.reset();

  private void Update()
  {
    this._delay_time_left -= Time.deltaTime;
    if ((double) this._delay_time_left > 0.0 || (double) this._time_left <= 0.0)
      return;
    this._time_left -= Time.deltaTime;
    this._group.alpha = Mathf.Lerp(this._min_alpha, this._max_alpha, this._time_left / this._duration);
  }

  private void reset()
  {
    this._delay_time_left = this._delay;
    this._time_left = this._duration;
    this._group.alpha = this._max_alpha;
  }
}

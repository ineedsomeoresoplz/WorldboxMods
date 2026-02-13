// Decompiled with JetBrains decompiler
// Type: FadeInOutAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class FadeInOutAnimation : MonoBehaviour
{
  private const float FADE_OUT_BOUND = 0.1f;
  private const float FADE_SPEED = 0.015f;
  private const float INTERVAL = 0.025f;
  public float alpha_max = 1f;
  private float _current_alpha;
  private float _timer = 0.025f;
  private bool _fade_out = true;
  [SerializeField]
  private Image _image;

  public void Awake() => this.checkInit();

  public void checkInit() => this._image = ((Component) this).GetComponent<Image>();

  private void updateAlpha()
  {
    this._timer -= Time.deltaTime;
    if ((double) this._timer >= 0.0)
      return;
    this._timer = 0.025f;
    if (this._fade_out)
    {
      this._current_alpha -= 0.015f;
      if ((double) this._current_alpha <= 0.10000000149011612)
      {
        this._current_alpha = 0.1f;
        this._fade_out = false;
      }
    }
    else
    {
      this._current_alpha += 0.015f;
      if ((double) this._current_alpha >= (double) this.alpha_max)
      {
        this._current_alpha = this.alpha_max;
        this._fade_out = true;
      }
    }
    Color color = ((Graphic) this._image).color;
    color.a = this._current_alpha;
    ((Graphic) this._image).color = color;
  }

  public void resetToFadeOut()
  {
    this._current_alpha = 1f;
    this._fade_out = true;
    this.updateAlpha();
  }

  public void resetToFadeIn()
  {
    this._current_alpha = 0.0f;
    this._fade_out = false;
    this.updateAlpha();
  }

  public void reset() => this.resetToFadeOut();

  private void OnEnable() => this.reset();

  private void Update() => this.updateAlpha();
}

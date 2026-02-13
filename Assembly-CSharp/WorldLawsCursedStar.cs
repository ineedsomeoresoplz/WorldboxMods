// Decompiled with JetBrains decompiler
// Type: WorldLawsCursedStar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class WorldLawsCursedStar : MonoBehaviour
{
  [SerializeField]
  private Image _empty_star;
  [SerializeField]
  private Image _filled_star;
  [SerializeField]
  private Sprite _filled_star_sprite;
  [SerializeField]
  private Sprite _egg_sprite;
  private bool _filled;

  public void setStarsTransparency(float pValue)
  {
    float num = 1f - pValue;
    Color color = ((Graphic) this._empty_star).color;
    color.a = num;
    ((Graphic) this._empty_star).color = color;
    color.a = pValue;
    ((Graphic) this._filled_star).color = color;
  }

  public void setColorMultiplyAlphaBoth(Color pColor, float pValue)
  {
    if ((double) pValue < 0.0)
      pValue = 0.0f;
    pColor.a = ((Graphic) this._empty_star).color.a * pValue;
    ((Graphic) this._empty_star).color = pColor;
    pColor.a = ((Graphic) this._filled_star).color.a * pValue;
    ((Graphic) this._filled_star).color = pColor;
  }

  public void toggleEgg(bool pState)
  {
    if (pState)
      this._filled_star.sprite = this._egg_sprite;
    else
      this._filled_star.sprite = this._filled_star_sprite;
  }

  public void toggleFilled(bool pState) => this._filled = pState;

  public bool isFilled() => this._filled;
}

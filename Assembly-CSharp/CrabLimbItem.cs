// Decompiled with JetBrains decompiler
// Type: CrabLimbItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (SpriteRenderer))]
public class CrabLimbItem : MonoBehaviour
{
  public CrabLimb crabLimb;
  public Sprite high_hp;
  public Sprite med_hp;
  public Sprite low_hp;
  internal SpriteRenderer _sprite_renderer;
  private Color _shade;
  private Color _dmg = new Color(1f, 0.0f, 0.0f, 1f);

  private void Awake()
  {
    this._sprite_renderer = ((Component) this).GetComponent<SpriteRenderer>();
    this._sprite_renderer.sprite = this.high_hp;
    this._shade = this._sprite_renderer.color;
  }

  internal void stateChange(CrabLimbState pState)
  {
    switch (pState)
    {
      case CrabLimbState.HighHP:
        this._sprite_renderer.sprite = this.high_hp;
        break;
      case CrabLimbState.MedHP:
        this._sprite_renderer.sprite = this.med_hp;
        break;
      case CrabLimbState.LowHP:
        this._sprite_renderer.sprite = this.low_hp;
        break;
    }
    this._sprite_renderer.color = this._dmg;
  }

  internal void flicker(float pProgress)
  {
    this._sprite_renderer.color = Color.Lerp(this._dmg, this._shade, pProgress);
  }
}

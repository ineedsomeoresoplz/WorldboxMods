// Decompiled with JetBrains decompiler
// Type: PossessionModeButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class PossessionModeButton : MonoBehaviour
{
  public PossessionActionMode mode;
  [SerializeField]
  private Image _image_icon;
  [SerializeField]
  private Image _image_background;

  public void updateGraphics(PossessionActionMode pCurrentSelectedMode)
  {
    if (this.mode == pCurrentSelectedMode)
    {
      ((Graphic) this._image_icon).color = Color.white;
      ((Graphic) this._image_background).color = Color.white;
    }
    else
    {
      ((Graphic) this._image_icon).color = new Color(0.3f, 0.3f, 0.3f, 1f);
      ((Graphic) this._image_background).color = Color.gray;
    }
  }
}

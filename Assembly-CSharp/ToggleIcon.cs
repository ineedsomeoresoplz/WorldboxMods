// Decompiled with JetBrains decompiler
// Type: ToggleIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class ToggleIcon : MonoBehaviour
{
  public Sprite spriteON;
  public Sprite spriteOFF;
  private Image image;

  private void Awake() => this.image = ((Component) this).GetComponent<Image>();

  internal void updateIcon(bool pEnabled)
  {
    if (Object.op_Equality((Object) this.image, (Object) null))
      this.image = ((Component) this).GetComponent<Image>();
    if (pEnabled)
      this.image.sprite = this.spriteON;
    else
      this.image.sprite = this.spriteOFF;
  }

  internal void updateIconMultiToggle(bool pActive, bool pEnabled)
  {
    if (Object.op_Equality((Object) this.image, (Object) null))
      this.image = ((Component) this).GetComponent<Image>();
    if (pActive)
      ((Component) this.image).gameObject.SetActive(true);
    else
      ((Component) this.image).gameObject.SetActive(false);
    if (pActive)
      ((Graphic) this.image).color = Color.white;
    else if (pEnabled)
      ((Graphic) this.image).color = Color32.op_Implicit(Toolbox.color_grey);
    else
      ((Graphic) this.image).color = Color.white;
    if (pEnabled)
      this.image.sprite = this.spriteON;
    else
      this.image.sprite = this.spriteOFF;
  }
}

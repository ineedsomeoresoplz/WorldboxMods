// Decompiled with JetBrains decompiler
// Type: SwitchButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class SwitchButton : MonoBehaviour
{
  public Color color_on = Color.white;
  public Color color_off = Color.gray;
  public Text text;
  public Image icon;

  public void setEnabled(bool pValue)
  {
    if (pValue)
      ((Component) this).GetComponent<CanvasGroup>().alpha = 1f;
    else
      ((Component) this).GetComponent<CanvasGroup>().alpha = 0.5f;
  }
}

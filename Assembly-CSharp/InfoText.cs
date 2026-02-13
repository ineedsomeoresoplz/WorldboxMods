// Decompiled with JetBrains decompiler
// Type: InfoText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class InfoText : MonoBehaviour
{
  public TextMesh text;
  public TextMesh shadow;

  private void Start()
  {
    ((Component) this.text).gameObject.GetComponent<Renderer>().sortingOrder = 1000;
    ((Component) this.shadow).gameObject.GetComponent<Renderer>().sortingOrder = 999;
  }

  public void setText(string pText)
  {
    this.text.text = pText;
    this.shadow.text = pText;
  }
}

// Decompiled with JetBrains decompiler
// Type: QuantumSpriteWithText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class QuantumSpriteWithText : QuantumSprite
{
  public TextMesh text;

  public void initText()
  {
    this.text = ((Component) ((Component) this).transform.Find("Text"))?.GetComponent<TextMesh>();
  }
}

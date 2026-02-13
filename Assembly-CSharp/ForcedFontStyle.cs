// Decompiled with JetBrains decompiler
// Type: ForcedFontStyle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class ForcedFontStyle
{
  public FontStyle style;
  public bool shadow;

  public ForcedFontStyle(FontStyle pStyle, bool pShadow = false)
  {
    this.style = pStyle;
    this.shadow = pShadow;
  }
}

// Decompiled with JetBrains decompiler
// Type: ColorArray
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ColorArray
{
  public List<Color32> colors;

  public ColorArray(float pR, float pG, float pB, float pA, float pAmount, float pMod = 1f)
  {
    this.colors = new List<Color32>();
    for (int index = 0; (double) index < (double) pAmount; ++index)
    {
      float num = index <= 0 ? 0.0f : 1f / pAmount * (float) index;
      Color color;
      // ISSUE: explicit constructor call
      ((Color) ref color).\u002Ector(pR, pG, pB, num * 1f * pMod);
      this.colors.Add(Color32.op_Implicit(color));
    }
  }

  public ColorArray(Color32 pColor, int pAmount)
    : this((float) pColor.r, (float) pColor.g, (float) pColor.b, (float) pColor.a, (float) pAmount)
  {
  }
}

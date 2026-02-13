// Decompiled with JetBrains decompiler
// Type: RectExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class RectExtensions
{
  public static Rect Resize(this Rect pRect, float pMultiplier)
  {
    float num1 = ((Rect) ref pRect).width * pMultiplier;
    float num2 = ((Rect) ref pRect).height * pMultiplier;
    float num3 = (float) (((double) ((Rect) ref pRect).width - (double) num1) / 2.0);
    float num4 = (float) (((double) ((Rect) ref pRect).height - (double) num2) / 2.0);
    ((Rect) ref pRect).width = num1;
    ((Rect) ref pRect).height = num2;
    ref Rect local1 = ref pRect;
    ((Rect) ref local1).x = ((Rect) ref local1).x + num3;
    ref Rect local2 = ref pRect;
    ((Rect) ref local2).y = ((Rect) ref local2).y + num4;
    return pRect;
  }
}

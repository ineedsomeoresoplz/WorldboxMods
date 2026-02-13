// Decompiled with JetBrains decompiler
// Type: Pixel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public struct Pixel(int pX, int pY, Color32 pColor)
{
  public readonly int x = pX;
  public readonly int y = pY;
  public readonly Color32 color = pColor;
}

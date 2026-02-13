// Decompiled with JetBrains decompiler
// Type: BuildingFundament
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public class BuildingFundament
{
  public readonly int left;
  public readonly int right;
  public readonly int top;
  public readonly int bottom;
  public readonly int width;
  public readonly int height;

  public BuildingFundament(int pLeft, int pRight, int pTop, int pBottom)
  {
    this.left = pLeft;
    this.right = pRight;
    this.top = pTop;
    this.bottom = pBottom;
    this.width = this.right + this.left + 1;
    this.height = this.top + this.bottom + 1;
  }
}

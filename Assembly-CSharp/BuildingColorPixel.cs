// Decompiled with JetBrains decompiler
// Type: BuildingColorPixel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public readonly struct BuildingColorPixel(
  Color32 pColor,
  Color32 pColorAbandoned,
  Color32 pColorRuin)
{
  public readonly Color32 color = pColor;
  public readonly Color32 color_abandoned = pColorAbandoned;
  public readonly Color32 color_ruin = pColorRuin;
}

// Decompiled with JetBrains decompiler
// Type: TooltipDirection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;

#nullable disable
[Flags]
public enum TooltipDirection
{
  None = 0,
  Up = 1,
  Down = 2,
  Right = 4,
  Left = 8,
  MagnetUp = 16, // 0x00000010
  MagnetDown = 32, // 0x00000020
  MagnetRight = 64, // 0x00000040
  MagnetLeft = 128, // 0x00000080
}

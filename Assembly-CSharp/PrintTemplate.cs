// Decompiled with JetBrains decompiler
// Type: PrintTemplate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class PrintTemplate
{
  public string name;
  public Texture2D graphics;
  internal PrintStep[] steps;
  internal int steps_per_tick = 1;
}

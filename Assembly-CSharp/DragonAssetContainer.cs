// Decompiled with JetBrains decompiler
// Type: DragonAssetContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class DragonAssetContainer
{
  public string name;
  public DragonState id;
  public Sprite[] frames;
  public DragonState[] states;
  public float speed = 0.1f;
}

// Decompiled with JetBrains decompiler
// Type: UnitSpriteAnimationData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class UnitSpriteAnimationData
{
  public string name;
  public Vector3 head;
  public Vector3 item;
  public Vector3 backpack;
  public bool showHead;
  public bool showItem;

  public UnitSpriteAnimationData()
  {
    this.head = new Vector3();
    this.head = new Vector3();
    this.backpack = new Vector3();
  }
}

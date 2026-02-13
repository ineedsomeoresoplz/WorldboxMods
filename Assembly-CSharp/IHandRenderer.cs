// Decompiled with JetBrains decompiler
// Type: IHandRenderer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public interface IHandRenderer
{
  Sprite[] getSprites();

  bool is_colored { get; }

  bool is_animated { get; }

  string getID() => (this as Asset).id;
}

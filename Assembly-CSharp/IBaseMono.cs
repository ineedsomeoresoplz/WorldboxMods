// Decompiled with JetBrains decompiler
// Type: IBaseMono
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public interface IBaseMono
{
  Transform transform { get; }

  GameObject gameObject { get; }

  T GetComponent<T>();

  T AddComponent<T>() where T : Component => this.gameObject.AddComponent<T>();

  bool HasComponent<T>() where T : Component => this.gameObject.HasComponent<T>();
}

// Decompiled with JetBrains decompiler
// Type: IDraggable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
public interface IDraggable : IEndDragHandler, IEventSystemHandler
{
  Transform transform { get; }

  bool spawn_particles_on_drag { get; }

  bool HasComponent<T>() => ((Component) this.transform).HasComponent<T>();

  void KillDrag();
}

// Decompiled with JetBrains decompiler
// Type: GameObjectExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class GameObjectExtensions
{
  public static T AddOrGetComponent<T>(this GameObject pGameObject) where T : Component
  {
    T obj;
    return !pGameObject.TryGetComponent<T>(ref obj) ? pGameObject.AddComponent<T>() : obj;
  }

  public static bool HasComponent<T>(this GameObject pGameObject)
  {
    T obj;
    return pGameObject.TryGetComponent<T>(ref obj);
  }

  public static bool HasComponent<T>(this Component pComponent)
  {
    return pComponent.gameObject.HasComponent<T>();
  }

  public static T AddComponent<T>(this Component pComponent) where T : Component
  {
    return pComponent.gameObject.AddComponent<T>();
  }
}

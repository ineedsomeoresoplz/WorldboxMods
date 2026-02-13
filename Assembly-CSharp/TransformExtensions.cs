// Decompiled with JetBrains decompiler
// Type: TransformExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public static class TransformExtensions
{
  public static Transform FindRecursive(this Transform pTransform, string pName)
  {
    return pTransform.FindRecursive((Func<Transform, bool>) (tChild => ((Object) tChild).name == pName));
  }

  public static Transform FindRecursive(this Transform pTransform, Func<Transform, bool> pSelector)
  {
    foreach (Transform pTransform1 in pTransform)
    {
      if (pSelector(pTransform1))
        return pTransform1;
      Transform recursive = pTransform1.FindRecursive(pSelector);
      if (Object.op_Inequality((Object) recursive, (Object) null))
        return recursive;
    }
    return (Transform) null;
  }

  public static T[] FindAllRecursive<T>(this Transform pTransform)
  {
    return pTransform.FindAllRecursive<T>((Func<Transform, bool>) (p => ((Component) p).HasComponent<T>()));
  }

  public static T[] FindAllRecursive<T>(this Transform pTransform, Func<Transform, bool> pSelector)
  {
    using (ListPool<T> list = new ListPool<T>())
    {
      foreach (Transform transform in pTransform)
      {
        if (pSelector(transform) && ((Component) transform).HasComponent<T>())
          list.Add(((Component) transform).GetComponent<T>());
        T[] allRecursive = transform.FindAllRecursive<T>(pSelector);
        if (allRecursive != null)
          list.AddRange(allRecursive);
      }
      return list.ToArray<T>();
    }
  }

  public static Transform FindParentWithName(this Transform pChildObject, params string[] pNames)
  {
    Transform parentWithName = (Transform) null;
    foreach (string pName in pNames)
    {
      parentWithName = pChildObject.FindParentWithName(pName);
      if (Object.op_Inequality((Object) parentWithName, (Object) null))
        break;
    }
    return parentWithName;
  }

  public static Transform FindParentWithName(this Transform pChildObject, string pName)
  {
    for (Transform transform = pChildObject; Object.op_Inequality((Object) transform.parent, (Object) null); transform = ((Component) transform.parent).transform)
    {
      if (((Object) ((Component) transform.parent).gameObject).name == pName)
        return transform.parent;
    }
    return (Transform) null;
  }

  public static int GetActiveSiblingIndex(this Transform pTransform)
  {
    int activeSiblingIndex = 0;
    Transform parent = pTransform.parent;
    int num = 0;
    for (int childCount = parent.childCount; num < childCount; ++num)
    {
      Transform child = parent.GetChild(num);
      if (((Component) child).gameObject.activeSelf)
      {
        if (Object.op_Equality((Object) child, (Object) pTransform))
          return activeSiblingIndex;
        ++activeSiblingIndex;
      }
    }
    return -1;
  }

  public static int CountActiveChildren(this Transform pTransform)
  {
    int num1 = 0;
    int num2 = 0;
    for (int childCount = pTransform.childCount; num2 < childCount; ++num2)
    {
      if (((Component) pTransform.GetChild(num2)).gameObject.activeSelf)
        ++num1;
    }
    return num1;
  }

  public static int CountChildren(this Transform pTransform, Func<Transform, bool> pSelector)
  {
    int num1 = 0;
    int num2 = 0;
    for (int childCount = pTransform.childCount; num2 < childCount; ++num2)
    {
      if (pSelector(pTransform.GetChild(num2)))
        ++num1;
    }
    return num1;
  }
}

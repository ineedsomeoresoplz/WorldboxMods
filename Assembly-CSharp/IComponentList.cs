// Decompiled with JetBrains decompiler
// Type: IComponentList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public interface IComponentList
{
  ListPool<NanoObject> getElements();

  void setShowAll();

  void setShowFavoritesOnly();

  void setShowDeadOnly();

  void setShowAliveOnly();

  void setDefault();

  void init(
    GameObject pNoItems,
    SortingTab pSortingTab,
    GameObject pListElementPrefab,
    Transform pListTransform,
    ScrollRect pScrollRect,
    Text pTitleCounter,
    Text pFavoritesCounter,
    Text pDeadCounter);
}

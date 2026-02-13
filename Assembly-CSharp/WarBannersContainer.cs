// Decompiled with JetBrains decompiler
// Type: WarBannersContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class WarBannersContainer : WarElement
{
  private ObjectPoolGenericMono<KingdomBanner> pool_elements;
  [SerializeField]
  private KingdomBanner _prefab;
  [SerializeField]
  private Transform _container;

  protected override void Awake()
  {
    this.pool_elements = new ObjectPoolGenericMono<KingdomBanner>(this._prefab, this._container);
    base.Awake();
    ((Component) this._prefab).gameObject.SetActive(false);
  }

  protected override void clear()
  {
    this.pool_elements.clear();
    base.clear();
  }

  protected IEnumerator showBanner(Kingdom pKingdom, bool pLeft = false, bool pWinner = false, bool pLoser = false)
  {
    WarBannersContainer bannersContainer = this;
    if (!pKingdom.isRekt())
    {
      yield return (object) new WaitForSecondsRealtime(0.025f);
      if (!pKingdom.isRekt())
      {
        bannersContainer.track_objects.Add((NanoObject) pKingdom);
        KingdomBanner next = bannersContainer.pool_elements.getNext();
        if (!((Component) next).HasComponent<DraggableLayoutElement>())
          ((Component) next).AddComponent<DraggableLayoutElement>();
        next.load((NanoObject) pKingdom);
        if (pLeft)
          next.hasLeftWar();
        if (pWinner)
          next.hasWon();
        if (pLoser)
          next.hasLost();
      }
    }
  }

  protected override void clearInitial()
  {
    for (int index = 0; index < this._container.childCount; ++index)
      Object.Destroy((Object) ((Component) this._container.GetChild(index)).gameObject);
    base.clearInitial();
  }
}

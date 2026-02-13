// Decompiled with JetBrains decompiler
// Type: InterestingPeopleElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class InterestingPeopleElement : MonoBehaviour
{
  private ObjectPoolGenericMono<PrefabUnitElement> _pool_elements;
  [SerializeField]
  private PrefabUnitElement _element;
  [SerializeField]
  private Text _counter;
  [SerializeField]
  private Transform _grid;

  private void Awake()
  {
    this._pool_elements = new ObjectPoolGenericMono<PrefabUnitElement>(this._element, this._grid);
    for (int index = 0; index < this._grid.childCount; ++index)
      Object.DestroyImmediate((Object) ((Component) this._grid.GetChild(index)).gameObject);
  }

  public void show(Actor pActor, int pValue)
  {
    this.showMember(pActor);
    this._counter.text = pValue.ToString();
  }

  private void showMember(Actor pActor)
  {
    PrefabUnitElement next = this._pool_elements.getNext();
    ((Component) next).transform.localScale = new Vector3(0.9f, 0.9f, 1f);
    next.show(pActor);
  }

  private void OnDisable() => this._pool_elements.clear();
}

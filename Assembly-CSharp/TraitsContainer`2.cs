// Decompiled with JetBrains decompiler
// Type: TraitsContainer`2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using LayoutGroupExt;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class TraitsContainer<TTrait, TTraitButton> : 
  MonoBehaviour,
  ITraitsContainer<TTrait, TTraitButton>
  where TTrait : BaseTrait<TTrait>
  where TTraitButton : TraitButton<TTrait>
{
  [SerializeField]
  private TTraitButton _prefab_trait;
  [SerializeField]
  private Transform _regular_title;
  [SerializeField]
  private Transform _unlocked_title;
  [SerializeField]
  private Transform _grid;
  private LayoutGroupExtended _layout_grid;
  private ObjectPoolGenericMono<TTraitButton> _pool_traits;
  private ITraitWindow<TTrait, TTraitButton> _trait_window;
  private Dictionary<TTrait, TTraitButton> _traits = new Dictionary<TTrait, TTraitButton>();

  private void Awake()
  {
    this._trait_window = ((Component) this).GetComponentInParent<ITraitWindow<TTrait, TTraitButton>>();
    this._pool_traits = new ObjectPoolGenericMono<TTraitButton>(this._prefab_trait, this._grid);
    this._layout_grid = ((Component) this._grid).GetComponent<LayoutGroupExtended>();
    ((Component) this._grid).gameObject.AddOrGetComponent<TraitsGrid>().on_change = new OnChange(this.sortTraits);
  }

  private void OnEnable()
  {
    if (Object.op_Inequality((Object) this._regular_title, (Object) null))
    {
      if (((Component) this._unlocked_title).gameObject.activeSelf)
        ((Component) this._regular_title).gameObject.SetActive(false);
      else
        ((Component) this._regular_title).gameObject.SetActive(true);
    }
    this.StartCoroutine(this.loadActiveTraits());
  }

  private void OnDisable()
  {
    this._traits.Clear();
    this._pool_traits.clear();
  }

  public void reloadTraits(bool pAnimated)
  {
    this.StopAllCoroutines();
    this.StartCoroutine(this.loadActiveTraits(pAnimated));
  }

  protected IEnumerator loadActiveTraits(bool pAnimated = true)
  {
    using (ListPool<TTrait> listPool = new ListPool<TTrait>((IEnumerable<TTrait>) this._trait_window.getTraits()))
    {
      this._traits.Clear();
      this._pool_traits.clear();
      using (ListPool<TTrait>.Enumerator enumerator = listPool.GetEnumerator())
      {
        while (enumerator.MoveNext())
          this.loadActiveTrait(enumerator.Current);
        yield break;
      }
    }
  }

  private void loadActiveTrait(TTrait pTraitAsset)
  {
    TTraitButton next = this._pool_traits.getNext();
    next.load(pTraitAsset);
    this._traits[pTraitAsset] = next;
    AugmentationUnlockedAction pAction = new AugmentationUnlockedAction(((IAugmentationsEditor) this._trait_window.getEditor()).reloadButtons);
    next.removeElementUnlockedAction(pAction);
    next.addElementUnlockedAction(pAction);
    ITraitsEditor<TTrait> editor = this._trait_window.getEditor();
    next.removeClickAction(new AugmentationButtonClickAction(editor.scrollToGroupStarter));
    next.addClickAction(new AugmentationButtonClickAction(editor.scrollToGroupStarter));
  }

  public void sortTraits()
  {
    using (ListPool<TTrait> pTraits = new ListPool<TTrait>((ICollection<TTrait>) this._traits.Keys))
    {
      pTraits.Sort((Comparison<TTrait>) ((a, b) => ((Component) (object) this._traits[a]).transform.GetSiblingIndex().CompareTo(((Component) (object) this._traits[b]).transform.GetSiblingIndex())));
      this._trait_window.sortTraits((IReadOnlyCollection<TTrait>) pTraits);
    }
  }

  public ObjectPoolGenericMono<TTraitButton> getTraitPool() => this._pool_traits;

  public IReadOnlyCollection<TTraitButton> getTraitButtons()
  {
    return (IReadOnlyCollection<TTraitButton>) this._traits.Values;
  }
}

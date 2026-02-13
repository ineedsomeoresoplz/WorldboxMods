// Decompiled with JetBrains decompiler
// Type: SelectedContainerTraits`4
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SelectedContainerTraits<TTrait, TTraitButton, TTraitContainer, TTraitEditor> : 
  SelectedElementBase<TTraitButton>,
  ISelectedContainerTrait
  where TTrait : BaseTrait<TTrait>
  where TTraitButton : TraitButton<TTrait>
  where TTraitContainer : ITraitsContainer<TTrait, TTraitButton>
  where TTraitEditor : ITraitsEditor<TTrait>
{
  [SerializeField]
  private TTraitButton _prefab_trait;

  protected virtual MetaType meta_type { get; }

  protected string window_id => AssetManager.meta_type_library.getAsset(this.meta_type).window_name;

  private void Awake()
  {
    this._pool = new ObjectPoolGenericMono<TTraitButton>(this._prefab_trait, this._grid);
    ((Component) this._grid).gameObject.AddOrGetComponent<TraitsGrid>();
  }

  public void update(NanoObject pNano) => this.refresh(pNano);

  protected override void refresh(NanoObject pNano)
  {
    this.clear();
    foreach (TTrait trait in (IEnumerable<TTrait>) this.getTraits())
      this.addButton(trait);
  }

  private void addButton(TTrait pObject)
  {
    TTraitButton next = this._pool.getNext();
    next.load(pObject);
    next.removeClickAction(new AugmentationButtonClickAction(this.showTraitsTabAndScroll));
    next.addClickAction(new AugmentationButtonClickAction(this.showTraitsTabAndScroll));
  }

  private void showTraitsTabAndScroll(GameObject pButton)
  {
    if (!this.canEditTraits())
      return;
    TTraitButton component = pButton.GetComponent<TTraitButton>();
    if (!InputHelpers.mouseSupported && !Tooltip.isShowingFor((object) component))
      return;
    ScrollWindow.showWindow(this.window_id);
    World.world.StartCoroutine(this.showTraitsTabAndScrollRoutine(component));
  }

  private IEnumerator showTraitsTabAndScrollRoutine(TTraitButton pTraitButton)
  {
    ScrollWindow tWindow = ScrollWindow.getCurrentWindow();
    TTraitEditor tEditor = ((Component) tWindow).GetComponentInChildren<TTraitEditor>(true);
    WindowMetaTab editorTab = tEditor.getEditorTab();
    if (Object.op_Inequality((Object) editorTab.container.getActiveTab(), (Object) editorTab))
    {
      editorTab.container.showTab(editorTab);
      yield return (object) new WaitForSeconds(Config.getScrollToGroupDelay());
    }
    foreach (TTraitButton traitButton in (IEnumerable<TTraitButton>) ((Component) tWindow).GetComponentInChildren<TTraitContainer>().getTraitButtons())
    {
      if ((object) traitButton.getElementAsset() == (object) pTraitButton.getElementAsset())
      {
        tEditor.scrollToGroupStarter(((Component) (object) traitButton).gameObject, true);
        break;
      }
    }
  }

  protected virtual IReadOnlyCollection<TTrait> getTraits() => throw new NotImplementedException();

  protected virtual bool canEditTraits() => throw new NotImplementedException();

  Transform ISelectedContainerTrait.get_transform() => ((Component) this).transform;
}

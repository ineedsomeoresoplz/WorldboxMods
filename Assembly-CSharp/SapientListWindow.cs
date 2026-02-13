// Decompiled with JetBrains decompiler
// Type: SapientListWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class SapientListWindow : ListWindow
{
  [SerializeField]
  private WindowMetaTab _tab_sapients;
  [SerializeField]
  private WindowMetaTab _tab_non_sapients;
  [SerializeField]
  private Text _sapient_counter;
  [SerializeField]
  private Text _non_sapient_counter;

  protected override void initComponent(IComponentList pComponent)
  {
    base.initComponent(pComponent);
    ISapientListComponent sapientListComponent = (ISapientListComponent) pComponent;
    sapientListComponent.setSapientCounter(this._sapient_counter);
    sapientListComponent.setNonSapientCounter(this._non_sapient_counter);
  }

  protected override void initTabsCallbacks(IComponentList pComponent)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    SapientListWindow.\u003C\u003Ec__DisplayClass5_0 cDisplayClass50 = new SapientListWindow.\u003C\u003Ec__DisplayClass5_0();
    base.initTabsCallbacks(pComponent);
    ISapientListComponent sapientListComponent = (ISapientListComponent) pComponent;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass50.tNoItems = this.getNoItems();
    this.setTabCallbacks(this._tab_sapients, new Action(sapientListComponent.setShowSapientOnly));
    // ISSUE: method pointer
    this._tab_sapients.tab_action.AddListener(new UnityAction<WindowMetaTab>((object) cDisplayClass50, __methodptr(\u003CinitTabsCallbacks\u003Eb__0)));
    this.setTabCallbacks(this._tab_non_sapients, new Action(sapientListComponent.setShowNonSapientOnly));
    // ISSUE: method pointer
    this._tab_non_sapients.tab_action.AddListener(new UnityAction<WindowMetaTab>((object) cDisplayClass50, __methodptr(\u003CinitTabsCallbacks\u003Eb__1)));
  }
}

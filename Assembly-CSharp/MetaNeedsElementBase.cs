// Decompiled with JetBrains decompiler
// Type: MetaNeedsElementBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class MetaNeedsElementBase : WindowMetaElementBase, IRefreshElement
{
  [SerializeField]
  private GameObject _container;
  [SerializeField]
  private Text _text;
  private Actor _actor;
  private IMetaWindow _window;

  protected override void Awake()
  {
    base.Awake();
    this.setupTooltip();
    this.setupClickAction();
    this._window = ((Component) this).GetComponentInParent<IMetaWindow>();
  }

  protected override IEnumerator showContent()
  {
    if (this._window.getCoreObject() is IMetaObject coreObject && coreObject.isAlive())
    {
      Actor pActorResult;
      string text = this.getText(coreObject, out pActorResult);
      this._text.text = text;
      this._actor = pActorResult;
      if (!string.IsNullOrEmpty(text))
      {
        this._container.gameObject.SetActive(true);
        yield break;
      }
    }
  }

  protected virtual string getText(IMetaObject pMeta, out Actor pActorResult)
  {
    throw new NotImplementedException();
  }

  private void setupTooltip()
  {
    TipButton tipButton;
    if (!this._container.TryGetComponent<TipButton>(ref tipButton))
      return;
    tipButton.hoverAction = new TooltipAction(this.tooltipAction);
  }

  private void setupClickAction()
  {
    Button button;
    if (!this._container.TryGetComponent<Button>(ref button))
      return;
    // ISSUE: method pointer
    ((UnityEvent) button.onClick).AddListener(new UnityAction((object) this, __methodptr(buttonAction)));
  }

  private void tooltipAction()
  {
    if (this._actor.isRekt())
      return;
    this._actor.showTooltip((object) this);
  }

  private void buttonAction()
  {
    if (this._actor.isRekt())
      return;
    ActionLibrary.openUnitWindow(this._actor);
  }

  protected override void clear()
  {
    base.clear();
    this._container.gameObject.SetActive(false);
  }
}

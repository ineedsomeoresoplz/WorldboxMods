// Decompiled with JetBrains decompiler
// Type: ButtonGraphScalePlusMinus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class ButtonGraphScalePlusMinus : MonoBehaviour
{
  public ButtonGraphScaleType button_scale_type;
  private GraphTimeScaleContainer _main_container;
  private GraphController _graph_controller;

  private void Awake()
  {
    // ISSUE: method pointer
    ((UnityEvent) ((Component) this).GetComponent<Button>().onClick).AddListener(new UnityAction((object) this, __methodptr(setScale)));
    this._main_container = ((Component) this).GetComponentInParent<GraphTimeScaleContainer>();
    this._graph_controller = ((Component) ((Component) this).transform.parent.parent).GetComponentInChildren<GraphController>();
  }

  public void setScale()
  {
    if (this.button_scale_type == ButtonGraphScaleType.Plus)
      this._main_container.timeScaleMinus();
    else
      this._main_container.timeScalePlus();
    this._graph_controller.forceUpdateGraph();
  }
}

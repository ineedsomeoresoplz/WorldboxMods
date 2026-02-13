// Decompiled with JetBrains decompiler
// Type: WorldAgeButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class WorldAgeButton : BaseWorldAgeElement
{
  [SerializeField]
  private Image _selected;

  protected override void prepare()
  {
    base.prepare();
    DraggableLayoutElement draggableLayoutElement;
    if (!((Component) this).TryGetComponent<DraggableLayoutElement>(ref draggableLayoutElement))
      return;
    draggableLayoutElement.start_being_dragged += new Action<DraggableLayoutElement>(this.onStartDrag);
  }

  private void onStartDrag(DraggableLayoutElement pOriginalElement)
  {
    ((Behaviour) this._selected).enabled = false;
  }

  public void toggleSelectedButton(bool pState)
  {
    if (!Object.op_Inequality((Object) this._selected, (Object) null))
      return;
    ((Graphic) this._selected).color = this.asset.pie_selection_color;
    ((Behaviour) this._selected).enabled = pState;
  }
}

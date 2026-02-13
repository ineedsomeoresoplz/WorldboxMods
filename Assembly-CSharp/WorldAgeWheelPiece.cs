// Decompiled with JetBrains decompiler
// Type: WorldAgeWheelPiece
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class WorldAgeWheelPiece : BaseWorldAgeElement, IDropHandler, IEventSystemHandler
{
  public Image mask;
  [SerializeField]
  private Image _highlight;
  [SerializeField]
  private Image _icon_frame;
  private int _index;

  public void init(int pIndex)
  {
    this._index = pIndex;
    this._tip_button.setHoverAction(new TooltipAction(this._tip_button.showTooltipDefault), false);
    // ISSUE: method pointer
    ((UnityEvent) this.button.onClick).AddListener(new UnityAction((object) this, __methodptr(clickThisPiece)));
  }

  public void toggleHighlight(bool pState)
  {
    ((Behaviour) this._highlight).enabled = pState;
    ((Graphic) this._highlight).color = this.asset.pie_selection_color;
    if (!((Component) this._highlight).HasComponent<FadeInOutAnimation>())
      return;
    ((Component) this._highlight).GetComponent<FadeInOutAnimation>().resetToFadeIn();
  }

  private void clickThisPiece() => World.world.era_manager.setCurrentSlotIndex(this._index, 0.0f);

  public void toggleIconFrame(bool pState)
  {
    if (!Object.op_Inequality((Object) this._icon_frame, (Object) null))
      return;
    ((Behaviour) this._icon_frame).enabled = pState;
  }

  public void OnDrop(PointerEventData pEventData)
  {
    if (Object.op_Equality((Object) pEventData.pointerDrag, (Object) null))
      return;
    WorldAgeButton component = pEventData.pointerDrag.GetComponent<WorldAgeButton>();
    if (Object.op_Equality((Object) component, (Object) null))
      return;
    WorldAgesWindow.setAgeAndSelectPiece(component.getAsset(), this);
  }

  public bool isCurrentAge() => this._index == World.world.era_manager.getCurrentSlotIndex();

  public int getIndex() => this._index;
}

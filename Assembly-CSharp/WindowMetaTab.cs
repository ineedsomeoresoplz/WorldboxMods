// Decompiled with JetBrains decompiler
// Type: WindowMetaTab
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class WindowMetaTab : MonoBehaviour
{
  [SerializeField]
  private CanvasGroup _canvas_group;
  public List<Transform> tab_elements = new List<Transform>();
  public WindowMetaTabEvent tab_action;
  internal WindowMetaTabButtonsContainer container;
  internal bool destroyed;
  private TipButton _tip_button;
  private string _worldtip_text;
  private bool _state = true;

  private void Awake()
  {
    // ISSUE: method pointer
    ((UnityEvent) ((Component) this).GetComponent<Button>().onClick).AddListener(new UnityAction((object) this, __methodptr(\u003CAwake\u003Eb__8_0)));
    this._tip_button = ((Component) this).GetComponent<TipButton>();
    this._worldtip_text = this.getWorldTipText();
    this._tip_button.setHoverAction(new TooltipAction(this.checkShowTooltip));
  }

  public void doAction()
  {
    this.tab_action.Invoke(this);
    this.checkShowWorldTip();
  }

  public void checkShowWorldTip()
  {
    if (Object.op_Equality((Object) this._tip_button, (Object) null) || InputHelpers.mouseSupported)
      return;
    WorldTip.showNowTop(this._worldtip_text, false);
  }

  private void checkShowTooltip()
  {
    if (!InputHelpers.mouseSupported)
      return;
    Tooltip.show((object) this, "tip", new TooltipData()
    {
      tip_name = this._tip_button.textOnClick,
      tip_description = this._tip_button.textOnClickDescription,
      tip_description_2 = this._tip_button.text_description_2
    });
  }

  private void OnDestroy()
  {
    this.destroyed = true;
    if (!((Component) this).gameObject.HasComponent<PlatformRemover>())
      return;
    this.container.removeTab(this);
  }

  public bool getState() => this._state;

  public void toggleActive(bool pState)
  {
    this._state = pState;
    this._canvas_group.alpha = !this._state ? 0.0f : 1f;
    this._canvas_group.interactable = this._state;
    this._canvas_group.blocksRaycasts = this._state;
  }

  public string getWorldTipText()
  {
    string worldTipText = LocalizedTextManager.getText(this._tip_button.textOnClick);
    if (!string.IsNullOrEmpty(this._tip_button.textOnClickDescription))
      worldTipText = $"{worldTipText}\n<size=9>{LocalizedTextManager.getText(this._tip_button.textOnClickDescription)}</size>";
    return worldTipText;
  }
}

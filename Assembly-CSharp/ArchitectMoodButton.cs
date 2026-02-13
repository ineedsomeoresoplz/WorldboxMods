// Decompiled with JetBrains decompiler
// Type: ArchitectMoodButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class ArchitectMoodButton : MonoBehaviour
{
  [SerializeField]
  protected Button button;
  [SerializeField]
  protected TipButton _tip_button;
  [SerializeField]
  protected Image _icon;
  [SerializeField]
  private Image _selected;
  private ArchitectMood _asset;
  private ArchitectMoodAction _click_callback;

  private void Awake()
  {
    // ISSUE: method pointer
    ((UnityEvent) this.button.onClick).AddListener(new UnityAction((object) this, __methodptr(\u003CAwake\u003Eb__6_0)));
  }

  public ArchitectMood getAsset() => this._asset;

  public virtual void setAsset(ArchitectMood pAsset)
  {
    this._asset = pAsset;
    this._icon.sprite = this._asset.getSprite();
    this._tip_button.textOnClick = pAsset.getLocaleID();
  }

  public void toggleSelectedButton(bool pState)
  {
    if (!Object.op_Inequality((Object) this._selected, (Object) null))
      return;
    ((Graphic) this._selected).color = Toolbox.makeColor(this._asset.color_main);
    ((Behaviour) this._selected).enabled = pState;
  }

  public void setIconActiveColor(bool pState)
  {
    float num = !pState ? 0.55f : 1f;
    Color color;
    // ISSUE: explicit constructor call
    ((Color) ref color).\u002Ector(num, num, num);
    ((Graphic) this._icon).color = color;
  }

  public void addClickCallback(ArchitectMoodAction pAction) => this._click_callback += pAction;
}

// Decompiled with JetBrains decompiler
// Type: BaseWorldAgeElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class BaseWorldAgeElement : MonoBehaviour
{
  [SerializeField]
  protected Button button;
  [SerializeField]
  protected TipButton _tip_button;
  [SerializeField]
  protected Image _icon;
  protected WorldAgeAsset asset;
  protected WorldAgeElementAction click_callback;

  private void Awake() => this.prepare();

  protected virtual void prepare()
  {
    // ISSUE: method pointer
    ((UnityEvent) this.button.onClick).AddListener(new UnityAction((object) this, __methodptr(\u003Cprepare\u003Eb__6_0)));
  }

  public WorldAgeAsset getAsset() => this.asset;

  public virtual void setAge(WorldAgeAsset pAsset)
  {
    this.asset = pAsset;
    this._icon.sprite = this.asset.getSprite();
    this._tip_button.type = "world_age";
    this._tip_button.textOnClick = pAsset.id;
  }

  public void setIconActiveColor(bool pState)
  {
    float num = !pState ? 0.55f : 1f;
    Color color;
    // ISSUE: explicit constructor call
    ((Color) ref color).\u002Ector(num, num, num);
    ((Graphic) this._icon).color = color;
  }

  public void addClickCallback(WorldAgeElementAction pAction) => this.click_callback += pAction;

  public void removeClickCallback(WorldAgeElementAction pAction) => this.click_callback -= pAction;

  public WorldAgeElementAction getClickCallback() => this.click_callback;

  public void clearClickCallbacks() => this.click_callback = (WorldAgeElementAction) null;

  public Button getButton() => this.button;
}

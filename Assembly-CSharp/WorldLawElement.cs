// Decompiled with JetBrains decompiler
// Type: WorldLawElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EAF20368-35E7-4BB6-B082-5477D61019A6
// Assembly location: C:\Users\Takko\Downloads\Worldbox .NET\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class WorldLawElement : MonoBehaviour
{
  [SerializeField]
  private Button _button;
  [SerializeField]
  private Image _icon;
  [SerializeField]
  private Image _selection;
  private WorldLawCategory _category;
  private WorldLawAsset _asset;
  private bool _initialized;

  private string _law_id => this._asset?.id;

  public void init(WorldLawAsset pAsset)
  {
    this._initialized = true;
    this._asset = pAsset;
    if (!string.IsNullOrEmpty(this._asset.icon_path))
      this._icon.sprite = SpriteTextureLoader.getSprite(this._asset.icon_path);
    ((Component) this._button).GetComponent<TipButton>().setHoverAction((TooltipAction) (() =>
    {
      if (!InputHelpers.mouseSupported)
        return;
      this.showTooltip();
    }));
    ((Object) this._button).name = this._law_id;
    this._category = ((Component) this).GetComponentInParent<WorldLawCategory>();
  }

  private void OnEnable()
  {
    if (!Config.game_loaded || SmoothLoader.isLoading() || !this._initialized)
      return;
    this.updateStatus();
  }

  public void click()
  {
    if (!InputHelpers.mouseSupported)
    {
      if (!Tooltip.isShowingFor((object) this))
      {
        this.showTooltip();
        return;
      }
      Tooltip.hideTooltipNow();
    }
    if (this._asset.requires_premium && !Config.hasPremium)
    {
      ScrollWindow.showWindow("premium_menu");
    }
    else
    {
      PlayerOptionData pOption = World.world.world_laws.dict[this._law_id];
      bool boolVal = pOption.boolVal;
      if (this._asset.can_turn_off)
        pOption.boolVal = !pOption.boolVal;
      else if (!pOption.boolVal)
        pOption.boolVal = true;
      if (pOption.boolVal && !boolVal)
      {
        PlayerOptionAction onStateEnabled = this._asset.on_state_enabled;
        if (onStateEnabled != null)
          onStateEnabled(pOption);
      }
      World.world.world_laws.updateCaches();
      PlayerOptionAction onSwitch = pOption.on_switch;
      if (onSwitch != null)
        onSwitch(pOption);
      this.updateStatus();
    }
  }

  private void showTooltip()
  {
    Tooltip.show((object) this, "world_law", new TooltipData()
    {
      world_law = this._asset
    });
  }

  public void updateStatus()
  {
    bool flag = this.isLawEnabled();
    ((Behaviour) this._selection).enabled = flag;
    if (flag)
      ((Graphic) this._icon).color = Color.white;
    else
      ((Graphic) this._icon).color = Color.grey;
    this._category?.updateCounter();
  }

  public void addListener(UnityAction pAction)
  {
    ((UnityEvent) this._button.onClick).AddListener(pAction);
  }

  public void setSelectionColor(Color pColor) => ((Graphic) this._selection).color = pColor;

  public bool isLawEnabled() => World.world.world_laws.dict[this._law_id].boolVal;
}
